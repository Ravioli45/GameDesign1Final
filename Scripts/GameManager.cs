using Godot;
using System;

public partial class GameManager : Node
{

    public static GameManager Instance { get; private set; }

    [Export] public PlayerStats playerStats;
    [Export(PropertyHint.File)] public string GameOverScenePath;
    private PlayerStats startingStats;
    public string musicType = "chiptune";

    public override void _Ready()
    {
        if (Instance != null)
        {
            GD.Print("Halt and catch fire (again)");
            return;
        }

        startingStats = (PlayerStats)playerStats.Duplicate();
        Instance = this;
    }

    public void GameOver()
    {
        AudioManager.Instance.PlayBGM($"gameover_{musicType}");
        GetTree().ChangeSceneToFile(GameOverScenePath);
    }

    public void SwitchLevel(PackedScene level, StringName next_song)
    {
        AudioManager.Instance.PlayBGM(next_song + "_" + musicType);
        GetTree().CallDeferred("change_scene_to_packed", level);
    }

    public void SetDisabled(bool disabled)
    {
        playerStats.disabled = disabled;
    }

    public void ResetStats()
    {
        playerStats.disabled = startingStats.disabled;
        playerStats.health = startingStats.health;
        playerStats.maxHealth = startingStats.maxHealth;
        playerStats.attack = startingStats.attack;
        playerStats.speed = startingStats.speed;
        playerStats.level = startingStats.level;
        playerStats.exp = startingStats.exp;
        playerStats.expToNextLevel = startingStats.expToNextLevel;
        playerStats.gold = startingStats.gold;
        playerStats.critRate = startingStats.critRate;
        playerStats.critDamage = startingStats.critDamage;
        playerStats.meterCharge = startingStats.meterCharge;
        playerStats.maxMeter = startingStats.maxMeter;
    }
}
