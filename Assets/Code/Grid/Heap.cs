using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heap<T> where T : IHeapItem<T>
{
    T[] _items;
    private int _currentItemCount;

    public Heap(int maxHeapSize)
    {
        _items = new T[maxHeapSize];
    }
    public void Add(T item)
    {
        item._heapIndex = _currentItemCount;
        _items[_currentItemCount] = item;
        SortUp(item);
        _currentItemCount++;
    }

    public T RemoveFirst()
    {
        T firstItem = _items[0];
        _currentItemCount--;
        _items[0] = _items[_currentItemCount];
        _items[0]._heapIndex = 0;
        SortDown(_items[0]);
        return firstItem;
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    public int Count
    {
        get { return _currentItemCount; }
    }

    public bool Contains(T item)
    {
        return Equals(_items[item._heapIndex], item);
    }

    void SortDown(T item)
    {
        while (true)
        {
            int childIndexLeft = item._heapIndex * 2 + 1;
            int childIndexRight = item._heapIndex * 2 + 2;
            int swapIndex = 0;

            if (childIndexLeft < _currentItemCount)
            {
                swapIndex = childIndexLeft;
                if (childIndexRight < _currentItemCount)
                {
                    if (_items[childIndexLeft].CompareTo(_items[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }

                if (item.CompareTo(_items[swapIndex]) < 0)
                {
                    Swap(item, _items[swapIndex]);
                }
                else
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }
    }

    void SortUp(T item)
    {
        int parentIndex = (item._heapIndex - 1) / 2;

        while (true)
        {
            T parentItem = _items[parentIndex];
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }

            parentIndex = (item._heapIndex - 1) / 2;
        }
    }

    void Swap(T itemA, T itemB)
    {
        _items[itemA._heapIndex] = itemB;
        _items[itemB._heapIndex] = itemA;
        int itemAIndex = itemA._heapIndex;
        itemA._heapIndex = itemB._heapIndex;
        itemB._heapIndex = itemAIndex;

    }
}

public interface IHeapItem<T> : IComparable<T>
{
    int _heapIndex { get; set; }
}