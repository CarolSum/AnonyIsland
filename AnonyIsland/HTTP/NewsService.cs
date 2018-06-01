using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using AnonyIsland.Models;

namespace AnonyIsland.HTTP
{
    /// <summary>
    /// 新闻相关服务
    /// </summary>
    class NewsService
    {
        static readonly string _urlRecentNews = "http://wcf.open.cnblogs.com/news/recent/paged/{0}/{1}";  //page_index page_size
        static readonly string _urlRecommendNews = "http://wcf.open.cnblogs.com/news/recommend/paged/{0}/{1}";  //page_index page_size
        static readonly string _urlNewsContent = "http://wcf.open.cnblogs.com/news/item/{0}"; //news_id
        static readonly string _urlNewsComment = "http://wcf.open.cnblogs.com/news/item/{0}/comments/{1}/{2}";  //news_id page_index page_size

        /// <summary>
        /// 获取首页新闻
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async static Task<List<CnNews>> GetRecentNewsAsync(int pageIndex,int pageSize)
        {
            try
            {
                string url = string.Format(_urlRecentNews, pageIndex, pageSize);
                string xml = await BaseService.SendGetRequest(url);

                if (xml != null)
                {
                    List<CnNews> listNews = new List<CnNews>();
                    CnNews news;

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xml);

                    XmlNode feed = doc.ChildNodes[1];

                    foreach (XmlNode node in feed.ChildNodes)
                    {
                        if (node.Name.Equals("entry"))
                        {
                            news = new CnNews();
                            foreach (XmlNode node2 in node.ChildNodes)
                            {
                                if (node2.Name.Equals("id"))
                                {
                                    news.Id = node2.InnerText;
                                }
                                if (node2.Name.Equals("title"))
                                {
                                    news.Title = node2.InnerText;
                                }
                                if (node2.Name.Equals("summary"))
                                {
                                    news.Summary = node2.InnerText;
                                }
                                if (node2.Name.Equals("published"))
                                {
                                    DateTime t = DateTime.Parse(node2.InnerText);
                                    news.PublishTime = t.ToString();
                                }
                                if (node2.Name.Equals("updated"))
                                {
                                    news.UpdateTime = node2.InnerText;
                                }
                                if (node2.Name.Equals("link"))
                                {
                                    news.NewsRawUrl = node2.Attributes["href"].Value;
                                }
                                if (node2.Name.Equals("diggs"))
                                {
                                    news.Diggs = node2.InnerText;
                                }
                                if (node2.Name.Equals("views"))
                                {
                                    news.Views = node2.InnerText;
                                }
                                if (node2.Name.Equals("comments"))
                                {
                                    news.Comments = node2.InnerText;
                                }
                                if (node2.Name.Equals("topic"))
                                {
                                    if (node2.HasChildNodes)
                                    {
                                        news.TopicName = node2.InnerText;
                                    }
                                    else
                                    {
                                        news.TopicName = "";
                                    }
                                }
                                if (node2.Name.Equals("topicIcon"))
                                {
                                    if (node2.HasChildNodes)
                                    {
                                        news.TopicIcon = node2.InnerText;
                                    }
                                    else
                                    {
                                        news.TopicIcon = "http://static.cnblogs.com/images/logo_small.gif";
                                    }
                                }
                                if (node2.Name.Equals("sourceName"))
                                {
                                    news.SourceName = node2.InnerText;
                                }
                            }
                            listNews.Add(news);
                        }
                    }
                    return listNews;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 获取指定新闻内容
        /// </summary>
        /// <param name="newsId"></param>
        /// <returns></returns>
        public async static Task<string> GetNewsContentAsync(string newsId)
        {
            try
            {
                string url = string.Format(_urlNewsContent, newsId);
                string xml = await BaseService.SendGetRequest(url);
                if (xml != null)
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xml);

                    if (doc.ChildNodes.Count == 2 && doc.ChildNodes[1].ChildNodes[3].Name.Equals("Content"))
                    {
                        return "<style>body{font-family:微软雅黑;font-size=14px}</style>" + doc.ChildNodes[1].ChildNodes[3].InnerText;

                    }

                    return null;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 获取指定新闻评论
        /// </summary>
        /// <param name="newsId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async static Task<List<CnNewsComment>> GetNewsCommentsAysnc(string newsId,int pageIndex,int pageSize)
        {
            try
            {
                string url = string.Format(_urlNewsComment, newsId, pageIndex, pageSize);
                string xml = await BaseService.SendGetRequest(url);
                if (xml != null)
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xml);

                    List<CnNewsComment> listComments = new List<CnNewsComment>();
                    CnNewsComment comment;
                    XmlNode feed = doc.ChildNodes[1];

                    foreach (XmlNode node in feed.ChildNodes)
                    {
                        if (node.Name.Equals("entry"))
                        {
                            comment = new CnNewsComment();
                            foreach (XmlNode node2 in node.ChildNodes)
                            {
                                if (node2.Name.Equals("id"))
                                {
                                    comment.Id = node2.InnerText;
                                }
                                if (node2.Name.Equals("published"))
                                {
                                    DateTime t = DateTime.Parse(node2.InnerText);
                                    comment.PublishTime = t.ToString();
                                }
                                if (node2.Name.Equals("updated"))
                                {
                                    comment.UpdateTime = node2.InnerText;
                                }
                                if (node2.Name.Equals("author"))
                                {
                                    comment.AuthorName = node2.ChildNodes[0].InnerText;
                                    comment.AuthorHome = node2.ChildNodes[1].InnerText;
                                    comment.AuthorAvatar = "http://pic.cnblogs.com/avatar/simple_avatar.gif";  //api中没有头像url
                                }
                                if (node2.Name.Equals("content"))
                                {
                                    comment.Content = node2.InnerText;
                                }
                            }
                            listComments.Add(comment);
                        }
                    }
                    return listComments;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 获取推荐新闻
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async static Task<List<CnNews>> GetTopDiggsAsync(int pageIndex,int pageSize)
        {
            try
            {
                string url = string.Format(_urlRecommendNews, pageIndex, pageSize);
                string xml = await BaseService.SendGetRequest(url);

                if (xml != null)
                {
                    List<CnNews> listNews = new List<CnNews>();
                    CnNews news;

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xml);

                    XmlNode feed = doc.ChildNodes[1];

                    foreach (XmlNode node in feed.ChildNodes)
                    {
                        if (node.Name.Equals("entry"))
                        {
                            news = new CnNews();
                            foreach (XmlNode node2 in node.ChildNodes)
                            {
                                if (node2.Name.Equals("id"))
                                {
                                    news.Id = node2.InnerText;
                                }
                                if (node2.Name.Equals("title"))
                                {
                                    news.Title = node2.InnerText;
                                }
                                if (node2.Name.Equals("summary"))
                                {
                                    news.Summary = node2.InnerText;
                                }
                                if (node2.Name.Equals("published"))
                                {
                                    DateTime t = DateTime.Parse(node2.InnerText);
                                    news.PublishTime = t.ToString();
                                }
                                if (node2.Name.Equals("updated"))
                                {
                                    news.UpdateTime = node2.InnerText;
                                }
                                if (node2.Name.Equals("link"))
                                {
                                    news.NewsRawUrl = node2.Attributes["href"].Value;
                                }
                                if (node2.Name.Equals("diggs"))
                                {
                                    news.Diggs = node2.InnerText;
                                }
                                if (node2.Name.Equals("views"))
                                {
                                    news.Views = node2.InnerText;
                                }
                                if (node2.Name.Equals("comments"))
                                {
                                    news.Comments = node2.InnerText;
                                }
                                if (node2.Name.Equals("topic"))
                                {
                                    if (node2.HasChildNodes)
                                    {
                                        news.TopicName = node2.InnerText;
                                    }
                                    else
                                    {
                                        news.TopicName = "";
                                    }
                                }
                                if (node2.Name.Equals("topicIcon"))
                                {
                                    if (node2.HasChildNodes)
                                    {
                                        news.TopicIcon = node2.InnerText;
                                    }
                                    else
                                    {
                                        news.TopicIcon = "http://static.cnblogs.com/images/logo_small.gif";
                                    }
                                }
                                if (node2.Name.Equals("sourceName"))
                                {
                                    news.SourceName = node2.InnerText;
                                }
                            }
                            listNews.Add(news);
                        }
                    }
                    return listNews;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
