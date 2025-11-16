using Fiap.Web.Alunos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Fiap.Web.Alunos.Controllers
{
    public class ClienteController : Controller
    {
        public IList<ClienteModel> Clientes { get; set; }
        public IList<RepresentanteModel> Representantes { get; set; } 
        public ClienteController()
        {
            Clientes = GerarClientesMocados();
            Representantes = GerarRepresentantesMocados();
        }
        public IActionResult Index()
        {
            if (Clientes == null)
            {
                Clientes = new List<ClienteModel>();
            }
            return View(Clientes);
        }
        [HttpGet]
        public IActionResult Create()
        {
            Console.WriteLine("Executou a Action Cadastrar()");
            var selectListRepresentantes =
                new SelectList(Representantes,
                                nameof(RepresentanteModel.RepresentanteId),
                                nameof(RepresentanteModel.NomeRepresentante));
           
            ViewBag.Representantes = selectListRepresentantes;
           
            return View(new ClienteModel());
        }
        [HttpPost]
        public IActionResult Create(ClienteModel clienteModel)
        {
            Console.WriteLine("Gravando o cliente");
            TempData["mensagemSucesso"] = $"O cliente {clienteModel.Nome} foi cadastrado com suceso";
            return RedirectToAction(nameof(Index));
           
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var selectListRepresentantes = 
                new SelectList(Representantes,
                    nameof(RepresentanteModel.RepresentanteId),
                    nameof(RepresentanteModel.NomeRepresentante));
            ViewBag.Representantes = selectListRepresentantes;
            var clienteConsultado = Clientes.Where(c => c.ClienteId == id).FirstOrDefault();
            return View(clienteConsultado);
        }

        [HttpPost]
        public IActionResult Edit(ClienteModel clienteModel)
        {
            TempData["mensagemSucesso"] = $"O cliente {clienteModel.Nome} foi editado com suceso";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Detail(int id)
        {
            var selectListRepresentantes =
                new SelectList(Representantes,
                    nameof(RepresentanteModel.RepresentanteId),
                    nameof(RepresentanteModel.NomeRepresentante));
            ViewBag.Representantes = selectListRepresentantes;
            var clienteConsultado = Clientes.Where(c => c.ClienteId == id).FirstOrDefault();
            return View(clienteConsultado);
        }
       
        public static List<RepresentanteModel> GerarRepresentantesMocados()
        {
            var representantes = new List<RepresentanteModel>
            {
                new RepresentanteModel { RepresentanteId = 1, NomeRepresentante = "Representante 1", Cpf = "111.111.111-11" },
                new RepresentanteModel { RepresentanteId = 2, NomeRepresentante = "Representante 2", Cpf = "222.222.222-22" },
                new RepresentanteModel { RepresentanteId = 3, NomeRepresentante = "Representante 3", Cpf = "333.333.333-33" },
                new RepresentanteModel { RepresentanteId = 4, NomeRepresentante = "Representante 4", Cpf = "444.444.444-44" },
                new RepresentanteModel { RepresentanteId = 5, NomeRepresentante = "Representante 5", Cpf = "555.555.555-55" },
                new RepresentanteModel { RepresentanteId = 6, NomeRepresentante = "Representante 6", Cpf = "666.666.666-66" },
                new RepresentanteModel { RepresentanteId = 7, NomeRepresentante = "Representante 7", Cpf = "777.777.777-77" },
            };
            return representantes;
        }
       
        public static List<ClienteModel> GerarClientesMocados()
        {
            var clientes = new List<ClienteModel>();
            for (int i = 1; i <= 5; i++)
            {
                var cliente = new ClienteModel
                {
                    ClienteId = i,
                    Nome = "Cliente" + i,
                    Sobrenome = "Sobrenome" + i,
                    Email = "cliente" + i + "@example.com",
                    DataNascimento = DateTime.Now.AddYears(-30),
                    Observacao = "Observação do cliente " + i,
                    RepresentanteId = i,
                    Representante = new RepresentanteModel
                    {
                        RepresentanteId = i,
                        NomeRepresentante = "Representante" + i,
                        Cpf = "00000000191"
                    }
                };
                clientes.Add(cliente);
            }
            return clientes;
        }
    }
}