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
    public class ComentarioController : ApiController
    {
       
        //POST /api/Comentario/
        public HttpResponseMessage Post(TDU_Comentario comentario)
        {
            Lib_Primavera.Model.RespostaErro erro = new Lib_Primavera.Model.RespostaErro();
            erro = Lib_Primavera.PriIntegration.InsereComentarioObj(comentario);
            if (erro.Erro == 0)
            {
                return Request.CreateResponse(HttpStatusCode.Created, erro.Descricao);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, erro.Descricao);
            }
        }

        //DELETE /api/Comentario/
        public HttpResponseMessage Delete(TDU_Comentario comentario)
        {
            Lib_Primavera.Model.RespostaErro erro = new Lib_Primavera.Model.RespostaErro();
            try
            {
                erro = Lib_Primavera.PriIntegration.DelComentario(comentario);
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

