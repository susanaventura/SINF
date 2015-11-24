﻿using Interop.StdBE800;
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
        //public float CurrentStock { get; set; }

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
            //if(objArtigo.Valor("STKActual") != null) this.CurrentStock = objArtigo.Valor("STKActual");

        }

        public class QueryParams {
            public int Offset {get; set;}
            public int Limit { get; set; }
            public string CodProduct { get; set; }
            public string CodCategory { get; set; }
            public string CodStore { get; set; }
            public bool FilterOnSale { get; set; }
            public bool FilterPoints { get; set; }
            public bool Count { get; set; }
            public bool SortDate { get; set; }
            public string SearchString { get; set; }
            public bool SortLastSold { get; set; }

            public QueryParams() {
                this.Offset = 0;
                this.Limit = 1;
                this.CodProduct = "";
                this.CodCategory = "";
                this.CodStore = "";
                this.FilterOnSale = false;
                this.FilterPoints = false;
                this.Count = false;
                this.SortDate = false;
                this.SearchString = "";
                this.SortLastSold = false;
            }
        }

        public static String GetQuery(QueryParams param)
        {
            String query = "";
            String cols = "Artigo.Artigo, Artigo.Descricao, Artigo.UnidadeBase, Artigo.Familia, Artigo.Desconto, ArtigoMoeda.PVP1, ArtigoMoeda.PVP6, ArtigoMoeda.Moeda";
            String outcols = "Artigo, Descricao, UnidadeBase, Familia, Desconto, PVP1, PVP6, Moeda";
            if (param.Count) cols = "COUNT(*) AS Count";

            if (!param.Count) query = "SELECT " + outcols + " FROM ("; else query = "";
            
                // Select Cols
                query += "SELECT " + cols + " ";
                if (!param.Count) {
                    query += ", ROW_NUMBER() OVER (ORDER BY ";
                        // Order
                        if (param.SortDate) query += "Artigo.DataUltimaActualizacao DESC";
                        else if (param.SortLastSold) query += "Artigo.DataUltSaida DESC";
                        else query += "Artigo.Artigo ASC";
                    query += ") AS RowNum ";
                }

                // Join Tables
                query += "FROM Artigo ";
                query += "JOIN ArtigoMoeda ON Artigo.Artigo = ArtigoMoeda.Artigo ";

                // Conditions
                query += "WHERE (1=1) ";
                if (param.CodProduct != "") query += "AND Artigo.Artigo='" + param.CodProduct + "' ";
                if (param.CodCategory != "") query += "AND Artigo.Familia='" + param.CodCategory + "' ";
                if (param.CodStore != "") query += "AND Artigo.Artigo IN (SELECT Artigo FROM ArtigoArmazem WHERE Armazem='" + param.CodStore + "') ";
                if (param.FilterPoints) query += "AND ArtigoMoeda.PVP6 > 0 ";
                if (param.FilterOnSale) query += "AND Artigo.Desconto > 0 ";
                if (param.SearchString != "") query += "AND Artigo.Descricao LIKE '%" + param.SearchString+"%' ";

   
                if (!param.Count) query += ") AS MyDerivedTable WHERE MyDerivedTable.RowNum BETWEEN " + (param.Offset + 1) + " AND " + (param.Offset + param.Limit);

           return query;        
        }
    }
}