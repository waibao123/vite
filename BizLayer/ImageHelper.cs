using EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLayer
{
    public static class ImageHelper
    {
        static string DefaultUrl = "DefaultImage.jpg";

        public static List<string> GetPics(string pathBaseDir, int webId, string productName, PickPicMode mode)
        {
            if (mode == PickPicMode.Content)
                return new List<string>();
            productName = productName.Replace("+", null);
            List<string> result = new List<string>();
            string pathProduct = string.Format("{0}\\{1}\\{2}", pathBaseDir, webId, productName);
            SearchOption so = mode != PickPicMode.Content ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories;

            string[] array = null;
            try
            {
                array = Directory.GetFiles(pathProduct, "*", so);
            }
            catch
            { }
            if (array == null || array.Length == 0)
            {
                result.Add(DefaultUrl);
                return result;
            }
            foreach (string fullpath in array)
            {
                string lastPath = fullpath.Replace(pathProduct + "\\", null);
                if (mode == PickPicMode.Content && lastPath.Contains("\\"))
                {
                    result.Add(string.Format("{0}/{1}/{2}", webId, productName, lastPath));
                    continue;
                }
                try
                {
                    Image img = Bitmap.FromFile(fullpath);
                    int ratio = img.Height / img.Width;
                    if (ratio > 3 && mode != PickPicMode.Content)
                        continue;
                    if (mode == PickPicMode.Content)
                    {
                        if (ratio >= 2)
                        {
                            result.Add(string.Format("{0}/{1}/{2}", webId, productName, lastPath));
                            return result;
                        }
                        continue;
                    }
                    result.Add(string.Format("{0}/{1}/{2}", webId, productName, lastPath));
                }
                catch { continue; }
                if (mode == PickPicMode.FirstGallery && result.Count > 0)
                    return result;
            }
            if (result.Count == 0)
                result.Add(DefaultUrl);
            return result;
        }
    }
}
