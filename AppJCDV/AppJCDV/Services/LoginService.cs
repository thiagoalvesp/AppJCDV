using AppJCDV.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppJCDV.Services
{
    public class LoginService
    {
        const string URL_API_USUARIO = "https://backendjcdv20181201083159.azurewebsites.net/api";

        public async Task<Token> GetToken()
        {
            try
            {
                var mockUser = new Usuario() {
                    Id = new Guid("a2a2a40a-1c6b-4097-9131-4e0de5ac5cd4"),
                    Email = "thiago-alvesp@live.com",
                    Senha = "123" };
                var loginApi = RestService.For<ILoginService>(URL_API_USUARIO);
                Token token = await loginApi.Authenticate(mockUser);
                return token;
            }
            catch (Exception) //Send msg to UI
            {
                throw;
            }
        }
    }
}
