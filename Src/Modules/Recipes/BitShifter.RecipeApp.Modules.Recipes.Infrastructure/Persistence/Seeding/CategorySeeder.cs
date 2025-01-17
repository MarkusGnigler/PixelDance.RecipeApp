﻿using System.Linq;
using System.Threading.Tasks;
using BitShifter.Modules.Recipes.Domain.Entities;

namespace BitShifter.Modules.Recipes.Infrastructure.Persistence.Seeding
{
    internal static class CategorySeeder
    {
        public static string CategoryToSeed { get; } = "Nuddelgerichte";

        public async static Task SeedCategory(ApplicationDbContext context)
        {
            if (context.Categories.Any()) return;

            var category = new Category(CategoryToSeed);

            await context.Categories.AddAsync(category);

            await context.SaveChangesAsync();
        }
    }
}
