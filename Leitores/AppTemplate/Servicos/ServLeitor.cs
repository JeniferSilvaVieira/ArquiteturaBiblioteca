using LeitoresApp.DTO;

namespace LeitoresApp.Servicos
{
    public class ServLeitor
    {
        private readonly DataContext _dataContext;

        public ServLeitor(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void InserirLeitor(Leitor novoLeitor)
        {
            ValidarDadosDoLeitor(novoLeitor.Nome, novoLeitor.Email);

            if (_dataContext.Leitores.Any(p => p.Email == novoLeitor.Email))
            {
                throw new Exception("Já existe um leitor cadastrado com este e-mail.");
            }

            novoLeitor.Id = Guid.NewGuid();
            novoLeitor.DataCadastro = DateTime.Today;
            novoLeitor.Ativo = true;

            _dataContext.Add(novoLeitor);
            _dataContext.SaveChanges();
        }

        public void ValidarDadosDoLeitor(string nome, string email)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                throw new Exception("Falta informar o nome do leitor.");
            }

            if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            {
                throw new Exception("Informe um e-mail válido para o leitor.");
            }
        }

        public List<Leitor> ListarLeitores()
        {
            return _dataContext.Leitores.ToList();
        }

        public Leitor BuscarLeitorPorId(Guid id)
        {
            var leitor = _dataContext.Leitores.FirstOrDefault(p => p.Id == id);

            if (leitor == null)
            {
                throw new Exception("Leitor não encontrado.");
            }

            return leitor;
        }

        public Leitor EditarLeitor(Guid id, EditarLeitorDTO editarLeitorDto)
        {
            var leitor = BuscarLeitorPorId(id);

            ValidarDadosDoLeitor(editarLeitorDto.Nome, editarLeitorDto.Email);

            if (_dataContext.Leitores.Any(p => p.Email == editarLeitorDto.Email && p.Id != id))
            {
                throw new Exception("Já existe outro leitor cadastrado com este e-mail.");
            }

            leitor.Nome = editarLeitorDto.Nome;
            leitor.Email = editarLeitorDto.Email;
            leitor.Telefone = editarLeitorDto.Telefone;

            _dataContext.SaveChanges();

            return leitor;
        }

        public Leitor AlterarSituacaoLeitor(Guid id, AlterarSituacaoLeitorDTO alterarSituacaoDto)
        {
            var leitor = BuscarLeitorPorId(id);

            leitor.Ativo = alterarSituacaoDto.Ativo;

            _dataContext.SaveChanges();

            return leitor;
        }
    }
}
