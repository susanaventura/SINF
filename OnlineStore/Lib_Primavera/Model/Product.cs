using Interop.StdBE800;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore.Lib_Primavera.Model
{
    public class Product
    {
        public String codProduct { get; set; }
        public String description { get; set; }
        public String main_image { get; set; }
        public String[] images { get; set; }
        public String price { get; set; }
        public String currency { get; set; }
        public String unit { get; set; }
        public String category { get; set; }
        //public String availability { get; set; }
        //public int points { get; set; }

        public Product() { }
        public Product(StdBELista objArtigo, bool fetchImages)
        {
            this.codProduct = objArtigo.Valor("Artigo");
            this.description = objArtigo.Valor("Descricao");

            if (fetchImages) {
                // Main Image
                StdBELista objAnexo = PriEngine.Engine.Consulta("Select top 1 Id From Anexos Where Anexos.chave='" + codProduct + "' AND Anexos.Tabela=4 AND Anexos.Tipo='IPR'");
                if (!objAnexo.Vazia()) this.main_image = objAnexo.Valor("Id"); else this.main_image = "null";

                // Aux images
                objAnexo = PriEngine.Engine.Consulta("Select Id From Anexos Where Anexos.chave='" + codProduct + "' AND Anexos.Tabela=4 AND Anexos.Tipo='IMG'");
                this.images = new String[objAnexo.NumLinhas()];
                for (int i = 0; !objAnexo.NoFim(); objAnexo.Seguinte()) this.images[i++] = objAnexo.Valor("Id");
            }
            else
            {
                this.main_image = objArtigo.Valor("Id");
                this.images = new String[] {};
            }

            this.price = ((double)objArtigo.Valor("PVP1")).ToString();
            this.currency = objArtigo.Valor("Moeda");
            this.unit = objArtigo.Valor("UnidadeBase");
            this.category = objArtigo.Valor("Familia");
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