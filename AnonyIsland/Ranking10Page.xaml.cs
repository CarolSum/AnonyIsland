using AnonyIsland.Data;
using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AnonyIsland.Tools;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace AnonyIsland
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Ranking10Page : Page
    {
        private CN10TopDiggList _list_10Views;

        public Ranking10Page()
        {
            this.InitializeComponent();
            FrostedGlassEffect.Initialize(bgGrid);
            _list_10Views = new CN10TopDiggList();
            _list_10Views.DataLoaded += () => Loading.IsActive = false;
            _list_10Views.DataLoading += () => Loading.IsActive = true;
        }

        // 点击blogitem跳转到详情页
        private void View10ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(BlogContentPage), new object[] { e.ClickedItem });
        }
    }
}
