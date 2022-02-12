﻿using Microsoft.AspNetCore.Mvc;

namespace PixelDance.Shared.Infrastructure.Bootstrapper.Filters
{
    internal static class DependencyInjection
    {
        public static MvcOptions AddCoreFilters(this MvcOptions options)
        {
            options.Filters.Add<DomainExceptionFilterAttribute>();
            options.Filters.Add<UnknownExceptionFilterAttribute>();

            return options;
        }
    }
}
