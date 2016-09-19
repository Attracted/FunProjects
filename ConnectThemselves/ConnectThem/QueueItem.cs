using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConnectThemselves
{
   public class QueueItem
   {
      public QueueItemType QueueItemType { get; set; }
      public Cell Cell { get; set; }

      public QueueItem()
      {
      }
   }
}
