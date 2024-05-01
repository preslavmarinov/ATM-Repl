using Common;
using Data.Entities;
using Services.Interfaces;
using Services.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ATMService
    {
        private readonly IBankService bankService;

        private readonly IAuthenticationService authenticationService;

        public ATMService(IBankService bankService, IAuthenticationService authenticationService)
        {
            this.bankService = bankService;
            this.authenticationService = authenticationService;
        }


        public void DelegateCommand(string input, Guid clientId)
        {
            var validator = CommandValidator.IsValid(input);

            if(!validator.Item1)
            {
                if (validator.Item2 == "") return;

                Console.WriteLine(validator.Item2);
                return;
            }

            string[] commands = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            decimal amount = -1;
            string message;

            switch (commands[0])
            {
                case "withdraw":
                    amount = decimal.Parse(commands[1]);
                    message = bankService.Withdraw(amount, clientId);
                    Console.WriteLine(message);
                    break;
                case "transfer":
                    amount = decimal.Parse(commands[1]);
                    message = bankService.Transfer(clientId, commands[2], amount);
                    Console.WriteLine(message);
                    break;
                case "balance":
                    decimal balance = bankService.GetBalance(clientId);
                    ClientStatus status =  bankService.GetClientStatus(clientId);
                    Console.WriteLine($"Balance: {balance}, Status: {status}");
                    break;
                case "help":
                    HelpCommad();
                    break;
                default:
                    return;
            }
        }

        public Guid Login(string email, string password)
        {
            Guid id = authenticationService.Login(email, password);

            if (id != Guid.Empty) 
            {
                bankService.SetClientStatus(id);
                bankService.SetProfitability(id);
                DisplayAvailableCommands();
            }
            
            return id;
        }

        public List<ATMEntity> GetATMs()
        {
            return bankService.GetATMs();
        }

        static void DisplayAvailableCommands()
        {
            Console.WriteLine("withdraw - Withdraw funds from account");
            Console.WriteLine("transfer - Transfer funds from your account to another");
            Console.WriteLine("balance - Get balance details and recent transaction");
            Console.WriteLine("help - Get Detailed explanation how each command works");
            Console.WriteLine("exit - Logout of your account and stop the application");
            Console.WriteLine();
        }

        static void HelpCommad()
        {
            Console.WriteLine("withdraw => withdraw <amount>");
            Console.WriteLine("  --  <amount> must be a number grater than 0.");
            Console.WriteLine();
            Console.WriteLine("transfer - transfer <amount> <receiver_email>");
            Console.WriteLine("  --  <amount> must be a number grater than 0.");
            Console.WriteLine("  --  <receiver_email> must be a valid email of a user.");
            Console.WriteLine();
            Console.WriteLine("balance - balance");
            Console.WriteLine();
            Console.WriteLine("help - help");
            Console.WriteLine();
            Console.WriteLine("exit - exit");
            Console.WriteLine();
        }
    }
}
