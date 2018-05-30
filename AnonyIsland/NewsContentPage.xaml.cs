﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace AnonyIsland
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class NewsContentPage : Page
    {
        /// <summary>
        /// 当前显示新闻
        /// </summary>
        private CNNews _news;

        public NewsContentPage()
        {
            this.InitializeComponent();
            if (App.AlwaysShowNavigation)
            {
                Home.Visibility = Visibility.Collapsed;
            }
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
                    NewsContent.NavigateToString(news_content);
                }
                Loading.IsActive = false;
            }
        }
        /// <summary>
        /// 点击标题栏刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            Loading.IsActive = true;
            string news_content = await NewsService.GetNewsContentAsync(_news.ID);
            if (news_content != null)
            {
                if (App.Theme == ApplicationTheme.Dark)  //暗主题
                {
                    news_content += "<style>body{background-color:black;color:white;}</style>";
                }
                NewsContent.NavigateToString(news_content);
                Loading.IsActive = false;
            }
        }
        /// <summary>
        /// 点击标题栏查看评论
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Comment_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NewsCommentPage), new object[] { _news });
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
        /// 点击评论小图标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SymbolIcon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if ((sender as SymbolIcon).Symbol == Symbol.Comment)
            {
                this.Frame.Navigate(typeof(NewsCommentPage), new object[] { _news });
            }
        }

        /// <summary>
        /// 打开主菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Home_Click(object sender, RoutedEventArgs e)
        {
            ((Window.Current.Content as Frame).Content as MainPage).ShowNavigationBarOneTime();
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