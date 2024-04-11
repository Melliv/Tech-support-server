using Domain.App;
using Microsoft.EntityFrameworkCore;

namespace DAL.APP.EF;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Request> Request { get; set; } = default!;
}