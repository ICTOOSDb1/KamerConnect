using KamerConnect.Repositories;
using Newtonsoft.Json;

namespace KamerConnect.DataAccess.GeoLocation.Repositories;

public class GeoLocationRepository : IGeoLocationRepository
{
    public async Task<string> GetGeoCode(string search)
    {
        string baseUrl = "https://geodata.nationaalgeoregister.nl/locatieserver/v3/free";
        string requestUrl = $"{baseUrl}?q={Uri.EscapeDataString(search)}";

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();

            string jsonResponse = await response.Content.ReadAsStringAsync();
            dynamic data = JsonConvert.DeserializeObject(jsonResponse);

            return data?.response.docs[0].centroide_ll;
        }
    }
}
