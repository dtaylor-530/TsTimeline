namespace SandBox
{
    public partial class MediaUserControl : UserControl
    {
        public MediaService ViewModel;

        public MediaUserControl()
        {
            InitializeComponent();


            this.Loaded += (s, e) =>
            {
                ViewModel = this.DataContext as MediaService;
            };
        }


        private void PlayPause_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.PlayPause();
            if (ViewModel.IsPlaying)
            {
                //mediaPlayer.Pause();
                PlayPauseButton.Content = "⏸";
            }
            else
            {
                //mediaPlayer.Play();
                PlayPauseButton.Content = "▶";
            }

        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Next();
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Previous();
        }
    }
}
