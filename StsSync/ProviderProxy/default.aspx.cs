using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;

namespace ProviderProxy
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ProviderManager pm = new ProviderManager();
            var providers = pm.GetAllIProviders();

            StringBuilder sb = new StringBuilder();
            panel1.Controls.Clear();
            foreach (IProvider provider in providers)
            {
                ListType listType = provider.GetProviderType(provider.ID);
                Uri baseUrl = new Uri(Request.Url.Scheme + "://" + Request.Url.Host + ":" + Request.Url.Port);
                sb.Append("<a href=\"");
                sb.Append(ListsHelper.GetStsUrl(provider.ID, "Sts Sync", baseUrl));
                sb.Append("\">" + provider.Name + "</a>");
                sb.Append("<br/>");
            }
            panel1.Controls.Add(new LiteralControl(sb.ToString()));
        }

    }
}
