using Godot;
using System;

public partial class Gold : Node2D
{
	public void OnPickup(Node2D body)
    {
        if(body is Player)
        {
            GameManager.Instance.playerStats.gold++;
			CallDeferred("queue_free");
        }
    }
}
