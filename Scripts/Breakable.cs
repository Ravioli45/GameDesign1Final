using Godot;
using System;

public partial class Breakable : StaticBody2D
{
	[Export] PackedScene gold;
    [Export] PackedScene healthOrb;
    [Export] private int minGold = 0;
    [Export] private int maxGold = 5;
    [Export] private int minHealthOrb = 0;
    [Export] private int maxHealthOrb = 1;

    public void Break()
    {
        int amount_gold_drop = GD.RandRange(minGold, maxGold);
        int amount_health_orb_drop = GD.RandRange(minHealthOrb, maxHealthOrb);
        for (int i = 0; i < amount_gold_drop; i++)
        {
            Node2D Instance = gold.Instantiate<Node2D>();
            Instance.Position = new Vector2(this.Position.X + GD.RandRange(-5, 5), this.Position.Y + GD.RandRange(-5, 5));
            //this.GetTree().Root.CallDeferred("add_child",Instance);
            GetParent().CallDeferred("add_child", Instance);
		}
        for (int i = 0; i < amount_health_orb_drop; i++)
        {
            Node2D Instance = healthOrb.Instantiate<Node2D>();
            Instance.Position = new Vector2(this.Position.X + GD.RandRange(-5, 5), this.Position.Y + GD.RandRange(-5, 5));
            //this.GetTree().Root.CallDeferred("add_child",Instance);
            GetParent().CallDeferred("add_child", Instance);
		}
       
        AudioManager.Instance.PlaySFX("box_break");
        CallDeferred("queue_free");
    }
}
