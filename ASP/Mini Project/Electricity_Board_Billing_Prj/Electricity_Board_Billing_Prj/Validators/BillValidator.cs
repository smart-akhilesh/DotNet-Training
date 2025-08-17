namespace Electricity_Board_Billing_Prj
{
    public class BillValidator
    {
        public static string ValidateUnitsConsumed(int units)
        {
            if (units <= 0) return "Given units is invalid";
            return "";
        }
    }
}
