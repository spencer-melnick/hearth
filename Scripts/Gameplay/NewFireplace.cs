using Godot;
using System;

public class NewFireplace : Fireplace, IInteractive
{
    private Area2D _area;
    private InteractTooltip _tooltip;
    private Globals _globals;
    private BasicCharacter _mainCharacter;
    private Light2D _fireplaceLight;

    public bool Lit = false;

    [Export]
    public float MaxHeatSpeed = 20.0f;

    [Export]
    public float MinHeatSpeed = 5.0f;

    [Export]
    public Gradient FadeGradient;

    private Sprite _veins;
    private Sprite _glow;
    private Sprite _hearth;
    private AnimationPlayer _animationPlayer;


    public override void _Ready()
    {
        _area = GetNode("Area2D") as Area2D;
        _tooltip = GetNode("interact_tooltip") as InteractTooltip;
        _globals = GetNode("/root/Globals") as Globals;
        _fireplaceLight = GetNode("fireplace_light") as Light2D;

        _veins = GetNode("veins") as Sprite;
        _glow = GetNode("glow") as Sprite;
        _hearth = GetNode("hearth") as Sprite;
        _animationPlayer = GetNode("AnimationPlayer") as AnimationPlayer;

        var characters = GetTree().GetNodesInGroup("main_character");

        if (characters != null)
        {
            _mainCharacter = characters[0] as BasicCharacter;
        }   
    }

    public void TryInteract(BasicCharacter character)
    {
        if (character.FuelHeld)
        {
            IFuel fuel = character.HeldObject as IFuel;
            if (fuel != null)
            {

                _globals.NewFireplaceHealth += fuel.GetFuelAmount();

                if (!Lit)
                {
                    _animationPlayer.Play("flash");
                }
            }

            character.DestroyHeldObject();
            _tooltip.FadeOut();
        }
    }

    public void PauseGame()
    {
        _globals.IsGameRunning = false;
    }

    public void UnpauseGame()
    {
        _globals.IsGameRunning = true;
    }

    public Area2D GetInteractiveArea()
    {
        return _area;
    }

    public void LightHearth()
    {
        Lit = true;
    }

    public override void _Process(float delta)
    {
        _tooltip.Enabled = _mainCharacter.FuelHeld;

        if (Lit)
        {
            HeatSpeed = MinHeatSpeed +
                        (_globals.FireplaceHealth - _globals.MinFireplaceHealth)
                        / (100.0f - _globals.MinFireplaceHealth)
                        * (MaxHeatSpeed - MinHeatSpeed);

            float energy = FadeGradient.Interpolate(_globals.NewFireplaceHealth / 100.0f).Gray();

            _veins.SelfModulate = new Color(1.0f, 1.0f, 1.0f, energy);
            _glow.SelfModulate = new Color(1.0f, 1.0f, 1.0f, energy);

            _fireplaceLight.Energy = energy;
        }
    }
}
