using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace TweetScheduler.View.UserControls
{
    /// <summary>
    ///     Interaction logic for ViewTweetUserControl.xaml
    /// </summary>
    public partial class ViewTweetUserControl : UserControl
    {
        public ViewTweetUserControl()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}