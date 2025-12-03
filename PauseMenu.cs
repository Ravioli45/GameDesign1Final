using Godot;
using System;
using System.ComponentModel;
using System.Net;

public partial class PauseMenu : Control
{
	[Export(PropertyHint.File)] string MainMenuPath;

	public bool paused = false;
    // Called when the node enters the scene tree for the first time.

    public void OnResumePressed()
    {
        paused = false;
    }

	public void OnMenuPressed()
    {
        //GetTree().ChangeSceneToPacked(MainMenu);
		GetTree().ChangeSceneToFile(MainMenuPath);
		//GetTree().UnloadCurrentScene();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("pause"))
        {
           paused = !paused;
        }

		if (!paused)
        {
            Hide();
			Engine.TimeScale = 1;
        }
        else
        {
            Show();
			Engine.TimeScale = 0;
        }
    }
}