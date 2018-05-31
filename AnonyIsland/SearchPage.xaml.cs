using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using AnonyIsland.HTTP;
using AnonyIsland.Models;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Composition;
using Microsoft.Graphics.Canvas.Effects;
using Windows.UI;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace AnonyIsland
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SearchPage : Page
    {
        /// <summary>
        /// 当前显示的博客列表
        /// </summary>
        private ObservableCollection<CNBlog> _list_blogs = new ObservableCollection<CNBlog>();

        public SearchPage()
        {
            this.InitializeComponent();
            initializeFrostedGlass(bgGrid);
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

      
        private void _list_blogs_DataLoading()
        {
            Loading.IsActive = true;
        }
    
        private void _list_blogs_DataLoaded()
        {
            Loading.IsActive = false;
        }

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="e"></param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            object[] parameters = e.Parameter as object[];
            if (parameters != null && parameters.Length == 2)
            {
                string txt = parameters[0].ToString();  //关键字
                List<CNBlog> refresh_blogs = await SearchService.SearchBlogs(txt, 1);
                if (refresh_blogs != null)
                {
                    _list_blogs.Clear();
                    refresh_blogs.ForEach((b) => _list_blogs.Add(b));
                    Loading.IsActive = false;
                }
            }
            else
            {
                Loading.IsActive = false;
            }
        }

        /// <summary>
        /// 页面离开
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }
     
    
        private void BlogsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(BlogContentPage), new object[] { e.ClickedItem });
        }
       

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await(new MessageDialog("先点击查看全文再推荐哟!")).ShowAsync();
        }

    }
}
