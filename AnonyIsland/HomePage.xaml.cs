using Windows.UI.Xaml.Controls;
using AnonyIsland.Data;
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
        private readonly CnBlogList _listBlogs;
        public HomePage()
        {
            InitializeComponent();
            FrostedGlassEffect.Initialize(bgGrid);
            BlogsListView.ItemsSource = _listBlogs = new CnBlogList();
            _listBlogs.DataLoaded += _list_blogs_DataLoaded;
            _listBlogs.DataLoading += _list_blogs_DataLoading;
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
            Frame.Navigate(typeof(BlogContentPage), new[] { e.ClickedItem });
        }
    }
}
