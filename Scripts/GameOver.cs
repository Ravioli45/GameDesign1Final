using Godot;
using System;

public partial class GameOver : Control
{
    [Export(PropertyHint.FilePath)] private string MainMenuPath;

    public override void _Ready()
    {
        base._Ready();
    }

    public void OnMainMenuPressed()
    {
        GetTree().ChangeSceneToFile(MainMenuPath);
    }
}
