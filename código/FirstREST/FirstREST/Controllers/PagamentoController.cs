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
    public class PagamentoController : ApiController
    {
        // GET api/cliente/5    
        public Pagamento Get(string id)
        {
            Lib_Primavera.Model.Pagamento pagamento = Lib_Primavera.PriIntegration.GetPagamentoUser(id);
            if (pagamento == null)
            {
                throw new HttpResponseException(
                        Request.CreateResponse(HttpStatusCode.NotFound));
            }
            else
            {
                return pagamento;
            }
        }
    }
}
