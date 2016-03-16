using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstREST.Lib_Primavera.Model
{
    public class UserBan
    {
        public string ID //Cliente
        {
            get;
            set;
        }

         public string Action // True = Banir, False = Desbanir
        {
            get;
            set;
        }
    }

}
