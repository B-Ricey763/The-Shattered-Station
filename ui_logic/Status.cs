using Godot;
using System;

public class Status : Control
{
    private ProgressBar _healthBar;

    public override void _Ready()
    {
        _healthBar = GetNode<ProgressBar>("Container/HealthBar");
    }

    public void HealthChanged(float health)
    {
        _healthBar.Value = health;
    }
}
