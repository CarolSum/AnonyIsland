using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using AnonyIsland.HTTP;
using AnonyIsland.Models;
using AnonyIsland.Tools;

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
        private CnBlog _blog;
        string _commentHtml = "";

        private void HideScrollbar(ref string html)
        {
            html += "<style>body{-ms-overflow-style:none;}</style>";
        }

        public BlogContentPage()
        {
            InitializeComponent();
            RegisterForShare();
        }

        private void RegisterForShare()
        {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += ShareLinkHandler;
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
                if(parameters.Length==1 && (parameters[0] as CnBlog)!=null)
                {
                    _blog = parameters[0] as CnBlog;

                    BlogTitle.Text = _blog.Title;
                    AuthorName.Content = _blog.AuthorName;
                    PublishTime.Text = _blog.PublishTime;
                    Views.Text = _blog.Views;
                    Diggs.Text = "["+_blog.Diggs + "]";
                    Comments.Text = _blog.Comments;
                    BitmapImage bi = new BitmapImage { UriSource = new Uri(_blog.AuthorAvator) };
                    Avatar.Source = bi;
                    AuthorName.Tag = _blog.BlogApp;
                    string blogBody = await BlogService.GetBlogContentAsync(_blog.Id);
                    if (blogBody != null)
                    {
                        HideScrollbar(ref blogBody);
                        BlogContent.NavigateToString(blogBody);
                    }

                    // 获取评论数据
                    _commentHtml = CommentTool.BaseChatHtml;
                    HideScrollbar(ref _commentHtml);
                    BlogComment.NavigateToString(_commentHtml);
                    List<CnBlogComment> listComments = await BlogService.GetBlogCommentsAsync(_blog.Id, 1, 199);

                    if (listComments != null)
                    {
                        string comments = "";
                        foreach (CnBlogComment comment in listComments)
                        {
                            comments += CommentTool.Receive(comment.AuthorAvatar,
                                comment.AuthorName == _blog.AuthorName ? "[博主]" + _blog.AuthorName : comment.AuthorName,
                                comment.Content, comment.PublishTime, comment.Id);
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
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private void CommentUserClik_Handler(Object sender, NotifyEventArgs e)
        {
            if (e.Value != null)
            {
                string[] args = e.Value.Split(new[] { "-" }, StringSplitOptions.None);
                if (args.Length == 2)
                {
                    Frame.Navigate(typeof(UserHomePage), new object[] { args[0], args[1] });
                }
            }
        }

        private void AuthorName_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(UserHomePage), new[] { (sender as HyperlinkButton).Tag.ToString(),(sender as HyperlinkButton).Content });
        }


        private void Share_Click(object sender, RoutedEventArgs e)
        {
            DataTransferManager.ShowShareUI();
        }
    }
}
