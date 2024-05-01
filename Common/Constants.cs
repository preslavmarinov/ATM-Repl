using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class Constants
    {
        public const int MAX_LOGIN_ATTEMPTS = 3;

        public static readonly string[] COMMANDS = new string[] {"withdraw", "transfer", "balance", "help", "exit" };
    }
}
