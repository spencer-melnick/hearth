using Godot;
using System;

public class HealthBar : Control
{
    private ProgressBar _progressBar;
    private Globals _globals;
    public override void _Ready()
    {
        _globals = GetNode("/root/Globals") as Globals;
        _progressBar = GetNode("ProgressBar") as ProgressBar;
    }

   public override void _Process(float delta)
   {
        _progressBar.Value = _globals.PlayerHealth;
   }
}
