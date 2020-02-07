using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Desafio.Umbler.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
        {

        }

        public DbSet<Domain> Domains { get; set; }
        public Task<Domain> SearchDomain(DatabaseContext db, string domainName)
        {
            var domain = db.Domains.FirstOrDefaultAsync(d => d.Name == domainName);
            return domain;
        }

    }
}