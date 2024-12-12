using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Globomatics.Web.ValueProviders
{
    public class SessionValueProviderFactory : IValueProviderFactory
    {
        public Task CreateValueProviderAsync(ValueProviderFactoryContext context)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));

            var session =context.ActionContext.HttpContext.Session;

            if (session != null && session.Keys.Any()) 
            {
                //add value provider by instantiating a new value provider object
                context.ValueProviders.Add(new SessionValueProvider(BindingSource.ModelBinding,session));
            }

            return Task.CompletedTask;
        }
    }
}
