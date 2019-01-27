using Godot;
using System;

public class TitleScreen : Node2D
{
    private AnimationPlayer _animationPlayer;
    private bool _showing = true;

    public override void _Ready()
    {
        SetProcessInput(true);

        _animationPlayer = GetNode("AnimationPlayer") as AnimationPlayer;
        _animationPlayer.Play("idle");
    }

    public void StartGame()
    {
        _showing = false;
        _animationPlayer.Play("fade_out");

        Globals globals = GetNode("/root/Globals") as Globals;
        globals.IsGameRunning = true;
    }

    public override void _Input(InputEvent @event)
    {
        if (_showing)
        {
            if (@event is InputEventKey eventKey)
            {
                if (eventKey.Pressed)
                {
                    StartGame();
                }
            }
            else if (@event is InputEventJoypadButton eventJoypadButton)
            {
                if (eventJoypadButton.Pressed)
                {
                    StartGame();
                }
            }
        }
    }
}
