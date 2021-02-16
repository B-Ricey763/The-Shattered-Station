using Godot;
using System;

public class Laser : RayCast2D
{
    [Export]
    public readonly float VisualDuration = 0.1f;
    [Export]
    public readonly float Range = 100.0f;

    private Line2D _line;
    private Timer _timer;

    public override void _Ready()
    {
        _line = GetNode<Line2D>("Line");
        _timer = GetNode<Timer>("Timer");

        _timer.WaitTime = VisualDuration;
    }

    public Node2D Fire()
    {        
        _line.Show();

        float dist = (GetCollider() is Node2D) ? (GetCollisionPoint() - GlobalPosition).Length() : Range;
        // Display using right vector as forward
        _line.SetPointPosition(1, Vector2.Right * dist);
        // Start the timer to hide the laser
        _timer.Start();
        // Return the collision point
        return GetCollider() as Node2D;
        // TODO: Add asteroid hit dection here?
    }

    public void OnTimeout()
    {
        // Hide the laser after the time has run out
        _line.Hide();
    }
}
