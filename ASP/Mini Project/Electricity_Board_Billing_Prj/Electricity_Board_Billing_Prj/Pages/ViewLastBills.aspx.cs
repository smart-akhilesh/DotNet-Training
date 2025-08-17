using System;
using System.Collections.Generic;
using System.Text;

namespace Electricity_Board_Billing_Prj
{
    public partial class ViewLastBills : System.Web.UI.Page
    {
        private ElectricityBoard electricityBoard = new ElectricityBoard();

        protected void Page_Load(object sender, EventArgs e)
        {
         
            if (Session["AdminUser"] == null)
            {
                Response.Redirect("~/Account/Login.aspx");
                return;
            }
        }

        protected void btnViewLastBills_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtLastNBills.Text, out int n) || n <= 0)
                {
                    lblLastBills.Text = " Enter a valid positive number.";
                    return;
                }

                List<ElectricityBill> lastBills = electricityBoard.Generate_N_BillDetails(n);

                if (lastBills.Count == 0)
                {
                    lblLastBills.Text = "No bills found in the database.";
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (ElectricityBill b in lastBills)
                    {
                        sb.Append("EB Bill for ").Append(b.ConsumerNumber)
                          .Append(" (").Append(b.ConsumerName).Append(")")
                          .Append(" → Units: ").Append(b.UnitsConsumed)
                          .Append(", Amount: ").Append(b.BillAmount)
                          .Append("<br/>");
                    }
                    lblLastBills.Text = sb.ToString();
                }
            }
            catch (Exception ex)
            {
                lblLastBills.Text = " Error retrieving bills: " + ex.Message;
            }
        }
    }
}
