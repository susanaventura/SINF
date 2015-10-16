using Interop.GcpBE800;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore.Lib_Primavera
{
    public class PriIntegration
    {
        /*** Clients ***/

        public static Lib_Primavera.Model.Client GetCliente(string codCliente)
        {


            GcpBECliente objCli = new GcpBECliente();


            Model.Client myCli = new Model.Client();

            if (PriEngine.InitializeCompany(OnlineStore.Properties.Settings.Default.Company.Trim(), OnlineStore.Properties.Settings.Default.User.Trim(), OnlineStore.Properties.Settings.Default.Password.Trim()) == true)
            {

                if (PriEngine.Engine.Comercial.Clientes.Existe(codCliente) == true)
                {
                    objCli = PriEngine.Engine.Comercial.Clientes.Edita(codCliente);
                    myCli.id = objCli.get_Cliente();
                    myCli.name = objCli.get_Nome();
                    myCli.currency = objCli.get_Moeda();
                    myCli.taxpayer_num = objCli.get_NumContribuinte();
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



    }
}