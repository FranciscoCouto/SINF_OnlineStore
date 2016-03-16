using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace FirstREST.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        // GET: /Home/
        [System.Web.Mvc.HttpGet]
        public ActionResult Index(String op, String op_dois)
        {
            if (op == "Categoria")
            {
                if (op_dois == null)
                {
                    List<Lib_Primavera.Model.Artigo> artigos = Lib_Primavera.PriIntegration.ListaArtigos();

                    artigos.OrderByDescending(p => p.ID);

                    IEnumerable<Lib_Primavera.Model.Artigo> temp = artigos.Take(3);

                    ViewBag.top = temp;


                    return View("/Views/Home/Index.cshtml");
                }

                else
                {

                    List<Lib_Primavera.Model.Artigo> artigos = Lib_Primavera.PriIntegration.ListaArtigos();
                    List<Lib_Primavera.Model.Artigo> artigos2 = new List<Lib_Primavera.Model.Artigo>();

                    foreach (var ar in artigos)
                    {
                        if (ar.SubFamilia == op_dois)
                        {
                            artigos2.Add(ar);
                        }
                    }

                    //if(artigos2.Count() == 0)
                    //    return View("/Views/Home/Index.cshtml");

                    //else{
                     ViewBag.cat = op_dois;
                        ViewBag.artigos = artigos2;
                        return View("/Views/ArtigoPage/produtos.cshtml");

                    //}

                }

            }


            else if (op == "Artigos")
            {

                if (op_dois == null)
                {

                    IEnumerable<Lib_Primavera.Model.Artigo> artigos = Lib_Primavera.PriIntegration.ListaArtigos();


                    ViewBag.artigos = artigos;

                    return View("/Views/ArtigoPage/produtos.cshtml");
                }

                else
                {

                    Lib_Primavera.Model.Artigo artigo = Lib_Primavera.PriIntegration.GetArtigo(op_dois);

                    ViewBag.id = artigo.ID;
                    ViewBag.model = artigo.Marca;
                    ViewBag.preco = Math.Round(double.Parse(artigo.Preço) * (1 + 23.0 / 100.0), 2);
                    ViewBag.descricao = artigo.Descricao;
                    ViewBag.stoke = artigo.SubFamilia;
                    ViewBag.imagem = artigo.CDU_Imagem;
                    ViewBag.stk = artigo.STKActual;
                    ViewBag.Nome = artigo.Nome;
    
                    var index = 0;
                    var index2 = 0;

                    List<Lib_Primavera.Model.Artigo> listArts = Lib_Primavera.PriIntegration.GetArtigosByCategoria(artigo.SubFamilia);

                    foreach (var v in listArts)
                    {
                        if (v.ID.Equals(artigo.ID))
                        {
                            index2 = index;

                        }
                        index++;
                    }

                    listArts.RemoveAt(index2);
                    ViewBag.artigos = listArts.Take(3);


                     //fazer os recomendados , que acho que nao esta a dar a outra funçao.

                    List<Lib_Primavera.Model.TDU_Comentario> listComs = Lib_Primavera.PriIntegration.ListaComentarios(artigo.ID);

                    ViewBag.comentarios = listComs;

                    //fazer os recomendados , que acho que nao esta a dar a outra funçao.

                    //Lib_Primavera.Model.Artigo artigos = Lib_Primavera.PriIntegration.GetArtigoByCategoria(artigo.SubFamilia);

                    return View("/Views/ArtigoPage/Index.cshtml");
                }
            }
            else if (op == "Carrinho")
            {

                if (Session["username"] == null)
                {

                    List<Lib_Primavera.Model.Artigo> artigos = Lib_Primavera.PriIntegration.ListaArtigos();

                    artigos.OrderByDescending(p => p.ID);

                    IEnumerable<Lib_Primavera.Model.Artigo> temp = artigos.Take(3);

                    ViewBag.top = temp;
                    return View("/Views/Home/Index.cshtml");


                }
                else
                {
                    string session = Session["username"].ToString();

                    Lib_Primavera.Model.Carrinho cart = Lib_Primavera.PriIntegration.GetCarrinhoUser(session);

                    ViewBag.idCar = cart.ID;
                    ViewBag.owner = cart.ID_Cliente;
                    ViewBag.Nome = Session["name"];
                        ViewBag.produtos = cart.ID_Produtos;

                        List<Lib_Primavera.Model.Armazem> listArms = Lib_Primavera.PriIntegration.ListaArmazens();

                        ViewBag.armazens = listArms;
                   

                    return View("/Views/ArtigoPage/carrinho.cshtml");
                }
            }
            else if (op == "Encomendas")
            {

                if (Session["username"] == null)
                {
                    List<Lib_Primavera.Model.Artigo> artigos = Lib_Primavera.PriIntegration.ListaArtigos();

                    artigos.OrderByDescending(p => p.ID);

                    IEnumerable<Lib_Primavera.Model.Artigo> temp = artigos.Take(3);

                    ViewBag.top = temp;
                    return View("/Views/Home/Index.cshtml");


                }
                else
                {
                    string session = Session["username"].ToString();

                    List<Lib_Primavera.Model.DocVenda> encomendas = Lib_Primavera.PriIntegration.GET_Pedidos(session);

                    ViewBag.Nome = Session["name"];
                    ViewBag.Encomendas = encomendas;


                    return View("/Views/ArtigoPage/Encomendas.cshtml");
                }
            }

            else if (op == "Wishlist")
            {

                if (Session["username"] == null)
                {
                    List<Lib_Primavera.Model.Artigo> artigos = Lib_Primavera.PriIntegration.ListaArtigos();

                    artigos.OrderByDescending(p => p.ID);

                    IEnumerable<Lib_Primavera.Model.Artigo> temp = artigos.Take(3);

                    ViewBag.top = temp;
                    return View("/Views/Home/Index.cshtml");


                }
                else
                {
                    string session = Session["username"].ToString();

                    Lib_Primavera.Model.Wishlist wishlist = Lib_Primavera.PriIntegration.GetWishlistUser(Session["username"].ToString());

                    ViewBag.Nome = Session["name"];
                    ViewBag.Wishlist = wishlist.ID_Produtos;


                    return View("/Views/ArtigoPage/Wishlist.cshtml");
                }
            }



            else if (op == "Login")
            {

                return View("/Views/Home/Login.cshtml");

            }

            else if (op == "Register")
            {

                return View("/Views/Home/Register.cshtml");

            }

            else if (op == "Contacto")
            {

                return View("/Views/Home/Contacto.cshtml");

            }

            else if (op == null && op_dois == null)
            {

                List<Lib_Primavera.Model.Artigo> artigos = Lib_Primavera.PriIntegration.ListaArtigos();

                artigos.OrderByDescending(p => p.ID);
                artigos.Reverse();

                IEnumerable<Lib_Primavera.Model.Artigo> temp = artigos.Take(3);

                ViewBag.top = temp;


                return View("/Views/Home/Index.cshtml");
            }

            else if (op == "Logout")
            {
                Session.Clear();
                List<Lib_Primavera.Model.Artigo> artigos = Lib_Primavera.PriIntegration.ListaArtigos();

                artigos.OrderByDescending(p => p.ID);

                IEnumerable<Lib_Primavera.Model.Artigo> temp = artigos.Take(3);

                ViewBag.top = temp;
                return View("/Views/Home/Index.cshtml");
            }
       
            else
                return null;
        }


        [System.Web.Mvc.HttpPost]
        public void addCarrinho(string idProduto)
        {
            Lib_Primavera.Model.RespostaErro erro = new Lib_Primavera.Model.RespostaErro();
            Lib_Primavera.Model.TDU_CarrinhoProduto carrinhoLinha = new Lib_Primavera.Model.TDU_CarrinhoProduto();
            carrinhoLinha.CDU_idProduto = idProduto;
            Lib_Primavera.Model.Carrinho carrinho = Lib_Primavera.PriIntegration.getCarrinhoID(Session["username"].ToString());
            carrinhoLinha.CDU_idCarrinho = carrinho.ID;

            erro = Lib_Primavera.PriIntegration.InsereCarrinhoObj(carrinhoLinha);

            if (erro.Erro == 0)
            {
                Console.Write(idProduto);
            }

            Response.Redirect("/Home/Artigos");
        }

        [System.Web.Mvc.HttpPost]
        public void addWishlist(string idProduto)
        {
            Lib_Primavera.Model.RespostaErro erro = new Lib_Primavera.Model.RespostaErro();
            Lib_Primavera.Model.TDU_WishlistProduto wishlistLinha = new Lib_Primavera.Model.TDU_WishlistProduto();
            Lib_Primavera.Model.Wishlist Wishlist = new Lib_Primavera.Model.Wishlist();
            Wishlist = Lib_Primavera.PriIntegration.GetWishlistUser(Session["username"].ToString());
            wishlistLinha.CDU_idProduto = idProduto;
            wishlistLinha.CDU_idWishlist = Wishlist.idWishlist;

            erro = Lib_Primavera.PriIntegration.InsereWishlistObj(wishlistLinha);

            if (erro.Erro == 0)
            {
                Console.Write(idProduto);
            }

            Response.Redirect("/Home/Wishlist");
        }

        [System.Web.Mvc.HttpPost]
        public void removeCarrinho(string idProduto)
        {
            Lib_Primavera.Model.RespostaErro erro = new Lib_Primavera.Model.RespostaErro();
            Lib_Primavera.Model.TDU_CarrinhoProduto carrinhoLinha = new Lib_Primavera.Model.TDU_CarrinhoProduto();
            carrinhoLinha.CDU_idProduto = idProduto;
            Lib_Primavera.Model.Carrinho carrinho = Lib_Primavera.PriIntegration.GetCarrinhoUser(Session["username"].ToString());
            carrinhoLinha.CDU_idCarrinho = carrinho.ID;

            erro = Lib_Primavera.PriIntegration.DelArtigoCarrinho(carrinhoLinha);

            if (erro.Erro == 0)
            {
                ;
            }


            Response.Redirect("/Home/Carrinho");
        }

        [System.Web.Mvc.HttpPost]
        public void atualizaCarrinho(string idCarrinhoProduto, string Quantidade, string Armazem)
        {
            Lib_Primavera.Model.RespostaErro erro = new Lib_Primavera.Model.RespostaErro();
            Lib_Primavera.Model.TDU_CarrinhoProduto carrinhoLinha = new Lib_Primavera.Model.TDU_CarrinhoProduto();
            carrinhoLinha.CDU_idCarrinhoProduto = idCarrinhoProduto;
            carrinhoLinha.CDU_Quantidade = Quantidade;
            carrinhoLinha.CDU_Armazem = Armazem;

            Lib_Primavera.Model.Carrinho carrinho = Lib_Primavera.PriIntegration.GetCarrinhoUser(Session["username"].ToString());
            carrinhoLinha.CDU_idCarrinho = carrinho.ID;

            erro = Lib_Primavera.PriIntegration.atualizaCarrinho(carrinhoLinha);

            if (erro.Erro == 0)
            {
                Console.Write(erro.Descricao);
            }


            Response.Redirect("/Home/Carrinho");
        }

        [System.Web.Mvc.HttpPost]
        public void addComentario(string idProduto, string comment)
        {
            Lib_Primavera.Model.RespostaErro erro = new Lib_Primavera.Model.RespostaErro();
            Lib_Primavera.Model.TDU_Comentario linhaComentario = new Lib_Primavera.Model.TDU_Comentario();
            linhaComentario.CDU_Conteudo = comment;
            linhaComentario.CDU_idProduto = idProduto;
            linhaComentario.CDU_idUtilizador = Session["username"].ToString();

            erro = Lib_Primavera.PriIntegration.InsereComentarioObj(linhaComentario);

            if (erro.Erro == 0)
            {
                Console.Write(comment);
            }

            Console.Write(idProduto);
            Response.Redirect("/Home/Artigos/" + idProduto);
        }

        [System.Web.Mvc.HttpPost]
        public void Index(String Username, String Password,String Remember)
        {
            IEnumerable<Lib_Primavera.Model.Cliente> clientes = Lib_Primavera.PriIntegration.ListaClientes();
            bool encontrou = false;
            string idCli = "";

            foreach (var cli in clientes)
            {
                if (cli.NomeCliente.Equals(Username) && cli.Password.Equals(Password))
                {
                    idCli = cli.ID;
                    encontrou = true;
                }
            }

            ViewBag.Username = Username;
            ViewBag.Password = Password;

            if (!encontrou)
                 Response.Redirect("/Home/Login");
            else
            {
                Session.Add("username", idCli);
                Session.Add("name", Username);
                Response.Redirect("/Home");
            }
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Register(String Username, String Email, String InputContribuinte, String InputPassword, String InputPassword2, String InputMorada, String InputTelefone)
        {
            IEnumerable<Lib_Primavera.Model.Cliente> clientes = Lib_Primavera.PriIntegration.ListaClientes();
            bool encontrou = false;
            Lib_Primavera.Model.RespostaErro erro = new Lib_Primavera.Model.RespostaErro();
            Lib_Primavera.Model.Cliente cli = new Lib_Primavera.Model.Cliente();
            cli.NomeCliente = Username;
            cli.Password = InputPassword;
            cli.NumContribuinte = InputContribuinte;
            cli.Email = Email;
            cli.Morada = InputMorada;
            cli.Telemóvel = InputTelefone;
           
            foreach (var client in clientes)
            {
                if (client.NomeCliente.Equals(Username))
                {
                    encontrou = true;
                }
            }

            if (encontrou || (!InputPassword.Equals(InputPassword2)))
            {
                return View("/Views/Home/Register.cshtml");
            }

            else
                erro = Lib_Primavera.PriIntegration.InsereClienteObj(cli);

            if (erro.Erro == 0)
            {
                return View("/Views/Home/Login.cshtml");
            }

            else
            {
                return View("/Views/Home/Register.cshtml");
            }

        }


        [System.Web.Mvc.HttpPost]
        public void pagamento(string s, string idCar)
        {
            Lib_Primavera.Model.RespostaErro erro = new Lib_Primavera.Model.RespostaErro();
            Lib_Primavera.Model.DocVenda  dv = new   Lib_Primavera.Model.DocVenda();
            dv.Entidade = Session["username"].ToString();
            dv.DocType = "FA";
            erro = Lib_Primavera.PriIntegration.Encomendas_New(dv);

            if (erro.Erro == 0)
            {
                Lib_Primavera.Model.TDU_CarrinhoProduto carrinho = new Lib_Primavera.Model.TDU_CarrinhoProduto();
                carrinho.CDU_idCarrinho = idCar;
                Lib_Primavera.PriIntegration.DelAllCarrinho(carrinho);
            }


            Response.Redirect("/Home/Artigos");
        
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Search(string wordsearch)
        {

            IEnumerable<Lib_Primavera.Model.Artigo> artigos = Lib_Primavera.PriIntegration.SearchArtigosNome(wordsearch);

            IEnumerable<Lib_Primavera.Model.Artigo> temp = artigos.Take(6);


            ViewBag.artigos = temp;

            return View("/Views/ArtigoPage/produtos.cshtml");

        }
       
        [System.Web.Mvc.HttpPost]
        public ActionResult Filtro(string filtro, string categoria)
        {


            List<Lib_Primavera.Model.Artigo> artigos = Lib_Primavera.PriIntegration.ListaArtigos();
            List<Lib_Primavera.Model.Artigo> artigos2 = new List<Lib_Primavera.Model.Artigo>();

            foreach (var ar in artigos)
            {
                if (ar.SubFamilia.Equals(categoria))
                {
                    artigos2.Add(ar);
                }
            }

            if (filtro == "caro")
            {
                artigos2.Sort((y, x) => float.Parse(x.Preço).CompareTo(float.Parse(y.Preço)));
            }

            else
            {
                artigos2.Sort((x, y) => float.Parse(x.Preço).CompareTo(float.Parse(y.Preço)));

            }

            ViewBag.cat = categoria;

            if (categoria != "")
                ViewBag.artigos = artigos2;
            else
            {
                if (filtro == "caro")
                {
                    artigos.Sort((y, x) => float.Parse(x.Preço).CompareTo(float.Parse(y.Preço)));
                }

                else
                {
                    artigos.Sort((x, y) => float.Parse(x.Preço).CompareTo(float.Parse(y.Preço)));

                }

                ViewBag.artigos = artigos;

            }

            return View("/Views/ArtigoPage/produtos.cshtml");

        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Filtro2(string priceMaximum, string priceMinimun, string categoria)
        {

            List<Lib_Primavera.Model.Artigo> artigos = Lib_Primavera.PriIntegration.ListaArtigos();
            List<Lib_Primavera.Model.Artigo> artigos2 = new List<Lib_Primavera.Model.Artigo>();

            foreach (var ar in artigos)
            {
                if (ar.SubFamilia.Equals(categoria) && float.Parse(ar.Preço) <= float.Parse(priceMaximum) && float.Parse(ar.Preço) >= float.Parse(priceMinimun))
                {
                    artigos2.Add(ar);
                }

                else if (categoria.Equals("") && float.Parse(ar.Preço) <= float.Parse(priceMaximum) && float.Parse(ar.Preço) >= float.Parse(priceMinimun))
                {

                    artigos2.Add(ar);

                }
            }

            

            ViewBag.cat = categoria;
            ViewBag.artigos = artigos2;

            return View("/Views/ArtigoPage/produtos.cshtml");

        }

    }
}

