using Godot;
using System;

public partial class PlayerHUD : CanvasLayer
{
	[Export] Label Gold;
	[Export] ProgressBar meter;
	[Export] ProgressBar HP;
	[Export] Label Level;
	[Export] ProgressBar EXP;

	[Export] Label HPLabel;

	public override void _Ready()
	{
		base._Ready();
		Gold.Text = GameManager.Instance.playerStats.gold.ToString();

		HP.Value = GameManager.Instance.playerStats.health;
		HP.MaxValue = GameManager.Instance.playerStats.maxHealth;
		HPLabel.Text = GameManager.Instance.playerStats.health.ToString() + "/" + GameManager.Instance.playerStats.maxHealth.ToString(); ;
		meter.Value = GameManager.Instance.playerStats.meterCharge;
		Level.Text = $"{GameManager.Instance.playerStats.level}";
		EXP.Value = GameManager.Instance.playerStats.exp;
		EXP.MaxValue = GameManager.Instance.playerStats.expToNextLevel;
    }

	public override void _Process(double delta)
	{
		Gold.Text = GameManager.Instance.playerStats.gold.ToString();

		HP.Value = GameManager.Instance.playerStats.health;
		HP.MaxValue = GameManager.Instance.playerStats.maxHealth;
		HPLabel.Text = GameManager.Instance.playerStats.health.ToString() + "/" + GameManager.Instance.playerStats.maxHealth.ToString(); ;
		meter.Value = GameManager.Instance.playerStats.meterCharge;
		Level.Text = $"{GameManager.Instance.playerStats.level}";
		EXP.Value = GameManager.Instance.playerStats.exp;
		EXP.MaxValue = GameManager.Instance.playerStats.expToNextLevel;
	}
}
