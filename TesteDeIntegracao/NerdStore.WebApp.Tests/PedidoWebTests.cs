using Xunit;
using NerdStore.WebApp.Tests.Config;
using NerdStore.WebApp.MVC;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Net.Http;
using AngleSharp.Html.Parser;
using System.Linq;

namespace NerdStore.WebApp.Tests
{

    [Collection(nameof(IntegrationWebFixtureCollection))]
    public class PedidoWebTests
    {
        private readonly IntegrationTestsFixture<StartupWebTests> _testsFixture;

        public PedidoWebTests(IntegrationTestsFixture<StartupWebTests> testsFixture)
        {
            _testsFixture = testsFixture;
        }


        [Fact(DisplayName = "Adicionar Item em Novo Pedido")]
        [Trait("Categoria", "Integração Web - Pedido")]
        public async Task AdicionarItem_NovoPedido_DeveAtualizarValorTotal()
        {
            // Arrange
            var produdoId = new Guid("f575d8fd-61d4-4853-c563-08da4a1b0193");
            const int quantidade = 2;

            var initialResponse = await _testsFixture.Client.GetAsync($"/produto-detalhe/{produdoId}");
            initialResponse.EnsureSuccessStatusCode();

            var formData = new Dictionary<string, string>
            {
                {"Id", produdoId.ToString() },
                {"quantidade", quantidade.ToString() }
            };

            await _testsFixture.RealizarLoginWeb();


            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/meu-carrinho")
            {
                Content = new FormUrlEncodedContent(formData)
            };

            // Act
            var postResponse = await _testsFixture.Client.SendAsync(postRequest);

            // Usar isso pra ver o que vem dentro do response
            //var teste = await postResponse.Content.ReadAsStringAsync();

            // Assert

            postResponse.EnsureSuccessStatusCode();

            var html = new HtmlParser()
                .ParseDocumentAsync(await postResponse.Content.ReadAsStringAsync())
                .Result
                .All;

            var formQuantidade = html?.FirstOrDefault(c => c.Id == "quantidade").GetAttribute("value").ApenasNumeros();
            var valorUnitario = html?.FirstOrDefault(c => c.Id == "valorUnitario")?.TextContent.Split(".")[0]?.ApenasNumeros();
            var valorTotal = html?.FirstOrDefault(c => c.Id == "valorTotal")?.TextContent.Split(".")[0]?.ApenasNumeros();

            Assert.Equal(valorTotal, valorUnitario * formQuantidade);
        }
    }
}
