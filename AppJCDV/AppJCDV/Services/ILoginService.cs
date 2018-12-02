using AppJCDV.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppJCDV.Services
{
    interface ILoginService
    {
        [Post("/login")]
        Task<Token> Authenticate([Body]Usuario usuario);
    }
}
