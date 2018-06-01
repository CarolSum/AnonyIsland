using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace AnonyIsland
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //private CNBlogList _list_blogs;
        private bool _flag;
        /// <summary>
        /// 构造方法
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            ContentFrame.Navigate(typeof(HomePage));
        }

        private async void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                if (!_flag)
                {
                    _flag = true;
                    SettingDialog st = new SettingDialog(this);
                    await st.ShowAsync();
                    _flag = false;
                }
            }
            else
            {
                // find NavigationViewItem with Content that equals InvokedItem
                var item = sender.MenuItems.OfType<NavigationViewItem>().First(x => (string)x.Content == (string)args.InvokedItem);
                NavView_Navigate(item);
            }
        }

        private void NavView_Navigate(NavigationViewItem item)
        {
            switch (item.Tag)
            {
                case "home":
                    ContentFrame.Navigate(typeof(HomePage));
                    break;

                case "news":
                    ContentFrame.Navigate(typeof(NewsPage));
                    break;

                case "rank48":
                    ContentFrame.Navigate(typeof(Ranking48Page));
                    break;

                case "rank10":
                    ContentFrame.Navigate(typeof(Ranking10Page));
                    break;
                    
                case "rankNews":
                    ContentFrame.Navigate(typeof(RankingNewsPage));
                    break;
            }
        }

        private async void ASB_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            string txt = args.QueryText;
            if(args.ChosenSuggestion == null && !txt.Equals(""))
            {
                if (!App.HaveDoSearch)
                {
                    ContentFrame.Navigate(typeof(SearchPage), new object[] { txt });
                    App.HaveDoSearch = true;
                    await Task.Delay(100);
                }
                ContentFrame.Navigate(typeof(SearchPage), new object[] { txt });
            }
        }
    }
}
