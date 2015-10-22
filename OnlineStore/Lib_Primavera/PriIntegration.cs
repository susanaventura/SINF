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
            Lib_Primavera.Model.ErrorResponse error = new Model.ErrorResponse();

            GcpBECliente objCli = new GcpBECliente();
            Model.Client myCli = new Model.Client();

            if (Util.checkCredentials())
            {

                if (PriEngine.Engine.Comercial.Clientes.Existe(codClient))
                {
                    objCli = PriEngine.Engine.Comercial.Clientes.Edita(codClient);
                    myCli.CodClient = objCli.get_Cliente();
                    myCli.Name = objCli.get_Nome();
                    myCli.Currency = objCli.get_Moeda();
                    myCli.Email = objCli.get_B2BEnderecoMail(); //todo
                    myCli.Taxpayer_num = objCli.get_NumContribuinte();
                    myCli.Billing_address = objCli.get_Morada();
                    myCli.Delivery_address = objCli.get_Morada();

                    return myCli;
                }
                else
                {
                    return null;
                }
            } 
                else
            {
                return null;
            }
        }

        public static Lib_Primavera.Model.ErrorResponse CreateClient(Model.Client cli)
        {

            Lib_Primavera.Model.ErrorResponse error = new Model.ErrorResponse();


            GcpBECliente myCli = new GcpBECliente();

            try
            {
                if (Util.checkCredentials())
                {

                    myCli.set_Cliente(cli.CodClient);
                    myCli.set_Nome(cli.Name);
                    myCli.set_NumContribuinte(cli.Taxpayer_num);
                    myCli.set_Moeda(cli.Currency);
                    myCli.set_Morada(cli.Delivery_address);
                    myCli.set_Morada2(cli.Billing_address);
                    myCli.set_B2BEnderecoMail(cli.Email);

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



        public static Lib_Primavera.Model.ErrorResponse UpdCliente(Lib_Primavera.Model.Client client)
        {
            Lib_Primavera.Model.ErrorResponse erro = new Model.ErrorResponse();


            GcpBECliente objCli = new GcpBECliente();

            try
            {

                if (Util.checkCredentials())
                {

                    if (PriEngine.Engine.Comercial.Clientes.Existe(client.CodClient) == false)
                    {
                        erro.Error = 1;
                        erro.Description = "The client does not exist";
                        return erro;
                    }
                    else
                    {

                        objCli = PriEngine.Engine.Comercial.Clientes.Edita(client.CodClient);
                        objCli.set_EmModoEdicao(true);

                        objCli.set_Nome(client.Name);
                        objCli.set_NumContribuinte(client.Taxpayer_num);
                        objCli.set_Moeda(client.Currency);
                        objCli.set_Morada(client.Delivery_address);
                        objCli.set_Morada2(client.Billing_address); //TODO onde colocar??
                        objCli.set_B2BEnderecoMail(client.Email);


                        PriEngine.Engine.Comercial.Clientes.Actualiza(objCli);

                        erro.Error = 0;
                        erro.Description = "Sucess";
                        return erro;
                    }
                }
                else
                {
                    erro.Error = 1;
                    erro.Description = "Wrong credentials";
                    return erro;

                }

            }

            catch (Exception ex)
            {
                erro.Error = 1;
                erro.Description = ex.Message;
                return erro;
            }

        }


        public static Lib_Primavera.Model.ErrorResponse DelCliente(string codCliente)
        {

            Lib_Primavera.Model.ErrorResponse error = new Model.ErrorResponse();
            GcpBECliente objCli = new GcpBECliente();


            try
            {

                if (Util.checkCredentials())
                {
                    if (!PriEngine.Engine.Comercial.Clientes.Existe(codCliente))
                    {
                        error.Error = 1;
                        error.Description = "The client does not exist";
                        return error;
                    }
                    else
                    {

                        PriEngine.Engine.Comercial.Clientes.Remove(codCliente);
                        error.Error = 0;
                        error.Description = "Client deleted";
                        return error;
                    }
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