using FluentValidation;
using Void.BLL.Services.Abstractions;
using Void.WebAPI.DTOs.Coin;
using Void.WebAPI.Validators.Extensions;

namespace Void.WebAPI.Validators
{
    public class BlacklistedCoinAddDtoValidator : AbstractValidator<BlacklistedCoinAddDto>
    {
        public BlacklistedCoinAddDtoValidator(ICryptoDataProvider dataProvider)
        {
            RuleFor(x => x.Id)
                .Cascade(CascadeMode.Stop)
                .IsCoinId(dataProvider);

            RuleFor(x => x.Reason)
                .NotNull()
                .NotEmpty()
                .Length(1, 256);
        }
    }
}
