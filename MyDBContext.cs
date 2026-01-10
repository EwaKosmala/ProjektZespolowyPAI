using ListaZakupow.Model.DataModels;
using Microsoft.EntityFrameworkCore;

namespace lab1_gr1.Models;

/// <summary>
/// Kontekst bazy danych dla aplikacji ListaZakupow.
/// Zawiera DbSet-y dla użytkowników, przepisów, składników, harmonogramów i list zakupów.
/// </summary>
public class MyDBContext : DbContext
{
    /// <summary>Użytkownicy</summary>
    public virtual DbSet<User> Users { get; set; }

    /// <summary>Przepisy</summary>
    public virtual DbSet<Recipe> Recipes { get; set; }

    /// <summary>Składniki</summary>
    public virtual DbSet<Ingredient> Ingredients { get; set; }

    /// <summary>Składniki przypisane do przepisów</summary>
    public virtual DbSet<RecipeIngredient> RecipeIngredients { get; set; }

    /// <summary>Harmonogramy przepisów dla użytkowników</summary>
    public virtual DbSet<RecipeSchedule> RecipeSchedules { get; set; }

    /// <summary>Listy zakupów</summary>
    public virtual DbSet<ShoppingList> ShoppingLists { get; set; }

    /// <summary>Elementy list zakupów (składniki)</summary>
    public virtual DbSet<ShoppingListItem> ShoppingListItems { get; set; }

    /// <summary>
    /// Konstruktor kontekstu z opcjami.
    /// </summary>
    /// <param name="options">Opcje kontekstu</param>
    public MyDBContext(DbContextOptions<MyDBContext> options) : base(options) { }

    /// <summary>
    /// Konfiguracja relacji między encjami oraz kluczy głównych.
    /// </summary>
    /// <param name="modelBuilder">ModelBuilder używany do konfiguracji encji</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RecipeIngredient>()
            .HasKey(ri => new { ri.RecipeId, ri.IngredientId });

        modelBuilder.Entity<ShoppingListItem>()
            .HasKey(sli => new { sli.ShoppingListId, sli.IngredientId });

        modelBuilder.Entity<RecipeSchedule>()
            .HasOne(rs => rs.User)
            .WithMany(u => u.RecipeSchedules)
            .HasForeignKey(rs => rs.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RecipeSchedule>()
            .HasOne(rs => rs.Recipe)
            .WithMany(r => r.RecipeSchedules)
            .HasForeignKey(rs => rs.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
