using Raylib_cs;

namespace DuckHunt_Raylib
{
  static class Program
  {
    static void Main(string[] args)
    {
      int screenWidth = 1100;
      int screenHeight = 700;

      Raylib.InitWindow(screenWidth, screenHeight, "Duck Hunt with Raylib-cs project!");
      Raylib.SetTargetFPS(60);
      Raylib.InitAudioDevice();

      GamePlay gamePlay = new GamePlay(screenWidth, screenHeight);

      gamePlay.LoadContent();
      gamePlay.Initialize();

      // if (gamePlay.IsKeyPressed() == true)
      // {
      //   gamePlay.LoadContent();
      //   gamePlay.Initialize();
      // }

      while (!Raylib.WindowShouldClose())
      {
        gamePlay.Time();
        gamePlay.Update();
        gamePlay.Draw();
      }

      gamePlay.UnloadContent();
      Raylib.CloseWindow();
    }
  }
}
