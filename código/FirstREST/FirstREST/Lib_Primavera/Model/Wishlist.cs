using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstREST.Lib_Primavera.Model
{
    public class Wishlist
    {
        public string idClient
        {
            get;
            set;
        }

        public string idWishlist
        {
            get;
            set;
        }

        public List<Model.Artigo> ID_Produtos 
        {
            get;
            set;
        }
    }
}