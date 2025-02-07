using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeTrackingSystemVerticalSlicingWithCQRS.Helpers;

public class Result<T>
{
    protected internal Result(bool isSuccess, T value, List<string> errors)
    {
        if (isSuccess && errors.Any())
        {
            throw new InvalidOperationException("Cannot create successful result with errors");
        }
        IsSuccess = isSuccess;
        Value = value;
        Errors = errors;
    }

    public bool IsSuccess { get; private set; }
    public T Value { get; private set; }
    public List<string> Errors { get; private set; }

    //private Result(bool isSuccess, T value, List<string> errors)
    //{
    //    IsSuccess = isSuccess;
    //    Value = value;
    //    Errors = errors;
    //}

    public static Result<T> Success(T value)
    {
        return new Result<T>(true, value, new List<string>());
    }

    public static Result<T> Failure(IEnumerable<string> errors)
    {
        return new Result<T>(false, default(T), errors.ToList());
    }

    public static Result<T> Failure(ValidationResult validationResult)
    {
        var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
        return new Result<T>(false, default(T), errors);
    }
}
