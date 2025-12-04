using Godot;
using System;

[GlobalClass]
public partial class PlayerStats : Resource
{
    [Export] public int health { get; set; } = 1;
    [Export] public int maxHealth { get; set; } = 1;
    [Export] public int attack { get; set; } = 1;
    [Export] public int speed { get; set; } = 100;

    [Export] public int level { get; set; } = 1;
    private int _exp;
    [Export]
    public int exp
    {
        get => _exp;
        set
        {
            _exp = value;
            if (value >= expToNextLevel)
            {
                //GD.Print("level up");
                level++;
                maxHealth += 20;
                health += 20;
                attack += 3;

                expToNextLevel += 20;
                _exp = 0;
            }
            else
            {
                _exp = value;
            }
            //GD.Print($"exp: {_exp}");
        }
    }
    [Export] public int expToNextLevel { get; set; } = 20;
    [Export] public int gold { get; set; } = 0;
    [Export] public float critRate { get; set; } = 0;
    [Export] public float critDamage { get; set; } = 0;
    [Export] public int meterCharge { get; set; } = 0;
    [Export] public int maxMeter { get; set; } = 10;

    public PlayerStats() : this(1, 1, 1, 100, 1, 0, 20, 0, 0, 0, 0, 10) { }

    public PlayerStats(int _health, int _maxHealth, int _attack, int _speed, int _level,
        int _exp, int _expToNextLevel, int _gold, float _critRate, float _critDamage, int _meterCharge, int _maxMeter)
    {
        health = _health;
        maxHealth = _maxHealth;
        attack = _attack;
        speed = _speed;
        //currentHealth = _currentHealth;
        level = _level;
        exp = _exp;
        expToNextLevel = _expToNextLevel;
        gold = _gold;
        critRate = _critRate;
        critDamage = _critDamage;
        meterCharge = _meterCharge;
        maxMeter = _maxMeter;
    }

    public static PlayerStats operator +(PlayerStats lhs, PlayerStats rhs)
    {
        lhs.health = Mathf.Min(lhs.health + rhs.health, lhs.maxHealth);
        lhs.maxHealth += rhs.maxHealth;
        lhs.attack += rhs.attack;
        lhs.speed += rhs.speed;
        lhs.exp += rhs.exp;
        lhs.expToNextLevel += rhs.expToNextLevel;
        lhs.gold += rhs.gold;
        lhs.critRate += rhs.critRate;
        lhs.critDamage += rhs.critDamage;
        lhs.meterCharge += rhs.meterCharge;
        lhs.maxMeter += rhs.maxMeter;
        return lhs;
    }
}
