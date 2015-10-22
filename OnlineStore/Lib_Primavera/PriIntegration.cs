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


        //START PRODUCT
        #region Product

    
        public static Lib_Primavera.Model.Product GetProduct(string codProduct)
        {

            GcpBEArtigo objArtigo = new GcpBEArtigo();
            Model.Product myProd = new Model.Product();

            if (PriEngine.InitializeCompany(OnlineStore.Properties.Settings.Default.Company.Trim(), OnlineStore.Properties.Settings.Default.User.Trim(), OnlineStore.Properties.Settings.Default.Password.Trim()) == true)
            {
                if (PriEngine.Engine.Comercial.Artigos.Existe(codProduct))
                {
                    objArtigo = PriEngine.Engine.Comercial.Artigos.Edita(codProduct);
                    myProd.codProduct = objArtigo.get_Artigo();
                    myProd.description = objArtigo.get_Descricao();
                    myProd.main_image = "todo";
                    myProd.images = new String[] {"todo1", "todo2", "todo3"};
                    myProd.price = "todo";
                    myProd.unit = objArtigo.get_UnidadeBase();
                    myProd.points = 0;

                    return myProd;
                }
                else return null;
            }
            else return null;
        }

        public static List<Model.Product> ListProducts(int indexStart = 0, int indexEnd = 0)
        {

            StdBELista objList;
            Model.Product myProd;
            List<Model.Product> listArts = new List<Model.Product>();

            if (PriEngine.InitializeCompany(OnlineStore.Properties.Settings.Default.Company.Trim(), OnlineStore.Properties.Settings.Default.User.Trim(), OnlineStore.Properties.Settings.Default.Password.Trim()))
            {
                objList = PriEngine.Engine.Consulta("SELECT artigo, descricao From Artigo");

                // iterate to the correct index
                for (int i = 0; i < indexStart && !objList.NoFim(); i++) objList.Seguinte();

                // fetch a page of elements
                for (int i = indexStart; i < indexEnd && !objList.NoFim(); i++)
                {
                    myProd = new Model.Product();
                    myProd.codProduct = objList.Valor("artigo");
                    myProd.description = objList.Valor("descricao");
                    myProd.main_image = "todo";
                    myProd.images = new String[] { "todo1", "todo2", "todo3" };
                    myProd.price = "todo";
                    //myProd.unit = objList.Valor("unidade base");
                    myProd.points = 0;

                    listArts.Add(myProd);
                    objList.Seguinte();
                }

                return listArts;
            }
            else return null;
        }

        #endregion Product; //END PRODUCT
    }
}