

using System.Text.Json.Nodes;
using KamerConnect.Models;
using KamerConnect.Repositories;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KamerConnect.DataAccess.GeoLocation.Repositories;

public class GeoLocationRepository : IGeoLocationRepository
{
    public async Task<Point> GetGeoCode(string search)
    {
        string requestUrl = $"{Environment.GetEnvironmentVariable("PDOK_URL")}/search/v3_1/free?rows=1&fq=bron:BAG&q={Uri.EscapeDataString(search)}";

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

    public async Task<Polygon> GetRangePolygon(double timeRange, Point startLocation)
    {
        string requestUrl = $"{Environment.GetEnvironmentVariable("ORS_URL")}/ors/v2/isochrones/driving-car";

        try
        {
            using (HttpClient client = new HttpClient())
            {
                var requestBody = new
                {
                    locations = new[] { new[] { startLocation.X, startLocation.Y } },
                    range = new[] { timeRange },
                    interval = timeRange
                };

                var jsonBody = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(requestUrl, content);
                response.EnsureSuccessStatusCode();

                string jsonResponse = await response.Content.ReadAsStringAsync();
                var jsonObject = JObject.Parse(jsonResponse);
                var coordinates = jsonObject["features"][0]["geometry"]["coordinates"][0]
                       .Select(coord => new NetTopologySuite.Geometries.Coordinate((double)coord[0], (double)coord[1]))
                       .ToArray();

                var geometryFactory = new GeometryFactory();
                return geometryFactory.CreatePolygon(coordinates);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}