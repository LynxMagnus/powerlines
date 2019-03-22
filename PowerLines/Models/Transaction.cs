using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PowerLines.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }

        public int BankId { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime Date { get; set; }

        public string Description { get; set; }
        
        public decimal Value { get; set; }

        public Transaction()
        {
        }

        public Transaction(int bankId)
        {
            BankId = bankId;
        }

        public Transaction(int bankId, DateTime date, string description, decimal value):this(bankId)
        {
            Date = date;
            Description = description;
            Value = value;
        }
    }
}