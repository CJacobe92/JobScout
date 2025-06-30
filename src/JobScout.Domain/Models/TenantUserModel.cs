using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScout.Domain.Models
{

    public class TenantUserModel(string firstName, string lastName, string email, string userName, Guid tenantId)
    {
        public Guid Id { get; init; }
        public string FirstName { get; private set; } = firstName;
        public string LastName { get; private set; } = lastName;
        public string Email { get; private set; } = email;
        public string UserName { get; private set; } = userName;
        public DateTime? CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public Guid TenantId { get; private set; } = tenantId;

        //Response data model
        public TenantUserModel(
            Guid id,
            string firstName,
            string lastName,
            string username,
            string email,
            DateTime createdAt,
            DateTime updatedAt,
            Guid tenantId) : this(firstName, lastName, username, email, tenantId)
        {
            Id = id;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public void Update(
            string? firstName,
            string? lastName,
            string? email,
            string? userName,
            Guid? tenantId)
        {
            if (!string.IsNullOrWhiteSpace(firstName))
                FirstName = firstName;

            if (!string.IsNullOrWhiteSpace(lastName))
                LastName = lastName;

            if (!string.IsNullOrWhiteSpace(email))
                Email = email;

            if (!string.IsNullOrWhiteSpace(userName))
                UserName = userName;

            if (tenantId.HasValue)
                TenantId = tenantId.Value;
        }

    }
}
