using Sqids;

namespace CommonTestUtilities.IdEncryption;
public class IdEncripterBuilder
{
	public static SqidsEncoder<long> Build()
	{
		return new SqidsEncoder<long>(new()
		{
			MinLength = 3,
			Alphabet = "PQRL9KzJAfOBsvTty7ZloYewbCHE1iquXhxp60S8IrNF5dgcM2GjDkUWan4Vm3",
		});
	}
}

