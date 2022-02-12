﻿using System;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using MediatR;
using PixelDance.Shared.Infrastructure.Guards;
using PixelDance.Modules.Recipes.Domain.Entities;
using PixelDance.Shared.Abstractions.EfCore.Repository;
using PixelDance.Modules.Recipes.Domain.Specifications;
using PixelDance.Modules.Recipes.Domain.Exceptions;

[assembly: InternalsVisibleTo("PixelDance.Tests.Modules.Recipes.Application")]
namespace PixelDance.Modules.Recipes.Application.Categories.Command.DeleteCategory
{
    public class DeleteCategory : IRequest<Guid>
    {
        public Guid Id { get; set; }
    }

    internal class DeleteCategoryCommand : IRequestHandler<DeleteCategory, Guid>
    {
        private readonly IRepository<Category> _repository;
        private readonly IRepository<Recipe> _recipeRepository;

        public DeleteCategoryCommand(IRepository<Category> repository, IRepository<Recipe> recipeRepository)
        {
            _repository = repository;
            _recipeRepository = recipeRepository;
        }

        public async Task<Guid> Handle(DeleteCategory request, CancellationToken cancellationToken)
        {
            var entityToDelete = await _repository.GetByIdAsync(request.Id);
            Guard.AssertNotFound(entityToDelete, $"No category with id \"{request.Id}\" found.");

            await ThrowIfCategoryIsInUse(entityToDelete);

            await _repository.DeleteAsync(entityToDelete);

            await _repository.SaveChangesAsync(cancellationToken);

            return entityToDelete.Id;
        }

        private async Task ThrowIfCategoryIsInUse(Category entityToDelete)
        {
            var getRecipsByCategoryName = new RecipeByCategoryNameSpec(entityToDelete.Name);
            bool categoryInUse = await _recipeRepository
                .AnyAsync(getRecipsByCategoryName);
            if (categoryInUse)
                throw new CategoryIsAlreadyInUseException(entityToDelete);

        }
    }
}
