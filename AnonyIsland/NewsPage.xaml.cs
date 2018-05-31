using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using AnonyIsland.HTTP;
using AnonyIsland.Models;
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
using AnonyIsland.Data;
using Windows.UI.Composition;
using Windows.UI.Xaml.Hosting;
using Microsoft.Graphics.Canvas.Effects;
using Windows.UI;
using AnonyIsland.Tools;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace AnonyIsland
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class NewsPage : Page
    {
        /// <summary>
        /// 当前页面加载的新闻列表
        /// </summary>
        private CNNewsList _list_news;
        public NewsPage()
        {
            this.InitializeComponent();
            FrostedGlassEffect.Initialize(bgGrid);
            _list_news = new CNNewsList();
            _list_news.DataLoaded += () => Loading.IsActive = false;
            _list_news.DataLoading += () => Loading.IsActive = true;
        }
        
        /// <summary>
        /// 点击查看新闻内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(NewsContentPage), new object[] { e.ClickedItem });
        }
    }
}
