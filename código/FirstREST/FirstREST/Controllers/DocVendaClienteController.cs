using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FirstREST.Lib_Primavera.Model;

namespace FirstREST.Controllers
{
    public class DocVendaClienteController : ApiController
    {
        // GET api/docvendacliente/  
        public IEnumerable<Lib_Primavera.Model.DocVenda> Get(string id) //id user
        {
            IEnumerable<Lib_Primavera.Model.DocVenda> docvenda = Lib_Primavera.PriIntegration.Encomenda_GetByCliente(id);
            if (docvenda == null)
            {
                throw new HttpResponseException(
                        Request.CreateResponse(HttpStatusCode.NotFound));

            }
            else
            {
                return docvenda;
            }
        }

    }
}