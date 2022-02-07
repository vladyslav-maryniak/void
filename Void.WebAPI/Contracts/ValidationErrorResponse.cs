using System.Collections.Generic;

namespace Void.WebAPI.Contracts
{
    public class ValidationErrorResponse
    {
        public List<ValidationErrorModel> Errors { get; set; } = new();
    }
}
