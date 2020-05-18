using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Linq;
using HtmlAgilityPack;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using System.Net.Http;

namespace YmlCreate
{
    public class Parsing
    {
        public static void Start()
        {
            var baseURL = @"https://hub.docker.com/search?q=&type=image";
            List<string> pages = new List<string>();
            pages.AddRange(
                Enumerable
                .Range(1, 97)
                .Select(el => $"{baseURL}&page={el}")
            );
            HtmlDocument html = new HtmlDocument();
            IWebDriver driver = new ChromeDriver();
            //WebClient client = new WebClient();
            //client.Proxy = null;
            ServicePointManager.DefaultConnectionLimit = 20;
            WebDriverWait waitForElement = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            HttpClient client = new HttpClient();
            foreach (string page in pages)
            {
                //string txtHTML = GetPage(page);
                //html.LoadHtml(txtHTML);
                driver.Url = page;
                waitForElement.Until(ExpectedConditions.ElementIsVisible(By.Id("searchResults")));
                html.LoadHtml(driver.PageSource);
                List<HtmlNode> temp = html.DocumentNode.SelectNodes("//a[@data-testid='imageSearchResult']").ToList();
                foreach(HtmlNode serviceNode in temp)
                {
                    HtmlNode loc = serviceNode.FirstChild.FirstChild.FirstChild.FirstChild;
                    string name = loc.Attributes[0].Value;
                    if (loc.Name == "img"&&loc.Attributes["src"].Value!="")
                    {
                        using (var response = client.GetAsync(loc.Attributes[2].Value))
                        {
                            response.Wait();
                            using (var inputStream = response.Result.Content.ReadAsByteArrayAsync())
                            {
                                if (response.Result.StatusCode != HttpStatusCode.NotFound)
                                    AllServices.services.Add(new Service(name.Remove(name.Length - 5), inputStream.Result));
                                else
                                    AllServices.services.Add(new Service(name.Remove(name.Length - 5)));
                            }
                        }
                        //img = client.DownloadData(loc.Attributes["src"].Value);
                    }
                    else
                        AllServices.services.Add(new Service(name.Remove(name.Length - 5)));
                }
            }
            client.Dispose();
            driver.Dispose();
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream("MyFile.bin", FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, AllServices.services);
            stream.Close();
        }
        public static string GetPage(string url)
        {
            IWebDriver driver = new ChromeDriver();
            driver.Url = url;
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10);
            return driver.PageSource;
        }
    }
}
