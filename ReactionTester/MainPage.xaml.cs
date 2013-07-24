using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;
using Windows.UI;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace ReactionTester
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : ReactionTester.Common.LayoutAwarePage
    {
        /// <summary>
        /// Timer, that is triggered once Start-button is pressed
        /// </summary>
        private DispatcherTimer timerStart;

        /// <summary>
        /// Stopwatch, that takes time in milliseconds
        /// </summary>
        private Stopwatch stopWatch;


        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }



        /// <summary>
        /// Sees first if the stopwatch is running, and if it is, does nothing
        /// Otherwise tries to parse Textboxes to numbers, if it's unable to do so, sets default values(5 and 10)
        /// After that creates double between those values, and sets the timer to tick after that time has passed
        /// </summary>
        /// <param name="sender">Sender for the event</param>
        /// <param name="e">Arguments for the event, not needed</param>
        private void ButtonStart_Click_1(object sender, RoutedEventArgs e)
        {
            if (stopWatch != null) if (stopWatch.IsRunning == true) return;

            TextBlockTime.Text = "00:00:00";
            GridColor.Background = new SolidColorBrush(Colors.Red);
            TextBlockButtonColor.Text = "Press here when the light turns green";
            double time1, time2;
            try
            {
                time1 = Convert.ToDouble(TextBox1Seconds.Text);
                time2 = Convert.ToDouble(TextBox2Seconds.Text);

                if (time1 > time2)
                {
                    double temp = time1;
                    time1 = time2;
                    time2 = temp;
                }
            }
            catch (Exception ex)
            {
                if (ex is FormatException || ex is OverflowException)
                {
                    time1 = 5;
                    time2 = 10;
                }
                else
                {
                    throw;
                }
            }
            Random randomizer = new Random();
            double randomNumber = time1 + (randomizer.NextDouble() * (time2 - time1));
            String randomNumberAsString = "" + randomNumber;

            char[] separator = new char[] { '.' };
            string[] fields = randomNumberAsString.Split(separator, StringSplitOptions.None);
            int seconds, milliseconds;
            try
            {
                seconds = Convert.ToInt16(fields[0]);
                milliseconds = Convert.ToInt16(fields[1].Substring(0, 2));
            }
            catch (Exception ex)
            {
                if (ex is ArgumentOutOfRangeException || ex is FormatException || ex is OverflowException)
                {
                    seconds = 7;
                    milliseconds = 70;
                }
                else
                {
                    throw;
                }
            }


            timerStart = new DispatcherTimer();
            timerStart.Interval = new TimeSpan(0, 0, 0, seconds, milliseconds);
            timerStart.Tick += TriggerColor;
            timerStart.Start();
        }


        /// <summary>
        /// User is free to click the button, so sets it green and sets the text on button to Press here
        /// Also starts the stopwatch that takes time in milliseconds how long does it take for the user to press the button
        /// </summary>
        /// <param name="sender">Sender for the event</param>
        /// <param name="e">Arguments for the event, not needed</param>
        private void TriggerColor(object sender, object e)
        {
            timerStart.Stop();
            GridColor.Background = new SolidColorBrush(Colors.Green);
            TextBlockButtonColor.Text = "Press here!";
            stopWatch = new Stopwatch();
            stopWatch.Start();
        }



        /// <summary>
        /// Sees if the stopwatch isn't running, in which case just returns.
        /// Otherwise stops the stopwatch and sets in minutes:seconds:milliseconds how long it took for the user to press the button        
        /// </summary>
        /// <param name="sender">Sender for the event</param>
        /// <param name="e">Arguments for the event, not needed</param>
        private void ButtonColor_Click_1(object sender, RoutedEventArgs e)
        {
            if (stopWatch == null || stopWatch.IsRunning == false) return;

            long reactionMilliseconds = stopWatch.ElapsedMilliseconds;
            stopWatch.Stop();
            GridColor.Background = new SolidColorBrush(Colors.Red);

            long tempMillisec = reactionMilliseconds;
            long minutes, seconds, milliseconds;
            minutes = tempMillisec / 60000;
            tempMillisec = tempMillisec - (minutes * 60000);
            seconds = tempMillisec / 1000;
            tempMillisec = tempMillisec - (seconds * 1000);
            milliseconds = tempMillisec;

            String newTextblockText = "";
            if (minutes < 10) newTextblockText = "0" + minutes + ":";
            else newTextblockText = minutes + ":";

            if (seconds < 10) newTextblockText = newTextblockText + "0" + seconds + ":";
            else newTextblockText = newTextblockText + seconds + ":";

            if (milliseconds < 10) newTextblockText = newTextblockText + "00" + milliseconds;
            else if (milliseconds < 100) newTextblockText = newTextblockText + "0" + milliseconds;
            else newTextblockText = newTextblockText + milliseconds;

            TextBlockTime.Text = newTextblockText;

            TextBlockButtonColor.Text = "Press the Start-button below to Start";
        }

        private void GridColor_PointerPressed_1(object sender, PointerRoutedEventArgs e)
        {

            if (stopWatch == null || stopWatch.IsRunning == false) return;

            long reactionMilliseconds = stopWatch.ElapsedMilliseconds;
            stopWatch.Stop();
            GridColor.Background = new SolidColorBrush(Colors.Red);

            long tempMillisec = reactionMilliseconds;
            long minutes, seconds, milliseconds;
            minutes = tempMillisec / 60000;
            tempMillisec = tempMillisec - (minutes * 60000);
            seconds = tempMillisec / 1000;
            tempMillisec = tempMillisec - (seconds * 1000);
            milliseconds = tempMillisec;

            String newTextblockText = "";
            if (minutes < 10) newTextblockText = "0" + minutes + ":";
            else newTextblockText = minutes + ":";

            if (seconds < 10) newTextblockText = newTextblockText + "0" + seconds + ":";
            else newTextblockText = newTextblockText + seconds + ":";

            if (milliseconds < 10) newTextblockText = newTextblockText + "00" + milliseconds;
            else if (milliseconds < 100) newTextblockText = newTextblockText + "0" + milliseconds;
            else newTextblockText = newTextblockText + milliseconds;

            TextBlockTime.Text = newTextblockText;

            TextBlockButtonColor.Text = "Press the Start-button below to Start";
        }
    }
}
