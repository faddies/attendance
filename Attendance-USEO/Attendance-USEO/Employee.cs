using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace Attendance_USEO
{
    class Employee
    {
        
        public string MachineName { get; set; }
        public string UserName { get; set; }
        public string Ipv6Id { get; set; }
        public string LocalIpAddress { get; set; }
        public string MacAddress { get; set; }
        public string ConnectedWifi { get; set; }

        public Employee()
        { 
        }

        public Employee(string machineName, string userName, string ipv6Id, string localIpAddress, string macAddress, string connectedWifi)
        {
            MachineName = machineName;
            UserName = userName;
            Ipv6Id = ipv6Id;
            LocalIpAddress = localIpAddress;
            MacAddress = macAddress;
            ConnectedWifi = connectedWifi;
        }

        public void GenerateData() {
            
            
            //--Mac Address--//
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    MacAddress += nic.GetPhysicalAddress().ToString();
                    break;
                }
            }

            //--Machine Name--//
            MachineName = Environment.MachineName;
            UserName = Environment.UserName;
            Ipv6Id = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();
            LocalIpAddress = Dns.GetHostByName(Dns.GetHostName()).AddressList[1].ToString();



            //--SSID--//
            Process processstart = new Process();
            processstart.StartInfo.FileName = "netsh.exe";
            processstart.StartInfo.Arguments = "wlan show interfaces";
            processstart.StartInfo.UseShellExecute = false;
            processstart.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
            processstart.StartInfo.RedirectStandardOutput = true;
            processstart.Start();

            string GetSSIDInfo = processstart.StandardOutput.ReadToEnd();
            string getssidName = GetSSIDInfo.Substring(GetSSIDInfo.IndexOf("SSID"));
            getssidName = getssidName.Substring(getssidName.IndexOf(":"));
            ConnectedWifi = getssidName.Substring(2, getssidName.IndexOf("\n")).Trim();




        }

    }
}
