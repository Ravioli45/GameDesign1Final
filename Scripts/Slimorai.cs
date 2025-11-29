using Godot;
using System;

public partial class Slimorai : Entity
{
	
	[Export] AnimationTree Animator;
    [Export] PackedScene SlimeSword;
	[Export] PackedScene Fireball;
	[Export] PackedScene Slime1;
	[Export] PackedScene Slime2;

	private int HP = 1;
	private Player PlayerNode;
	private Vector2 Direction = new Vector2(0,0);
	private BossState State = BossState.Idle;
	
	private bool EndAttack = false;
	private bool MeleeAttacking = false;
	private bool MeleeCooldown = false;
	private bool InAttack = false;
	private bool  usedVuln =  false;
	private  int AttackType = 0;
	// Called when the node enters the scene tree for the first time.

	public void OnPlayerEnter(Node2D body)
    {
        if(body is Player p)
        {
			PlayerNode = p;
			State = BossState.Walking;
			
            
        }
    }

	public override void TakeDamage(int base_damage, bool Element)
    {
        
        HP-=base_damage;
    }

	public void ShootFire(Vector2 direction)
    {
		
		CharacterBody2D instance = Fireball.Instantiate<CharacterBody2D>();
        instance.Position = this.Position + new Vector2(0,2);
		Direction = (PlayerNode.GlobalPosition - this.GlobalPosition).Normalized();
		instance.Rotation = Mathf.Atan2(Direction.Y , Direction.X);
		
		instance.Velocity = Direction *200;

		if(instance is Fireball f)
        {
            f.damage = 1;
        }
		GetTree().Root.AddChild(instance);
    }

	public void ShootSwords(Vector2 direction)
    {
		
		CharacterBody2D instance = SlimeSword.Instantiate<CharacterBody2D>();
        instance.Position = this.Position + new Vector2(0,2);
		Direction = (PlayerNode.GlobalPosition - this.GlobalPosition).Normalized();
		instance.Rotation = Mathf.Atan2(Direction.Y , Direction.X);
		
		instance.Velocity = Direction *200;

		if(instance is SlimeSword S)
        {
            S.damage = 1;
        }
		GetTree().Root.AddChild(instance);
    }

	public async void Attack(int  type)
    {	
		InAttack = true;
		if(type == 0){
		ShootSwords(Direction);
        await ToSignal(GetTree().CreateTimer(0.5), "timeout");
		ShootSwords(Direction);
		await ToSignal(GetTree().CreateTimer(0.5), "timeout");
		ShootSwords(Direction);
		await ToSignal(GetTree().CreateTimer(0.5), "timeout");
		ShootSwords(Direction);
		await ToSignal(GetTree().CreateTimer(0.5), "timeout");
		ShootSwords(Direction);
		await ToSignal(GetTree().CreateTimer(4), "timeout");
		AttackType = 1;
		}else if(type == 1)
        {
        await ToSignal(GetTree().CreateTimer(1), "timeout");
		ShootFire(Direction);
        await ToSignal(GetTree().CreateTimer(0.25), "timeout");
		ShootFire(Direction);
		await ToSignal(GetTree().CreateTimer(0.25), "timeout");
		ShootFire(Direction);
		await ToSignal(GetTree().CreateTimer(0.25), "timeout");
		ShootFire(Direction);
		await ToSignal(GetTree().CreateTimer(0.25), "timeout");
		ShootFire(Direction);
		await ToSignal(GetTree().CreateTimer(0.25), "timeout");
		ShootFire(Direction);
		await ToSignal(GetTree().CreateTimer(0.25), "timeout");
		ShootFire(Direction);
		await ToSignal(GetTree().CreateTimer(0.25), "timeout");
		ShootFire(Direction);
		await ToSignal(GetTree().CreateTimer(0.25), "timeout");
		ShootFire(Direction);
		await ToSignal(GetTree().CreateTimer(2), "timeout");
		AttackType = 0;
        }
		EndAttack = true;
		InAttack = false;
		
    }
	public async void Vulnerable()
    {
		
		usedVuln = true;
		
		await ToSignal(GetTree().CreateTimer(2), "timeout");
		
		EndAttack = true;
		
		
		
		
    }
	public async void CollideHit(Player target)
    {
        MeleeAttacking = true;
		InAttack = true;
        target.TakeDamage(1, false);
        // Knockback stuff could go here
        // Here we prevent multi-hit by making the Attack CD 1 second
        MeleeCooldown = true;
        await ToSignal(GetTree().CreateTimer(1), "timeout");
		MeleeCooldown  = false;
		
		MeleeAttacking = false;
    }

	public async void Die()
    {
		State = BossState.Die;
		await ToSignal(GetTree().CreateTimer(1.2), "timeout");
		CharacterBody2D instance1 = Slime1.Instantiate<CharacterBody2D>();
        instance1.Position = this.Position + new Vector2(-8,0);

		CharacterBody2D instance2 = Slime2.Instantiate<CharacterBody2D>();
        instance2.Position = this.Position + new Vector2(8,0);
        
		GetTree().Root.AddChild(instance1);
		GetTree().Root.AddChild(instance2);
		this.CallDeferred("queue_free");
    }
	public override void _Ready()
    {
        
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
    {
        if (HP <= 0)
        {
            Die();
        }
        if(State == BossState.Walking)
        {
			if (!MeleeAttacking && !MeleeCooldown) {
			Direction = (PlayerNode.GlobalPosition - this.GlobalPosition).Normalized();
			Animator.Set("parameters/Chasing/blend_position", Direction);
			Velocity = Direction*50;
			MoveAndSlide();
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
            if (!MeleeCooldown && !MeleeAttacking && EndAttack)
            {
				InAttack = false;
				EndAttack = false;
				usedVuln = false;
            	State = BossState.Attacking;
            }
           	else if (!usedVuln)
            {
			Vulnerable();
            }

        }else if(State == BossState.Attacking)
        {
			
			if(AttackType == 0)
            {
                State = BossState.SwordAttacking;
            }else if(AttackType == 1)
            {
                State = BossState.FireAttacking;
            }
			
        }else if(State == BossState.SwordAttacking)
		{
			GD.Print("SWORD");
			Direction = (PlayerNode.GlobalPosition - this.GlobalPosition).Normalized();
			if(EndAttack)
            {
				EndAttack = false;
                State = BossState.Walking;
            }else if(!InAttack){
			Attack(AttackType);
			}
		
        
            
        }else if(State == BossState.FireAttacking)
        {
           GD.Print("FIRRE");
			Direction = (PlayerNode.GlobalPosition - this.GlobalPosition).Normalized();
			if(EndAttack)
            {
				EndAttack = false;
                State = BossState.Walking;
            }else if(!InAttack){
			Attack(AttackType);
			}
        }
    }

	public bool IsWalking()
    {
        return State == BossState.Walking;
    }

	public bool IsSwordAttacking()
    {
        return State == BossState.Attacking;
    }

	public bool IsFireAttacking()
    {
        return State == BossState.Attacking;
    }

	public bool IsDie()
    {
        return State == BossState.Die;
    }
}
