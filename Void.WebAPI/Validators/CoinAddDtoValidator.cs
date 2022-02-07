using FluentValidation;
using System.Linq;
using Void.BLL.Services.Abstractions;
using Void.WebAPI.DTOs.Coin;

namespace Void.WebAPI.Validators
{
    public class CoinAddDtoValidator : AbstractValidator<CoinAddDto>
    {
        public CoinAddDtoValidator(ICryptoDataProvider dataProvider)
        {
            RuleFor(x => x.Id)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .Length(1, 55)
                .Matches("^([a-z0-9]*)(-[a-z0-9]+)*$")
                .MustAsync(async (id, cancellationToken) =>
                {
                    var coins = await dataProvider.GetSupportedCoinsAsync(cancellationToken);
                    return coins.Select(x => x.Id).Contains(id);
                })
                .WithMessage("Could not find coin with the given 'Id'");
        }
    }
}
