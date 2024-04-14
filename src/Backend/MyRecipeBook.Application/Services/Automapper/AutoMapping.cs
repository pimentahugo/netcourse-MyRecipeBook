using AutoMapper;
using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Application.Services.Automapper;
public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToDomain();
    }

    private void RequestToDomain()
    {
        CreateMap<RequestedRegisterUserJson, Domain.Entities.User>()
            .ForMember(destin => destin.Password, option => option.Ignore());
    }

    private void DomainToRequest()
    {

    }
}

