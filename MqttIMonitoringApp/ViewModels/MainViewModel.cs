using Caliburn.Micro;
using MqttMonitoringApp.Helpers;
using MqttMonitoringApp.ViewModels;
using System;
using System.Windows;

namespace MqttMonitoringApp.ViewModels
{
    public class MainViewModel : Conductor<object>
    {
        public MainViewModel()
        {
            DisplayName = "MQTT Monitoring App- v0.9";
        }

        protected override void OnDeactivate(bool close)
        {
            if(Commons.BROKERCLIENT.IsConnected)
            {
                Commons.BROKERCLIENT.Disconnect();
                Commons.BROKERCLIENT = null;
            }

            base.OnDeactivate(close);
        }

        public void ExitProgram()
        {
            Environment.Exit(0);
        }

        public void ExitApp()
        {
            ExitProgram();
        }

        public void LoadDataBaseView()
        {
            if (Commons.BROKERCLIENT != null)
            {
                ActivateItem(new DataBaseViewModel());
            }
            else
            {
                var wManager = new WindowManager();
                wManager.ShowDialog(new ErrorPopupViewModel("Error|MQTT가 실행되지 않았습니다."));
            }
        }

        public void LoadRealTimeView()
        {
            ActivateItem(new RealTimeViewModel());
        }

        public void LoadHistoryView()
        {
            ActivateItem(new HistoryViewModel());
        }

        public void PopInfoDialog()
        {
            TaskStart();
        }

        private void TaskStart()
        {
            var wManager = new WindowManager();
            var result = wManager.ShowDialog(new CustomPopupViewModel("New Broker"));
            
            if(result == true)
            {
                //MessageBox.Show("Start Subscribe!");
                ActivateItem(new DataBaseViewModel());

            }
        }
    }
}
