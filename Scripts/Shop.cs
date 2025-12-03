using Godot;
using System;
using System.Collections;

public partial class Shop : Node2D
{
	[Export] Godot.Collections.Array<ItemStats> items;

	[Export] Item item1;
	[Export] Item item2;
	[Export] Item item3;

	[Export] AnimatedSprite2D Tinglorb;

	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {
		item1.UpdateStats(items[GD.RandRange(0,4)]);
		item2.UpdateStats(items[GD.RandRange(0,4)]);
		item3.UpdateStats(items[GD.RandRange(0,4)]);

		Tinglorb.Play();
        
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
