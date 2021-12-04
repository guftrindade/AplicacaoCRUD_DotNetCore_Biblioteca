using ExercicioBiblioteca.Context;
using ExercicioBiblioteca.InputModel;
using ExercicioBiblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExercicioBiblioteca.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LivroController : ControllerBase
    {
        private readonly BibliotecaDbContext _bibliotecaDbContext;

        public LivroController(BibliotecaDbContext bibliotecaDbContext)
        {
            _bibliotecaDbContext = bibliotecaDbContext;
        }

        #region Métodos_HTTP_GET

        [HttpGet("filtrar-por-ano")]
        public async Task<IActionResult> FiltrarPorAnoLancamento(int anoLancamento)
        {
            var livros = await _bibliotecaDbContext.Livros.Where(x => x.AnoLancamento.Equals(anoLancamento)).ToListAsync();
            if (livros.Any())
                return Ok(livros);

            return NotFound("Nenhum livro encontrado!");
        }


        [HttpGet("filtrar-por-descricao")]
        public async Task<IActionResult> FiltrarPorDescricao(string descricao)
        {
            var livros = await _bibliotecaDbContext.Livros.Where(x => x.Descricao.Contains(descricao)).ToListAsync();
            if (livros.Any())
                return Ok(livros);

            return NotFound("Nenhum registro foi encontrado!");
        }


        [HttpGet("filtrar-por-isbn")]
        public async Task<IActionResult>FiltrarPorISBN(int isbn)
        {
            var livro = await _bibliotecaDbContext.Livros.Where(x => x.ISBN == isbn).FirstOrDefaultAsync();
            if (livro == null)
            {
                return NotFound("Livro não existe no banco de dados");
            }

            return Ok(livro);
        }
        
        [HttpGet("listar-todos")]
        public async Task<IActionResult> Listar()
        {
            return Ok(await _bibliotecaDbContext.Livros.ToListAsync());
        }

        #endregion

        #region Métodos_HTTP_PUT
        [HttpPut("atualizar-livro")]
        public async Task<IActionResult>Atualizar(AtualizarLivroInput dadosEntrada)
        {
            var livro = await _bibliotecaDbContext.Livros.Where(x => x.Codigo == dadosEntrada.Codigo).FirstOrDefaultAsync();
            if (livro == null)
                return NotFound("Livro não existe!");

            livro.Descricao = dadosEntrada.Descricao;
            livro.ISBN = dadosEntrada.ISBN;
            livro.AnoLancamento = dadosEntrada.AnoLancamento;

            _bibliotecaDbContext.Livros.Update(livro);
            await _bibliotecaDbContext.SaveChangesAsync();

            return Ok(livro);
        }
        #endregion

        #region Métodos_HTTP_POST
        [HttpPost]
        public async Task<IActionResult> CadastrarLivro(LivroInput dadosEntrada)
        {
            var livro = new Livro()
            {
                Descricao = dadosEntrada.Descricao,
                ISBN = dadosEntrada.ISBN,
                AnoLancamento = dadosEntrada.AnoLancamento,
                CodigoEditora = dadosEntrada.CodigoEditora,
                CodigoAutor = dadosEntrada.CodigoAutor
            };

            await _bibliotecaDbContext.Livros.AddAsync(livro);
            await _bibliotecaDbContext.SaveChangesAsync();

            return Ok(
                        new
                        {
                            success = true,
                            data = new
                            {
                                descricao = livro.Descricao,
                                isbn = livro.ISBN
                            }
                        });       
        }
        #endregion

        #region Métodos_HTTP_DELETE
        [HttpDelete]
        public async Task<IActionResult> Deletar(int codigo)
        {
            var livro = await _bibliotecaDbContext.Livros.Where(x => x.Codigo == codigo).FirstOrDefaultAsync();
            if (livro == null)
                return NotFound("Livro não encontrado!");

            _bibliotecaDbContext.Livros.Remove(livro);
            await _bibliotecaDbContext.SaveChangesAsync();

            return Ok("Livro deletado com Sucesso!");
        }
        #endregion
    }
}
