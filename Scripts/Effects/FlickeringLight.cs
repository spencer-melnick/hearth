using Godot;
using System;

public class FlickeringLight : Light2D
{
    [Export]
    public Vector2 FlickerSpacing = new Vector2(4.0f, 3.0f);

    [Export]
    public float FlickerTime = 0.5f;

    private Random _random = new Random();

    private Timer _timer = null;

    public override void _Ready()
    {
        _timer = new Timer();
        AddChild(_timer);

        _timer.WaitTime = FlickerTime;
        _timer.OneShot = true;

        _timer.Start();
    }

   public override void _Process(float delta)
   {
       if (_timer.IsStopped())
       {
           Vector2 position = new Vector2();

           position.x = FlickerSpacing.x * ((float)_random.NextDouble() * 2.0f - 1.0f);
           position.y = FlickerSpacing.y * ((float)_random.NextDouble() * 2.0f - 1.0f);

           SetPosition(position);
           _timer.Start();
       }
   }
}
