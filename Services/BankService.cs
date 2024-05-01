using Common;
using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class BankService : IBankService
    {
        private readonly ATMContext context;

        public BankService(ATMContext context)
        {
            this.context = context;
        }

        public string Transfer(Guid senderId, string recepientEmail, decimal amount)
        {
            if (!SufficientFunds(senderId, amount, TransactionType.Transfer)) return "Transfer failed - Insufficient funds";
            

            var sender = GetClientById(senderId);
            sender.Balance -= amount;

            var recepient = context.Clients.FirstOrDefault(x => x.Email == recepientEmail);

            if (recepient == null) return $"User with email: {recepientEmail} not found";

            if (senderId == recepient.Id) return "Cannot transfer funds to yourself";

            recepient.Balance += amount;

            context.Clients.Update(sender);
            context.Clients.Update(recepient);
            context.Transactions.Add(new TransactionEntity
            {
                SenderId = senderId,
                Sender = sender,
                ReceiverId = recepient.Id,
                Receiver = recepient,
                Type = TransactionType.Transfer,
                Amount = amount,
                Fee = 0,
                Date = DateTime.Now
            });
            
            context.SaveChanges();

            return $"Succesfully transfered {amount} to {recepientEmail}";
        }

        public string Withdraw(decimal amount, Guid clientId)
        { 
            if (!SufficientFunds(clientId, amount, TransactionType.Withdraw)) return "Withdrawal failed - Insufficient funds"; ;

            var client = GetClientById(clientId);

            if (client == null) return "Account not found";

            decimal fee = GetFee(amount);
            client.Balance -= amount + fee;

            context.Clients.Update(client);
            context.Transactions.Add(new TransactionEntity
            {
                SenderId = clientId,
                Sender = client,
                ReceiverId = clientId,
                Receiver = client,
                Type = TransactionType.Withdraw,
                Amount = amount,
                Fee = fee,
                Date = DateTime.Now
            });

            context.SaveChanges();

            return $"Succesfully withdrawed {amount}";
        }

        public decimal GetBalance(Guid clientId)
        {
            return GetClientById(clientId).Balance;
        }

        public bool SufficientFunds(Guid clientId, decimal amount, TransactionType type)
        {
            decimal balance = GetBalance(clientId);

            if (type == TransactionType.Transfer) return balance > amount;

            return balance > amount + GetFee(amount);
        }

        public ClientStatus GetClientStatus(Guid clientId)
        {
            return GetClientById(clientId).Status;
        }

        public void SetClientStatus(Guid clientId)
        {
            DateTime today = DateTime.Now;
            DateTime start = today.AddMonths(-1).AddDays(-today.Day + 1);
            DateTime end = today.AddDays(-today.Day);

            var withdrawalTransactions = context.Transactions.Where(t =>
                t.SenderId == clientId &&
                t.Type == TransactionType.Withdraw &&
                t.Date >= start && t.Date <= end
                ).ToList();

            var client = GetClientById(clientId);

            if (withdrawalTransactions.Count <= 10) client.Status = ClientStatus.Standard;
            else if (withdrawalTransactions.Count > 20) client.Status = ClientStatus.Platinum;
            else client.Status = ClientStatus.Premium;

            context.SaveChanges();
        }

        public ClientEntity GetClientById(Guid clientId)
        {
            return context.Clients.FirstOrDefault(t => t.Id == clientId);
        }

        public void SetProfitability(Guid clientId)
        {
            var client = GetClientById(clientId);
            DateTime today = DateTime.Now;

            int n = today.Month - client.LastLogin.Month;
            client.LastLogin = today;

            if (n == 0) 
            {
                context.Clients.Update(client);
                context.SaveChanges();

                return;
            }

            for(int i=0; i<n; i++)
            {
                decimal profitability = client.Balance * 0.01m;
                client.Balance += profitability;
            }

            context.Clients.Update(client);

            context.SaveChanges();
        }

        public List<ATMEntity> GetATMs()
        {
            return context.ATMs.ToList();
        }

        static decimal GetFee(decimal amount)
        {
            if (amount > 1000) return amount * 0.1m;
            else if (amount < 100) return amount * 0.03m;
            else return amount * 0.05m;
        }
    }
}
