using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Security;
using System.Web.SessionState;
using System.Xml.Linq;
using VirtualPathProvider;

namespace ProviderProxy
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e) {
            MasterPageVirtualPathProvider vpp = new MasterPageVirtualPathProvider();
            HostingEnvironment.RegisterVirtualPathProvider(vpp);

            ProviderProxy.ProviderManager.RegisterProvider(new AdventureWorksProvider.Contacts());
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            
            /*HttpContext ctx = HttpContext.Current;
            string path = ctx.Request.Url.AbsolutePath;

            if (path.IndexOf("_vti_bin") >= 0)
            {
                string newUrl = ctx.Request.ApplicationPath +
                   "/Service.asmx";
                ctx.RewritePath(newUrl);
            }   
            */
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}