using System;
using System.Collections.Generic;
using System.Text;
using MultithreadVSAsync.Model;

namespace MultithreadVSAsync.ViewModel
{
    internal class ViewModel : IViewModel.IViewModel
    {
        public static void Choose()
        {
            Console.WriteLine("Welcome to this Async vs Multithreadin comparaison.");
            Console.WriteLine("Please choose the method you want to test :");
            Console.WriteLine("1 - Multithreading");
            Console.WriteLine("2 - Asynchronisme");
            Console.WriteLine("3 - Exit");
            Console.Write("Your choice: ");
        }
        public static void ApplyChoice(string choice)
        {
            switch (choice)
            {
                case "0":
                    Choose();
                    break;
                case "1":
                    Console.Clear();
                    Multithread.MultithreadMethod();
                    break;
                case "2":
                    Console.Clear();
                    Asynchronisme.AsynchronismeMethod();
                    break;
                case "3":
                    Console.Clear();
                    Console.WriteLine("Goodbye !");
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Invalid choice, please try again.");
                    Choose();
                    break;
            }
        }
        public void Start()
        {
            string choice = "0";
            while (choice != "1" && choice != "2" && choice != "3") {
                ApplyChoice(choice);
                choice = Console.ReadLine();
            }
            ApplyChoice(choice);

        }
    }
}
