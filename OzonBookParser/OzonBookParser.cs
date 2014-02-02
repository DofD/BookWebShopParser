namespace OzonBookParser
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using BookParser;

    using HtmlAgilityPack;

    class OzonBookParser : IBookParser
    {
        public string Name
        {
            get { return "Парсер сайта Ozon"; }
        }

        public List<Book> Parse(List<string> webPages)
        {
            return webPages.AsParallel().Select(this.Parse).Where(x => x != null).ToList();
        }

        private Book Parse(string webPage)
        {
            HtmlDocument HD = null;
            try
            {
                var web = new HtmlWeb
                {
                    AutoDetectEncoding = false,
                    OverrideEncoding = Encoding.GetEncoding("windows-1251"),
                };

                HD = web.Load(webPage);
            }
            catch (Exception e)
            {
                //TODO Запилить логер
            }

            Book result = null;

            if (HD != null)
            {
                var NoAltElements = HD.DocumentNode.SelectNodes("//*[@id=\"PageHeader\"]/div[3]/div/ul/li[1]/a");

                if (NoAltElements != null)
                {
                    if (NoAltElements.Any(noAltElement => noAltElement.InnerText.ToLower() != "книги"))
                    {
                        return null;
                    }
                }

                result = new Book(); // TODO не есть гуд

                var title = HD.DocumentNode.SelectSingleNode("//*[@id=\"PageContent\"]/div/div[1]/div/div[2]/div[1]/div[1]/h1");
                if (title != null)
                {
                    result.Title = title.InnerText;
                }

                if (string.IsNullOrEmpty(result.Title))
                {
                    return null;
                }

                var basicProperty = HD.DocumentNode.SelectSingleNode("//*[@id=\"js_basic_properties\"]");
                if (basicProperty != null)
                {
                    foreach (var childNode in basicProperty.ChildNodes.Where(x => !string.IsNullOrEmpty(x.InnerText.Trim())))
                    {
                        this.SetField(result, childNode);
                    }
                }

                // Устанавливаем стоимость
                var nodePrice = HD.DocumentNode.SelectSingleNode("//*[@id=\"ctl33_ctl00_SaleBlockPanel\"]/div/div[1]/div[2]/div/span[1]");
                if (nodePrice != null)
                {
                    decimal price;
                    if (decimal.TryParse(nodePrice.InnerText, out price))
                    {
                        result.Price = price;
                    }
                }
            }

            return result;
        }

        private void SetField(Book book, HtmlNode childNode)
        {
            var itemprop = childNode.Attributes["itemprop"] != null
                                ? childNode.Attributes["itemprop"].Value
                                : string.Empty;

            switch (itemprop)
            {
                case "author": book.Autors.AddRange(this.GetTitles(childNode.ChildNodes));
                    break;
                case "inLanguage":
                    book.Language = childNode.InnerText.Split(':')[1];
                    break;
                case "publisher":
                    book.PublishingHouse = this.GetTitles(childNode.ChildNodes).FirstOrDefault();
                    break;
                case "isbn":
                    book.ISBN = childNode.InnerText;
                    break;
                default:
                    this.SetOtherField(book, childNode);
                    break;
            }
        }

        private void SetOtherField(Book book, HtmlNode childNode)
        {
            var text = childNode.InnerText.Trim();

            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            if (text.StartsWith("Переводчи"))
            {
                var translatorsStr = text.Split(':')[1];

                foreach (var translator in translatorsStr.Split(','))
                {
                    book.Translators.Add(translator);
                }

                return;
            }

            if (text.StartsWith("Серия"))
            {
                book.Series = this.GetTitles(new[] { childNode }).FirstOrDefault();
            }
        }

        private IEnumerable<string> GetTitles(IEnumerable<HtmlNode> childNodes)
        {
            return childNodes.Where(x => x.Attributes["title"] != null).Select(x => x.Attributes["title"].Value).ToList();
        }
    }
}
