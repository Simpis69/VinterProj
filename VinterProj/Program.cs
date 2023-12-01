using Raylib_cs; //gör så att du kan använda raylib
using System.ComponentModel;
using System.Data;
using System.Numerics;

Raylib.InitWindow(1000, 750, "Vinter Projekt"); //storlek på fönstret
Raylib.SetTargetFPS(60);

Vector2 enemyPosition = new Vector2(800, 350); //där fienden spawnar
float enemyRadius = 40; //radien på fienden
Vector2 enemyVelocity = new Vector2(7); //hastigheten på fienden i mängden pixlar
Vector2[] linearPath = new Vector2[] { new Vector2(180, 350), new Vector2(799, 350), new Vector2(180, 350), new Vector2(799, 350) }; //de kordinaterna som fienden rör sig mellan
int currentPathIndex = 0;

Vector2 movement = new Vector2(4, 4); //hastigheten på spelaren
Rectangle characterRect = new Rectangle(100, 330, 32, 32); //storlek och placering av spelaren
Vector2 goalPos = new Vector2(0, 0);
Rectangle goalRect = default; // Make sure to define goalRect

string scene = "start";
bool renderGame = false; 

        //en matris för där alla olika block ska vara
int[][] level1 = {new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2},
                  new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2},
                  new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2},
                  new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2},
                  new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2},
                  new int[] { 2, 2, 2, 2, 0, 2, 2, 2, 2, 2, 2, 2, 0, 2, 2, 2, 2, 2, 2, 2},
                  new int[] { 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 2},
                  new int[] { 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 2},
                  new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 0, 2, 2, 2, 2, 2, 2, 2, 0, 2, 2, 2},
                  new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2},
                  new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2},
                  new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2},
                  new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2},
                  new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2},
                  new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}};

void DrawLevel(int[][] level) //ritar ut banan efter matrisen
{
    //kollar värdet på siffrorna i banan för att veta vad som ska placeras på mappen
    for (int row = 0; row <= level.Length - 1; row++)
    {
        for (int column = 0; column <= level[row].Length - 1; column++)
        {
            if (level[row][column] == 1)
            {
                goalPos.X = column * 50 + 25;
                goalPos.Y = row * 50 + 25;

                int goalSize = 50;
                goalRect = new Rectangle((int)goalPos.X - goalSize / 2, (int)goalPos.Y - goalSize / 2, goalSize, goalSize);
                Raylib.DrawRectangleRec(goalRect, Color.GREEN);
            }
            else if (level[row][column] == 2)
            {
                Rectangle block = new Rectangle(column * 50, row * 50, 50, 50);
                Raylib.DrawRectangleRec(block, Color.GRAY);
            }
        }
    }
}


// ser till att uppdater fiendens rörelse längs med den linjära vägen
void UpdateLinearMovement(Vector2[] path, ref int currentIndex, ref Vector2 position, Vector2 velocity)
{
    if (currentIndex < path.Length - 1) // kollar om fienden kan röra sig längre innan den ska byta håll
    {
        Vector2 target = path[currentIndex + 1]; // informerar fienden om vilken punkt den ska röra sig mot härnäst
        Vector2 direction = Vector2.Normalize(target - position); // gör så att fienden vet vilket håll den ska åt
        position += direction * velocity; // ser till att fienden rör sig jämn och inte "hackar" framåt

        if (Vector2.Distance(position, target) < velocity.Length()) // kollar om fienden är nog nära sin vändpunkt
        {
            currentIndex++; // gör att fienden faktiskt rör sig mot andra hållet
        }
    }
    else
    {
        currentIndex = 0; //gör att fienden loopar på spåret så den inte stannar
    }
}

void resetGame() // resetar hela spelet när du dör
{
    characterRect = new Rectangle(100, 330, 32, 32);
    enemyPosition = new Vector2(800, 350); 
    currentPathIndex = 0; 
}

//kollar om du kolliderar med en vägg
bool CheckCollisionWithBlocks(Rectangle rect, int[][] level)
{   //en loop för alla rows i leveln
    for (int row = 0; row < level.Length; row++)
    {       //en loop för alla collumner
        for (int column = 0; column < level[row].Length; column++)
        {       //kollar om det finns en 2a och gör ett block på banan
            if (level[row][column] == 2)
            {
                Rectangle block = new Rectangle(column * 50, row * 50, 50, 50);
                //kollar om man kolliderar med ett block
                if (Raylib.CheckCollisionRecs(rect, block))
                {
                    return true; //upptäcker om du nuddar en vägg och ser till att du inte kan gå igenom
                }
            }
        }
    }

    return false; //upptäcker om du inte nuddar en vägg så att du kan röra på dig
}

