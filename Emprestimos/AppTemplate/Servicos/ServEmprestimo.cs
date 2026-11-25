using EmprestimosApp.DTO;

namespace EmprestimosApp.Servicos
{
    public class ServEmprestimo
    {
        private const int PrazoEmDias = 14;
        private const int LimiteEmprestimosPorLeitor = 3;

        private readonly DataContext _dataContext;
        private readonly LivroClient _livroClient;
        private readonly LeitorClient _leitorClient;
        private readonly HistoricoClient _historicoClient;
        private readonly ILogger<ServEmprestimo> _logger;

        public ServEmprestimo(
            DataContext dataContext,
            LivroClient livroClient,
            LeitorClient leitorClient,
            HistoricoClient historicoClient,
            ILogger<ServEmprestimo> logger)
        {
            _dataContext = dataContext;
            _livroClient = livroClient;
            _leitorClient = leitorClient;
            _historicoClient = historicoClient;
            _logger = logger;
        }

        public async Task RealizarEmprestimo(Emprestimo emprestimo)
        {
            if (emprestimo.CodigoLivro == Guid.Empty)
            {
                throw new Exception("Falta informar o livro.");
            }

            if (emprestimo.CodigoLeitor == Guid.Empty)
            {
                throw new Exception("Falta informar o leitor.");
            }

            var leitor = await _leitorClient.BuscarLeitor(emprestimo.CodigoLeitor);

            if (!leitor.Ativo)
            {
                throw new Exception("O leitor está inativo e não pode fazer empréstimos.");
            }

            var emprestimosEmAndamento = _dataContext.Emprestimos
                .Where(p => p.CodigoLeitor == emprestimo.CodigoLeitor
                         && p.Status == EnumStatusEmprestimo.EmAndamento)
                .ToList();

            if (emprestimosEmAndamento.Count >= LimiteEmprestimosPorLeitor)
            {
                throw new Exception($"O leitor já atingiu o limite de {LimiteEmprestimosPorLeitor} empréstimos em andamento.");
            }

            if (emprestimosEmAndamento.Any(p => p.CodigoLivro == emprestimo.CodigoLivro))
            {
                throw new Exception("O leitor já está com um exemplar deste livro.");
            }

            var livro = await _livroClient.BuscarLivro(emprestimo.CodigoLivro);

            if (livro.QuantidadeDisponivel <= 0)
            {
                throw new Exception("Não há exemplares disponíveis deste livro.");
            }

            emprestimo.Id = Guid.NewGuid();
            emprestimo.DataEmprestimo = DateTime.Today;
            emprestimo.DataPrevistaDevolucao = DateTime.Today.AddDays(PrazoEmDias);
            emprestimo.DataDevolucao = null;
            emprestimo.Status = EnumStatusEmprestimo.EmAndamento;

            var retirada = new AlterarEstoqueDTO();
            retirada.Tipo = EnumTipoEstoque.Retirada;

            await _livroClient.AlterarEstoque(emprestimo.CodigoLivro, retirada);

            try
            {
                _dataContext.Add(emprestimo);
                _dataContext.SaveChanges();
            }
            catch
            {
                // Se não conseguiu gravar o empréstimo, devolve o exemplar ao estoque
                var compensacao = new AlterarEstoqueDTO();
                compensacao.Tipo = EnumTipoEstoque.Devolucao;

                await _livroClient.AlterarEstoque(emprestimo.CodigoLivro, compensacao);

                throw;
            }

            var descricao = $"Empréstimo do livro '{livro.Titulo}' para {leitor.Nome}. " +
                            $"Devolução prevista para {emprestimo.DataPrevistaDevolucao:dd/MM/yyyy}.";

            await RegistrarHistorico(emprestimo, livro.Titulo, leitor.Nome, EnumTipoEvento.Emprestimo, descricao);
        }

