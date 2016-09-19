using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ConnectThemselves
{
   public class QueueEventArgs : EventArgs
   {
      public QueueItem Item { get; set; }
      
      public QueueEventArgs()
         : base()
      {
      }
   }
   
   public class QueueManager
   {
      private Queue<QueueItem> _Queue;
      private Timer _Timer;

      public bool StopAutoMove { get; set; }
      public bool StopAutoReset { get; set; }

      public delegate void QueueTimerElapsedHandler(object sender, QueueEventArgs e);
      public event QueueTimerElapsedHandler OnQueueTimerElapsed;

      public QueueManager()
      {
         _Queue = new Queue<QueueItem>();
         _Timer = new Timer();
         _Timer.Interval = 10;
         _Timer.Enabled = true;
         _Timer.Tick += new EventHandler(TimerElapsed_Event);
      }

      public void AddToQueue(QueueItem item)
      {
         if (item.QueueItemType == QueueItemType.ResetGameAuto && StopAutoReset)
         {
            return;
         }
         if (item.QueueItemType == QueueItemType.NextMoveAuto && StopAutoMove)
         {
            return;
         }

         _Queue.Enqueue(item);
         _Timer.Start();
      }

      public void SetTimerInterval(int interval)
      {
         _Timer.Interval = interval;
      }

      public void TimerElapsed_Event(object sender, EventArgs e)
      {
         if (_Queue.Count == 0 || OnQueueTimerElapsed == null) return;
         _Timer.Stop();

         var item = _Queue.Dequeue();

         if (item.QueueItemType == QueueItemType.ResetGame || item.QueueItemType == QueueItemType.ResetGameAuto)
         {
            _Queue.Clear();
         }

         QueueEventArgs args = new QueueEventArgs() { Item = item };
         OnQueueTimerElapsed(this, args);
      }
   }
}
