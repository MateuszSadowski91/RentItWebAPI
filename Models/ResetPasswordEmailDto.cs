namespace RentItAPI.Models
{
    public class ResetPasswordEmailDto
    {
        public string EmailAddress { get; set; }
        public string ResetToken { get; set; }
    }
}