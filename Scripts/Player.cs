using Godot;
using System;
using System.ComponentModel.DataAnnotations;

public enum PlayerState
{
    Idle,
    Running,
    Dashing,
    Attacking,
    Ulting,
}

[GlobalClass]
public partial class Player : Entity
{

    [Export] private int attackFrames = 60;
    private int attackCountdown = 0;

    [Export] private int dashFrames = 60;
    private int dashCountdown = 0;
    private Vector2 dashDirection = new(0, 0);
    [Export] private float dashSpeed = 10;
    [Export] private int ultFrames = 60;
    private int ultCountdown = 0;
    private Vector2 ultDirection = new(0, 0);
    [Export] private float ultSpeed = 700;

    [Export] protected PlayerStats stats;
    protected PlayerState state = PlayerState.Idle;

    [Export] private bool enhancedState = false;
    [Export] private Area2D ultHitbox;

    

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        CpuParticles2D Enhancedparticles = GetNode<CpuParticles2D>("Enhanced Form");
        CpuParticles2D particles = GetNode<CpuParticles2D>("Ult Particles");

        Vector2 direction = Input.GetVector("left", "right", "up", "down");

        // don't do this again if already attacking
        if (Input.IsActionJustPressed("attack") && state != PlayerState.Attacking && state != PlayerState.Dashing && state != PlayerState.Ulting)
        {
            Vector2 mouse_pos = GetGlobalMousePosition();
            animator.Set("parameters/Attack/blend_position", (mouse_pos - GlobalPosition).Normalized());
            //GD.Print(mouse_pos);
            attackCountdown = attackFrames;
            //direction = new Vector2(0, 0);
            Velocity = new Vector2(0, 0);
            state = PlayerState.Attacking;
            AudioManager.Instance.PlaySFX("player_attack");
        }

        if (Input.IsActionJustPressed("dash") && state != PlayerState.Dashing && state != PlayerState.Attacking && state != PlayerState.Ulting)
        {
            Vector2 mouse_pos = GetGlobalMousePosition();

            // set blend space parameter
            dashCountdown = dashFrames;

            dashDirection = (mouse_pos - GlobalPosition).Normalized();
            animator.Set("parameters/Dash/blend_position", dashDirection);
            animator.Set("parameters/Idle/blend_position", dashDirection);
            animator.Set("parameters/Running/blend_position", dashDirection);

            state = PlayerState.Dashing;
            AudioManager.Instance.PlaySFX("player_dash");
        }

        if (Input.IsActionJustPressed("meter") && state != PlayerState.Attacking && state != PlayerState.Dashing && state != PlayerState.Ulting)
        {
            if (stats.meterCharge == stats.maxMeter)
            {

                if (enhancedState)
                {

                    Vector2 mouse_pos = GetGlobalMousePosition();

                    // set blend space parameter
                    ultCountdown = ultFrames;

                    ultDirection = (mouse_pos - GlobalPosition).Normalized();
                    animator.Set("parameters/Dash/blend_position", ultDirection);
                    animator.Set("parameters/Idle/blend_position", ultDirection);
                    animator.Set("parameters/Running/blend_position", ultDirection);
                    // animator.Set("parameters/Ulting/blend_position", ultDirection);

                    SetCollisionLayerValue(2, false);
                    SetCollisionMaskValue(3, false);
                    state = PlayerState.Ulting;
                    GD.Print("Ultimate Activated");
                    AudioManager.Instance.PlaySFX("ult_dash");
                }

                else
                {
                    enhancedState = true;
                    stats.meterCharge = 0;
                    GD.Print("Enhanced Form Activated");
                    AudioManager.Instance.PlaySFX("enhance_state");
                    // Turn on particle effects and maybe SFX here

                    Enhancedparticles.Emitting = true;
                    Enhancedparticles.Visible = true;
                    Enhancedparticles.Restart();
                }
            }
        }

