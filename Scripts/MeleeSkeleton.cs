using Godot;
using System;
using System.Runtime.CompilerServices;

enum EnemyState
{
	Idle,
	Chase,
	Attack,
}

public partial class MeleeSkeleton : Enemy
{
	//General Enemy Variables
	private EnemyState State = EnemyState.Idle;
	private Vector2 direction = new Vector2(0,0);

	//Attacking Variables
	private int to_hit_lag = 0;
	private int hit_timer = 0;
	bool has_attacked = false;

	bool player_got_away;

	Entity player;
	public void when_player_enters(Node2D body)
	{
		player = (Entity)body;
		
		if (body is Player p)
		{
			
			State = EnemyState.Chase;
			
			

		}
	}
	
	public void on_attack_enter(Node2D body)
    {
		GD.Print("ENter");
		player_got_away = false;
	
        if(body is Player p && !has_attacked)
        {
			to_hit_lag = 30;
            State = EnemyState.Attack;
        }
    }

	public void on_attack_exit(Node2D body)
    {
		GD.Print("EXIT");
	  player_got_away = true;
    }
	public void when_player_exits(Node2D body)
	{
		
        if(body is Player p)
        {
			State = EnemyState.Idle;
			GD.Print(State);
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
			direction = (player.GlobalPosition - this.GlobalPosition).Normalized();
			Velocity = speed * direction;
			animator.Set("parameters/Walk/blend_position", direction);
			has_attacked = false;
			
			
		}
		else if (State == EnemyState.Idle)
		{
			Velocity = new Vector2(0, 0);
            if (hit_timer > 0)
            {
                State = EnemyState.Attack;
            }
			
		}
		else if (State == EnemyState.Attack)
        {
			to_hit_lag--;
			
            Velocity = new Vector2(0,0);
			direction = (player.GlobalPosition - this.GlobalPosition).Normalized();
			animator.Set("parameters/Attack/blend_position", direction);

			if(to_hit_lag <= 0 && !player_got_away && !has_attacked)
            {
				//Swing and Hits
				GD.Print("Swing and Hit");
				player.TakeDamage(attack,false);
				hit_timer = 30;
				has_attacked = true;
                //Proc Damage and add hitlag has_attacked = true
            }else if(to_hit_lag <=0 && player_got_away && !has_attacked){
                //Swing and misses
				GD.Print("Swing and Miss");
				hit_timer = 30;
				has_attacked = true;
				
            }

			hit_timer --;
			if(hit_timer<=0 && has_attacked && player_got_away)
            {
				GD.Print("CHASE HE GOT AWAY");
				//Default Chase Player after missing attack
                State = EnemyState.Chase;
            }else if(hit_timer<=10 && to_hit_lag<=0 && !player_got_away)
            {
				GD.Print("ATTACK AGAIN");
				//Edge Case for when the player re enters during hit lag
                to_hit_lag = 30;
				has_attacked = false;
				player_got_away = false;
				State = EnemyState.Idle;
				
            }

        }

		
            
        
        
		

		MoveAndSlide();
    }

	public bool IsIdle()
    {
        return State == EnemyState.Idle;
    }

    public bool IsChasing()
    {
        return State == EnemyState.Chase;
    }

	public bool IsAttacking()
    {
        return State == EnemyState.Attack;
    }
}
