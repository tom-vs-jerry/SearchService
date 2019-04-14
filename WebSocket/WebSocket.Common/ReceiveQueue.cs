using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace WebSocket.Common
{   
        /// <summary>
        /// Udp广播数据接收缓冲
        /// </summary>
        public class ReceiveQueue
        {

            /// <summary>
            /// 
            /// </summary>
            public event EventHandler EnQueueEvent = null;

            #region == 字段/属性 ==
            /// <summary>
            /// 磁盘数据库文件名队列
            /// </summary>
            //private Queue<byte[]> dataQueue = null;

            //private Queue<byte[]> DataQueue
            //{
            //    get { return dataQueue; }
            //    set { dataQueue = value; }
            //}

            /// <summary>
            /// socket 异步操作队列
            /// </summary>
            private Queue<SocketAsyncEventArgs> eventQueue = null;

            public Queue<SocketAsyncEventArgs> EventQueue
            {
                get { return eventQueue; }
                set { eventQueue = value; }
            }
            /// <summary>
            /// 队排中元素个数
            /// </summary>
            public int QueueCount
            {
                get
                {
                    return this.EventQueue.Count;
                }
            }

            #endregion

            #region == 构造器 ==

            /// <summary>
            /// 构造器
            /// </summary>
            public ReceiveQueue()
            {
                this.EventQueue = new Queue<SocketAsyncEventArgs>();

            }

            #endregion

            #region == 方法 ==

            /// <summary>
            /// 添加元素到相应队列中
            /// </summary>
            /// <param name="item">udp数据包</param>
            public void EnQueueItem(SocketAsyncEventArgs item)
            {
                lock (((ICollection)this.EventQueue).SyncRoot)
                {
                    this.EventQueue.Enqueue(item);
                    if (this.EnQueueEvent != null)
                        this.EnQueueEvent(this, new EventArgs());
                }
            }

            /// <summary>
            /// 从队列中取出开始元素并删除
            /// </summary>
            /// <returns></returns>
            public SocketAsyncEventArgs DeQueueItem()
            {
                SocketAsyncEventArgs result = null;
                lock (((ICollection)this.EventQueue).SyncRoot)
                {
                    result = this.EventQueue.Dequeue();
                }
                return result;
            }

            /// <summary>
            /// 取队列中开始处第一个元素但不删除
            /// </summary>
            /// <returns></returns>
            public SocketAsyncEventArgs PeekQueueItem()
            {
                SocketAsyncEventArgs result = null;
                lock (((ICollection)this.EventQueue).SyncRoot)
                {
                    result = this.EventQueue.Peek();
                }
                return result;
            }

            /// <summary>
            /// 清除队列中元素
            /// </summary>
            public void ClearQueue()
            {
                lock (((ICollection)this.EventQueue).SyncRoot)
                {
                    this.EventQueue.Clear();
                }
            }

            #endregion

       
    }
}
