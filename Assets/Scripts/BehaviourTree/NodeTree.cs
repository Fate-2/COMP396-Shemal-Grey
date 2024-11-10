using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeStates
{
    FAILURE, SUCCESS, RUNNING
}

public abstract class NodeTree
{
    public delegate NodeStates NodeReturn();
    protected NodeStates _nodeState;

    public NodeStates nodeStates { get { return _nodeState; } }

    public NodeTree() { }

    public abstract NodeStates Evaluate();
}

public class Selector : NodeTree
{
    protected List<NodeTree> _nodes = new();
    public Selector(List<NodeTree> nodes)
    {
        this._nodes = nodes;
    }

    public override NodeStates Evaluate()
    {
        foreach (var node in _nodes)
        {
            switch (node.Evaluate())
            {
                case NodeStates.FAILURE:
                    continue;
                case NodeStates.SUCCESS:
                    _nodeState = NodeStates.SUCCESS;
                    return _nodeState;
                case NodeStates.RUNNING:
                    _nodeState = NodeStates.RUNNING;
                    return _nodeState;
                default:
                    continue;
            }
        }
        _nodeState = NodeStates.FAILURE;
        return _nodeState;
    }
}
public class Sequence : NodeTree
{
    protected List<NodeTree> _nodes = new();
    public Sequence(List<NodeTree> nodes)
    {
        _nodes = nodes;
    }
    public override NodeStates Evaluate()
    {
        bool anyChildStillRunning = false;
        foreach (var node in _nodes)
        {
            switch (node.Evaluate())
            {
                case NodeStates.FAILURE:
                    _nodeState = NodeStates.FAILURE;
                    return _nodeState;
                case NodeStates.SUCCESS:
                    continue;
                case NodeStates.RUNNING:
                    anyChildStillRunning = true;
                    continue;
                default:
                    _nodeState = NodeStates.SUCCESS;
                    return _nodeState;
            }
        }
        _nodeState = anyChildStillRunning ? NodeStates.RUNNING : NodeStates.SUCCESS;
        return _nodeState;
    }
}
public class Inverter : NodeTree
{
    private NodeTree _node;
    public NodeTree node { get { return _node; } }

    public Inverter(NodeTree node)
    {
        _node = node;
    }
    public override NodeStates Evaluate()
    {
        switch (_node.Evaluate())
        {
            case NodeStates.FAILURE:
                _nodeState = NodeStates.SUCCESS;
                break;
            case NodeStates.SUCCESS:
                _nodeState = NodeStates.FAILURE;
                break;
            case NodeStates.RUNNING:
                _nodeState = NodeStates.RUNNING;
                break;
            default:
                _nodeState = NodeStates.SUCCESS;
                break;
        }
        return _nodeState;
    }
}
public class ActionNode : NodeTree
{
    public delegate NodeStates ActionNodeDelegate();
    private ActionNodeDelegate _nodeAction;

    public ActionNode(ActionNodeDelegate nodeAction)
    {
        _nodeAction = nodeAction;
    }

    public override NodeStates Evaluate()
    {
        switch (_nodeAction())
        {
            case NodeStates.FAILURE:
                _nodeState = NodeStates.FAILURE;
                break;
            case NodeStates.SUCCESS:
                _nodeState = NodeStates.SUCCESS;
                break;
            case NodeStates.RUNNING:
                _nodeState = NodeStates.RUNNING;
                break;
            default:
                _nodeState = NodeStates.FAILURE;
                break;
        }
        return _nodeState;
    }
}