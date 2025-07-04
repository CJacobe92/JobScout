﻿using JobScout.Domain.SeedWork;
using JobScout.Domain.Tenants.Contracts;
using JobScout.Domain.Tenants.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JobScout.Domain.Tenants
{
    public class Tenant : Entity<TenantId>, IAggregateRoot
    {
        public string CompanyName { get; private set; }
        public string Slug { get; private set; }
        public bool IsActivated { get; private set; }
        public bool WelcomeEmailSent { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        private Tenant(
            string companyName,
            string slug
            )
        {
            this.Id = TenantId.New();
            CompanyName = companyName;
            Slug = slug;
            IsActivated = false;
            WelcomeEmailSent = false;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public static Tenant Create(
            string companyName,
            string slug
            )
        {
            var tenant = new Tenant(companyName, slug);

            tenant.AddDomainEvent(new TenantCreatedEvent(tenant.Id, companyName));

            return tenant;
        }

        public void MarkAsActivated()
        {
            this.IsActivated = true;
            TouchLastUpdated();
        }

        public void MarkAsWelcomeEmailSent()
        {
            this.WelcomeEmailSent = true;
            TouchLastUpdated();
        }

        private void TouchLastUpdated()
        {
            UpdatedAt = DateTime.UtcNow;
        }
        private Tenant() { }
    }
}
