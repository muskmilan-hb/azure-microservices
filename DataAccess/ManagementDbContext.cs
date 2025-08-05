using Microsoft.EntityFrameworkCore;

namespace Wpm.Management.Api.DataAccess;

public class ManagementDbContext(DbContextOptions<ManagementDbContext> dbContextOptions)
    : DbContext(dbContextOptions)
{
    public DbSet<Pet> Pets { get; set; }
    public DbSet<Breed> Breeds { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Breed>().HasData(
            [
                new Breed(1, "Beagle"),
                new Breed(2, "Indie")
            ]
        );

        modelBuilder.Entity<Pet>().HasData(
            [
                new Pet(){ Id=1, Name="Cookie", Age=5, BreedID = 2},
                new Pet(){ Id=2, Name="Bruno", Age=2, BreedID = 1},
                new Pet(){ Id=3, Name="Maui", Age=10, BreedID = 1},
            ]
        );
    }
}

public static class ManagementDbContextExtensions
{
    public static void EnsureDbIsCreated(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetService<ManagementDbContext>();
        context!.Database.EnsureCreated();
    }
}

public class Pet
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int Age { get; set; }

    public int BreedID { get; set; }
    public Breed Breed { get; set; }
}

public record Breed(int Id, string? Name);
