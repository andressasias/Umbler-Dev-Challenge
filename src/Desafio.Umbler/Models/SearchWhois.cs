using Desafio.Umbler.Models;
using DnsClient;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Whois.NET;

namespace Desafio.Umbler.Models
{
    public class SearchWhois
    {
        public async Task<Domain> SearchInWhoisAsync(string domainName)
        {
            Console.WriteLine("Entrei no search Whois");
            Domain domain;
            //busca valores na api do whois
            var response = await WhoisClient.QueryAsync(domainName);
            var lookup = new LookupClient();
            var result = await lookup.QueryAsync(domainName, QueryType.ANY);
            var record = result.Answers.ARecords().FirstOrDefault();
            var address = record?.Address;
            var ip = address?.ToString();
            var hostResponse = await WhoisClient.QueryAsync(ip);

            //verifica se houve erro na pesquisa. Se houver, cria um domínio com nome Error e a mensagem de erro
            if (result.HasError)
            {
                domain = new Domain
                {
                    Name = "Error",
                    WhoIs = result.ErrorMessage
                };
            }
            //se não houve erro, atribui os valores certos ao domínio.
            else
            {
                domain = new Domain
                {
                    Name = domainName,
                    Ip = ip,
                    UpdatedAt = DateTime.Now,
                    WhoIs = response.Raw,
                    Ttl = record?.TimeToLive ?? 0,
                    HostedAt = hostResponse.OrganizationName
                };
            }
            return domain;
        }
    }
}
