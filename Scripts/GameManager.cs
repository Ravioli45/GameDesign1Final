using Godot;
using System;

public partial class GameManager : Node
{

    public static GameManager Instance{ get; private set; }

    [Export] public PlayerStats playerStats;

    public override void _Ready()
    {
        if (Instance != null)
        {
            GD.Print("Halt and catch fire (again)");
            return;
        }

        Instance = this;
    }
}
