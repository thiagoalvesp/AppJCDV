using AppJCDV.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppJCDV
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = new MainPageViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<FeatureNotSupportedException>(this,
                "FeatureNotSupported",
                async (fnsEx) => {
                    await DisplayAlert("Error", "Funcionalidade não suportada.", "OK");
                });

            MessagingCenter.Subscribe<PermissionException>(this,
               "Permission",
               async (pEx) => {
                   await DisplayAlert("Error", "Aplicativo sem permissão para capturar a localização.", "OK");
               });

            MessagingCenter.Subscribe<Exception>(this,
               "Exception",
               async (ex) => {
                   await DisplayAlert("Error", "Ocorreu um problema inesperado, entre em contato com o suporte.", "OK");
               });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<FeatureNotSupportedException>(this, "FeatureNotSupported");
            MessagingCenter.Unsubscribe<PermissionException>(this, "Permission");
            MessagingCenter.Unsubscribe<Exception>(this, "Exception");
        }

    }
}
