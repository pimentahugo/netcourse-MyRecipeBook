using AutoMapper;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
//using MyRecipeBook.Domain.Enums;
using MyRecipeBook.Communication.Enums;
using Sqids;

namespace MyRecipeBook.Application.Services.Automapper;
public class AutoMapping : Profile
{
    private readonly SqidsEncoder<long> _idEncoder;

    public AutoMapping(SqidsEncoder<long> idEnconder)
    {
		_idEncoder = idEnconder;

        RequestToDomain();
        DomainToRequest();
    }

    private void RequestToDomain()
    {
        CreateMap<RequestRegisterUserJson, Domain.Entities.User>()
            .ForMember(destin => destin.Password, option => option.Ignore());

		CreateMap<RequestRecipeJson, Domain.Entities.Recipe>()
		   .ForMember(destin => destin.Instructions, option => option.Ignore())
           .ForMember(destin => destin.Ingredients, opt => opt.MapFrom(source => source.Ingredients.Distinct()))
           .ForMember(destin => destin.DishTypes, opt => opt.MapFrom(source => source.DishTypes.Distinct()));

        CreateMap<string, Domain.Entities.Ingredient>()
            .ForMember(destin => destin.Item, opt => opt.MapFrom(source => source));

		CreateMap<DishType, Domain.Entities.DishType>()
			.ForMember(destin => destin.Type, opt => opt.MapFrom(source => source));

        CreateMap<RequestInstructionJson, Domain.Entities.Instruction>();
	}

    private void DomainToRequest()
    {
        CreateMap<Domain.Entities.User, ResponseUserProfileJson>();
        CreateMap<Domain.Entities.Recipe, ResponseRegisteredRecipeJson>()
            .ForMember(destin => destin.Id, config => config.MapFrom(source => _idEncoder.Encode(source.Id)));
	}
}

