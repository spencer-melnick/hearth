using Godot;
using System;

public class Globals : Node
{
    private float _playerHealth = 100.0f;

    private BasicCharacter _mainCharacter;

    public BasicCharacter MainCharacter
    {
        get
        {
            return _mainCharacter;
        }
    }

    private float _minFireplaceHealth = 10.0f;

    public float MinFireplaceHealth
    {
        set
        {
            _minFireplaceHealth = value;

            if (_fireplaceHealth < _minFireplaceHealth)
            {
                _fireplaceHealth = _minFireplaceHealth;
            }
        }

        get
        {
            return _minFireplaceHealth;
        }
    }

    public float PlayerHealth
    {
        get {
            return _playerHealth;
        }

        set {
            if (value < 0.0f)
            {
				_mainCharacter.Die();
                GD.Print("Player died at " + _mainCharacter.GetGlobalPosition());
                GD.Print(_mainCharacter.GetGlobalPosition().Length() + " from spawn");
                IsGameRunning = false;
                value = 0.0f;
            }
            else if (value > 100.0f)
            {
                _playerHealth = 100.0f;
            }
            else
            {
                _playerHealth = value;
            }
        }
    }

    private float _fireplaceHealth = 100.0f;
    public float FireplaceHealth
    {
        get
        {
            return _fireplaceHealth;
        }

        set
        {
            if (value > 100.0f)
            {
                _fireplaceHealth = 100.0f;
            }
            else if (value < _minFireplaceHealth)
            {
                _fireplaceHealth = _minFireplaceHealth;
            }
            else
            {
                _fireplaceHealth = value;
            }
        }
    }

    public bool IsNewFireplaceVisible = false;

    public float NewFireplaceHealth = 0.0f;

    public bool IsGameRunning = true;

    public override void _Ready()
    {
        var characters = GetTree().GetNodesInGroup("main_character");

        if (characters != null)
        {
            _mainCharacter = characters[0] as BasicCharacter;
        }   
    }
}
