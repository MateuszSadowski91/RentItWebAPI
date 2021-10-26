namespace RentItAPI.Models
{
    public class GrantAdminRoleDto
    {
        public string UserEmail { get; set; }
        public int GrantedRoleId { get; set; }
    }
}