using JobScout.Domain.ValueObjects;
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
        public Email Email { get; private set; }
        public UserName UserName { get; private set; }
        public string Password { get; private set; }
        public string? RefreshToken { get; private set; }
        public DateTime? RefreshTokenExpiry { get; private set; }

        public TenantUser(
          string firstName,
          string lastName,
          string email,
          string userName,
          string password,
          string? refreshToken,
          DateTime? refreshTokenExpiry)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = new Email(email);
            UserName = new UserName(userName);
            Password = password;
            RefreshToken = refreshToken;
            RefreshTokenExpiry = refreshTokenExpiry;
        }

        public TenantUser(
            Guid id,
            string firstName,
            string lastName,
            string email,
            string userName,
            string password,
            string? refreshToken,
            DateTime? refreshTokenExpiry)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = new Email(email);
            UserName = new UserName(userName);
            Password = password;
            RefreshToken = refreshToken;
            RefreshTokenExpiry = refreshTokenExpiry;
        }
    }
}
