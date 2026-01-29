using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Infrastracture.Keyclock
{
    //public class KeyclockOptions
    //{
    //    public string Issuer { get; set; } = default!;
    //    public string Audience { get; set; } = default!;
    //    public string MetadataAddress { get; set; } = default!;
    //    public bool RequireHttpsMetadata { get; set; }
    //}

    public class KeyclockOptions 
    {
        public string RealmUrl { get; set; } = default!;
        public string AdminUrl { get; set; } = default!;
        public string TokenUrl { get; set; }= default!;
        public string ClientId { get; set; } = default!;
        public string ClientSecret { get; set; }=default!;
        public string AdminUserName { get; set; } = default!;
        public string AdminPassword { get; set; } = default!;
        public string Issure { get; set; }= default!;
        public string Audience { get; set;} = default!;
        public string MetadataAddress { get; set; } = default!;
        public bool RequireHttpsMetadata { get; set;}

       
    }

    public class KeyclockService(HttpClient httpClient, KeyclockOptions options)
    {
        private readonly HttpClient httpClient = httpClient;
        private readonly KeyclockOptions keyclockOptions = options;

        private async Task<string> GetAccessToken()
        {
            // grant_type=password&client_id=admin-cli&username=foe&password=admin
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type","password"),
                new KeyValuePair<string, string>("client_id",keyclockOptions.ClientId),
                new KeyValuePair<string, string>("client_secret",keyclockOptions.ClientSecret)
            });
            var response= await httpClient.PostAsync(keyclockOptions.TokenUrl, content);
            response.EnsureSuccessStatusCode();
            var json =await response.Content.ReadFromJsonAsync<Dictionary<string,string>>();

            return json["access_token"];
        }
    }
}
