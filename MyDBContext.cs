using Microsoft.EntityFrameworkCore;
using lab1_gr1.Models;
using ListaZakupow.Model.DataModels;

namespace lab1_gr1.Models
{
    public class MyDBContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Recipe> Recipes { get; set; }
        public virtual DbSet<Ingredient> Ingredients { get; set; }
        public virtual DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public virtual DbSet<RecipeSchedule> RecipeSchedules { get; set; }
        public virtual DbSet<ShoppingList> ShoppingLists { get; set; }
        public virtual DbSet<ShoppingListItem> ShoppingListItems { get; set; }

        public MyDBContext(DbContextOptions<MyDBContext> options) : base(options) { }

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
                .OnDelete(DeleteBehavior.Restrict); // zamiast Cascade

            modelBuilder.Entity<RecipeSchedule>()
                .HasOne(rs => rs.Recipe)
                .WithMany(r => r.RecipeSchedules)
                .HasForeignKey(rs => rs.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
