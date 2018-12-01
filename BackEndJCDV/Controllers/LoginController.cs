using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using BackEndJCDV.JWT;
using Domain.Entity;
using LiteDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BackEndJCDV.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private IConfiguration _config;
        private string DATABASE_PATH;
        private string ADMIN_USER;
        private string ADMIN_PSW;
        const string COLLECTION_USUARIOS_NAME = "usuarios";

        public LoginController(IConfiguration Configuration)
        {
            _config = Configuration;
            DATABASE_PATH = _config["DataBasePath"];
            ADMIN_USER = _config["Admin:User"];
            ADMIN_PSW = _config["Admin:Password"];

            if (string.IsNullOrEmpty(DATABASE_PATH))
                DATABASE_PATH = Directory.GetCurrentDirectory() + "\\Data\\Banco.db";
        }

        [AllowAnonymous]
        [HttpPost]
        public object Post(
            [FromBody]Usuario usuario,
            [FromServices]SigningConfigurations signingConfigurations,
            [FromServices]TokenConfigurations tokenConfigurations)
        {
            bool credenciaisValidas = false;
            //Verifico se o usuario é valido
            if (usuario != null 
                && !string.IsNullOrWhiteSpace(usuario.Email)
                && !string.IsNullOrWhiteSpace(usuario.Senha))
            {
                if (usuario.Email == ADMIN_USER && usuario.Senha == ADMIN_PSW)
                {
                    credenciaisValidas = true;
                }
                else
                {
                    using (var db = new LiteRepository(new LiteDatabase(DATABASE_PATH)))
                    {
                        var usuarioDb = db.SingleOrDefault<Usuario>(x => x.Email == usuario.Email
                        && x.Senha == usuario.Senha, COLLECTION_USUARIOS_NAME);
                        if (usuarioDb != null)
                            credenciaisValidas = true;
                    }
                }
            }
            //Se as credencias não estão validas eu mando um objeto anonimo falando que não está OK :(
            if (!credenciaisValidas)
                return new
                {
                    authenticated = false,
                    message = "Falha ao autenticar"
                };

            //Se estão validas eu mando o token JWT
            ClaimsIdentity identity = new ClaimsIdentity(
                    new GenericIdentity(usuario.Email, "Login"),
                    new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Email)
                    }
                );

            DateTime dataCriacao = DateTime.Now;
            DateTime dataExpiracao = dataCriacao +
                TimeSpan.FromSeconds(tokenConfigurations.Seconds);

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = tokenConfigurations.Issuer,
                Audience = tokenConfigurations.Audience,
                SigningCredentials = signingConfigurations.SigningCredentials,
                Subject = identity,
                NotBefore = dataCriacao,
                Expires = dataExpiracao
            });
            var token = handler.WriteToken(securityToken);
            return new
            {
                authenticated = true,
                created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                accessToken = token,
                message = "OK"
            };
        }
    }
}