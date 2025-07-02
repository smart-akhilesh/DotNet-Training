using System;

namespace Assignment5
{
    public class InsufficientBalanceException : Exception
    {
        public InsufficientBalanceException(string message) : base(message) { }
    }

    public class Accounts
    {
        public int AccountNo { get; set; }
        public string CustomerName { get; set; }
        public string AccountType { get; set; }
        public int Balance { get; private set; }

        public Accounts(int accountNo, string customerName, string accountType, int openingBalance)
        {
            AccountNo = accountNo;
            CustomerName = customerName;
            AccountType = accountType;
            Balance = openingBalance;
        }

        public void Credit(int amount)
        {
            Balance += amount;
            Console.WriteLine($"Rs {amount} deposited successfully");
        }

        public void Debit(int amount)
        {
            if (amount > Balance)
                throw new InsufficientBalanceException("Not enough balance for withdrawal.");
            Balance -= amount;
            Console.WriteLine($"Rs {amount} withdrawn successfully");
        }

        public void PerformTransaction(char type, int amount)
        {
            switch (char.ToUpper(type))
            {
                case 'D':
                    Credit(amount);
                    break;
                case 'W':
                    Debit(amount);
                    break;
                default:
                    Console.WriteLine("Invalid transaction type.");
                    break;
            }
        }

        public void ShowData()
        {
            Console.WriteLine("\nAccount Details:");
            Console.WriteLine($"Account No   : {AccountNo}");
            Console.WriteLine($"Name         : {CustomerName}");
            Console.WriteLine($"Account Type : {AccountType}");
            Console.WriteLine($"Balance      : {Balance}");
        }
    }

    class Program3
    {
        public static void Main()
        {
            try
            {
                Console.WriteLine("Enter the deatails to create a new account");
                Console.Write("Account No: ");
                int accNo = Convert.ToInt32(Console.ReadLine());

                Console.Write("Customer Name: ");
                string name = Console.ReadLine();

                Console.Write("Account Type: ");
                string type = Console.ReadLine();

                Console.Write("Opening Balance: ");
                int opening = Convert.ToInt32(Console.ReadLine());

                var acc = new Accounts(accNo, name, type, opening);

                bool exit = false;

                while (!exit)
                {
                    Console.WriteLine("\nChoose an option:");
                    Console.WriteLine("1. Deposit");
                    Console.WriteLine("2. Withdraw");
                    Console.WriteLine("3. Show Account Details");
                    Console.WriteLine("4. Exit");

                    Console.Write("Enter choice (1-4): ");
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            Console.Write("Enter amount to deposit: ");
                            int depositAmt = Convert.ToInt32(Console.ReadLine());
                            acc.PerformTransaction('D', depositAmt);
                            break;

                        case "2":
                            Console.Write("Enter amount to withdraw: ");
                            int withdrawAmt = Convert.ToInt32(Console.ReadLine());
                            acc.PerformTransaction('W', withdrawAmt);
                            break;

                        case "3":
                            acc.ShowData();
                            break;

                        case "4":
                            exit = true;
                            break;

                        default:
                            Console.WriteLine("Invalid option.");
                            break;
                    }
                }
            }
            catch (InsufficientBalanceException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input format.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong: " + ex.Message);
            }
            Console.Read();
        }
    }
}