bool CheckCollisionWithEnemy(Vector2 playerPosition, float playerSize, Vector2 enemyPosition, float enemyRadius)
{       // kollar om du nuddar fienden
    if (Raylib.CheckCollisionCircles(playerPosition, playerSize / 2, enemyPosition, enemyRadius))
    {   //om koden upptäcker en kollision mellan spelaren och fienden dör då
        return true;
    }
    //koden ser till att du inte nuddar fienden
    return false;
}

while (!Raylib.WindowShouldClose())
{
    Raylib.BeginDrawing();

    if (scene == "start") //startskärmen i början av spelet
    {
        Raylib.ClearBackground(Color.SKYBLUE);
        Raylib.DrawText("press space to start", 10, 10, 32, Color.BLACK);

        if (Raylib.IsKeyDown(KeyboardKey.KEY_SPACE)) 
        {
            scene = "game"; // byter scen från startskärmen till spelet
            resetGame(); // återställer allas position
            renderGame = true; //Gör att spelets map renderas 
        }
    }           //är spelet igång så kan du röra karaktären med WASD
    else if (scene == "game" && renderGame)
    {
        Rectangle nextPosition = characterRect; //variabel för kollisioner

        if (Raylib.IsKeyDown(KeyboardKey.KEY_W))
        {
            nextPosition.Y -= movement.Y;
        }
        else if (Raylib.IsKeyDown(KeyboardKey.KEY_S))
        {
            nextPosition.Y += movement.Y;
        }
        else if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
        {
            nextPosition.X -= movement.X;
        }
        else if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
        {
            nextPosition.X += movement.X;
        }
            //kollar om du nuddar fienden och tar dig till startskärmen 
        if (CheckCollisionWithEnemy(new Vector2(nextPosition.X + nextPosition.Width / 2, nextPosition.Y + nextPosition.Height / 2),
                                    Math.Max(nextPosition.Width, nextPosition.Height), enemyPosition, enemyRadius))
        {
            scene = "start"; //tar dig tillbaks till startskärmen  
            resetGame(); // tar allt tillbaka till sin ursprungs punkt
            renderGame = false; // spelet är inte renderat eftersom att du är i startskärmen
        }
        else
        {       // Kollisioner med väggar så gör att du inte kan ta dig ut genom kartan samt uppdaterar spelarens position
            if (!CheckCollisionWithBlocks(nextPosition, level1))
            {
                characterRect = nextPosition;
            }
        }
            //kollar om du kommer i mål och byter scenen till win screenen
        if (Raylib.CheckCollisionRecs(new Rectangle(characterRect.X, characterRect.Y, characterRect.Width, characterRect.Height), goalRect))
        {
            scene = "win";
        }

        // clearar bakgrunden, skapar karaktären och renderar in banan
        Raylib.ClearBackground(Color.PURPLE); //clearar bakgrunden och gör skärmen till lila
        Raylib.DrawRectangleRec(characterRect, Color.RED); //ritar ut karaktären samt gör den röd
        DrawLevel(level1);

        // uppdaterar fiendens rörelse på den linjära banan, bestämmer vart fienden ska åka, bestämmer den nuvarande positionen och bestämmer hastigheten som fienden rör sig på
        UpdateLinearMovement(linearPath, ref currentPathIndex, ref enemyPosition, enemyVelocity);
        // ritar ut fienden med de bestämda X och Y värdena, radien av cirkeln och färgen
        Raylib.DrawCircle((int)enemyPosition.X, (int)enemyPosition.Y, enemyRadius, Color.DARKBLUE);
    }
    else if (scene == "win")
    {   //win scenen
        Raylib.ClearBackground(Color.GREEN); //gör bakgrunden grön
        Raylib.DrawText("You Win! Press SPACE to play again", 10, 10, 32, Color.BLACK); //skriver ett meddelande

        //byter till game scenen
        if (Raylib.IsKeyDown(KeyboardKey.KEY_SPACE))
        {
            scene = "game";
            resetGame();
            renderGame = true;
        }
    }

    Raylib.EndDrawing();
}