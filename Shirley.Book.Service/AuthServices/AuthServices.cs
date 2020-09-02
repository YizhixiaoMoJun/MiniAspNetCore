using BookApi.Model;
using Microsoft.IdentityModel.Tokens;
using Shirley.Book.Model;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Shirley.Book.Service.AuthServices
{
    public class AuthServices : IAuthServices
    {
        public async Task<BaseResponse> Login(UserInfo userInfo)
        {
            var claims = new[]
           {
                new Claim(ClaimTypes.Name, userInfo.UserName),
                new Claim(ClaimTypes.NameIdentifier,Guid.NewGuid().ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Const.SecurityKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken
            (
                claims: claims,
                issuer: Const.Domain,
                audience: Const.Domain,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);
            return await Task.FromResult(new BaseResponse<string>
            {
                Result = true,
                Data = new JwtSecurityTokenHandler().WriteToken(token)
            });

        }
    }
}
