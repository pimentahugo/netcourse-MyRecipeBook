using AutoMapper;
using MyRecipeBook.Application.Services.Automapper;

namespace CommonTestUtilities.Mapper;
public class MapperBuilder
{
	public static IMapper Build()
	{
		return new AutoMapper.MapperConfiguration(options =>
		{
			options.AddProfile(new AutoMapping());
		}).CreateMapper();
	}
}

