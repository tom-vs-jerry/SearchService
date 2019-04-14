using System;
using System.Collections.Generic;
using System.Text;

namespace WebSocket.ClientEngine
{
    public interface IBufferSetter
    {
        void SetBuffer(ArraySegment<byte> bufferSegment);
    }
}
