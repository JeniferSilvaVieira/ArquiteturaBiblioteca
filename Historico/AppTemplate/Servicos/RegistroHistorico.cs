namespace HistoricoApp.Servicos
{
    public class RegistroHistorico
    {
        public Guid Id { get; set; }

        public DateTime DataEvento { get; set; }

        public EnumTipoEvento TipoEvento { get; set; }

        public Guid CodigoEmprestimo { get; set; }

        public Guid CodigoLivro { get; set; }

        public Guid CodigoLeitor { get; set; }

        // Guardamos uma "foto" do título e do nome no momento do evento,
        // assim o histórico continua legível mesmo que o livro/leitor mude depois.
        public string TituloLivro { get; set; } = string.Empty;

        public string NomeLeitor { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;
    }

    public enum EnumTipoEvento
    {
        Emprestimo = 1,
        Devolucao = 2
    }
}
