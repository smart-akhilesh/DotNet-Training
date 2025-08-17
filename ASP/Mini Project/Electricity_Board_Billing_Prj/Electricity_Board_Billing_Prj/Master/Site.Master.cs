using System;

namespace Electricity_Board_Billing_Prj
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AdminUser"] != null)
            {
                btnLogout.Visible = true;
            }
            else
            {
                btnLogout.Visible = false;
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Account/Login.aspx");
        }
    }
}
