using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace AppJCDV.Services
{
    interface IUsuarioService
    {
        [Patch("/usuarios/{id}/location")]
        Task SendLocation(
           string id,[Body]Location location,
           [Header("Authorization")] string bearerToken);

        [Get("/usuarios/{id}/location")]
        Task<Location> GetLocation(Guid id, [Header("Authorization")] string bearerToken); 
    }
}
