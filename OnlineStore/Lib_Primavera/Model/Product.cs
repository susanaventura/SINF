using Interop.StdBE800;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore.Lib_Primavera.Model
{
    public class Product
    {
        public String CodProduct { get; set; }
        public String Description { get; set; }
        public String Main_image { get; set; }
        public String[] Images { get; set; }
        public String Price { get; set; }
        public String Currency { get; set; }
        public String Unit { get; set; }
        public String Category { get; set; }
        //public String Availability { get; set; }
        //public int Points { get; set; }

        public Product() { }
        public Product(StdBELista objArtigo, bool fetchImages)
        {
            this.CodProduct = objArtigo.Valor("Artigo");
            this.Description = objArtigo.Valor("Descricao");

            if (fetchImages) {
                // Main Image
                StdBELista objAnexo = PriEngine.Engine.Consulta("Select top 1 Id From Anexos Where Anexos.chave='" + CodProduct + "' AND Anexos.Tabela=4 AND Anexos.Tipo='IPR'");
                if (!objAnexo.Vazia()) this.Main_image = objAnexo.Valor("Id"); else this.Main_image = "null";

                // Aux images
                objAnexo = PriEngine.Engine.Consulta("Select Id From Anexos Where Anexos.chave='" + CodProduct + "' AND Anexos.Tabela=4 AND Anexos.Tipo='IMG'");
                this.Images = new String[objAnexo.NumLinhas()];
                for (int i = 0; !objAnexo.NoFim(); objAnexo.Seguinte()) this.Images[i++] = objAnexo.Valor("Id");
            }
            else
            {
                this.Main_image = objArtigo.Valor("Id");
                this.Images = new String[] {};
            }

            this.Price = ((double)objArtigo.Valor("PVP1")).ToString();
            this.Currency = objArtigo.Valor("Moeda");
            this.Unit = objArtigo.Valor("UnidadeBase");
            this.Category = objArtigo.Valor("Familia");
            //myProd.points = 0;

        }

        public static String GetQuery(int offset=0, int limit=1, bool getImage=false, string codProduct="", string codCategory="", string codStore="") {
            String query = "";
            String cols = "Artigo.Artigo, Artigo.Descricao, Artigo.UnidadeBase, Artigo.Familia, ArtigoMoeda.PVP1, ArtigoMoeda.Moeda";
            String outcols = "Artigo, Descricao, UnidadeBase, Familia, PVP1, Moeda";
            if (getImage) { cols += ", Anexos.Id"; outcols += ", Id"; }
          

            query = "SELECT " + outcols + " FROM (";
            
                // Select Cols
                query += "SELECT "+cols+", ROW_NUMBER() OVER (ORDER BY Artigo.Artigo) AS RowNum ";

                // Join Tables
                query += "FROM Artigo ";
                query += "JOIN ArtigoMoeda ON Artigo.Artigo = ArtigoMoeda.Artigo ";
                if (getImage) query += "LEFT JOIN Anexos ON Artigo.Artigo = Anexos.Chave AND Anexos.Tabela=4 AND Anexos.Tipo='IPR' ";

                // Conditions
                query += "WHERE (1=1) ";
                if (codProduct != "") query += "AND Artigo.Artigo='" + codProduct + "' ";
                if (codCategory != "") query += "AND Artigo.Familia='" + codCategory + "' ";
                if (codStore != "") query += "AND Artigo.Artigo IN (SELECT Artigo FROM ArtigoArmazem WHERE Armazem='" + codStore + "') ";

            
           query += ") AS MyDerivedTable WHERE MyDerivedTable.RowNum BETWEEN " + (offset+1) + " AND " + (offset+limit);
                

            return query;
        
        }
    }
}