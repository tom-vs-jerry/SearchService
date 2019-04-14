using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchService.Common
{
    /// <summary>
    /// 描述：缓存区管理
    /// 作者：Sofia
    /// 日期：2016-09-09
    /// </summary>
    public class XQueueManager<T>
    {

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler EnQueueEvent = null;

        #region == 字段/属性 ==
        /// <summary>
        /// 磁盘数据库文件名队列
        /// </summary>
        private ConcurrentQueue<T> _DataQueue = new ConcurrentQueue<T>();



        /// <summary>
        /// 磁盘数据库文件名队列
        /// </summary>
        protected ConcurrentQueue<T> DataQueue
        {
            get { return _DataQueue; }
        }

        /// <summary>
        /// 队排中元素个数
        /// </summary>
        public int QueueCount
        {
            get
            {
                return this._DataQueue.Count;
            }
        }

        #endregion

        #region == 构造器 ==

        /// <summary>
        /// 构造器
        /// </summary>
        public XQueueManager()
        {
            this._DataQueue = new ConcurrentQueue<T>();

        }

        #endregion

        #region == 方法 ==

        /// <summary>
        /// 添加元素到相应队列中
        /// </summary>
        /// <param name="item">udp数据包</param>
        public void EnQueueItem(T item)
        {
            lock (((ICollection)this._DataQueue).SyncRoot)
            {
                this._DataQueue.Enqueue(item);
                if (this.EnQueueEvent != null)
                {
                    this.EnQueueEvent(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// 从队列中取出开始元素并删除
        /// </summary>
        /// <returns></returns>
        public T DeQueueItem()
        {
            T result = default(T);
            lock (((ICollection)this._DataQueue).SyncRoot)
            {
                bool flag = this._DataQueue.TryDequeue(out result);

            }
            return result;
        }

        /// <summary>
        /// 取队列中开始处第一个元素但不删除
        /// </summary>
        /// <returns></returns>
        public T PeekQueueItem()
        {
            T result = default(T);
            lock (((ICollection)this._DataQueue).SyncRoot)
            {
                bool flag = this._DataQueue.TryPeek(out result);
            }
            return result;
        }

        /// <summary>
        /// 清除队列中元素
        /// </summary>
        public void ClearQueue()
        {
            lock (((ICollection)this._DataQueue).SyncRoot)
            {
                //this._DataQueue.Clear();
            }
        }

        #endregion


    }
}
