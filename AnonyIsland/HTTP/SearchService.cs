using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using AnonyIsland.Models;

namespace AnonyIsland.HTTP
{
    /// <summary>
    /// 搜索服务
    /// </summary>
    class SearchService
    {
        static readonly string _urlSearchBlogs = "http://zzk.cnblogs.com/s?w={0}&t=b&p={1}";  //blog_keywords page_index

        public static async Task<List<CnBlog>> SearchBlogs(string keywords,int pageIndex)
        {
            try
            {
                string url = string.Format(_urlSearchBlogs, keywords, pageIndex);
                string html = await BaseService.SendGetRequest(url);

                if (html != null)
                {
                    html = html.Split(new[] { "<div class=\"forflow\">" }, StringSplitOptions.None)[1]
                        .Split(new[] { "<div class=\"forflow\" id=\"sidebar\">" },StringSplitOptions.None)[0]
                        .Split(new[] {"<div id=\"paging_block\""},StringSplitOptions.None)[0];
                    html = "<?xml version=\"1.0\" encoding=\"utf - 8\" ?> " + "<result>" + html + "</result>";
                    List<CnBlog> listBlogs = new List<CnBlog>();
                    CnBlog blog;

                    XmlDocument doc = new XmlDocument();
                    
                    doc.LoadXml(html);

                    XmlNode searchItems = doc.ChildNodes[1];
                    if (searchItems != null)
                    {
                        foreach (XmlNode node in searchItems.ChildNodes)
                        {
                            blog = new CnBlog();
                            blog.Title = node.ChildNodes[0].InnerText;
                            blog.Summary = node.ChildNodes[2].InnerText;
                            blog.AuthorName = node.ChildNodes[4].ChildNodes[0].InnerText;
                            blog.AuthorHome = node.ChildNodes[4].ChildNodes[0].ChildNodes[0].Attributes["href"].Value;
                            blog.BlogApp = blog.AuthorHome.Split('/')[3];
                            blog.PublishTime = node.ChildNodes[4].ChildNodes[1].InnerText;
                            if(node.ChildNodes[4].ChildNodes[2]!=null)
                            {
                                if(node.ChildNodes[4].ChildNodes[2].InnerText.Contains("推荐"))
                                {
                                    blog.Diggs = node.ChildNodes[4].ChildNodes[2].InnerText.Split('(')[1].TrimEnd(')');
                                }
                                if (node.ChildNodes[4].ChildNodes[2].InnerText.Contains("评论"))
                                {
                                    blog.Comments = "[" + node.ChildNodes[4].ChildNodes[2].InnerText.Split('(')[1].TrimEnd(')') + "]";
                                }
                                if(node.ChildNodes[4].ChildNodes[2].InnerText.Contains("浏览"))
                                {
                                    blog.Views = "[" + node.ChildNodes[4].ChildNodes[2].InnerText.Split('(')[1].TrimEnd(')') + "]";
                                }
                            }
                            if (node.ChildNodes[4].ChildNodes[3] != null)
                            {
                                if (node.ChildNodes[4].ChildNodes[3].InnerText.Contains("推荐"))
                                {
                                    blog.Diggs = node.ChildNodes[4].ChildNodes[3].InnerText.Split('(')[1].TrimEnd(')');
                                }
                                if (node.ChildNodes[4].ChildNodes[3].InnerText.Contains("评论"))
                                {
                                    blog.Comments = "[" + node.ChildNodes[4].ChildNodes[3].InnerText.Split('(')[1].TrimEnd(')') + "]";
                                }
                                if (node.ChildNodes[4].ChildNodes[3].InnerText.Contains("浏览"))
                                {
                                    blog.Views = "[" + node.ChildNodes[4].ChildNodes[3].InnerText.Split('(')[1].TrimEnd(')') + "]";
                                }
                            }
                            if (node.ChildNodes[4].ChildNodes[4] != null)
                            {
                                if (node.ChildNodes[4].ChildNodes[4].InnerText.Contains("推荐"))
                                {
                                    blog.Diggs = node.ChildNodes[4].ChildNodes[4].InnerText.Split('(')[1].TrimEnd(')');
                                }
                                if (node.ChildNodes[4].ChildNodes[4].InnerText.Contains("评论"))
                                {
                                    blog.Comments = "[" + node.ChildNodes[4].ChildNodes[4].InnerText.Split('(')[1].TrimEnd(')') + "]";
                                }
                                if (node.ChildNodes[4].ChildNodes[4].InnerText.Contains("浏览"))
                                {
                                    blog.Views = "[" + node.ChildNodes[4].ChildNodes[4].InnerText.Split('(')[1].TrimEnd(')') + "]";
                                }
                            }
                            blog.BlogRawUrl = node.ChildNodes[5].InnerText;
                            blog.AuthorAvator = "http://pic.cnblogs.com/avatar/simple_avatar.gif";

                            string[] strs = blog.BlogRawUrl.Split('/');
                            blog.Id = strs[strs.Length - 1].Split('.')[0];

                            if (blog.Diggs == null)
                            {
                                blog.Diggs = "0";
                            }
                            if (blog.Comments == null)
                            {
                                blog.Comments = "[0]";
                            }
                            listBlogs.Add(blog);
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
    }
}
