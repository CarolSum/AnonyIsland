using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using AnonyIsland.Data;
using AnonyIsland.Tools;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace AnonyIsland
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class UserHomePage : Page
    {
        /// <summary>
        /// 当前博主的blog_app
        /// </summary>
        private string _blogApp;

        /// <summary>
        /// 当前页面加载的博客列表
        /// </summary>
        private CnUserBlogList _listBlogs;

        public UserHomePage()
        {
            InitializeComponent();
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
                    _blogApp = parameters[0].ToString();
                    PageTitle.Text = parameters[1] + " 的博客";

                    BlogsListView.ItemsSource = _listBlogs = new CnUserBlogList(_blogApp);

                    _listBlogs.DataLoaded += () => Loading.IsActive = false;
                    _listBlogs.DataLoading += () => Loading.IsActive = true;
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
            Frame.Navigate(typeof(BlogContentPage), new[] {e.ClickedItem});
        }

        /// <summary>
        /// 点击后退
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
    }
}
