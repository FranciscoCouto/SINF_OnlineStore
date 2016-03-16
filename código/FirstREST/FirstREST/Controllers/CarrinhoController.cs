using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FirstREST.Lib_Primavera.Model;

namespace FirstREST.Controllers
{
    public class CarrinhoController : ApiController
    {
        // GET: /api/carrinho

        public IEnumerable<Lib_Primavera.Model.Carrinho> Get()
        {
            return Lib_Primavera.PriIntegration.ListaCarrinhos();
        }


        // GET api/Carrinho/5    
        public Carrinho Get(string id)
        {
            Lib_Primavera.Model.Carrinho carrinho = Lib_Primavera.PriIntegration.GetCarrinhoUser(id);
            if (carrinho == null)
            {
                throw new HttpResponseException(
                        Request.CreateResponse(HttpStatusCode.NotFound));

            }
            else
            {
                return carrinho;
            }
        }

        //POST /api/Carrinho/
        public HttpResponseMessage Post(TDU_CarrinhoProduto carrinhoLinha)
        {
            Lib_Primavera.Model.RespostaErro erro = new Lib_Primavera.Model.RespostaErro();
            erro = Lib_Primavera.PriIntegration.InsereCarrinhoObj(carrinhoLinha);
            if (erro.Erro == 0)
            {
                return Request.CreateResponse(HttpStatusCode.Created, erro.Descricao);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, erro.Descricao);
            }
        }

        //DELETE /api/Carrinho/
        public HttpResponseMessage Delete(TDU_CarrinhoProduto carrinho)
        {
            Lib_Primavera.Model.RespostaErro erro = new Lib_Primavera.Model.RespostaErro();
            try
            {
                erro = Lib_Primavera.PriIntegration.DelArtigoCarrinho(carrinho);
                if (erro.Erro == 0)
                    return Request.CreateResponse(HttpStatusCode.OK, erro.Descricao);
                else return Request.CreateResponse(HttpStatusCode.NotFound, erro.Descricao);
            }
            catch (Exception exc)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, erro.Descricao + "|" + exc.Message);
            }
        }
    }
}
