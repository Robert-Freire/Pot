namespace Pot.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ProjectUser
    {
        public ProjectUser()
        {
            this.Expenses = new HashSet<Expense>();
        }

        [Key]
        [Column(Order = 0)]
        public Guid UserId { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid ProjectId { get; set; }

        public virtual ICollection<Expense> Expenses { get; set; }

        public virtual Project Project { get; set; }

        public virtual User User { get; set; }
    }
}
