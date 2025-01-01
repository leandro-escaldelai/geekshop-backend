namespace IdentityApi.Model
{

    public class User
    {

        public int? Id { get; set; }
	    
        public string? Name { get; set; }
        
        public string? Username { get; set; }
        
        public string? Password { get; set; }
        
        public bool IsActive { get; set; }

        public IEnumerable<UserRole> Roles { get; set; } = new List<UserRole>();

    }

}
