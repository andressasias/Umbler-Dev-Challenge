using System;
using Xunit;    
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq_Demo;
using Desafio.Umbler.Models;

namespace Desafio.Umbler.Test
{
    public class SearchInWhoisAsyncTesteMoq
    {
        private Mock<Domain> _mock;

        [TestMethod]
        public void SearchInWhoisAsync()
        {
            _mock = new Mock<Domain>();
            Domain domain = new Domain()
            {
                Name = "umbler.com",
                Ip = "10.10.10.10",
                UpdatedAt = DateTime.Now,
                WhoIs = "teste",
                Ttl = 300,
                HostedAt = "Umbler",
            };
        }
    }
}
