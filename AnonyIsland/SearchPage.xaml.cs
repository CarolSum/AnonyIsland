using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using AnonyIsland.HTTP;
using AnonyIsland.Models;
using AnonyIsland.Tools;

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
        private readonly ObservableCollection<CnBlog> _listBlogs = new ObservableCollection<CnBlog>();

        public SearchPage()
        {
            InitializeComponent();
            FrostedGlassEffect.Initialize(bgGrid);
        }
        
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            object[] parameters = e.Parameter as object[];
            if (parameters != null && parameters.Length == 1)
            {
                string txt = parameters[0].ToString(); //关键字
                List<CnBlog> searchBlogs = await SearchService.SearchBlogs(txt, 1);
                if (searchBlogs != null)
                {
                    _listBlogs.Clear();
                    searchBlogs.ForEach(b => _listBlogs.Add(b));
                    Loading.IsActive = false;
                }
            }
            else
            {
                Loading.IsActive = false;
            }
        }

        private void BlogsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(BlogContentPage), new[] { e.ClickedItem });
        }
    }
}
