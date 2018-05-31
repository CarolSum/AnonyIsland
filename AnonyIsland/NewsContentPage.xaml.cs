using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.ApplicationModel.DataTransfer;
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
    public sealed partial class NewsContentPage : Page
    {
        private static string _image_bridge = "http://sysu.at:9011/img?url=";

        private void HideScrollbar(ref string html)
        {
            html += "<style>body{-ms-overflow-style:none;}</style>";
        }

        /// <summary>
        /// 当前显示新闻
        /// </summary>
        private CNNews _news;
        private string _commentsTotalHtml = "";

        public NewsContentPage()
        {
            this.InitializeComponent();
            RegisterForShare();
        }
        private void RegisterForShare()
        {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager,
                DataRequestedEventArgs>(this.ShareLinkHandler);
        }

        private void ShareLinkHandler(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataRequest request = args.Request;
            request.Data.Properties.Title = "分享新闻";
            request.Data.Properties.Description = "向好友分享这篇新闻";
            request.Data.SetWebLink(new Uri(_news.NewsRawUrl));
        }

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="e"></param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Object[] parameters = e.Parameter as Object[];
            if(parameters != null && parameters.Length == 1)
            {
                _news = parameters[0] as CNNews;

                BlogTitle.Text = _news.Title;
                NewsSource.Text = _news.SourceName;
                PublishTime.Text = _news.PublishTime;
                Diggs.Text = "[" + _news.Diggs + "]";
                Views.Text = _news.Views;
                Comments.Text = _news.Comments;

                string news_content = await NewsService.GetNewsContentAsync(_news.ID);

                if(news_content != null)
                {
                    if (App.Theme == ApplicationTheme.Dark)  //暗主题
                    {
                        news_content += "<style>body{background-color:black;color:white;}</style>";
                    }

                    //string pattern = "<img src=\"(.*)\"";
                    //news_content = Regex.Replace(news_content, pattern, m => $"<img src=\"{_image_bridge}{m.Groups[1].Value}\"");
                    HideScrollbar(ref news_content);
                    NewsContent.NavigateToString(news_content);
                }

                // 获取评论数据
                _commentsTotalHtml = CommentTool.BaseChatHtml;
                if (App.Theme == ApplicationTheme.Dark)
                {
                    _commentsTotalHtml += "<style>body{background-color:black;color:white;}</style>";
                }
                HideScrollbar(ref _commentsTotalHtml);
                NewsComment.NavigateToString(_commentsTotalHtml);

                List<CNNewsComment> refresh_comments = await NewsService.GetNewsCommentsAysnc(_news.ID, 1, 200);

                if (refresh_comments != null)
                {
                    string comments = "";
                    foreach (CNNewsComment comment in refresh_comments)
                    {
                        comments += CommentTool.Receive(comment.AuthorAvatar,
                        comment.AuthorName,
                        comment.Content, comment.PublishTime, comment.ID);
                    }
                    comments += "<a id='ok'></a>";

                    _commentsTotalHtml = _commentsTotalHtml.Replace("<a id='ok'></a>", "") + comments + "<a id='ok'></a>";

                    HideScrollbar(ref _commentsTotalHtml);
                    NewsComment.NavigateToString(_commentsTotalHtml);

                    Loading.IsActive = false;
                }

                Loading.IsActive = false;
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

        /// <summary>
        /// 点击标题栏后退
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
        /// 分享
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Share_Click(object sender, RoutedEventArgs e)
        {
            Windows.ApplicationModel.DataTransfer.DataTransferManager.ShowShareUI();
        }
    }
}
