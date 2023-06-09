using Godot;
using System;

public class Game : Node2D
{
    Player player;
    Timer gameTimer;
    Label gameTimeText, playerAmmoText;
    CanvasLayer gameOverScreen, pauseMenuScreen;
    PackedScene alienScene, timePowerUp, ammoPowerUp;
    Random random;

    public static int gameTime = 100;

    public override void _Ready()
    {
        random = new Random();
        player = GetNode<Player>("/root/Game/Player");

        gameTimer = GetNode<Timer>("/root/Game/GameTimer");
        gameTimeText = GetNode<Label>("/root/Game/Player UI/VBoxContainer2/GameTime");
        playerAmmoText = GetNode<Label>("/root/Game/Player UI/VBoxContainer3/PlayerAmmunition");



        gameTimer.Start();
    }

    public override void _Process(float delta)
    {
        if (player != null)
        {
            gameTimeText.Text = $"Time: {gameTime}";
            playerAmmoText.Text = $"Ammo: {player.ammo}";
        }

        if (player.Health <= 0)
        {
            gameOverScreen = GetNode<CanvasLayer>("/root/Game/GameOverScreen");
            gameOverScreen.Visible = true;

            pauseMenuScreen = GetNode<CanvasLayer>("/root/Game/PauseMenu");
            pauseMenuScreen.QueueFree();

            GetTree().Paused = true;
        }

        if (gameTime <= 0)
        {
            gameOverScreen = GetNode<CanvasLayer>("/root/Game/GameOverScreen");
            gameOverScreen.Visible = true;

            pauseMenuScreen = GetNode<CanvasLayer>("/root/Game/PauseMenu");
            pauseMenuScreen.QueueFree();

            GetTree().Paused = true;
        }

        if (gameTime <= 0)
        {
            gameTimer.Stop();
        }

        if (gameTime <= 30 && gameTime > 9)
        {
            gameTimeText.Modulate = new Color(255, 255, 0);

            if (gameTime <= 10)
            {
                gameTimeText.Modulate = new Color(1, 0, 0);
            }
        }

    }

    public void _on_GameTimer_timeout()
    {
        gameTime -= 1;
    }

    public void RestartGame()
    {
        GetTree().ReloadCurrentScene();
        gameTime = 100;
    }

    //Alien Spawn
    public void _on_Timer_timeout()
    {
        var alienType = random.Next(1, 3);

        alienScene = GD.Load<PackedScene>("res://Scenes/Alien.tscn");
        Alien alien = (Alien)alienScene.Instance();
        alien.Position = new Vector2(random.Next(-2000, 2000), random.Next(-600, 600));
        alien.chaseRange = 400;
        alien.SetAlienType(alienType);
        AddChild(alien);
    }
}
