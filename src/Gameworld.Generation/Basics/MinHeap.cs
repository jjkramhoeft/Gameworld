namespace Gameworld.Generation.Basics;

/// <summary>
/// A binary min-heap of (key, cell index) pairs. Ties on the key are broken by the smaller
/// cell index, so results do not depend on insertion order (map-generation.md section 2.5).
/// </summary>
public sealed class MinHeap
{
    private double[] _keys = new double[256];
    private int[] _items = new int[256];

    public int Count { get; private set; }

    public void Clear() => Count = 0;

    public void Push(double key, int item)
    {
        if (Count == _keys.Length)
        {
            Array.Resize(ref _keys, Count * 2);
            Array.Resize(ref _items, Count * 2);
        }
        var i = Count++;
        while (i > 0)
        {
            var parent = (i - 1) >> 1;
            if (!Less(key, item, _keys[parent], _items[parent])) break;
            _keys[i] = _keys[parent];
            _items[i] = _items[parent];
            i = parent;
        }
        _keys[i] = key;
        _items[i] = item;
    }

    public (double Key, int Item) Pop()
    {
        var top = (_keys[0], _items[0]);
        Count--;
        if (Count > 0)
        {
            var key = _keys[Count];
            var item = _items[Count];
            var i = 0;
            while (true)
            {
                var child = 2 * i + 1;
                if (child >= Count) break;
                if (child + 1 < Count && Less(_keys[child + 1], _items[child + 1], _keys[child], _items[child]))
                    child++;
                if (!Less(_keys[child], _items[child], key, item)) break;
                _keys[i] = _keys[child];
                _items[i] = _items[child];
                i = child;
            }
            _keys[i] = key;
            _items[i] = item;
        }
        return top;
    }

    private static bool Less(double ka, int ia, double kb, int ib) => ka < kb || (ka == kb && ia < ib);
}
