using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebApp.Helpers
{
    public static class ModelStateHelper
    {
        public static List<string> GetErrors(ModelStateDictionary modelState)
        {
            List<string> errorMessages = new List<string>();
            foreach (var value in modelState.Values)
            {
                foreach (var error in value.Errors)
                {
                    errorMessages.Add(error.ErrorMessage);
                }
            }

            return errorMessages;
        }
    }
}
