using System;

class Program
{
    static void Main()
    {
        Console.Write("Enter a number: ");
        int num = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine(Factorial(num));
    }

    static int Factorial(int num)
    {
        if (num == 1 || num == 0)
        {
            return 1;
        }
        else
        {
            return num * Factorial(num - 1);
        }
    }
}