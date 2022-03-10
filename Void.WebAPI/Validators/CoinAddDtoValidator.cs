using FluentValidation;
using Void.BLL.Services.Abstractions;
using Void.WebAPI.DTOs.Coin;
using Void.WebAPI.Validators.Extensions;

namespace Void.WebAPI.Validators
{
    public class CoinAddDtoValidator : AbstractValidator<CoinAddDto>
    {
        public CoinAddDtoValidator(ICryptoDataProvider dataProvider, ICoinService coinService)
        {
            RuleFor(x => x.Id)
                .Cascade(CascadeMode.Stop)
                .IsCoinId(dataProvider)
                .MustAsync(async (id, cancellationToken) =>
                {
                    var coinOptions = await coinService.GetBlacklistedCoinAsync(id, cancellationToken);
                    return coinOptions.IsNone;
                })
                .WithMessage("Coin with the given 'Id' exists in the blacklist");;
        }
    }
}
