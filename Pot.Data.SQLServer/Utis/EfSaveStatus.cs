namespace Pot.Data.SQLServer.Utis
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.Validation;
    using System.Linq;

    using Pot.Data.Infraestructure;

    /// <summary>
    /// The ef save status.
    /// </summary>
    public class EfSaveStatus : SaveStatus
    {
        /// <summary>
        /// This converts the Entity framework errors into Validation errors
        /// </summary>
        /// <param name="errorsList">
        /// The errors List.
        /// </param>
        /// <returns>
        /// The <see cref="SaveStatus"/>.
        /// </returns>
        public SaveStatus SetErrors(IEnumerable<DbEntityValidationResult> errorsList)
        {
            this.Errors =
                errorsList.SelectMany(
                    x => x.ValidationErrors.Select(y => new ValidationResult(y.ErrorMessage, new[] { y.PropertyName }))).ToList();
            return this;
        }
    }
}