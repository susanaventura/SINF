using Interop.GcpBE800;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore.Lib_Primavera
{
    public static class Util
    {

        public static bool checkCredentials()
        {
            return PriEngine.InitializeCompany(OnlineStore.Properties.Settings.Default.Company.Trim(), OnlineStore.Properties.Settings.Default.User.Trim(), OnlineStore.Properties.Settings.Default.Password.Trim());
        }



        #region Clients

        public static Lib_Primavera.Model.ErrorResponse setClientValues(GcpBECliente myCli, Model.Client cli, bool validCredentials)
        {
            Lib_Primavera.Model.ErrorResponse error = new Model.ErrorResponse();

            if(validCredentials){
                myCli.set_Cliente(cli.CodClient);
                myCli.set_Nome(cli.Name);
                myCli.set_Moeda(cli.Currency);
                myCli.set_Morada(cli.Address);
                myCli.set_Distrito("00");
                myCli.set_CodigoPostal(cli.Postal_Addr);
                myCli.set_LocalOperacao(cli.Op_Zone);
                myCli.set_LocalidadeCodigoPostal(cli.Local);
                myCli.set_B2BEnderecoMail(cli.Email);
                myCli.set_NumContribuinte(cli.Taxpayer_num);
                myCli.set_PessoaSingular(true);
                myCli.set_TipoTerceiro("001");

                PriEngine.Engine.Comercial.Clientes.Actualiza(myCli);

                error.Error = 0;
                error.Description = "Sucess";
                return error;
            }
            else
            {
                error.Error = 1;
                error.Description = "Wrong credentials";
                return error;

            }
        }

        #endregion


        #region ErrorResponse

        public static Lib_Primavera.Model.ErrorResponse Success()
        {
            Lib_Primavera.Model.ErrorResponse error = new Model.ErrorResponse();
            error.Error = 0;
            error.Description = Lib_Primavera.Model.ErrorResponse.SUCCESS;
            error.Detail = error.Description;
            return error;
        }

        public static Lib_Primavera.Model.ErrorResponse ErrorWrongCredentials()
        {
            Lib_Primavera.Model.ErrorResponse error = new Model.ErrorResponse();
            error.Error = 1;
            error.Description = Lib_Primavera.Model.ErrorResponse.WRONG_CREDENTIALS;
            error.Detail = error.Description;
            return error;
        }


        public static Lib_Primavera.Model.ErrorResponse ErrorClientNotFound()
        {
            Lib_Primavera.Model.ErrorResponse error = new Model.ErrorResponse();
            error.Error = 1;
            error.Description = Lib_Primavera.Model.ErrorResponse.CLIENT_NOT_FOUND;
            error.Detail = error.Description;
            return error;
        }

        public static Lib_Primavera.Model.ErrorResponse ErrorException(Exception ex)
        {
            Lib_Primavera.Model.ErrorResponse error = new Model.ErrorResponse();
            error.Error = 1;
            error.Description = ex.Message;
            error.Detail = ex.ToString();
            
            return error;
        }


        
        #endregion

        #region Orders
        public const string OBS_ONLINE_STORE = "Online Store";
        #endregion
    }
}