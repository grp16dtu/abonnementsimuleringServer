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
    public class BasicAuthenticationAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                //throw new Exception();
            }

            else
            {
                try
                {
                    string authToken = actionContext.Request.Headers.Authorization.Parameter;
                    string decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));
                    string[] tokens = decodedToken.Split(',', ':');
                    int? aftalenummer;
                    string brugernavn = "";
                    string kodeord = "";

                    if (tokens.Length == 2)
                    {
                        MySQL mySql = new MySQL();
                        brugernavn = tokens[0];
                        kodeord = tokens[1];
                        aftalenummer = mySql.HentEconomicAftalenummer(brugernavn, kodeord);

                        Debug.WriteLine("Bruger: {0}, Kodeord: {1}",brugernavn, kodeord);
                        DataSet mysqlData = mySql.HentBruger(brugernavn, kodeord);


                        Debug.WriteLine("Test{0} ",mysqlData.Tables[0].Rows[0]["brugerFornavn"]);
                        

                        Bruger bruger = new Bruger(mysqlData);

                        

                        if (aftalenummer != null)
                        {
                            User webApiForespoerger = new User();
                            webApiForespoerger.UserName = aftalenummer + ":" + brugernavn;

                            Debug.WriteLine("Bruger: {0}, Kode: {1}, Aft: {2}",brugernavn,kodeord,aftalenummer);

                            string[] roller;
                            if (bruger.Ansvarlig)
                                roller = new string[]{"Ansvarlig, Bruger"};
                            else
                                roller = new string[] { "Bruger" };

                            HttpContext.Current.User = new GenericPrincipal(new ApiIdentity(webApiForespoerger), roller);
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
                    Debug.WriteLine("Fejl ved login: " + e);
                    actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                }
            }
        }
    }

}