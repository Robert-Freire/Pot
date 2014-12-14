namespace Pot.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// The user.
    /// </summary>
    public class User
    {
        public User()
        {
            this.Expenses = new HashSet<Expense>();
            this.ProjectUsers = new HashSet<ProjectUser>();
        }

        [Key]
        public Guid UserId { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [Required]
        [StringLength(250)]
        public string Mail { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] Version { get; set; }

        public virtual ICollection<Expense> Expenses { get; set; }

        public virtual ICollection<ProjectUser> ProjectUsers { get; set; }
    }
}
