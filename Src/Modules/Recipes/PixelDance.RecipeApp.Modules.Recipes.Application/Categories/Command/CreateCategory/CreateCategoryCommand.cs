﻿using System;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using MediatR;
using AutoMapper;
using PixelDance.Modules.Recipes.Domain.Entities;
using PixelDance.Shared.Infrastructure.Guards;
using PixelDance.Shared.Abstractions.EfCore.Repository;
using PixelDance.Modules.Recipes.Domain.Specifications;

[assembly: InternalsVisibleTo("PixelDance.Tests.Modules.Recipes.Application")]
namespace PixelDance.Modules.Recipes.Application.Categories.Command.CreateCategory
{
    public class CreateCategory : IRequest<CategoryDto>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    internal class CreateCategoryCommand : IRequestHandler<CreateCategory, CategoryDto>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Category> _repository;

        public CreateCategoryCommand(IMapper mapper, IRepository<Category> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<CategoryDto> Handle(CreateCategory request, CancellationToken cancellationToken)
        {
            Guard.AssertNotNullAndNotEmpty<InvalidOperationException>(request.Name, "Category empty name not allowed.");

            var getByNameSpec = new CategoryByNameSpec(request.Name);
            if (await _repository.AnyAsync(getByNameSpec)) 
                throw new ArgumentException("Category allready exists");

            var category = new Category(request.Name);

            await _repository.AddAsync(category);

            await _repository.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CategoryDto>(category);
        }
    }
}
