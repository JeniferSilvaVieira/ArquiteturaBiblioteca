using HistoricoApp.DTO;
using HistoricoApp.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace HistoricoApp.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class HistoricoController : ControllerBase
    {
        private readonly ServHistorico _servHistorico;

        public HistoricoController(ServHistorico servHistorico)
        {
            _servHistorico = servHistorico;
        }

        [HttpPost("registrar")]
        public IActionResult Registrar(RegistrarHistoricoDTO registrarDto)
        {
            try
            {
                var novoRegistro = new RegistroHistorico();

                novoRegistro.TipoEvento = registrarDto.TipoEvento;
                novoRegistro.CodigoEmprestimo = registrarDto.CodigoEmprestimo;
                novoRegistro.CodigoLivro = registrarDto.CodigoLivro;
                novoRegistro.CodigoLeitor = registrarDto.CodigoLeitor;
                novoRegistro.TituloLivro = registrarDto.TituloLivro;
                novoRegistro.NomeLeitor = registrarDto.NomeLeitor;
                novoRegistro.Descricao = registrarDto.Descricao;

                _servHistorico.RegistrarEvento(novoRegistro);

                return Ok(novoRegistro);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public IActionResult Listar()
        {
            try
            {
                var historico = _servHistorico.ListarHistorico();

                return Ok(historico);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("por-leitor/{codigoLeitor}")]
        public IActionResult ListarPorLeitor(Guid codigoLeitor)
        {
            try
            {
                var historico = _servHistorico.ListarPorLeitor(codigoLeitor);

                return Ok(historico);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("por-livro/{codigoLivro}")]
        public IActionResult ListarPorLivro(Guid codigoLivro)
        {
            try
            {
                var historico = _servHistorico.ListarPorLivro(codigoLivro);

                return Ok(historico);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("por-emprestimo/{codigoEmprestimo}")]
        public IActionResult ListarPorEmprestimo(Guid codigoEmprestimo)
        {
            try
            {
                var historico = _servHistorico.ListarPorEmprestimo(codigoEmprestimo);

                return Ok(historico);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
