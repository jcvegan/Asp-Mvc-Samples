using System.Web.Mvc;

namespace Jcvegan.Web.CustomPrincipal.Extensions.View {
    public abstract class CustomWebViewPage : WebViewPage {
        public new virtual Principal.CustomPrincipal User => base.User as Principal.CustomPrincipal;
    }

    public abstract class CustomWebViewPage<TModel> : WebViewPage<TModel> {
        public new virtual Principal.CustomPrincipal User => base.User as Principal.CustomPrincipal;
    }
}