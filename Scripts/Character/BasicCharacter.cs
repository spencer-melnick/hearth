using Godot;
using System;
using System.Collections.Generic;

using AdventureGame;
public class BasicCharacter : KinematicBody2D
{
    [Export]
	public float WalkSpeed = 80.0f;

	[Export]
	public float FreezeSpeed = 10.0f;

	[Export]
	public Vector2 WalkScale = new Vector2(1.0f, 1.0f);

	private Globals _globals;

	private bool _fuelHeld = false;

	private Node2D _carriedObject = null;

	private Particles2D _healthParticles;

	public Node2D HeldObject
	{
		get
		{
			return _carriedObject;
		}
	}

	private Node2D _carryAttachPoint;
	private AnimationPlayer _animationPlayer;
	private List<AudioStreamPlayer> _footprintPlayers;
	private Random _random;
	
	private String _currentAnimation = "Walk_Down";
	private bool _animationPlaying = false;

	public bool FuelHeld
	{
		get
		{
			return _fuelHeld;
		}
	}
	
	private struct AxisMapping
	{
		public string ActionName;
		public Vector2 Axis;
		
		public AxisMapping(string actionName, Vector2 axis)
		{
			ActionName = actionName;
			Axis = axis;
		}
	}
	
	private readonly AxisMapping[] _axisMappings = {
		new AxisMapping("character_move_left", new Vector2(-1.0f, 0.0f)),
		new AxisMapping("character_move_right", new Vector2(1.0f, 0.0f)),
		new AxisMapping("character_move_up", new Vector2(0.0f, -1.0f)),
		new AxisMapping("character_move_down", new Vector2(0.0f, 1.0f))
	};

    public override void _Ready()
    {
		_globals = GetNode("/root/Globals") as Globals;
		_carryAttachPoint = GetNode("carry_attach_point") as Node2D;
		_healthParticles = GetNode("health_particles") as Particles2D;
		_animationPlayer = GetNode("AnimationPlayer") as AnimationPlayer;

		_footprintPlayers = new List<AudioStreamPlayer>();
		_footprintPlayers.Add(GetNode("footprint_sound_1") as AudioStreamPlayer);
		_footprintPlayers.Add(GetNode("footprint_sound_2") as AudioStreamPlayer);

		_random = new Random();
    }

	public override void _Process(float delta)
	{
		if (_globals.IsGameRunning)
		{
			CheckHeat(delta);

			if (Input.IsActionJustPressed("character_primary_interact"))
			{
				_checkInteractions();
			}
		}
	}
	
	private void _updateAnimation(Vector2 axisInput)
	{
		String nextAnimation = "";
		
		bool shouldPlay = true;
		
		if (axisInput.LengthSquared() == 0.0f)
		{
			shouldPlay = false;
			nextAnimation = _currentAnimation;
			nextAnimation = nextAnimation.Replace("_Carry", "");
		}
		else if (axisInput.x > 0.0f)
		{
			nextAnimation = "Walk_Right";
		}
		else if (axisInput.x < 0.0f)
		{
			nextAnimation = "Walk_Left";
		}
		else if (axisInput.y > 0.0f)
		{
			nextAnimation = "Walk_Down";
		}
		else if (axisInput.y < 0.0f)
		{
			nextAnimation = "Walk_Up";
		}
		
		if (_carriedObject != null)
		{
			nextAnimation += "_Carry";
		}
		
		if (nextAnimation != _currentAnimation)
		{
			_animationPlayer.Play(nextAnimation);
			_currentAnimation = nextAnimation;
			
			if (!shouldPlay)
			{
				_animationPlaying = false;
				_animationPlayer.Seek(0.0f, true);
				_animationPlayer.Stop(false);
			}
			else
			{
				_animationPlaying = true;
			}
		}
		else if (!_animationPlaying && shouldPlay)
		{
			_animationPlaying = true;
			_animationPlayer.Play(_currentAnimation);
		}
		else if (_animationPlaying && !shouldPlay)
		{
			_animationPlaying = false;
			_animationPlayer.Stop();
		}
	}

	public override void _PhysicsProcess(float delta) {
		if (_globals.IsGameRunning)
		{
			Vector2 axisInput = new Vector2(0.0f, 0.0f);

			foreach (AxisMapping axisMapping in _axisMappings)
			{
				if (Input.IsActionPressed(axisMapping.ActionName)) {
					axisInput += axisMapping.Axis;
				}
			}
			_updateAnimation(axisInput);

			axisInput = axisInput.Normalized();
			
			Vector2 velocity = axisInput * WalkSpeed * WalkScale;
			KinematicCollision2D collision = MoveAndCollide(velocity * delta);

			if (collision != null)
			{
				Node2D collider = collision.Collider as Node2D;
				if (collider != null && collider.IsInGroup("pushable")) {
					IPushable pushedObject = collider as IPushable;
					pushedObject?.TryPush(collision.Normal, delta);
				}

				velocity = velocity.Slide(collision.Normal);
				MoveAndCollide(velocity * delta);
			}
		}
	}

	private void _checkInteractions()
	{
		var interactables = GetTree().GetNodesInGroup("interactive");

		foreach (Node node in interactables)
		{
			IInteractive interactable = node as IInteractive;

			if (interactable != null)
			{
				Area2D interactiveArea = interactable.GetInteractiveArea();

				if (interactiveArea.OverlapsBody(this))
				{
					interactable.TryInteract(this);
				}
			}
		}
	}

	private void CheckHeat(float delta)
	{
		var fireplaces = GetTree().GetNodesInGroup("fireplace");
		float heatRate = -FreezeSpeed;

		foreach (Node node in fireplaces)
		{
			Fireplace fireplace = node as Fireplace;

			if (fireplace != null)
			{
				float distance = fireplace.GlobalPosition.DistanceTo(GlobalPosition);
				
				if (distance < fireplace.HeatInnerRadius)
				{
					heatRate += fireplace.HeatSpeed;
				}
				else if (distance < fireplace.HeatOuterRadius)
				{
					heatRate += fireplace.HeatSpeed * (fireplace.HeatOuterRadius - distance)
								/ (fireplace.HeatOuterRadius - fireplace.HeatInnerRadius);
				
				}
			}
		}

		_healthParticles.Emitting = false;
		_globals.PlayerHealth += heatRate * delta;
			
		if (_globals.PlayerHealth < 100.0f && heatRate > 0.0f)
		{
			_healthParticles.Emitting = true;
		}
	}

	public void PickupObject(Node2D node)
	{
		if (_carriedObject == null)
		{
			if (node.IsInGroup("fuel"))
			{
				_fuelHeld = true;
			}
			
			_carriedObject = node;
			node.GetParent().RemoveChild(node);
			_carryAttachPoint.AddChild(node);
			node.SetPosition(new Vector2());
		}
	}

	public void DestroyHeldObject()
	{
		_carryAttachPoint.RemoveChild(_carriedObject);
		_carriedObject.QueueFree();
		_carriedObject = null;

		if (_fuelHeld)
		{
			_fuelHeld = false;
		}
	}

	public void PlayFootprintSound()
	{
		int footprintSoundNumber = _random.Next(0, _footprintPlayers.Count);
		_footprintPlayers[footprintSoundNumber].Play();
		GD.Print("Footprint sound");
	}
}
