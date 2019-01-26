using Godot;
using System;

public class InteractTooltip : Node2D
{
    private Area2D _area;
    private AnimationPlayer _animationPlayer;

    [Export]
    public bool Enabled = true;

    [Export]
    public bool OneShot = false;

    [Export]
    public bool UseAreaTrigger = true;

    private enum State
    {
        Invisible,
        Animating,
        FadedIn,
        FadedOut
    }

    private State _state = State.Invisible;

    public override void _Ready()
    {
        _area = GetNode("Area2D") as Area2D;
        _area.Connect("body_entered", this, nameof(_onBodyEntered));

        _animationPlayer = GetNode("AnimationPlayer") as AnimationPlayer;
        _animationPlayer.Connect("animation_finished", this, nameof(_onAnimationFinished));
    }

    public void FadeOut()
    {
        if (_state == State.Invisible)
        {
            _state = State.FadedOut;
        }
        else
        {
            _fadeOut();
        }
    }

    private void _fadeOut()
    {
        _animationPlayer.Play("fade_out");
    }

    public void FadeIn()
    {
        if (Enabled && _state == State.Invisible)
        {
            _animationPlayer.Play("fade_in");
            _state = State.Animating;
        }  
    }

    private void _onBodyEntered(Godot.Object body)
    {
        if (UseAreaTrigger)
        {        
            BasicCharacter character = body as BasicCharacter;

            if (character != null)
            {
                FadeIn();
            }
        }
    }

    private void _onAnimationFinished(String name)
    {
        if (name == "fade_in")
        {
            if (_state == State.FadedOut)
            {
                _fadeOut();
            }
            else
            {
                _state = State.FadedIn;
            }
        }

        if (name == "fade_out" && !OneShot)
        {
            if (!OneShot)
            {
                _state = State.Invisible;
            }
        }
    }
}
