using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore.Lib_Primavera.Model
{
    public class ErrorResponse
    {

        public static string WRONG_CREDENTIALS = "Wrong credentials";
        public static string CLIENT_NOT_FOUND = "Client not found";


        public int Error
        { get; set; }

        public string Description
        { get; set; }


       

    }
}