using System.Web;
using System.Web.Mvc;

namespace RabbitMQ_WebAPI_Demo.Producer
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
