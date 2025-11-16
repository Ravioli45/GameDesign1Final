using Godot;
using System;

enum PlayerState
{
    Idle,
    Running,   
}

[GlobalClass]
public abstract partial class Player : Entity
{

    private PlayerState state = PlayerState.Idle;

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        Vector2 direction = Input.GetVector("left", "right", "up", "down");

        if (Input.IsActionJustPressed("attack"))
        {
            Attack();
        }

        switch (state)
        {
            case PlayerState.Idle:

            case PlayerState.Running:

                if (direction.IsZeroApprox())
                {
                    state = PlayerState.Idle;
                }
                else
                {
                    state = PlayerState.Running;
                }

                // speed from Entity.cs
                Velocity = direction * speed;

                break;
        }



        if (!direction.IsZeroApprox())
        {
            // set blend tree parameters
            animator.Set("parameters/Idle/blend_position", direction);
            animator.Set("parameters/Running/blend_position", direction);
        }

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
}
