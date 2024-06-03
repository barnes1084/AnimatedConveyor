using CommonTools;
using Logix;
using System;
using System.Net;
using System.Threading;
using System.Windows;

namespace AnimatedConveyor
{
    public partial class MainWindow : Window
    {
        Timer timer;
        private Animation conveyorAnimation;

        // PLC Tags
        LogixTcpClient plc;
        bool p1_photoeye = false;
        bool p1_motorOn = false;


        public MainWindow()
        {
            InitializeComponent();

            ConnectToPLC();
            conveyorAnimation = new Animation(80, 95, 300, 95, RootCanvas);
            timer = new Timer(state => StartLiveFeed(), null, 0, 1000);
            AppDomain.CurrentDomain.ProcessExit += (s, e) => { timer.Dispose(); plc.Close(); };
        }


        // State machine
        private void StartLiveFeed()
        {
            GetPlcTagValues();
            Dispatcher.Invoke(() =>
            {
                if (p1_photoeye)
                {
                    conveyorAnimation.StartConveyorAnimation(RootCanvas, 80, 300, 95, 95);
                }

                if (!p1_motorOn && !conveyorAnimation.IsAnimationPaused)
                {
                    conveyorAnimation.PauseConveyorAnimation();
                }

                if (conveyorAnimation.IsAnimationPaused && p1_motorOn)
                {
                    conveyorAnimation.ResumeConveyorAnimation();
                }
            });
        }


        // PLC Connections
        private void ConnectToPLC()
        {
            string ipAddress = "172.18.173.52";
            try
            {
                IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(ipAddress), 44818);
                plc = new LogixTcpClient(endpoint, 1, 0);
            }
            catch (Exception e)
            {
                Log.ToFile($"{e.TargetSite}: {e.Message}");
                timer?.Dispose();
            }
        }

        private void GetPlcTagValues()
        {
            try
            {
                if (plc == null || plc.IsConnecting)
                {
                    Log.ToFile($"PLC is connecting...");
                    return; // Exit if PLC is still connecting
                }
                else if (!plc.IsConnected)
                {
                    Log.ToFile($"PLC is not connected.");
                    ReconnectToPLC();
                    return;
                }
                else if (plc.IsConnected)
                {
                    // Read tags only if PLC is connected
                    p1_photoeye = plc.ReadBool("P1_PRESENCE_INPUT");
                    p1_motorOn = plc.ReadBool("P1_Start.EN");
                }
            }
            catch (Exception e)
            {
                Log.ToFile($"{e.TargetSite}: {e.Message}");
                ReconnectToPLC();
            }
        }

        private void ReconnectToPLC()
        {
            try
            {
                if (plc != null)
                {
                    plc.Close(); // Close existing connection
                    plc = null; // Set to null to avoid accessing disposed object
                }

                Thread.Sleep(2000); // Wait before reconnecting
                ConnectToPLC(); // Re-establish connection
            }
            catch (Exception e)
            {
                Log.ToFile($"{e.TargetSite}: {e.Message}");
                timer?.Dispose();
            }
        }


        // Button calls
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                conveyorAnimation.StartConveyorAnimation(RootCanvas, 80, 300, 95, 95);
            });
        }
        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                conveyorAnimation.PauseConveyorAnimation();
            });
        }
        private void ResumeButton_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                conveyorAnimation.ResumeConveyorAnimation();
            });
        }
    }
}
