using Windows.UI.Xaml.Controls;
using AnonyIsland.Data;
using AnonyIsland.Tools;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace AnonyIsland
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class RankingNewsPage : Page
    {
        private readonly CnTopNewsList _listNews;

        public RankingNewsPage()
        {
            InitializeComponent();
            FrostedGlassEffect.Initialize(bgGrid);
            _listNews = new CnTopNewsList();
            _listNews.DataLoaded += () => Loading.IsActive = false;
            _listNews.DataLoading += () => Loading.IsActive = true;
        }
       
        private void NewsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(NewsContentPage), new[] { e.ClickedItem });
        }
    }
}
