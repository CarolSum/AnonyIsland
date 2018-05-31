using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// “内容对话框”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上进行了说明

namespace AnonyIsland
{
    public sealed partial class SettingDialog : ContentDialog
    {
        readonly ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;
        MainPage _mainPage;
        public SettingDialog(MainPage mainPage)
        {
            InitializeComponent();
            _mainPage = mainPage;
            LoadConfig();
        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }
        /// <summary>
        /// 加载数目选择变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).SelectedIndex == -1)
            {
                return;
            }

            if ((sender as ComboBox).Name.Equals("NewsCount"))
            {
                _localSettings.Values["NewsCountOneTime"] = (e.AddedItems[0] as ComboBoxItem).Content.ToString();
                App.NewsCountOneTime = int.Parse(_localSettings.Values["NewsCountOneTime"].ToString());
            }
            else
            {
                _localSettings.Values["BlogCountOneTime"] = (e.AddedItems[0] as ComboBoxItem).Content.ToString();
                App.BlogCountOneTime = int.Parse(_localSettings.Values["BlogCountOneTime"].ToString());
            }
        }
      
        private void LoadConfig()
        {
            if (NewsCount.Items == null)
            {
                return;
            }
            for (int i = 0; i < NewsCount.Items.Count; i++)
            {
                if ((NewsCount.Items[i] as ComboBoxItem).Content.ToString() == App.NewsCountOneTime.ToString())
                {
                    NewsCount.SelectedIndex = i;
                    break;
                }
            }
            for (int i = 0; i < BlogCount.Items.Count; i++)
            {
                if ((BlogCount.Items[i] as ComboBoxItem).Content.ToString() == App.BlogCountOneTime.ToString())
                {
                    BlogCount.SelectedIndex = i;
                    break;
                }
            }
        }
    }
}
