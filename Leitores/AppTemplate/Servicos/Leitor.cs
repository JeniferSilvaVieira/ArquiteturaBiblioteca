namespace LeitoresApp.Servicos
{
    public class Leitor
    {
        public Guid Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Telefone { get; set; } = string.Empty;

        public bool Ativo { get; set; }

        public DateTime DataCadastro { get; set; }
    }
}
