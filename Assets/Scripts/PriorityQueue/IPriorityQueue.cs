using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PriorityQueue
{
    public interface IPriorityQueue<T, in TKey> where TKey : IComparable<TKey>
    {
        /// <summary>
        /// Inserts an Item with a Priority
        /// </summary>
        /// <param name="item"></param>
        /// <param name="priority"></param>
        void Enqueue(T item, TKey priority);

        /// <summary>
        /// Peek method
        /// </summary>
        /// <returns> The element with the highest Priority </returns>
        T Peek();

        /// <summary>
        /// Pop Method
        /// Deletes the Element with the highest Priority
        /// </summary>
        /// <returns> Returns the element with the highest Priority </returns>
        T Dequeue();
    }
}
