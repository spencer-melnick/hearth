using Godot;
using System;

public class Fireplace : Node2D
{
    [Export]
    public float HeatInnerRadius = 50.0f;

    [Export]
    public float HeatOuterRadius = 100.0f;

    [Export]
    public bool HealEnabled = false;

    [Export]
    public float HealInnerRadius = 10.0f;

    [Export]
    public float HealOuterRadius = 20.0f;

    [Export]
    public float HealSpeed = 20.0f;


    public override void _Ready()
    {

    }
}
