using Godot;
using System;

public partial class Fireball : CharacterBody2D
{
	[Export] AnimatedSprite2D sprite;
	public int damage;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {
		sprite.Play("default");
        AudioManager.Instance.PlaySFX("fireball_spawn");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();
               for (int i = 0; i < GetSlideCollisionCount(); i++)
            {
                var collision = GetSlideCollision(i);
                if (collision.GetCollider() is Player p)
                {
                    AudioManager.Instance.PlaySFX("fireball_die");
                    p.TakeDamage(damage,false, new Vector2(0,0));
					this.CallDeferred("queue_free");
                    break;
                }
            }

        	if (GetSlideCollisionCount()>0)
        	{
                AudioManager.Instance.PlaySFX("fireball_die");
            	this.CallDeferred("queue_free");
        	}
			
			
    }
}
