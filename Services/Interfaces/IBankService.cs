using Common;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IBankService
    {
        string Withdraw(decimal amount, Guid clientId);

        string Transfer(Guid senderId, string recepientEmail, decimal amount);

        decimal GetBalance(Guid clientId);

        bool SufficientFunds(Guid clientId, decimal amount, TransactionType type);

        ClientStatus GetClientStatus(Guid clientId);

        void SetClientStatus(Guid clientId);

        ClientEntity GetClientById(Guid clientId);

        void SetProfitability(Guid clientId);

        List<ATMEntity> GetATMs();
    }
}
