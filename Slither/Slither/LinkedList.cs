using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slither
{
   public class LinkedList<T>
   {
      public class Node<T>
      {
         public Node<T> Next { get; set; }
         public Node<T> Prev { get; set; }
         public T Data { get; set; }
      }

      private Node<T> _Head;
      private Node<T> _Current;

      public LinkedList()
      {
      }

      public void Add(T data)
      {
         if (_Head == null)
         {
            _Head = new Node<T>();
            _Head.Data = data;

            _Head.Next = null;
            _Head.Prev = null;

            _Current = _Head;
            return;
         }

         Node<T> toAdd = new Node<T>();
         toAdd.Data = data;

         Node<T> tail = _Head;

         while (tail.Next != null)
         {
            tail = tail.Next;
         }
         // Assert.IsTrue(tail.Next == null);

         tail.Next = toAdd;
         toAdd.Prev = tail;
         toAdd.Next = null;
      }

      public T Current()
      {
         return _Current.Data;
      }

      public T GetNext()
      {
         Node<T> next = _Current.Next;

         if (next == null)
         {
            return default(T);
         }

         _Current = next;
         return _Current.Data;
      }

      public T GetPrev()
      {
         Node<T> prev = _Current.Prev;

         if (prev == null)
         {
            return default(T);
         }

         _Current = prev;
         return _Current.Data;
      }

      public T PeekNext()
      {
         Node<T> next = _Current.Next;

         if (next == null)
         {
            return default(T);
         }

         return next.Data;
      }

      public T PeekPrev()
      {
         Node<T> prev = _Current.Prev;

         if (prev == null)
         {
            return default(T);
         }

         return prev.Data;
      }
   }
}