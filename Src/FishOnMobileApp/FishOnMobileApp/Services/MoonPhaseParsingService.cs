using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FishOn.Model;
using FishOn.Repositories;
using FishOn.Utils;
using Xamarin.Forms;

namespace FishOn.Services
{
    public interface IMoonPhaseParsingService
    {
        Task<MoonPhase> GetCurrentMoonPhaseAsync();
    }

    public class MoonPhaseParsingService : IMoonPhaseParsingService
    {
        private IFishOnHttpRepository _httpClient;
        private string _rawHtml;
        private const string _baseURL = "http://www.moongiant.com/phase/";

        public MoonPhaseParsingService()
        {
            _httpClient = new FishOnHttpRepository();
        }

        public MoonPhaseParsingService(IFishOnHttpRepository httpRepository)
        {
            _httpClient = httpRepository;
        }

        public async Task<MoonPhase> GetCurrentMoonPhaseAsync()
        {
            _rawHtml = await _httpClient.GetRawStringAsync($"{_baseURL}{DateTime.Now.ToString("d")}");
            MoonPhase result = new MoonPhase()
            {
                Age = ParseOutValue("Moon Age:").ToDouble(),
                IlluminationPercent = ParseOutValue("Illumination:").ToInt(),
                Label = ParseOutValue("Phase:")
            };
            
            return result;
        }


        private string ParseOutValue(string labelName)
        {
            //eg "Phase:<span>Waxing Gibbous</span>
            int startIndex = _rawHtml.IndexOf(labelName);
            if (startIndex == -1)
            {
                return null;
            }

            startIndex = _rawHtml.IndexOf("<span", startIndex);
            if (startIndex == -1)
            {
                return null;
            }

            startIndex = _rawHtml.IndexOf(">", startIndex);
            int endIndex = _rawHtml.IndexOf("</", startIndex);

            if (endIndex == -1)
            {
                return null;
            }

            var result = _rawHtml.Substring(startIndex, endIndex - startIndex).Replace("%", "");
            return result;
        }

    }

    public class MoonPhase
    {
        public string Label { get; set; }
        public double Age { get; set; }
        public int IlluminationPercent { get; set; }
    }
   
}
