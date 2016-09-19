using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using TwinCars.Properties;

namespace TwinCars
{  
   public enum Turning
   {
      None,
      Right,
      Left
   }

   public enum Lane
   {
      Left,
      Right
   }
   
   public class Car
   {
      Color DeepBlue = Color.FromArgb(0, 10, 25);

      public float Angle { get; set; }
      public float Distance { get; set; }

      public Turning TurningEnum { get; set; }
      public Lane CarLane { get; set; }
      public Bitmap CarImage { get; set; }
      public Bitmap TurningCarImage { get; set; }
      public int XAxis { get; set; }
      public int YAxis { get; set; }

      public bool EnableTurn { get; set; }

      public int _TurningDistance;

      public Car(Lane lane)
      {
         EnableTurn = false;
         YAxis = 290;

         Angle = 0f;
         TurningEnum = Turning.None;
         CarLane = lane;
         if (lane == Lane.Left)
         {
            _TurningDistance = 0; //0-25 inclusive
            CarImage = Resources.RedCar;
            TurningCarImage = CarImage;
            XAxis = 11;
         }
         else //if(lane == Lane.Right)
         {
            _TurningDistance = 25; //0-25 inclusive
            Distance = 25f;
            CarImage = Resources.BlueCar;
            TurningCarImage = CarImage;
            XAxis = 111;
         }
      }

      public void UpdateLanes()
      {
         if (!EnableTurn)
         {
            return;
         }

         switch (TurningEnum)
         {
            case Turning.Left:
               _TurningDistance--;
               TurningCarImage = RotateImageGoingLeft();
               break;
            case Turning.Right:
               _TurningDistance++;
               TurningCarImage = RotateImageGoingRight();
               break;
            case Turning.None:
            default:
               break;
         }

         if (_TurningDistance == 0 || _TurningDistance == 25)
         {
            if (TurningEnum == Turning.Right)
            {
               CarLane = Lane.Right;
            }
            else if (TurningEnum == Turning.Left)
            {
               CarLane = Lane.Left;
            }

            TurningEnum = Turning.None;
            EnableTurn = false;
         }
      }

      public void ChangeLanes()
      {
         if (!EnableTurn)
         {
            return;
         }

         if (TurningEnum == Turning.None)
         {
            if (CarLane == Lane.Left)
            {
               TurningEnum = Turning.Right;
            }
            else if (CarLane == Lane.Right)
            {
               TurningEnum = Turning.Left;
            }
         }
         else if (TurningEnum == Turning.Right)
         {
            TurningEnum = Turning.Left;
         }
         else if (TurningEnum == Turning.Left)
         {
            TurningEnum = Turning.Right;
         }
      }

      public Bitmap GetImage(out int x, out int y)
      {
         x = XAxis + 2 * (int)Distance;
         y = YAxis;
         return TurningCarImage;
      }

      public int GetXAxis()
      {
         return XAxis + 2 * (int)Distance;
      }

      public Bitmap RotateImageGoingRight()
      {
         Angle = (float)(-22 * Math.Cos(_TurningDistance / (1.25 * Math.PI)) + 23);
         Distance = (float)(-12.5 * Math.Cos(_TurningDistance / (2.5 * Math.PI)) + 12.5);
         return GameForm.RotateImg(CarImage, Angle, DeepBlue);
      }

      public Bitmap RotateImageGoingLeft()
      {
         Angle = (float)(22 * Math.Cos(_TurningDistance / (1.25 * Math.PI)) - 23);
         Distance = (float)(-12.5 * Math.Cos(_TurningDistance / (2.5 * Math.PI)) + 12.5);
         return GameForm.RotateImg(CarImage, Angle, DeepBlue);
      }
   }
}
