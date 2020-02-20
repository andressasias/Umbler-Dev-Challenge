using System;
using Moq;
using Desafio.Umbler;
using Desafio.Umbler.Models;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XUnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void SearchInWhoisAsync()
        {
            Domain domain = new Domain()
            {
                Name = "umbler.com",
                Ip = "187.84.237.146",
                UpdatedAt = DateTime.Now,
                WhoIs = "Domain Name: UMBLER.COM\nRegistry Domain ID: 1805958813_DOMAIN_COM - VRSN\nRegistrar WHOIS Server: whois.publicdomainregistry.com\nRegistrar URL: www.publicdomainregistry.com\nUpdated Date: 2019 - 04 - 16T14: 20:33Z\nCreation Date: 2013 - 06 - 03T20: 42:15Z\nRegistrar Regi",
                Ttl = 60,
                HostedAt = "Umbler Provedor Internet LTDA",
            };

            string domainName = "umbler.com.br";

            Mock<SearchWhois> mock = new Mock<SearchWhois>();
            mock.Setup(m => m.SearchInWhoisAsync(domainName)).Returns(Task.FromResult(domain));

            SearchWhois verif = new SearchWhois();

            var resultadoEsperado = mock.Object.SearchInWhoisAsync(domainName);
            var resultado = verif.SearchInWhoisAsync(domainName);
            
            Assert.AreEqual(resultado, resultadoEsperado);
        }
    }
}







