using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using FishOn.Model;

namespace FishOn.Repositories
{
    public interface IFishOnHttpRepository
    {
        Task HttpPostAsync(Lake lake);
        Task HttpPostAsync(WayPoint lake);
        Task HttpPostAsync(Species lake);
        Task<List<Species>> GetSpeciesAsync();
        Task<List<Species>> GetSpeciesAsync(int lakeId);
        Task<List<WayPoint>> GetWayPointsAsync(int lakeId);
        Task<XElement> GetXMLAsync(string url);
        Task<string> GetRawStringAsync(string url);
    }

    public class FishOnHttpRepository : IFishOnHttpRepository
    {
        private const string Base_Service_Url = "";

        public async Task HttpPostAsync(Lake lake)
        {
           
        }

        public async Task HttpPostAsync(WayPoint lake)
        {

        }

        public async Task HttpPostAsync(Species lake)
        {

        }
        
        public async Task<List<Species>> GetSpeciesAsync()
        {
            return null;
        }

        public async Task<List<Species>> GetSpeciesAsync(int lakeId)
        {
            return null;
        }

        public async Task<List<WayPoint>> GetWayPointsAsync(int lakeId)
        {
            return null;
        }

        public async Task<XElement> GetXMLAsync(string url)
        {
            var xmlString = await GetRawStringAsync(url);
            return xmlString == null ? null :XElement.Parse(xmlString);
        }

        public async Task<string> GetRawStringAsync(string url)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Charset", "ISO-8859-1");

                var httpResponse = await httpClient.GetAsync(url);
                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    return await httpResponse.Content.ReadAsStringAsync();
                }

                return null;
            }
        }
    }
}
