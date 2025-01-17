﻿using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

using BitShifter.Modules.Identity.Domain.AppUsers;

namespace BitShifter.Modules.Identity.Core.Tokenizer
{
    internal class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        private readonly UserManager<AppUser> _userManager;

        public TokenService(IConfiguration config, UserManager<AppUser> userManager)
        {
            string tokenKey = config.GetValue<string>("TokenKey");

            _userManager = userManager;

            _key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(tokenKey));
        }

        public async Task<string> CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);

            claims.AddRange(
                roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
