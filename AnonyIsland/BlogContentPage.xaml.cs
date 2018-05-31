using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Popups;
using Windows.ApplicationModel.DataTransfer;
using AnonyIsland.HTTP;
using AnonyIsland.Models;
using AnonyIsland.Tools;
using System.Diagnostics;
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
    public sealed partial class BlogContentPage : Page
    {
        /// <summary>
        /// 当前查看博客
        /// </summary>
        private CNBlog _blog;
        string _commentHtml = "";

        public BlogContentPage()
        {
            this.InitializeComponent();
            RegisterForShare();
            //initializeFrostedGlass(bgGrid);
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

        private void RegisterForShare()
        {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager,
                DataRequestedEventArgs>(this.ShareLinkHandler);
        }

       
        private void ShareLinkHandler(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataRequest request = args.Request;
            request.Data.Properties.Title = "分享博客";
            request.Data.Properties.Description = "向好友分享这篇博客";
            request.Data.SetWebLink(new Uri(_blog.BlogRawUrl));
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            object[] parameters = e.Parameter as object[];
            if (parameters != null)
            {
                if(parameters.Length==1 && (parameters[0] as CNBlog)!=null)
                {
                    _blog = parameters[0] as CNBlog;

                    BlogTitle.Text = _blog.Title;
                    AuthorName.Content = _blog.AuthorName;
                    PublishTime.Text = _blog.PublishTime;
                    Views.Text = _blog.Views;
                    Diggs.Text = "["+_blog.Diggs + "]";
                    Comments.Text = _blog.Comments;
                    BitmapImage bi = new BitmapImage { UriSource = new Uri(_blog.AuthorAvator) };
                    Avatar.Source = bi;
                    AuthorName.Tag = _blog.BlogApp;
                    string blog_body = await BlogService.GetBlogContentAsync(_blog.ID);
                    if (blog_body != null)
                    {
                        if (App.Theme == ApplicationTheme.Dark)  //暗主题
                        {
                            blog_body += "<style>body{background-color:black;color:white;}</style>";
                        }
                        BlogContent.NavigateToString(blog_body);
                    }

                    // 获取评论数据
                    _commentHtml = ChatBoxTool.BaseChatHtml;
                    if (App.Theme == ApplicationTheme.Dark)
                    {
                        _commentHtml += "<style>body{background-color:black;color:white;}</style>";
                    }
                    BlogComment.NavigateToString(_commentHtml);
                    List<CNBlogComment> list_comments = await BlogService.GetBlogCommentsAsync(_blog.ID, 1, 199);

                    if (list_comments != null)
                    {
                        string comments = "";
                        foreach (CNBlogComment comment in list_comments)
                        {
                            comments += ChatBoxTool.Receive(comment.AuthorAvatar,
                                comment.AuthorName == _blog.AuthorName ? "[博主]" + _blog.AuthorName : comment.AuthorName,
                                comment.Content, comment.PublishTime, comment.ID);
                        }

                        _commentHtml = _commentHtml.Replace("<a id='ok'></a>", "") + comments + "<a id='ok'></a>";
                        Debug.Write(_commentHtml);
                        BlogComment.NavigateToString(_commentHtml);
                    }

                    Loading.IsActive = false;
                }
            }
        }


        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }

        private void Comment_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BlogCommentPage), new object[] { _blog });
        }

  
        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            Loading.IsActive = true;
            string blog_body = await BlogService.GetBlogContentAsync(_blog.ID);
            if (blog_body != null)
            {
                BlogContent.NavigateToString(blog_body);
                Loading.IsActive = false;
            }
        }

   
        private void AuthorName_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(UserHome), new object[] { (sender as HyperlinkButton).Tag.ToString(),(sender as HyperlinkButton).Content });
        }


        private void Share_Click(object sender, RoutedEventArgs e)
        {
            Windows.ApplicationModel.DataTransfer.DataTransferManager.ShowShareUI();
        }
    }
}
