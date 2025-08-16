using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Asp_Assignment_1
{
    public partial class Validator : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          
        }

        protected void NameFamily_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = !txtName.Text.Equals(txtFamily.Text, StringComparison.OrdinalIgnoreCase);
        }

        protected void btnCheck_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                lblResult.Text = "All validations passed!";
                lblResult.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                lblResult.Text = "Please correct the errors above.";
                lblResult.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}
