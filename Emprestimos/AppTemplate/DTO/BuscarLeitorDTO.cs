namespace EmprestimosApp.DTO
{
    public class BuscarLeitorDTO
    {
        public Guid Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public bool Ativo { get; set; }
    }
}
