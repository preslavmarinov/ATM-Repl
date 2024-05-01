using Common;
using Data.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class ClientEntity : BaseEntity
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public decimal Balance { get; set; }

        public DateTime LastLogin { get; set; }

        public ClientStatus Status { get; set; }

        public ICollection<TransactionEntity>? TransactionsSent { get; set; }

        public ICollection<TransactionEntity>? TransactionsReceived { get; set; }

    }
}
