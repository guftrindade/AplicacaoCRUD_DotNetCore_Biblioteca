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
    public class AutoresController : ControllerBase
    {
        private readonly BibliotecaDbContext _bibliotecaDbContext;

        public AutoresController(BibliotecaDbContext bibliotecaDbContext)
        {
            _bibliotecaDbContext = bibliotecaDbContext;
        }


        #region Métodos_HTTP_GET

        [HttpGet("filtrar-por-nome")]
        public async Task<IActionResult>FiltrarPorNome(string nome)
        {
            var autores = await _bibliotecaDbContext.Autores.Where(x => x.Nome.Contains(nome)).ToListAsync();
            if (autores.Any())
                return Ok(autores);

            return NotFound("Nenhum Autor encontrado com o nome mencionado!");
        }

        [HttpGet("listar-todos")]
        public async Task<IActionResult> Listar()
        {
            return Ok(await _bibliotecaDbContext.Autores.ToListAsync());
        }
        #endregion

        #region Métodos_HTTP_PUT
        
        [HttpPut]
        public async Task<IActionResult>Atualizar(AtualizarAutorInput dadosEntrada)
        {
            var autor = await _bibliotecaDbContext.Autores.Where(x => x.Codigo == dadosEntrada.Codigo).FirstOrDefaultAsync();
            if (autor == null)
                return NotFound("Autor não encontrado!");

            autor.Nome = dadosEntrada.Nome;
            _bibliotecaDbContext.Autores.Update(autor);
            await _bibliotecaDbContext.SaveChangesAsync();

            return Ok(autor);

        }
        #endregion

        #region Métodos_HTTP_POST
        [HttpPost]
        public async Task<IActionResult> CadastrarAutor(AutorInput dadosEntrada)
        {
            var autor = new Autor()
            {
                Nome = dadosEntrada.Nome
            };

            await _bibliotecaDbContext.Autores.AddAsync(autor);
            await _bibliotecaDbContext.SaveChangesAsync();

            return Ok();
        }
        #endregion

        #region Métodos_HTTP_DELETE
        [HttpDelete]
        public async Task<IActionResult> Deletar(int codigo)
        {
            var autor = await _bibliotecaDbContext.Autores.Where(x => x.Codigo == codigo).FirstOrDefaultAsync();
            if (autor == null)
                return NotFound("Autor não encontrado!");

            _bibliotecaDbContext.Autores.Remove(autor);
            await _bibliotecaDbContext.SaveChangesAsync();

            return Ok("Autor deletado com sucesso!");
        }
        #endregion
    }
}
