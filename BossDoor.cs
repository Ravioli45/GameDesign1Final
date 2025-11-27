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
			Bars.SetDeferred("enabled", false);
        }
    }
	public void OnPlayerEnter(Node2D body)
    {
        if(body is Player p)
        {
			GD.Print("Player Entered");
			
			Bars.SetDeferred("enabled", true);
            BossDetection.SetDeferred("disabled", false);
        }
    }
	
}

