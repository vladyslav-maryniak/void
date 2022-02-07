using FluentValidation;
using System.Linq;
using Void.BLL.Services.Abstractions;
using Void.WebAPI.DTOs.Exchange;

namespace Void.WebAPI.Validators
{
    public class ExchangeAddDtoValidator : AbstractValidator<ExchangeAddDto>
    {
        public ExchangeAddDtoValidator(ICryptoDataProvider dataProvider)
        {
            RuleFor(x => x.Id)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .Length(1, 35)
                .Matches("^([a-z0-9]*)(-[a-z0-9]+)*$")
                .MustAsync(async (id, cancellationToken) =>
                {
                    var exchanges = await dataProvider.GetSupportedExchangesAsync(cancellationToken);
                    return exchanges.Select(x => x.Id).Contains(id);
                })
                .WithMessage("Could not find exchange with the given 'Id'");
        }
    }
}
