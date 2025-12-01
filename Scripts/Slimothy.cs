using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Slimothy : Enemy
{
    private EnemyState State = EnemyState.Idle;
	private Vector2 Direction = new Vector2(0,0);
    private Node2D PlayerNode;
    private bool isLunging = false;
    private bool inLungeCD = false;
    private bool inAttackCD = false;
	public void OnPlayerEntered(Node2D body)
	{
		if (body is Player p)
		{
            PlayerNode = p;
			Direction = (p.GlobalPosition - this.GlobalPosition).Normalized();
			State = EnemyState.Chase;
		}
	}
	
	public void OnPlayerExited(Node2D body)
	{
        if(body is Player p)
        {
			State = EnemyState.Idle;
        }
    }

    public async void Lunge()
    {
        isLunging = true;
        await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
        isLunging = false;
        inLungeCD = true;
        await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
        inLungeCD = false;
    }

    public async void CollideHit(Player target)
    {
        GD.Print("ATTACK");
        target.TakeDamage(stats.attack, false);
        // Knockback stuff could go here
        // Here we prevent multi-hit by making the Attack CD 1 second
        inAttackCD = true;
        await ToSignal(GetTree().CreateTimer(1), "timeout");
        inAttackCD = false;
    }

    public override void _Ready()
    {
        base._Ready();
	}

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (State == EnemyState.Chase)
        {
            // Movement is based on periodic straight lunges
            if (!isLunging && !inLungeCD) {
                Direction = (PlayerNode.GlobalPosition - this.GlobalPosition).Normalized();
                animator.Set("parameters/Chasing/blend_position", Direction);
                Lunge();
            }
            else if (!inLungeCD) Velocity = stats.speed * Direction;
            else Velocity = new Vector2(0,0);
        }
        else if (State == EnemyState.Idle)
		{
			Velocity = new Vector2(0, 0);
		}
        MoveAndSlide();
        // Check collisions in order to have collision-based attacks
        if (!inAttackCD && isLunging)
        {
            for (int i = 0; i < GetSlideCollisionCount(); i++)
            {
                var collision = GetSlideCollision(i);
                if (collision.GetCollider() is Player p)
                {
                    CollideHit(p);
                    break;
                }
            }
        }
        if (is_element_applied) elementGauge = Math.Max(elementGauge-1, 0);
		if (elementGauge <= 0) {
			is_element_applied = false;
			//GD.Print(is_element_applied);
		}
		//else if(elementGauge % 10 == 0) GD.Print($"{elementGauge}, {is_element_applied}");
    }

    private bool IsChasing()
    {
        return State == EnemyState.Chase;
    }
    private bool IsIdle()
    {
        return State == EnemyState.Idle;
    }


}
