using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScout.Domain.Models
{

    public class TenantUser
    {
        public Guid Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string UserName { get; private set; }
        public string? Password { get; private set; }
        public DateTime? CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        public Guid TenantId { get; private set; }

        //Request data model
        public TenantUser(
          string firstName,
          string lastName,
          string email,
          string password,
          Guid tenantId)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            UserName = email;
            Password = password;
            TenantId = tenantId;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        //Response data model
        public TenantUser(
            Guid id,
            string firstName,
            string lastName,
            string email,
            string username,
            Guid tenantId,
            DateTime createdAt,
            DateTime updateAt)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            UserName = username;
            TenantId = tenantId;
            CreatedAt = createdAt;
            UpdatedAt = updateAt;
        }
    }
}
