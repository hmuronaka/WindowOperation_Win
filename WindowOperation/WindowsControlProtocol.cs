using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowOperation
{
    public class WindowsControlProtocol
    {
        static private h.mu.Logger.LogFileWriter LOGGER = new h.mu.Logger.LogFileWriter(Properties.Settings.Default, "log_");

        // 受信した際に通知するデリゲート.
        public Action<String, String> receivedDelegate
        {
            set;
            get;
        }
        public void fireReceivedDelegate(String ipAddress, String requestData)
        {
            // @normal
            if (this.receivedDelegate != null)
            {
                this.receivedDelegate(ipAddress, requestData);
            }
        }

        /// <summary>
        /// 一番最後に受信したUDPデータグラムの送信元IPアドレス.
        /// </summary>
        public System.Net.IPEndPoint lastEndPoint
        {
            private set;
            get;
        }

        /// <summary>
        /// UDP通信.
        /// </summary>
        private UdpComponent udpComponent;

        /// <summary>
        /// コマンド一覧.
        /// </summary>
        private Dictionary<String, WindowsControlCommand> commandMap;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public WindowsControlProtocol(int udpPort)
        {
            initUdpComponent(udpPort);

            initCommandMap();
        }

        private void initUdpComponent(int udpPort)
        {
            this.udpComponent = new UdpComponent(udpPort);
            this.udpComponent.receiveDelegate = this.analyzePacket;
        }

        private void initCommandMap()
        {
            commandMap = new Dictionary<string, WindowsControlCommand>();
        }

        public void addCommand(String name, WindowsControlCommand command)
        {
            commandMap[name] = command;
        }

        public void removeCommand(String name)
        {
            commandMap.Remove(name);
        }

        public void setCommandMap(Dictionary<String, WindowsControlCommand> commandMap)
        {
            this.commandMap = commandMap;
        }

        public bool start()
        {
            bool result = false;
            // @normal.
            if (!this.udpComponent.isRunning)
            {
                this.udpComponent.start();
                result = true;

                LOGGER.info("started windows control protocol");
            }
            else
            {
                LOGGER.warn("thisis already started!!");
            }
            return result;
        }

        public bool stop()
        {
            bool result = false;
            // @normal.
            if (this.udpComponent.isRunning)
            {
                this.udpComponent.stop();
                result = true;

                LOGGER.info("stopped windows control protocol");

            }
            else
            {
                LOGGER.warn("this is not running!!");
            }
            return result;
        }

        /// <summary>
        /// 受信データを解析し、それに対するコマンド(処理)を実行する.
        /// 現状受信スレッドの中で直接解析し、コマンドを実行している。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="buffer"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public bool analyzePacket(UdpComponent source, System.Net.IPEndPoint fromEndPoint, byte[] buffer)
        {
            try
            {
                this.lastEndPoint = fromEndPoint;

                String request = Encoding.UTF8.GetString(buffer);

                fireReceivedDelegate(this.lastEndPoint.ToString(), request);

                RequestTokenizer tokenizer = new RequestTokenizer(request);

                String command = null;
                String data = null;
                int code = -1;

                // 順序規定なしのデータ取得.
                while (tokenizer.nextToken())
                {
                    switch (tokenizer.tokenType)
                    {
                        case RequestTokenizer.TokenType.COMMAND:
                            command = tokenizer.token;
                            break;
                        case RequestTokenizer.TokenType.DATA:
                            data = tokenizer.token;
                            break;
                        case RequestTokenizer.TokenType.NUMBER:
                            code = Int32.Parse(tokenizer.token);
                            break;
                    }

                    if (command != null && data != null && code == 0)
                    {
                        execute(fromEndPoint, command, data);
                        command = null;
                        data = null;
                    }
                }
            }
            catch(Exception e)
            {
                LOGGER.error("failed to analyze received data", e);
                return false;
            }
            return true;
        }

        private void execute(System.Net.IPEndPoint endPoint, String commandName, String dataStr)
        {
            WindowsControlCommand command = null;
            if (this.commandMap.TryGetValue(commandName, out command))
            {
                StringBuilder responseString = new StringBuilder();
                int resultCode = 0;
                command.execute(dataStr, responseString, ref resultCode);

                makeResponseDataAndSend(endPoint, commandName, responseString.ToString(), resultCode);
            }
        }

        private void makeResponseDataAndSend(System.Net.IPEndPoint endPoint, String commandName, String response, int resultCode)
        {
            ResponseMaker responseMaker = new ResponseMaker(commandName, response, resultCode);

            String editedReponse = responseMaker.make();

            byte[] buffer = Encoding.UTF8.GetBytes(editedReponse);
            if (endPoint != null)
            {
                this.udpComponent.send(buffer, endPoint);
            }
        }
    }
}
