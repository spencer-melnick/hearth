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

    public override void _Ready()
    {
        
    }
}
