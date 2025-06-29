using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScout.Domain.Models
{
    public class CoreUser
    {
        public Guid Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public string? RefreshToken { get; private set; }
        public DateTime? RefreshTokenExpiry { get; private set; }

        public CoreUser(
          string firstName,
          string lastName,
          string email,
          string password,
          string? refreshToken,
          DateTime? refreshTokenExpiry)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            UserName = email;
            Password = password;
            RefreshToken = refreshToken;
            RefreshTokenExpiry = refreshTokenExpiry;
        }

        public CoreUser(
            Guid id,
            string firstName,
            string lastName,
            string email,
            string password,
            string? refreshToken,
            DateTime? refreshTokenExpiry)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            UserName = email;
            Password = password;
            RefreshToken = refreshToken;
            RefreshTokenExpiry = refreshTokenExpiry;
        }

    }
}
