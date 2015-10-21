using Interop.GcpBE800;
using Interop.StdBE800;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore.Lib_Primavera
{
    public class PriIntegration
    {
        
        //START CLIENT
        #region Client

        public static Lib_Primavera.Model.Client GetClient(string codClient)
        {


            GcpBECliente objCli = new GcpBECliente();


            Model.Client myCli = new Model.Client();

            if (Util.checkCredentials())
            {

                if (PriEngine.Engine.Comercial.Clientes.Existe(codClient) == true)
                {
                    objCli = PriEngine.Engine.Comercial.Clientes.Edita(codClient);
                    myCli.codClient = objCli.get_Cliente();
                    myCli.name = objCli.get_Nome();
                    myCli.currency = objCli.get_Moeda();
                    myCli.email = objCli.get_EnderecoWeb(); //todo
                    myCli.taxpayer_num = objCli.get_NumContribuinte();
                    myCli.billing_address = objCli.get_Morada();
                    myCli.delivery_address = objCli.get_Morada();

                    return myCli;
                }
                else
                {
                    return null;
                }
            }
            else
                return null;
        }


        public static Lib_Primavera.Model.ErrorResponse CreateClient(Model.Client cli)
        {

            Lib_Primavera.Model.ErrorResponse error = new Model.ErrorResponse();


            GcpBECliente myCli = new GcpBECliente();

            try
            {
                if (Util.checkCredentials())
                {

                    myCli.set_Cliente(cli.codClient);
                    myCli.set_Nome(cli.name);
                    myCli.set_NumContribuinte(cli.taxpayer_num);
                    myCli.set_Moeda(cli.currency);
                    myCli.set_Morada(cli.delivery_address);
                    myCli.set_Morada2(cli.billing_address);

                    PriEngine.Engine.Comercial.Clientes.Actualiza(myCli);

                    error.Error = 0;
                    error.Description = "Success";
                    return error;
                }
                else
                {
                    error.Error = 1;
                    error.Description = "Wrong credentials";
                    return error;
                }
            }

            catch (Exception ex)
            {
                error.Error = 1;
                error.Description = ex.Message;
                return error;
            }


        }


        #endregion Client; //END CLIENT
    }
}