using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using JumpMan.Properties;

namespace JumpMan
{    
   public class Background
   {
      private CircularList<float> Offsets;
      private float Offset;
      
      private Direction OffsetDirection;
      private Bitmap Image;
      private Bitmap OffsetImage;

      private Rectangle Shape = new Rectangle(0, 0, GlobalValues.BACKGROUND_WIDTH, GlobalValues.BACKGROUND_HEIGHT);

      public Point Location { get; set; }
      public Point FurthestLocation { get; set; }

      public bool EnableOffset { get; set; }

      private List<Player> _Obstacles;
      private Random _Seed;
      private int _Frequency;
      public int PassedObstacles { get; private set; }

      public bool Hit { get; private set; }

      public Background()
      {
         EnableOffset = false;

         Offsets = new CircularList<float>();
         //Offsets.Add(0);
         //Offsets.Add(100);
         //Offsets.Add(200);
         //Offsets.Add(300);
         //Offsets.Add(400);
         //Offsets.Add(500);
         //Offsets.Add(600);
         //Offsets.Add(700);
         //Offsets.Add(800);
         //Offsets.Add(900);

         Offsets.Add(0);
         Offsets.Add(900);
         Offsets.Add(800);
         Offsets.Add(700);
         Offsets.Add(600);
         Offsets.Add(500);
         Offsets.Add(400);
         Offsets.Add(300);
         Offsets.Add(200);
         Offsets.Add(100);

         Offset = 0f;

         OffsetDirection = Direction.None;

         Image = Resources.Hills;

         OffsetImage = new Bitmap(GlobalValues.FORM_WIDTH, GlobalValues.FORM_HEIGHT);

         using (Graphics g = Graphics.FromImage(OffsetImage))
         {
            int x = Location.X + Convert.ToInt32(Offset);
            int y = Location.Y;
            g.DrawImage(Image, x, y);

            x -= GlobalValues.BACKGROUND_WIDTH;
            g.DrawImage(Image, x, y);
         }

         Location = new Point(0, 0);
         FurthestLocation = Location;

         _Obstacles = new List<Player>();
         _Seed = new Random();
         _Frequency = 100;
      }

      public void UpdateOffset(bool enableLeft, bool enableRight)
      {
         if (!EnableOffset)
         {
            return;
         }

         OffsetImage = ShiftImage();

         if (!ContinueOffset(enableLeft, enableRight))
         {
            SetNextOffset(OffsetDirection);
            OffsetDirection = Direction.None;
            EnableOffset = false;
         }

         //if (Offset == GetNextOffset(OffsetDirection))
         //{
         //   SetNextOffset(OffsetDirection);
         //   OffsetDirection = Direction.None;
         //   EnableTurn = false;
         //}
      }

      private float GetNextOffset(Direction direction)
      {
         if (direction == Direction.Left)
         {
            return Offsets.PeekPrev();
         }
         else if (direction == Direction.Right)
         {
            return Offsets.PeekNext();
         }
         else
         {
            return Offsets.Current();
         }
      }

      private void SetNextOffset(Direction direction)
      {
         if (direction == Direction.Left)
         {
            Offsets.GetPrev();
         }
         else if (direction == Direction.Right)
         {
            Offsets.GetNext();
         }
      }

      public void ChangeLanes(Direction dir)
      {
         if (!EnableOffset)
         {
            return;
         }

         if (OffsetDirection == Direction.None)
         {
            OffsetDirection = dir;
         }
      }

      public Bitmap GetImage()
      {
         return new Bitmap(OffsetImage);
      }

      public Bitmap ShiftImage()
      {
         if (OffsetDirection == Direction.Left)
         {
            Offset += GlobalValues.BACKGROUND_SHIFT_INCREMENT;

            var loc = Location;
            loc.Offset(-GlobalValues.BACKGROUND_SHIFT_INCREMENT, 0);
            Location = loc;

            if (Offset >= GlobalValues.BACKGROUND_WIDTH)
            {
               Offset -= GlobalValues.BACKGROUND_WIDTH;
            }
         }
         else if (OffsetDirection == Direction.Right)
         {
            Offset -= GlobalValues.BACKGROUND_SHIFT_INCREMENT;

            var loc = Location;
            loc.Offset(GlobalValues.BACKGROUND_SHIFT_INCREMENT, 0);
            Location = loc;

            if (Location.X > FurthestLocation.X)
            {
               FurthestLocation = Location;
               GenerateRandomObstacle();
            }

            if (Offset < 0)
            {
               Offset += GlobalValues.BACKGROUND_WIDTH;
            }
         }

         //return GameForm.RotateImage(Image, Offset);

         //var newBM = new Bitmap(OffsetImage);
         //using (Graphics g = Graphics.FromImage(newBM))
         //{
         //   int x = Location.X + Convert.ToInt32(Offset);
         //   int y = Location.Y;
         //   g.DrawImage(Image, x, y);
         //}

         using (Graphics g = Graphics.FromImage(OffsetImage))
         {
            int x = Convert.ToInt32(Offset);
            int y = 0;
            g.DrawImage(Image, x, y);

            int xprime = x - GlobalValues.BACKGROUND_WIDTH;
            g.DrawImage(Image, xprime, y);

            // NOTE: Player constantly inside a Rectangle(0,0,28,28) at point(75,150)
            // New Rectangle becomes (75,150,28,28);

            for (int i = _Obstacles.Count - 1; i >= 0; i--)
            {
               var obstacle = _Obstacles[i];

               int xObstacle = 0;
               int yObstacle = 0;
               var newBM = obstacle.GetImage(out xObstacle, out yObstacle);
               xObstacle = xObstacle - Location.X + GlobalValues.BACKGROUND_WIDTH;

               if ((xObstacle + GlobalValues.PLAYER_WIDTH) < 0)
               {
                  _Obstacles.RemoveAt(i);
                  PassedObstacles++;
               }
               else
               {
                  g.DrawImage(newBM, xObstacle, yObstacle);
               }
            }
         }

         return OffsetImage;
      }

      private void GenerateRandomObstacle()
      {
         if (_Seed.Next(_Frequency) == 0)
         {
            var obstacle = new Player();
            obstacle.Location = new Point(FurthestLocation.X, 150);
            _Obstacles.Add(obstacle);
            
            // NOTE: Increases difficulty over time
            if (_Frequency > 0)
            {
               _Frequency--;
            }
         }
      }

      public bool? CheckObstacleCollision(Point location)
      {
         var playerShape = new Rectangle(location.X, location.Y,
            GlobalValues.PLAYER_WIDTH, GlobalValues.PLAYER_HEIGHT);
         
         foreach (var obstacle in _Obstacles)
         {
            int xObstacle = obstacle.Location.X - Location.X + GlobalValues.BACKGROUND_WIDTH;
            int yObstacle = obstacle.Location.Y;

            var obstacleShape = new Rectangle(xObstacle, yObstacle, 
               GlobalValues.PLAYER_WIDTH, GlobalValues.PLAYER_HEIGHT);
            if (obstacleShape.IntersectsWith(playerShape))
            {
               if (playerShape.Bottom <= GlobalValues.PLAYER_START_Y + 10)
               {
                  return false;
               }
               else
               {
                  return true;
               }               
            }
         }

         return null;
      }

      private bool ContinueOffset(bool enableLeft, bool enableRight)
      {
         return (enableLeft && OffsetDirection == Direction.Left)
            || (enableRight && OffsetDirection == Direction.Right);
      }
   }
}