using AutoMapper;
using EStore.Application.Interfaces;
using EStore.Application.IRepositories;
using EStore.Domain.Entities;
using EStore.Domain.EntityDtos.OrderDtos;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Application.Services
{
    public class LoginService:ILoginService
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IMapper _mapper;
        private IConfiguration _Config;
        public LoginService(ILoginRepository loginRepository, IMapper mapper, IConfiguration config)
        {
            _loginRepository = loginRepository;
            _mapper = mapper;
            _Config = config;
        }

        public Task<User> AuthenticateUser(LoginReq loginDetails)
        {
            var logindto=_mapper.Map<User>(loginDetails);   
            var user = _loginRepository.GetUserByCredentails(logindto.Email,logindto.PasswordHash);

            if (user == null)
                return null;
            return user;
        }

        

        public string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_Config["Jwt:Issuer"], _Config["Jwt:Audience"], null, expires: DateTime.Now.AddMinutes(300), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);

        }
        public async Task<string> ProvideToken(LoginReq login)
        {
            var user = await AuthenticateUser(login);

            if (user == null)
            {              
                return null;
            }
            var token = GenerateToken(user);
            return token;
        }
        public string GeneratePasswordResetToken(User user)
        {
            var token =  GenerateToken(user);
            return token;
        }
     
    }
}
