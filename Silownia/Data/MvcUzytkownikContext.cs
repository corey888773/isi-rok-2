using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Silownia.Models;

namespace MvcUzytkownik.Data
{
    public class MvcUzytkownikContext : DbContext
    {
        public MvcUzytkownikContext (DbContextOptions<MvcUzytkownikContext> options)
            : base(options)
        {
        }

        public DbSet<Silownia.Models.Uzytkownik> Uzytkownik { get; set; } = default!;
        public DbSet<Silownia.Models.Karnet> Karnet { get; set; } = default!;
        public DbSet<Silownia.Models.Wejscie> Wejscie { get; set; } = default!;

    }
}
