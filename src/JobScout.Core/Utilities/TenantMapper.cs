using JobScout.Core.ViewModels;
using JobScout.Domain.Models;

namespace JobScout.Core.Utilities;

public static class TenantMapper
{
    public static TenantViewModel ToViewModel(this TenantModel model) =>
    new(model.Id, model.CompanyName, model.CreatedAt, model.UpdatedAt);

    public static IEnumerable<TenantViewModel> ToViewModel(this IEnumerable<TenantModel> models) =>
    models.Select(m => m.ToViewModel());

}
