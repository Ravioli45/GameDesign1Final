using Godot;
using System;

public partial class FollowCamera : Camera2D
{

    [Export] public Node2D target;
    [Export] public bool followTarget = true;

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (followTarget && target != null)
        {
            GlobalPosition = target.GlobalPosition;
        }
    }
}
