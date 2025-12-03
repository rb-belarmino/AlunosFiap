using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Fiap.Web.Alunos.ViewModels
{
    public class ClienteCreateViewModel
    {
        public int ClienteId { get; set; }
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [Display(Name = "Nome")]
        [StringLength(50, ErrorMessage = "O nome não pode exceder 50 caracteres.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O sobrenome é obrigatório.")]
        [Display(Name = "Sobrenome")]
        [StringLength(50, ErrorMessage = "O sobrenome não pode exceder 50 caracteres.")]
        public string Sobrenome { get; set; }
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [Display(Name = "E-mail")]
        [EmailAddress(ErrorMessage = "Insira um endereço de e-mail válido.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
        [Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }
        [Display(Name = "Observação")]
        [StringLength(200, ErrorMessage = "A observação não pode exceder 200 caracteres.")]
        public string? Observacao { get; set; }
        [Display(Name = "Representante")]
        [Required(ErrorMessage = "A seleção de um representante é obrigatória.")]
        [Range(1, int.MaxValue, ErrorMessage = "O representante selecionado é inválido.")]
        public int RepresentanteId { get; set; }
        [Display(Name = "Representantes")]
        public SelectList Representantes { get; set; }
        public ClienteCreateViewModel()
        {
            Representantes = new SelectList(Enumerable.Empty<SelectListItem>());
        }
    }
}