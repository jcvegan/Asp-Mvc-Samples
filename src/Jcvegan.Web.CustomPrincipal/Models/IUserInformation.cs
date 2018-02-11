namespace Jcvegan.Web.CustomPrincipal.Models {
    public interface IUserInformation {
        string FirstName { get; set; }
        string LastName { get; set; }
        string Country { get; set; }
        string City { get; set; }
        string ZipCode { get; set; }
        string Address { get; set; }
    }
}