using FluentValidation;
using System.Linq;
using Void.BLL.Services.Abstractions;
using Void.WebAPI.Validators.Abstractions;

namespace Void.WebAPI.Validators.Extensions
{
    public static class CoinValidationRules
    {
        public static IRuleBuilderOptions<T, string> IsCoinId<T>(
            this IRuleBuilder<T, string> ruleBuilder, ICryptoDataProvider dataProvider)
            where T : IIdentifiable<string> 
            => ruleBuilder
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
