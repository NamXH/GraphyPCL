using System;
using Newtonsoft.Json;

namespace GraphyPCL
{
    public class GeonamesCountrySubdivision
    {
        [JsonProperty(PropertyName = "adminCode1")]
        public string AdminCode
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "countryCode")]
        public string CountryCode
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "adminName1")]
        public string AdminName
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "countryName")]
        public string CountryName
        {
            get;
            set;
        }
    } 
}