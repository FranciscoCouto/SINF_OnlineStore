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
    public class BanirController : ApiController
    {
        // POST api/clientes
        [HttpPost]
        public HttpResponseMessage Post(Lib_Primavera.Model.UserBan cliente)
        {
            Lib_Primavera.Model.RespostaErro erro = Lib_Primavera.PriIntegration.Ban(cliente);

            if (erro.Erro == 0)
            {
                var response = Request.CreateResponse(
                   HttpStatusCode.Created, erro.Descricao);
                return response;
            }

            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, erro.Descricao);
            }
        }

    }
}
