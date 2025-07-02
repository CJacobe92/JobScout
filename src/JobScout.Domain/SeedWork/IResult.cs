namespace JobScout.Domain.SeedWork
{
    public interface IResult<T>
    {
        public bool IsSuccess { get; }
        public T? Value { get; }
        public IReadOnlyList<string> Errors { get; }
    }
}