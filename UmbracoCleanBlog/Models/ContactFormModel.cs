using System.ComponentModel.DataAnnotations;

namespace UmbracoCleanBlog.Models
{
    public class ContactFormModel
    {
        [Required(ErrorMessage = "Name is Required")]
        [MinLength(3, ErrorMessage = "Min Length of User Name is 3 Chars.")]
        [MaxLength(20, ErrorMessage = "Max Length of User Name is 20 Chars.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Email is invalid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Subject is Required")]
        [MinLength(3, ErrorMessage = "Min Length of Subject is 3 Chars.")]
        [MaxLength(20, ErrorMessage = "Max Length of Subject is 20 Chars.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Message is Required")]
        [MinLength(5, ErrorMessage = "Min Length of Message is 5 Chars.")]
        [MaxLength(300, ErrorMessage = "Max Length of Message is 300 Chars.")]
        public string Message { get; set; }
    }
}
