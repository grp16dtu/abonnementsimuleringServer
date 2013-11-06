using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Providers.Entities;

namespace AbonnementsimuleringServer.Autorisation
{
    public class BasicAuthenticationAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                throw new Exception();
            }

            else
            {
                string authToken = actionContext.Request.Headers.Authorization.Parameter;
                string decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));
                string[] tokens = decodedToken.Split(',', ':');
                int aftalenummer;
                string brugernavn = "";
                string kodeord = "";

                if (tokens.Length == 3)
                {
                    aftalenummer = Convert.ToInt16(tokens[0]);
                    brugernavn = tokens[1];
                    kodeord = tokens[2];

                    User bruger = new User();
                    bruger.UserName = aftalenummer + ":" + brugernavn;

                    byte[] bytes = new byte[16];
                    BitConverter.GetBytes(aftalenummer).CopyTo(bytes, 0);
                    bruger.UserId= new Guid(bytes);

                    HttpContext.Current.User = new GenericPrincipal(new ApiIdentity(bruger), new string[] { });
                    base.OnActionExecuting(actionContext);
                }

                else
                    actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }

            
            


        }
    }

}