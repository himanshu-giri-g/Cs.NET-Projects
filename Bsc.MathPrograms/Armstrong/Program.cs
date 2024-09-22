using System;

class Program
{
    static void Main()
    {
        Console.Write("Enter a Number: ");
        int a = Convert.ToInt32(Console.ReadLine());
        string b = a.ToString();
        int c = b.Length;
        int sum = 0;
        foreach (char i in b)
        {
            sum += (int)Math.Pow(int.Parse(i.ToString()), c);
        }
        if (sum == a)
        {
            Console.WriteLine($"The No. {a} is an Armstrong Number.");
        }
        else
        {
            Console.WriteLine($"The No. {a} is not an Armstrong Number.");
        }
    }
}