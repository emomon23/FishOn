using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

        public static double ToDouble(this string str)
        {
            double result = 0;
            double.TryParse(str, out result);
            return result;
        }
    }
}
