using Godot;
using System;
using System.ComponentModel;
using System.Threading.Tasks.Dataflow;

[GlobalClass]
public abstract partial class Entity : CharacterBody2D
{
    //Stats
    [Export] protected float speed = 1;
    [Export] protected int health = 1;
    [Export] protected int max_health = 1;
    [Export] protected int attack = 1;
    



    [Export] protected AnimationTree animator;
    

    public virtual void TakeDamage(int base_damage, bool Element)
    {
       
        health -= base_damage;

    }
}
