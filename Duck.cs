using Raylib_cs;
using System.Numerics;

namespace DuckHunt
{
  class Duck
  {
    public Rectangle duckRect;
    public bool isFlying;
    public bool[] isDead;
    public int lives;
    public bool gameOver;

    float timer;
    float interval;
    public int currentFrame;
    int frameCount;
    int spriteWidth;
    int spriteHeight;
    public Rectangle sourceRect;
    bool flipHorizontally;
    int flyTime;
    int leftRight;

    int fallTime;

    Random rand;

    public int shot;
    public Sound gunCock;

    public Duck(Sound gunCockSound)
    {
      isDead = new bool[20];
      rand = new Random();
      duckRect = new Rectangle(rand.Next(200, 801), 450, 74, 70);
      isFlying = true;

      timer = 0f;
      interval = 1000f / 20f;
      currentFrame = 0;
      frameCount = 8;
      spriteWidth = 34;
      spriteHeight = 31;

      shot = 3;
      gunCock = gunCockSound;
    }

    public void Update()
    {
      float deltaTime = Raylib.GetFrameTime();
      timer += deltaTime * 1000;

      if (timer > interval)
      {
        if (isFlying)
        {
          currentFrame++;
          if (currentFrame > frameCount - 6)
            currentFrame = 0;
          timer = 0f;

          if (flyTime == 0)
          {
            leftRight = rand.Next(2);
            flyTime = 10;
          }

          if (duckRect.X >= 1026)
          {
            leftRight = 0;
            flyTime = 10;
          }
          if (duckRect.X <= 0)
          {
            leftRight = 1;
            flyTime = 10;
          }

          if (leftRight == 0)
          {
            duckRect.X -= 15;
            duckRect.Y -= 10;
            flipHorizontally = true;
          }
          else
          {
            duckRect.X += 15;
            duckRect.Y -= 10;
            flipHorizontally = false;
          }

          flyTime--;

          if (duckRect.Y <= -71)
          {
            duckRect.X = rand.Next(200, 801);
            duckRect.Y = 450;
            lives++;
            if (lives == 20)
            {
              gameOver = true;
              isFlying = false;
            }
            if (shot != 3 && !gameOver)
            {
              Raylib.PlaySound(gunCock);
              shot = 3;
            }
          }
        }
        else
        {
          if (fallTime <= 50)
          {
            currentFrame = 6;
          }
          else if (duckRect.Y <= 450)
          {
            currentFrame = 7;
            duckRect.Y += 5;
          }
          else
          {
            isFlying = true;
            duckRect.X = rand.Next(200, 801);
            duckRect.Y = 450;
            fallTime = 0;
            if (lives == 20)
            {
              gameOver = true;
              isFlying = false;
            }
            if (!gameOver)
            {
              Raylib.PlaySound(gunCock);
              shot = 3;
            }
          }
          fallTime++;
        }

        sourceRect = new Rectangle(currentFrame * spriteWidth, 0, spriteWidth, spriteHeight);
      }
    }

    public void Draw(Texture2D duckTexture)
    {
      Rectangle destRect = new Rectangle(duckRect.X, duckRect.Y, duckRect.Width, duckRect.Height);
      Vector2 origin = new Vector2(0, 0);
      Raylib.DrawTexturePro(duckTexture, sourceRect, destRect, origin, 0f, flipHorizontally ? Raylib_cs.Color.White : Raylib_cs.Color.White);
    }
  }
}