using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using FishOn.Model;
using FishOn.Model.ViewModel;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace FishOn.Utils
{
    public static class Extensions
    {
        public static string GetValue(this XElement element, string xPathQuery)
        {
            var resultElement = XPathQuery(element, xPathQuery);
            if (resultElement != null)
            {
                return resultElement.Value;
            }

            return null;
        }

        public static XElement XPathQuery(this XElement element, string xPathQuery)
        {
            var queryParts = xPathQuery.Split('/');
            // FavoriteMovies/Movie[@category='Drama']/Title/Value

            var currentElement = element;

            foreach (var part in queryParts)
            {
                if (currentElement == null)
                {
                    break;
                }

                var nodeName = part;
                var attrName = "";
                var attrValue = "";

                if (part.Contains("[@"))
                {
                    //Movie[@cateogyr='Drama']   
                    nodeName = part.Substring(0, part.IndexOf("["));
                    var temp = part.Substring(nodeName.Length).Split('=');
                    attrName = temp[0].Replace("[", "").Replace("@", "");
                    attrValue = temp[1].Replace("'", "").Replace("]", "");
                }

                if (!string.IsNullOrEmpty(attrValue))
                {
                    currentElement =
                        currentElement.Elements(nodeName).FirstOrDefault(e => e.Attribute(attrName).Value == attrValue);
                }
                else
                {
                    currentElement = currentElement.Element(nodeName);
                }
            }
            return currentElement;

        }

        public static int ToInt(this string str)
        {
            int result = 0;
            int.TryParse(str, out result);
            return result;
        }

        public static bool ToBool(this string str)
        {
            var temp = str.ToLower();
            if (temp == "true" || temp == "t" || temp == "yes" || temp == "y")
            {
                return true;
            }

            return false;
        }

        public static Page FindPage(this List<Page> pages, string pageTitle)
        {
            return pages.SingleOrDefault(p => p.Title == pageTitle);
        }

        public static double ToDouble(this string str)
        {
            double result = 0;
            double.TryParse(str, out result);
            return result;
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNotNullOrEmpty(this string str)
        {
            return !string.IsNullOrEmpty(str);
        }

        public static void MoveUp(this List<Species> species, Species speciesToMove)
        {
            int index = FindIndex(species, speciesToMove);
            if (index > 0)
            {
                var temp = speciesToMove.DisplayOrder;
                speciesToMove.DisplayOrder = species[index - 1].DisplayOrder;
                species[index - 1].DisplayOrder = temp;
            }
        }

        public static void MoveDown(this List<Species> species, Species speciesToMove)
        {
            int index = FindIndex(species, speciesToMove);
            if (index < species.Count-1)
            {
                var temp = speciesToMove.DisplayOrder;
                speciesToMove.DisplayOrder = species[index + 1].DisplayOrder;
                species[index + 1].DisplayOrder = temp;
            }
        }

        public static int FindIndex(this List<Species> species, Species speciesToFind)
        {
            for (int i = 0; i < species.Count; i++)
            {
                if (speciesToFind.SpeciesId == 0)
                {
                    if (speciesToFind.Name == species[i].Name)
                    {
                        return i;
                    }
                }
                else if (species[i].SpeciesId == speciesToFind.SpeciesId)
                {
                    return i;
                }
            }

            return -1;
        }

        public static object Clone<T>(this Object obj)
        {
            var jsonString = JsonConvert.SerializeObject(obj);
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public static string GetStringValue(this List<AppSetting> settings, string name)
        {
            if (settings == null)
            {
                return null;
            }

            var result =
                settings.SingleOrDefault(s => s.SettingName == name);

            return result?.Value;
        }

        public static void AddItems(this Picker picker, IEnumerable<string> items)
        {
            foreach (var item in items)
            {
                picker.Items.Add(item);
            }
        }

        public static void AddSpeciesCaught(this List<SpeciesCaughtViewModel> list, Model.FishOn fishCaught)
        {
            SpeciesCaughtViewModel speciesCaught = list.SingleOrDefault(s => s.SpeciesName == fishCaught.Species.Name);

            if (speciesCaught == null)
            {
                speciesCaught.SpeciesName = fishCaught.Species.Name;
                list.Add(speciesCaught);
            }

            int i = 0;

            for (i = 0; i < speciesCaught.FishCaught.Count; i++)
            {
                if (speciesCaught.FishCaught[i].DateCaught > fishCaught.DateCaught)
                {
                    break;
                }
            }

            speciesCaught.FishCaught.Insert(i, fishCaught);
        }
    }
}
