using Godot;
using System;

[GlobalClass]
public partial class PlayerStats : Resource
{
    [Export] public int health { get; set; } = 1;
    [Export] public int maxHealth { get; set; } = 1;
    [Export] public int attack { get; set; } = 1;
    [Export] public int speed { get; set; } = 100;

    [Export] public int exp { get; set; } = 0;
    [Export] public int expToNextLevel { get; set; } = 20;
    [Export] public int gold { get; set; } = 0;
    [Export] public float critRate { get; set; } = 0;
    [Export] public float critDamage { get; set; } = 0;
    [Export] public int meterCharge { get; set; } = 0;
    [Export] public int maxMeter { get; set; } = 10;

    public PlayerStats() : this(1, 1, 1, 100, 0, 20, 0, 0, 0, 0, 10) { }

    public PlayerStats(int _health, int _maxHealth, int _attack, int _speed,
        int _exp, int _expToNextLevel, int _gold, float _critRate, float _critDamage, int _meterCharge, int _maxMeter)
    {
        health = _health;
        maxHealth = _maxHealth;
        attack = _attack;
        speed = _speed;
        //currentHealth = _currentHealth;
        exp = _exp;
        expToNextLevel = _expToNextLevel;
        gold = _gold;
        critRate = _critRate;
        critDamage = _critDamage;
        meterCharge = _meterCharge;
        maxMeter = _maxMeter;
    }
}
