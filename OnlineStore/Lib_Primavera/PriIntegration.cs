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