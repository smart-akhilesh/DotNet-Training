using System;
using System.Collections.Generic;
using System.Text;

namespace Electricity_Board_Billing_Prj
{
    public partial class BillEntry : System.Web.UI.Page
    {
        private ElectricityBoard electricityBoard = new ElectricityBoard();
        private List<string> processedBills = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
      
            if (Session["AdminUser"] == null)
            {
                Response.Redirect("~/Account/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                ResetForm();
            }
            else if (ViewState["ProcessedBills"] != null)
            {
                processedBills = (List<string>)ViewState["ProcessedBills"];
            }
        }

        protected void btnStartEntry_Click(object sender, EventArgs e)
        {
            try
            {
                int n = int.Parse(txtNumberOfBills.Text);
                if (n <= 0) throw new Exception("Enter a positive number");

                ViewState["NumberOfBills"] = n;
                ViewState["CurrentBillIndex"] = 1;
                processedBills = new List<string>();
                ViewState["ProcessedBills"] = processedBills;

                lblTotalBills.Text = n.ToString();
                lblCurrentBill.Text = "1";

                divNumberOfBills.Visible = false;
                divBillEntry.Visible = true;
                divResults.Visible = false;
             
                lblMessage.Text = "";
                btnStartOver.Visible = false; 
            }
            catch (Exception ex)
            {
                ShowMessage("Error: " + ex.Message, false);
            }
        }

        protected void btnAddBill_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtUnitsConsumed.Text, out int units))
                {
                    ShowMessage("Please enter a valid number for Units Consumed", false);
                    return;
                }

                string unitsValidation = BillValidator.ValidateUnitsConsumed(units);
                if (!string.IsNullOrEmpty(unitsValidation))
                {
                    ShowMessage(unitsValidation, false);
                    return;
                }

     
                ElectricityBill bill = new ElectricityBill
                {
                    ConsumerNumber = txtConsumerNumber.Text,
                    ConsumerName = txtConsumerName.Text,
                    UnitsConsumed = units
                };

                electricityBoard.CalculateBill(bill);
                electricityBoard.AddBill(bill);

                processedBills.Add($"{bill.ConsumerNumber} {bill.ConsumerName} {bill.UnitsConsumed} Bill Amount : {bill.BillAmount}");
                ViewState["ProcessedBills"] = processedBills;

                int current = (int)ViewState["CurrentBillIndex"];
                int total = (int)ViewState["NumberOfBills"];

                if (current >= total)
                {
                    ShowResults();
                }
                else
                {
                    current++;
                    ViewState["CurrentBillIndex"] = current;
                    lblCurrentBill.Text = current.ToString();

                    ClearBillEntryForm();
                    ShowMessage($"Bill added successfully! Current bill: {current} of {total}", true);
                }
            }
            catch (FormatException ex)
            {
                ShowMessage(ex.Message, false);
            }
            catch (Exception ex)
            {
                ShowMessage("Error processing bill: " + ex.Message, false);
            }
        }

       
        protected void btnReset_Click(object sender, EventArgs e) => ClearBillEntryForm();
        protected void btnStartOver_Click(object sender, EventArgs e) => ResetForm();

        private void ShowResults()
        {
            divBillEntry.Visible = false;
            divResults.Visible = true;

            StringBuilder sb = new StringBuilder();
            foreach (string bill in processedBills)
            {
                sb.AppendLine(bill + "<br/>");
            }
            lblResults.Text = sb.ToString();

            ShowMessage("All bills processed successfully!", true);
            btnStartOver.Visible = true;
        }

        private void ClearBillEntryForm()
        {
            txtConsumerNumber.Text = "";
            txtConsumerName.Text = "";
            txtUnitsConsumed.Text = "";
        }

        private void ResetForm()
        {
            ViewState.Clear();
            processedBills.Clear();

            divNumberOfBills.Visible = true;
            divBillEntry.Visible = false;
            divResults.Visible = false;
         

            txtNumberOfBills.Text = "";
            lblResults.Text = "";
            lblMessage.Text = "";

            btnStartOver.Visible = false;
            ClearBillEntryForm();
        }

        private void ShowMessage(string message, bool success)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = success ? "success-message" : "error-message";
        }
    }
}
