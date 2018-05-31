using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
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
    public sealed partial class BlogCommentPage : Page
    {
        string _totalHtml = "";
        CNBlog _blog;
        string _at_comment_id = "";

        public BlogCommentPage()
        {
            this.InitializeComponent();
            if (App.AlwaysShowNavigation)
            {
                Home.Visibility = Visibility.Collapsed;
            }
            
        }

        private void HideScrollbar(ref string html)
        {
            html += "<style>body{-ms-overflow-style:none;}</style>";
        }

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="e"></param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            object[] parameters = e.Parameter as object[];
            if (parameters != null && parameters.Length == 1)
            {
                _blog = parameters[0] as CNBlog;
                BlogTitle.Text = _blog.Title;
                Author.Content = _blog.AuthorName;
                PubishTime.Text = _blog.PublishTime;

                _totalHtml = ChatBoxTool.BaseChatHtml;
                if (App.Theme == ApplicationTheme.Dark)
                {
                    _totalHtml += "<style>body{background-color:black;color:white;}</style>";
                }

                HideScrollbar(ref _totalHtml);
                BlogComment.NavigateToString(_totalHtml);
                List<CNBlogComment> list_comments = await BlogService.GetBlogCommentsAsync(_blog.ID, 1, 199);

                if(list_comments != null)
                {
                    string comments = "";
                    foreach (CNBlogComment comment in list_comments)
                    {
                        comments += ChatBoxTool.Receive(comment.AuthorAvatar,
                            comment.AuthorName == _blog.AuthorName ? "[博主]" + _blog.AuthorName : comment.AuthorName,
                            comment.Content, comment.PublishTime, comment.ID);
                    }

                    _totalHtml = _totalHtml.Replace("<a id='ok'></a>", "") + comments + "<a id='ok'></a>";
                    HideScrollbar(ref _totalHtml);
                    BlogComment.NavigateToString(_totalHtml);
                    Loading.IsActive = false;
                }
            }
        }
        /// <summary>
        /// 点击后退
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }
        /// <summary>
        /// 点击博主昵称 转到博客主页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Author_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(UserHome), new object[] { _blog.BlogApp, _blog.AuthorName });
        }
        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            _totalHtml = ChatBoxTool.BaseChatHtml;
            if (App.Theme == ApplicationTheme.Dark)
            {
                _totalHtml += "<style>body{background-color:black;color:white;}</style>";
            }
            HideScrollbar(ref _totalHtml);
            BlogComment.NavigateToString(_totalHtml);
            Loading.IsActive = true;
            List<CNBlogComment> list_comments = await BlogService.GetBlogCommentsAsync(_blog.ID, 1, 199);

            if (list_comments != null)
            {
                string comments = "";
                foreach (CNBlogComment comment in list_comments)
                {
                    comments += ChatBoxTool.Receive(comment.AuthorAvatar,
                        comment.AuthorName == _blog.AuthorName ? "[博主]" + _blog.AuthorName : comment.AuthorName,
                        comment.Content, comment.PublishTime, comment.ID);
                }

                _totalHtml = _totalHtml.Replace("<a id='ok'></a>", "") + comments + "<a id='ok'></a>";

                HideScrollbar(ref _totalHtml);
                BlogComment.NavigateToString(_totalHtml);
                Loading.IsActive = false;
            }
        }
     
    }
}
