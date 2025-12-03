using Godot;
using System;

public partial class Teleporter : Sprite2D
{
    [Export] public PackedScene NextLevel;
    [Export] public RichTextLabel nextLevelLabel;
    [Export] public string BGMName = "dungeon1";
    private bool playerInArea = false;
    private Player playerNode;
    public override void _Ready()
    {
        Visible = false;
    }

    public void StepOn(Node2D body)
    {
        if (body is Player p)
        {
            playerNode = p;
            playerInArea = true;
            nextLevelLabel.Visible = true;
        }
    }
    public void StepOff(Node2D body)
    {
        if (body is Player)
        {
            playerInArea = false;
            nextLevelLabel.Visible = false;
        }
    }
    public void OnTeleporterClick(Node _viewport, InputEvent @event, int shape_idx)
    {
        if (Visible && playerInArea && @event is InputEventMouseButton mouseButton && mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left)
        {
            playerNode.DepleteMeter(10);
            playerNode.enhancedState = false;
            AudioManager.Instance.PlayBGM(BGMName);
            GetTree().CallDeferred("change_scene_to_packed", NextLevel);
        }
    }
}
