using System.Numerics;
using DuckHunt;
using Raylib_cs;

namespace DuckHunt_Raylib
{
  public class GamePlay
  {

    // Screen
    private int screenWidth;
    private int screenHeight;
    private Vector2 center;

    // Positions
    private Vector2 gameOverPos;
    private Vector2 scorePos;
    private Vector2 duckScorePos;
    private Vector2 posScorePos;
    private int posScore;
    private Vector2 finalScorePos;
    private Vector2 finalNumScorePos;

    // Rectangles (areas)
    private Rectangle screenRect;
    private Rectangle floraRect;
    private Rectangle directRect;
    private Rectangle dogRect;
    private Rectangle endDogRect;
    private Rectangle[] cloudRect;
    private Rectangle[] pointDuckRect;
    private Rectangle shotRect;
    private Rectangle sourceRect;
    private Rectangle retRect;
    private Rectangle exRect;

    // Sounds
    private Sound quackQuack;
    private Sound sniff;
    private Sound gunshot;
    private Sound gunCock;
    private Sound gunDry;
    private Sound quackSound;

    // Graphics
    // private Texture2D directions;
    private int isShooting;
    private Texture2D dog;
    private Texture2D screen;
    private Texture2D flora;
    private Texture2D duck;
    private Texture2D endDog;
    private Texture2D pointDuck;
    private Texture2D cloud;
    private Texture2D shot0;
    private Texture2D shot1;
    private Texture2D shot2;
    private Texture2D shot3;
    private Texture2D reticle;
    private Texture2D explosion;
    private int score;

    // Timer
    private float timer;
    private float interval;
    private float deltaTime;
    private float jumpTime;

    // Frames
    private int currentFrame;
    private int frameCount;

    // Sprites
    private int spriteWidth;
    private int spriteHeight;

    // Ducks
    private Duck quack;

    // Fonts
    private Font quartz;


    public GamePlay(int width, int height)
    {
      screenWidth = width;
      screenHeight = height;
    }

    public void Time()
    {
      deltaTime = Raylib.GetFrameTime();
    }

    public void Initialize()
    {
      directRect = new Rectangle(225, 75, 650, 325);
      dogRect = new Rectangle(-190, 410, 190, 138);
      timer = 0f;
      jumpTime = 0f;
      interval = 1000f / 7f;
      currentFrame = 0;
      frameCount = 8;
      spriteWidth = 55;
      spriteHeight = 47;

      screenRect = new Rectangle(0, 0, screenWidth, screenHeight);
      floraRect = new Rectangle(0, 38, screenWidth, 500);

      retRect = new Rectangle(525, 325, 65, 65);
      exRect = new Rectangle(535, 335, 45, 45);
      center = new Vector2(557, 557);

      quack = new Duck(gunCock, quackQuack);

      shotRect = new Rectangle(113, 607, 80, 45);

      duckScorePos = new Vector2(275, 592);
      scorePos = new Vector2(825, 623);
      posScorePos = new Vector2(825, 555);

      gameOverPos = new Vector2(400, 150);
      finalScorePos = new Vector2(475, 200);
      finalNumScorePos = new Vector2(490, 250);
      endDogRect = new Rectangle(485, 425, 150, 109);

      pointDuckRect = new Rectangle[20];
      for (int x = 0; x < 20; x++)
      {
        if (x < 10)
          pointDuckRect[x] = new Rectangle(x * 30 + 415, 607, 20, 20);
        else
          pointDuckRect[x] = new Rectangle(pointDuckRect[x - 10].X, 632, 20, 20);
      }

      cloudRect = new Rectangle[4];
      cloudRect[0] = new Rectangle(-140, 0, 350, 100);
      cloudRect[1] = new Rectangle(140, 110, 350, 100);
      cloudRect[2] = new Rectangle(420, 0, 350, 100);
      cloudRect[3] = new Rectangle(700, 110, 350, 100);
    }

