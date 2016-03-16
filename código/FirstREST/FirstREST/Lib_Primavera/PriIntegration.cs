using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Interop.ErpBS800;
using Interop.StdPlatBS800;
using Interop.StdBE800;
using Interop.GcpBE800;
using ADODB;
using Interop.IGcpBS800;
//using Interop.StdBESql800;
//using Interop.StdBSSql800;  

namespace FirstREST.Lib_Primavera
{
    public class PriIntegration
    {
        # region Cliente

        public static List<Model.Cliente> ListaClientes()
        {
            StdBELista objList;

            List<Model.Cliente> listClientes = new List<Model.Cliente>();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {

                //objList = PriEngine.Engine.Comercial.Clientes.LstClientes();

                objList = PriEngine.Engine.Consulta("SELECT Cliente, Nome, NumContrib as NumContribuinte, Fac_Mor AS morada,  ClienteAnulado, Fac_Local, Fac_Tel, CDU_PASSWORD, EnderecoWeb  FROM  CLIENTES");


                while (!objList.NoFim())
                {
                    listClientes.Add(new Model.Cliente
                    {
                        ID = objList.Valor("Cliente"),
                        NomeCliente = objList.Valor("Nome"),
                        NumContribuinte = objList.Valor("NumContribuinte"),
                        Morada = objList.Valor("morada"),
                        ClienteBanido = objList.Valor("ClienteAnulado").ToString(),
                        Localidade = objList.Valor("Fac_Local"),
                        Telemóvel = objList.Valor("Fac_Tel"),
                        Email = objList.Valor("EnderecoWeb"),
                        //Password = PriEngine.Platform.Criptografia.Descripta(objList.Valor("CDU_PASSWORD"), 50)
                        Password = objList.Valor("CDU_PASSWORD")
                    });
                    objList.Seguinte();

                }

                return listClientes;
            }
            else
                return null;

        }

        public static Lib_Primavera.Model.Cliente GetCliente(string id)
        {


            StdBELista objList;

            //List<Model.Cliente> listClientes = new List<Model.Cliente>();
            Model.Cliente myCli = new Model.Cliente();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {

                //objList = PriEngine.Engine.Comercial.Clientes.LstClientes();

                objList = PriEngine.Engine.Consulta("SELECT Cliente, Nome, NumContrib as NumContribuinte, Fac_Mor AS campo_exemplo,  ClienteAnulado,  Fac_Local, Fac_Tel, EnderecoWeb FROM  CLIENTES WHERE Cliente='" + id + "'"); //supostamente nao se quer listar a password

                while (!objList.NoFim())
                {

                    myCli.ID = objList.Valor("Cliente");
                    myCli.NomeCliente = objList.Valor("Nome");
                    myCli.NumContribuinte = objList.Valor("NumContribuinte");
                    myCli.Morada = objList.Valor("campo_exemplo");
                    myCli.ClienteBanido = objList.Valor("ClienteAnulado").ToString();
                    myCli.Localidade = objList.Valor("Fac_Local");
                    myCli.Telemóvel = objList.Valor("Fac_Tel");
                    myCli.Email = objList.Valor("EnderecoWeb");


                    objList.Seguinte();

                }

                return myCli;
            }
            else
                return null;
        }

        public static Lib_Primavera.Model.RespostaErro UpdCliente(Lib_Primavera.Model.Cliente cliente)
        {
            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();


            GcpBECliente objCli = new GcpBECliente();

            StdBECampos campos = new StdBECampos();
            StdBECampo campo = new StdBECampo();


            try
            {

                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {

                    if (PriEngine.Engine.Comercial.Clientes.Existe(cliente.ID) == false)
                    {
                        erro.Erro = 1;
                        erro.Descricao = "O cliente não existe";
                        return erro;
                    }
                    else
                    {

                        objCli = PriEngine.Engine.Comercial.Clientes.Edita(cliente.ID);
                        objCli.set_EmModoEdicao(true);

                        objCli.set_Nome(cliente.NomeCliente);
                        objCli.set_NumContribuinte(cliente.NumContribuinte);
                        objCli.set_EnderecoWeb(cliente.Email);
                        objCli.set_Morada(cliente.Morada);
                        objCli.set_Telefone(cliente.Telemóvel);
                        objCli.set_Localidade(cliente.Localidade);

                        //EDITAR A PASSWORD
                        campo.Nome = "CDU_Password";
                        PriEngine.Platform.Criptografia.Encripta(cliente.Password, 50);
                        campo.Valor = cliente.Password;
                        campos.Insere(campo);

                        PriEngine.Engine.Comercial.Clientes.Actualiza(objCli);

                        erro.Erro = 0;
                        erro.Descricao = "Sucesso";
                        return erro;
                    }
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir a empresa";
                    return erro;

                }

            }

            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }

        }

