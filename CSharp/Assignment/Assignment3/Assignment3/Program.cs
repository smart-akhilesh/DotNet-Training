using System;

namespace Assignment3
{
    class Accounts
    {
        private double accountNo;
        private string customerName;
        private string accountType;
        private double balance;
        public Accounts(double accountNo, string customerName, string accountType)
        {
            this.accountNo = accountNo;
            this.customerName = customerName;
            this.accountType = accountType;
            this.balance = 0;
        }

        public void Credit(double amount)
        {
            balance += amount;
            Console.WriteLine($"Rs {amount} deposited. Updated Balance: Rs {balance}");
        }

        public void Debit(double amount)
        {
            if (amount > balance)
            {
                Console.WriteLine("Insufficient Balance!");
            }
            else
            {
                balance -= amount;
                Console.WriteLine($"Rs {amount} withdrawn. Updated Balance: Rs {balance}");
            }
        }

        public void ProcessTransaction(string transactionType, double amount)
        {
            if (transactionType.ToLower() == "d")
            {
                Credit(amount);
            }
            else if (transactionType.ToLower() == "w")
            {
                Debit(amount);
            }
            else
            {
                Console.WriteLine("Invalid Transaction Type!");
            }
        }

        public void ShowData()
        {
            Console.WriteLine("\n----- Account Information -----");
            Console.WriteLine($"Account Number  : {accountNo}");
            Console.WriteLine($"Customer Name   : {customerName}");
            Console.WriteLine($"Account Type    : {accountType}");
            Console.WriteLine($"Current Balance : Rs {balance}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter Account Number: ");
            double accNo = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter Customer Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter Account Type (Saving/Current): ");
            string accType = Console.ReadLine();

            Accounts acc = new Accounts(accNo, name, accType);

            Console.Write("Enter Transaction Type (d for deposit / w for withdrawal): ");
            string transType = Console.ReadLine();

            Console.Write("Enter Amount: ");
            double amt = Convert.ToDouble(Console.ReadLine());

            acc.ProcessTransaction(transType, amt);

            acc.ShowData();
            Console.Read();
        }
    }
}

