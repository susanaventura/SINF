using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore.Lib_Primavera.Model
{
    public class ErrorResponse
    {

        public const string WRONG_CREDENTIALS = "Wrong credentials";
        public const string CLIENT_NOT_FOUND = "Client not found";
        public const string SUCCESS = "Success";

        public int Error
        { get; set; }

        public string Description
        { get; set; }

    }
}