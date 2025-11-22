using Fiap.Web.Alunos.Data.Contexts;
using Fiap.Web.Alunos.Models;
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
            ViewBag.Representantes = 
                new SelectList(_context.Representantes.ToList(),
                    "RepresentanteId",
                    "NomeRepresentante");
            return View();
        }
        [HttpPost]
        public IActionResult Create(ClienteModel clienteModel)
        {
            _context.Clientes.Add(clienteModel);
            _context.SaveChanges();
            TempData["mensagemSucesso"] = $"O cliente {clienteModel.Nome} foi cadastrado com sucesso";
            return RedirectToAction(nameof(Index));
           
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