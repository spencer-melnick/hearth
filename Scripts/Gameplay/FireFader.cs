using Godot;
using System;

public class FireFader : Node2D
{
    [Export]
    public bool Enabled = true;

    [Export]
    public float FadeSpeed = 1.0f;

    private Globals _globals;

    public override void _Ready()
    {
        _globals = GetNode("/root/Globals") as Globals;
    }

   public override void _Process(float delta)
   {
        if (_globals.IsGameRunning)
        {
            _globals.FireplaceHealth -= FadeSpeed * delta;
        }
   }
}
