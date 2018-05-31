using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Popups;
using Windows.ApplicationModel.DataTransfer;
using AnonyIsland.HTTP;
using AnonyIsland.Models;
using AnonyIsland.Tools;
using System.Diagnostics;
using Windows.UI.Composition;
using Windows.UI.Xaml.Hosting;
using Microsoft.Graphics.Canvas.Effects;
using Windows.UI;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace AnonyIsland
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class BlogContentPage : Page
    {
        /// <summary>
        /// 当前查看博客
        /// </summary>
        private CNBlog _blog;
        string _commentHtml = "";

        private void HideScrollbar(ref string html)
        {
            html += "<style>body{-ms-overflow-style:none;}</style>";
        }

        public BlogContentPage()
        {
            this.InitializeComponent();
            RegisterForShare();
        }

        private void RegisterForShare()
        {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += this.ShareLinkHandler;
        }

       
        private void ShareLinkHandler(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataRequest request = args.Request;
            request.Data.Properties.Title = "分享博客";
            request.Data.Properties.Description = "向好友分享这篇博客";
            request.Data.SetWebLink(new Uri(_blog.BlogRawUrl));
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            object[] parameters = e.Parameter as object[];
            if (parameters != null)
            {
                if(parameters.Length==1 && (parameters[0] as CNBlog)!=null)
                {
                    _blog = parameters[0] as CNBlog;

                    BlogTitle.Text = _blog.Title;
                    AuthorName.Content = _blog.AuthorName;
                    PublishTime.Text = _blog.PublishTime;
                    Views.Text = _blog.Views;
                    Diggs.Text = "["+_blog.Diggs + "]";
                    Comments.Text = _blog.Comments;
                    BitmapImage bi = new BitmapImage { UriSource = new Uri(_blog.AuthorAvator) };
                    Avatar.Source = bi;
                    AuthorName.Tag = _blog.BlogApp;
                    string blog_body = await BlogService.GetBlogContentAsync(_blog.ID);
                    if (blog_body != null)
                    {
                        HideScrollbar(ref blog_body);
                        BlogContent.NavigateToString(blog_body);
                    }

                    // 获取评论数据
                    _commentHtml = CommentTool.BaseChatHtml;
                    HideScrollbar(ref _commentHtml);
                    BlogComment.NavigateToString(_commentHtml);
                    List<CNBlogComment> list_comments = await BlogService.GetBlogCommentsAsync(_blog.ID, 1, 199);

                    if (list_comments != null)
                    {
                        string comments = "";
                        foreach (CNBlogComment comment in list_comments)
                        {
                            comments += CommentTool.Receive(comment.AuthorAvatar,
                                comment.AuthorName == _blog.AuthorName ? "[博主]" + _blog.AuthorName : comment.AuthorName,
                                comment.Content, comment.PublishTime, comment.ID);
                        }

                        _commentHtml = _commentHtml.Replace("<a id='ok'></a>", "") + comments + "<a id='ok'></a>";
                        Debug.Write(_commentHtml);
                        HideScrollbar(ref _commentHtml);
                        BlogComment.NavigateToString(_commentHtml);
                    }

                    Loading.IsActive = false;
                }
            }
        }


        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }

        private void CommentUserClik_Handler(Object sender, NotifyEventArgs e)
        {
            if (e.Value != null)
            {
                string[] args = e.Value.Split(new string[] { "-" }, StringSplitOptions.None);
                if (args.Length == 2)
                {
                    this.Frame.Navigate(typeof(UserHome), new object[] { args[0], args[1] });
                }
            }
        }

        private void AuthorName_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(UserHome), new object[] { (sender as HyperlinkButton).Tag.ToString(),(sender as HyperlinkButton).Content });
        }


        private void Share_Click(object sender, RoutedEventArgs e)
        {
            Windows.ApplicationModel.DataTransfer.DataTransferManager.ShowShareUI();
        }
    }
}
