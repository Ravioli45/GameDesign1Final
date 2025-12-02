using Godot;
using System;

[GlobalClass]
public abstract partial class Entity : CharacterBody2D
{
    
    [Export] protected AnimationTree animator;


    public abstract void TakeDamage(int base_damage, bool Element, Vector2 directionHit);
}
