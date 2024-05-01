using Data;
using Services.Interfaces;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.DI;
using Common;

namespace ATM
{
    public class Engine
    {
        public void Run()
        {
            ServiceContainer container = ServiceContainer.GetInstance();
            ATMService atm = container.GetService<ATMService>();

            string chosenATM = ChooseATM(atm);

            Guid clientId = Guid.Empty;
            int i = 0;

            while(i < Constants.MAX_LOGIN_ATTEMPTS)
            {
                clientId = Login(atm);

                if (clientId == Guid.Empty) Console.WriteLine("Invalid credentials");
                else break;

                i++;
            }

            if (clientId != Guid.Empty)
            {
                while (true)
                {
                    ATMService atmService = container.GetService<ATMService>();

                    Console.Write($"{chosenATM} > ");
                    string command = Console.ReadLine().ToLower().Trim();

                    if (command == "exit")
                    {
                        Console.WriteLine("Logging out and Exiting");
                        break;
                    }

                    atmService.DelegateCommand(command, clientId);
                }
            }
            
        }

        static string ChooseATM(ATMService atmService)
        {
            var atms = atmService.GetATMs();

            foreach (var atm in atms)
            {
                Console.WriteLine(atm.Name);
            }

            Console.WriteLine();
            Console.Write("Choose ATM buy writing it's name: ");

            string chosenATM = Console.ReadLine().Trim();

            return chosenATM;
        }

        static Guid Login(ATMService atmService)
        {
            Console.WriteLine("Login");

            Console.Write("Enter Email: ");
            string email = Console.ReadLine();

            Console.Write("Enter Password: ");
            string password = GetMaskedPasswordInput();

            return atmService.Login(email, password);
        }

        static string GetMaskedPasswordInput()
        {
            string password = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                if (key.Key != ConsoleKey.Enter)
                {
                    if (key.Key == ConsoleKey.Backspace && password.Length == 0) continue;

                    if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                    {
                        password = password.Substring(0, password.Length - 1);
                        Console.Write("\b \b");
                    } 
                    else
                    {
                        password += key.KeyChar;
                        Console.Write("*");
                    }
                }
            }
            while (key.Key != ConsoleKey.Enter);

            Console.WriteLine();

            return password;
        }
    }
}
