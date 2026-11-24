using EmprestimosApp.DTO;

namespace EmprestimosApp.Servicos
{
    public class HistoricoClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HistoricoClient(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task Registrar(RegistrarHistoricoDTO registrarHistoricoDto)
        {
            var urlHistoricoApp = _configuration["HistoricoUrl"];

            if (string.IsNullOrEmpty(urlHistoricoApp))
            {
                throw new Exception("A URL do micro de histórico não foi configurada.");
            }

            var metodoRegistrar = urlHistoricoApp.TrimEnd('/') + "/api/Historico/registrar";

            try
            {
                var response = await _httpClient.PostAsJsonAsync(metodoRegistrar, registrarHistoricoDto);

                if (!response.IsSuccessStatusCode)
                {
                    var mensagem = await response.Content.ReadAsStringAsync();

                    throw new Exception("Erro ao registrar histórico: " + mensagem);
                }
            }
            catch (HttpRequestException)
            {
                throw new Exception("O micro de histórico está indisponível.");
            }
        }
    }
}
