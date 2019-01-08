using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCP服务器端
{
    class Program
    {
        static void Main(string[] args)
        {
            StartSeverAsync();
            Console.ReadKey();
        }

        static void StartSeverAsync()              //异步接收信息
        {
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //IPAddress ipAddress=new IPAddress(new byte[]{ 192, 168, 1, 101 });
            IPAddress ipAddress = IPAddress.Parse("192.168.1.101");
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 88);
            serverSocket.Bind(ipEndPoint);       //绑定ip和端口号
            serverSocket.Listen(10);     //开始监听，同时处理的链接个数

            //Socket clientSocket = serverSocket.Accept();       //接收一个客户端链接
            serverSocket.BeginAccept(AcceptCallBack, serverSocket);
            
            ////接收客户端的一条消息
            //byte[] dataBuffer = new byte[1024];
            //int count = clientSocket.Receive(dataBuffer);
            //string msgReceive = System.Text.Encoding.UTF8.GetString(dataBuffer, 0, count);
            //Console.WriteLine(msgReceive);
            //Console.ReadKey();
            //clientSocket.Close();
            //serverSocket.Close();            
        }
        static Message msg=new Message();

        static void AcceptCallBack(IAsyncResult ar)
        {
            Socket serverSocket = ar.AsyncState as Socket;
            Socket clientSocket = serverSocket.EndAccept(ar);

            //向客户端发送消息
            string msgStr = "hello,你好。";
            byte[] data = System.Text.Encoding.UTF8.GetBytes(msgStr);
            clientSocket.Send(data);
            clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallback, clientSocket);

            serverSocket.BeginAccept(AcceptCallBack, serverSocket);
        }

        static byte[] dataBuffer = new byte[1024];
        static void ReceiveCallback(IAsyncResult ar)
        {
            Socket clientSocket = null;
            try
            {
                clientSocket = ar.AsyncState as Socket;
                int count = clientSocket.EndReceive(ar);       //结束挂起的异步读取
                if (count == 0)          //处理客户端正常关闭的情况
                {
                    clientSocket.Close();
                    return;
                }
                msg.AddCount(count);
                //string msgStr = Encoding.UTF8.GetString(dataBuffer, 0, count);
                //Console.WriteLine("从客户端接收的数据：" + count +"   "+ msgStr);
                msg.ReadMessage();
                //clientSocket.BeginReceive(dataBuffer, 0, 1024, SocketFlags.None, ReceiveCallback, clientSocket);      //一个回调，循环接收信息
                clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallback, clientSocket);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                if (clientSocket != null)         //处理客户端非正常关闭的情况
                {
                    clientSocket.Close();
                }
            }
        }

        void StartServerSnc()        //同步接收信息
        {
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //IPAddress ipAddress=new IPAddress(new byte[]{ 192, 168, 1, 101 });
            IPAddress ipAddress = IPAddress.Parse("192.168.1.101");
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 88);
            serverSocket.Bind(ipEndPoint);       //绑定ip和端口号
            serverSocket.Listen(10);     //开始监听，同时处理的链接个数
            Socket clientSocket = serverSocket.Accept();       //接收一个客户端链接

            //向客户端发送消息
            string msg = "hello,你好。";
            byte[] data = System.Text.Encoding.UTF8.GetBytes(msg);
            clientSocket.Send(data);

            //接收客户端的一条消息
            byte[] dataBuffer = new byte[1024];
            int count = clientSocket.Receive(dataBuffer);
            string msgReceive = System.Text.Encoding.UTF8.GetString(dataBuffer, 0, count);
            Console.WriteLine(msgReceive);
            Console.ReadKey();

            clientSocket.Close();
            serverSocket.Close();
        }
    }
}
