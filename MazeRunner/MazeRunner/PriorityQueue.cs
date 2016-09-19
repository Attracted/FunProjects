using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeRunner
{
   // A* needs only a WeightedGraph and a location type L, and does *not*
   // have to be a grid. However, in the example code I am using a grid.
   public interface WeightedGraph<L>
   {
      int Cost(Location a, Location b);
      IEnumerable<Location> Neighbors(Location id);
   }


   public struct Location
   {
      // Implementation notes: I am using the default Equals but it can
      // be slow. You'll probably want to override both Equals and
      // GetHashCode in a real project.

      public readonly int X, Y;
      public Location(int x, int y)
      {
         X = x;
         Y = y;
      }

      public override bool Equals(object obj)
      {
         if (obj == null)
         {
            return false;
         }

         Location a = (Location)obj;

         return (X == a.X) && (Y == a.Y);
      }
      public static int Diff(Location a, Location b)
      {
         return (a.X - b.X) + (a.Y - b.Y);
      }
      public bool Equals(Location a)
      {
         if ((object)a == null)
         {
            return false;
         }

         return (X == a.X) && (Y == a.Y);
      }
      public static bool operator ==(Location a, Location b)
      {
         if (System.Object.ReferenceEquals(a, b))
         {
            return true;
         }

         return a.X == b.X && a.Y == b.Y;
      }

      public static bool operator !=(Location a, Location b)
      {
         return !(a == b);
      }


      public override int GetHashCode()
      {
         return X ^ Y;
      }
   }


   public class SquareGrid : WeightedGraph<Location>
   {
      // Implementation notes: I made the fields public for convenience,
      // but in a real project you'll probably want to follow standard
      // style and make them private.

      public static readonly Location[] DIRS = new[]
        {
            new Location(1, 0),
            new Location(0, -1),
            new Location(-1, 0),
            new Location(0, 1)
        };

      public int Width, Height;
      public HashSet<Location> Walls = new HashSet<Location>();
      public HashSet<Location> Forests = new HashSet<Location>();
      public List<Tuple<char, Location>> Portals = new List<Tuple<char, Location>>();

      public SquareGrid(int width, int height)
      {
         Width = width;
         Height = height;
      }

      public SquareGrid(SquareGrid grid)
      {
         Width = grid.Width;
         Height = grid.Height;
         Walls = new HashSet<Location>(grid.Walls);
         Forests = new HashSet<Location>(grid.Forests);
         Portals = new List<Tuple<char, Location>>(grid.Portals);
      }

      public bool InBounds(Location id)
      {
         return 0 <= id.X && id.X < Width
             && 0 <= id.Y && id.Y < Height;
      }

      public bool Passable(Location id)
      {
         return !Walls.Contains(id);
      }

      public int Cost(Location a, Location b)
      {
         return Forests.Contains(b) ? 5 : 1;
      }

      public bool CheckPortalExists(Location loc)
      {
         bool ret = Portals.Any(p => p.Item2 == loc);
         return ret;
      }
      
      public Location GetOtherPortalOrDefault(Location loc)
      {         
         var tuple = Portals.FirstOrDefault(t => t.Item2 == loc);
         if (tuple == null)
         {
            return loc;
         }

         var portal = Portals.FirstOrDefault(t => t.Item1 == tuple.Item1 && t.Item2 != loc);
         if (portal == null)
         {
            return loc;
         }

         return portal.Item2;
      }

      public IEnumerable<Location> Neighbors(Location id)
      {
         foreach (var dir in DIRS)
         {
            Location next = new Location(id.X + dir.X, id.Y + dir.Y);
            if (InBounds(next) && Passable(next))
            {
               var ret = GetOtherPortalOrDefault(next);
               if (ret != next)
               {
                  //// Try to jump out of the portal, otherwise land on the other side of the portal.
                  //var jumpForward = new Location(ret.X + dir.X, ret.Y + dir.Y);
                  //var jumpBackward = new Location(ret.X - dir.X, ret.Y - dir.Y);

                  //if (InBounds(jumpForward) && Passable(jumpForward) && !CheckPortalExists(jumpForward))
                  //{
                  //   yield return jumpForward;
                  //}
                  //else if (InBounds(jumpBackward) && Passable(jumpBackward) && !CheckPortalExists(jumpBackward))
                  //{
                  //   yield return jumpBackward;
                  //}
                  {
                     yield return ret;
                  }
               }
               else
               {
                  yield return next;
               }
            }
            //else if (!InBounds(next))
            //{
            //   var ret = GetOtherPortalOrDefault(id);
            //   if (ret != id)
            //   {
            //      yield return ret;
            //   }
            //}
         }
      }
   }


   public class PriorityQueue<T>
   {
      // I'm using an unsorted array for this example, but ideally this
      // would be a binary heap. Find a binary heap class at
      // http://visualstudiomagazine.com/articles/2012/11/01/priority-queues-with-c.aspx

      private List<Tuple<T, int>> elements = new List<Tuple<T, int>>();

      public int Count
      {
         get { return elements.Count; }
      }

      public void Enqueue(T item, int priority)
      {
         elements.Add(Tuple.Create(item, priority));
      }

      public T Dequeue()
      {
         int bestIndex = 0;

         for (int i = 0; i < elements.Count; i++)
         {
            if (elements[i].Item2 < elements[bestIndex].Item2)
            {
               bestIndex = i;
            }
         }

         T bestItem = elements[bestIndex].Item1;
         elements.RemoveAt(bestIndex);
         return bestItem;
      }
   }


   public class AStarSearch
   {
      public Dictionary<Location, Location> cameFrom
          = new Dictionary<Location, Location>();
      public Dictionary<Location, int> costSoFar
          = new Dictionary<Location, int>();

      // Note: a generic version of A* would abstract over Location and
      // also Heuristic
      static public int Heuristic(Location a, Location b)
      {
         return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
      }

      public AStarSearch(WeightedGraph<Location> graph, Location start, Location goal)
      {
         var frontier = new PriorityQueue<Location>();
         frontier.Enqueue(start, 0);

         cameFrom.Add(start, start);
         costSoFar.Add(start, 0);

         while (frontier.Count > 0)
         {
            var current = frontier.Dequeue();

            if (current.Equals(goal))
            {
               break;
            }

            foreach (var next in graph.Neighbors(current))
            {
               int newCost = costSoFar[current]
                   + graph.Cost(current, next);
               if (!costSoFar.ContainsKey(next)
                   || newCost < costSoFar[next])
               {
                  if (costSoFar.ContainsKey(next))
                  {
                     costSoFar[next] = newCost;
                  }
                  else
                  {
                     costSoFar.Add(next, newCost);
                  }
                  int priority = newCost + Heuristic(next, goal);
                  frontier.Enqueue(next, priority);
                  if (cameFrom.ContainsKey(next))
                  {
                     cameFrom[next] = current;
                  }
                  else
                  {
                     cameFrom.Add(next, current);
                  }
               }
            }
         }
      }
   }

   public class Test
   {
      static void DrawGrid(SquareGrid grid, AStarSearch astar)
      {
         // Print out the cameFrom array
         for (var y = 0; y < 10; y++)
         {
            for (var x = 0; x < 10; x++)
            {
               Location id = new Location(x, y);
               Location ptr = id;
               if (!astar.cameFrom.TryGetValue(id, out ptr))
               {
                  ptr = id;
               }
               if (grid.Walls.Contains(id)) { Console.Write("##"); }
               else if (ptr.X == x + 1) { Console.Write("\u2192 "); }
               else if (ptr.X == x - 1) { Console.Write("\u2190 "); }
               else if (ptr.Y == y + 1) { Console.Write("\u2193 "); }
               else if (ptr.Y == y - 1) { Console.Write("\u2191 "); }
               else { Console.Write("* "); }
            }
            Console.WriteLine();
         }
      }
   }
}
