using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CountTheDays
{
   public enum Months
   {
      January, 
      February,
      March,
      April,
      May,
      June,
      July,
      August,
      September,
      October,
      November,
      December
   }
   
   public class Program
   {
      static void Main(string[] args)
      {
         while(true)
         {
            try
            {
               Console.Write("Enter START date in the form of Year/Month/Day: ");
               var startDay = DateTime.Parse(Console.ReadLine());

               Console.Write("Enter END date in the form of Year/Month/Day: ");
               var endDate = DateTime.Parse(Console.ReadLine());

               var days = (endDate - startDay).TotalDays;

               Console.Write("Enter average DAYS worked in a WEEK: ");
               var workingDays = days * double.Parse(Console.ReadLine()) / 7;

               Console.Write("Enter average HOURS worked in a DAY: ");
               var workingHours = workingDays * double.Parse(Console.ReadLine());

               Console.Write("Enter average PAY: ");
               var workingPay = double.Parse(Console.ReadLine());

               Console.Write("Enter average DEDUCTIBLE (< 1): ");
               var deductible = double.Parse(Console.ReadLine());

               Console.Write("In this time, you have made $");
               Console.WriteLine(workingHours * workingPay * (1 - deductible));
            }
            catch (FormatException e)
            {
               Console.WriteLine("Improper inputs");
            }
            finally
            {
               Console.WriteLine();
               Console.WriteLine();
               Console.WriteLine();
            }
         }
      }
   }
}
