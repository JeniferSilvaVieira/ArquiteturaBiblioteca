namespace LivrosApp.DTO
{
    public class AlterarEstoqueDTO
    {
        public EnumTipoEstoque Tipo { get; set; }
    }

    public enum EnumTipoEstoque
    {
        Retirada = 1,
        Devolucao = 2
    }
}
