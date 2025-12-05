using Godot;
using System;

public partial class WizardSkeleton : Enemy
{	

	[Export] PackedScene Fireball;

	private EnemyState State = EnemyState.Idle;
	private Vector2 Direction = new Vector2(0,0);
    private Node2D PlayerNode;
    private bool isTeleporting = false;
    private bool inTeleportCD = false;
    private bool inAttackCD = false;
	
	
	public void OnPlayerEntered(Node2D body)
	{
		if (body is Player p)
		{
			//GD.Print("Entered The Spell Circle");
            PlayerNode = p;
			Direction = (p.GlobalPosition - this.GlobalPosition).Normalized();
			State = EnemyState.Attack;
		}
	}

	public void OnPlayerExited(Node2D body)
	{
        if(body is Player p)
        {
			State = EnemyState.Idle;
        }
    }

	public async void Teleport()
    {
		
		CpuParticles2D particles = GetNode<CpuParticles2D>("Teleportparticles");
		particles.Emitting = true;
		particles.Visible = true;
		particles.Restart();
        isTeleporting = true;
        await ToSignal(GetTree().CreateTimer(2f), "timeout");
        isTeleporting = false;
		Velocity = new Vector2(GD.RandRange(-1,1), GD.RandRange(-1,1)) * 10000;
		MoveAndSlide();
		particles.Visible = false;
		particles.Emitting = false;
		Velocity = new Vector2(0,0);
		MoveAndSlide();
        inTeleportCD = true;
		if(State == EnemyState.Teleport){
			
			State = EnemyState.Attack;
		}
        await ToSignal(GetTree().CreateTimer(5f), "timeout");
        inTeleportCD = false;
		
		
    }

	public async void FIREBALL(Player target)
	{

		inAttackCD = true;
		await ToSignal(GetTree().CreateTimer(1), "timeout");
		CpuParticles2D particles = GetNode<CpuParticles2D>("Attack Particles");
		particles.Emitting = true;
		particles.Visible = true;
		particles.Restart();
		await ToSignal(GetTree().CreateTimer(2), "timeout");
		particles.Visible = false;
		particles.Emitting = false;
		inAttackCD = false;

		//GD.Print("ITS WIZARD TIME MOTHERFUCKERS");
		// CREATE FIRE BALL AND SET ITS VELOCITY = 200 * target.Direction
		CharacterBody2D instance = Fireball.Instantiate<CharacterBody2D>();
		instance.Position = this.Position;
		Direction = (PlayerNode.GlobalPosition - this.GlobalPosition).Normalized();
		instance.Rotation = Mathf.Atan2(Direction.Y, Direction.X);

		instance.Velocity = Direction * 200;

		if (instance is Fireball f)
		{
			f.damage = stats.attack;
		}




		//GetTree().Root.AddChild(instance);
		GetParent().AddChild(instance);
		
    }

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		
		
       
		if(State == EnemyState.Attack)
        {
			
          
		  
			//Will Teleport Randomly while attacking the player
            if (!isTeleporting && !inTeleportCD && !inAttackCD) {
                State = EnemyState.Teleport;
                Teleport();
				
			}

            if (!inAttackCD && !isTeleporting)
            {
				
                
				FIREBALL((Player)PlayerNode);
            }
        	else Velocity = new Vector2(0,0);
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

	public bool IsAttacking()
    {
        return State == EnemyState.Attack;
    }
	public bool IsIdle()
    {
        return State == EnemyState.Idle;
    }
	public bool IsTeleporting()
    {
        return State == EnemyState.Teleport;
    }
}
