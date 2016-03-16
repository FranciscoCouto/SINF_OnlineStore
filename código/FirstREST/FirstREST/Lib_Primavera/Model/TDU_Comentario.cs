using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstREST.Lib_Primavera.Model
{
    public class TDU_Comentario
    {

        public string CDU_idProduto 
        {
            get;
            set;
        }

        public string CDU_idComentario
        {
            get;
            set;
        }

        public string nomeCliente 
        {
            get;
            set;
        }

        public string CDU_Conteudo 
        {
            get;
            set;
        }

        public string CDU_idUtilizador //so para insere e remover comentario
        {
            get;
            set;
        }

    }
}