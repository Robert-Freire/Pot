namespace Pot.Data.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Expenses")]
    public class Expense
    {
        [Key]
        public Guid ExpenseId { get; set; }

        public Guid ProjectId { get; set; }

        public Guid UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }

        public decimal Amount { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] Version { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Fecha { get; set; }

        public virtual Project Project { get; set; }

        public virtual ProjectUser ProjectUser { get; set; }

        public virtual User User { get; set; }
    }
}
