using Caliburn.Micro;
using MqttMonitoringApp.Helpers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace MqttMonitoringApp.ViewModels
{
    public class RealTimeViewModel : Conductor<object>
    {
        double livingTempValue;
        public double LivingTempValue
        {
            get => livingTempValue;
            set
            {
                livingTempValue = value;
                NotifyOfPropertyChange(() => LivingTempValue);
            }
        }

        double livingHumiValue;
        public double LivingHumiValue
        {
            get => livingHumiValue;
            set
            {
                livingHumiValue = value;
                NotifyOfPropertyChange(() => LivingHumiValue);
            }
        }

        double livingPressValue;
        public double LivingPressValue
        {
            get => livingPressValue;
            set
            {
                livingPressValue = value;
                NotifyOfPropertyChange(() => LivingPressValue);
            }
        }

        double diningTempValue;
        public double DiningTempValue
        {
            get => diningTempValue;
            set
            {
                diningTempValue = value;
                NotifyOfPropertyChange(() => DiningTempValue);
            }
        }

        double diningHumiValue;
        public double DiningHumiValue
        {
            get => diningHumiValue;
            set
            {
                diningHumiValue = value;
                NotifyOfPropertyChange(() => DiningHumiValue);
            }
        }

        double diningPressValue;
        public double DiningPressValue
        {
            get => diningPressValue;
            set
            {
                diningPressValue = value;
                NotifyOfPropertyChange(() => DiningPressValue);
            }
        }

        double bedTempValue;
        public double BedTempValue
        {
            get => bedTempValue;
            set
            {
                bedTempValue = value;
                NotifyOfPropertyChange(() => BedTempValue);
            }
        }

        double bedHumiValue;
        public double BedHumiValue
        {
            get => bedHumiValue;
            set
            {
                bedHumiValue = value;
                NotifyOfPropertyChange(() => BedHumiValue);
            }
        }

        double bedPressValue;
        public double BedPressValue
        {
            get => bedPressValue;
            set
            {
                bedPressValue = value;
                NotifyOfPropertyChange(() => BedPressValue);
            }
        }
        public RealTimeViewModel()
        {
            LivingTempValue = 0f;

            if (Commons.BROKERCLIENT != null && Commons.BROKERCLIENT.IsConnected)
            {
                Commons.BROKERCLIENT.MqttMsgPublishReceived += BROKERCLIENT_MqttMsgPublishReceived;
            }
        }

        private void BROKERCLIENT_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Message);
            var currDatas = JsonConvert.DeserializeObject<Dictionary<string, string>>(message);

            switch (currDatas["Dev_Id"].ToString())
            {
                case "LivingRoom":
                    LivingTempValue = double.Parse(currDatas["Temp"]);
                    LivingHumiValue = double.Parse(currDatas["Humid"]);
                    LivingPressValue = double.Parse(currDatas["Press"]);
                    break;
                case "DiningRoom":
                    DiningTempValue = double.Parse(currDatas["Temp"]);
                    DiningHumiValue = double.Parse(currDatas["Humid"]);
                    DiningPressValue = double.Parse(currDatas["Press"]);
                    break;
                case "BedRoom":
                    BedTempValue = double.Parse(currDatas["Temp"]);
                    BedHumiValue = double.Parse(currDatas["Humid"]);
                    BedPressValue = double.Parse(currDatas["Press"]);
                    break;
                default:
                    break;
            }
        }
    }
}