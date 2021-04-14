using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Google.Apis.Sheets.v4;
using Google.Apis.Auth.OAuth2;
using System.IO;
using System.Collections;

namespace Attendance_USEO
{
    public partial class Form1 : Form
    {
        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static readonly string SpreadSheetId = "1wD02lplHt-nuJLLzcCa_PBgEiIx-Lf6dYvwZ6OptjAY";
        static readonly string applicationName = "Attendance";
        static readonly string BaseSheetName = "BaseData";
        static SheetsService service;

        public Form1()
        {
            InitializeComponent();

            GoogleCredential credential;
            using (var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read)) {

                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(Scopes);
            }

            service = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer()
            {

                HttpClientInitializer = credential,
                ApplicationName = applicationName,
            });


            var rowsgot = ReadEntries();
            foreach (var row in rowsgot) {
                textBox1.Text = (string)row[0];
            }
        }

        static List<IList> ReadEntries()
        {
            var range = $"{BaseSheetName}!A1:A5";
            var request = service.Spreadsheets.Values.Get(SpreadSheetId, range);
            var response = request.Execute();
            var values = response.Values;
            return (List<IList>)values;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string MachineNameText = string.Empty;
            MachineNameText = Environment.MachineName;
            MachineNameText = MachineNameText + Environment.NewLine;
            MachineNameText = MachineNameText + Environment.UserName;
            MachineNameText = MachineNameText + Environment.NewLine;
            MachineNameText = MachineNameText + Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();
            MachineNameText = MachineNameText + Environment.NewLine;
            MachineNameText = MachineNameText + Dns.GetHostByName(Dns.GetHostName()).AddressList[1].ToString();

            string macAddresses = "";

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    macAddresses += nic.GetPhysicalAddress().ToString();
                    break;
                }
            }

            Process processstart = new Process();
            processstart.StartInfo.FileName = "netsh.exe";
            processstart.StartInfo.Arguments = "wlan show interfaces";
            processstart.StartInfo.UseShellExecute = false;
            processstart.StartInfo.RedirectStandardOutput = true;
            processstart.Start();

            string GetSSIDInfo = processstart.StandardOutput.ReadToEnd();
            string getssidName = GetSSIDInfo.Substring(GetSSIDInfo.IndexOf("SSID"));
            getssidName = getssidName.Substring(getssidName.IndexOf(":"));
            getssidName = getssidName.Substring(2, getssidName.IndexOf("\n")).Trim();

            MachineNameText = MachineNameText + Environment.NewLine;
            MachineNameText = MachineNameText + getssidName;
            
            
            MachineNameText = MachineNameText + Environment.NewLine;
            MachineNameText = MachineNameText + macAddresses;
            textBox1.Text = MachineNameText;
            
        }
    }
}
