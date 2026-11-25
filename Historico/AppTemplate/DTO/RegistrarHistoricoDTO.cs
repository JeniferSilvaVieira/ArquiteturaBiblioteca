using HistoricoApp.Servicos;

namespace HistoricoApp.DTO
{
    public class RegistrarHistoricoDTO
    {
        public EnumTipoEvento TipoEvento { get; set; }

        public Guid CodigoEmprestimo { get; set; }

        public Guid CodigoLivro { get; set; }

        public Guid CodigoLeitor { get; set; }

        public string TituloLivro { get; set; } = string.Empty;

        public string NomeLeitor { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;
    }
}
