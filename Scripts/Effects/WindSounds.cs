using Godot;
using System;

public class WindSounds : Node2D
{
    [Export]
    public float ChanceOfStrongWind = 0.4f;

    private Random _random = new Random();
    private AnimationPlayer _animationPlayer;

    public override void _Ready()
    {
        _animationPlayer = GetNode("AnimationPlayer") as AnimationPlayer;

        OnAnimationFinished("none");
        _animationPlayer.Connect("animation_finished", this, nameof(OnAnimationFinished));
    }

    public void OnAnimationFinished(String animation)
    {
        if ((float)_random.NextDouble() < ChanceOfStrongWind)
        {
            _animationPlayer.Play("strong_wind");
        }
        else
        {
            _animationPlayer.Play("gentle_wind");
        }
    }
}
