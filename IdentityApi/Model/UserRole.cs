namespace IdentityApi.Model
{

    public class UserRole
    {

        public int? Id { get; set; }

        public User? User { get; set; }

        public Client? Client { get; set; }

        public Role? Role { get; set; }

    }

}
