/* Scenario:
You're developing a banking system where users can withdraw money from their account. If the withdrawal 
amount exceeds the daily limit (e.g., ₹50,000), the system must throw a custom exception called DailyLimitExceededException.
Question:
Create a custom exception class DailyLimitExceededException that includes a message and the attempted 
withdrawal amount. Then, implement a method Withdraw(decimal amount) in a BankAccount class that throws
this exception if the withdrawal exceeds the limit.
Demonstrate how you would catch this custom exception and display an appropriate message to the user.
*/
using System;

namespace CodeChallenge1
{  
    class DailyLimitExceededException : ApplicationException
    {
        public int AmountAttempted { get; }

        public DailyLimitExceededException(string message, int amount) : base(message)
        {
            AmountAttempted = amount;
        }
    }
    
    class BankAccount
    {
        public int Balance { get; set; }
        public void Withdraw(int amount)
        {
            if (amount > 50000)
            {
                throw new DailyLimitExceededException("Withdrawal amount exceeds daily limit.", amount);
            }

            if (amount > Balance)
            {
                Console.WriteLine("Insufficient funds.");
                return;
            }

            Balance -= amount;
            Console.WriteLine($"Withdrawal successful. Remaining balance: {Balance}");
        }


        static void Main()
        {
            BankAccount account = new BankAccount { Balance = 100000 };
            Console.WriteLine("Available balance. : {0}",account.Balance);

            Console.WriteLine("How much amount do you want to withdraw");
            int amount = Convert.ToInt32(Console.ReadLine());

            try
            {
                account.Withdraw(amount);
            }
            catch (DailyLimitExceededException e)
            {
                Console.WriteLine($"{e.Message} You attempted to withdraw {e.AmountAttempted}.");
            }

            Console.Read();
        }
    }

}


  