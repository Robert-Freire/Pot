namespace Pot.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Project
    {
        public Project()
        {
            this.Expenses = new HashSet<Expense>();
            this.ProjectUsers = new HashSet<ProjectUser>();
        }

        public Guid ProjectId { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] Version { get; set; }

        public virtual ICollection<Expense> Expenses { get; set; }

        public virtual ICollection<ProjectUser> ProjectUsers { get; set; }
    }
}
