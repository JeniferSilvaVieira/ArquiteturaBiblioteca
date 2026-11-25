using EmprestimosApp.DTO;

namespace EmprestimosApp.Servicos
{
    public class LeitorClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public LeitorClient(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<BuscarLeitorDTO> BuscarLeitor(Guid codigoLeitor)
        {
            var urlLeitoresApp = _configuration["LeitoresUrl"];

            if (string.IsNullOrEmpty(urlLeitoresApp))
            {
                throw new Exception("A URL do micro de leitores não foi configurada.");
            }

            var metodoBuscarLeitor = urlLeitoresApp.TrimEnd('/') + "/api/Leitor/" + codigoLeitor;

            try
            {
                var response = await _httpClient.GetAsync(metodoBuscarLeitor);

                if (!response.IsSuccessStatusCode)
                {
                    var mensagem = await response.Content.ReadAsStringAsync();

                    throw new Exception("Erro ao buscar leitor: " + mensagem);
                }

                var leitorDto = await response.Content.ReadFromJsonAsync<BuscarLeitorDTO>();

                if (leitorDto == null)
                {
                    throw new Exception("Leitor não encontrado.");
                }

                return leitorDto;
            }
            catch (HttpRequestException)
            {
                throw new Exception("O micro de leitores está indisponível.");
            }
        }
    }
}
