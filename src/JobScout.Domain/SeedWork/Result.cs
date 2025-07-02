namespace JobScout.Domain.SeedWork
{
    public class Result<T> : IResult<T>
    {
        public bool IsSuccess { get; private set; }
        public T? Value { get; private set; }
        public IReadOnlyList<string> Errors { get; private set; } = [];

        private Result(bool isSuccess, T? value, List<string> errors)
        {
            IsSuccess = isSuccess;
            Value = value;
            Errors = errors;
        }

        public static Result<T> Success(T value) =>
            new(true, value, []);

        public static Result<T> Failure(params string[] errors) =>
            new(false, default, errors.ToList());
    }

}
