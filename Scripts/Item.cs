using Godot;
using System;

public partial class Item : Sprite2D
{

    [Export] private ItemStats itemInfo;


    [Export] private RichTextLabel itemText;
    [Export] private Texture2D goldSprite;
    [Export] private AnimationPlayer animationPlayer;

    public override void _Ready()
    {
        base._Ready();

        Texture = itemInfo.sprite;
        itemText.Clear();
        itemText.AppendText($"{itemInfo.itemName}\n{itemInfo.goldCost} ");
        itemText.AddImage(goldSprite);
        itemText.Visible = false;
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

    public void OnMouseEntered()
    {
        itemText.Visible = true;
        animationPlayer.Play("Hover");
    }
    public void OnMouseExitied()
    {
        itemText.Visible = false;
        animationPlayer.Play("Idle");
    }
}
