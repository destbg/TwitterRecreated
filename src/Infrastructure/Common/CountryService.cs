using System;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Infrastructure.Common
{
    public class CountryService : ICountryService
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IConfiguration _configuration;

        public CountryService(ICurrentUserService currentUser, IConfiguration configuration)
        {
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<string> GetCountry()
        {
            var ip = _currentUser.Ip;
#if DEBUG
            if (ip == "::1" || ip == "127.0.0.1" || ip == "192.168.0.105")
                return "BG";
#endif
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-rapidapi-host", "apility-io-ip-geolocation-v1.p.rapidapi.com");
            client.DefaultRequestHeaders.Add("x-rapidapi-key", _configuration.GetValue<string>("GeoLocationKey"));
            var responce = await client.GetStringAsync("https://apility-io-ip-geolocation-v1.p.rapidapi.com/" + ip);
            var data = JsonConvert.DeserializeObject<CountryJson>(responce);
            return data.Ip.Country;
        }
    }
}
