using LivrosApp.DTO;

namespace LivrosApp.Servicos
{
    public class ServLivro
    {
        private readonly DataContext _dataContext;

        public ServLivro(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void InserirLivro(Livro novoLivro)
        {
            ValidarDadosDoLivro(novoLivro.Titulo, novoLivro.Autor, novoLivro.Isbn, novoLivro.QuantidadeTotal);

            if (_dataContext.Livros.Any(p => p.Isbn == novoLivro.Isbn))
            {
                throw new Exception("Já existe um livro cadastrado com este ISBN.");
            }

            novoLivro.Id = Guid.NewGuid();
            novoLivro.DataCadastro = DateTime.Today;
            novoLivro.QuantidadeDisponivel = novoLivro.QuantidadeTotal;

            _dataContext.Add(novoLivro);
            _dataContext.SaveChanges();
        }

        public void ValidarDadosDoLivro(string titulo, string autor, string isbn, int quantidadeTotal)
        {
            if (string.IsNullOrWhiteSpace(titulo))
            {
                throw new Exception("Falta informar o título do livro.");
            }

            if (string.IsNullOrWhiteSpace(autor))
            {
                throw new Exception("Falta informar o autor do livro.");
            }

            if (string.IsNullOrWhiteSpace(isbn))
            {
                throw new Exception("Falta informar o ISBN do livro.");
            }

            if (quantidadeTotal <= 0)
            {
                throw new Exception("A quantidade de exemplares deve ser maior que zero.");
            }
        }

        public List<Livro> ListarLivros()
        {
            return _dataContext.Livros.ToList();
        }

        public Livro BuscarLivroPorId(Guid id)
        {
            var livro = _dataContext.Livros.FirstOrDefault(p => p.Id == id);

            if (livro == null)
            {
                throw new Exception("Livro não encontrado.");
            }

            return livro;
        }

        public Livro EditarLivro(Guid id, EditarLivroDTO editarLivroDto)
        {
            var livro = BuscarLivroPorId(id);

            ValidarDadosDoLivro(editarLivroDto.Titulo, editarLivroDto.Autor, editarLivroDto.Isbn, editarLivroDto.QuantidadeTotal);

            if (_dataContext.Livros.Any(p => p.Isbn == editarLivroDto.Isbn && p.Id != id))
            {
                throw new Exception("Já existe outro livro cadastrado com este ISBN.");
            }

            // Se a quantidade total mudou, o disponível acompanha a diferença
            var diferenca = editarLivroDto.QuantidadeTotal - livro.QuantidadeTotal;

            if (livro.QuantidadeDisponivel + diferenca < 0)
            {
                throw new Exception("A nova quantidade é menor que o número de exemplares emprestados.");
            }

            livro.Titulo = editarLivroDto.Titulo;
            livro.Autor = editarLivroDto.Autor;
            livro.Isbn = editarLivroDto.Isbn;
            livro.QuantidadeTotal = editarLivroDto.QuantidadeTotal;
            livro.QuantidadeDisponivel += diferenca;

            _dataContext.SaveChanges();

            return livro;
        }

        // Chamado pelo micro de Empréstimos (retirada ao emprestar, devolução ao devolver)
        public void AlterarEstoque(Guid id, AlterarEstoqueDTO alterarEstoqueDto)
        {
            var livro = BuscarLivroPorId(id);

            if (alterarEstoqueDto.Tipo == EnumTipoEstoque.Retirada)
            {
                if (livro.QuantidadeDisponivel <= 0)
                {
                    throw new Exception("Não há exemplares disponíveis deste livro.");
                }

                livro.QuantidadeDisponivel--;
            }
            else if (alterarEstoqueDto.Tipo == EnumTipoEstoque.Devolucao)
            {
                if (livro.QuantidadeDisponivel >= livro.QuantidadeTotal)
                {
                    throw new Exception("Todos os exemplares deste livro já estão na biblioteca.");
                }

                livro.QuantidadeDisponivel++;
            }
            else
            {
                throw new Exception("Tipo de movimentação de estoque inválido.");
            }

            _dataContext.SaveChanges();
        }
    }
}
