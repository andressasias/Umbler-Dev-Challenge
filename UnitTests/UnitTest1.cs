using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Desafio.Umbler;


namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        //Definição do método de testes (data annotation TestMethod);
        [TestMethod]
        public void SearchInWhoisAsync()
        {
            //definição da string 

            Domain domain = new Domain()
            {
                Name = "umbler.com",
                Ip = "187.84.237.146",
                UpdatedAt = DateTime.Now,
                WhoIs = "Domain Name: UMBLER.COM\nRegistry Domain ID: 1805958813_DOMAIN_COM - VRSN\nRegistrar WHOIS Server: whois.publicdomainregistry.com\nRegistrar URL: www.publicdomainregistry.com\nUpdated Date: 2019 - 04 - 16T14: 20:33Z\nCreation Date: 2013 - 06 - 03T20: 42:15Z\nRegistrar Regi",
                Ttl = 60,
                HostedAt = "Umbler Provedor Internet LTDA",
            };

            string domainName = "umbler.com";


            //Criação do objeto Mock, que utiliza a interface ISearchWhois, sendo, então, fortemente tipado. 
            //Vale ressaltar que o objeto Mock poderia emular qualquer implementação da interface definida, caso existisse mais de uma;
            Mock<ISearchWhois> mock = new Mock<ISearchWhois>();
            //Realização do setup de objeto Mock, que foi definido para esperar um retorno de produto barato. 
            //Note que, independentemente do valor do preço no objeto produtoBarato, o valor de retorno do objeto Mock será sempre a string “Produto barato!”, pois assim foi definido;
            mock.Setup(m => m.SearchInWhoisAsync(domainName)).Returns(domain);

            //Criação do objeto SearchWhois para comparação posterior;
            SearchWhois verif = new SearchWhois();

            //Execução do método SearchInWhoisAsync no objeto Mock;
            var resultadoEsperado = mock.Object.SearchInWhoisAsync(domainName);

            //Execução do método SearchInWhoisAsync no objeto normal;
            var resultado = verif.SearchInWhoisAsync(domainName);
            //Seção assert do teste, que faz com que o teste passe caso os resultados sejam iguais.
            Assert.AreEqual(resultado, resultadoEsperado);

        }
    }
}
