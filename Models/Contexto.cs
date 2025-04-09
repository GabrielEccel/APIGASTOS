using APIGastos.Models;
using Microsoft.EntityFrameworkCore;

namespace APIGastos.Data
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {
        }

        public DbSet<Gasto> Gastos { get; set; }
    }
}