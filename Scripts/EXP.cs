using Godot;
using System;

public partial class EXP : Node2D
{
	public void OnPickup(Node2D body)
    {
        if(body is Player )
        {
            GameManager.Instance.playerStats.exp += 1;
			CallDeferred("queue_free");
			
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
