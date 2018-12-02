﻿using AppJCDV.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppJCDV.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {

        private bool continueGetLocation;

        public bool ContinueGetLocation
        {
            get { return continueGetLocation; }
            set {
                continueGetLocation = value;
                BtnText = continueGetLocation ? "Stop Send My Location!" : "Start Send My Location!";
            }
        }

        private string btnText = "Start Send My Location!";

        public string BtnText
        {
            get { return btnText; }
            set {
                btnText = value;
                OnPropertyChanged();
            }
        }

        private double? latitude;

        public double? Latitude
        {
            get { return latitude; }
            set {
                latitude = value;
                OnPropertyChanged();
            }
        }

        private double? longitude;

        public double? Longitude
        {
            get { return longitude; }
            set {
                longitude = value;
                OnPropertyChanged();
            }
        }

        private double? altitude;

        public double? Altitude
        {
            get { return altitude; }
            set {
                altitude = value;
                OnPropertyChanged();
            }
        }

        public MainPageViewModel()
        {
            GetGeoLocationCommand = new Command( async () =>
            {
                ContinueGetLocation = !ContinueGetLocation;

                while (ContinueGetLocation)
                {
                    await GetGeoLocation();
                }
            });
        }

        private async Task GetGeoLocation()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                    location = await Geolocation.GetLocationAsync(request);
                }

                if (location != null)
                {
                    Latitude = location.Latitude;
                    Longitude = location.Longitude;
                    Altitude = location.Altitude;
                }

                await new UsuarioService().SendLocation(location);


            }
            catch (FeatureNotSupportedException fnsEx)
            {
                ContinueGetLocation = false;
                MessagingCenter.Send<FeatureNotSupportedException>
                (fnsEx, "FeatureNotSupported");
            }
            catch (PermissionException pEx)
            {
                ContinueGetLocation = false;
                MessagingCenter.Send<PermissionException>
                    (pEx, "Permission");
            }
            catch (Exception ex)
            {
                ContinueGetLocation = false;
                MessagingCenter.Send<Exception>
                 (ex, "Exception");
            }
        }

        public ICommand GetGeoLocationCommand { get; set; }

    }
}
