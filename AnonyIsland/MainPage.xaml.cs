using AnonyIsland.Data;
using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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


        //private CNBlogList _list_blogs;
        private bool flag = false;
        /// <summary>
        /// 构造方法
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
            ContentFrame.Navigate(typeof(HomePage));
            //mainNavigationList.SelectedIndex = 1;
            //initializeFrostedGlass(bgGrid);
            //BlogsListView.ItemsSource = _list_blogs = new CNBlogList();
            //_list_blogs.DataLoaded += _list_blogs_DataLoaded;
            //_list_blogs.DataLoading += _list_blogs_DataLoading;
        }

        ///// <summary>
        ///// 博客列表开始加载
        ///// </summary>
        //private void _list_blogs_DataLoading()
        //{
        //    Loading.IsActive = true;
        //}
   
        //private void _list_blogs_DataLoaded()
        //{
        //    Loading.IsActive = false;
        //}
       

        //private void initializeFrostedGlass(UIElement glassHost)
        //{
        //    Visual hostVisual = ElementCompositionPreview.GetElementVisual(glassHost);
        //    Compositor compositor = hostVisual.Compositor;
        //    var glassEffect = new GaussianBlurEffect
        //    {
        //        BlurAmount = 10.0f,
        //        BorderMode = EffectBorderMode.Hard,
        //        Source = new ArithmeticCompositeEffect
        //        {
        //            MultiplyAmount = 0,
        //            Source1Amount = 0.3f,
        //            Source2Amount = 0.3f,
        //            Source1 = new CompositionEffectSourceParameter("backdropBrush"),
        //            Source2 = new ColorSourceEffect
        //            {
        //                Color = Color.FromArgb(255, 245, 245, 245)
        //            }
        //        }
        //    };
        //    var effectFactory = compositor.CreateEffectFactory(glassEffect);
        //    var backdropBrush = compositor.CreateBackdropBrush();
        //    var effectBrush = effectFactory.CreateBrush();
        //    effectBrush.SetSourceParameter("backdropBrush", backdropBrush);
        //    var glassVisual = compositor.CreateSpriteVisual();
        //    glassVisual.Brush = effectBrush;
        //    ElementCompositionPreview.SetElementChildVisual(glassHost, glassVisual);
        //    var bindSizeAnimation = compositor.CreateExpressionAnimation("hostVisual.Size");
        //    bindSizeAnimation.SetReferenceParameter("hostVisual", hostVisual);
        //    glassVisual.StartAnimation("Size", bindSizeAnimation);
        //}

        //// 点击blogitem跳转到详情页
        //private void BlogsListView_ItemClick(object sender, ItemClickEventArgs e)
        //{
        //    this.Frame.Navigate(typeof(BlogContentPage), new object[] { e.ClickedItem });
        //}


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

        private async void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                if (!flag)
                {
                    flag = true;
                    SettingDialog st = new SettingDialog(this);
                    await st.ShowAsync();
                    flag = false;
                }
            }
            else
            {
                // find NavigationViewItem with Content that equals InvokedItem
                var item = sender.MenuItems.OfType<NavigationViewItem>().First(x => (string)x.Content == (string)args.InvokedItem);
                NavView_Navigate(item);
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

                case "rank48":
                    ContentFrame.Navigate(typeof(Ranking48Page));
                    break;

                case "rank10":
                    ContentFrame.Navigate(typeof(Ranking10Page));
                    break;
                    
                case "rankNews":
                    ContentFrame.Navigate(typeof(RankingNewsPage));
                    break;
            }
        }

        private void ASB_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            string txt = args.QueryText;
            if(args.ChosenSuggestion == null && !txt.Equals(""))
            {
                ContentFrame.Navigate(typeof(SearchPage), new object[] { txt, 0 });
            }
        }
    }
}
