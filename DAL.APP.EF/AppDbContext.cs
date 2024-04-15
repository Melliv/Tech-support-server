using Domain.App;
using Microsoft.EntityFrameworkCore;

namespace DAL.APP.EF;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Ticket> Ticket { get; set; } = default!;
}