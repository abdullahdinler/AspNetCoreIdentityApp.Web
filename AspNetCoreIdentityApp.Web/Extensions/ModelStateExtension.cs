using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AspNetCoreIdentityApp.Web.Extensions
{
    public static class ModelStateExtension
    {
        public static void AddModelErrorExtension(this ModelStateDictionary modelStateDictionary, List<string> errors)
        {
            // ModelStateDictionary sınıfına errors adında bir liste alır ve bu listeyi döngü ile ModelStateDictionary sınıfına ekler.
            errors.ForEach(error => modelStateDictionary.AddModelError(string.Empty, error));
        }
    }
}
