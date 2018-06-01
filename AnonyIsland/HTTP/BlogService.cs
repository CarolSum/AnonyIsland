using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using AnonyIsland.Models;

namespace AnonyIsland.HTTP
{
    /// <summary>
    /// 博客相关服务
    /// </summary>
    static class BlogService
    {
        static readonly string _urlRecentBlog = "http://wcf.open.cnblogs.com/blog/sitehome/paged/{0}/{1}"; //page_index page_size
        static readonly string _url48Views = "http://wcf.open.cnblogs.com/blog/48HoursTopViewPosts/{0}";  //item_count
        static readonly string _url10Diggs = "http://wcf.open.cnblogs.com/blog/TenDaysTopDiggPosts/{0}";  //item_count
        static readonly string _urlUserBlog = "http://wcf.open.cnblogs.com/blog/u/{0}/posts/{1}/{2}";  //blog_app page_index page_size
        static readonly string _urlRecommendBloger = "http://wcf.open.cnblogs.com/blog/bloggers/recommend/{0}/{1}";  //page_index page_size
        static readonly string _urlBlogContent = "http://wcf.open.cnblogs.com/blog/post/body/{0}";  //post_id
        static readonly string _urlBlogComment = "http://wcf.open.cnblogs.com/blog/post/{0}/comments/{1}/{2}";  //post_id page_index page_size

