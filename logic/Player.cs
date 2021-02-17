using Godot;
using System;

public class Player : KinematicBody2D
{
    [Export]
    public float MaxSpeed = 20.0f;
    [Export]
    public float Acceleration = 4.5f;
    [Export]
    public float Deceleration = 16.0f;
    [Export]
    public readonly float Damage = 1.0f;

    private Vector2 _velocity = new Vector2();
    private Vector2 _direction = new Vector2();

    private Camera2D _cam;
    private Laser _laser;

    public override void _Ready()
    {
        _cam = GetNode<Camera2D>("Camera");
        _laser = GetNode<Laser>("Laser");
    }

    public override void _PhysicsProcess(float delta)
    {
        ProcessInput(delta);
        ProcessMovement(delta);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton eventMouseButton)
            OnMouseClick(eventMouseButton);
        else if (@event is InputEventMouseMotion eventMouseMotion)
            OnMoveMove(eventMouseMotion);
    }

    private void OnMoveMove(InputEventMouseMotion eventMouseMotion)
    {
        LookAt(GetGlobalMousePosition());
    }

    private void OnMouseClick(InputEventMouseButton eventMouseButton)
    {
        if (eventMouseButton.Pressed && eventMouseButton.ButtonIndex == (int) ButtonList.Left) 
        {
            Node2D hit = _laser.Fire();
            
            // Possibly redundant code, see Turret.cs for details
            if (hit is Asteroid asteroid)
                asteroid.Health -= Damage;
        }

    }

    /// <summary> Captures Player Input and applies it to direction and velocity </summary>
    private void ProcessInput(float delta)
    {
        // Reset our direction vector
        _direction = new Vector2();

        /* CAPTURING PLAYER MOVEMENT INPUT */

        Vector2 inputVector = new Vector2();

        if (Input.IsActionPressed("movement_up"))
            inputVector.y -= 1;
        if (Input.IsActionPressed("movement_down"))
            inputVector.y += 1;
        if (Input.IsActionPressed("movement_left"))
            inputVector.x -= 1;
        if (Input.IsActionPressed("movement_right"))
            inputVector.x += 1;

        // Normalize so diagonals don't go faster than cardinals
        inputVector = inputVector.Normalized();

        // Apply input to direction (used in Movement)
        _direction += inputVector;
    }

    /// <summary> Uses player input to move the body </summary>
    private void ProcessMovement(float delta)
    {
        _direction = _direction.Normalized();

        Vector2 target = _direction;
        target *= MaxSpeed;

        float currentAcceleration = _direction.Dot(_velocity) > 0 ? Acceleration : Deceleration;
        _velocity = _velocity.LinearInterpolate(target, currentAcceleration * delta);        
        _velocity = MoveAndSlide(_velocity, new Vector2(0, 0), false, 4);
    }

}
