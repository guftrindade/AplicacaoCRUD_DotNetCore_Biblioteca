using ExercicioBiblioteca.Context;
using ExercicioBiblioteca.InputModel;
using ExercicioBiblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ExercicioBiblioteca.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmprestimoController : ControllerBase
    {
        private readonly BibliotecaDbContext _bibliotecaDbContext;

        //Construtor
        public EmprestimoController(BibliotecaDbContext bibliotecaDbContext)
        {
            _bibliotecaDbContext = bibliotecaDbContext;
        }

        [HttpGet("listar-emprestimo")]
        public async Task<IActionResult> Listar()
        {
            return Ok(await _bibliotecaDbContext.Emprestimos
                            .Include(x => x.Leitor)
                            .ToListAsync());
        }


        [HttpPost("cadastrar-emprestimo")]
        public async Task<IActionResult> CadastrarEmprestimo(EmprestimoInput dadosEntrada)
        {
            var emprestimo = new Emprestimo
            {
                CodigoLeitor = dadosEntrada.CodigoLeitor,
                DataEmprestimo = DateTime.Now,
                DataDevolucao = DateTime.Now.AddDays(3)
            };

            await _bibliotecaDbContext.Emprestimos.AddAsync(emprestimo);
            await _bibliotecaDbContext.SaveChangesAsync();

            return Ok(
                        new
                        {
                            numeroEmprestimoGerado = emprestimo.Numero
                        });
        }

        [HttpGet("listar-item-emprestimos")]
        public async Task<IActionResult> ListarItensEmprestimos()
        {
            return Ok(await _bibliotecaDbContext.ItensEmprestimos.ToListAsync());
        }

        [HttpPost("cadastrar-item-emprestimo")]
        public async Task<IActionResult> CadastrarItensEmprestimo(ItemEmprestimoInput dadosEntrada)
        {
            var livro = await _bibliotecaDbContext.Livros.Where(x => x.Codigo == dadosEntrada.CodigoLivro).FirstOrDefaultAsync();
            if (livro != null)
            {
                var itemEmprestimo = new ItensEmprestimo()
                {
                    NumeroEmprestimo = dadosEntrada.NumeroEmprestimo,
                    CodigoLivro = dadosEntrada.CodigoLivro
                };

                await _bibliotecaDbContext.ItensEmprestimos.AddAsync(itemEmprestimo);
                await _bibliotecaDbContext.SaveChangesAsync();

                return Ok();
            }

            return NotFound("Livro não encontrado!");
        }
    }
}
