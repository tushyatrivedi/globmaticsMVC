using Globomatics.Infrastructure.Repositories;

namespace Globomatics.Web.Repositories
{
    public class SessionStateRepository : IStateRepository
    {
        private readonly IHttpContextAccessor contextAccessor;

        public SessionStateRepository(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }
        public string GetValue(string key)
        {
            return contextAccessor.HttpContext?.Session.GetString(key) ?? string.Empty; 
        }

        public void Remove(string key)
        {
            contextAccessor.HttpContext?.Session.Remove(key);
        }

        public void SetValue(string key, string value)
        {
            contextAccessor.HttpContext?.Session.SetString(key, value);
        }
    }
}
