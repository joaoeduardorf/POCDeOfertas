using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Precos.Admin.API.Models;

namespace Precos.Admin.API.Data
{
    public class PrecosAdminAPIContext : DbContext
    {
        public PrecosAdminAPIContext (DbContextOptions<PrecosAdminAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Precos.Admin.API.Models.Preco> Preco { get; set; } = default!;
    }
}
