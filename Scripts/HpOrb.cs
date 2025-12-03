using Godot;
using System;
using System.ComponentModel.DataAnnotations;

public partial class HpOrb : Node2D
{
	public void OnPickup(Node2D body)
    {
        if(body is Player)
        {
           
            GameManager.Instance.playerStats.health = Math.Min(GameManager.Instance.playerStats.health+10, GameManager.Instance.playerStats.maxHealth);
			CallDeferred("queue_free");
        }
    }
}
