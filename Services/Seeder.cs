using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class Seeder : ISeeder
    {
        private readonly ATMContext context;

        public Seeder(ATMContext context)
        {
            this.context = context;
        }

        public void Seed()
        {

            if (this.context.Clients.Any())
            {
                return;
            }

            context.Clients.AddRange(
                new ClientEntity
                {
                    Name = "Ivan",
                    Email = "ivan@gmail.com",
                    Password = BCrypt.Net.BCrypt.HashString("ivan12345"),
                    Balance = 3000,
                    Status = ClientStatus.Standard,
                    LastLogin = DateTime.Now,
                },
                 new ClientEntity
                 {
                     Name = "Georgi",
                     Email = "georgi@gmail.com",
                     Password = BCrypt.Net.BCrypt.HashString("georgi12345"),
                     Balance = 3000,
                     Status = ClientStatus.Standard,
                     LastLogin = DateTime.Now,
                 },
                  new ClientEntity
                  {
                      Name = "Dragan",
                      Email = "dragan@gmail.com",
                      Password = BCrypt.Net.BCrypt.HashString("dragan12345"),
                      Balance = 3000,
                      Status = ClientStatus.Standard,
                      LastLogin = DateTime.Now,
                  },
                   new ClientEntity
                   {
                       Name = "Alex",
                       Email = "alex@gmail.com",
                       Password = BCrypt.Net.BCrypt.HashString("alex12345"),
                       Balance = 500,
                       Status = ClientStatus.Standard,
                       LastLogin = DateTime.Now,
                   }
            );

            if(context.ATMs.Any())
            {
                return;
            }

            context.ATMs.AddRange(
                new ATMEntity { Name = "ATM-1" },
                new ATMEntity { Name = "ATM-2" },
                new ATMEntity { Name = "ATM-3" },
                new ATMEntity { Name = "ATM-4" },
                new ATMEntity { Name = "ATM-5" }
            );

            context.SaveChanges();
        }
    }
}
