using AirplaneCore.Plus.Web.Data;
using Microsoft.Extensions.Configuration;

namespace AirplaneCore.Plus.Web.Cores
{
    public class Strings
    {
        private readonly IConfiguration _config;

        public Strings(IConfiguration config)
        {
            this._config = config;
        }

        public AppSettings AppSettings
        {
            get
            {
                return this._config.GetSection("AppSettings")?.Get<AppSettings>();
            }
        }

        public Secrets Secrets
        {
            get
            {
                var secrets = this._config.GetSection("Secrets")?.Get<Secrets>();
                if (secrets == null)
                {
                    secrets = this._config.GetSection("AppSettings:Secrets")?.Get<Secrets>();
                }
                return secrets;
            }
        }
    }
}
