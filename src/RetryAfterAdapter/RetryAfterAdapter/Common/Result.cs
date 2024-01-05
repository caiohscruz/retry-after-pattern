using System;

namespace RetryAfterAdapter.Common
{
    public class Result<TValue, TError>
    {
        /// <summary>
        /// Expected Result of Processing
        /// </summary>
        public readonly TValue? Value;

        /// <summary>
        ///  Processing error
        /// </summary>
        public readonly TError? Error;

        private bool _isSuccess;

        private Result(TValue value)
        {
            _isSuccess = true;
            Value = value;
            Error = default;
        }

        private Result(TError error)
        {
            _isSuccess = false;
            Value = default;
            Error = error;
        }

        // The public static implicit operator Result<TValue, TError>(TValue value) declaration defines an implicit conversion operator.
        // When a value of type TValue is used in a context where a Result<TValue, TError> is expected (for example, in an assignment),
        // the compiler can use this operator to automatically convert TValue to a Result<TValue, TError>. Essentially
        // this simplifies how Result objects are created from TValue values. The same applies to the following statement.

        public static implicit operator Result<TValue, TError>(TValue value) => new Result<TValue, TError>(value);

        public static implicit operator Result<TValue, TError>(TError error) => new Result<TValue, TError>(error);

        /// <summary>
        /// Processes one or another function depending on the success or failure status of this Result
        /// </summary>
        /// <param name="success">
        /// Function to be executed if the Result in question is successful
        /// </param>
        /// <param name="failure">
        /// Function to be executed if the Result in question is a failure
        /// </param>
        /// <returns>
        /// Returns the executed function result
        /// </returns>
        public Result<TValue, TError> Match(Func<TValue, Result<TValue, TError>> success, Func<TError, Result<TValue, TError>> failure)
        {
            if (_isSuccess)
            {
                return success(Value!);
            }
            return failure(Error!);
        }
    }
}
