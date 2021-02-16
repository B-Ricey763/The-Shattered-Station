using Godot;
using Godot.Collections;
using System;

public class Tether : Node
{
    public bool Linked {get; private set;}

    private Node2D[] _nodes;
    private Line2D _line;

    public override void _Ready()
    {
        SetProcess(false);
        _line = GetNode<Line2D>("Line");
    }

    public override void _Process(float delta)
    {
        for (int i = 0; i < _nodes.Length; i++)
        {
            _line.SetPointPosition(i, _nodes[i].GlobalPosition);
        }
    }

    public void Link(params Node2D[] nodes)
    {
        SetProcess(true);
        Linked = true;
        _line.Show();

        _nodes = nodes;
        
        // Resize our lines list
        _line.ClearPoints();
        foreach (Node2D node in _nodes) 
            _line.AddPoint(node.GlobalPosition);
    }

    public void Unlink()
    {
        SetProcess(false);
        Linked = false;
        _line.Hide();
    }
}
