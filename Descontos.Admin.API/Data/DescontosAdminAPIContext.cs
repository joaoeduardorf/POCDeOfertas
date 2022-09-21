using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Descontos.Admin.API.Models;

namespace Descontos.Admin.API.Data
{
    public class DescontosAdminAPIContext : DbContext
    {
        public DescontosAdminAPIContext (DbContextOptions<DescontosAdminAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Descontos.Admin.API.Models.Desconto> Desconto { get; set; } = default!;
    }
}
