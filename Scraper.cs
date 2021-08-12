using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CsvHelper;
using HtmlAgilityPack;
using System.Globalization;

namespace Webscraper
{
    public class Scraper
    {
        private ObservableCollection<EntryModel> _entries = new ObservableCollection<EntryModel>();

        public ObservableCollection<EntryModel> Entries
        {
            get { return _entries; }
            set { _entries = value; }
        }
        public void ScrapeData(string page)
        {
            var web = new HtmlWeb();
            var doc = web.Load(page);

            var Articles = doc.DocumentNode.SelectNodes("//*[@class = 'article-single']");

            foreach (var article in Articles)
            {
                var header = HttpUtility.HtmlDecode(article.SelectSingleNode(".//li[@class = 'article-header']").InnerText);
                var description = HttpUtility.HtmlDecode(article.SelectSingleNode(".//li[@class = 'article-copy']").InnerText);
                Debug.Print($"Title: {header} \n Description: {description}");
                _entries.Add(new EntryModel { Title = header, Description = description});
            }
        }

        public void Export()
        {
            using (TextWriter tw = File.CreateText("SampleData.csv"))
            {
                using (CsvWriter cw = new CsvWriter(tw, new CultureInfo("en-US")))
                {
                    foreach (var entry in Entries)
                    {
                        cw.WriteRecord(entry);
                        cw.NextRecord();
                    }
                }
            }
        }
    }
}
