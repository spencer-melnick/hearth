using Godot;
using System;

public class FirstFireplace : Fireplace, IInteractive
{
    private Area2D _area;
    private InteractTooltip _tooltip;
    private Globals _globals;
    private BasicCharacter _mainCharacter;
    private Light2D _fireplaceLight;

    [Export]
    public float MaxHeatSpeed = 20.0f;

    [Export]
    public float MinHeatSpeed = 5.0f;

    [Export]
    public Gradient FadeGradient;


    public override void _Ready()
    {
        _area = GetNode("Area2D") as Area2D;
        _tooltip = GetNode("interact_tooltip") as InteractTooltip;
        _globals = GetNode("/root/Globals") as Globals;
        _fireplaceLight = GetNode("fireplace_light") as Light2D;

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
                _globals.FireplaceHealth += fuel.GetFuelAmount();
            }

            character.DestroyHeldObject();
            _tooltip.FadeOut();
        }
    }

    public Area2D GetInteractiveArea()
    {
        return _area;
    }

    public override void _Process(float delta)
    {
        _tooltip.Enabled = _mainCharacter.FuelHeld;

        if (_mainCharacter.FuelHeld)
        {
            _tooltip.FadeIn();
        }

        HeatSpeed = MinHeatSpeed + (_globals.FireplaceHealth / 100.0f) *
                    (MaxHeatSpeed - MinHeatSpeed);

        float energy = FadeGradient.Interpolate(_globals.FireplaceHealth / 100.0f).Gray();

        Sprite veins = GetNode("veins") as Sprite;
        veins.SelfModulate = new Color(1.0f, 1.0f, 1.0f, energy);

        Sprite glow = GetNode("glow") as Sprite;
        glow.SelfModulate = new Color(1.0f, 1.0f, 1.0f, energy);

        _fireplaceLight.Energy = energy;
    }
}
