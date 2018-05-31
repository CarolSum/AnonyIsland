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
    public sealed partial class UserHome : Page
    {
        /// <summary>
        /// 当前博主的blog_app
        /// </summary>
        private string _blog_app;

        /// <summary>
        /// 当前页面加载的博客列表
        /// </summary>
        private CNUserBlogList _list_blogs;

        public UserHome()
        {
            this.InitializeComponent();
            FrostedGlassEffect.Initialize(bgGrid);
        }

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            object[] parameters = e.Parameter as object[];
            if (parameters != null)
            {
                if (parameters.Length == 2) //blogapp  nickname
                {
                    _blog_app = parameters[0].ToString();
                    PageTitle.Text = parameters[1].ToString() + " 的博客";

                    BlogsListView.ItemsSource = _list_blogs = new CNUserBlogList(_blog_app);

                    _list_blogs.DataLoaded += () => Loading.IsActive = false;
                    _list_blogs.DataLoading += () => Loading.IsActive = true;
                }
            }
        }

        /// <summary>
        /// 点击查看博客正文
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BlogsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(BlogContentPage), new object[] {e.ClickedItem});
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
    }
}
