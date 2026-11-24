using EmprestimosApp.DTO;
using EmprestimosApp.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace EmprestimosApp.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class EmprestimoController : ControllerBase
    {
        private readonly ServEmprestimo _servEmprestimo;

        public EmprestimoController(ServEmprestimo servEmprestimo)
        {
            _servEmprestimo = servEmprestimo;
        }

        [HttpPost("realizar-emprestimo")]
        public async Task<IActionResult> RealizarEmprestimo(RealizarEmprestimoDTO realizarEmprestimoDto)
        {
            try
            {
                var novoEmprestimo = new Emprestimo();

                novoEmprestimo.CodigoLivro = realizarEmprestimoDto.CodigoLivro;
                novoEmprestimo.CodigoLeitor = realizarEmprestimoDto.CodigoLeitor;

                await _servEmprestimo.RealizarEmprestimo(novoEmprestimo);

                return Ok(novoEmprestimo);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPatch("devolver-livro/{id}")]
        public async Task<IActionResult> DevolverLivro(Guid id)
        {
            try
            {
                var emprestimo = await _servEmprestimo.DevolverLivro(id);

                return Ok(emprestimo);
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
                var emprestimos = _servEmprestimo.ListarEmprestimos();

                return Ok(emprestimos);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("atrasados")]
        public IActionResult ListarAtrasados()
        {
            try
            {
                var emprestimos = _servEmprestimo.ListarEmprestimosAtrasados();

                return Ok(emprestimos);
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
                var emprestimos = _servEmprestimo.ListarEmprestimosPorLeitor(codigoLeitor);

                return Ok(emprestimos);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult BuscarPorId(Guid id)
        {
            try
            {
                var emprestimo = _servEmprestimo.BuscarEmprestimoPorId(id);

                return Ok(emprestimo);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
