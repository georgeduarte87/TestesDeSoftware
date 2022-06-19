using System;
using Xunit;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using NerdStore.WebApp.MVC;

namespace NerdStore.WebApp.Tests.Config
{
    [CollectionDefinition(nameof(IntegrationWebFixtureCollection))]
    public class IntegrationWebFixtureCollection : ICollectionFixture<IntegrationTestsFixture<StartupWebTests>> { }


    [CollectionDefinition(nameof(IntegrationApiFixtureCollection))]
    public class IntegrationApiFixtureCollection : ICollectionFixture<IntegrationTestsFixture<StartupApiTests>> { }


    public class IntegrationTestsFixture<TSStartup> : IDisposable where TSStartup : class
    {
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

        public void Dispose()
        {
            Client.Dispose();
            Factory.Dispose();
        }
    }
}
