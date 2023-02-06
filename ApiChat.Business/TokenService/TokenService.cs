using ApiChat.DataAccess.Models;
using ApiChat.Domain.ModelsDTO.TokenDTO;
using ApiChat.Domain.UnitOfWork;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApiChat.Business.TokenService
{
    public class TokenService : ITokenService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TokenService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<TokenResponse> GetToken(Credentials credentials)
        {
            var userResponse = (await _unitOfWork.Repository<TblUser>().GetAllAsync(x => x.Email == credentials.Email)).FirstOrDefault();
            if (userResponse == null) throw new Exception("El usuario no existe.");

            if (userResponse.Password != credentials.Password) throw new Exception("Contraseña incorrecta");

            var Token = GenerateToken(userResponse);

            return new() { APPToken = Token };
        }

        private string GenerateToken(TblUser tblUser)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, tblUser.NickName),
                new Claim(ClaimTypes.Name, tblUser.FullName),
                new Claim(ClaimTypes.Email, tblUser.Email)
            };

            var Handler = new JwtSecurityTokenHandler();
            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes("asdfgDHJFDSKAJYTRQBFIgfkdjbvifdgvbbnkjhkijsdgfaksjdfhknsbvilasg")), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = Handler.CreateToken(descriptor);

            return Handler.WriteToken(token);
        }
    }
}
