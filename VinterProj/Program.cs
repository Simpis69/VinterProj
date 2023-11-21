using Raylib_cs; //tillåter användning av raylib
using System.ComponentModel;
using System.Numerics; //gör så att man kan använda vectorer

Raylib.InitWindow(1000,650, "hello");
Raylib.SetTargetFPS(60);



  
Vector2 movement = new  Vector2(10, 10);
Vector2 position = new Vector2(400, 300);

Rectangle characterRect = new Rectangle(10,10,32,32);

List<Rectangle> walls = new();   



walls.Add(new Rectangle(100,120,5,320)); //vänstra vertikala väggen i spawn
walls.Add(new Rectangle(295,120,5,270)); //högra vertikala väggen i spawn
walls.Add(new Rectangle(100,115,200,5)); //taket i spawn
walls.Add(new Rectangle(100,440,280,5)); //botten i spawn


string scene = "start";

// int points = 0;



while (!Raylib.WindowShouldClose())
{

// ------------------------------------------------------------------------------------------------------------------------------------
//                                                      GAME LOGIC
// ------------------------------------------------------------------------------------------------------------------------------------
 
    if (scene == "start")
    {
        if(Raylib.IsKeyDown(KeyboardKey.KEY_SPACE))
         {
            scene = "game";
         }
    }
    // movement WASD
    else if(scene == "start")
  {
    movement = Vector2.Zero;}

    
        if(Raylib.IsKeyDown(KeyboardKey.KEY_W))
       { characterRect.Y -= movement.Y;  
    }
    else if(Raylib.IsKeyDown(KeyboardKey.KEY_S))
    {
    characterRect.Y += movement.Y;
    }
    else if(Raylib.IsKeyDown(KeyboardKey.KEY_A))
    {
    characterRect.X -= movement.X;
    }
    else if(Raylib.IsKeyDown(KeyboardKey.KEY_D))
    {
    characterRect.X += movement.X;
    }



// ------------------------------------------------------------------------------------------------------------------------------------  
//                                                       RENDERING
// ------------------------------------------------------------------------------------------------------------------------------------ 

Raylib.BeginDrawing();
if (scene == "start")
{
    Raylib.ClearBackground(Color.SKYBLUE);
    Raylib.DrawText("press space to start", 10, 10, 32, Color.BLACK);
}
else if(scene == "game")
{
Raylib.DrawRectangleRec(characterRect, Color.RED);

Raylib.ClearBackground(Color.PURPLE);


foreach (Rectangle wall in walls)
{
    Raylib.DrawRectangleRec(wall, Color.BLACK);
}


}
Raylib.EndDrawing();
}

