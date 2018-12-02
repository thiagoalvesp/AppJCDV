using AppJCDV.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace AppJCDV.Services
{
    public class UsuarioService
    {
        //No inicio vou considerar que o usuario já existe e vou somente atualizar a posição com intuito de repassar
        const string URL_API_USUARIO = "https://backendjcdv20181201083159.azurewebsites.net/api";

        public async Task SendLocation(Location location)
        {
            try
            {
                var mockUser = new Usuario()
                {
                    Id = new Guid("a2a2a40a-1c6b-4097-9131-4e0de5ac5cd4"),
                    Email = "thiago-alvesp@live.com",
                    Senha = "123"
                };
                var token = await new LoginService().GetToken();

                var usuarioApi = RestService.For<IUsuarioService>(URL_API_USUARIO);
                await usuarioApi.SendLocation(
                    mockUser.Id.ToString(), 
                    location, 
                    $"Bearer {token.accessToken}");
            }
            catch (Exception ex) //send msg to UI
            {
                throw;
            }
        }

        public async Task<Location> GetLocation(Guid id)
        {
            try
            {
                var token = await new LoginService().GetToken();

                var usuarioApi = RestService.For<IUsuarioService>(URL_API_USUARIO);
                var location = await usuarioApi.GetLocation(
                    id,
                    $"Bearer {token.accessToken}");

                return location;
            }
            catch (Exception ex) //send msg to UI
            {
                throw;
            }
        }

    }
}
