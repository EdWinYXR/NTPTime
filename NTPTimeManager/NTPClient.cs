using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

/*
    个人微信：a7761075
    联系邮箱：yinxurong@darsh.cn
    创建时间：2022/11/10 11:18:23
    主要用途：
    更改记录：
                  时间：            更改记录：
*/

namespace NTPTimeManager
{
    public class NTPClient
    {
        private IPAddress _server { get; set; }
        public NTPClient(string server)
        {
            if (string.IsNullOrEmpty(server))
            {
                throw new ArgumentException("Must be non-empty", "server");
            }
            IPAddress m_server;
            if(IPAddress.TryParse(server, out m_server))
            {
                _server = m_server;
            }
            else
            {
                throw new ArgumentException("must be valid ip address", "server");
            }
        }
        [DllImport("kernel32.dll")]
        private static extern bool SetLocalTime(ref Systemtime time);
        [StructLayout(LayoutKind.Sequential)]
        private struct Systemtime
        {
            public short year;
            public short month;
            public short dayOfWeek;
            public short day;
            public short hour;
            public short minute;
            public short second;
            public short milliseconds;
        }

        /// <summary>
        /// NTP获得时间
        /// </summary>
        /// <param name="_server">ntp地址</param>
        /// <returns>本初子午线时间</returns>
        public  DateTime GetUtc()
        {
            byte[] array = new byte[48];
            array[0] = 27;
            IPAddress[] addressList = Dns.GetHostEntry(_server).AddressList;
            IPEndPoint remoteEP = new IPEndPoint(addressList[0], 123);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)
            {
                ReceiveTimeout = 3000
            };
            socket.Connect(remoteEP);
            socket.Send(array);
            socket.Receive(array);
            socket.Close();
            ulong num = ((ulong)array[40] << 24) | ((ulong)array[41] << 16) | ((ulong)array[42] << 8) | array[43];
            ulong num2 = ((ulong)array[44] << 24) | ((ulong)array[45] << 16) | ((ulong)array[46] << 8) | array[47];
            ulong num3 = num * 1000 + num2 * 1000 / 4294967296uL;
            return new DateTime(1900, 1, 1).AddMilliseconds((long)num3);
        }
        /// <summary>
        /// 设置系统时间
        /// </summary>
        /// <param name="dt">需要设置的时间</param>
        /// <returns>返回系统时间设置状态，true为成功，false为失败</returns>
        public  bool SetLocalDateTime(DateTime dt)
        {
            Systemtime st;
            st.year = (short)dt.Year;
            st.month = (short)dt.Month;
            st.dayOfWeek = (short)dt.DayOfWeek;
            st.day = (short)dt.Day;
            st.hour = (short)dt.Hour;
            st.minute = (short)dt.Minute;
            st.second = (short)dt.Second;
            st.milliseconds = (short)dt.Millisecond;
            bool rt = SetLocalTime(ref st);
            return rt;
        }
    }
}
