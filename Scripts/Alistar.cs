using Godot;
using System;
using System.Data;

enum BossState
{
    Idle,
	Projectile,
	Walking,
	Attacking,
	Charging,
	Tackle,
}
public partial class Alistar : CharacterBody2D
{
    [Export] AnimationTree Animator;
    [Export] PackedScene Fireball;

	private Player PlayerNode;
	private Vector2 Direction = new Vector2(0,0);
    private Vector2 StaticDirection = new Vector2(0,0);
    bool UnlockDirection = true;
	private int AttackSelection = -1;
	private BossState State = BossState.Idle;
    private bool InAttack = false;
    private int Damage = 1;
    private int ChargeDamage = 2;
    public bool fightStarted = false;

    private bool InMeleeRange = false;
    private bool MeleeAttacking = false;

    private bool MeleeCooldown = false;

    private bool endAttack = false;
   

	public void PlayerEntersFight(Node2D body)
    {
       
        if(body is Player p){
        CollisionShape2D BossPlayerDetection = GetNode<CollisionShape2D>("PlayerDetectionHandeler/PlayerDetection");
        BossPlayerDetection.SetDeferred("disabled",false);
        
        fightStarted = true;
        
        }
    }

	public void OnDetectionEnter(Node2D body)
    {
		if (body is Player p)
		{
		
            PlayerNode = p;
			Direction = (p.GlobalPosition - this.GlobalPosition).Normalized();

			
		}
        
    }

    public void PlayerEntersMelee(Node2D body)
    {
        if(body is Player p){
        InMeleeRange = true;
        }
    }

    public void PlayerExitsMelee(Node2D body)
    {
        if(body is Player p){
        InMeleeRange = false;
        }
    }
	//Vulnerable represents the states the boss can be in seperated by Idle or vulnerable phases
	public async void Vulnerable()
    {
        

		//Vulnerability frames for player to attack
		await ToSignal(GetTree().CreateTimer(2f), "timeout");
        if(AttackSelection == 0)
        {
			//Begins his Charge tackle attack
            
        	State = BossState.Charging;
            //Stalls for the Kicking Animation
            await ToSignal(GetTree().CreateTimer(1.5f), "timeout");
            //Calls 
            State = BossState.Tackle;
			
        } else if(AttackSelection == 1)
        {
            
			//Begins his walking/melee attack cycle
			State = BossState.Walking;
            MeleeCooldown = true;
            await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
            MeleeCooldown = false;
            await ToSignal(GetTree().CreateTimer(5f), "timeout");
            
            endAttack = true;
            
           
            
        } else if(AttackSelection == 2)
        {
            //Begins his tantrum attack
            State = BossState.Projectile;

            //Handles the dirt particles
            CpuParticles2D particles = GetNode<CpuParticles2D>("GroundPound");
		    particles.Emitting = true;
		    particles.Visible = true;
		    particles.Restart();

            //Shoots 5 Projectiles with 0.5 seconds in between each
            for(int i = 0; i < 5 ; i++){
            await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
            Direction = (PlayerNode.GlobalPosition - this.GlobalPosition).Normalized();
            StaticDirection = Direction;
            ProjectileAttack(StaticDirection);
            }

            //Makes particles dissapear
            particles.Visible = false;
		    particles.Emitting = false;

            State = BossState.Idle;
            InAttack = false;

            
        }
        //Resets back to first type of attack
        if(AttackSelection == 2)
        {
            AttackSelection = -1;
        }
    }

	public async void Tackle(Vector2 StaticDirection)
    {
            //Charges in a set direction, stops when hits player or anything else
            Velocity = 300 * StaticDirection;
            MoveAndSlide();
           for (int i = 0; i < GetSlideCollisionCount(); i++)
            {
                var collision = GetSlideCollision(i);
                if (collision.GetCollider() is Player p)
                {
                    
                    p.TakeDamage(ChargeDamage,false);
                    InAttack = false;
					State = BossState.Idle;
                    
                    break;
                }
            }

        	if (GetSlideCollisionCount()>0)
        	{
                
            	State = BossState.Idle;
                 InAttack = false;
        	}
			
    }

    public async void MeleeAttack()
    {
        
        //Not Functional Await functions bug out the timer in the vulnerable function
            State = BossState.Attacking;
            MeleeAttacking = true;
           
            //Stall to hit ground first
            await ToSignal(GetTree().CreateTimer(1f), "timeout");
            MeleeAttacking = false;

            MeleeCooldown = true;
            
            await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
            State = BossState.Walking;
            await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
            MeleeCooldown = false;
            
        
    }

    public async void ProjectileAttack(Vector2 StaticDirection)
    {
        //Spawns Projectiles
        
        CharacterBody2D instance = Fireball.Instantiate<CharacterBody2D>();
        instance.Position = this.Position;
        instance.Rotation = Mathf.Atan2(StaticDirection.Y , StaticDirection.X);
		instance.Velocity = StaticDirection *200;
        GetTree().Root.AddChild(instance);
    }


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        
        
        if(fightStarted){

		if(State == BossState.Idle)
        {
            //Every Action Starts and ends in IDLE
            //Each State needs to begin with InAttack = True to not go back to idle
            if(!InAttack ){
            InAttack = true;
            AttackSelection++;
            endAttack = false;
            Vulnerable();
            }
        }

		else if (State == BossState.Charging)
        {
            //Checks for player positions feeds into tackle
            Direction = (PlayerNode.GlobalPosition - this.GlobalPosition).Normalized();
            StaticDirection = Direction;
            Animator.Set("parameters/Tackle/blend_position", StaticDirection);
            Animator.Set("parameters/Charging/blend_position", StaticDirection);
        }

		else if(State == BossState.Tackle)
        {
            
            Tackle(StaticDirection);
        }

        else if (State == BossState.Walking)
        {
            
            
            if(!MeleeCooldown && !MeleeAttacking && endAttack )
                {
                 
                     InAttack = false;
                    State = BossState.Idle;
                }

            if (!MeleeAttacking && !MeleeCooldown) {
                Direction = (PlayerNode.GlobalPosition - this.GlobalPosition).Normalized();
                Animator.Set("parameters/Chasing/blend_position", Direction);
                Velocity = 75 * Direction;
                if(InMeleeRange){
                MeleeAttack();
                }
                MoveAndSlide();
            }
            

            
             
        }

        else if(State == BossState.Projectile)
        {
            
        }

        else if(State == BossState.Attacking)
        {
               
        }

        }
	}




	public bool IsIdle()
    {
        return State == BossState.Idle;
    }
	public bool IsProjectile()
    {
        return State == BossState.Projectile;
    }
	public bool IsWalking()
    {
        return State == BossState.Walking;
    }
	public bool IsAttacking()
    {
        return State == BossState.Attacking;
    }
	public bool IsCharging()
    {
        return State == BossState.Charging;
    }
	public bool IsTackle()
    {
        return State == BossState.Tackle;
    }

}
