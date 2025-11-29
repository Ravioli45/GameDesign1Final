using Godot;
using System;

[GlobalClass]
public abstract partial class Enemy : Entity
{

	[Export] PackedScene exp;
	[Export] PackedScene gold;
	[Export] public bool is_element_applied;
	[Export] public int amount_gold_drop;
	[Export] PackedScene damageNumbers;

	protected int currentHealth = -1;

	[Export] protected EnemyStats stats;

	public override void _Ready()
	{
		base._Ready();
		currentHealth = stats.maxHealth;
    }


	public override void TakeDamage(int base_damage, bool Element)
	{
		DamageNumbers Instance = damageNumbers.Instantiate<DamageNumbers>();
		Instance.GlobalPosition = this.GlobalPosition;
		Instance.elementAttack = Element;
		
		if (Element && is_element_applied)
		{
			base_damage *= 2;
		}
		else if (Element && !is_element_applied)
		{
			this.is_element_applied = true;
		}

		currentHealth -= base_damage;
		Instance.Text = base_damage.ToString();
		GetTree().Root.AddChild(Instance);

		if (currentHealth <= 0)
		{
			//GD.Print("enemy died: " + base_damage);
		}
	}
	public void Die()
    {
		
		int exp_amount = GD.RandRange(1, 4);
		for (int i = 0; i < exp_amount; i++)
		{
			Node2D Instance = exp.Instantiate<Node2D>();
			Instance.Position = new Vector2(this.Position.X + GD.RandRange(-5, 5), this.Position.Y + GD.RandRange(-5, 5));
		}

		for (int i = 0; i < amount_gold_drop; i++)
		{
			Node2D Instance = gold.Instantiate<Node2D>();
			Instance.Position = new Vector2(this.Position.X + GD.RandRange(-5, 5), this.Position.Y + GD.RandRange(-5, 5));
		}

		//ADD DIE SOUND EFFECT HERE
    }
}
