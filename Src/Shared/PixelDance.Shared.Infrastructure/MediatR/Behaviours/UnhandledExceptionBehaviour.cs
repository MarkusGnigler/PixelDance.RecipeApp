﻿using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PixelDance.Shared.Infrastructure.MediatR.Behaviours
{
    public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(
            TRequest request, 
            CancellationToken cancellationToken, 
            RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                var requestName = typeof(TRequest).Name;

                Console.WriteLine(
                    $"PixelDance Request: Unhandled Exception for Request {requestName} {request}", 
                    ex.Message);

                throw;
            }
        }
    }
}
