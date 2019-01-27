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
            var characters = GetTree().GetNodesInGroup("main_character");

            if (characters != null)
            {
                return characters[0] as BasicCharacter;
            }

            return null;   
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
				MainCharacter.Die();
                GD.Print("Player died at " + MainCharacter.GetGlobalPosition());
                GD.Print(MainCharacter.GetGlobalPosition().Length() + " from spawn");
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

    public bool IsGameRunning = false;

    public void Restart()
    {
        _playerHealth = 100.0f;
        IsNewFireplaceVisible = false;
        _fireplaceHealth = 100.0f;
        NewFireplaceHealth = 0.0f;
    }

    public override void _Ready()
    {
        
    }
}
