using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCP服务器端
{
    class Message
    {
        private byte[] data=new byte[1024];
        private int startIndex = 0;   //我们在数组里面存取的数据的字节数

        public void AddCount(int count)
        {
            startIndex += count;
        }

        public byte[] Data
        {
            get { return data; }
        }

        public int StartIndex
        {
            get { return startIndex; }
        }

        public int RemainSize               //剩余空间
        {
            get { return data.Length - startIndex; }
        }

        /// <summary>
        /// 解析数据或者叫做读取数据
        /// </summary>
        public void ReadMessage()
        {
            while (true)
            {
                if (startIndex <= 4) return;
                int count = BitConverter.ToInt32(data, 0);
                if ((startIndex - 4) >= count)
                {
                    Console.WriteLine(startIndex);
                    Console.WriteLine(count);
                    string s = Encoding.UTF8.GetString(data, 4, count);
                    Console.WriteLine("解析出一条数据：" + s);
                    Array.Copy(data, count + 4, data, 0, startIndex - 4 - count);
                    startIndex -= (count + 4);
                }
                else
                {
                    break;
                }
            }

            

        }
    }
}
