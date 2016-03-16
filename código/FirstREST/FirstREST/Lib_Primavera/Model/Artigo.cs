using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstREST.Lib_Primavera.Model
{
    public class Artigo
    {
        public string ID
        {
            get;
            set;
        }
        public string Nome
        {
            get;
            set;
        }

        public string DescArtigo //nao e descriçao do artigo
        {
            get;
            set;
        }

        public string Descricao //descriçao do artigo
        {
            get;
            set;
        }

         public string Desconto 
        {
            get;
            set;
        }

        public string Familia
        {
            get;
            set;
        }

        public string Marca
        {
            get;
            set;
        }

        public string Modelo
        {
            get;
            set;
        }

        public string STKActual
        {
            get;
            set;
        }

        public string SubFamilia
        {
            get;
            set;
        }

        public string Preço //PCPadrao (table)
        {
            get;
            set;
        }

        public String Wishlist
        {
            get;
            set;
        }


        public String CDU_Imagem//imagens do artigo
        {
            get;
            set;
        }

        public List<Model.TDU_Comentario> comentarios
        {
            get;
            set;
        }

        //opcional para o carrinho

        public string Quantidade 
        {
            get;
            set;
        }

        public string Armazem
        {
            get;
            set;
        }
        public string CDU_idCarrinhoProduto
        {
            get;
            set;
        }
    }
}