using Godot;
using System;

enum EnemyState
{
	Idle,
	Chase,
}

public partial class MeleeSkeleton : Enemy
{
	private EnemyState State = EnemyState.Idle;
	private Vector2 Direction = new Vector2(0,0);
	public void when_player_enters(Node2D body)
	{
		GD.Print("h");
		if (body is Player p)
		{
			Direction = (p.GlobalPosition - this.GlobalPosition).Normalized();
			State = EnemyState.Chase;
		}
	}
	
	public void when_player_exits(Node2D body)
	{
		GD.Print("g");
        if(body is Player p)
        {
			State = EnemyState.Idle;
        }
    }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if (State == EnemyState.Chase)
		{
			Velocity = speed * Direction;
		}
		else if (State == EnemyState.Idle)
		{
			Velocity = new Vector2(0, 0);
		}

		MoveAndSlide();
    }
}
