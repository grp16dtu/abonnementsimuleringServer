using AbonnementsimuleringServer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Providers.Entities;

namespace AbonnementsimuleringServer.Autorisation
{
    public class BasicAuthAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null)
                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            
            else
            {
                try
                {
                    string authToken = actionContext.Request.Headers.Authorization.Parameter;
                    string decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));
                    string[] tokens = decodedToken.Split(':');
                    string brugernavn = "";
                    string kodeord = "";

                    if (tokens.Length == 2)
                    {
                        MySQL mySql = new MySQL();

                        brugernavn = tokens[0];
                        kodeord = tokens[1];

                        DataSet mySqlBrugerData = mySql.HentBruger(brugernavn, kodeord);
                        DataSet mySqlEconomicData = mySql.HentEconomicOplysninger(brugernavn, kodeord);

                        if (mySqlBrugerData != null && mySqlEconomicData != null)
                        {
                            Bruger bruger = new Bruger(mySqlBrugerData);
                            Konto konto = new Konto(mySqlEconomicData, bruger);
                            HttpContext.Current.User = new GenericPrincipal(new ApiIdentitet(konto), new string[] { });
                            base.OnActionExecuting(actionContext);
                        }

                        else
                        {
                            actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                        }
                    }

                    else
                    {
                        actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Aut. fejl: " + e.Message);
                    actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                }
            }
        }
    }

}