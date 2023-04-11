using System;

namespace PESEL_Validator
{
    class Program
    {
        static void Main(string[] args)
        {
            string pesel;
            Console.WriteLine("Enter your PESEL.");
            pesel = Console.ReadLine();
            Console.WriteLine();

            PESEL userPESEL = new PESEL(pesel);

            Console.WriteLine(userPESEL.Sex);
            Console.WriteLine($"{userPESEL.Day} {userPESEL.Month} {userPESEL.Year}");

            Console.ReadKey();
        }
    }
}