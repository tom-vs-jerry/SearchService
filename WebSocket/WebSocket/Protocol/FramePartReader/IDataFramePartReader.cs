﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebSocket.Protocol.FramePartReader
{
    interface IDataFramePartReader
    {
        int Process(int lastLength, WebSocketDataFrame frame, out IDataFramePartReader nextPartReader);
    }
}
