using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Locadora.Models;
using Locadora.Data;

namespace Locadora.Controllers
{
    [Route("api/emprestimos")]
    [ApiController]
    public class EmprestimosController : ControllerBase
    {
        private readonly LocadoraDbContext _context;

        public EmprestimosController(LocadoraDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetEmprestimos()
        {
            var emprestimos = await _context.Emprestimos
                .Include(e => e.Cliente)
                .Include(e => e.Filme)
                .Select(e => new
                {
                    Id = e.Id,
                    ClienteNome = e.Cliente.Nome,
                    FilmeNome = e.Filme.Titulo,
                    DataEmprestimo = e.DataEmprestimo,
                    DataDevolucao = e.DataDevolucao
                })
                .ToListAsync();

            return emprestimos;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetEmprestimo(int id)
        {
            var emprestimo = await _context.Emprestimos
                .Where(e => e.Id == id)
                .Include(e => e.Cliente)
                .Include(e => e.Filme)
                .Select(e => new
                {
                    Id = e.Id,
                    ClienteNome = e.Cliente.Nome,
                    FilmeNome = e.Filme.Titulo,
                    DataEmprestimo = e.DataEmprestimo,
                    DataDevolucao = e.DataDevolucao
                })
                .FirstOrDefaultAsync();

            if (emprestimo == null)
            {
                return NotFound();
            }

            return emprestimo;
        }


        [HttpPost]
        public async Task<ActionResult<Emprestimo>> PostEmprestimo(Emprestimo emprestimo)
        {
            // Verificar se o Cliente e o Filme existem no banco de dados
            var cliente = await _context.Clientes.FindAsync(emprestimo.ClienteId);
            var filme = await _context.Filmes.FindAsync(emprestimo.FilmeId);

            // Verificar se o Cliente e o Filme são válidos
            if (cliente == null || filme == null)
            {
                return BadRequest("Cliente ou Filme não encontrados.");
            }

            // Verificar se o cliente atende aos requisitos de idade para o filme
            if (cliente.Idade < filme.ClassificacaoEtaria)
            {
                return BadRequest("O cliente não atende aos requisitos de idade para este filme.");
            }

            // Carregar o Cliente e o Filme relacionados
            emprestimo.Cliente = cliente;
            emprestimo.Filme = filme;

            _context.Emprestimos.Add(emprestimo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmprestimo", new { id = emprestimo.Id }, emprestimo);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmprestimo(int id, [FromBody] Emprestimo novoEmprestimo)
        {
            var emprestimo = await _context.Emprestimos.FindAsync(id);

            if (emprestimo == null)
            {
                return NotFound();
            }

            emprestimo.DataEmprestimo = novoEmprestimo.DataEmprestimo;
            emprestimo.DataDevolucao = novoEmprestimo.DataDevolucao;

            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmprestimo(int id)
        {
            var emprestimo = await _context.Emprestimos.FindAsync(id);
            if (emprestimo == null)
            {
                return NotFound();
            }

            _context.Emprestimos.Remove(emprestimo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmprestimoExists(int id)
        {
            return _context.Emprestimos.Any(e => e.Id == id);
        }
    }
}