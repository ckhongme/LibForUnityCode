using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace K.system
{
    public class SystemNetTool
    {
        /// <summary>
        /// 获取本机的 私有IP地址
        /// </summary>
        /// <param name="addressFamily"> IPv4 = InterNetwork,  IPv6 = InterNetworkV6 </param>
        /// <returns></returns>
        public static string GetPrivateIpStr(AddressFamily addressFamily = AddressFamily.InterNetwork)
        {
            var adapter = GetActiveAdapter(addressFamily);
            if (adapter == null) return null;

            string ipStr = "";
            foreach (UnicastIPAddressInformation ip in adapter.GetIPProperties().UnicastAddresses)
            {
                if (ip.Address.AddressFamily == addressFamily)
                {
                    ipStr = ip.Address.ToString();
                    break;
                }
            }
            return ipStr;
        }

        /// <summary>
        /// 获取网络适配器的名称
        /// </summary>
        /// <param name="addressFamily"></param>
        /// <returns></returns>
        public static string GetAdapterName(AddressFamily addressFamily = AddressFamily.InterNetwork)
        {
            var adapter = GetActiveAdapter(addressFamily);
            if (adapter == null) return null;
            return adapter.Name;
        }

        /// <summary>
        /// 获取 MAC 地址
        /// </summary>
        /// <param name="addressFamily"></param>
        /// <returns></returns>
        public static string GetMAC(AddressFamily addressFamily = AddressFamily.InterNetwork)
        {
            var adapter = GetActiveAdapter(addressFamily);
            if (adapter == null) return null;
            return adapter.GetPhysicalAddress().ToString();
        }

        /// <summary>
        /// 获取网络接口的速度
        /// </summary>
        /// <param name="addressFamily"></param>
        /// <returns></returns>
        public static string GetSpeed(AddressFamily addressFamily = AddressFamily.InterNetwork)
        {
            var adapter = GetActiveAdapter(addressFamily);
            if (adapter == null) return null;
            return adapter.Speed.ToString();
        }

        /// <summary>
        /// 获取 网络适配器 （接口类型是 Wifi 或者 以太网）
        /// </summary>
        /// <param name="addressFamily"> IPv4 = InterNetwork,  IPv6 = InterNetworkV6 </param>
        /// <returns></returns>
        public static NetworkInterface GetActiveAdapter(AddressFamily addressFamily)
        {
            foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                //如果接口类型是 Wifi 或者 以太网
                if (adapter.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    if (adapter.OperationalStatus == OperationalStatus.Up)
                        return adapter;
                }
            }
            return null;
        }


        /// <summary>
        /// 获取本机的 公有IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetPublicIpStr()
        {
            return null;
        }
    }
}