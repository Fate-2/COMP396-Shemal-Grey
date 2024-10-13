using System.Collections;

public class PriorityQueue 
{
    private ArrayList _nodes = new ArrayList();

    public int Length => _nodes.Count;

    public bool Contains(object node) => _nodes.Contains(node);


    public Node GetFirst()
    {
        if (_nodes.Count > 0)
        {
            return (Node)_nodes[0];
        }
        return null;
    }

    public void Add(Node node)
    {
        _nodes.Add(node);
        _nodes.Sort(); // Use the IComparable interface to compare and sort it
    }

    public void Remove(Node node)
    {
        _nodes.Remove(node);
        _nodes.Sort();
    }
}
