/*  
Copyright (c) Microsoft.  All rights reserved.  Licensed under the MIT License.  See LICENSE in the root of the repository for license information 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EventHub.OpenTsdbTelnetWriter
{
    public class TsdbTelnetClient:ITsdbWriter, IDisposable
    {
        private Socket _socket;
        private bool _disposed;
        private string _hostName;
        private int _port;
        private bool _reuseConnection;
        public TsdbTelnetClient(string hostName, int port)
        {
            this._hostName = hostName;
            this._port = port;
            this._reuseConnection = true;
        }  
        public TsdbTelnetClient(string hostName, int port, bool reuseConnection)
        {
            this._hostName = hostName;
            this._port = port;
            this._reuseConnection = reuseConnection; 
        }
        public void Write(DataPoint dataPoint)
        {
            if (_socket == null)
            {
                _socket = GetConnectedSocket(_hostName, _port);
            }
            string msg = "put " + dataPoint.ToString();
            int bytecount = _socket.Send(new ASCIIEncoding().GetBytes(msg + Environment.NewLine));
            if (!_reuseConnection)
            {
                _socket.Close();
                _socket.Dispose();
            }
        }
        public void WriteList(List<DataPoint> dataPoints)
        {
            if (_socket == null)
            {
                _socket = GetConnectedSocket(_hostName, _port);
            }
            foreach (DataPoint dataPoint in dataPoints)
            {
                string msg = "put " + dataPoint.ToString();
                int bytecount = _socket.Send(new ASCIIEncoding().GetBytes(msg + Environment.NewLine));
                if (!_reuseConnection)
                {
                    _socket.Close();
                    _socket.Dispose();
                    _socket = null; 
                }
            }
        }

        private static Socket GetConnectedSocket(string hostName, int port)
        {
            var hostEntry = Dns.GetHostAddresses(hostName);
            IPEndPoint endPoint = new IPEndPoint(hostEntry.First(), port);

            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(endPoint);
            return socket;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                if (_socket != null)
                {
                    _socket.Close();
                    _socket.Dispose();
                }
                this._disposed = true;
            }
        }
    }
}