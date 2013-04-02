using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WindowOperation
{
    public partial class Form1 : Form
    {
        private WindowsControlProtocol protocol = null;
        private List<String> windowNameList;

        public Form1()
        {
            InitializeComponent();
            this.windowNameList = new List<String>();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBoxPort.Text = "50001";
            connectButton.PerformClick();
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.protocol == null)
                {
                    int port = Int32.Parse(textBoxPort.Text);
                    this.protocol = new WindowsControlProtocol(port);
                    this.protocol.receivedDelegate = onReceivedDelegate;

                    Dictionary<String, WindowsControlCommand> commandMap = new Dictionary<string, WindowsControlCommand>();
                    commandMap["@GetWindowList"] = new GetWindowListCommand();
                    commandMap["@TopWindow"] = new TopWindowCommand();
                    commandMap["@GetMostRecentlyFiles"] = new GetMostRecentlyUsedList();
                    commandMap["@OpenFile"] = new OpenFileCommand();
                    this.protocol.setCommandMap(commandMap);
                    this.protocol.start();
                    this.connectButton.Text = "切断";
                }
                else
                {
                    this.protocol.stop();
                    this.protocol.receivedDelegate = null;
                    this.protocol = null;
                    this.connectButton.Text = "接続";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("入力に誤りがあります。" + ex.Message);
            }
        }

    
        private void updateListViewWindow(String ipAddress, String request)
        {
            if (this.listViewWindow.InvokeRequired)
            {
                this.listViewWindow.Invoke((Action<String, String>)updateListViewWindow, ipAddress, request);
            }
            else
            {
                this.listViewWindow.BeginUpdate();
                if (this.listViewWindow.Items.Count > 10)
                {
                    this.listViewWindow.Items.RemoveAt(0);
                }
                StringBuilder builder = new StringBuilder();
                builder.Append(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                builder.Append("  ");
                builder.Append(ipAddress);
                builder.Append("  ");
                builder.Append(request);
                this.listViewWindow.Items.Add(builder.ToString());
                this.listViewWindow.EndUpdate();
            }
        }

        public void onReceivedDelegate(String ipAddress, String request)
        {
            updateListViewWindow(ipAddress, request);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.protocol.stop();
        }



    }
}
