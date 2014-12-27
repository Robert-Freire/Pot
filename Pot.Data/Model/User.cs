namespace Pot.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.AspNet.Identity.EntityFramework;

    /// <summary>
    /// The user.
    /// </summary>
    public class User: IdentityUser
    {
        public User()
        {
            this.Expenses = new HashSet<Expense>();
            this.ProjectUsers = new HashSet<ProjectUser>();
        }

        [Key]
        public Guid UserId { get; set; }

        //[Required]
        //[StringLength(250)]
        //public string Name
        //{
        //    get
        //    {
        //        return UserName;
        //    }

        //    set
        //    {
        //        UserName = value;
        //    }
        //}

        //[Required]
        //[StringLength(250)]
        //public string Mail { get; set; }

        //[Column(TypeName = "timestamp")]
        //[MaxLength(8)]
        //[Timestamp]
        //public byte[] Version { get; set; }

        public virtual ICollection<Expense> Expenses { get; set; }

        public virtual ICollection<ProjectUser> ProjectUsers { get; set; }
    }
}
