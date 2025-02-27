using Raylib_cs;
using static Raylib_cs.Raylib;
using System;

namespace DuckHunt
{
  static class Program
  {
    // The main entry point for the application.
    static void Main(string[] args)
    {
      Game game = new Game();
      game.Run();
    }
  }
}

