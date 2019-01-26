using Godot;
using System;

public class InteractTooltip : Node2D
{
    private Area2D _area;
    private AnimationPlayer _animationPlayer;

    private enum State
    {
        Invisible,
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

    private void _onBodyEntered(Godot.Object body)
    {
        if (_state == State.Invisible)
        {
            BasicCharacter character = body as BasicCharacter;
            if (character != null)
            {
                _animationPlayer.Play("fade_in");
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
    }
}
