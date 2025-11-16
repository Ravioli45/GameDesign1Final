using Godot;
using System;

[GlobalClass]
public abstract partial class Entity : CharacterBody2D
{
    [Export] protected float speed = 1;

    [Export] protected int health = 1;
    [Export] protected int max_health = 1;

    [Export] protected AnimationTree animator;

    public void TakeDamage(int base_damage)
    {
        health -= base_damage;
    }
}
