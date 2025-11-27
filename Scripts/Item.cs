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

        UpdateStats(itemInfo);
        itemText.Visible = false;
    }

    // now this can be called at runtime (probably when populatin)
    public void UpdateStats(ItemStats newStats)
    {
        itemInfo = newStats;

        if (itemInfo != null)
        {
            Texture = itemInfo.sprite;
            itemText.Clear();
            itemText.AppendText($"{itemInfo.itemName}\n{itemInfo.goldCost} ");
            itemText.AddImage(goldSprite);
        }
        else
        {
            itemText.Clear();
            itemText.AppendText("No item resource on this item :(");
        }
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

    // both of these from Control node
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
