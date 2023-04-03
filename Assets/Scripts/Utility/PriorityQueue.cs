using System.Collections.Generic;
using System.Linq;

class PriorityQueue<T>
{
    private SortedDictionary<float, Queue<T>> dict = new SortedDictionary<float, Queue<T>>();

    public void Enqueue(T item, float priority)
    {
        if (!dict.TryGetValue(priority, out Queue<T> queue))
        {
            queue = new Queue<T>();
            dict.Add(priority, queue);
        }
        queue.Enqueue(item);
    }

    public T Dequeue()
    {
        var kvp = dict.First();
        var queue = kvp.Value;
        var item = queue.Dequeue();
        if (queue.Count == 0)
        {
            dict.Remove(kvp.Key);
        }
        return item;
    }

    public T Peek()
    {
        var kvp = dict.First();
        return kvp.Value.Peek();
    }

    public int Count => dict.Sum(kvp => kvp.Value.Count);
}
