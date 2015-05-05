using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Nth.Eindhoven.Fontys
{
    /// <summary>
    /// Interaction logic for Navigation.xaml
    /// </summary>
    public partial class Navigation : UserControl
    {
        public Navigation( )
        {
            InitializeComponent( );
        }

        // --------------------------------------------------------------------

        private void BuildClick( object sender, RoutedEventArgs e )
        {
            if ( Build != null )
            {
                Build( sender, e );
            }
        }

        // --------------------------------------------------------------------

        private void RunClick( object sender, RoutedEventArgs e )
        {
            if ( Run != null
                 && Status == StateMode.Stop )
            {
                Status = StateMode.Play;
                Run( sender, e );
                return;
            }

            if ( Pause != null
                 && Status == StateMode.Play )
            {
                Status = StateMode.Pause;
                Pause( sender, e );
                return;
            }

            if ( Resume != null
                && Status == StateMode.Pause )
            {
                Status = StateMode.Play;
                Resume( sender, e );
            }
        }

        // --------------------------------------------------------------------

        private void StopClick( object sender, RoutedEventArgs e )
        {
            if ( Stop != null )
            {
                Status = StateMode.Stop;
                Stop( sender, e );
            }
        }

        // --------------------------------------------------------------------

        public StateMode Status
        {
            get
            {
                return state;
            }
            set
            {
                if ( value == StateMode.Stop
                     && Stop != null )
                {
                    btnPlay.Content = Resources[ "play" ] as Image;
                }

                if ( value == StateMode.Pause
                     && Pause != null )
                {
                    btnPlay.Content = Resources[ "play" ] as Image;
                }

                if ( value == StateMode.Play
                     && Run != null )
                {
                    btnPlay.Content = Resources[ "pause" ] as Image;
                }

                state = value;
            }
        }

        // --------------------------------------------------------------------

        public enum StateMode
        {
            Stop = 0,
            Play,
            Pause
        }

        // --------------------------------------------------------------------

        private void PreviewRunClick( object sender, MouseButtonEventArgs e )
        {
            if ( PreviewRun != null )
            {
                PreviewRun( sender, e );
            }
        }

        // --------------------------------------------------------------------

        public event EventHandler Build;
        public event EventHandler Run;
        public event EventHandler Stop;
        public event EventHandler Resume;
        public event EventHandler Pause;
        public event EventHandler PreviewRun;

        private StateMode state;
    }
}
