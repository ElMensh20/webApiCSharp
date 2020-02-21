using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Text;
using System;
using System.Threading;
using System.Security.Principal;

namespace EmployeeServies
{
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            else
            {
                string autherizationToken = actionContext.Request.Headers.Authorization.Parameter;
                string decodedAutherizationToken = Encoding.Default.GetString(Convert.FromBase64String(autherizationToken));
                string[] userNamePasswordArray = decodedAutherizationToken.Split(':');
                string username = userNamePasswordArray[0];
                string password = userNamePasswordArray[1];
                if (EmployeeSecurity.login(username, password))
                {
                    String[] myStringArray = { "Manager", "Teller" };
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username), myStringArray);
                }
                else
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
        }
    }
}