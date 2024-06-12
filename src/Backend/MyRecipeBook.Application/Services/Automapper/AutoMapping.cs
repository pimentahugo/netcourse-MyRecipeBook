using AutoMapper;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.Services.Automapper;
public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToDomain();
        DomainToRequest();
    }

    private void RequestToDomain()
    {
        CreateMap<RequestRegisterUserJson, Domain.Entities.User>()
            .ForMember(destin => destin.Password, option => option.Ignore());
    }

    private void DomainToRequest()
    {
        CreateMap<Domain.Entities.User, ResponseUserProfileJson>();
	}
}

