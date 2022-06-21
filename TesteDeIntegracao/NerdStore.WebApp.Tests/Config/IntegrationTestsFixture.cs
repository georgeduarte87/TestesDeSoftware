using System;
using Xunit;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using NerdStore.WebApp.MVC;
using System.Text.RegularExpressions;
using Bogus;

namespace NerdStore.WebApp.Tests.Config
{
    [CollectionDefinition(nameof(IntegrationWebFixtureCollection))]
    public class IntegrationWebFixtureCollection : ICollectionFixture<IntegrationTestsFixture<StartupWebTests>> { }


    [CollectionDefinition(nameof(IntegrationApiFixtureCollection))]
    public class IntegrationApiFixtureCollection : ICollectionFixture<IntegrationTestsFixture<StartupApiTests>> { }


    public class IntegrationTestsFixture<TSStartup> : IDisposable where TSStartup : class
    {
        public string AntiForgeryFieldName = "__RequestVerificationToken";

        public string UsuarioEmail;
        public string UsuarioSenha;

        public readonly LojaAppFactory<TSStartup> Factory;
        public HttpClient Client;

        public IntegrationTestsFixture()
        {

            var clientOptions = new WebApplicationFactoryClientOptions
            {

            };

            Factory = new LojaAppFactory<TSStartup>();
            Client = Factory.CreateClient();
        }

        public void GerarUserSenha()
        {
            var faker = new Faker("pt_BR");
            UsuarioEmail = faker.Internet.Email().ToLower();
            UsuarioSenha = faker.Internet.Password(8, false, "", "@1Ab_");
        }


        public string ObterAntiForgeryToken(string htmlBody)
        {
            var requestVerificationTokenMatch =
                Regex.Match(htmlBody, $@"\<input name=""{AntiForgeryFieldName}"" type=""hidden"" value=""([^""]+)"" \/\>");

            if (requestVerificationTokenMatch.Success)
            {
                return requestVerificationTokenMatch.Groups[1].Captures[0].Value;
            }

            throw new ArgumentException($"Anti forgery token '{AntiForgeryFieldName}' não encontrado no HTML", nameof(htmlBody));
        }

        public void Dispose()
        {
            Client.Dispose();
            Factory.Dispose();
        }
    }
}