        /// <summary>
        /// 分页获取首页博客
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async static Task<List<CnBlog>> GetRecentBlogsAsync(int pageIndex,int pageSize)
        {
            try
            {
                string url = string.Format(_urlRecentBlog, pageIndex, pageSize);
                string xml = await BaseService.SendGetRequest(url);
                if (xml != null)
                {
                    List<CnBlog> listBlogs = new List<CnBlog>();
                    CnBlog cnblog;
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xml);
                    XmlNode feed = doc.ChildNodes[1];
                    foreach (XmlNode node in feed.ChildNodes)
                    {
                        if (node.Name.Equals("entry"))
                        {
                            cnblog = new CnBlog();
                            foreach (XmlNode node2 in node.ChildNodes)
                            {
                                if (node2.Name.Equals("id"))
                                {
                                    cnblog.Id = node2.InnerText;
                                }
                                if (node2.Name.Equals("title"))
                                {
                                    cnblog.Title = node2.InnerText;
                                }
                                if (node2.Name.Equals("summary"))
                                {
                                    cnblog.Summary = node2.InnerText + "...";
                                }
                                if (node2.Name.Equals("published"))
                                {
                                    DateTime t = DateTime.Parse(node2.InnerText);
                                    cnblog.PublishTime = "发表于 " + t;
                                }
                                if (node2.Name.Equals("updated"))
                                {
                                    cnblog.UpdateTime = node2.InnerText;
                                }
                                if (node2.Name.Equals("author"))
                                {
                                    cnblog.AuthorName = node2.ChildNodes[0].InnerText;
                                    cnblog.AuthorHome = node2.ChildNodes[1].InnerText;
                                    cnblog.AuthorAvator = node2.ChildNodes[2].InnerText.Equals("") ? "http://pic.cnblogs.com/avatar/simple_avatar.gif" : node2.ChildNodes[2].InnerText;
                                }
                                if (node2.Name.Equals("link"))
                                {
                                    cnblog.BlogRawUrl = node2.Attributes["href"].Value;
                                }
                                if (node2.Name.Equals("blogapp"))
                                {
                                    cnblog.BlogApp = node2.InnerText;
                                }
                                if (node2.Name.Equals("diggs"))
                                {
                                    cnblog.Diggs = node2.InnerText;
                                }
                                if (node2.Name.Equals("views"))
                                {
                                    cnblog.Views = "["+node2.InnerText+"]";
                                }
                                if (node2.Name.Equals("comments"))
                                {
                                    cnblog.Comments = "["+node2.InnerText+"]";
                                }
                            }
                            listBlogs.Add(cnblog);
                        }
                    }
                    return listBlogs;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 获取指定博客正文
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        public async static Task<string> GetBlogContentAsync(string postId)
        {
            try
            {
                string url = string.Format(_urlBlogContent, postId);
                string xml = await BaseService.SendGetRequest(url);
                if (xml != null)
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xml);

                    if (doc.ChildNodes.Count == 2 && doc.ChildNodes[1].Name.Equals("string"))
                    {
                        return "<style>body{font-family:微软雅黑;font-size=14px}</style>" + doc.ChildNodes[1].InnerText;
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
        /// 根据博主blog_app获取博客列表
        /// </summary>
        /// <param name="blogApp"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async static Task<List<CnBlog>> GetBlogsByUserAsync(string blogApp,int pageIndex,int pageSize)
        {
            try
            {
                string url = string.Format(_urlUserBlog, blogApp, pageIndex, pageSize);
                string xml = await BaseService.SendGetRequest(url);
                if (xml != null)
                {
                    List<CnBlog> listBlogs = new List<CnBlog>();
                    CnBlog cnblog;
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xml);
                    XmlNode feed = doc.ChildNodes[1];
                    foreach (XmlNode node in feed.ChildNodes)
                    {
                        if (node.Name.Equals("entry"))
                        {
                            cnblog = new CnBlog();
                            foreach (XmlNode node2 in node.ChildNodes)
                            {
                                if (node2.Name.Equals("id"))
                                {
                                    cnblog.Id = node2.InnerText;
                                }
                                if (node2.Name.Equals("title"))
                                {
                                    cnblog.Title = node2.InnerText;
                                }
                                if (node2.Name.Equals("summary"))
                                {
                                    cnblog.Summary = node2.InnerText + "...";
                                }
                                if (node2.Name.Equals("published"))
                                {
                                    DateTime t = DateTime.Parse(node2.InnerText);
                                    cnblog.PublishTime = "发表于 " + t;
                                }
                                if (node2.Name.Equals("updated"))
                                {
                                    cnblog.UpdateTime = node2.InnerText;
                                }
                                if (node2.Name.Equals("author"))
                                {
                                    cnblog.AuthorName = node2.ChildNodes[0].InnerText;
                                    cnblog.AuthorHome = node2.ChildNodes[1].InnerText;
                                    cnblog.AuthorAvator = doc.ChildNodes[1].ChildNodes[3].InnerText.Equals("") ? "http://pic.cnblogs.com/avatar/simple_avatar.gif" : doc.ChildNodes[1].ChildNodes[3].InnerText;
                                }
                                if (node2.Name.Equals("link"))
                                {
                                    cnblog.BlogRawUrl = node2.Attributes["href"].Value;
                                }
                                if (node2.Name.Equals("diggs"))
                                {
                                    cnblog.Diggs = node2.InnerText;
                                }
                                if (node2.Name.Equals("views"))
                                {
                                    cnblog.Views = "[" + node2.InnerText + "]";
                                }
                                if (node2.Name.Equals("comments"))
                                {
                                    cnblog.Comments = "[" + node2.InnerText + "]";
                                }
                            }
                            cnblog.BlogApp = cnblog.AuthorHome.Split('/')[3];
                            listBlogs.Add(cnblog);
                        }
                    }
                    return listBlogs;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 根据博客id获取评论
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async static Task<List<CnBlogComment>> GetBlogCommentsAsync(string postId,int pageIndex,int pageSize)
        {
            try
            {
                string url = string.Format(_urlBlogComment, postId, pageIndex, pageSize);
                string xml = await BaseService.SendGetRequest(url);
                if (xml != null)
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xml);

                    List<CnBlogComment> listComments = new List<CnBlogComment>();
                    CnBlogComment comment;
                    XmlNode feed = doc.ChildNodes[1];

                    foreach (XmlNode node in feed.ChildNodes)
                    {
                        if (node.Name.Equals("entry"))
                        {
                            comment = new CnBlogComment();
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
                                    comment.AuthorAvatar = "http://pic.cnblogs.com/avatar/simple_avatar.gif";
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
        /// 获取48小时阅读排行榜
        /// </summary>
        /// <param name="itemCount"></param>
        /// <returns></returns>
        public async static Task<List<CnBlog>> Get48TopViewsAysnc(int itemCount)
        {
            try
            {
                string url = string.Format(_url48Views, itemCount);
                string xml = await BaseService.SendGetRequest(url);
                if(xml != null)
                {
                    List<CnBlog> listBlogs = new List<CnBlog>();
                    CnBlog cnblog;
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xml);
                    XmlNode feed = doc.ChildNodes[1];
                    foreach (XmlNode node in feed.ChildNodes)
                    {
                        if (node.Name.Equals("entry"))
                        {
                            cnblog = new CnBlog();
                            foreach (XmlNode node2 in node.ChildNodes)
                            {
                                if (node2.Name.Equals("id"))
                                {
                                    cnblog.Id = node2.InnerText;
                                }
                                if (node2.Name.Equals("title"))
                                {
                                    cnblog.Title = node2.InnerText;
                                }
                                if (node2.Name.Equals("summary"))
                                {
                                    cnblog.Summary = node2.InnerText + "...";
                                }
                                if (node2.Name.Equals("published"))
                                {
                                    DateTime t = DateTime.Parse(node2.InnerText);
                                    cnblog.PublishTime = "发表于 " + t;
                                }
                                if (node2.Name.Equals("updated"))
                                {
                                    cnblog.UpdateTime = node2.InnerText;
                                }
                                if (node2.Name.Equals("author"))
                                {
                                    cnblog.AuthorName = node2.ChildNodes[0].InnerText;
                                    cnblog.AuthorHome = node2.ChildNodes[1].InnerText;
                                    if (node2.ChildNodes.Count == 3)
                                    {
                                        cnblog.AuthorAvator = node2.ChildNodes[2].InnerText.Equals("") ? "http://pic.cnblogs.com/avatar/simple_avatar.gif" : node2.ChildNodes[2].InnerText;
                                    }
                                    else
                                    {
                                        cnblog.AuthorAvator = "http://pic.cnblogs.com/avatar/simple_avatar.gif";
                                    }
                                }
                                if (node2.Name.Equals("link"))
                                {
                                    cnblog.BlogRawUrl = node2.Attributes["href"].Value;
                                }
                                if (node2.Name.Equals("diggs"))
                                {
                                    cnblog.Diggs = node2.InnerText;
                                }
                                if (node2.Name.Equals("views"))
                                {
                                    cnblog.Views = "[" + node2.InnerText + "]";
                                }
                                if (node2.Name.Equals("comments"))
                                {
                                    cnblog.Comments = "[" + node2.InnerText + "]";
                                }
                            }
                            cnblog.BlogApp = cnblog.AuthorHome.Split('/')[3];
                            listBlogs.Add(cnblog);
                        }
                    }
                    return listBlogs;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 十天推荐榜
        /// </summary>
        /// <param name="itemCount"></param>
        /// <returns></returns>
        public async static Task<List<CnBlog>> Get10TopDiggsAysnc(int itemCount)
        {
            try
            {
                string url = string.Format(_url10Diggs, itemCount);
                string xml = await BaseService.SendGetRequest(url);

                if (xml != null)
                {
                    List<CnBlog> listBlogs = new List<CnBlog>();
                    CnBlog cnblog;
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xml);
                    XmlNode feed = doc.ChildNodes[1];
                    foreach (XmlNode node in feed.ChildNodes)
                    {
                        if (node.Name.Equals("entry"))
                        {
                            cnblog = new CnBlog();
                            foreach (XmlNode node2 in node.ChildNodes)
                            {
                                if (node2.Name.Equals("id"))
                                {
                                    cnblog.Id = node2.InnerText;
                                }
                                if (node2.Name.Equals("title"))
                                {
                                    cnblog.Title = node2.InnerText;
                                }
                                if (node2.Name.Equals("summary"))
                                {
                                    cnblog.Summary = node2.InnerText + "...";
                                }
                                if (node2.Name.Equals("published"))
                                {
                                    DateTime t = DateTime.Parse(node2.InnerText);
                                    cnblog.PublishTime = "发表于 " + t;
                                }
                                if (node2.Name.Equals("updated"))
                                {
                                    cnblog.UpdateTime = node2.InnerText;
                                }
                                if (node2.Name.Equals("author"))
                                {
                                    cnblog.AuthorName = node2.ChildNodes[0].InnerText;
                                    cnblog.AuthorHome = node2.ChildNodes[1].InnerText;
                                    if (node2.ChildNodes.Count == 3)
                                    {
                                        cnblog.AuthorAvator = node2.ChildNodes[2].InnerText.Equals("") ? "http://pic.cnblogs.com/avatar/simple_avatar.gif" : node2.ChildNodes[2].InnerText;
                                    }
                                    else
                                    {
                                        cnblog.AuthorAvator = "http://pic.cnblogs.com/avatar/simple_avatar.gif";
                                    }
                                }
                                if (node2.Name.Equals("link"))
                                {
                                    cnblog.BlogRawUrl = node2.Attributes["href"].Value;
                                }
                                if (node2.Name.Equals("diggs"))
                                {
                                    cnblog.Diggs = node2.InnerText;
                                }
                                if (node2.Name.Equals("views"))
                                {
                                    cnblog.Views = "[" + node2.InnerText + "]";
                                }
                                if (node2.Name.Equals("comments"))
                                {
                                    cnblog.Comments = "[" + node2.InnerText + "]";
                                }
                            }
                            cnblog.BlogApp = cnblog.AuthorHome.Split('/')[3];
                            listBlogs.Add(cnblog);
                        }
                    }
                    return listBlogs;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 获取推荐博主
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async static Task<List<CnBloger>> GetTopDiggBlogers(int pageIndex,int pageSize)
        {
            try
            {
                string url = string.Format(_urlRecommendBloger, pageIndex, pageSize);
                string xml = await BaseService.SendGetRequest(url);
                if (xml != null)
                {
                    List<CnBloger> listBlogers = new List<CnBloger>();
                    CnBloger bloger;
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xml);

                    XmlNode feed = doc.ChildNodes[1];
                    foreach (XmlNode node in feed.ChildNodes)
                    {
                        if (node.Name.Equals("entry"))
                        {
                            bloger = new CnBloger();
                            foreach (XmlNode node2 in node.ChildNodes)
                            {
                                if (node2.Name.Equals("id"))
                                {
                                    bloger.BlogerHome = node2.InnerText;
                                }
                                if(node2.Name.Equals("title"))
                                {
                                    bloger.BlogerName = node2.InnerText;
                                }
                                if(node2.Name.Equals("updated"))
                                {
                                    DateTime t = DateTime.Parse(node2.InnerText);
                                    bloger.UpdateTime = "最后更新 " + t;
                                }
                                if(node2.Name.Equals("blogapp"))
                                {
                                    bloger.BlogApp = node2.InnerText;
                                }
                                if(node2.Name.Equals("avatar"))
                                {
                                    bloger.BlogerAvator = node2.InnerText.Equals("") ? "http://pic.cnblogs.com/avatar/simple_avatar.gif" : node2.InnerText;
                                }
                                if(node2.Name.Equals("postcount"))
                                {
                                    bloger.PostCount = "发表博客 " + node2.InnerText + "篇";
                                }
                            }
                            listBlogers.Add(bloger);
                        }
                    }
                    return listBlogers;
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
