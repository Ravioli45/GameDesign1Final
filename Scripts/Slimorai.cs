using Godot;
using System;

public partial class Slimorai : Entity
{
	
	[Export] AnimationTree Animator;
    [Export] PackedScene SlimeSword;
	[Export] PackedScene Fireball;
	[Export] PackedScene Slime1;
	[Export] PackedScene Slime2;
	[Export] PackedScene damageNumbers;

	[Export] private int HP = 75;
	private Player PlayerNode;
	private Vector2 Direction = new Vector2(0,0);
	private BossState State = BossState.Idle;
	
	private bool EndAttack = false;
	private bool MeleeAttacking = false;
	private bool MeleeCooldown = false;
	private bool InAttack = false;
	private bool  usedVuln =  false;
	private  int AttackType = 0;
	public bool fightStarted = false;
	// Called when the node enters the scene tree for the first time.
	public bool is_element_applied = false;
    public int elementGauge = 0;

	public void OnPlayerEnter(Node2D body)
	{
		if (body is Player p)
		{
			PlayerNode = p;
			State = BossState.Walking;


		}
	}
	public void PlayerEntersFight(Node2D body)
    {
       
        if(body is Player p){
		//GD.Print("Ben");
        CollisionShape2D BossPlayerDetection = GetNode<CollisionShape2D>("PlayerFinder/PlayerDetection");
        BossPlayerDetection.SetDeferred("disabled",false);
        
		//if (!fightStarted) AudioManager.Instance.PlayBGM($"boss{level}_{GameManager.Instance.musicType}");
        fightStarted = true;
        
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

	public void ShootSwords(Vector2 direction)
	{

		CharacterBody2D instance = SlimeSword.Instantiate<CharacterBody2D>();
		instance.Position = this.Position + new Vector2(0, 2);
		Direction = (PlayerNode.GlobalPosition - this.GlobalPosition).Normalized();
		instance.Rotation = Mathf.Atan2(Direction.Y, Direction.X);

		instance.Velocity = Direction * 200;

		if (instance is SlimeSword S)
		{
			S.damage = 16;
		}
		//GetTree().Root.AddChild(instance);
		GetParent().AddChild(instance);
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
        target.TakeDamage(1, false, Direction);
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

		//GetTree().Root.AddChild(instance1);
		//GetTree().Root.AddChild(instance2);
		GetParent().AddChild(instance1);
		GetParent().AddChild(instance2);
		this.CallDeferred("queue_free");
    }
	public override void _Ready()
    {
        
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(fightStarted){
		if (HP <= 0)
		{
			Die();
		}
		if (State == BossState.Walking)
		{
			if (!MeleeAttacking && !MeleeCooldown)
			{
				Direction = (PlayerNode.GlobalPosition - this.GlobalPosition).Normalized();
				Animator.Set("parameters/Chasing/blend_position", Direction);
				Velocity = Direction * 50;
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

		}
		else if (State == BossState.Attacking)
		{

			if (AttackType == 0)
			{
				State = BossState.SwordAttacking;
			}
			else if (AttackType == 1)
			{
				State = BossState.FireAttacking;
			}

		}
		else if (State == BossState.SwordAttacking)
		{
			//GD.Print("SWORD");
			Direction = (PlayerNode.GlobalPosition - this.GlobalPosition).Normalized();
			if (EndAttack)
			{
				EndAttack = false;
				State = BossState.Walking;
			}
			else if (!InAttack)
			{
				Attack(AttackType);
			}



		}
		else if (State == BossState.FireAttacking)
		{
			//GD.Print("FIRRE");
			Direction = (PlayerNode.GlobalPosition - this.GlobalPosition).Normalized();
			if (EndAttack)
			{
				EndAttack = false;
				State = BossState.Walking;
			}
			else if (!InAttack)
			{
				Attack(AttackType);
			}
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

	public bool IsSwordAttacking()
    {
        return State == BossState.SwordAttacking;
    }

	public bool IsFireAttacking()
    {
        return State == BossState.FireAttacking;
    }

	public bool IsDie()
    {
        return State == BossState.Die;
    }
}
