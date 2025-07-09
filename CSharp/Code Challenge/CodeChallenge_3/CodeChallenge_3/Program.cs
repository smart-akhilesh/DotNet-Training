using System;

namespace CodeChallenge_3
{
    class CricketTeam
    {
        public (double average, int sum, int NoOfMatches) PointsCalculation(int NoOfMatches)
        {
            int sum = 0;

            for (int i = 0; i < NoOfMatches; i++)
            {
                try
                {
                    Console.WriteLine($"Enter the score in match {i + 1}:");
                    int score = Convert.ToInt32(Console.ReadLine());
                    sum += score;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid score. Please enter a valid integer.");
                    i--; 
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unexpected error: " + ex.Message);
                    i--;
                }
            }

            double average = (double)sum / NoOfMatches;
            return (average, sum, NoOfMatches);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Enter the number of matches:");
                int NoOfMatches = Convert.ToInt32(Console.ReadLine());

                if (NoOfMatches <= 0)
                {
                    Console.WriteLine("Number of matches must be greater than zero.");
                    return;
                }

                CricketTeam cricteam = new CricketTeam();
                var (average, sum, matches) = cricteam.PointsCalculation(NoOfMatches);

                Console.WriteLine($"\nThe average of the score :  {average}");
                Console.WriteLine($"Sum of the score         :  {sum}");
                Console.WriteLine($"Number of matches        :  {matches}");
            }
            catch (FormatException)
            {
                Console.WriteLine("Please enter a valid integer for number of matches.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.Message);
            }

            Console.Read();
        }
    }
}