        public static Lib_Primavera.Model.RespostaErro DelCliente(string id)
        {

            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
            GcpBECliente objCli = new GcpBECliente();


            try
            {

                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {
                    if (PriEngine.Engine.Comercial.Clientes.Existe(id) == false)
                    {
                        erro.Erro = 1;
                        erro.Descricao = "O cliente não existe";
                        return erro;
                    }
                    else
                    {

                        PriEngine.Engine.Comercial.Clientes.Remove(id);
                        erro.Erro = 0;
                        erro.Descricao = "Sucesso";
                        return erro;
                    }
                }

                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir a empresa";
                    return erro;
                }
            }

            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }

        }

        public static bool existeEmail(string email)
        {
            StdBELista objList;
            objList = PriEngine.Engine.Consulta("SELECT Cliente FROM  CLIENTES WHERE EnderecoWeb='" + email + "'");

            if (objList != null)
            {
                if (objList.Vazia())
                    return false;
                return true;
            }
            return false;
        }

        public static bool existeNome(string nome)
        {
            StdBELista objList;
            objList = PriEngine.Engine.Consulta("SELECT Cliente FROM  CLIENTES WHERE Nome='" + nome + "'");

            if (objList != null)
            {
                if (objList.Vazia())
                    return false;
                return true;
            }
            return false;
        }

        public static Lib_Primavera.Model.RespostaErro InsereClienteObj(Model.Cliente cli)
        {

            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();


            GcpBECliente myCli = new GcpBECliente();

            StdBECampos campos = new StdBECampos();
            StdBECampo campo = new StdBECampo();

            try
            {
                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {
                    myCli.set_NumContribuinte(cli.NumContribuinte);
                    myCli.set_Morada(cli.Morada);
                    myCli.set_Telefone(cli.Telemóvel);
                    myCli.set_Localidade(cli.Localidade);
                    myCli.set_Moeda("EUR");

                    //Dar o id correto (ainda nao esta muito bem) , e por acaso isto verifica se tem o mesmo id automaticamente
                    List<Model.Cliente> clientes = ListaClientes();
                    int id;

                    if (clientes.Count >= 2)
                    {
                        id = clientes.Count;
                    }
                    else
                        id = 1;

                    myCli.set_Cliente(id.ToString());

                    //Verificação Email
                    if (cli.Email == null)
                    {
                        erro.Erro = 1;
                        erro.Descricao = "Introduza um email válido, por favor.";
                        return erro;
                    }
                    else if (existeEmail(cli.Email))
                    {
                        erro.Erro = 1;
                        erro.Descricao = "Email já existente.";
                        return erro;
                    }
                    else
                    {
                        myCli.set_EnderecoWeb(cli.Email);
                    }

                    //Verificação nome
                    if (cli.NomeCliente == null)
                    {
                        erro.Erro = 1;
                        erro.Descricao = "Introduza um nome, por favor.";
                        return erro;
                    }
                    else if (existeNome(cli.NomeCliente))
                    {
                        erro.Erro = 1;
                        erro.Descricao = "Introduza outro nome, por favor.";
                        return erro;
                    }
                    else
                    {
                        myCli.set_Nome(cli.NomeCliente);
                    }



                    //inserir a password
                    campo.Nome = "CDU_Password";
                    //É preciso aumentar o espaço da base de dados, 20 nao chega para encripta-la tem que ter 50 para ai ou mais.
                    //campo.Valor = PriEngine.Platform.Criptografia.Encripta(cli.Password, 20);
                    PriEngine.Platform.Criptografia.Encripta(cli.Password, 50);
                    campo.Valor = cli.Password;
                    campos.Insere(campo);
                    myCli.set_CamposUtil(campos);

                    PriEngine.Engine.Comercial.Clientes.Actualiza(myCli);

                    erro.Erro = 0;
                    erro.Descricao = "Sucesso";
                    return erro;
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir empresa";
                    return erro;
                }
            }

            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }


        }



        #endregion Cliente;   // -----------------------------  END   CLIENTE    -----------------------


        #region Artigo

        public static Lib_Primavera.Model.Artigo GetArtigo(string id)
        {

            StdBELista objList;
            StdBELista objImage;
            StdBELista objComents;
            StdBELista objRating;
            StdBELista objWishlist;

            Model.Artigo myArt = new Model.Artigo();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {

                //objList = PriEngine.Engine.Comercial.Clientes.LstClientes();

                objList = PriEngine.Engine.Consulta("SELECT CDU_Nome, ARTIGO.Artigo, ArtigoMoeda.Artigo, CDU_Descricao, Desconto, STKActual, Familia, SubFamilia, Marca, Modelo, CDU_Imagem, PVP1 FROM  ARTIGO, ArtigoMoeda WHERE ARTIGO.Artigo='" + id + "'AND  ARTIGO.Artigo = ArtigoMoeda.Artigo");
                
                while (!objList.NoFim())
                {

                    myArt.ID = objList.Valor("Artigo");
                    float desconto = objList.Valor("Desconto");
                    myArt.Desconto = desconto.ToString();
                    //myArt.DescArtigo = objList.Valor("Descricao");
                    double stokeAtual = objList.Valor("STKActual");
                    myArt.STKActual = stokeAtual.ToString();
                    double preco = objList.Valor("PVP1");
                    myArt.Preço = Math.Round(Convert.ToDouble(preco) * 1.23, 2).ToString();
                    myArt.Familia = objList.Valor("Familia");
                    myArt.SubFamilia = objList.Valor("SubFamilia");
                    myArt.Marca = objList.Valor("Marca");
                    myArt.Modelo = objList.Valor("Modelo");
                    myArt.CDU_Imagem = objList.Valor("CDU_Imagem");
                    myArt.Descricao = objList.Valor("CDU_Descricao");
                    myArt.Nome = objList.Valor("CDU_Nome");
              



                    objComents = PriEngine.Engine.Consulta("SELECT Nome, CDU_idComentario, CDU_idProduto, CDU_Conteudo FROM TDU_Comentario, CLIENTES WHERE CDU_idProduto='" + myArt.ID + "' AND CDU_idUtilizador = Cliente");
                    myArt.comentarios = new List<Model.TDU_Comentario>();
                   

                    while (!objComents.NoFim())
                    {
                        Model.TDU_Comentario temp = new Model.TDU_Comentario();
                        temp.CDU_idProduto = objComents.Valor("CDU_idProduto");
                        temp.nomeCliente = objComents.Valor("Nome");
                        temp.CDU_idComentario = objComents.Valor("CDU_idComentario").ToString();
                        temp.CDU_Conteudo = objComents.Valor("CDU_Conteudo");

                        myArt.comentarios.Add(temp);

                        objComents.Seguinte();
                    }

                 
                    /*objWishlist = PriEngine.Engine.Consulta("SELECT * FROM TDU_Wishlist, TDU_WishlistProduto WHERE CDU_idUtilizador ='" + "ALCAD" + "' AND CDU_idProduto='" + myArt.ID + "' AND TDU_Wishlist.CDU_idWishlist = TDU_WishlistProduto.CDU_idWishlist");
                    if (!objWishlist.NoFim())
                    {
                        myArt.Wishlist = "True";
                    }
                    else
                        myArt.Wishlist = "False";
                    */
                    objList.Seguinte();

                }

                return myArt;
            }
            else
                return null;

        }

        public static List<Model.Artigo> GetArtigosByCategoria(string sub_familia) //Recomendados
        {

            StdBELista objList;

           

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {

                //objList = PriEngine.Engine.Comercial.Clientes.LstClientes();
                List<Model.Artigo> listArts = new List<Model.Artigo>();
                objList = PriEngine.Engine.Consulta("SELECT CDU_Nome, ARTIGO.Artigo, CDU_Descricao, Desconto, STKActual, Familia, SubFamilia, Marca, Modelo, CDU_Imagem, PVP1 FROM  ARTIGO, ArtigoMoeda WHERE SubFamilia='" + sub_familia + "' AND ArtigoMoeda.Artigo = ARTIGO.Artigo");

                while (!objList.NoFim())
                {
                    Model.Artigo myArt = new Model.Artigo();
                    myArt.ID = objList.Valor("Artigo");
                    float desconto = objList.Valor("Desconto");
                    myArt.Desconto = desconto.ToString();
                    //myArt.DescArtigo = objList.Valor("Descricao");
                    double stokeAtual = objList.Valor("STKActual");
                    myArt.STKActual = stokeAtual.ToString();
                    double preco = objList.Valor("PVP1");
                    myArt.Preço = Math.Round(Convert.ToDouble(preco) * 1.23, 2).ToString();
                    myArt.Familia = objList.Valor("Familia");
                    myArt.SubFamilia = objList.Valor("SubFamilia");
                    myArt.Marca = objList.Valor("Marca");
                    myArt.Modelo = objList.Valor("Modelo");
                    myArt.CDU_Imagem = objList.Valor("CDU_Imagem");
                    myArt.Descricao = objList.Valor("CDU_Descricao");
                    myArt.Nome = objList.Valor("CDU_Nome");
                    listArts.Add(myArt);
                    objList.Seguinte();

                }

                return listArts;
            }
            else
                return null;

        }

        public static List<Model.Artigo> ListaArtigos()
        {

            StdBELista objList;

            Model.Artigo art = new Model.Artigo();
            List<Model.Artigo> listArts = new List<Model.Artigo>();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {
                objList = PriEngine.Engine.Consulta("SELECT CDU_Nome, ARTIGO.Artigo, ArtigoMoeda.Artigo, Descricao, Desconto, STKActual, Familia, SubFamilia, Marca, Modelo, CDU_Imagem, CDU_Descricao, PVP1 FROM  ARTIGO, ArtigoMoeda WHERE ARTIGO.Artigo = ArtigoMoeda.Artigo");

                //objList = PriEngine.Engine.Comercial.Artigos.LstArtigos();

                while (!objList.NoFim())
                {
                    art = new Model.Artigo();
                    art.ID = objList.Valor("artigo");
                    art.DescArtigo = objList.Valor("descricao");
                    art.Desconto = objList.Valor("desconto").ToString();
                    art.STKActual = objList.Valor("stkactual").ToString();
                    art.Preço = Math.Round(Convert.ToDouble(objList.Valor("PVP1")) * 1.23, 2).ToString();
                    art.Familia = objList.Valor("familia");
                    art.SubFamilia = objList.Valor("subfamilia");
                    art.Marca = objList.Valor("marca");
                    art.Modelo = objList.Valor("modelo");
                    art.CDU_Imagem = objList.Valor("CDU_Imagem");
                    art.Nome = objList.Valor("CDU_Nome");
                    art.Descricao = objList.Valor("CDU_Descricao");

                    listArts.Add(art);
                    objList.Seguinte();
                }

                return listArts;

            }
            else
            {
                return null;

            }

        }

        public static Lib_Primavera.Model.RespostaErro InsereArtigoObj(Model.Artigo art)
        {

            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();


            GcpBEArtigo myArt = new GcpBEArtigo();

            try
            {
                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {

                    myArt.set_Artigo(art.ID);
                    myArt.set_Descricao(art.DescArtigo);
                    myArt.set_Desconto(Convert.ToSingle(art.Desconto));
                    myArt.set_StkActual(Convert.ToSingle(art.STKActual));
                    myArt.set_PCPadrao(Convert.ToSingle(art.Preço));
                    myArt.set_Familia(art.Familia);
                    myArt.set_SubFamilia(art.SubFamilia);
                    myArt.set_Marca(art.Marca);
                    myArt.set_Modelo(art.Modelo);
                    myArt.set_IVA("23");

                    PriEngine.Engine.Comercial.Artigos.Actualiza(myArt);

                    erro.Erro = 0;
                    erro.Descricao = "Sucesso";
                    return erro;
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir empresa";
                    return erro;
                }
            }

            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }


        }

        public static Lib_Primavera.Model.RespostaErro UpdArtigo(Lib_Primavera.Model.Artigo art)
        {
            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();


            GcpBEArtigo objArt = new GcpBEArtigo();


            try
            {

                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {

                    if (PriEngine.Engine.Comercial.Artigos.Existe(art.ID) == false)
                    {
                        erro.Erro = 1;
                        erro.Descricao = "O artigo não existe";
                        return erro;
                    }
                    else
                    {

                        objArt = PriEngine.Engine.Comercial.Artigos.Edita(art.ID);
                        objArt.set_EmModoEdicao(true);

                        objArt.set_Artigo(art.ID);
                        objArt.set_Descricao(art.DescArtigo);
                        objArt.set_Desconto(Convert.ToSingle(art.Desconto));
                        objArt.set_StkActual(Convert.ToSingle(art.STKActual));
                        objArt.set_PCPadrao(Convert.ToSingle(art.Preço));
                        objArt.set_Familia(art.Familia);
                        objArt.set_SubFamilia(art.SubFamilia);
                        objArt.set_Marca(art.Marca);
                        objArt.set_Modelo(art.Modelo);


                        PriEngine.Engine.Comercial.Artigos.Actualiza(objArt);

                        erro.Erro = 0;
                        erro.Descricao = "Sucesso";
                        return erro;
                    }
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir a empresa";
                    return erro;

                }

            }

            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }

        }

        public static Lib_Primavera.Model.RespostaErro DelArtigo(string id)
        {

            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
            GcpBEArtigo objArt = new GcpBEArtigo();


            try
            {

                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {
                    if (PriEngine.Engine.Comercial.Artigos.Existe(id) == false)
                    {
                        erro.Erro = 1;
                        erro.Descricao = "O cliente não existe";
                        return erro;
                    }
                    else
                    {

                        PriEngine.Engine.Comercial.Artigos.Remove(id);
                        erro.Erro = 0;
                        erro.Descricao = "Sucesso";
                        return erro;
                    }
                }

                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir a empresa";
                    return erro;
                }
            }

            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }

        }




        #endregion Artigo


        #region DocCompra


        public static List<Model.DocCompra> VGR_List()
        {

            StdBELista objListCab;
            StdBELista objListLin;
            Model.DocCompra dc = new Model.DocCompra();
            List<Model.DocCompra> listdc = new List<Model.DocCompra>();
            Model.LinhaDocCompra lindc = new Model.LinhaDocCompra();
            List<Model.LinhaDocCompra> listlindc = new List<Model.LinhaDocCompra>();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {
                objListCab = PriEngine.Engine.Consulta("SELECT id, NumDocExterno, Entidade, DataDoc, NumDoc, TotalMerc, Serie From CabecCompras where TipoDoc='VGR'");
                while (!objListCab.NoFim())
                {
                    dc = new Model.DocCompra();
                    dc.id = objListCab.Valor("id");
                    dc.NumDocExterno = objListCab.Valor("NumDocExterno");
                    dc.Entidade = objListCab.Valor("Entidade");
                    dc.NumDoc = objListCab.Valor("NumDoc");
                    dc.Data = objListCab.Valor("DataDoc");
                    dc.TotalMerc = objListCab.Valor("TotalMerc");
                    dc.Serie = objListCab.Valor("Serie");
                    objListLin = PriEngine.Engine.Consulta("SELECT idCabecCompras, Artigo, Descricao, Quantidade, Unidade, PrecUnit, Desconto1, TotalILiquido, PrecoLiquido, Armazem, Lote from LinhasCompras where IdCabecCompras='" + dc.id + "' order By NumLinha");
                    listlindc = new List<Model.LinhaDocCompra>();

                    while (!objListLin.NoFim())
                    {
                        lindc = new Model.LinhaDocCompra();
                        lindc.IdCabecDoc = objListLin.Valor("idCabecCompras");
                        lindc.CodArtigo = objListLin.Valor("Artigo");
                        lindc.DescArtigo = objListLin.Valor("Descricao");
                        lindc.Quantidade = objListLin.Valor("Quantidade");
                        lindc.Unidade = objListLin.Valor("Unidade");
                        lindc.Desconto = objListLin.Valor("Desconto1");
                        lindc.PrecoUnitario = objListLin.Valor("PrecUnit");
                        lindc.TotalILiquido = objListLin.Valor("TotalILiquido");
                        lindc.TotalLiquido = objListLin.Valor("PrecoLiquido");
                        lindc.Armazem = objListLin.Valor("Armazem");
                        lindc.Lote = objListLin.Valor("Lote");

                        listlindc.Add(lindc);
                        objListLin.Seguinte();
                    }

                    dc.LinhasDoc = listlindc;

                    listdc.Add(dc);
                    objListCab.Seguinte();
                }
            }
            return listdc;
        }


        public static Model.RespostaErro VGR_New(Model.DocCompra dc)
        {
            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();


            GcpBEDocumentoCompra myGR = new GcpBEDocumentoCompra();
            GcpBELinhaDocumentoCompra myLin = new GcpBELinhaDocumentoCompra();
            GcpBELinhasDocumentoCompra myLinhas = new GcpBELinhasDocumentoCompra();

            PreencheRelacaoCompras rl = new PreencheRelacaoCompras();
            List<Model.LinhaDocCompra> lstlindv = new List<Model.LinhaDocCompra>();

            try
            {
                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {
                    // Atribui valores ao cabecalho do doc
                    //myEnc.set_DataDoc(dv.Data);
                    myGR.set_Entidade(dc.Entidade);
                    myGR.set_NumDocExterno(dc.NumDocExterno);
                    myGR.set_Serie(dc.Serie);
                    myGR.set_Tipodoc("VGR");
                    myGR.set_TipoEntidade("F");
                    // Linhas do documento para a lista de linhas
                    lstlindv = dc.LinhasDoc;
                    PriEngine.Engine.Comercial.Compras.PreencheDadosRelacionados(myGR, rl);
                    foreach (Model.LinhaDocCompra lin in lstlindv)
                    {
                        PriEngine.Engine.Comercial.Compras.AdicionaLinha(myGR, lin.CodArtigo, lin.Quantidade, lin.Armazem, "", lin.PrecoUnitario, lin.Desconto);
                    }


                    PriEngine.Engine.IniciaTransaccao();
                    PriEngine.Engine.Comercial.Compras.Actualiza(myGR, "Teste");
                    PriEngine.Engine.TerminaTransaccao();
                    erro.Erro = 0;
                    erro.Descricao = "Sucesso";
                    return erro;
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir empresa";
                    return erro;

                }

            }
            catch (Exception ex)
            {
                PriEngine.Engine.DesfazTransaccao();
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }
        }


        #endregion DocCompra


        #region DocsVenda

        public static List<Model.DocVenda> GET_Pedidos(string idCliente)
        {
            StdBELista objList, objListLin, objDataLiq;
            List<Model.DocVenda> listdv = new List<Model.DocVenda>();
            List<Model.LinhaDocVenda> listlindv = new List<Model.LinhaDocVenda>();
            Model.LinhaDocVenda lindv;

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {
                objList = PriEngine.Engine.Consulta("select Id,Data,TotalMerc, TotalIva, Estado from CabecDoc JOIN CabecDocStatus ON CabecDoc.Id = CabecDocStatus.IdCabecDoc where TipoDoc = 'FA' and Entidade = '" + idCliente + "' order By NumDoc DESC");
                while (!objList.NoFim())
                {
                    Model.DocVenda dv = new Model.DocVenda();
                    dv.id = objList.Valor("Id");
                    dv.Data = objList.Valor("Data");
                    dv.PrecoFinal = (double.Parse(objList.Valor("TotalMerc").ToString()) + double.Parse((objList.Valor("TotalIva").ToString())));
                    //dv.PrecoFinal = objList.Valor("TotalMerc") + "€ + IVA";
                    
                    if (objList.Valor("Estado") == "T")
                    {
                        dv.estado = "Pronto";
                    }
                    else if (objList.Valor("Estado") == "P")
                    {
                        dv.estado = "Pendente";
                    }
                    else dv.estado = "Anulado";

                    objListLin = PriEngine.Engine.Consulta("SELECT Artigo,Descricao,Quantidade from LinhasDoc where IdCabecDoc='" + dv.id + "'");
                    listlindv = new List<Model.LinhaDocVenda>();

                    while (!objListLin.NoFim())
                    {
                        lindv = new Model.LinhaDocVenda();
                        lindv.DescArtigo = objListLin.Valor("Descricao");
                        lindv.CodArtigo = objListLin.Valor("Artigo");
                        lindv.Quantidade = objListLin.Valor("Quantidade");
                        listlindv.Add(lindv);
                        objListLin.Seguinte();
                    }

                    objDataLiq = PriEngine.Engine.Consulta("SELECT DataLiq from Historico where idDoc='" + dv.id + "'");

                    try {
                        
                        dv.DataLiq = objDataLiq.Valor("DataLiq").ToString();
                        if (dv.DataLiq != "")
                        dv.DataLiq = "Pago";
                        else
                            dv.DataLiq = "Por Pagar";
                        

                    }
                    catch (Exception e)
                    {
                        dv.DataLiq = "Por Pagar";
                    }
                    
                    

                    dv.LinhasDoc = listlindv;

                    listdv.Add(dv);
                    objList.Seguinte();
                }
            }
            return listdv;
        }

        public static Model.RespostaErro Encomendas_New(Model.DocVenda dv)
        {
            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
            GcpBEDocumentoVenda myEnc = new GcpBEDocumentoVenda();

            GcpBELinhaDocumentoVenda myLin = new GcpBELinhaDocumentoVenda();

            GcpBELinhasDocumentoVenda myLinhas = new GcpBELinhasDocumentoVenda();

            PreencheRelacaoVendas rl = new PreencheRelacaoVendas();
            List<Model.LinhaDocVenda> lstlindv = new List<Model.LinhaDocVenda>();

            try
            {
                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {
                    // Atribui valores ao cabecalho do doc
                    //myEnc.set_DataDoc(dv.Data);

                    string serie = "C";
                    double desconto = 0.0;
                    string armazem = "A2";


                    myEnc.set_Entidade(dv.Entidade);
                    myEnc.set_Serie(serie);
                    //myEnc.set_Serie(dv.Serie);
                    myEnc.set_Tipodoc(dv.DocType);
                    myEnc.set_TipoEntidade("C");
                    // Linhas do documento para a lista de linhas
                    lstlindv = dv.LinhasDoc;

                    PriEngine.Engine.Comercial.Vendas.PreencheDadosRelacionados(myEnc, rl);


                    
                       
                   
                    double pvp = 0;
                    if (dv.LinhasDoc != null)
                        foreach (Model.LinhaDocVenda lin in lstlindv)
                        {
                            pvp = PriEngine.Engine.Comercial.ArtigosPrecos.DaPrecoArtigoMoeda(lin.CodArtigo, "EUR", "UN", "PVP1", false, 0);
                            PriEngine.Engine.Comercial.Vendas.AdicionaLinha(myEnc, lin.CodArtigo, lin.Quantidade, armazem, "", pvp, desconto);
                        }
                    else
                    {

                        List<Model.Artigo> temp = GetCarrinhoUser(dv.Entidade).ID_Produtos;
                        foreach (Model.Artigo lin in temp)
                        {
                            pvp = PriEngine.Engine.Comercial.ArtigosPrecos.DaPrecoArtigoMoeda(lin.ID, "EUR", "UN", "PVP1", false, 0);
                            PriEngine.Engine.Comercial.Vendas.AdicionaLinha(myEnc, lin.ID, Int32.Parse(lin.Quantidade), lin.Armazem, "", pvp, desconto);
                        }
                    }


                    // PriEngine.Engine.Comercial.Compras.TransformaDocumento(

                    PriEngine.Engine.IniciaTransaccao();
                    PriEngine.Engine.Comercial.Vendas.Actualiza(myEnc, "Teste");
                    PriEngine.Engine.TerminaTransaccao();
                    erro.Erro = 0;
                    erro.Descricao = "Sucesso";
                    return erro;
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir empresa";
                    return erro;

                }

            }
            catch (Exception ex)
            {
                try
                {
                    PriEngine.Engine.DesfazTransaccao();
                }
                catch { }
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }
        }

        public static List<Model.DocVenda> Encomendas_List()
        {

            StdBELista objListCab;
            StdBELista objListLin;
            Model.DocVenda dv = new Model.DocVenda();
            List<Model.DocVenda> listdv = new List<Model.DocVenda>();
            Model.LinhaDocVenda lindv = new Model.LinhaDocVenda();
            List<Model.LinhaDocVenda> listlindv = new
            List<Model.LinhaDocVenda>();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {
                objListCab = PriEngine.Engine.Consulta("SELECT id, Entidade, Data, NumDoc, TotalMerc, TotalIva, TotalDesc, Serie From CabecDoc where TipoDoc='FA'");
                while (!objListCab.NoFim())
                {
                    dv = new Model.DocVenda();
                    dv.id = objListCab.Valor("id");
                    dv.Entidade = objListCab.Valor("Entidade");
                    dv.NumDoc = objListCab.Valor("NumDoc");
                    dv.Data = objListCab.Valor("Data");

                    dv.Serie = objListCab.Valor("Serie");
                    double preço = 0;

                    objListLin = PriEngine.Engine.Consulta("SELECT idCabecDoc, Artigo, Descricao, Quantidade, Unidade, PrecUnit, Desconto1, TotalILiquido, PrecoLiquido from LinhasDoc where IdCabecDoc='" + dv.id + "' order By NumLinha");
                    listlindv = new List<Model.LinhaDocVenda>();

                    while (!objListLin.NoFim())
                    {
                        lindv = new Model.LinhaDocVenda();
                        lindv.IdCabecDoc = objListLin.Valor("idCabecDoc");
                        lindv.CodArtigo = objListLin.Valor("Artigo");
                        lindv.DescArtigo = objListLin.Valor("Descricao");
                        lindv.Quantidade = objListLin.Valor("Quantidade");
                        lindv.Unidade = objListLin.Valor("Unidade");
                        lindv.Desconto = objListLin.Valor("Desconto1");
                        lindv.PrecoUnitario = objListLin.Valor("PrecUnit");
                        lindv.TotalILiquido = objListLin.Valor("TotalILiquido");
                        lindv.TotalLiquido = objListLin.Valor("PrecoLiquido");

                        //preço ja com iva e a descontar os descontos
                        preço += lindv.TotalILiquido - lindv.Desconto;


                        listlindv.Add(lindv);
                        objListLin.Seguinte();
                    }
                    dv.PrecoFinal = preço;
                    dv.LinhasDoc = listlindv;
                    listdv.Add(dv);
                    objListCab.Seguinte();
                }
            }
            return listdv;
        }

        public static Model.DocVenda Encomenda_Get(string numdoc)
        {


            StdBELista objListCab;
            StdBELista objListLin;
            Model.DocVenda dv = new Model.DocVenda();
            Model.LinhaDocVenda lindv = new Model.LinhaDocVenda();
            List<Model.LinhaDocVenda> listlindv = new List<Model.LinhaDocVenda>();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {


                string st = "SELECT id, Entidade, Data, NumDoc, TotalMerc, Serie From CabecDoc where TipoDoc='FA' and NumDoc='" + numdoc + "'";
                objListCab = PriEngine.Engine.Consulta(st);
                dv = new Model.DocVenda();

                dv = new Model.DocVenda();
                dv.id = objListCab.Valor("id");
                dv.Entidade = objListCab.Valor("Entidade");
                dv.NumDoc = objListCab.Valor("NumDoc");
                dv.Data = objListCab.Valor("Data");
                dv.Serie = objListCab.Valor("Serie");
                double preço = 0;

                objListLin = PriEngine.Engine.Consulta("SELECT idCabecDoc, Artigo, Descricao, Quantidade, Unidade, PrecUnit, Desconto1, TotalILiquido, PrecoLiquido from LinhasDoc where IdCabecDoc='" + dv.id + "' order By NumLinha");
                listlindv = new List<Model.LinhaDocVenda>();

                while (!objListLin.NoFim())
                {
                    lindv = new Model.LinhaDocVenda();
                    lindv.IdCabecDoc = objListLin.Valor("idCabecDoc");
                    lindv.CodArtigo = objListLin.Valor("Artigo");
                    lindv.DescArtigo = objListLin.Valor("Descricao");
                    lindv.Quantidade = objListLin.Valor("Quantidade");
                    lindv.Unidade = objListLin.Valor("Unidade");
                    lindv.Desconto = objListLin.Valor("Desconto1");
                    lindv.PrecoUnitario = objListLin.Valor("PrecUnit");
                    lindv.TotalILiquido = objListLin.Valor("TotalILiquido");
                    lindv.TotalLiquido = objListLin.Valor("PrecoLiquido");
                    preço += lindv.TotalILiquido - lindv.Desconto;

                    listlindv.Add(lindv);
                    objListLin.Seguinte();
                }

                dv.PrecoFinal = preço;
                dv.LinhasDoc = listlindv;
                return dv;
            }
            return null;
        }

        public static List<Model.DocVenda> Encomenda_GetByCliente(string cliente)
        {
            ErpBS objMotor = new ErpBS();

            StdBELista objListCab;
            StdBELista objListLin;
            Model.DocVenda dv = new Model.DocVenda();
            List<Model.DocVenda> listdv = new List<Model.DocVenda>();
            Model.LinhaDocVenda lindv = new Model.LinhaDocVenda();
            List<Model.LinhaDocVenda> listlindv = new
            List<Model.LinhaDocVenda>();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {
                objListCab = PriEngine.Engine.Consulta("SELECT id, Entidade, Data, NumDoc, TotalMerc, TotalIva, TotalDesc, Serie  From CabecDoc where TipoDoc='FA' AND Entidade='" + cliente + "'");
                while (!objListCab.NoFim())
                {
                    dv = new Model.DocVenda();
                    dv.id = objListCab.Valor("id");
                    dv.Entidade = objListCab.Valor("Entidade");
                    dv.NumDoc = objListCab.Valor("NumDoc");
                    dv.Data = objListCab.Valor("Data");
                    dv.Serie = objListCab.Valor("Serie");

                    double preço = 0;


                    objListLin = PriEngine.Engine.Consulta("SELECT idCabecDoc, Artigo, Descricao, Quantidade, Unidade, PrecUnit, Desconto1, TotalILiquido, PrecoLiquido from LinhasDoc where IdCabecDoc='" + dv.id + "' order By NumLinha");
                    listlindv = new List<Model.LinhaDocVenda>();

                    while (!objListLin.NoFim())
                    {
                        lindv = new Model.LinhaDocVenda();
                        lindv.IdCabecDoc = objListLin.Valor("idCabecDoc");
                        lindv.CodArtigo = objListLin.Valor("Artigo");
                        lindv.DescArtigo = objListLin.Valor("Descricao");
                        lindv.Quantidade = objListLin.Valor("Quantidade");
                        lindv.Unidade = objListLin.Valor("Unidade");
                        lindv.Desconto = objListLin.Valor("Desconto1");
                        lindv.PrecoUnitario = objListLin.Valor("PrecUnit");
                        lindv.TotalILiquido = objListLin.Valor("TotalILiquido");
                        lindv.TotalLiquido = objListLin.Valor("PrecoLiquido");
                        preço += lindv.TotalILiquido - lindv.Desconto;

                        listlindv.Add(lindv);
                        objListLin.Seguinte();
                    }

                    dv.PrecoFinal = preço;
                    dv.LinhasDoc = listlindv;
                    listdv.Add(dv);
                    objListCab.Seguinte();
                }
            }
            return listdv;
        }

        #endregion DocsVenda


        # region Carrinho

        public static Lib_Primavera.Model.Carrinho GetCarrinhoUser(string id_user)
        {

            StdBELista objListCarrinho;
            StdBELista objList;


            Model.Artigo art = new Model.Artigo();
            Model.Carrinho carr = new Model.Carrinho();
            List<Model.Artigo> listArtigos = new List<Model.Artigo>();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {
                objList = PriEngine.Engine.Consulta("SELECT CDU_idCarrinho , CDU_idCliente, CDU_idProduto FROM  TDU_CarrinhoCompras, TDU_CarrinhoProduto WHERE CDU_idCliente='" + id_user + "' AND CDU_idCarrinho = CDU_idCarrinhoCompras");

                //objList = PriEngine.Engine.Comercial.Artigos.LstArtigos();

                while (!objList.NoFim())
                {
                    carr = new Model.Carrinho();
                    carr.ID = objList.Valor("CDU_idCarrinho").ToString();
                    carr.ID_Cliente = objList.Valor("CDU_idCliente").ToString();
                    String idTemp = objList.Valor("CDU_idProduto").ToString();

                    objListCarrinho = PriEngine.Engine.Consulta("SELECT CDU_Armazem, CDU_Quantidade, CDU_idCarrinhoProduto, CDU_Nome, ARTIGO.Artigo, ArtigoMoeda.Artigo, CDU_Imagem, CDU_Descricao, Desconto, STKActual, PVP1, Familia, SubFamilia, Marca, Modelo FROM  ARTIGO, TDU_CarrinhoProduto, ArtigoMoeda WHERE ARTIGO.Artigo = '" + idTemp + "' AND CDU_idProduto = ARTIGO.Artigo AND ARTIGO.Artigo = ArtigoMoeda.Artigo");

                    while (!objListCarrinho.NoFim())
                    {
                        art = new Model.Artigo();
                        art.ID = objListCarrinho.Valor("artigo");
                        art.DescArtigo = objListCarrinho.Valor("CDU_Descricao");
                        art.Desconto = objListCarrinho.Valor("desconto").ToString();
                        art.STKActual = objListCarrinho.Valor("stkactual").ToString();
                        art.Preço = objListCarrinho.Valor("PVP1").ToString();
                        art.Familia = objListCarrinho.Valor("familia");
                        art.SubFamilia = objListCarrinho.Valor("subfamilia");
                        art.Marca = objListCarrinho.Valor("marca");
                        art.Modelo = objListCarrinho.Valor("modelo");
                        art.CDU_Imagem = objListCarrinho.Valor("CDU_Imagem");
                        art.Nome = objListCarrinho.Valor("CDU_Nome");
                        art.CDU_idCarrinhoProduto = objListCarrinho.Valor("CDU_idCarrinhoProduto").ToString();
                        art.Quantidade = objListCarrinho.Valor("CDU_Quantidade").ToString();
                        art.Armazem = objListCarrinho.Valor("CDU_Armazem").ToString();
                        listArtigos.Add(art);
                        objListCarrinho.Seguinte();
                    }
                    //falta as imagens
                    carr.ID_Produtos = listArtigos;
                    objList.Seguinte();
                }

                return carr;

            }
            else
            {
                return null;

            }

        }

        public static Lib_Primavera.Model.Carrinho getCarrinhoID(string id_user)
        {

            StdBELista objList;


            Model.Artigo art = new Model.Artigo();
            Model.Carrinho carr = new Model.Carrinho();
            List<Model.Artigo> listArtigos = new List<Model.Artigo>();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {
                objList = PriEngine.Engine.Consulta("SELECT CDU_idCarrinhoCompras, CDU_idCliente FROM  TDU_CarrinhoCompras WHERE CDU_idCliente='" + id_user+ "'");

                //objList = PriEngine.Engine.Comercial.Artigos.LstArtigos();

                while (!objList.NoFim())
                {
                    carr = new Model.Carrinho();
                    carr.ID = objList.Valor("CDU_idCarrinhoCompras").ToString();
                    carr.ID_Cliente = objList.Valor("CDU_idCliente").ToString();
                
                    objList.Seguinte();
                }



                return carr;

            }
            else
            {
                return null;

            }

        }

        public static Lib_Primavera.Model.RespostaErro atualizaCarrinho(Model.TDU_CarrinhoProduto linhaCarrinho) {
            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
            StdBECamposChave tdu_carrinhoChaves = new StdBECamposChave();
            if (Int32.Parse(linhaCarrinho.CDU_Quantidade) < 1)
            {
                erro.Erro = 1;
                erro.Descricao = "Quantidade errada";
                return erro;
            }
            try
            {
                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {
                    tdu_carrinhoChaves.AddCampoChave("CDU_idCarrinhoProduto", linhaCarrinho.CDU_idCarrinhoProduto);
                    tdu_carrinhoChaves.AddCampoChave("CDU_idCarrinho", linhaCarrinho.CDU_idCarrinho);

                    if (PriEngine.Engine.TabelasUtilizador.Existe("TDU_CarrinhoProduto", tdu_carrinhoChaves))
                    {
                        PriEngine.Engine.TabelasUtilizador.ActualizaValorAtributo("TDU_CarrinhoProduto", tdu_carrinhoChaves, "CDU_Quantidade", linhaCarrinho.CDU_Quantidade);
                        PriEngine.Engine.TabelasUtilizador.ActualizaValorAtributo("TDU_CarrinhoProduto", tdu_carrinhoChaves, "CDU_Armazem", linhaCarrinho.CDU_Armazem);
                   
                    }
                    else
                    {
                        erro.Erro = 1;
                        erro.Descricao = "Artigo não existe no carrinho";
                        return erro;
                    }
                    erro.Erro = 0;
                    erro.Descricao = "Sucesso";
                    return erro;
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir empresa";
                    return erro;
                }
            }
            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }
        }

        public static Lib_Primavera.Model.RespostaErro DelArtigoCarrinho(Model.TDU_CarrinhoProduto carrinho)
        {
            StdBELista objList;
            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
            StdBECamposChave tdu_carrinho = new StdBECamposChave();

            try
            {
                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {

                    objList = PriEngine.Engine.Consulta("SELECT CDU_idCarrinhoProduto FROM TDU_CarrinhoProduto WHERE CDU_idProduto = '"+carrinho.CDU_idProduto +"' AND CDU_idCarrinho='" + carrinho.CDU_idCarrinho+ "'");

                    if (!objList.NoFim())
                    {
                        carrinho.CDU_idCarrinhoProduto = objList.Valor("CDU_idCarrinhoProduto").ToString();
                    }
                    tdu_carrinho.AddCampoChave("CDU_idCarrinho", carrinho.CDU_idCarrinho);
                    tdu_carrinho.AddCampoChave("CDU_idCarrinhoProduto", carrinho.CDU_idCarrinhoProduto);
                    tdu_carrinho.AddCampoChave("CDU_idProduto", carrinho.CDU_idProduto);

                    //se forem so estas as chaves da tabela CarrinhoProduto
                    PriEngine.Engine.TabelasUtilizador.Remove("TDU_CarrinhoProduto", tdu_carrinho);
                    erro.Erro = 0;
                    erro.Descricao = "Sucesso";
                    return erro;
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir empresa";
                    return erro;
                }
            }
            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }
        }

        public static Lib_Primavera.Model.RespostaErro InsereCarrinhoObj(Model.TDU_CarrinhoProduto carrinho)
        {
            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
            StdBECamposChave tdu_carrinho = new StdBECamposChave();
            StdBERegistoUtil tdu_carrinhoNovo = new StdBERegistoUtil();
            StdBECampos cmps = new StdBECampos();
            StdBECampo CDU_idCarrinho = new StdBECampo();
            StdBECampo CDU_idCarrinhoProduto = new StdBECampo();
            StdBECampo CDU_idProduto = new StdBECampo();
            StdBELista objList;


            try
            {
                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {

                    objList = PriEngine.Engine.Consulta("SELECT CDU_idCarrinhoProduto FROM TDU_CarrinhoProduto WHERE CDU_idProduto = '"+carrinho.CDU_idProduto +"' AND CDU_idCarrinho='" + carrinho.CDU_idCarrinho+ "'");

                    if (!objList.NoFim())
                    {
                        carrinho.CDU_idCarrinhoProduto = objList.Valor("CDU_idCarrinhoProduto").ToString();

                        tdu_carrinho.AddCampoChave("CDU_idCarrinho", carrinho.CDU_idCarrinho);
                        tdu_carrinho.AddCampoChave("CDU_idCarrinhoProduto", carrinho.CDU_idCarrinhoProduto);
                        tdu_carrinho.AddCampoChave("CDU_idProduto", carrinho.CDU_idProduto);
                   

                    }
                    else { 


                    objList = PriEngine.Engine.Consulta("SELECT MAX(CDU_idCarrinhoProduto) AS max FROM TDU_CarrinhoProduto");

                    //objList = PriEngine.Engine.Comercial.Artigos.LstArtigos();
                    int nextid = 1;
                    string max_str = objList.Valor("max").ToString();

                    if (max_str == "")
                        nextid = 1;
                    else
                    { 
                    int max_sum = Int32.Parse(max_str);
                    while (!objList.NoFim())
                    {
                        nextid += max_sum;
                        objList.Seguinte();
                    }
                    }
                    carrinho.CDU_idCarrinhoProduto = nextid.ToString();

                 
                    CDU_idCarrinho.Nome = "CDU_idCarrinho";
                    CDU_idCarrinhoProduto.Nome = "CDU_idCarrinhoProduto";
                    CDU_idProduto.Nome = "CDU_idProduto";



                    
                    CDU_idCarrinho.Valor = carrinho.CDU_idCarrinho;
                    CDU_idCarrinhoProduto.Valor = carrinho.CDU_idCarrinhoProduto;
                    CDU_idProduto.Valor = carrinho.CDU_idProduto;


                    
                    cmps.Insere(CDU_idProduto);
                    cmps.Insere(CDU_idCarrinho);
                    cmps.Insere(CDU_idCarrinhoProduto);
                    tdu_carrinhoNovo.set_Campos(cmps);
                    PriEngine.Engine.TabelasUtilizador.Actualiza("TDU_CarrinhoProduto", tdu_carrinhoNovo);

                   }
                   
                    erro.Erro = 0;
                    erro.Descricao = "Sucesso";
                    return erro;
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir empresa";
                    return erro;
                }
            }
            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }
        }

        public static List<Model.Carrinho> ListaCarrinhos()
        {


            StdBELista objListCarrinho;
            StdBELista objList;


            Model.Artigo myArt = new Model.Artigo();
            Model.Carrinho carr = new Model.Carrinho();
            List<Model.Carrinho> listCarrinhos = new List<Model.Carrinho>();
            List<Model.Artigo> listArtigos = new List<Model.Artigo>();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {
                objList = PriEngine.Engine.Consulta("SELECT CDU_idCarrinho, CDU_idCliente, CDU_idProduto FROM  TDU_CarrinhoCompras, TDU_CarrinhoProduto");

                //objList = PriEngine.Engine.Comercial.Artigos.LstArtigos();

                while (!objList.NoFim())
                {
                    carr = new Model.Carrinho();
                    carr.ID = objList.Valor("CDU_idCarrinho").ToString();
                    carr.ID_Cliente = objList.Valor("CDU_idCliente").ToString();
                    string idTemp = objList.Valor("CDU_idProduto");

                    objListCarrinho = PriEngine.Engine.Consulta("SELECT Artigo, Descricao, Desconto, STKActual, PCPadrao, Familia, SubFamilia, Marca, Modelo FROM  ARTIGO WHERE Artigo='" + idTemp + "'");
                    listArtigos = new List<Model.Artigo>();

                    while (!objListCarrinho.NoFim())
                    {

                        myArt.ID = objListCarrinho.Valor("Artigo");
                        float desconto = objListCarrinho.Valor("Desconto");
                        myArt.Desconto = desconto.ToString();
                        myArt.DescArtigo = objListCarrinho.Valor("Descricao");
                        double stokeAtual = objListCarrinho.Valor("STKActual");
                        myArt.STKActual = stokeAtual.ToString();
                        double preco = objListCarrinho.Valor("PCPadrao");
                        myArt.Preço = preco.ToString();
                        myArt.Familia = objListCarrinho.Valor("Familia");
                        myArt.SubFamilia = objListCarrinho.Valor("SubFamilia");
                        myArt.Marca = objListCarrinho.Valor("Marca");
                        myArt.Modelo = objListCarrinho.Valor("Modelo");

                        listArtigos.Add(myArt);
                        objListCarrinho.Seguinte();
                    }
                    //falta as imagens
                    carr.ID_Produtos = listArtigos;
                    listCarrinhos.Add(carr);
                    objList.Seguinte();
                }

                /*
                 Consulta("SELECT CDU_idCarrinho, CDU_idCliente, CDU_idCarrinhoCompras, Artigo, Descricao, Desconto, STKActual, PCPadrao, Familia, SubFamilia, Marca, Modelo FROM  ARTIGO, TDU_idCarrinhoCompras, TDU_CarrinhoProduto WHERE CDU_idProduto = Artigo");

                 * 
                 * */


                return listCarrinhos;

            }
            else
            {
                return null;

            }

        }

        public static Lib_Primavera.Model.RespostaErro DelAllCarrinho(Model.TDU_CarrinhoProduto carrinho)
        {
            StdBELista objList;
            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
            StdBECamposChave tdu_carrinho = new StdBECamposChave();

            try
            {
                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {

                    objList = PriEngine.Engine.Consulta("SELECT * FROM TDU_CarrinhoProduto WHERE CDU_idCarrinho = '" + carrinho.CDU_idCarrinho + "'");

                    while (!objList.NoFim())
                    {
                        String temp1, temp2, temp3;
                        temp1 = carrinho.CDU_idCarrinho;
                        temp2 = objList.Valor("CDU_idCarrinhoProduto").ToString();
                        temp3 = objList.Valor("CDU_idProduto").ToString();
                        tdu_carrinho.AddCampoChave("CDU_idCarrinho", carrinho.CDU_idCarrinho);
                        tdu_carrinho.AddCampoChave("CDU_idCarrinhoProduto", objList.Valor("CDU_idCarrinhoProduto").ToString());
                        tdu_carrinho.AddCampoChave("CDU_idProduto", objList.Valor("CDU_idProduto").ToString());

                        //se forem so estas as chaves da tabela CarrinhoProduto
                        PriEngine.Engine.TabelasUtilizador.Remove("TDU_CarrinhoProduto", tdu_carrinho);
                    }
                   
                    erro.Erro = 0;
                    erro.Descricao = "Sucesso";
                    return erro;
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir empresa";
                    return erro;
                }
            }
            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }
        }

        public static List<Lib_Primavera.Model.Armazem> ListaArmazens()
        {

            {
                StdBELista objList;

                List<Model.Armazem> listArmazens = new List<Model.Armazem>();

                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {

                    //objList = PriEngine.Engine.Comercial.Clientes.LstClientes();

                    objList = PriEngine.Engine.Consulta("SELECT Armazem, Armazens.Descricao, Distritos.Descricao as Distrito, Pais  FROM  Armazens, Distritos WHERE Armazens.Distrito = Distritos.Distrito");


                    while (!objList.NoFim())
                    {
                        listArmazens.Add(new Model.Armazem
                        {
                            ID = objList.Valor("Armazem"),
                            Descrição = objList.Valor("Descricao"),
                            Localidade = objList.Valor("Distrito"),
                            Pais = objList.Valor("Pais")

                        });
                        objList.Seguinte();

                    }

                    return listArmazens;
                }
                else
                    return null;

            }
        }

        #endregion Carrinho


        # region Pagamento

        public static Lib_Primavera.Model.Pagamento GetPagamentoUser(string id_user)
        {

            StdBELista objListCarrinho;
            StdBELista objList;


            Model.Artigo art = new Model.Artigo();
            Model.Pagamento pag = new Model.Pagamento();

            List<Model.Artigo> listArtigos = new List<Model.Artigo>();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {
                objList = PriEngine.Engine.Consulta("SELECT ModoPag, CDU_idCarrinho, CDU_idCliente, CDU_idProduto FROM  CLIENTES, TDU_CarrinhoCompras, TDU_CarrinhoProduto  WHERE CDU_idCliente='" + id_user + "' AND Cliente='" + id_user + "' AND CDU_idCarrinho = CDU_idCarrinhoCompras");

                //objList = PriEngine.Engine.Comercial.Artigos.LstArtigos();

                while (!objList.NoFim())
                {
                    pag = new Model.Pagamento();
                    pag.ID_Carrinho = objList.Valor("CDU_idCarrinho").ToString();
                    pag.ID_Cliente = objList.Valor("CDU_idCliente").ToString();
                    pag.ModoPagamento = objList.Valor("ModoPag").ToString();
                    String idTemp = objList.Valor("CDU_idProduto").ToString();

                    objListCarrinho = PriEngine.Engine.Consulta(" SELECT Artigo, Descricao, Desconto, STKActual, PCPadrao, Familia, SubFamilia, Marca, Modelo FROM  ARTIGO WHERE Artigo = '" + idTemp + "'");
                    listArtigos = new List<Model.Artigo>();

                    while (!objListCarrinho.NoFim())
                    {
                        art = new Model.Artigo();
                        art.ID = objListCarrinho.Valor("artigo");
                        art.DescArtigo = objListCarrinho.Valor("descricao");
                        art.Desconto = objListCarrinho.Valor("desconto").ToString();
                        art.STKActual = objListCarrinho.Valor("stkactual").ToString();
                        art.Preço = objListCarrinho.Valor("pcpadrao").ToString();
                        art.Familia = objListCarrinho.Valor("familia");
                        art.SubFamilia = objListCarrinho.Valor("subfamilia");
                        art.Marca = objListCarrinho.Valor("marca");
                        art.Modelo = objListCarrinho.Valor("modelo");

                        listArtigos.Add(art);
                        objListCarrinho.Seguinte();
                    }
                    //falta as imagens
                    pag.ID_Produtos = listArtigos;
                    float tempTotal = 0;
                    for (int i = 0; i < pag.ID_Produtos.Count; i++)
                        tempTotal += float.Parse(pag.ID_Produtos.ElementAt(i).Preço);

                    pag.total = tempTotal.ToString();
                    objList.Seguinte();
                }

                /*
                 Consulta("SELECT CDU_idCarrinho, CDU_idCliente, CDU_idCarrinhoCompras, Artigo, Descricao, Desconto, STKActual, PCPadrao, Familia, SubFamilia, Marca, Modelo FROM  ARTIGO, TDU_idCarrinhoCompras, TDU_CarrinhoProduto WHERE CDU_idProduto = Artigo");

                 * 
                 * */


                return pag;

            }
            else
            {
                return null;

            }

        }


        #endregion Pagamento


        # region Wishlist

        public static Lib_Primavera.Model.Wishlist GetWishlistUser(string id_user)
        {

            StdBELista objList;
            StdBELista objProdutos;


            Model.Artigo art = new Model.Artigo();
            Model.Wishlist wish = new Model.Wishlist();

            List<Model.Artigo> listArtigos = new List<Model.Artigo>();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {
                objList = PriEngine.Engine.Consulta("SELECT CDU_idWishlist FROM TDU_Wishlist WHERE CDU_idUtilizador='" + id_user + "' ");



                while (!objList.NoFim())
                {

                    wish.idClient = id_user;
                    wish.idWishlist = objList.Valor("CDU_idWishlist").ToString();

                    objProdutos = PriEngine.Engine.Consulta(" SELECT ARTIGO.Artigo, ArtigoMoeda.Artigo, CDU_Imagem, desconto, STKActual, CDU_Descricao, PVP1, Familia, SubFamilia, Marca, CDU_Nome, Modelo FROM  ARTIGO,ArtigoMoeda, TDU_WishlistProduto WHERE ARTIGO.Artigo = CDU_Produto AND ARTIGO.Artigo = ArtigoMoeda.Artigo AND CDU_Wishlist = '" + wish.idWishlist + "'");      
                    listArtigos = new List<Model.Artigo>();

                    while (!objProdutos.NoFim())
                    {
                        art = new Model.Artigo();
                        art.ID = objProdutos.Valor("artigo");
                        art.DescArtigo = objProdutos.Valor("CDU_Descricao");
                        art.Desconto = objProdutos.Valor("desconto").ToString();
                        art.Preço = objProdutos.Valor("PVP1").ToString();
                        art.STKActual = objProdutos.Valor("STKActual").ToString();
                        art.CDU_Imagem = objProdutos.Valor("CDU_Imagem");
                        art.Familia = objProdutos.Valor("familia");
                        art.SubFamilia = objProdutos.Valor("subfamilia");
                        art.Marca = objProdutos.Valor("marca");
                        art.Modelo = objProdutos.Valor("modelo");
                        art.Nome = objProdutos.Valor("cdu_nome");

                        listArtigos.Add(art);
                        objProdutos.Seguinte();
                    }
                    //falta as imagens
                    wish.ID_Produtos = listArtigos;
                    objList.Seguinte();
                }

                return wish;

            }
            else
            {
                return null;

            }

        }

        public static Lib_Primavera.Model.RespostaErro DelArtigoWishlist(Model.TDU_WishlistProduto wishLinha)
        {

            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
            StdBECamposChave tdu_wish = new StdBECamposChave();

            try
            {
                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {
                    tdu_wish.AddCampoChave("CDU_idWishlist", wishLinha.CDU_idWishlist);
                    tdu_wish.AddCampoChave("CDU_idProduto", wishLinha.CDU_idProduto);

                    //se forem so estas as chaves da tabela CarrinhoProduto
                    PriEngine.Engine.TabelasUtilizador.Remove("TDU_WishlistProduto", tdu_wish);
                    erro.Erro = 0;
                    erro.Descricao = "Sucesso";
                    return erro;
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir empresa";
                    return erro;
                }
            }
            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }
        }

        public static Lib_Primavera.Model.RespostaErro InsereWishlistObj(Model.TDU_WishlistProduto wishLinha)
        {
            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
            StdBECamposChave tdu_wish = new StdBECamposChave();
            StdBERegistoUtil tdu_wishNovo = new StdBERegistoUtil();
            StdBECampos cmps = new StdBECampos();
            StdBECampo CDU_idWishlist = new StdBECampo();
            StdBECampo CDU_idProduto = new StdBECampo();
            StdBECampo CDU_idWishlistProduto = new StdBECampo();
            StdBELista objList;


            try
            {
                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {
                    objList = PriEngine.Engine.Consulta("SELECT COUNT(*) AS max FROM TDU_WishlistProduto");

                    //objList = PriEngine.Engine.Comercial.Artigos.LstArtigos();
                    int nextid = 1;
                    if (objList != null)
                    {
                        nextid += objList.Valor("max");
                        objList.Seguinte();
                    }

                    CDU_idWishlist.Nome = "CDU_Wishlist";


                     
                    CDU_idProduto.Nome = "CDU_Produto";
                    CDU_idWishlistProduto.Nome = "CDU_WishlistProduto";



                    CDU_idWishlist.Valor = wishLinha.CDU_idWishlist;
                    CDU_idProduto.Valor = wishLinha.CDU_idProduto;
                    CDU_idWishlistProduto.Valor = nextid;


                    cmps.Insere(CDU_idProduto);
                    cmps.Insere(CDU_idWishlist);
                    cmps.Insere(CDU_idWishlistProduto);
                    tdu_wishNovo.set_Campos(cmps);
                    PriEngine.Engine.TabelasUtilizador.Actualiza("TDU_WishlistProduto", tdu_wishNovo);



                    erro.Erro = 0;
                    erro.Descricao = "Sucesso";
                    return erro;
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir empresa";
                    return erro;
                }
            }
            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }
        }


        #endregion Wishlist


        # region Search
        public static List<Model.Artigo> SearchArtigosNome(string id)
        {
            StdBELista objList;
            List<Model.Artigo> listArtigos = new List<Model.Artigo>();
            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {


                objList = PriEngine.Engine.Consulta("SELECT DISTINCT CDU_Nome, ARTIGO.Artigo, ArtigoMoeda.Artigo,  CDU_Descricao, Desconto, STKActual, PCPadrao, Familia, SubFamilia, Marca, Modelo,  CDU_Imagem, PVP1 FROM  ARTIGO, ArtigoMoeda  WHERE ARTIGO.Artigo = ArtigoMoeda.Artigo AND (CDU_Descricao LIKE '%" + id + "%' OR Marca LIKE '%" + id + "%' OR CDU_Nome LIKE '%" + id + "%')");
       
                //objList = PriEngine.Engine.Comercial.Artigos.LstArtigos();


                while (!objList.NoFim())
                {
                    listArtigos.Add(new Model.Artigo
                    {
                        ID = objList.Valor("artigo"),
                        Descricao = objList.Valor("CDU_Descricao"),
                        Desconto = objList.Valor("desconto").ToString(),
                        STKActual = objList.Valor("stkactual").ToString(),
                        Preço = Math.Round(Convert.ToDouble(objList.Valor("PVP1")) * 1.23, 2).ToString(),
                        Familia = objList.Valor("familia"),
                        SubFamilia = objList.Valor("subfamilia"),
                        Marca = objList.Valor("marca"),
                        Modelo = objList.Valor("modelo"),
                        CDU_Imagem = objList.Valor("CDU_Imagem"),
                        Nome = objList.Valor("CDU_Nome")

                    });
                    objList.Seguinte();
                }


                return listArtigos;
            }
            else
                return null;
        }

        #endregion Search


        # region Banir
        public static Lib_Primavera.Model.RespostaErro Ban(Model.UserBan cliente)
        {
            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();


            GcpBECliente objCli = new GcpBECliente();


            try
            {

                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {

                    if (PriEngine.Engine.Comercial.Clientes.Existe(cliente.ID) == false)
                    {
                        erro.Erro = 1;
                        erro.Descricao = "O cliente não existe";
                        return erro;
                    }
                    else
                    {

                        if(cliente.Action == "TRUE")
                        PriEngine.Engine.Comercial.Clientes.ActualizaValorAtributo(cliente.ID, "ClienteAnulado", 1);
                        else if (cliente.Action == "FALSE")
                            PriEngine.Engine.Comercial.Clientes.ActualizaValorAtributo(cliente.ID, "ClienteAnulado", 0);

                        erro.Erro = 0;
                        erro.Descricao = "Sucesso";
                        return erro;
                    }
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir a empresa";
                    return erro;

                }

            }

            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }
        }

        #endregion Banir


        # region Comentario


        public static Lib_Primavera.Model.RespostaErro DelComentario(Model.TDU_Comentario com)
        {
            StdBELista objList;
            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
            StdBECamposChave tdu_comentario = new StdBECamposChave();

            try
            {
                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {

                    tdu_comentario.AddCampoChave("CDU_idComentario", com.CDU_idComentario);

                    //se forem so estas as chaves da tabela CarrinhoProduto
                    PriEngine.Engine.TabelasUtilizador.Remove("TDU_Comentario", tdu_comentario);
                    erro.Erro = 0;
                    erro.Descricao = "Sucesso";
                    return erro;
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir empresa";
                    return erro;
                }
            }
            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }
        }

        public static Lib_Primavera.Model.RespostaErro InsereComentarioObj(Model.TDU_Comentario com)
        {
            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
            StdBERegistoUtil tdu_comentario = new StdBERegistoUtil();
            StdBECampos cmps = new StdBECampos();
            StdBECampo CDU_idComentario = new StdBECampo();
            StdBECampo CDU_idUtilizador = new StdBECampo();
            StdBECampo CDU_Conteudo = new StdBECampo();
            StdBECampo CDU_idProduto = new StdBECampo();
            StdBELista objList;


            try
            {
                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {


                    objList = PriEngine.Engine.Consulta("SELECT COUNT(*) AS numC FROM TDU_Comentario");
                        //objList = PriEngine.Engine.Comercial.Artigos.LstArtigos();
                        int nextid = 1;
                        if (objList != null)
                        {
                            nextid += objList.Valor("numC");
                            objList.Seguinte();
                        }
                        com.CDU_idComentario = nextid.ToString();

                        CDU_idComentario.Nome = "CDU_idComentario";
                        CDU_idUtilizador.Nome = "CDU_idUtilizador";
                        CDU_idProduto.Nome = "CDU_idProduto";
                        CDU_Conteudo.Nome = "CDU_Conteudo";



                        CDU_idComentario.Valor = com.CDU_idComentario;
                        CDU_idUtilizador.Valor = com.CDU_idUtilizador;
                        CDU_idProduto.Valor = com.CDU_idProduto;
                        CDU_Conteudo.Valor = com.CDU_Conteudo;


                        cmps.Insere(CDU_idComentario);
                        cmps.Insere(CDU_idProduto);
                        cmps.Insere(CDU_Conteudo);
                        cmps.Insere(CDU_idUtilizador);
                        tdu_comentario.set_Campos(cmps);
                        PriEngine.Engine.TabelasUtilizador.Actualiza("TDU_Comentario", tdu_comentario);

                    

                    erro.Erro = 0;
                    erro.Descricao = "Sucesso";
                    return erro;
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir empresa";
                    return erro;
                }
            }
            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }
        }

        public static List<Model.TDU_Comentario> ListaComentarios(String id)
        {

            StdBELista objList;

            Model.TDU_Comentario com = new Model.TDU_Comentario();
            List<Model.TDU_Comentario> listComs = new List<Model.TDU_Comentario>();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {
                objList = PriEngine.Engine.Consulta("SELECT CDU_idProduto, CDU_idComentario, CDU_Conteudo FROM TDU_Comentario WHERE CDU_idProduto ='" + id + "'");

                //objList = PriEngine.Engine.Comercial.Artigos.LstArtigos();

                while (!objList.NoFim())
                {
                    com = new Model.TDU_Comentario();
                    com.CDU_idProduto = objList.Valor("CDU_idProduto").ToString();
                    com.CDU_idComentario = objList.Valor("CDU_idComentario").ToString();
                    com.CDU_Conteudo = objList.Valor("CDU_Conteudo").ToString();

                    listComs.Add(com);
                    objList.Seguinte();
                }

                return listComs;

            }
            else
            {
                return null;

            }

        }


        #endregion Comentario

    }
}