using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FirstREST.Controllers
{
    public class ArmazemController : Controller
    {
        // GET: /armazens/

        public IEnumerable<Lib_Primavera.Model.Armazem> Get()
        {
            return Lib_Primavera.PriIntegration.ListaArmazens();
        }

    }
}
