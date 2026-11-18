using LeitoresApp.DTO;
using LeitoresApp.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace LeitoresApp.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class LeitorController : ControllerBase
    {
        private readonly ServLeitor _servLeitor;

        public LeitorController(ServLeitor servLeitor)
        {
            _servLeitor = servLeitor;
        }

        [HttpPost]
        public IActionResult Inserir(InserirLeitorDTO inserirLeitorDto)
        {
            try
            {
                var novoLeitor = new Leitor();

                novoLeitor.Nome = inserirLeitorDto.Nome;
                novoLeitor.Email = inserirLeitorDto.Email;
                novoLeitor.Telefone = inserirLeitorDto.Telefone;

                _servLeitor.InserirLeitor(novoLeitor);

                return Ok(novoLeitor);
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
                var leitores = _servLeitor.ListarLeitores();

                return Ok(leitores);
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
                var leitor = _servLeitor.BuscarLeitorPorId(id);

                return Ok(leitor);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Editar(Guid id, EditarLeitorDTO editarLeitorDto)
        {
            try
            {
                var leitor = _servLeitor.EditarLeitor(id, editarLeitorDto);

                return Ok(leitor);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPatch("/api/alterar-situacao-leitor/{id}")]
        public IActionResult AlterarSituacao(Guid id, AlterarSituacaoLeitorDTO alterarSituacaoDto)
        {
            try
            {
                var leitor = _servLeitor.AlterarSituacaoLeitor(id, alterarSituacaoDto);

                return Ok(leitor);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
