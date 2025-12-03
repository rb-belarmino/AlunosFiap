using Fiap.Web.Alunos.Data.Contexts;
using Fiap.Web.Alunos.Models;
using Fiap.Web.Alunos.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Fiap.Web.Alunos.Controllers
{
    public class ClienteController : Controller
    {
        private readonly DatabaseContext _context;
        public ClienteController(DatabaseContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var clientes = _context.Clientes.Include(c => c.Representante).ToList();
            
            return View(clientes);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new ClienteCreateViewModel
            {
                Representantes = new SelectList(_context.Representantes.ToList(), "RepresentanteId", "NomeRepresentante")
            };
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Create(ClienteCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var cliente = new ClienteModel
                {
                    ClienteId = viewModel.ClienteId,
                    Nome = viewModel.Nome,
                    Sobrenome = viewModel.Sobrenome,
                    Email = viewModel.Email,
                    DataNascimento = viewModel.DataNascimento,
                    Observacao = viewModel.Observacao,
                    RepresentanteId = viewModel.RepresentanteId
                };
                _context.Clientes.Add(cliente);
                _context.SaveChanges();
                TempData["mensagemSucesso"] = $"O cliente {viewModel.Nome} foi cadastrado com sucesso";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                viewModel.Representantes = new SelectList(_context.Representantes.ToList(), "RepresentanteId", "NomeRepresentante", viewModel.RepresentanteId);
                return View(viewModel);
            }
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var cliente = _context.Clientes.Find(id);
            if (cliente == null)
            {
                return NotFound();
            }
            else
            {
                ViewBag.Representantes = 
                    new SelectList(_context.Representantes.ToList(),
                        "RepresentanteId",
                        "NomeRepresentante",
                        cliente.RepresentanteId);
                return View(cliente);
            }
        }

        [HttpPost]
        public IActionResult Edit(ClienteModel clienteModel)
        {
            _context.Clientes.Update(clienteModel);
            _context.SaveChanges();
            TempData["mensagemSucesso"] = $"O cliente {clienteModel.Nome} foi editado com sucesso";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Detail(int id)
        {
            var cliente = _context.Clientes
                .Include(c => c.Representante)
                .FirstOrDefault(c => c.ClienteId == id);
            if (cliente == null)
            {
                return NotFound();
            }
            else
            {
                return View(cliente);
            }
        }
        
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var cliente = _context.Clientes.Find(id);

            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
                _context.SaveChanges();
                TempData["mensagemSucesso"] = $"O cliente {cliente.Nome} foi excluído com sucesso";
            }
            else
            {
                TempData["mensagemErro"] = "Cliente não encontrado";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}