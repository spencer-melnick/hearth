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
            }
            else
            {
                _playerHealth = value;
            }
        }
    }

    public override void _Ready()
    {
        
    }
}
