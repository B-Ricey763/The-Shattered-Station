using Godot;
using System;
using System.Collections.Generic;

public abstract class Tower : RigidBody2D
{
    public bool IsPowered {get; private set;}

    protected List<Tether> _tethers = new List<Tether>();

    protected Health _health;
    protected PackedScene _tetherScene;

    public override void _Ready()   
    {
        _health = GetNode<Health>("Health");
        _tetherScene = GD.Load<PackedScene>("res://scenes/Tether.tscn");
    }
    
    public void OnPowerRangeBodyEntered(Node node)
    {
        if (node is Node2D n && n.IsInGroup("powered"))
            Power(n);
    }

    public void OnPowerRangeBodyExited(Node node)
    {
        if (node is Node2D n && n.IsInGroup("powered"))
            Unpower(n);       
    }

    public void Power(Node2D powerNode)
    {
        
        // Create a new tether
        Tether tether = _tetherScene.Instance() as Tether;
        AddChild(tether);
        tether.Link(this, powerNode);
        _tethers.Add(tether);

        // Power our tether if it is not already
        if (!IsPowered)
        {
            IsPowered = true;
            AddToGroup("powered");
        }
    }

    public void Unpower(Node2D powerNode)
    {
        
        // Delete the tether
        Tether tether = FindTether(powerNode);

        if (IsInstanceValid(tether))
        {
            tether.QueueFree();
            _tethers.Remove(tether);
        }

        if (_tethers.Count == 0)
        {
            IsPowered = false;
            RemoveFromGroup("powered");
        }
    }

    private Tether FindTether(Node2D node)
    {
        foreach (Tether t in _tethers)
        {
            if (t.Nodes[1] == node)
                return t;
        }
        return null;
    }
}
