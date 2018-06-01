using Windows.UI.Xaml.Controls;
using AnonyIsland.Data;
using AnonyIsland.Tools;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace AnonyIsland
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Ranking10Page : Page
    {
        private readonly Cn10TopDiggList _list10Views;

        public Ranking10Page()
        {
            InitializeComponent();
            FrostedGlassEffect.Initialize(bgGrid);
            _list10Views = new Cn10TopDiggList();
            _list10Views.DataLoaded += () => Loading.IsActive = false;
            _list10Views.DataLoading += () => Loading.IsActive = true;
        }

        // 点击blogitem跳转到详情页
        private void View10ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(BlogContentPage), new[] { e.ClickedItem });
        }
    }
}
