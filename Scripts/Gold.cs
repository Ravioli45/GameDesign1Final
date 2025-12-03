using Godot;
using System;

public partial class Gold : Node2D
{
	public void OnPickup(Node2D body)
    {
        if(body is Player)
        {
            AudioManager.Instance.PlaySFX("coin_pickup");
            GameManager.Instance.playerStats.gold++;
			CallDeferred("queue_free");
        }
    }
}
