using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using FishOn.Model;
using FishOn.Model.ViewModel;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace FishOn.Utils
{
    public static class Extensions
    {
        private static Random _rnd = new Random(DateTime.Now.Millisecond);
        public static string GetValue(this XElement element, string xPathQuery)
        {
            var resultElement = XPathQuery(element, xPathQuery);
            if (resultElement != null)
            {
                return resultElement.Value;
            }

            return null;
        }

        public static string PickOne(this string[] strArray)
        {
            if (strArray == null || strArray.Length == 0)
            {
                return "";
            }

            if (strArray.Length == 1)
                return strArray[0];

            var index = _rnd.Next(0, strArray.Length - 1);
            return strArray[index];
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

        public static string Capitalize(this string str)
        {
            if (str.IsNullOrEmpty())
            {
                return str;
            }

            if (str.Length == 1)
            {
                return str.ToUpper();
            }

            return $"{str.Substring(0, 1).ToUpper()}{str.Substring(1)}";
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

        public static string LeadWithUpperCase(this string str)
        {
            if (str.IsNullOrEmpty())
            {
                return str;
            }

            if (str.Length == 1)
            {
                return str.ToUpper();
            }

            var result = $"{str.Substring(0, 1).ToUpper()}{str.Substring(1)}";

            return result;
        }

        public static void GoBackOnePage(this INavigation navigation)
        {
            var currentPage = navigation.NavigationStack[navigation.NavigationStack.Count - 1];
            navigation.RemovePage(currentPage);
        }

        public static bool IsDate(this string str)
        {
            DateTime d;

            return DateTime.TryParse(str, out d);
        }

        public static DateTime ToDate(this string str)
        {
            DateTime result;

            DateTime.TryParse(str, out result);
            return result;
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

        public static void MoveFishCaughtToDifferentWayPoint(this List<WayPoint> originalList, Model.FishOn fishCaught)
        {
            var originalFish = originalList.SelectMany(w => w.FishCaught).SingleOrDefault(f => f.FishOnId == fishCaught.FishOnId);
            if (originalFish != null)
            {
                var originalWayPointId = originalFish.OriginalWayPointId;
                var originalWayPoint = originalList.SingleOrDefault(w => w.WayPointId == originalWayPointId);
                originalWayPoint.FishCaught.Remove(originalFish);
            }

            var movedToWayPoint = originalList.SingleOrDefault(w => w.WayPointId == fishCaught.WayPointId);
            movedToWayPoint.FishCaught.Add(fishCaught);
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

        public static void CreateAddToolbarButton(this ContentPage page, Func<Func<Task>, Task> addFunction, Func<Task> callBack)
        {
            page.ToolbarItems.Add(new ToolbarItem("Add", null, async () =>
            {
                await addFunction(callBack);
            }));

            //Add a space
            page.ToolbarItems.Add(new ToolbarItem(" ", null, () =>
            {

            }));
        }

        public static void CreateSaveToolbarButton(this ContentPage page, Func<Task> saveFunction , Func<Task> deleteFunction = null)
        {
            if (deleteFunction != null)
            {
                page.ToolbarItems.Add(new ToolbarItem("Delete", null, async () =>
                {
                    await deleteFunction();
                }));


                //Add a space
                page.ToolbarItems.Add(new ToolbarItem(" ", null, () =>
                {

                }));
            }

            page.ToolbarItems.Add(new ToolbarItem("Save", null, async () =>
            {
                await saveFunction();
            }));

            //Add a space
            page.ToolbarItems.Add(new ToolbarItem(" ", null, () =>
            {

            }));
        }

        public static void CreateCancelButton(this ContentPage page, Func<Task> cancelFunction)
        {
           
            page.ToolbarItems.Add(new ToolbarItem("Cancel", null, async () =>
            {
                await cancelFunction();
            }));

            //Add a space
            page.ToolbarItems.Add(new ToolbarItem(" ", null, () =>
            {

            }));
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

    }
}
