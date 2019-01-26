using Godot;
using System;

public class Globals : Node
{
    private float _playerHealth = 100.0f;
    public float PlayerHealth
    {
        get {
            return _playerHealth;
        }

        set {
            if (value < 0.0f)
            {
                // Do something here
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
            else if (value < 10.0f)
            {
                _fireplaceHealth = 10.0f;
            }
            else
            {
                _fireplaceHealth = value;
            }
        }
    }

    public override void _Ready()
    {
        
    }
}
