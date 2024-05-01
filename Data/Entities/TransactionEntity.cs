using Common;
using Data.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class TransactionEntity : BaseEntity
    {
        public Guid SenderId { get; set; }

        public ClientEntity Sender { get; set; }

        public Guid ReceiverId { get; set; }

        public ClientEntity Receiver { get; set; }

        public TransactionType Type { get; set; }

        public decimal Amount { get; set; }

        public decimal Fee { get; set; }

        public DateTime Date { get; set; }
    }
}
