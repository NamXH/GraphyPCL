using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using XLabs.Ioc;
using XLabs.Platform.Services.Geolocation;

namespace GraphyPCL
{
    public static class GeolocationManager
    {
        private static readonly string _geonamesUsername = "namhoang";
        private static readonly string _reverseGeocodingServiceBaseUri = "http://api.geonames.org/";

        private static IGeolocator _geolocator;

        public static IGeolocator Geolocator
        {
            get
            {
                if (_geolocator == null)
                {
                    //                    _geolocator = DependencyService.Get<IGeolocator>(); // Pre XLabs 2.0
                    _geolocator = Resolver.Resolve<IGeolocator>();
                }
                return _geolocator;
            }
        }

        private static Position _geolocation;

        public static Position Geolocation
        {
            get
            {
                return _geolocation;
            }
        }

        private static GeonamesCountrySubdivision _countrySubdivision;

        public static GeonamesCountrySubdivision CountrySubdivision
        {
            get
            {
                return _countrySubdivision;
            }
        }

        public static async Task UpdateGeolocation()
        {
            //            Geolocator.StartListening(1000, 1); // It seems we must not start&stop manually in order for timeout to work
            try
            {
                _geolocation = await Geolocator.GetPositionAsync(10000);
            }
            catch (Exception)
            {
                _geolocation = null;
            }
            //            Geolocator.StopListening(); // It seems we must not start&stop manually in order for timeout to work

            // Get location name from lat, long
            if (_geolocation != null)
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(_reverseGeocodingServiceBaseUri);
                var endpointAddress = String.Format("countrySubdivisionJSON?lat={0}&lng={1}&username={2}", _geolocation.Latitude, _geolocation.Longitude, _geonamesUsername);

                HttpResponseMessage response = null;
                try
                {
                    response = await client.GetAsync(endpointAddress);
                }
                catch (Exception)
                {
                    _countrySubdivision = null;
                    return;
                }

                var json = response.Content.ReadAsStringAsync().Result;
                _countrySubdivision = JsonConvert.DeserializeObject<GeonamesCountrySubdivision>(json);
            }
        }
    }
}

