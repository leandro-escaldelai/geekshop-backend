using Microsoft.AspNetCore.Authorization;

namespace ProductApi.Model
{

    public class Roles
    {

        public const string Admin = "admin";

        public const string Customer = "customer";

    }



    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {

        public AdminAuthorizeAttribute() : base(Model.Roles.Admin) { }

    }


}
