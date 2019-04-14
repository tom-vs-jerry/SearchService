﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WebSocket.ProtoBase
{
    /// <summary>
    /// The interface for protocol encoder of text messages
    /// </summary>
    public interface IProtoTextEncoder
    {
        /// <summary>
        /// Encode text message
        /// </summary>
        /// <param name="message">the message to be encoded</param>
        /// <returns>the output binary data</returns>
        IList<ArraySegment<byte>> EncodeText(string message);
    }
}
