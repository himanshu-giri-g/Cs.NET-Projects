using System;

class Program
{
    static void Main()
    {
        Console.Write("Enter a word: ");
        string strN = Console.ReadLine();

        if (strN.SequenceEqual(strN.Reverse()))
        {
            Console.WriteLine($"The word {strN} is a Palindrome.");
        }
        else
        {
            Console.WriteLine($"The word {strN} is not a Palindrome.");
        }
    }
}