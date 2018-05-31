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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace AnonyIsland
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        int _preSelectNavigation = -1;
        bool _ignoreNavigation = false;


        private CNBlogList _list_blogs;

        /// <summary>
        /// 构造方法
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
            //mainNavigationList.SelectedIndex = 1;
            ShowNavigationBar(App.AlwaysShowNavigation);
            initializeFrostedGlass(bgGrid);
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

        #region  事件处理程序
        /// <summary>
        /// 导航栏隐现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBoxItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //ListBoxItem tapped_item = sender as ListBoxItem;
            //if (tapped_item != null && tapped_item.Tag != null && tapped_item.Tag.ToString().Equals("0")) //汉堡按钮
            //{
            //    mainSplitView.IsPaneOpen = !mainSplitView.IsPaneOpen;
            //}
        }
        /// <summary>
        /// 导航
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mainNavigationList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (_ignoreNavigation)
            //{
            //    _ignoreNavigation = false;
            //    return;
            //}
            //ListBoxItem tapped_item = mainNavigationList.SelectedItems[0] as ListBoxItem;
            //if (tapped_item != null && tapped_item.Tag != null && tapped_item.Tag.ToString().Equals("1")) //首页
            //{
            //    mainSplitView.IsPaneOpen = false;
            //    _preSelectNavigation = mainNavigationList.SelectedIndex;
            //    mainFrame.Navigate(typeof(HomePage));
            //}
            //if (tapped_item != null && tapped_item.Tag != null && tapped_item.Tag.ToString().Equals("2")) //新闻
            //{
            //    mainSplitView.IsPaneOpen = false;
            //    _preSelectNavigation = mainNavigationList.SelectedIndex;
            //    mainFrame.Navigate(typeof(NewsPage));
            //}
            //if (tapped_item != null && tapped_item.Tag != null && tapped_item.Tag.ToString().Equals("3")) //排行榜
            //{
            //    mainSplitView.IsPaneOpen = false;
            //    _preSelectNavigation = mainNavigationList.SelectedIndex;
            //    mainFrame.Navigate(typeof(RankingPage));
            //}
            //if (tapped_item != null && tapped_item.Tag != null && tapped_item.Tag.ToString().Equals("6")) //收藏
            //{
            //    mainSplitView.IsPaneOpen = false;
            //    _preSelectNavigation = mainNavigationList.SelectedIndex;
            //    mainFrame.Navigate(typeof(CollectionPage));
            //}
            //if (tapped_item != null && tapped_item.Tag != null && tapped_item.Tag.ToString().Equals("7")) //搜索
            //{
            //    mainSplitView.IsPaneOpen = false;
            //    SearchDialog sd = new SearchDialog();
            //    ContentDialogResult result = await sd.ShowAsync();
            //    if(result == ContentDialogResult.Primary)  //确定
            //    {
            //        _preSelectNavigation = mainNavigationList.SelectedIndex;
            //        mainFrame.Navigate(typeof(SearchPage), new object[] { sd.KeyWords, sd.SearchType });
            //    }
            //    else  //取消
            //    {
            //        _ignoreNavigation = true;
            //        mainNavigationList.SelectedIndex = _preSelectNavigation;
            //    }
            //}
            //if (tapped_item != null && tapped_item.Tag != null && tapped_item.Tag.ToString().Equals("10")) //设置
            //{
            //    mainSplitView.IsPaneOpen = false;
            //    SettingDialog st = new SettingDialog(this);
            //    await st.ShowAsync();
            //    //
            //    mainNavigationList.SelectedIndex = 1;
            //}
        }
        #endregion

        /// <summary>
        /// 设置主页面导航显示方式
        /// </summary>
        /// <param name="show"></param>
        public void ShowNavigationBar(bool show)
        {
            //mainSplitView.DisplayMode = show ? SplitViewDisplayMode.CompactOverlay : SplitViewDisplayMode.Overlay;
        }
        /// <summary>
        /// 打开导航栏一次
        /// </summary>
        public void ShowNavigationBarOneTime()
        {
            //mainSplitView.IsPaneOpen = true;
        }

        private void initializeFrostedGlass(UIElement glassHost)
        {
            Visual hostVisual = ElementCompositionPreview.GetElementVisual(glassHost);
            Compositor compositor = hostVisual.Compositor;
            var glassEffect = new GaussianBlurEffect
            {
                BlurAmount = 10.0f,
                BorderMode = EffectBorderMode.Hard,
                Source = new ArithmeticCompositeEffect
                {
                    MultiplyAmount = 0,
                    Source1Amount = 0.7f,
                    Source2Amount = 0.3f,
                    Source1 = new CompositionEffectSourceParameter("backdropBrush"),
                    Source2 = new ColorSourceEffect
                    {
                        Color = Color.FromArgb(255, 245, 245, 245)
                    }
                }
            };
            var effectFactory = compositor.CreateEffectFactory(glassEffect);
            var backdropBrush = compositor.CreateHostBackdropBrush();
            var effectBrush = effectFactory.CreateBrush();
            effectBrush.SetSourceParameter("backdropBrush", backdropBrush);
            var glassVisual = compositor.CreateSpriteVisual();
            glassVisual.Brush = effectBrush;
            ElementCompositionPreview.SetElementChildVisual(glassHost, glassVisual);
            var bindSizeAnimation = compositor.CreateExpressionAnimation("hostVisual.Size");
            bindSizeAnimation.SetReferenceParameter("hostVisual", hostVisual);
            glassVisual.StartAnimation("Size", bindSizeAnimation);
        }

        // 点击blogitem跳转到详情页
        private void BlogsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(BlogContentPage), new object[] { e.ClickedItem });
        }


        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            // set the initial SelectedItem 
            foreach (NavigationViewItemBase item in NavView.MenuItems)
            {
                if (item is NavigationViewItem && item.Tag.ToString() == "home")
                {
                    NavView.SelectedItem = item;
                    break;
                }
            }
        }

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                ContentFrame.Navigate(typeof(HomePage));
            }
            else
            {
                // find NavigationViewItem with Content that equals InvokedItem
                var item = sender.MenuItems.OfType<NavigationViewItem>().First(x => (string)x.Content == (string)args.InvokedItem);
                NavView_Navigate(item as NavigationViewItem);
            }
        }

        private void NavView_Navigate(NavigationViewItem item)
        {
            switch (item.Tag)
            {
                case "home":
                    ContentFrame.Navigate(typeof(HomePage));
                    break;

                case "news":
                    ContentFrame.Navigate(typeof(NewsPage));
                    break;

                case "ranks":
                    ContentFrame.Navigate(typeof(RankingPage));
                    break;

                case "collections":
                    ContentFrame.Navigate(typeof(CollectionPage));
                    break;
            }
        }
    }
}
