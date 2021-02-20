using Godot;
using System;

public class Main : Node2D
{   
    [Export]
    public int AsteroidBatchCount = 3;
    [Export]
    public readonly float TowerSpawnIncrease = 0.05f;
    [Export]
    public readonly float Radius = 25.0f;
    [Export]
    public readonly float TowerImpulseSpeed = 100.0f;
    [Export]
    public readonly PackedScene[] Towers;

    private Random _rand = new Random();
    private PackedScene _asteroid;
    private int _asteroidSpawns = 0;

    // Terrible practice, but I don't care
    private Station _station;
    private Timer _asteroidTimer;
    private Timer _towerTimer;
    private Globals _globals;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _asteroid = GD.Load<PackedScene>("res://scenes/Asteroid.tscn");

        _globals = GetNode<Globals>("/root/Globals");
        _asteroidTimer = GetNode<Timer>("AsteroidSpawnTimer");
        _towerTimer = GetNode<Timer>("TowerSpawnTimer");
        _station = GetNode<Station>("Station");

        _globals.Load();
    }

    public void SpawnAsteroid()
    {
        for (int i = 0; i < AsteroidBatchCount; i++)
        {
            // Spawn in the asteroid
            Vector2 pos = GetRandomPosition(Radius);
            Asteroid asteroid = _asteroid.Instance() as Asteroid;
            asteroid.GlobalPosition = pos;
            asteroid.Target = _station;
            AddChild(asteroid);
        }
      
        // Increase speed
        Asteroid.TargetingSpeed += 0.1f;
        // Increase Batch count every 10 times
        if (_asteroidSpawns % 10 == 0)
            AsteroidBatchCount++;

        _asteroidSpawns++;
    }

    public void SpawnTower()
    {  
        // Spawn Tower

        // Get the random position
        Vector2 pos = GetRandomPosition(Radius);
        // Pick a random tower
        Tower tower = Towers[_rand.Next(Towers.Length)].Instance() as Tower;
        AddChild(tower);
        tower.GlobalPosition = pos + _station.GlobalPosition;
        Vector2 baseImpulse = (_station.GlobalPosition - tower.GlobalPosition).Normalized();
        Vector2 randomDirection = GetRandomPosition(1).Normalized();
        tower.ApplyCentralImpulse((baseImpulse + randomDirection).Normalized() * TowerImpulseSpeed);

        // Increase Spawn time
        _towerTimer.WaitTime += TowerSpawnIncrease;
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
