using System.Web.Http;
using FabrikamCustomerServiceBot.App_Start;

namespace FabrikamCustomerServiceBot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(config =>
            {
                BotConfig.Register(config);
            });
        }
    }
}
