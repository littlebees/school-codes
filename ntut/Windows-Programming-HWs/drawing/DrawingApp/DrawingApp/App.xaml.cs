using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace DrawingApp
{
    /// <summary>
    /// 提供應用程式專屬行為以補充預設的應用程式類別。
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// 初始化單一應用程式物件。這是第一行執行之撰寫程式碼，
        /// 而且其邏輯相當於 main() 或 WinMain()。
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// 在應用程式由終端使用者正常啟動時叫用。當啟動應用
        /// 將在例如啟動應用程式時使用以開啟特定檔案。
        /// </summary>
        /// <param name="e">關於啟動要求和處理序的詳細資料。</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {

                }
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// 在巡覽至某頁面失敗時叫用
        /// </summary>
        /// <param name="sender">導致巡覽失敗的框架</param>
        /// <param name="e">有關巡覽失敗的詳細資料</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// 在應用程式暫停執行時叫用。應用程式狀態會儲存起來，
        /// 但不知道應用程式即將結束或繼續，而且仍將記憶體
        /// 的內容保持不變。
        /// </summary>
        /// <param name="sender">暫停之要求的來源。</param>
        /// <param name="e">有關暫停之要求的詳細資料。</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }
    }
}
