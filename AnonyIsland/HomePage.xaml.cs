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
using AnonyIsland.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using AnonyIsland.Data;
using AnonyIsland.HTTP;
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
    public sealed partial class HomePage : Page
    {
        /// <summary>
        /// 首页博客列表
        /// </summary>
        private CNBlogList _list_blogs;
        public HomePage()
        {
            this.InitializeComponent();
            FrostedGlassEffect.Initialize(bgGrid);
            BlogsListView.ItemsSource = _list_blogs = new CNBlogList();
            _list_blogs.DataLoaded += _list_blogs_DataLoaded;
            _list_blogs.DataLoading += _list_blogs_DataLoading;
        }

        /// <summary>
        /// 博客列表开始加载
        /// </summary>
        private void _list_blogs_DataLoading()
        {
            Loading.IsActive = true;
        }
        /// <summary>
        /// 博客列表加载完毕
        /// </summary>
        private void _list_blogs_DataLoaded()
        {
            Loading.IsActive = false;
        }

        // 点击blogitem跳转到详情页
        private void BlogsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(BlogContentPage), new object[] { e.ClickedItem });
        }
    }
}
