using Godot;
using System;

public partial class DamageNumbers : RichTextLabel
{

    public bool elementAttack = false;
    public int lifetime = 25;
    public override void _Ready()
    {
        Scale = new Vector2(0.5f, 0.5f);
        GlobalPosition += new Vector2(GD.RandRange(-5, 5) + 5,GD.RandRange(-5, 5) - 28);
        if (elementAttack)
        {
            Text = "[color=yellow][outline_color=deep_sky_blue]" + Text;
            AddThemeFontSizeOverride("normal_font_size", 32);
        }
    }
    public override void _PhysicsProcess(double delta)
    {
        if (delta > 0)
        {
            lifetime--;
            if (lifetime >= 20) Scale += new Vector2(0.1f, 0.1f);
            GlobalPosition += new Vector2(0, -0.5f);
            if (lifetime <= 0) QueueFree();
        }
    }


}
