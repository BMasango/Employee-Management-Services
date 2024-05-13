using EMS.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Runtime.Serialization;

namespace EMS.Core.Domain.Models.Request
{
    public class Registration
    {
        /// <summary>
        /// User's first name
        /// </summary>
        [DataMember]
        [Required(ErrorMessage = "Firstname is a required field.")]
        public string FirstName { get; set; }

        /// <summary>
        /// User's last name or surname
        /// </summary>
        [DataMember]
        [Required(ErrorMessage = "Lastname is a required field.")]
        public string LastName { get; set; }

        /// <summary>
        /// User's line managers email
        /// </summary>
        [DataMember]
        [Required(ErrorMessage = "Line manager's email is a required field.")]
        public string LineManagersEmail { get; set; }

        /// <summary>
        /// User's age
        /// </summary>
        [DataMember]
        [Required(ErrorMessage = "Age is a required field.")]
        public int Age { get; set; }

        /// <summary>
        /// User's email address
        /// </summary>
        [DataMember]
        [EmailAddress]
        [Required(ErrorMessage = "Email is a required field.")]
        public string Email { get; set; }

        /// <summary>
        /// User's role type
        /// </summary>
        [DataMember]
        [Required(ErrorMessage = "Role is a required field.")]
        public string Role { get; set; }

        /// <summary>
        /// User's password
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