        public async Task<Emprestimo> DevolverLivro(Guid id)
        {
            var emprestimo = BuscarEmprestimoPorId(id);

            if (emprestimo.Status == EnumStatusEmprestimo.Devolvido)
            {
                throw new Exception("Este empréstimo já foi devolvido.");
            }

            var livro = await _livroClient.BuscarLivro(emprestimo.CodigoLivro);
            var leitor = await _leitorClient.BuscarLeitor(emprestimo.CodigoLeitor);

            var devolucao = new AlterarEstoqueDTO();
            devolucao.Tipo = EnumTipoEstoque.Devolucao;

            await _livroClient.AlterarEstoque(emprestimo.CodigoLivro, devolucao);

            emprestimo.DataDevolucao = DateTime.Today;
            emprestimo.Status = EnumStatusEmprestimo.Devolvido;

            try
            {
                _dataContext.SaveChanges();
            }
            catch
            {
                // Se não conseguiu gravar a devolução, tira o exemplar do estoque de novo
                var compensacao = new AlterarEstoqueDTO();
                compensacao.Tipo = EnumTipoEstoque.Retirada;

                await _livroClient.AlterarEstoque(emprestimo.CodigoLivro, compensacao);

                throw;
            }

            var diasDeAtraso = (DateTime.Today - emprestimo.DataPrevistaDevolucao.Date).Days;

            var descricao = $"Devolução do livro '{livro.Titulo}' por {leitor.Nome}.";

            if (diasDeAtraso > 0)
            {
                descricao += $" Devolvido com {diasDeAtraso} dia(s) de atraso.";
            }
            else
            {
                descricao += " Devolvido dentro do prazo.";
            }

            await RegistrarHistorico(emprestimo, livro.Titulo, leitor.Nome, EnumTipoEvento.Devolucao, descricao);

            return emprestimo;
        }

        // O histórico é "melhor esforço": se o micro de histórico estiver fora do ar,
        // o empréstimo/devolução já feito não é desfeito; apenas registramos um aviso no log.
        private async Task RegistrarHistorico(
            Emprestimo emprestimo,
            string tituloLivro,
            string nomeLeitor,
            EnumTipoEvento tipoEvento,
            string descricao)
        {
            try
            {
                var registrarDto = new RegistrarHistoricoDTO();

                registrarDto.TipoEvento = tipoEvento;
                registrarDto.CodigoEmprestimo = emprestimo.Id;
                registrarDto.CodigoLivro = emprestimo.CodigoLivro;
                registrarDto.CodigoLeitor = emprestimo.CodigoLeitor;
                registrarDto.TituloLivro = tituloLivro;
                registrarDto.NomeLeitor = nomeLeitor;
                registrarDto.Descricao = descricao;

                await _historicoClient.Registrar(registrarDto);
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Não foi possível registrar o histórico do empréstimo {Id}.", emprestimo.Id);
            }
        }

        public List<Emprestimo> ListarEmprestimos()
        {
            return _dataContext.Emprestimos
                .OrderByDescending(p => p.DataEmprestimo)
                .ToList();
        }

        public List<Emprestimo> ListarEmprestimosPorLeitor(Guid codigoLeitor)
        {
            return _dataContext.Emprestimos
                .Where(p => p.CodigoLeitor == codigoLeitor)
                .OrderByDescending(p => p.DataEmprestimo)
                .ToList();
        }

        public List<Emprestimo> ListarEmprestimosAtrasados()
        {
            var hoje = DateTime.Today;

            return _dataContext.Emprestimos
                .Where(p => p.Status == EnumStatusEmprestimo.EmAndamento
                         && p.DataPrevistaDevolucao < hoje)
                .OrderBy(p => p.DataPrevistaDevolucao)
                .ToList();
        }

        public Emprestimo BuscarEmprestimoPorId(Guid id)
        {
            var emprestimo = _dataContext.Emprestimos.FirstOrDefault(p => p.Id == id);

            if (emprestimo == null)
            {
                throw new Exception("Empréstimo não encontrado.");
            }

            return emprestimo;
        }
    }
}
