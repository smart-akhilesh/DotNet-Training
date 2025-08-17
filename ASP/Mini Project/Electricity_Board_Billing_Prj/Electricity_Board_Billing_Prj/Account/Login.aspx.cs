using System;
using System.Configuration;

namespace Electricity_Board_Billing_Prj.Account
{
    public partial class Login : System.Web.UI.Page
    {
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string u = ConfigurationManager.AppSettings["AdminUser"];
            string p = ConfigurationManager.AppSettings["AdminPassword"];

            if (txtUsername.Text == u && txtPassword.Text == p)
            {
                Session["AdminUser"] = u;
                Response.Redirect("~/Pages/BillEntry.aspx");
            }
            else
            {
                lblMessage.Text = "Invalid login credentials";
            }
        }
    }
}
