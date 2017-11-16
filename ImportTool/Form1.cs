using DataAccessLayer;
using EntityLayer.DbEntity;
using EntityLayer.Enums;
using Framework;
using Microsoft.Office.Interop.Excel;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImportTool
{
    public partial class Form1 : Form
    {
        Dictionary<string, int> DicCatagory;
        Dictionary<string, int> DicAttr;

        public Form1()
        {
            InitializeComponent();
            DicCatagory = new Dictionary<string, int>();
            DicAttr = new Dictionary<string, int>();

            LoadData((int)WebsiteEnum.EnerVite);
            LoadData((int)WebsiteEnum.OZ);
            LoadData((int)WebsiteEnum.NTSA);
        }

        int GetColIndex(char c)
        {
            return c - 'A' + 1;
        }

        string GetCellText(Worksheet ws, int row, char col)
        {
            return ((Range)ws.Cells[row, GetColIndex(col)]).Text.ToString().Trim().Trim('\n');

        }

        void LoadData(int websiteId)
        {
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            object missing = System.Reflection.Missing.Value;
            Workbook wb = app.Application.Workbooks.Open("C:\\envr.xlsx", missing, true, missing, missing, missing, missing, missing, missing, true, missing, missing, missing, missing, missing);
            Worksheet ws = (Worksheet)wb.Worksheets.get_Item(websiteId);
            int rowMax = ws.UsedRange.Cells.Rows.Count;
            for (int i = 3; i <= rowMax; i++)
            {
                Product p = new Product();
                p.Name = GetCellText(ws, i, ('B'));
                p.Title = GetCellText(ws, i, ('I'));
                p.SubTitle = GetCellText(ws, i, ('J'));
                if (p.SubTitle == "无")
                    p.SubTitle = string.Empty;
                p.Price = 1234;
                p.PurchaseUrl = GetCellText(ws, i, ('H'));
                if (GetCellText(ws, i, ('A')) == "X")
                    p.IsRecommand = websiteId;
                if (FormatTools.IsAnyNullOrWhiteSpace(p.Name, p.Title))
                    break;
                p = CreateProduct(p);

                string[] categoryNames = ((Range)ws.Cells[i, GetColIndex('E')]).Text.ToString().Trim('\n').Split('/');
                foreach (string name in categoryNames)
                {
                    if (string.IsNullOrWhiteSpace(name))
                        continue;
                    int catagoryId = GetCatagoryId(name, websiteId);
                    string sql = string.Format("INSERT INTO productcatebelong values({0},{1})", catagoryId, p.Id);
                    DalFactory.ImportDal.ExecuteNonQuery(sql);
                }

                if (string.IsNullOrWhiteSpace(p.PurchaseUrl))
                    continue;
                //string sqlDelAttrMapping = "DELETE FROM productattrmapping WHERE ProductId=" + p.Id;
                Dictionary<string, string> kvps = GetAttrsFromJD(p.PurchaseUrl);
                int idx = 1;
                foreach (var kvp in kvps)
                {
                    ProductAttrMapping pam = new ProductAttrMapping();
                    pam.ProductId = p.Id;
                    pam.AttrId = GetAttrId(kvp.Key);
                    pam.AttrValue = kvp.Value;
                    pam.Idx = idx++;
                    DalFactory.ImportDal.InsertItem(pam);
                }

            }
            wb.Close();
        }

        Product CreateProduct(Product p)
        {
            //Product p = new Product();
            //p.Name = props[0];
            //p.Title = props[2];
            //p.SubTitle = props[3];
            //p.Price = 111;
            //p.PurchaseUrl = props[1];
            //p.CategoryId = GetCatagoryId(props[4]);

            DalFactory.ImportDal.InsertItem(p);

            string sqlQuery = string.Format("SELECT * FROM product WHERE Name='{0}'", p.Name);
            return DalFactory.ImportDal.GetItem<Product>(sqlQuery, null);
        }

        int GetCatagoryId(string name, int webId)
        {
            if (DicCatagory.ContainsKey(name + webId))
                return DicCatagory[name + webId];
            string sqlQuery = string.Format("SELECT * FROM productcategory WHERE WebId={0} AND CategoryName='{1}'", webId, name);
            ProductCategory pc = DalFactory.ImportDal.GetItem<ProductCategory>(sqlQuery, null);
            if (pc == null)
            {
                string sqlInsert = string.Format("INSERT INTO productcategory VALUES(0,{0},'{1}')", webId, name);
                DalFactory.ImportDal.ExecuteNonQuery(sqlInsert);
                pc = DalFactory.ImportDal.GetItem<ProductCategory>(sqlQuery, null);
            }
            DicCatagory[name + webId] = pc.Id;
            return pc.Id;
        }

        int GetAttrId(string name)
        {
            if (DicAttr.ContainsKey(name))
                return DicAttr[name];
            string sqlQuery = string.Format("SELECT * FROM productattr WHERE AttrName='{0}'", name);
            ProductAttr pa = DalFactory.ImportDal.GetItem<ProductAttr>(sqlQuery, null);
            if (pa == null)
            {
                string sqlInsert = string.Format("INSERT INTO productattr VALUES(0,'{0}')", name);
                DalFactory.ImportDal.ExecuteNonQuery(sqlInsert);
                pa = DalFactory.ImportDal.GetItem<ProductAttr>(sqlQuery, null);
            }
            DicAttr[name] = pa.Id;
            return pa.Id;
        }

        Dictionary<string, string> GetAttrsFromJD(string url)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            string html = GetHtml(url).Replace("\r\n", null).Replace("\n", null);
            Regex regUL = new Regex("<ul class=\"parameter2\">[\\s\\S]+?</ul>");
            string ul = regUL.Match(html).Value;
            Regex regLI = new Regex("<li[^>]+>(?<li>[\\s\\S]+?)</li>");
            Regex regTag = new Regex("<[\\s\\S]+?>");
            foreach (Match m in regLI.Matches(ul))
            {
                string li = m.Groups["li"].Value;
                int idx = li.IndexOf('：');
                string key = li.Substring(0, idx);
                string value = li.Substring(idx + 1);
                value = regTag.Replace(value, string.Empty).Trim();
                result[key] = value;
            }
            return result;
        }

        string GetHtml(string url)
        {
            HttpWebResponse response = null;
            StreamReader sr = null;
            HttpWebRequest webRequest = null;
            try
            {
                webRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
                webRequest.ServicePoint.ConnectionLimit = 20;
                webRequest.Method = "GET";
                webRequest.ContentType = "text/html";
                webRequest.UserAgent = "UAUAUAUA";
                webRequest.Referer = "https://www.google.co.jp";
                webRequest.Timeout = 5000;
                webRequest.AllowAutoRedirect = false;

                response = (HttpWebResponse)webRequest.GetResponse();
                sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("GB2312"));
                return sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (response != null)
                    response.Close();
                if (sr != null)
                    sr.Dispose();
            }
        }


    }
}
