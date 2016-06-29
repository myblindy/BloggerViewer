using System;
using System.Collections.Generic;
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

namespace BloggerViewer.Controls
{
    /// <summary>
    /// Interaction logic for FancyImage.xaml
    /// </summary>
    public partial class FancyImage : UserControl
    {
        public Uri Source
        {
            get { return (Uri)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(Uri), typeof(FancyImage), new PropertyMetadata((string)null, OnSourceChanged));

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var _this = (FancyImage)d;

            var img = new BitmapImage();
            img.DownloadCompleted += (_, __) =>
            {
                _this.imgDisplay.Visibility = Visibility.Visible;
                _this.LayoutRoot.Children.Remove(_this.pnlLoading);
            };
            img.BeginInit();
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.CreateOptions = BitmapCreateOptions.None;
            img.UriSource = _this.Source;
            img.EndInit();

            _this.imgDisplay.Source = img;
        }

        public FancyImage()
        {
            InitializeComponent();
        }
    }
}
