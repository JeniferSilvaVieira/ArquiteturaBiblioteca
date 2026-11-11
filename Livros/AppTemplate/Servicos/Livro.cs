namespace LivrosApp.Servicos
{
    public class Livro
    {
        public Guid Id { get; set; }

        public string Titulo { get; set; } = string.Empty;

        public string Autor { get; set; } = string.Empty;

        public string Isbn { get; set; } = string.Empty;

        public int QuantidadeTotal { get; set; }

        public int QuantidadeDisponivel { get; set; }

        public DateTime DataCadastro { get; set; }
    }
}
