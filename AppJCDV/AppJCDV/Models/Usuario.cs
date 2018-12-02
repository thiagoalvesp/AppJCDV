using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace AppJCDV.Models
{
    public class Usuario
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public Location Location { get; set; }
    }
}
