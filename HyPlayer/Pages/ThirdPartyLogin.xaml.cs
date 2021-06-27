﻿using System;
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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace HyPlayer.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ThirdPartyLogin : Page
    {
        private readonly string QQLoginUri = @"https://graph.qq.com/oauth2.0/show?which=Login&display=pc&client_id=100495085&response_type=code&redirect_uri=https://music.163.com/back/qq&state=OBWNvfkvZv";
        private readonly string WXLoginUri = @"https://open.weixin.qq.com/connect/qrconnect?appid=wxe280063f5fb2528a&response_type=code&redirect_uri=https://music.163.com/back/weichat&scope=snsapi_login&state=HyPlayerUWP&lang=zh_CN#wechat_redirect";
        private readonly string WBLoginUri = @"https://api.weibo.com/oauth2/authorize?client_id=301575942&response_type=code&redirect_uri=http://music.163.com/back/weibo&scope=friendships_groups_read,statuses_to_me_read,follow_app_official_microblog&state=HyPlayerUWP";
        public ThirdPartyLogin()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            switch (e.Parameter as String)
            {
                case "QQ":
                    ThirdPartyLoginWebview.Navigate(new Uri(QQLoginUri));
                    break;
                case "WX":
                    ThirdPartyLoginWebview.Navigate(new Uri(WXLoginUri));
                    break;
                case "WB":
                    ThirdPartyLoginWebview.Navigate(new Uri(WBLoginUri));
                    break;
            }
        }

        private void ThirdPartyLoginWebview_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            if (sender.Source.ToString().Contains("music.163.com/back/sns"))
            {
                
                var cookies = new Windows.Web.Http.Filters.HttpBaseProtocolFilter().CookieManager.GetCookies(new Uri("https://music.163.com"));
                string cookiestring = string.Empty;
                foreach(Windows.Web.Http.HttpCookie cookie in cookies)
                {
                    System.Net.Cookie rescookie = new System.Net.Cookie();
                    rescookie.Name = cookie.Name;
                    rescookie.Value = cookie.Value;
                    rescookie.HttpOnly = cookie.HttpOnly;
                    rescookie.Domain = cookie.Domain;
                    rescookie.Secure = cookie.Secure;

                    rescookie.Expires = ((DateTimeOffset)cookie.Expires).DateTime;
                    rescookie.Path = cookie.Path;
                    Common.ncapi.Cookies.Add(rescookie);
                }
                Common.PageBase.LoginDone();
            }
        }
    }
}