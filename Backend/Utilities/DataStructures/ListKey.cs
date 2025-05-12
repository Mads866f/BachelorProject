namespace Backend.Utilities.DataStructures;

public class ListKey<T> : IEqualityComparer<List<T>>
{
    public bool Equals(List<T>? x, List<T>? y)
    {
        return x.SequenceEqual(y);
    }

    public int GetHashCode(List<T> obj)
    {
        unchecked
        {
            int hash = 19;
            foreach (var item in obj)
                hash = hash * 31 + (item?.GetHashCode() ?? 0);
            return hash;
        }
    }
}