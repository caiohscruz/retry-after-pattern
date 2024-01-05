using RetryAfterAdapter.Common;

namespace RetryAfterAdapter.Interfaces
{
    public interface ProcessorInterface<TValue, TError>
    {
        public Result<TValue,TError> GetStatus { get; }

        public Result<TValue, TError> IsFailed { get; }
    }
}