    public void LoadContent()
    {
      screen = Raylib.LoadTexture("assets/screen.png");
      sniff = Raylib.LoadSound("assets/sniff.wav");
      Raylib.PlaySound(sniff);
      gunshot = Raylib.LoadSound("assets/gunshot.wav");
      gunCock = Raylib.LoadSound("assets/gunCock.wav");
      gunDry = Raylib.LoadSound("assets/gunDry.wav");
      quackSound = Raylib.LoadSound("assets/quackSound.wav");

      dog = Raylib.LoadTexture("assets/dogWalk.png");
      flora = Raylib.LoadTexture("assets/flora.png");
      explosion = Raylib.LoadTexture("assets/explosion.png");
      duck = Raylib.LoadTexture("assets/duck.png");
      endDog = Raylib.LoadTexture("assets/endDog.png");
      cloud = Raylib.LoadTexture("assets/cloud.png");

      reticle = Raylib.LoadTexture("assets/reticle.png");
      shot0 = Raylib.LoadTexture("assets/shot0.png");
      shot1 = Raylib.LoadTexture("assets/shot1.png");
      shot2 = Raylib.LoadTexture("assets/shot2.png");
      shot3 = Raylib.LoadTexture("assets/shot3.png");
      pointDuck = Raylib.LoadTexture("assets/pointDuck.png");

      // directions = Raylib.LoadTexture("assets/directions.png");
      quartz = Raylib.LoadFont("assets/Quartz.ttf");
    }

    public void UnloadContent()
    {
      // Raylib.UnloadTexture(directions);
      Raylib.UnloadTexture(dog);
      Raylib.UnloadSound(sniff);
      Raylib.UnloadTexture(screen);
      Raylib.UnloadTexture(flora);
      Raylib.UnloadTexture(reticle);
      Raylib.UnloadTexture(explosion);
      Raylib.UnloadSound(gunshot);
      Raylib.UnloadSound(gunCock);
      Raylib.UnloadSound(gunDry);
      Raylib.UnloadTexture(duck);
      Raylib.UnloadSound(quackSound);
      Raylib.UnloadTexture(shot0);
      Raylib.UnloadTexture(shot1);
      Raylib.UnloadTexture(shot2);
      Raylib.UnloadTexture(shot3);
      Raylib.UnloadFont(quartz);
      Raylib.UnloadTexture(pointDuck);
      Raylib.UnloadTexture(endDog);
      Raylib.UnloadTexture(cloud);
    }

    public void Update()
    {
      timer += deltaTime * 1000;

      if (timer > interval)
      {
        currentFrame++;

        if (!(currentFrame == 0 || currentFrame == 1 || currentFrame == 5) && dogRect.X <= 450)
          dogRect.X += 10;
        if (currentFrame > frameCount - 4 && dogRect.X <= 450)
          currentFrame = 0;
        if (dogRect.X >= 460 && jumpTime <= 50f)
        {
          currentFrame = 5;
          jumpTime += deltaTime * 1000;
        }
        if (jumpTime > 50f)
        {
          currentFrame = 6;
          dogRect.Y = 345;
          jumpTime += deltaTime * 1000;
        }
        if (jumpTime > 70f)
        {
          dogRect.Y = 360;
          currentFrame = 7;
        }
        if (jumpTime > 100f)
          Raylib.UnloadTexture(dog);

        timer = 0f;
      }

      sourceRect = new Rectangle(currentFrame * spriteWidth, 0, spriteWidth, spriteHeight);

      // Clouds scrolling
      for (int x = 0; x < 4; x++)
      {
        cloudRect[x].X -= 1;

        if (cloudRect[x].X + 350 <= 0)
          cloudRect[x].X = screenWidth;
      }

      if (!quack.gameOver)
      {
        Vector2 mousePosition = Raylib.GetMousePosition();
        retRect.X = mousePosition.X - retRect.Width / 2;
        retRect.Y = mousePosition.Y - retRect.Height / 2;
        exRect.X = retRect.X + 10;
        exRect.Y = retRect.Y + 10;
        center.X = retRect.X + 32;
        center.Y = retRect.Y + 32;

        if (retRect.X <= -32)
        {
          retRect.X = -32;
          exRect.X = -22;
        }
        if (retRect.Y <= -32)
        {
          retRect.Y = -32;
          exRect.Y = -22;
        }
        if (retRect.X >= screenWidth - 32)
        {
          retRect.X = screenWidth - 32;
          exRect.X = screenWidth - 22;
        }
        if (retRect.Y >= 500)
        {
          retRect.Y = 500;
          exRect.Y = 510;
        }

        quack.Update();

        if (quack.isFlying)
        {
          if (quack.shot == 3)
            posScore = ((int)quack.duckRect.Y + 70) * quack.shot;
          else
            posScore = ((int)quack.duckRect.Y + 70) * (quack.shot + 1);
        }

        if (posScore <= 0)
          posScore = 0;

        if (Raylib.IsMouseButtonDown(MouseButton.Left) && quack.shot == 0)
        {
          Raylib.PlaySound(gunDry);
        }

        if (Raylib.IsMouseButtonDown(MouseButton.Left) && quack.shot != 0)
        {
          isShooting = 5;
          Raylib.PlaySound(gunshot);
          quack.shot--;

          if (center.X >= quack.duckRect.X && center.X <= quack.duckRect.X + 65
              && center.Y >= quack.duckRect.Y && center.Y <= quack.duckRect.Y + 71
              && quack.isFlying)
          {
            quack.isFlying = false;
            Raylib.PlaySound(quackSound);
            score += posScore;
            quack.isDead[quack.lives] = true;
            quack.lives++;
          }
        }
        else
        {
          isShooting--;
        }
      }

      if (quack.gameOver)
      {
        if (endDogRect.Y > 350)
          endDogRect.Y -= 2;
      }
    }

