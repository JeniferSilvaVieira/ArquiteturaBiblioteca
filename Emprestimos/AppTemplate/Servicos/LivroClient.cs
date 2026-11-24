using EmprestimosApp.DTO;

namespace EmprestimosApp.Servicos
{
    public class LivroClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public LivroClient(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        private string ObterUrl()
        {
            var urlLivrosApp = _configuration["LivrosUrl"];

            if (string.IsNullOrEmpty(urlLivrosApp))
            {
                throw new Exception("A URL do micro de livros não foi configurada.");
            }

            return urlLivrosApp.TrimEnd('/');
        }

        public async Task<BuscarLivroDTO> BuscarLivro(Guid codigoLivro)
        {
            var metodoBuscarLivro = ObterUrl() + "/api/Livro/" + codigoLivro;

            try
            {
                var response = await _httpClient.GetAsync(metodoBuscarLivro);

                if (!response.IsSuccessStatusCode)
                {
                    var mensagem = await response.Content.ReadAsStringAsync();

                    throw new Exception("Erro ao buscar livro: " + mensagem);
                }

                var livroDto = await response.Content.ReadFromJsonAsync<BuscarLivroDTO>();

                if (livroDto == null)
                {
                    throw new Exception("Livro não encontrado.");
                }

                return livroDto;
            }
            catch (HttpRequestException)
            {
                throw new Exception("O micro de livros está indisponível.");
            }
        }

        public async Task AlterarEstoque(Guid codigoLivro, AlterarEstoqueDTO alterarEstoqueDto)
        {
            var metodoAlterarEstoque = ObterUrl() + "/api/alterar-estoque-livro/" + codigoLivro;

            try
            {
                var response = await _httpClient.PatchAsJsonAsync(metodoAlterarEstoque, alterarEstoqueDto);

                if (!response.IsSuccessStatusCode)
                {
                    var mensagem = await response.Content.ReadAsStringAsync();

                    throw new Exception("Erro ao alterar estoque do livro: " + mensagem);
                }
            }
            catch (HttpRequestException)
            {
                throw new Exception("O micro de livros está indisponível.");
            }
        }
    }
}
