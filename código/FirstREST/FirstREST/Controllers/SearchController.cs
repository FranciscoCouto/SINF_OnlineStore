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
    public class SearchController : ApiController
    {
        //
        // GET: /Search/
        public IEnumerable<Lib_Primavera.Model.Artigo> Get(string id)
        {
            IEnumerable<Lib_Primavera.Model.Artigo> artigos = Lib_Primavera.PriIntegration.SearchArtigosNome(id);
            return artigos;
        }

    }
}
