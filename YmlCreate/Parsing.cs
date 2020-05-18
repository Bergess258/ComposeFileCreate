using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Linq;
using HtmlAgilityPack;

namespace YmlCreate
{
    public class Parsing
    {
        public void Start()
        {
            var baseURL = @"https://hub.docker.com/search?q=&type=image";
            List<string> pages = new List<string>();
            pages.AddRange(
                Enumerable
                .Range(1, 97)
                .Select(el => $"{baseURL}&page={el}")
            );
            HtmlDocument html = new HtmlDocument();
            foreach(string page in pages)
            {
                string txtHTML = GetPage(page);
                html.LoadHtml(txtHTML);
                HtmlNodeCollection temp = html.DocumentNode.SelectSingleNode("//a[@data-testid='imageSearchResult']").ParentNode.ChildNodes;
                foreach(HtmlNode serviceNode in temp)
                {
                    HtmlNode loc = serviceNode.FirstChild.FirstChild.FirstChild.FirstChild;

                }
            }
        }
        public static string GetPage(string url)
        {
            var result = String.Empty;
            var request = (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseStream = response.GetResponseStream();
                if (responseStream != null)
                {
                    StreamReader streamReader;
                    if (response.CharacterSet != null)
                        streamReader = new StreamReader(responseStream, Encoding.GetEncoding(response.CharacterSet));
                    else
                        streamReader = new StreamReader(responseStream);
                    result = streamReader.ReadToEnd();
                    streamReader.Close();
                }
                response.Close();
            }
            return result;
        }
    }
}
