using Godot;
using System;

public partial class BossDoor : Node2D
{
	[Export] CollisionShape2D BossDetection;
	[Export] TileMapLayer Bars;

	public void OnBossAlive(Node2D body)
    {
        if (body.IsInGroup("Boss"))
        {
            GD.Print("Boss Found");
        }
    }

   public void OnBossDies(Node2D body)
    {
        if (body.IsInGroup("Boss"))
        {
            GD.Print("Boss Dies");
			Bars.Enabled = false;
        }
    }
	public void OnPlayerEnter(Node2D body)
    {
        if(body is Player p)
        {
			Bars.Enabled = true;
            BossDetection.Disabled = false;
        }
    }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
