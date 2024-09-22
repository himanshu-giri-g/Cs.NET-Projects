using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("---WELCOME TO TEMPERATURE CONVERTER---");
        Console.WriteLine("1. Celsius to Kelvin\n2. Kelvin to Celsius");

        int option = Convert.ToInt32(Console.ReadLine());

        if (option == 1)
        {
            Console.Write("Enter Temperature to be converted (in Celsius): ");
            int tempC = Convert.ToInt32(Console.ReadLine());
            double tempK = CelsiusToKelvin(tempC);
            Console.WriteLine($"The Temperature in Kelvin for the given temperature is: {tempK} K");
        }
        else if (option == 2)
        {
            Console.Write("Enter Temperature to be converted (in Kelvin): ");
            int tempK = Convert.ToInt32(Console.ReadLine());
            double tempC = KelvinToCelsius(tempK);
            Console.WriteLine($"The Temperature in Celsius for the given temperature is: {tempC} C");
        }
        else
        {
            Console.WriteLine("ERROR ! Please Choose a valid option.");
        }
    }

    static double CelsiusToKelvin(int temp)
    {
        return temp + 273;
    }

    static double KelvinToCelsius(int temp)
    {
        return temp - 273;
    }
}