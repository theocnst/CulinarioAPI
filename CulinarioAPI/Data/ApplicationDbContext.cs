using CulinarioAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CulinarioAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<UserCredentials> UserCredentials { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Instruction> Instructions { get; set; }
        public DbSet<NutritionInfo> NutritionInfos { get; set; }
        public DbSet<LikedRecipe> LikedRecipes { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // UserCredentials and UserProfile
            modelBuilder.Entity<UserCredentials>()
                .HasOne(uc => uc.UserProfile)
                .WithOne(up => up.UserCredentials)
                .HasForeignKey<UserProfile>(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Recipe and Ingredients
            modelBuilder.Entity<Recipe>()
                .HasMany(r => r.Ingredients)
                .WithOne(i => i.Recipe)
                .HasForeignKey(i => i.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Recipe and Instructions
            modelBuilder.Entity<Recipe>()
                .HasMany(r => r.Instructions)
                .WithOne(i => i.Recipe)
                .HasForeignKey(i => i.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Recipe and NutritionInfo
            modelBuilder.Entity<Recipe>()
                .HasOne(r => r.NutritionInfo)
                .WithOne(n => n.Recipe)
                .HasForeignKey<NutritionInfo>(n => n.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            // UserCredentials and Recipes (Admin relationship)
            modelBuilder.Entity<UserCredentials>()
                .HasMany(uc => uc.Recipes)
                .WithOne(r => r.Admin)
                .HasForeignKey(r => r.AdminId)
                .OnDelete(DeleteBehavior.Cascade);

            // UserProfile and Ratings
            modelBuilder.Entity<UserProfile>()
                .HasMany(up => up.Ratings)
                .WithOne(rt => rt.UserProfile)
                .HasForeignKey(rt => rt.UserProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            // UserProfile and Friends
            modelBuilder.Entity<UserProfile>()
                .HasMany(up => up.Friends)
                .WithOne(f => f.UserProfile)
                .HasForeignKey(f => f.UserProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friend>()
                .HasOne(f => f.FriendUserProfile)
                .WithMany()
                .HasForeignKey(f => f.FriendUserProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            // Recipe and Ratings
            modelBuilder.Entity<Recipe>()
                .HasMany(r => r.Ratings)
                .WithOne(rt => rt.Recipe)
                .HasForeignKey(rt => rt.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            // UserProfile and LikedRecipes
            modelBuilder.Entity<UserProfile>()
                .HasMany(up => up.LikedRecipes)
                .WithOne(lr => lr.UserProfile)
                .HasForeignKey(lr => lr.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Recipe and LikedRecipes
            modelBuilder.Entity<LikedRecipe>()
                .HasOne(lr => lr.Recipe)
                .WithMany()
                .HasForeignKey(lr => lr.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
