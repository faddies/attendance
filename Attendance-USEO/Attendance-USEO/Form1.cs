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


            //ReadEntries(textBox1);
                    }

        static void ReadEntries(TextBox text)
        {
            var range = $"{BaseSheetName}!A1:A5";
            var request = service.Spreadsheets.Values.Get(SpreadSheetId, range);
            var response = request.Execute();
            var values = response.Values;
            string data = string.Empty;
            foreach (var row in values) {
                data = data + row[0];
                data = data + Environment.NewLine;
            }
            text.Text = data;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Employee UserCreated = new Employee();
            UserCreated.GenerateData();
            textBox1.Text = UserCreated.MacAddress + Environment.NewLine + UserCreated.ConnectedWifi;
        }
    }
}
