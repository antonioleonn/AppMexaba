using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

public class SessionAuthorizeAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var controller = context.RouteData.Values["controller"]?.ToString() ?? "";
        var action = context.RouteData.Values["action"]?.ToString() ?? "";

        // Permitir libre acceso al Login
        if (controller.Equals("Login", StringComparison.OrdinalIgnoreCase))
        {
            base.OnActionExecuting(context);
            return;
        }

        // Verifica si existe perfil en sesión
        var perfil = context.HttpContext.Session.GetInt32("Perfil");

        if (perfil == null)
        {
            context.Result = new RedirectToRouteResult(
                new RouteValueDictionary(new { controller = "Login", action = "Login" })
            );
            return;
        }

        base.OnActionExecuting(context);
    }
}
