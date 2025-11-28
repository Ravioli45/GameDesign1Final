using Godot;
using System;

[GlobalClass]
public partial class ItemStats : Resource
{

    [Export] public string itemName = null;
    [Export] public int goldCost { get; set; } = 0;
    [Export] public Texture2D sprite { get; set; } = null;
    [Export] public PlayerStats statChanges = null;

    public ItemStats() : this(null, 0, null, null) { }

    public ItemStats(string _itemName, int _goldCost, Texture2D _sprite, PlayerStats _statChanges)
    {
        itemName = _itemName ?? "";
        goldCost = _goldCost;
        sprite = _sprite ?? new Texture2D();
        statChanges = _statChanges ?? new PlayerStats();
    }

}
