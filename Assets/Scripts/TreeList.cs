using System.Collections.Generic;

/*
 * This class is from Matthew Layton on Stack Overflow
 */
public sealed class TreeList<T> : List<TreeList<T>> {
    public List<T> Values { get; } = new List<T>();

    public TreeList<T> this[int index] {
        get {
            while (index > Count - 1) {
                Branch();
            }

            return base[index];
        }
    }

    public TreeList<T> Branch() {
        TreeList<T> result = new TreeList<T>();

        Add(result);

        return result;
    }
}