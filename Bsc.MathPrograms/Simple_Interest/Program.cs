using System;

class Program
{
    static void Main()
    {
        Console.Write("Enter Principal Amount (in ₹): ");
        int principal = Convert.ToInt32(Console.ReadLine());

        Console.Write("Enter Rate of Interest (in %): ");
        float rate = Convert.ToSingle(Console.ReadLine());

        Console.Write("Enter Time Period (in years): ");
        int time = Convert.ToInt32(Console.ReadLine());

        float simpleInterest = (principal * rate * time) / 100;

        Console.WriteLine($"Simple Interest for the given amount is: Rs.{simpleInterest}");
    }
}