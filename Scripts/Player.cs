using Godot;
using System;

public enum PlayerState
{
    Idle,
    Running,
    Dashing,
    Attacking,
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
    

    protected PlayerState state = PlayerState.Idle;

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        Vector2 direction = Input.GetVector("left", "right", "up", "down");

        // don't do this again if already attacking
        if (Input.IsActionJustPressed("attack") && state != PlayerState.Attacking && state != PlayerState.Dashing)
        {
            Vector2 mouse_pos = GetGlobalMousePosition();
            animator.Set("parameters/Attack/blend_position", (mouse_pos - GlobalPosition).Normalized());
            //GD.Print(mouse_pos);
            attackCountdown = attackFrames;
            //direction = new Vector2(0, 0);
            Velocity = new Vector2(0, 0);
            state = PlayerState.Attacking;
        }

        if (Input.IsActionJustPressed("dash") && state != PlayerState.Dashing && state != PlayerState.Attacking)
        {
            Vector2 mouse_pos = GetGlobalMousePosition();

            // set blend space parameter
            dashCountdown = dashFrames;

            dashDirection = (mouse_pos - GlobalPosition).Normalized();
            animator.Set("parameters/Dash/blend_position", dashDirection);
            animator.Set("parameters/Idle/blend_position", dashDirection);
            animator.Set("parameters/Running/blend_position", dashDirection);

            state = PlayerState.Dashing;
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
                Velocity = direction * speed;

                break;
        }

        MoveAndSlide();
    }

    // OnBodyEnter from weapon hitbox
    public void OnEnemyHit(Node2D body)
    {
        if (body is Entity enemy)
        {
            enemy.TakeDamage(attack, false);
        }
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
}
