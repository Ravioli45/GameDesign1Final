using Godot;
using System;

public partial class MainMenu : Control
{

    [Export] private PackedScene firstLevel;
    [Export] private Control mainButtons;
    [Export] private Control options;

    public override void _Ready()
    {
        base._Ready();

        mainButtons.Visible = true;
        options.Visible = false;
        AudioManager.Instance.PlayBGM("main_menu");
    }

    public void OnPlayButtonPress()
    {
        AudioManager.Instance.PlaySFX("button_click");
        AudioManager.Instance.PlayBGM("dungeon1");
        GetTree().ChangeSceneToPacked(firstLevel);
    }

    public void OnOptionButtonPress()
    {
        AudioManager.Instance.PlaySFX("button_click");
        mainButtons.Visible = false;
        options.Visible = true;
    }

    public void OnOptionBackButtonPress()
    {
        AudioManager.Instance.PlaySFX("button_click");
        mainButtons.Visible = true;
        options.Visible = false;
    }

    public void OnPressQuitButton()
    {
        AudioManager.Instance.PlaySFX("button_click");
        GetTree().Quit();
    }
}
