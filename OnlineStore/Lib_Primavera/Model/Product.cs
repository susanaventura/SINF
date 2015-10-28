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
        public double Price { get; set; }
        public String Currency { get; set; }
        public String Unit { get; set; }
        public String Category { get; set; }
        public float Discount { get; set; }
        public double Points { get; set; }

        public Product() { }
        public Product(StdBELista objArtigo)
        {
            this.CodProduct = objArtigo.Valor("Artigo");
            this.Description = objArtigo.Valor("Descricao");

            this.Price = objArtigo.Valor("PVP1");
            this.Points = objArtigo.Valor("PVP6"); if (this.Points <= 0) this.Points = -1;
            this.Discount = objArtigo.Valor("Desconto");
            this.Currency = objArtigo.Valor("Moeda");
            this.Unit = objArtigo.Valor("UnidadeBase");
            this.Category = objArtigo.Valor("Familia");

        }

        public static String GetQuery(int offset=0, int limit=1, string codProduct="", string codCategory="", string codStore="", bool filterOnSale=false, bool filterPoints=false) {
            String query = "";
            String cols = "Artigo.Artigo, Artigo.Descricao, Artigo.UnidadeBase, Artigo.Familia, Artigo.Desconto, ArtigoMoeda.PVP1, ArtigoMoeda.PVP6, ArtigoMoeda.Moeda";
            String outcols = "Artigo, Descricao, UnidadeBase, Familia, Desconto, PVP1, PVP6, Moeda";
          

            query = "SELECT " + outcols + " FROM (";
            
                // Select Cols
                query += "SELECT "+cols+", ROW_NUMBER() OVER (ORDER BY Artigo.Artigo) AS RowNum ";

                // Join Tables
                query += "FROM Artigo ";
                query += "JOIN ArtigoMoeda ON Artigo.Artigo = ArtigoMoeda.Artigo ";

                // Conditions
                query += "WHERE (1=1) ";
                if (codProduct != "") query += "AND Artigo.Artigo='" + codProduct + "' ";
                if (codCategory != "") query += "AND Artigo.Familia='" + codCategory + "' ";
                if (codStore != "") query += "AND Artigo.Artigo IN (SELECT Artigo FROM ArtigoArmazem WHERE Armazem='" + codStore + "') ";
                if (filterPoints) query += "AND ArtigoMoeda.PVP6 > 0 ";
                if (filterOnSale) query += "AND Artigo.Desconto > 0 ";

            
           query += ") AS MyDerivedTable WHERE MyDerivedTable.RowNum BETWEEN " + (offset+1) + " AND " + (offset+limit);
                
           return query;        
        }
    }
}