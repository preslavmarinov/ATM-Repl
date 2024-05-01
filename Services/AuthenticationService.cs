using Data;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ATMContext context;
        public AuthenticationService(ATMContext context)
        {
            this.context = context;
        }

        public Guid Login(string email, string password)
        {
            var client = context.Clients.FirstOrDefault(client => client.Email == email);

            if (client == null)
            {
                return Guid.Empty;
            }

            if (BCrypt.Net.BCrypt.Verify(password, client.Password)) return client.Id;
            else return Guid.Empty;
        }
    }
}