        switch (state)
        {
            case PlayerState.Dashing:
                dashCountdown--;

                if (dashCountdown <= 0)
                {
                    Velocity = new Vector2(0, 0);
                    state = PlayerState.Idle;
                }
                else
                {
                    Velocity = dashDirection * dashSpeed;
                }
                break;
            case PlayerState.Attacking:

                attackCountdown--;

                if (attackCountdown <= 0)
                {
                    state = PlayerState.Idle;
                }

                break;

            case PlayerState.Idle:

            case PlayerState.Running:

                if (direction.IsZeroApprox())
                {
                    state = PlayerState.Idle;
                }
                else
                {
                    // set blend space parameters
                    animator.Set("parameters/Idle/blend_position", direction);
                    animator.Set("parameters/Running/blend_position", direction);
                    state = PlayerState.Running;
                }

                // speed from Entity.cs
                Velocity = direction * stats.speed;

                break;
            
            case PlayerState.Ulting:
                // This Ult implementation is a glorified dash for Eugio
                ultCountdown--;
                

                if (ultCountdown <= 0)
                {
                   
                    Velocity = new Vector2(0, 0);
                    SetCollisionLayerValue(2, true);
                    SetCollisionMaskValue(3, true);
                    state = PlayerState.Idle;
                    enhancedState = false;
                    stats.meterCharge = 0;
                    // Turn off particle effects here
                    GD.Print("Ult over");
                    particles.Visible = false;
		            particles.Emitting = false;
                    Enhancedparticles.Visible = false;
		            Enhancedparticles.Emitting = false;
                }
                // Startup lag and endlag
                else if (ultCountdown > 30 || ultCountdown < 10)
                {
                    
		            particles.Emitting = true;
		            particles.Visible = true;
		            particles.Restart();

                    Velocity = new Vector2(0, 0);
                    ultHitbox.Monitoring = false;
                    ultHitbox.Visible = false; // For debug purposes
                }
                else
                {
                    Velocity = ultDirection * ultSpeed;
                    ultHitbox.Monitoring = true;
                    ultHitbox.Visible = true; // For debug purposes
                }
                break;
        }

        MoveAndSlide();
    }

    // OnBodyEnter from weapon hitbox
    public void OnEnemyHit(Node2D body)
    {
        if (body is Entity enemy)
        {
            // enemy.TakeDamage(stats.attack, false);
            // enhancedState's value coincides with the expected value for the Element bool
            enemy.TakeDamage(stats.attack, enhancedState);
            ChargeMeter();
        }
        else if (body is Breakable breakable) breakable.Break();
    }

    public void OnUltHit(Node2D body)
    {
        if (body is Entity enemy)
        {
            // Modify the damage value as needed
            //GD.Print($"Ult hit for {stats.attack*4} Damage");
            enemy.TakeDamage(stats.attack * 2, true);
        }
        else if (body is Breakable breakable) breakable.Break();
    }

    public override void TakeDamage(int base_damage, bool Element)
    {
        //base.TakeDamage(base_damage, Element);
        AudioManager.Instance.PlaySFX("hit");
        stats.health -= base_damage;
        DepleteMeter();
    }

    public void ChargeMeter ()
    {
        stats.meterCharge = Math.Min(stats.meterCharge + 1, stats.maxMeter);
        // Add functionality to interact with HUD if needed, including potential special effects for when meter is full
        GD.Print($"Charge increased to: {stats.meterCharge}");
    }

    public void DepleteMeter()
    {
        stats.meterCharge = Math.Max(stats.meterCharge - 1, 0);
        // Add functionality to interact with HUD if needed
        GD.Print($"Charge decreased to: {stats.meterCharge}");
    }

    public bool IsIdle()
    {
        return state == PlayerState.Idle;
    }

    public bool IsRunning()
    {
        return state == PlayerState.Running;
    }
    public bool IsDashing()
    {
        return state == PlayerState.Dashing;
    }
    public bool IsAttacking()
    {
        return state == PlayerState.Attacking;
    }
    public bool IsUlting()
    {
        return state == PlayerState.Ulting;
    }
}
