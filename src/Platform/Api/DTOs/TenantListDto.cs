namespace Api.Dtos;

public class TenantListDto
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public List<TenantDto> Items { get; set; } = new();
}
