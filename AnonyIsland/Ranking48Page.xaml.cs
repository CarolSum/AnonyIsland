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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace AnonyIsland
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Ranking48Page : Page
    {
        private CN48TopViewList _list_48Views;

        public Ranking48Page()
        {
            this.InitializeComponent();
            initializeFrostedGlass(bgGrid);
            _list_48Views = new CN48TopViewList();
            _list_48Views.DataLoaded += _list_blogs_DataLoaded;
            _list_48Views.DataLoading += _list_blogs_DataLoading;
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
        private void View48ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(BlogContentPage), new object[] { e.ClickedItem });
        }
    }
}
