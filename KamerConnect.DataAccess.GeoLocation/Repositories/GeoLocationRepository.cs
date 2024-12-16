

using System.Text.Json.Nodes;
using KamerConnect.Models;
using KamerConnect.Repositories;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

namespace KamerConnect.DataAccess.GeoLocation.Repositories;

public class GeoLocationRepository : IGeoLocationRepository
{
    public async Task<Point> GetGeoCode(string search)
    {
        string requestUrl = $"https://api.pdok.nl/bzk/locatieserver/search/v3_1/free?rows=1&fq=bron:BAG&q={Uri.EscapeDataString(search)}";

        try
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();

                string jsonResponse = await response.Content.ReadAsStringAsync();

                var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory();
                var wktReader = new WKTReader(geometryFactory);

                return (Point)wktReader.Read(JsonObject.Parse(jsonResponse)["response"]["docs"][0]["centroide_ll"].ToString());
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<string> Get(Isochrone isochrone, double range)
    {
        return "";
    }
    
  
}