using Godot;
using System;

public partial class Item : Sprite2D
{

    [Export] private ItemStats itemInfo;

    public override void _Ready()
    {
        base._Ready();

        Texture = itemInfo.sprite;
    }

    // InputEvent from Area2D
    public void OnItemClick(Node _viewport, InputEvent @event, int shape_idx)
    {
        if (@event is InputEventMouseButton mouseButton && mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left)
        {
            if (GameManager.Instance.playerStats.gold >= itemInfo.goldCost)
            {
                GameManager.Instance.playerStats += itemInfo.statChanges;
                GameManager.Instance.playerStats.gold -= itemInfo.goldCost;
                QueueFree();
            }
        }
    }
}
