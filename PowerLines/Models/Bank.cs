using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PowerLines.Models
{
    public class Bank
    {
        public int BankId { get; set; }

        public string UserId { get; set; }

        public virtual List<Transaction> Transactions { get; set; }

        public decimal Balance
        {
            get
            {
                return Transactions.Sum(x => x.Value);
            }
        }

        public Bank()
        {
            Transactions = new List<Transaction>();
        }

        public Bank(string userId) : this()
        {
            UserId = userId;
        }

        public void NewTransaction(Transaction transaction)
        {
            Transactions.Add(transaction);
        }
    }
}