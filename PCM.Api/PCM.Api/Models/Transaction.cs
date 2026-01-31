﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCM.Api.Models
{
    [Table("123_Transactions")]
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        public string? Description { get; set; }


        public int CategoryId { get; set; }
        public TransactionCategory Category { get; set; } = null!;


        public int? CreatedById { get; set; }
        public Member? CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
