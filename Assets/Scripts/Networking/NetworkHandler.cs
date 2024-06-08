using System.Net.Sockets;
using System.Net;
using System;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

namespace Unrez
{
    [RequireComponent(typeof(NetworkManager))]
    public class NetworkHandler : MonoBehaviour
    {
        [SerializeField]
        private bool _lan;
        [SerializeField]
        private string _ip;
        [SerializeField]
        private ushort _port = 7777;

        private void Start()
        {
            if (_lan)
            {
                _ip = GetLocalIPAddress();
                InitialiseTransport(_ip, _port);
            }
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        private void InitialiseTransport(string ip, ushort port)
        {
            UnityTransport unityTransport = NetworkManager.Singleton.GetComponent<UnityTransport>();

            unityTransport.SetConnectionData(ip, port);
        }
    }
}
