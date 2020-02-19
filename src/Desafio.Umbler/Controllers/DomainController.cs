using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Desafio.Umbler.Models;
using Whois.NET;
using Microsoft.EntityFrameworkCore;
using DnsClient;
using Microsoft.Extensions.Logging;


namespace Desafio.Umbler.Controllers
{
    [Route("api")]
    public class DomainController : Controller
    {
        private readonly DatabaseContext _db;       

        public DomainController(DatabaseContext db)
        {
            _db = db;
        }

        [HttpGet, Route("domain/{domainName}")]
        public async Task<IActionResult> Get(string domainName)
        {
            //procura se já tenho esse domínio no banco
            var whois = new SearchWhois();
            var domain = await _db.SearchDomain(_db, domainName);

            //se não encontrou o domínio no banco, pesquisa no whois e salva no banco
            if (domain == null)
            {
                domain = await whois.SearchInWhoisAsync(domainName);
                if(domain.Name == "Error")
                {
                    return BadRequest(domain.WhoIs);
                }
                _db.Domains.Add(domain);
            }

            var hora = DateTime.Now.Subtract(domain.UpdatedAt).TotalMinutes;

            //se já esgotou o ttl do domínio, preciso pesquisar novamente no whois e atualizar no banco.
            if (hora > domain.Ttl)
            {
                var domain2 = await whois.SearchInWhoisAsync(domainName);

                domain.Name = domain2.Name;
                domain.Ip = domain2.Ip;
                domain.UpdatedAt = DateTime.Now;
                domain.WhoIs = domain2.WhoIs;
                domain.Ttl = domain2.Ttl;
                domain.HostedAt = domain2.HostedAt;
            }

            //repasso os dados do domínio para minha ViewModel
            var domainView = new DomainViewModel
            {
                Name = domain.Name,
                Ip = domain.Ip ?? "n/a",
                WhoIs = domain.WhoIs,
                HostedAt = domain.HostedAt ?? "n/a"
            };

            //aguardando atualizar o banco
            await _db.SaveChangesAsync();

            //retornando a view com o domínio
            return Ok(domainView);
        }
    }
}
