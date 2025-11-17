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
public abstract partial class Player : Entity
{

    [Export] private int attackFrames = 60;
    private int attackCountdown = 0;

    protected PlayerState state = PlayerState.Idle;

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        Vector2 direction = Input.GetVector("left", "right", "up", "down");

        // don't do this again if already attacking
        if (Input.IsActionJustPressed("attack") && state != PlayerState.Attacking)
        {
            Vector2 mouse_pos = GetGlobalMousePosition();
            animator.Set("parameters/Attack/blend_position", (mouse_pos - GlobalPosition).Normalized());
            //GD.Print(mouse_pos);
            attackCountdown = attackFrames;
            //direction = new Vector2(0, 0);
            Velocity = new Vector2(0, 0);
            state = PlayerState.Attacking;
        }

        switch (state)
        {
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
                    // set blend tree parameters
                    animator.Set("parameters/Idle/blend_position", direction);
                    animator.Set("parameters/Running/blend_position", direction);
                    state = PlayerState.Running;
                }

                // speed from Entity.cs
                Velocity = direction * speed;

                break;
        }


        /*
        if (!direction.IsZeroApprox())
        {
            // set blend tree parameters
            animator.Set("parameters/Idle/blend_position", direction);
            animator.Set("parameters/Running/blend_position", direction);
        }
        */

        MoveAndSlide();
    }

    protected virtual void Attack()
    {
        GD.Print("Attack!!!");
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
