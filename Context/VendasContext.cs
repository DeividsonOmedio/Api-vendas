using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using tech_test_payment_api.Controllers.Entities;
using tech_test_payment_api.Entities;

namespace tech_test_payment_api.Controllers.Context
{
    public class VendasContext : DbContext
    {
        public VendasContext(DbContextOptions<VendasContext> options) : base(options)
        {

        }
        public DbSet<Vendedor> Vendedor{ get; set; }
        public DbSet<Vendas> Vendas{ get; set; }

    }
}