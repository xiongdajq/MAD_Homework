﻿using MAD_HW4.ViewModels;
using System.Collections;
using System.Diagnostics;
using Windows.ApplicationModel;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MAD_HW4
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page {
        private MainAdaptiveViewModel mainAdaptiveVM;
        //private int selectedItemIndex = -1;
        //private Todo selectedItem = null;

        public MainPage() {
            InitializeComponent();
            mainAdaptiveVM = new MainAdaptiveViewModel();
            Application.Current.Resuming += App_Resuming;
            Application.Current.Suspending += App_Suspending;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(360, 120));
            SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
            // Bind view adapter
            mainAdaptiveVM.PropertyChanged += MainAdaptiveVM_PropertyChanged;
        }
        
        private void MainAdaptiveVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // Bind back button
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = mainAdaptiveVM.ShowBackButton
                ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
            // Pass data to EditPage
            if (e.PropertyName == "SelectedItemIndex" && (sender as MainAdaptiveViewModel).SelectedItemIndex >= 0)
            {
                (EditFrame.Content as EditPage).ChangeEditingTodoData(TodoViewModel.getInstance().Todos[(sender as MainAdaptiveViewModel).SelectedItemIndex]);
            }
        }

        /*private bool CanGoBack {
            get {
                return EditFrame.Visibility == Visibility.Visible && Grid.GetColumn(EditFrame) == 0;
            }
        }*/

        private void App_BackRequested(object sender, BackRequestedEventArgs e) {
            //Frame rootFrame = Window.Current.Content as Frame;
            //if (rootFrame == null) return;
            //
            //if (CanGoBack && e.Handled == false) {
            //    e.Handled = true;
            GoBack();
            //}
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            JsonObject parameters = JsonObject.Parse(e.Parameter as string);
            if (parameters != null)
            {
                mainAdaptiveVM.FromString(parameters["MainAdaptiveState"].GetString());
            }
            ListFrame.Navigate(typeof(ListPage));
            EditFrame.Navigate(typeof(EditPage), e.Parameter);
            
        }

        private void App_Resuming(object sender, object e)
        {
            // Restore view states
            mainAdaptiveVM.FromString(ApplicationData.Current.LocalSettings.Values["MainAdaptiveState"] as string);
        }

        private void App_Suspending(object sender, SuspendingEventArgs e)
        {
            // Store Todo list to storage
            TodoViewModel.getInstance().SaveToStorage();
            // Store view states
            ApplicationData.Current.LocalSettings.Values["MainAdaptiveState"] = mainAdaptiveVM.ToString();
        }

        private void GoBack() {
            //Grid.SetColumn(EditFrame, 1);
            mainAdaptiveVM.SelectedItemIndex = -1;
            //UpdateButtons();
        }

        private void MainPage_SizeChanged(object sender, SizeChangedEventArgs e) {
            //UpdateButtons();
            mainAdaptiveVM.ScreenWidth = e.NewSize.Width > 720 ? ScreenWidthEnum.Wide : ScreenWidthEnum.Narrow;
        }

        //private void UpdateButtons() {
            /*SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = CanGoBack
                ? AppViewBackButtonVisibility.Visible
                : AppViewBackButtonVisibility.Collapsed;
            SaveButton.Visibility = !CanGoBack && EditFrame.Visibility == Visibility.Collapsed
                ? Visibility.Collapsed
                : Visibility.Visible;
            ResetButton.Visibility = !CanGoBack && EditFrame.Visibility == Visibility.Collapsed
                ? Visibility.Collapsed
                : Visibility.Visible;*/
        //}

        public void OnTodoItemClick(object sender, ItemClickEventArgs e) {
            /*selectedItem = e.ClickedItem as Todo;
            selectedItemIndex = TodoViewModel.getInstance().Todos.IndexOf(selectedItem);

            Frame rootFrame = Window.Current.Content as Frame;
            Grid.SetColumn(EditFrame, rootFrame.ActualWidth > 720 ? 1 : 0);
            EditFrame.Visibility = Visibility.Visible;

            EditPage editPage = EditFrame.Content as EditPage;
            editPage.UpdateView(e.ClickedItem as Todo);

            mainAdaptiveVM.SelectedItemIndex = selectedItemIndex;*/
        }

        public void OnSelectionChanged(int index) {
            //DeleteButton.Visibility = index >= 0 ? Visibility.Visible : Visibility.Collapsed;
            mainAdaptiveVM.SelectedItemIndex = index;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e) {
            // Prevent creating new Todo item, since local storage has not been implemented.
            return;
            /*
            TodoViewModel.getInstance().Todos.Add(new Todo());

            selectedItemIndex = TodoViewModel.getInstance().Todos.Count - 1;
            selectedItem = TodoViewModel.getInstance().Todos[selectedItemIndex];

            ListPage listPage = ListFrame.Content as ListPage;
            listPage.setSelected(selectedItemIndex);

            //Frame rootFrame = Window.Current.Content as Frame;
            //Grid.SetColumn(EditFrame, rootFrame.ActualWidth > 720 ? 1 : 0);
            //EditFrame.Visibility = Visibility.Visible;

            EditPage editPage = EditFrame.Content as EditPage;
            editPage.UpdateView(selectedItem);*/
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e) {
            TodoViewModel.getInstance().Todos[mainAdaptiveVM.SelectedItemIndex].CloneFrom((EditFrame.Content as EditPage).DisplayTodo);
            if (mainAdaptiveVM.ScreenWidth == ScreenWidthEnum.Narrow)
            {
                GoBack();
            }
            /*EditPage editPage = EditFrame.Content as EditPage;
            selectedItem.Title = editPage.DisplayTodo.Title;
            selectedItem.Detail = editPage.DisplayTodo.Detail;
            selectedItem.DueDate = editPage.DisplayTodo.DueDate;
            selectedItem.CoverSource = editPage.DisplayTodo.CoverSource;
            selectedItem.Done = editPage.DisplayTodo.Done;*/
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e) {
            (EditFrame.Content as EditPage).ChangeEditingTodoData(TodoViewModel.getInstance().Todos[mainAdaptiveVM.SelectedItemIndex]);
            /*EditPage editPage = EditFrame.Content as EditPage;
            editPage.DisplayTodo.Title = selectedItem.Title;
            editPage.DisplayTodo.Detail = selectedItem.Detail;
            editPage.DisplayTodo.DueDate = selectedItem.DueDate;
            editPage.DisplayTodo.CoverSource = selectedItem.CoverSource;
            editPage.DisplayTodo.Done = selectedItem.Done;*/
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Prevent creating new Todo item, since local storage has not been implemented.
            return;
            /*
            EditFrame.Visibility = Visibility.Collapsed;
            GoBack();
            if (selectedItemIndex >= 0)
            {
                TodoViewModel.getInstance().Todos.RemoveAt(selectedItemIndex);
            }*/
        }
    }
}
