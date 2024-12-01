using Microsoft.EntityFrameworkCore;
public class EstoqueContext : DbContext
{
    public EstoqueContext(DbContextOptions<EstoqueContext> options) : base(options) { }
    public DbSet<Produto> Produto { get; set; }
    public DbSet<Categoria> Categoria { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Produto>()
            .HasOne<Categoria>() 
            .WithMany()           
            .HasForeignKey(p => p.CategoriaId); 
    }
}
