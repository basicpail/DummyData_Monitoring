using Bogus;
using BogusMqttPublishApp;
using MetroFramework.Forms;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Text;
using System.Threading;
using uPLibrary.Networking.M2Mqtt;

namespace BogusMqttWinPublishApp
{
    public partial class MainForm : MetroForm
    {
        public string MqttBrokerUrl { get; set; }

        public  MqttClient BrokerClient { get; set; }

        private  Thread MqttThread { get; set; }

        private  Faker<SensorInfo> SensorFaker { get; set; }

        private string CurrValue { get; set; }

        public BackgroundWorker MqttWorker { get; set; }

        public MainForm()
        {
            InitializeComponent();
            
            InitializeAll(); //전체 초기화
            ConnectMqttBroker(); //MQTT 브로커 접속
            StartPublish(); //fake 센싱 메세지 전송
        }

        private void StartPublish()
        {
            MqttThread = new Thread(new ThreadStart(LoopPublish));
            //MqttThread = new Thread(()=>LoopPublish);
            MqttThread.Start();
        }

        private void LoopPublish()
        {
            while (true)
            {
                SensorInfo value = SensorFaker.Generate();
                CurrValue = JsonConvert.SerializeObject(value, Formatting.Indented);
                BrokerClient.Publish("home/device/data/", Encoding.Default.GetBytes(CurrValue)); //MQTT로 보내기
                //Console.WriteLine($"Published:{CurrValue}");

                this.Invoke(new Action(()=>
                {
                    RtbLog.AppendText($"Published : {CurrValue}\n\n");
                    RtbLog.ScrollToCaret();
                }));
                
                Thread.Sleep(1000);
            }

        }

        private void ConnectMqttBroker()
        {
            MqttBrokerUrl = TxtBrokerIp.Text;
            BrokerClient = new MqttClient(MqttBrokerUrl);
            BrokerClient.Connect("FakerDaemon");
        }

        private void InitializeAll()
        {
            MqttWorker = new BackgroundWorker();
            MqttWorker.DoWork += MqttWorker_DoWork;
            MqttWorker.WorkerReportsProgress = false;
            MqttWorker.WorkerSupportsCancellation = true;

            MqttBrokerUrl = "localhost"; //또는 127.0.0.1 210.119.12.77

            string[] Rooms = new[] { "DiningRoom", "LivingRoom", "BathRoom", "BedRoom", "Guest Room" };

            SensorFaker = new Faker<SensorInfo>()
                .RuleFor(s => s.Dev_Id, f => f.PickRandom(Rooms))
                .RuleFor(s => s.Curr_Time, f => f.Date.Past(0).ToString("yyyy-MM-dd HH:mm:ss.ff"))
                .RuleFor(s => s.Temp, f => float.Parse(f.Random.Float(15.0f, 34.9f).ToString("0.00")))
                .RuleFor(s => s.Humid, f => float.Parse(f.Random.Float(40.0f, 63.9f).ToString("0.00")))
                .RuleFor(s => s.Press, f => float.Parse(f.Random.Float(800.0f, 999.9f).ToString("0.00")));
        }

        private void MqttWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            LoopPublish();
        }

        private void BtnConnect_Click(object sender, System.EventArgs e)
        {
            ConnectMqttBroker();
            MqttWorker.RunWorkerAsync();
        }

        private void MainForm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if (MqttWorker.IsBusy)
            {
                MqttWorker.CancelAsync();
                MqttWorker.Dispose();
            }
            Environment.Exit(0);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
