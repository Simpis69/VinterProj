using Raylib_cs; //tillåter användning av raylib
using System.ComponentModel;
using System.Data;
using System.Numerics; //gör så att man kan använda vectorer

Raylib.InitWindow(1000,750, "Vinter Projekt");  //betsämemr storleken av fönstret  
Raylib.SetTargetFPS(60);


Vector2 enemyPosition = Vector2.Zero;
float enemyRadius = 20;
Vector2 enemyVelocity = new Vector2(2, 2);
Vector2[] linearPath = new Vector2[] { new Vector2(100, 120), new Vector2(100, 650)};
int currentPathIndex = 0;


  
Vector2 movement = new  Vector2(4, 4);  //bestämmer hastigheten som din karaktär rör sig i
Vector2 position = new Vector2(400, 300); 
Vector2 goalPos = new Vector2(0, 0);


Rectangle characterRect = new Rectangle(100,330,32,32); //bestämmer punkten som du spawnar på och storleken av karaktären
List<Rectangle> walls = new();   

float goalSize = 25;
string scene = "start";

// int points = 0;





int[][] level1 = {new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2},
                  new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2},
                  new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2},
                  new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2},
                  new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2},
                  new int[] { 2, 2, 2, 2, 0, 2, 2, 2, 2, 2, 2, 2, 0, 2, 2, 2, 2, 2, 2, 2},
                  new int[] { 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2},
                  new int[] { 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2},
                  new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 0, 2, 2, 2, 2, 2, 2, 2, 0, 2, 2, 2},
                  new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2},
                  new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2},
                  new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2},
                  new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2},
                  new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2},
                  new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}};



void DrawLevel(int[][] level)
{
    for (int row = 0; row <= level.Count() - 1; row++)
    {
        for (int column = 0; column <= level[row].Count() - 1; column++)
        {
            //If the current row and collumn is 1 then spawn a goal at those coordinates
            if (level[row][column] == 1)
            {
                goalPos.X = column * 50 + 25;
                goalPos.Y = row * 50 + 25;

                Raylib.DrawCircle((int)goalPos.X, (int)goalPos.Y, goalSize, Color.GREEN);
            }
            //If the current row and column is a 2 then spawn a block at those coordinates
            else if (level[row][column] == 2)
            {
                Rectangle block = new Rectangle(column * 50, row * 50, 50, 50);
                Raylib.DrawRectangleRec(block, Color.GRAY);
                Raylib.CheckCollisionRecs(characterRect, block);
            }     
        }
    }
}



void UpdateLinearMovement(Vector2[] path, ref int currentIndex, ref Vector2 position, float velocity)
{
    if (currentIndex < path.Length - 1)
    {
        Vector2 target = path[currentIndex + 1];
        Vector2 direction = Vector2.Normalize(target - position);
        position += direction * velocity;

        if (Vector2.Distance(position, target) < velocity)
        {
            currentIndex++;
        }
    }
}

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
    else if(scene == "game")
  {
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


   
      if (scene == "game" && enemyPosition == Vector2.Zero)
    {
        enemyPosition = linearPath[0];
    }


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

DrawLevel(level1);





if (scene == "game")
    {
        UpdateLinearMovement(linearPath, ref currentPathIndex, ref enemyPosition, enemyVelocity);

        Raylib.DrawCircle((int)enemyPosition.X, (int)enemyPosition.Y, enemyRadius, Color.ORANGE);
    }





}
Raylib.EndDrawing();
}

