using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstREST.Lib_Primavera.Model
{
    public class Pagamento
    {

        //  public class Carrinho //TDU_CarrinhoCompras


        public string ID_Carrinho //CDU_idCarrinhoCompras
        {
            get;
            set;
        }

        public string ID_Cliente //CDU_idCliente
        {
            get;
            set;
        }

        public string ModoPagamento // ModoPag
        {
            get;
            set;
        }


        public List<Model.Artigo> ID_Produtos //CDU_idProduto
        {
            get;
            set;
        }

        public string total
        {
            get;
            set;
        }
    }

}