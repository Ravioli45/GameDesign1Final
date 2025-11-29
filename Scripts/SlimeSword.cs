using Godot;
using System;


public partial class SlimeSword : CharacterBody2D
{
	[Export] Sprite2D sprite;
	public int damage;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
    {
		RotationDegrees+=10;
        MoveAndSlide();
               for (int i = 0; i < GetSlideCollisionCount(); i++)
            {
                var collision = GetSlideCollision(i);
                if (collision.GetCollider() is Player p)
                {
                    p.TakeDamage(damage,false);
					this.CallDeferred("queue_free");
                    break;
                }
            }

        	if (GetSlideCollisionCount()>0)
        	{
            	this.CallDeferred("queue_free");
        	}
    }
}
