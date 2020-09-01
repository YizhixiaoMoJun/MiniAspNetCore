﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BookApi.Model;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;

namespace BookApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpGet]
        public BaseResponse Login(string userName,string pwd)
        {
            var userInfo = new UserInfo
            {
                UserName = userName,
                Pwd = pwd
            };
            if (string.IsNullOrEmpty(userInfo.UserName) || string.IsNullOrEmpty(userInfo.Pwd))
            {
                Log.Information("UserName or Password is incorrect");
                return new BaseResponse { Message = "UserName or Password is incorrect", Result = false };        
            }

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
            return new BaseResponse
            {
                Result = true,
                Data = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }
    }
}