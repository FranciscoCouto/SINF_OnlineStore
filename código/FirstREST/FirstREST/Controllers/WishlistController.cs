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
    public class WishlistController : ApiController
    {
        // GET api/Wishlist/
    
        public Wishlist Get(string id)
        {
            Lib_Primavera.Model.Wishlist wish = Lib_Primavera.PriIntegration.GetWishlistUser(id);
            if (wish == null)
            {
                throw new HttpResponseException(
                        Request.CreateResponse(HttpStatusCode.NotFound));

            }
            else
            {
                return wish;
            }
        }

        //POST /api/Carrinho/
        public HttpResponseMessage Post(TDU_WishlistProduto wishLinha)
        {
            Lib_Primavera.Model.RespostaErro erro = new Lib_Primavera.Model.RespostaErro();
            erro = Lib_Primavera.PriIntegration.InsereWishlistObj(wishLinha);
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
        public HttpResponseMessage Delete(TDU_WishlistProduto wishLinha)
        {
            Lib_Primavera.Model.RespostaErro erro = new Lib_Primavera.Model.RespostaErro();
            try
            {
                erro = Lib_Primavera.PriIntegration.DelArtigoWishlist(wishLinha);
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
