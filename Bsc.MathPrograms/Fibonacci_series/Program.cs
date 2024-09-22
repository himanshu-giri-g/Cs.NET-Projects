using System;

class Program
{
    static void Main()
    {
        int n = 10;
        int num1 = 0;
        int num2 = 1;
        int nextNumber = num2;
        int count = 1;

        while (count <= n)
        {
            Console.Write(nextNumber + " ");
            count++;
            num1 = num2;
            num2 = nextNumber;
            nextNumber = num1 + num2;
        }
        Console.WriteLine();
    }
}