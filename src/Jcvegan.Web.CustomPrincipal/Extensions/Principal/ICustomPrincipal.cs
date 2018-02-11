using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.WebPages.Scope;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Jcvegan.Web.CustomPrincipal.Extensions.Principal {
    public interface ICustomPrincipal : IPrincipal {
        string Username { get; }
        string Email { get; }
        string FirstName { get; }
        string LastName { get; }
        string Country { get; }
        string City { get; }
        string ZipCode { get; }
        string Address { get; }
    }

    public class CustomPrincipal : ClaimsPrincipal, ICustomPrincipal {
        private readonly ApplicationUserManager _userManager = null;

        public CustomPrincipal(ClaimsIdentity identity,string username): base() {
            _userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var applicationUser = _userManager.FindByEmail(username);
            this.Identity = identity;
            if (applicationUser != null) {
                Username = applicationUser.UserName;
                Email = applicationUser.Email;
                FirstName = applicationUser.FirstName;
                LastName = applicationUser.LastName;
                Country = applicationUser.Country;
                City = applicationUser.City;
                ZipCode = applicationUser.ZipCode;
                Address = applicationUser.Address;
            }
        }

        public bool IsInRole(string role) {
            return _userManager.IsInRole(Username, role);
        }

        public IIdentity Identity { get; private set; }
        public string Username { get; private set; }
        public string Email { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Country { get; private set; }
        public string City { get; private set; }
        public string ZipCode { get; private set; }
        public string Address { get; private set; }
    }
}