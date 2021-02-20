using Godot;
using System;

public class Main : Node2D
{
    [Export]
    public readonly float Radius = 25.0f;
    [Export]
    public readonly float TowerImpulseSpeed = 100.0f;
    [Export]
    public readonly PackedScene[] Towers;

    private Random _rand = new Random();
    private PackedScene _asteroid;

    // Terrible practice, but I don't care
    private Station _station;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _asteroid = GD.Load<PackedScene>("res://scenes/Asteroid.tscn");

        _station = GetNode<Station>("Station");
    }

    public void SpawnAsteroid()
    {
        Vector2 pos = GetRandomPosition(Radius);
        Asteroid asteroid = _asteroid.Instance() as Asteroid;
        asteroid.GlobalPosition = pos;
        asteroid.Target = _station;
        AddChild(asteroid);
    }

    public void SpawnTower()
    {  
        // Get the random position
        Vector2 pos = GetRandomPosition(Radius);
        // Pick a random tower
        Tower tower = Towers[_rand.Next(Towers.Length)].Instance() as Tower;
        AddChild(tower);
        tower.GlobalPosition = pos + _station.GlobalPosition;
        Vector2 baseImpulse = (_station.GlobalPosition - tower.GlobalPosition).Normalized();
        Vector2 randomDirection = GetRandomPosition(1).Normalized();
        tower.ApplyCentralImpulse((baseImpulse + randomDirection).Normalized() * TowerImpulseSpeed);

    }

    private Vector2 GetRandomPosition(float radius)
    {
        // Doesn't work very well. Oh well.
        float theta = (float) (_rand.Next() * Math.PI * 2);
        float x = (float) (Math.Cos(theta) * radius);
        float y = (float) (Math.Sin(theta) * radius);
        return new Vector2(x, y);
    }
}
