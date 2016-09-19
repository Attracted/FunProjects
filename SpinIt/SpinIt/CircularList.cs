using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpinIt
{
   public class CircularList<T>
   {
      public class Node<T>
      {
         public Node<T> Next { get; set; }
         public Node<T> Prev { get; set; }
         public T Data { get; set; }
      }

      private Node<T> _Head;
      private Node<T> _Current;

      public CircularList()
      {
      }

      public void Add(T data)
      {
         if (_Head == null)
         {
            _Head = new Node<T>();
            _Head.Data = data;

            _Head.Next = _Head;
            _Head.Prev = _Head;

            _Current = _Head;
            return;
         }
         
         Node<T> toAdd = new Node<T>();
         toAdd.Data = data;

         Node<T> tail = _Head;

         while (tail.Next != _Head)
         {
            tail = tail.Next;
         }
         // Assert.IsTrue(tail.Next == Head);

         tail.Next = toAdd;
         toAdd.Prev = tail;

         toAdd.Next = _Head;
         _Head.Prev = toAdd;
      }
      
      public T Current()
      {
         return _Current.Data;
      }
      
      public T GetNext()
      {
         _Current = _Current.Next;
         return _Current.Data;
      }

      public T GetPrev()
      {
         _Current = _Current.Prev;
         return _Current.Data;
      }

      public T PeekNext()
      {
         return _Current.Next.Data;
      }

      public T PeekPrev()
      {
         return _Current.Prev.Data;
      }
   }
}