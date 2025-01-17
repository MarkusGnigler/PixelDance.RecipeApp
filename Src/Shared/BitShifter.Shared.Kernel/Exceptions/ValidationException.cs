﻿using System;
using System.Linq;
using System.Collections.Generic;
//using FluentValidation.Results;

namespace BitShifter.Shared.Kernel.Exceptions
{
    public class ValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationException()
            : base("One or more validation failures have occurred.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        //public ValidationException(IEnumerable<ValidationFailure> failures)
        //    : this()
        //{
        //    Errors = failures
        //        .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
        //        .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        //}
    }
}