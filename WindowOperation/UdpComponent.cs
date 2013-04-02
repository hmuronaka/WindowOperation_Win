using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowOperation
{
    public class UdpComponent
    {
        /// <summary>
        /// UdpComponentの受信待機処理を開始、終了、エラーのときに
        /// デリゲートへ通知する際のイベント種別.
        /// </summary>
        public enum EventType
        {
            STARTED,
            ENDED,
            ERROR
        };


        /// <summary>
        /// UDPサーバ
        /// </summary>
        private System.Net.Sockets.UdpClient server = null;

        /// <summary>
        /// 受信スレッド
        /// </summary>
        private System.Threading.Thread receiveThread;

        /// <summary>
        /// 受信用デリゲート
        /// </summary>
        public Func<UdpComponent, System.Net.IPEndPoint, byte[], bool> receiveDelegate
        {
            set;
            get;
        }

        /// <summary>
        /// 状態変化用デリゲート
        /// </summary>
        private Action<UdpComponent, EventType> changeStateDelegate
        {
            set;
            get;
        }

        /// <summary>
        /// サーバのリッスンポート.
        /// </summary>
        public int port
        {
            private set;
            get;
        }

        /// <summary>
        /// 終了要求.
        /// </summary>
        public bool isRequestExit
        {
            private set;
            get;
        }

        /// <summary>
        /// startでtrue, stopでfalse
        /// </summary>
        public bool isRunning
        {
            private set;
            get;
        }

        /// <summary>
        /// 排他用
        /// </summary>
        private Object locker;


        /// <summary>
        /// UDPのリッスンポートを指定する.
        /// </summary>
        /// <param name="port"></param>
        public UdpComponent(int port)
        {
            this.port = port;
            this.locker = new Object();
            this.isRunning = false;
        }

        /// <summary>
        /// 受信スレッドを開始して、通信可能にする.
        /// </summary>
        /// <returns></returns>
        public void start()
        {
            lock (locker)
            {
                if (this.receiveThread != null)
                {
                    throw new InvalidOperationException("既に受信スレッド実行中!![port:" + this.port + "]");
                }
                if (this.server != null)
                {
                    throw new InvalidOperationException("既に受信サーバ実行中!![port:" + this.port + "]");
                }
                this.isRequestExit = false;
                this.server = new System.Net.Sockets.UdpClient(this.port);
                this.receiveThread = new System.Threading.Thread(threadMain);
                this.isRunning = true;
                this.receiveThread.Start();
            }
        }

        /// <summary>
        /// スレッドを終了し、終了するまで待機する.
        /// </summary>
        public void stop()
        {
            lock (locker)
            {
                if (this.receiveThread == null && this.server == null)
                {
                    return;
                }
                else if (this.receiveThread == null || this.server == null)
                {
                    throw new InvalidOperationException("不正な状態で終了!![port:" + this.port + "][recvThread:" + this.receiveThread + "][server:" + this.server + "]");
                }
                requestStop();
                this.receiveThread.Join();
                this.server = null;
                this.receiveThread = null;
                this.isRunning = false;
            }
        }

        /// <summary>
        /// 終了要求する.
        /// </summary>
        public void requestStop()
        {
            this.isRequestExit = true;
            lock (locker)
            {
                if (this.server != null)
                {
                    this.server.Close();
                }
            }
        }

        private void threadMain()
        {
            if (this.changeStateDelegate != null)
            {
                this.changeStateDelegate(this, EventType.STARTED);
            }
            while (!this.isRequestExit)
            {
                try
                {
                    System.Net.IPEndPoint endpoint = null;
                    byte[] buffer = this.server.Receive(ref endpoint);
                    if (this.receiveDelegate != null)
                    {
                        if (this.receiveDelegate != null)
                        {
                            this.receiveDelegate(this, endpoint, buffer);
                        }
                    }
                }
                catch (Exception e)
                {
                    ;
                }
            }
        }

        /// <summary>
        /// 送信する.
        /// 
        /// stop()のタイミングによっては例外が飛ぶ.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="outEndPoint"></param>
        public void send(byte[] buffer, System.Net.IPEndPoint outEndPoint)
        {
            lock (locker)
            {
                if (!this.isRequestExit)
                {
                    this.server.Send(buffer, buffer.Length, outEndPoint);
                }
            }
        }
    }
}
