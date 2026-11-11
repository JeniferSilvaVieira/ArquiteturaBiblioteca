using LivrosApp.DTO;
using LivrosApp.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace LivrosApp.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class LivroController : ControllerBase
    {
        private readonly ServLivro _servLivro;

        public LivroController(ServLivro servLivro)
        {
            _servLivro = servLivro;
        }

        [HttpPost]
        public IActionResult Inserir(InserirLivroDTO inserirLivroDto)
        {
            try
            {
                var novoLivro = new Livro();

                novoLivro.Titulo = inserirLivroDto.Titulo;
                novoLivro.Autor = inserirLivroDto.Autor;
                novoLivro.Isbn = inserirLivroDto.Isbn;
                novoLivro.QuantidadeTotal = inserirLivroDto.QuantidadeTotal;

                _servLivro.InserirLivro(novoLivro);

                return Ok(novoLivro);
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
                var livros = _servLivro.ListarLivros();

                return Ok(livros);
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
                var livro = _servLivro.BuscarLivroPorId(id);

                return Ok(livro);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Editar(Guid id, EditarLivroDTO editarLivroDto)
        {
            try
            {
                var livro = _servLivro.EditarLivro(id, editarLivroDto);

                return Ok(livro);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPatch("/api/alterar-estoque-livro/{id}")]
        public IActionResult AlterarEstoque(Guid id, AlterarEstoqueDTO alterarEstoqueDto)
        {
            try
            {
                _servLivro.AlterarEstoque(id, alterarEstoqueDto);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
