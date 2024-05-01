using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Validator
{
    public static class CommandValidator
    {
        public static (bool, string) IsValid(string input)
        {
            string[] commands = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            if (commands.Length == 0) return (false, "");

            switch(commands[0])
            {
                case "withdraw":
                    if (commands.Length != 2) return (false, "Invalid usage of 'withdraw' command");
                    if (!IsDecimal(commands[1])) return (false, "Amount should be a decimal number");
                    break;
                case "transfer":
                    if (commands.Length != 3) return (false, "Invalid usage of 'transfer' command");
                    if (!IsDecimal(commands[1])) return (false, "Amount should be a decimal number");
                    break;
                case "balance":
                    if (commands.Length != 1) return (false, "Invalid usage of 'balance' command");
                    break;
                case "help":
                    if (commands.Length != 1) return (false, "Invalid usage of 'help' command");
                    break;
                default:
                    return (false, "Command not recognised");
            }

            return (true, "");
        }

        static bool IsDecimal(string value) 
        {
            return decimal.TryParse(value, out var result);
        }
    }
}
