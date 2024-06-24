using WebsupplyHHemo.Interface.Metodos;

namespace WebsupplyHHemo.Teste
{
    [TestClass]
    public class Testes
    {
        [TestMethod]
        public void InterfaceCentroCusto()
        {
            CentroCustoMetodo centroCusto = new CentroCustoMetodo();

            Console.WriteLine(centroCusto.ConsomeWS());
            Console.WriteLine(centroCusto.strMensagem);
        }

        [TestMethod]
        public void InterfaceCondicaoPagto()
        {
            CondicaoPagtoMetodo condicaoPagto = new CondicaoPagtoMetodo();

            Console.WriteLine(condicaoPagto.ConsomeWS());
            Console.WriteLine(condicaoPagto.strMensagem);
        }

        [TestMethod]
        public void InterfaceFormaPagto()
        {
            FormaPagtoMetodo formaPagto = new FormaPagtoMetodo();

            Console.WriteLine(formaPagto.ConsomeWS());
            Console.WriteLine(formaPagto.strMensagem);
        }

        [TestMethod]
        public void InterfaceFornecedores()
        {
            FornecedorMetodo fornecedorMetodo = new FornecedorMetodo();

            fornecedorMetodo.intCodForWebsupply = 675193;

            Console.WriteLine(fornecedorMetodo.Cadastra());
            Console.WriteLine(fornecedorMetodo.strMensagem);
            Console.WriteLine(fornecedorMetodo.intCodForWebsupply);
            Console.WriteLine(fornecedorMetodo.strCodForProtheus);
        }

        [TestMethod]
        public void InterfaceNatureza()
        {
            NaturezaMetodo naturezaMetodo = new NaturezaMetodo();

            Console.WriteLine(naturezaMetodo.ConsomeWS());
            Console.WriteLine(naturezaMetodo.strMensagem);
        }

        [TestMethod]
        public void InterfacePlanoConta()
        {
            PlanoContaMetodo planoConta = new PlanoContaMetodo();

            Console.WriteLine(planoConta.ConsomeWS());
            Console.WriteLine(planoConta.strMensagem);
        }

        [TestMethod]
        public void InterfaceTipoOperacao()
        {
            TipoOperacaoMetodo TipoOperacao = new TipoOperacaoMetodo();

            Console.WriteLine(TipoOperacao.ConsomeWS());
            Console.WriteLine(TipoOperacao.strMensagem);
        }

        [TestMethod]
        public void InterfaceUnidadesMedida()
        {
            UnidadeMedidaMetodo unidadeMedida = new UnidadeMedidaMetodo();

            Console.WriteLine(unidadeMedida.ConsomeWS());
            Console.WriteLine(unidadeMedida.strMensagem);
        }

        [TestMethod]
        public void InterfaceUnidadesFiliais()
        {
            UnidadesFiliaisMetodo unidadesFiliaisMetodo = new UnidadesFiliaisMetodo();

            Console.WriteLine(unidadesFiliaisMetodo.ConsomeWS());
            Console.WriteLine(unidadesFiliaisMetodo.strMensagem);
        }

        [TestMethod]
        public void InterfaceUsuarios()
        {
            UsuarioMetodo usuarioMetodo = new UsuarioMetodo();

            Console.WriteLine(usuarioMetodo.ConsomeWS());
            Console.WriteLine(usuarioMetodo.strMensagem);
        }

        [TestMethod]
        public void InterfaceUsuariosUnidades()
        {
            UsuarioUnidadeMetodo usuarioUnidadeMetodo = new UsuarioUnidadeMetodo();

            Console.WriteLine(usuarioUnidadeMetodo.ConsomeWS());
            Console.WriteLine(usuarioUnidadeMetodo.strMensagem);
        }
    }
}