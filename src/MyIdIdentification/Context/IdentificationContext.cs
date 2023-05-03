using Microsoft.EntityFrameworkCore;
using MyIdIdentification.Entities;

namespace MyIdIdentification.Context;

public class IdentificationContext : DbContext
{
    public DbSet<Document> Documents { get; set; }
    public DbSet<Provider> Providers { get; set; }
    public IdentificationContext(DbContextOptions<IdentificationContext> options) : base(options) { }
}