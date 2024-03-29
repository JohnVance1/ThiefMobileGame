using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using FibonacciHeap;


namespace PriorityQueue
{
    public class PriorityQueue<TElement, TPriority> : IPriorityQueue<TElement, TPriority>
        where TPriority : IComparable<TPriority>
    {
        private readonly FibonacciHeap<TElement, TPriority> heap;

        public int Count { get { return heap.Size(); } }

        /// <summary>
        /// The Constructor
        /// </summary>
        /// <param name="minPriority"> The Minimum value for the Priority. This is used when comparing </param>
        public PriorityQueue(TPriority minPriority)
        {
            heap = new FibonacciHeap<TElement, TPriority>(minPriority);
        }

        public void Enqueue(TElement item, TPriority priority)
        {
            heap.Insert(new FibonacciHeapNode<TElement, TPriority>(item, priority));
        }

        public TElement Peek()
        {
            return heap.Min().Data;
        }

        public TElement Dequeue()
        {
            return heap.RemoveMin().Data;
        }
    }
}
