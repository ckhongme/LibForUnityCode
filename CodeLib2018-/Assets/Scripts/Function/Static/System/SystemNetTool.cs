using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace K.system
{
    public class SystemNetTool
    {
        /// <summary>
        /// 获取本机的 IP地址
        /// </summary>
        /// <param name="addressFamily"> IPv4 = InterNetwork,  IPv6 = InterNetworkV6 </param>
        /// <returns></returns>
        public static string GetIpStr(AddressFamily addressFamily)
        {
            //Return null if addressFamily is Ipv6 but OS does not support it
            if (addressFamily == AddressFamily.InterNetworkV6 && !Socket.OSSupportsIPv6)
            {
                return null;
            }

            string ipStr = "";
            foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                //如果接口类型是 Wifi 或者 以太网
                if (adapter.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    if (adapter.OperationalStatus == OperationalStatus.Up)
                    {
                        foreach (UnicastIPAddressInformation ip in adapter.GetIPProperties().UnicastAddresses)
                        {
                            if (ip.Address.AddressFamily == addressFamily)
                            {
                                ipStr = ip.Address.ToString();
                                break;
                            }
                        }
                    }
                }
            }
            return ipStr;
        }


        //private NetworkInterface _GetAdapter()
        //{

        //}



    }
}