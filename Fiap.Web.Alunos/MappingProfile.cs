using AutoMapper;
using Fiap.Web.Alunos.Models;
using Fiap.Web.Alunos.ViewModels;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Permite valores nulos para coleções e destinos
        AllowNullCollections = true;
        AllowNullDestinationValues = true;

        // Mapeamentos
        CreateMap<ClienteModel, ClienteCreateViewModel>().ReverseMap();
    }
}