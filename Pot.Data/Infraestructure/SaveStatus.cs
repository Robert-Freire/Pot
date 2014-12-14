namespace Pot.Data.Infraestructure
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class SaveStatus
    {
        private List<ValidationResult> errors;

        public int UpdatedEntitiesNumber { get; set; }

        public bool IsValid
        {
            get
            {
                return this.errors == null || !this.errors.Any();
            }
        }

        public IList<ValidationResult> Errors
        {
            get
            {
                return this.errors ?? new List<ValidationResult>();
            }

            protected set
            {
                this.errors = value.ToList();
            }
        }

        public SaveStatus SetErrors(IEnumerable<ValidationResult> errorList)
        {
            this.errors = errorList.ToList();
            return this;
        }
    }
}