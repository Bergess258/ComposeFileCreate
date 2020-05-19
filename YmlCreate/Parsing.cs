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
        //Parsing Hub.Docker to get images and names of available services for docker-compose
        public static void Start()
        {
            var baseURL = @"https://hub.docker.com/search?q=&type=image";
            List<string> pages = new List<string>();
            pages.AddRange(
                Enumerable
                .Range(1, 99)
                .Select(el => $"{baseURL}&page={el}")
            );
            HtmlDocument html = new HtmlDocument();

            //Using Selenium because of content(services) loading on pages
            IWebDriver driver = new ChromeDriver();

            //Just for sure
            ServicePointManager.DefaultConnectionLimit = 20;

            WebDriverWait waitForElement = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            HttpClient client = new HttpClient();
            foreach (string page in pages)
            {
                driver.Url = page;
                waitForElement.Until(ExpectedConditions.ElementIsVisible(By.Id("searchResults")));
                html.LoadHtml(driver.PageSource);
                List<HtmlNode> temp = html.DocumentNode.SelectNodes("//a[@data-testid='imageSearchResult']").ToList();
                foreach(HtmlNode serviceNode in temp)
                {
                    HtmlNode loc = serviceNode.FirstChild.FirstChild.FirstChild.FirstChild;
                    string name = loc.Attributes[0].Value;
                    name = name.Remove(name.Length - 5);
                    if (loc.Name == "img")
                    {
                        using (var response = client.GetAsync(loc.Attributes["src"].Value))
                        {
                            response.Wait();
                            if (response.Result.StatusCode != HttpStatusCode.NotFound)
                                using (var inputStream = response.Result.Content.ReadAsByteArrayAsync())
                                    AllServices.services.Add(new Service(name, inputStream.Result));
                            else
                                AllServices.services.Add(new Service(name));
                        }
                    }
                    else
                        AllServices.services.Add(new Service(serviceNode.SelectSingleNode("//div[@class='styles__name___2198b']").InnerText));
                }
            }
            client.Dispose();
            driver.Dispose();
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream("MyFile.bin", FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
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
