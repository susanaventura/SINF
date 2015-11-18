using Interop.GcpBE800;
using Interop.StdBE800;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore.Lib_Primavera.Model
{
    public class Client
    {
        public String CodClient
        {
            get;
            set;

        }
        public String Name { get; set; }
        public String Email { get; set; }
        public String Address { get; set; } //morada
        public String Taxpayer_num { get; set; }
        public String Currency { get; set; }
        public String Postal_Addr { get; set; }
        public String Op_Zone { get; set; }
        public String Local { get; set; }

        public Client() { }
        public Client(StdBELista objClient)
        {

            this.CodClient = objClient.Valor("Cliente");// get_Cliente();
            this.Name = objClient.Valor("Nome");// get_Nome();
            this.Currency = objClient.Valor("Moeda");// get_Moeda();
            this.Email = objClient.Valor("B2BEnderecoMail");// get_B2BEnderecoMail();
            this.Taxpayer_num = objClient.Valor("NumContrib");// get_NumContribuinte();
            this.Address = objClient.Valor("Fac_Mor");// get_Morada();
            this.Local = objClient.Valor("Fac_Cploc");// get_LocalidadeCodigoPostal();
            this.Postal_Addr = objClient.Valor("Fac_Cp");// get_CodigoPostal();
            this.Op_Zone = objClient.Valor("LocalOperacao");// get_LocalOperacao();
        }

        public static String GetQuery(String codClient)
        {
            String query = "";
            String cols = "Cliente, Nome, Moeda, \"B2BEnderecoMail\", NumContrib, Fac_Mor, Fac_Cploc, Fac_Cp, LocalOperacao";


            query = "SELECT " + cols + " FROM Clientes WHERE Cliente = '" + codClient + "';";

            return query;
        }

    }
}