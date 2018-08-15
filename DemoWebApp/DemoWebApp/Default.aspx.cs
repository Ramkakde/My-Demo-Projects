using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DemoWebApp
{
    public partial class Default : System.Web.UI.Page
    {
        public static string BaseGlobleServiceURL = System.Configuration.ConfigurationManager.AppSettings["BaseGlobleServiceURL"]; 
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
    }
}