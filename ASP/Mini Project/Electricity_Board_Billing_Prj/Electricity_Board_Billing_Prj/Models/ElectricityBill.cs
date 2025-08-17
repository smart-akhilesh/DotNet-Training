using System;

namespace Electricity_Board_Billing_Prj
{
    public class ElectricityBill
    {
    
        private string consumerNumber;
        private string consumerName;
        private int unitsConsumed;
        private double billAmount;

        public string ConsumerNumber
        {
            get
            {
                return consumerNumber;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new FormatException("Consumer Number cannot be empty.");
                }

                if (!value.StartsWith("EB"))
                {
                    throw new FormatException("Consumer Number should start with 'EB'.");
                }

                if (value.Length != 7)
                {
                    throw new FormatException("Consumer Number should be 7 characters long, e.g., EB12345.");
                }

                string numberPart = value.Substring(2);

                if (!int.TryParse(numberPart, out int parsedNumber))
                {
                    throw new FormatException("The part after 'EB' should be a 5-digit number.");
                }
                consumerNumber = value;
            }
        }


        public string ConsumerName
        {
            get { return consumerName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new FormatException("Consumer Name cannot be empty");
                consumerName = value;
            }
        }


        public int UnitsConsumed
        {
            get { return unitsConsumed; }
            set
            {
                if (value <= 0)
                    throw new FormatException("Units Consumed must be greater than 0");
                unitsConsumed = value;
            }
        }


        public double BillAmount
        {
            get { return billAmount; }
            set
            {
                if (value < 0)
                    throw new FormatException("Bill Amount cannot be negative");
                billAmount = value;
            }
        }
    }
}
