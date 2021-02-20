using Godot;
using Godot.Collections;
using System;

public class Tether : Node
{
    public Node2D[] Nodes {get; private set;}

    private Line2D _line;

    public override void _Ready()
    {
        _line = GetNode<Line2D>("Line");
        SetProcess(false);
    }

    public override void _Process(float delta)
    {
        for (int i = 0; i < Nodes.Length; i++)
        {
            _line.SetPointPosition(i, Nodes[i].GlobalPosition);
        }
    }

    public void Link(params Node2D[] nodes)
    {
        GD.Print(_line);
        SetProcess(true);
        _line.Show();

        Nodes = nodes;
        
        // Resize our lines list
        _line.ClearPoints();
        foreach (Node2D node in Nodes) 
            _line.AddPoint(node.GlobalPosition);
    }

    public void Unlink()
    {
        SetProcess(false);
        _line.Hide();
    }
}
