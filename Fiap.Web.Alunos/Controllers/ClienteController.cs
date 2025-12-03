using AutoMapper;
using Fiap.Web.Alunos.Data.Contexts;
using Fiap.Web.Alunos.Models;
using Fiap.Web.Alunos.Services;
using Fiap.Web.Alunos.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore; 
namespace Fiap.Web.Alunos.Controllers
{
    public class ClienteController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly IClienteService _clienteService;
        public ClienteController(DatabaseContext context, IMapper mapper, IClienteService clienteService)
        {
            _context = context;
            _mapper = mapper;
            _clienteService = clienteService;
        }
        public IActionResult Index()
        {
            var clientes = _clienteService.ListarClientes();
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
            // Verifica se todos os dados enviados estão válidos conforme as regras definidas no ViewModel
            if (ModelState.IsValid)
            {
                var cliente = _mapper.Map<ClienteModel>(viewModel);
                _clienteService.CriarCliente(cliente);
                TempData["mensagemSucesso"] = $"O cliente {viewModel.Nome} foi cadastrado com sucesso";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Se os dados não estão válidos, recarrega a lista de representantes para a seleção na View
                viewModel.Representantes = new SelectList(_context.Representantes.ToList(), "RepresentanteId", "NomeRepresentante", viewModel.RepresentanteId);
                // Retorna a View com o ViewModel contendo os dados submetidos e os erros de validação
                return View(viewModel);
            }
        }
        // Anotação de uso do Verb HTTP Get
        [HttpGet]
        public IActionResult Detail(int id)
        {
            var cliente = _clienteService.ObterClientePorId(id);
            if (cliente == null) return NotFound();
            return View(cliente);
        }
        // Anotação de uso do Verb HTTP Get
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var cliente = _clienteService.ObterClientePorId(id);
            if (cliente == null) { 
                return NotFound();
            } else {  
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
            _clienteService.AtualizarCliente(clienteModel);
            TempData["mensagemSucesso"] = $"Os dados do cliente {clienteModel.Nome} foram alterados com sucesso";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            _clienteService.DeletarCliente(id);
            TempData["mensagemSucesso"] = $"Os dados do cliente foram removidos com sucesso";
            return RedirectToAction(nameof(Index));
        }
    }
}