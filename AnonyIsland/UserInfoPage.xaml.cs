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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using AnonyIsland.Data;
using AnonyIsland.HTTP;
using AnonyIsland.Models;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace AnonyIsland
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class UserInfoPage : Page
    {
        /// <summary>
        /// 用户的粉丝
        /// </summary>
        private CNFollowerList _list_followers;
        /// <summary>
        /// 用户的关注
        /// </summary>
        private CNFolloweeList _list_followees;
        /// <summary>
        /// 当前查看的用户
        /// </summary>
        private CNUserInfo _user;

        public UserInfoPage()
        {
            this.InitializeComponent();
            if (App.AlwaysShowNavigation)
            {
                Home.Visibility = Visibility.Collapsed;
            }
        }
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="e"></param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            object[] parameters = e.Parameter as object[];
            if (parameters != null && parameters.Length == 1)
            {
                _user = await UserService.GetUserInfo(parameters[0].ToString());
                if (_user != null)
                {
                    PageTitle.Text = _user.Name + " 的信息";
                    BitmapImage bi = new BitmapImage { UriSource = new Uri(_user.Avatar) };
                    UserAvatar.Source = bi;

                    UserName.Text = _user.Name;
                    Age.Text = _user.Age;
                    Followees.Text = _user.Followees;
                    Followers.Text = _user.Followers;
                    BlogHome.Content = _user.BlogHome;
                    Loading.IsActive = false;


                    ListFollowees.ItemsSource = _list_followees = new CNFolloweeList(_user.BlogApp);
                    ListFollowers.ItemsSource = _list_followers = new CNFollowerList(_user.BlogApp);

                    _list_followees.DataLoaded += _list_followees_DataLoaded;
                    _list_followees.DataLoading += _list_followees_DataLoading;

                    _list_followers.DataLoaded += _list_followers_DataLoaded;
                    _list_followers.DataLoading += _list_followers_DataLoading;
                }
            }
        }

        /// <summary>
        /// 后退
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if(this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }

        /// <summary>
        /// 点击用户图标  转到博客主页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserAvatar_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (_user != null)
            {
                this.Frame.Navigate(typeof(UserHome), new object[] { _user.BlogApp, _user.Name });
            }
        }
        /// <summary>
        /// 点击主页链接 转到博客主页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BlogHome_Click(object sender, RoutedEventArgs e)
        {
            if (_user != null)
            {
                this.Frame.Navigate(typeof(UserHome), new object[] { _user.BlogApp, _user.Name });
            }
        }
        /// <summary>
        /// 点击粉丝、关注 列表项  转到个人信息页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(UserInfoPage), new object[] { (e.ClickedItem as CNUserInfo).BlogApp });
        }
        /// <summary>
        /// 点击粉丝、关注 列表中的昵称  转到博客主页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(UserHome), new object[] { (sender as HyperlinkButton).Tag, (sender as HyperlinkButton).Content });
        }


        private void _list_followers_DataLoading()
        {
            Loading.IsActive = true;
        }

        private void _list_followers_DataLoaded()
        {
            Loading.IsActive = false;
            FollowerCount.Text = _list_followers.TotalCount.ToString();
        }

        private void _list_followees_DataLoading()
        {
            Loading.IsActive = true;
        }

        private void _list_followees_DataLoaded()
        {
            Loading.IsActive = false;
            FolloweeCount.Text = _list_followees.TotalCount.ToString();
        }

        /// <summary>
        /// 打开主菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Home_Click(object sender, RoutedEventArgs e)
        {
            ((Window.Current.Content as Frame).Content as MainPage).ShowNavigationBarOneTime();
        }
    }
}
