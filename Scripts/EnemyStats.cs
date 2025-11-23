using Godot;
using System;

[GlobalClass]
public partial class EnemyStats : Resource
{

    [Export] public int maxHealth { get; set; } = 1;
    [Export] public int attack { get; set; } = 1;
    [Export] public int speed { get; set; } = 1;

    public EnemyStats() : this(1, 1, 1) { }

    public EnemyStats(int _maxHealth, int _attack, int _speed)
    {
        maxHealth = _maxHealth;
        attack = _attack;
        speed = _speed;
    }
}
