using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Desafio.Umbler.Models
{
    interface ISearchWhois
    {
        Task<Domain> SearchInWhoisAsync(string domainName);
    }
}
