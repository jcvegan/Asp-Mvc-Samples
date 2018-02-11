using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jcvegan.Web.CustomPrincipal.Controllers {
    public abstract class CustomBaseController : Controller {
        public new virtual Extensions.Principal.CustomPrincipal User => (Extensions.Principal.CustomPrincipal) HttpContext.User;
    }
}