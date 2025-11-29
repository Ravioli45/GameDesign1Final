using Godot;
using System;
using System.Diagnostics.Metrics;

public partial class BossDoor : Node2D
{
	[Export] CollisionShape2D BossDetection;
    [Export] CollisionShape2D PlayerDetection;
	[Export] TileMapLayer Bars;

    private int counter = 0;

	public void OnBossAlive(Node2D body)
    {
        if (body.IsInGroup("Boss"))
        {
            GD.Print("Boss Found");
        }
    }

   public void OnBossDies(Node2D body)
    {
            counter++;
            if(!(body is Slimorai) && !(body is FireSlime) && !(body is SwordSlime )||counter == 3){
            GD.Print("Boss Dies");
			Bars.SetDeferred("enabled", false);
            PlayerDetection.SetDeferred("disabled", true);
        }
        
        
    }
	public void OnPlayerEnter(Node2D body)
    {
        if(body is Player p || body is FireSlime)
        {
			GD.Print("Player Entered");
			
			Bars.SetDeferred("enabled", true);
            BossDetection.SetDeferred("disabled", false);
        }
    }
	
}

