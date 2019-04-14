using System;
using System.Collections.Generic;
using System.Text;

namespace WebSocket.ClientEngine
{
    public class DataEventArgs : EventArgs
    {
        public byte[] Data { get; set; }

        public int Offset { get; set; }

        public int Length { get; set; }
    }
}
