using Godot;
using System;
using System.Data;
using System.Diagnostics.Metrics;

public partial class FireSlime : Entity
{

	[Export] AnimationTree Animator;
    [Export] PackedScene Fireball;
	[Export] PackedScene damageNumbers;

	private int HP = 75;
	private Player PlayerNode;
	private Vector2 Direction = new Vector2(0,0);
	private BossState State = BossState.Idle;
	
	private bool EndAttack = false;
	private bool MeleeAttacking = false;
	private bool MeleeCooldown = false;
	private bool InAttack = false;
	private bool  usedVuln =  false;

	// Called when the node enters the scene tree for the first time.
	public bool is_element_applied = false;
    public int elementGauge = 0;
	public void OnPlayerEnter(Node2D body)
    {
        if(body is Player p)
        {
			PlayerNode = p;
			State = BossState.Walking;
			
            
        }
    }

	public override void TakeDamage(int base_damage, bool Element, Vector2 directionHit)
	{
		DamageNumbers Instance = damageNumbers.Instantiate<DamageNumbers>();
		Instance.GlobalPosition = this.GlobalPosition + new Vector2(10, -10);
		Instance.elementAttack = Element;
		if (Element && is_element_applied)
		{
			base_damage *= 2;
			elementGauge = Math.Min(elementGauge + 500, 1000);
			Modulate = new Color(1, 1, (float)(0.2 * Math.Round(Math.Cos(elementGauge / 10)) + 0.5));
		}
		else if (Element && !is_element_applied)
		{
			this.is_element_applied = true;
			elementGauge = Math.Min(elementGauge + 500, 1000);
			Modulate = new Color(1, 1, (float)(0.2 * Math.Round(Math.Cos(elementGauge / 10)) + 0.5));
		}

		HP -= base_damage;

		if (HP <= 0)
		{
			//GD.Print("Slimorai died: " + base_damage);
			//Die();
			AudioManager.Instance.PlaySFX("enemy_die");
		}
		else
		{
			if (Element)
			{
				AudioManager.Instance.PlaySFX("elemental_hit");
			}
			else
			{
				AudioManager.Instance.PlaySFX("hit");
			}
		}
		this.Velocity = 150 * directionHit * -1;
		MoveAndSlide();
		Instance.Text = " " + base_damage.ToString();
		//GetTree().Root.AddChild(Instance);
		GetParent().AddChild(Instance);
    }

	public void ShootFire(Vector2 direction)
	{

		CharacterBody2D instance = Fireball.Instantiate<CharacterBody2D>();
		instance.Position = this.Position + new Vector2(0, 2);
		Direction = (PlayerNode.GlobalPosition - this.GlobalPosition).Normalized();
		instance.Rotation = Mathf.Atan2(Direction.Y, Direction.X);

		instance.Velocity = Direction * 200;

		if (instance is Fireball f)
		{
			f.damage = 16;
		}
		//GetTree().Root.AddChild(instance);
		GetParent().AddChild(instance);
    }

	public async void Attack(Vector2 Direction)
    {	
		InAttack = true;
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
		EndAttack = true;
		InAttack = false;
		
    }
	public async void Vulnerable()
    {
		
		usedVuln = true;
		
		await ToSignal(GetTree().CreateTimer(2), "timeout");
		
		EndAttack = true;
		
    }

	public async void Die()
    {	
		State = BossState.Die;
        await ToSignal(GetTree().CreateTimer(1.5), "timeout");
		this.CallDeferred("queue_free");
    }

	public async void CollideHit(Player target)
    {
        MeleeAttacking = true;
		InAttack = true;
        target.TakeDamage(1, false, Direction);
        // Knockback stuff could go here
        // Here we prevent multi-hit by making the Attack CD 1 second
        MeleeCooldown = true;
        await ToSignal(GetTree().CreateTimer(1), "timeout");
		MeleeCooldown  = false;
		
		MeleeAttacking = false;
    }
	public override void _Ready()
    {
        
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
    {
		if(HP <= 0)
        {
            Die();
        }
		
        if(State == BossState.Walking)
        {
			if (!MeleeAttacking && !MeleeCooldown) {
			Direction = (PlayerNode.GlobalPosition - this.GlobalPosition).Normalized()*-1;
			Animator.Set("parameters/Chasing/blend_position", Direction);
			Velocity = Direction*25;
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
			//GD.Print("ATTACK");
			
			if(EndAttack)
            {
				EndAttack = false;
                State = BossState.Walking;
            }else if(!InAttack){
			Attack(Direction);
			}
        }
		if (is_element_applied) {
			elementGauge = Math.Max(elementGauge-1, 0);
			Modulate = new Color(1, 1, (float)(0.2 * Math.Round(Math.Cos(elementGauge/10)) + 0.5));
			if (elementGauge <= 0) {
				is_element_applied = false;
				Modulate = new Color(1, 1, 1);
			}
		}
    }

	public bool IsWalking()
    {
        return State == BossState.Walking;
    }

	public bool IsAttacking()
    {
        return State == BossState.Attacking;
    }

	public bool IsDie()
    {
        return State == BossState.Die;
    }
}

