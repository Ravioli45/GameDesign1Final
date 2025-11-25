using Godot;
using System;

public partial class PlayerHUD : CanvasLayer
{
	[Export] Label Gold;
	[Export] ProgressBar meter;
	[Export] ProgressBar HP;
	[Export] Label Level;


	
	public override void _Process(double delta)
    {
        Gold.Text = GameManager.Instance.playerStats.gold.ToString();

		HP.Value = GameManager.Instance.playerStats.health;
		HP.MaxValue = GameManager.Instance.playerStats.maxHealth;
		meter.Value = GameManager.Instance.playerStats.meterCharge;
		
    }
}
