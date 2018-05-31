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
            initializeFrostedGlass(bgGrid);
            _list_news = new CNNewsList();
            _list_news.DataLoaded += _list_news_DataLoaded;
            _list_news.DataLoading += _list_news_DataLoading;
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
                    Source1Amount = 0.3f,
                    Source2Amount = 0.3f,
                    Source1 = new CompositionEffectSourceParameter("backdropBrush"),
                    Source2 = new ColorSourceEffect
                    {
                        Color = Color.FromArgb(255, 245, 245, 245)
                    }
                }
            };
            var effectFactory = compositor.CreateEffectFactory(glassEffect);
            var backdropBrush = compositor.CreateBackdropBrush();
            var effectBrush = effectFactory.CreateBrush();
            effectBrush.SetSourceParameter("backdropBrush", backdropBrush);
            var glassVisual = compositor.CreateSpriteVisual();
            glassVisual.Brush = effectBrush;
            ElementCompositionPreview.SetElementChildVisual(glassHost, glassVisual);
            var bindSizeAnimation = compositor.CreateExpressionAnimation("hostVisual.Size");
            bindSizeAnimation.SetReferenceParameter("hostVisual", hostVisual);
            glassVisual.StartAnimation("Size", bindSizeAnimation);
        }

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
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

        /// <summary>
        /// 新闻开始加载
        /// </summary>
        private void _list_news_DataLoading()
        {
            Loading.IsActive = true;
        }
        /// <summary>
        /// 新闻加载完毕
        /// </summary>
        private void _list_news_DataLoaded()
        {
            Loading.IsActive = false;
            //ListCount.Text = _list_news.TotalCount.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await (new MessageDialog("先点击查看全文再推荐哟!")).ShowAsync();
        }

    }
}
