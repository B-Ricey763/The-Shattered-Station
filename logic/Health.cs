using Godot;
using System;

public class Health : Node2D
{
    [Signal]
    public delegate void Changed();
    [Signal]
    public delegate void Died();

    [Export]
    public float Max {get; private set;}

    private float _value;

    public float Value {
        get => _value;
        set {
            _value = Mathf.Clamp(value, 0.0f, Max); // Clamping
            _bar.Value = value/Max * 100; // Visuals
            if (_value == 0) 
                EmitSignal(nameof(Died)); // Death
            else
                EmitSignal(nameof(Changed), _value); // Just changing
        }
    }

    private ProgressBar _bar;

    public override void _Ready()
    {
        _bar = GetNode<ProgressBar>("Gui/Container/HealthBar");
        Value = Max;
    }
}
