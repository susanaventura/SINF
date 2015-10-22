using Interop.StdBE800;
using OnlineStore.Lib_Primavera;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace OnlineStore
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            /*
            if (Util.checkCredentials())
            {
                //Add fields to user table

               // StdBERegistoUtil objEmail= new StdBERegistoUtil();
                StdBEDefCamposUtil email = new StdBEDefCamposUtil();
                StdBECampos cmps = new StdBECampos();
                StdBECampo email = new StdBECampo();

                
                email.Nome = "CDU_Email";
                email.Tipo = EnumTipoCampo.tcNVarchar;
                email.Valor = "valor";
               
                cmps.Insere(email);
                objEmail.Insere(email);

                //objEmail.set_Campos(cmps);
                //objEmail.Insere
                
                PriEngine.Engine.Comercial.Clientes.DaDefCamposUtilContactos().Insere(email);

                //GcpBECliente objCli = new GcpBECliente();

                

            }*/
        }
    }
}