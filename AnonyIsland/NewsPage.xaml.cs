using Windows.UI.Xaml.Controls;
using AnonyIsland.Data;
using AnonyIsland.Tools;

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
        private readonly CnNewsList _listNews;
        public NewsPage()
        {
            InitializeComponent();
            FrostedGlassEffect.Initialize(bgGrid);
            _listNews = new CnNewsList();
            _listNews.DataLoaded += () => Loading.IsActive = false;
            _listNews.DataLoading += () => Loading.IsActive = true;
        }
        
        /// <summary>
        /// 点击查看新闻内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(NewsContentPage), new[] { e.ClickedItem });
        }
    }
}
