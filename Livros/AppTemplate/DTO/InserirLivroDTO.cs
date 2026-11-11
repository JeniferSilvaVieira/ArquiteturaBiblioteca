namespace LivrosApp.DTO
{
    public class InserirLivroDTO
    {
        public string Titulo { get; set; } = string.Empty;

        public string Autor { get; set; } = string.Empty;

        public string Isbn { get; set; } = string.Empty;

        public int QuantidadeTotal { get; set; }
    }
}
