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

namespace Attendance_USEO
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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
