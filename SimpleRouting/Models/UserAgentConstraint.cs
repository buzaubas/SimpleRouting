namespace SimpleRouting.Models
{
    public class UserAgentConstraint : IRouteConstraint
    {
        private string requiredUserAgent;
        public UserAgentConstraint(string requiredUserAgent)
        {
            this.requiredUserAgent = requiredUserAgent;
        }
    
        public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return httpContext.Request.Headers["UserAgent"].ToString() != null 
                && httpContext.Request.Headers["UserAgent"].ToString().Contains(requiredUserAgent); 
        }
    }
}
