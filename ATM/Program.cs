using Data;
using Services;
using Services.DI;
using Services.Interfaces;
using System.Data;

namespace ATM
{
    internal class Program
    {

        static void Main(string[] args)
        {
            RegisterDIServices();

            Engine engine = new Engine();
            engine.Run();  
        }

        static void RegisterDIServices()
        {
            ServiceContainer container = ServiceContainer.GetInstance();

            container.AddTransient<ATMContext>();
            container.AddTransient<ISeeder, Seeder>();
            container.AddTransient<IAuthenticationService, AuthenticationService>();
            container.AddTransient<IBankService, BankService>();
            container.AddTransient<ATMService>();
        }
    }
}