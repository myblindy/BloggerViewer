﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BloggerViewer.Utils;
using Google.Apis.Blogger.v3.Data;

namespace BloggerViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<PostWrapper> Posts { get; private set; } = new ObservableCollection<PostWrapper>();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await BloggerWrapper.AuthenticateAsync();
            foreach (var post in await BloggerWrapper.GetPostsAsync())
                Posts.Add(post);
        }
    }
}
