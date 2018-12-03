using AppJCDV.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppJCDV.ViewModel
{
    public class GetPositionPageViewModel : BaseViewModel
    {
        private bool btnMapsEnabled;

        public bool BtnMapsEnabled
        {
            get { return btnMapsEnabled; }
            set {
                btnMapsEnabled = value;
                OnPropertyChanged();
            }
        }


        private Location location;
  
        public Location Location
        {
            get { return location; }
            set {
                location = value;
                if (location != null)
                    BtnMapsEnabled = true;
                OnPropertyChanged();
            }
        }

        private bool continueGetLocation;

        public bool ContinueGetLocation
        {
            get { return continueGetLocation; }
            set
            {
                continueGetLocation = value;
                BtnText = continueGetLocation ? "Stop Get User Location" : "Start Get User Location";
            }
        }

        private string btnText = "Start Get User Location";

        public string BtnText
        {
            get { return btnText; }
            set
            {
                btnText = value;
                OnPropertyChanged();
            }
        }

        private string userID = "a2a2a40a-1c6b-4097-9131-4e0de5ac5cd4";

        private string distance;

        public string Distance
        {
            get { return distance; }
            set {
                distance = value;
                OnPropertyChanged();
            }
        }


        public string UserID
        {
            get { return userID; }
            set {
                userID = value;
                OnPropertyChanged();
            }
        }


        private double? latitude;

        public double? Latitude
        {
            get { return latitude; }
            set
            {
                latitude = value;
                OnPropertyChanged();
            }
        }

        private double? longitude;

        public double? Longitude
        {
            get { return longitude; }
            set
            {
                longitude = value;
                OnPropertyChanged();
            }
        }

        private double? altitude;

        public double? Altitude
        {
            get { return altitude; }
            set
            {
                altitude = value;
                OnPropertyChanged();
            }
        }

        public GetPositionPageViewModel()
        {
            ShowOnMapsCommand = new Command(async () => {
                if (Location != null)
                    await Maps.OpenAsync(Location,
                      new MapsLaunchOptions(){MapDirectionsMode = MapDirectionsMode.Driving,});
            });

            GetGeoLocationCommand = new Command(async () =>
            {
                ContinueGetLocation = !ContinueGetLocation;
                //Por enquanto só irei consultar uma vez
                //while (ContinueGetLocation)
                //{
                    await GetGeoLocation();
                //}
            });
        }

        private async Task GetGeoLocation()
        {
            try
            {
                //Dados de localização do usuário consultado
                var apiLocation = await new UsuarioService().GetLocation(new Guid(userID));
                Latitude = apiLocation.Latitude;
                Longitude = apiLocation.Longitude;
                Altitude = apiLocation.Altitude;

                //Calculo da distancia com base na posição atual do device.
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location != null)
                {
                    var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                    location = await Geolocation.GetLocationAsync(request);
                }
                if (location != null)
                {
                    Distance = location.CalculateDistance(apiLocation, DistanceUnits.Kilometers)
                        .ToString($"F{2}");
                }

                Location = apiLocation;              
            
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
        public ICommand ShowOnMapsCommand { get; set; }

    }
}
