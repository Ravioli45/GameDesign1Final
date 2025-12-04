using Godot;
using System;

public partial class MainMenu : Control
{

    [Export] private PackedScene firstLevel;
    [Export] private Control mainButtons;
    [Export] private Control options;
    [Export] private Button ChiptuneButton;
    [Export] private Button InstrumentalButton;

    public override void _Ready()
    {
        base._Ready();

        mainButtons.Visible = true;
        options.Visible = false;
        AudioManager.Instance.PlayBGM($"mainmenu_{GameManager.Instance.musicType}");
        GameManager.Instance.ResetStats();
        SetMusicButtons(GameManager.Instance.musicType);
    }

    public void OnPlayButtonPress()
    {
        AudioManager.Instance.PlaySFX("button_click");
        AudioManager.Instance.PlayBGM($"dungeon1_{GameManager.Instance.musicType}");
        GetTree().ChangeSceneToPacked(firstLevel);
    }

    public void OnOptionButtonPress()
    {
        AudioManager.Instance.PlaySFX("button_click");
        mainButtons.Visible = false;
        options.Visible = true;
    }

    public void SetMusicButtons(string currentMusic)
    {
        if (currentMusic == "chiptune")
        {
            ChiptuneButton.ButtonPressed = true;
        }
        else if (currentMusic == "instrumental")
        {
            InstrumentalButton.ButtonPressed = true;
        }
        else
        {
            throw new Exception("music buttons are broken (oops)");
        }
    }

    public void OnChiptuneButtonPress()
    {
        AudioManager.Instance.PlaySFX("button_click");
        GameManager.Instance.musicType = "chiptune";
        AudioManager.Instance.PlayBGM($"mainmenu_{GameManager.Instance.musicType}");
    }
    public void OnInstrumentalButtonPress()
    {
        AudioManager.Instance.PlaySFX("button_click");
        GameManager.Instance.musicType = "instrumental";
        AudioManager.Instance.PlayBGM($"mainmenu_{GameManager.Instance.musicType}");
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