    public void Draw()
    {
      Raylib.BeginDrawing();
      Raylib.ClearBackground(Color.White);

      Raylib.DrawTexture(screen, 0, 0, Color.White);

      if (quack.shot == 3)
        Raylib.DrawTexture(shot3, (int)shotRect.X, (int)shotRect.Y, Color.White);
      else if (quack.shot == 2)
        Raylib.DrawTexture(shot2, (int)shotRect.X, (int)shotRect.Y, Color.White);
      else if (quack.shot == 1)
        Raylib.DrawTexture(shot1, (int)shotRect.X, (int)shotRect.Y, Color.White);
      else
        Raylib.DrawTexture(shot0, (int)shotRect.X, (int)shotRect.Y, Color.White);

      Raylib.DrawTextEx(quartz, "HIT:", duckScorePos, 40, 2, new Color(64, 191, 255, 255));
      Raylib.DrawTextEx(quartz, score.ToString(), scorePos, 40, 2, new Color(64, 191, 255, 255));
      Raylib.DrawTextEx(quartz, posScore.ToString(), posScorePos, 40, 2, new Color(64, 191, 255, 255));

      for (int x = 0; x < 20; x++)
      {
        if (quack.isDead[x])
          Raylib.DrawTexture(pointDuck, (int)pointDuckRect[x].X, (int)pointDuckRect[x].Y, Color.Red);
        else if (x == quack.lives)
          Raylib.DrawTexture(pointDuck, (int)pointDuckRect[x].X, (int)pointDuckRect[x].Y, Color.SkyBlue);
        else
          Raylib.DrawTexture(pointDuck, (int)pointDuckRect[x].X, (int)pointDuckRect[x].Y, Color.White);
      }

      if (dog.Id != 0)
      {
        foreach (Rectangle r in cloudRect)
          Raylib.DrawTexture(cloud, (int)r.X, (int)r.Y, Color.White);
        Raylib.DrawTexture(flora, (int)floraRect.X, (int)floraRect.Y, Color.White);
        Raylib.DrawTextureRec(dog, sourceRect, new Vector2(dogRect.X, dogRect.Y), Color.White);
        // Raylib.DrawTexture(directions, (int)directRect.X, (int)directRect.Y, Color.White);
      }
      else
      {
        Raylib.DrawTexturePro(duck, quack.sourceRect, quack.duckRect, Vector2.Zero, 0f, Color.White);
        if (quack.gameOver)
          Raylib.DrawTexture(endDog, (int)endDogRect.X, (int)endDogRect.Y, Color.White);
        foreach (Rectangle r in cloudRect)
          Raylib.DrawTexture(cloud, (int)r.X, (int)r.Y, Color.White);
        Raylib.DrawTexture(flora, (int)floraRect.X, (int)floraRect.Y, Color.White);
        if (isShooting > 0)
          Raylib.DrawTexture(explosion, (int)exRect.X, (int)exRect.Y, Color.White);
        if (!quack.gameOver)
          Raylib.DrawTexture(reticle, (int)retRect.X, (int)retRect.Y, Color.White);
      }

      if (quack.gameOver)
      {
        Raylib.DrawTextEx(quartz, "Game Over!", gameOverPos, 60, 2, Color.Black);
        Raylib.DrawTextEx(quartz, "Score:", finalScorePos, 40, 2, Color.Black);
        Raylib.DrawTextEx(quartz, score.ToString(), finalNumScorePos, 40, 2, Color.Black);
      }

      Raylib.EndDrawing();
    }

    public bool IsKeyPressed()
    {
      if (Raylib.IsKeyPressed(KeyboardKey.Space))
      {
        return true;
      }
      return false;
    }
  }
}
