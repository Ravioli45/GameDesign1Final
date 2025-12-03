using Godot;
using System;

public partial class Exp : Node2D
{
	public void OnPickup(Node2D body)
    {
        if(body is Player)
        {
            AudioManager.Instance.PlaySFX("exp_pickup");
            GameManager.Instance.playerStats.exp++;
			CallDeferred("queue_free");
        }
    }
}
