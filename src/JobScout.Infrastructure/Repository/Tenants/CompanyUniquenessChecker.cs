using System;
using JobScout.Domain.SeedWork;
using JobScout.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace JobScout.Infrastructure.Repository.Tenants;

public class CompanyUniquenessChecker(AppDbContext context) : IUniquenessChecker<string>
{
    private readonly AppDbContext _context = context;

    public async Task<bool> IsUniqueAsync(string companyName) =>
        !await _context.Tenants.AnyAsync(t => t.CompanyName == companyName);
}
