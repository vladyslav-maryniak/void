using System.Collections.Specialized;
using System.Linq;

namespace Void.BLL.Extensions
{
    public static class NameValueCollectionExtensions
    {
        public static string ToQueryString(this NameValueCollection collection)
        {
            var parameterValues = collection.AllKeys.Select(x => $"{x}={collection[x]}");
            return string.Join('&', parameterValues);
        }
    }
}
