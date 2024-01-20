using System;
using System.Linq;


Random random = new();
Console.CursorVisible = false;
int height = Console.WindowHeight - 1;
int width = Console.WindowWidth - 5;
bool shouldExit = false;

// Console position of the player
int playerX = 0;
int playerY = 0;

// Console position of the food
int foodX = 0;
int foodY = 0;

// Available player and food strings
string[] states = { "('-')", "(^-^)", "(X_X)" };
string[] foods = { "@@@@@", "$$$$$", "#####" };

// Current player string and length
string player = states[0];
int playerLength = player.Length;

// Index, type, and length of the current food
int food = 0;
string currentFood = "";
int foodLength = currentFood.Length;

bool[] eatenFood = new bool[foodLength];


InitializeGame();
while (!shouldExit)
{
    if (TerminalResized())
    {
        Console.Clear();
        Console.WriteLine("Console was resized. Program exiting.");
        shouldExit = true;
        break;
    }

    if (CheckIfFoodConsumed())
    {
        ChangePlayer();
        FreezePlayer();
        ShowFood();
    }

    Move();
}

Console.WriteLine();

// Returns true if the Terminal was resized 
bool TerminalResized()
{
    return height != Console.WindowHeight - 1 || width != Console.WindowWidth - 5;
}

// Displays random food at a random location
void ShowFood()
{
    // Update food to a random index
    food = random.Next(0, foods.Length);

    // Update food position to a random location
    foodX = random.Next(0, width - player.Length);
    foodY = random.Next(0, height - 1);

    // Display the food at the location
    Console.SetCursorPosition(foodX, foodY);
    currentFood = foods[food];
    foodLength = currentFood.Length;
    eatenFood = new bool[foodLength];
    Console.Write(currentFood);
}

// Changes the player to match the food consumed
void ChangePlayer()
{
    player = states[food];
    Console.SetCursorPosition(playerX, playerY);
    Console.Write(player);
}

// Temporarily stops the player from moving
void FreezePlayer()
{
    if (player == "(X_X)")
    {
        System.Threading.Thread.Sleep(1000);
        player = states[0];
    }
}

// Checks whether player is energetic
bool AcceleratePlayer()
{
    return player == "(^-^)";
}

bool CheckIfFoodConsumed()
{
    // check if player and food are on same horizontal line
    if (playerY == foodY)
    {
        // check if food and player overlap
        if ((playerX + playerLength - 1 >= foodX) && (playerX <= foodX + foodLength - 1))
        {
            // determine the pieces of food eaten
            for (int i = 0; i < foodLength; i++)
            {
                // check whether each piece of food is within the range of the player
                if (foodX + i >= playerX && foodX + i <= playerX + playerLength - 1)
                {
                    eatenFood[i] = true;
                }
            }

            // return whether every piece of food has been eaten
            return !eatenFood.Any(value => value == false);
        }
    }

    return false;
}

// Reads directional input from the Console and moves the player
void Move()
{
    int lastX = playerX;
    int lastY = playerY;

    switch (Console.ReadKey(true).Key)
    {
        case ConsoleKey.UpArrow:
            playerY--;
            break;
        case ConsoleKey.DownArrow:
            playerY++;
            break;
        case ConsoleKey.LeftArrow:
            if (AcceleratePlayer())
                playerX -= 3;
            else
                playerX--;
            break;
        case ConsoleKey.RightArrow:
            if (AcceleratePlayer())
                playerX += 3;
            else
                playerX++;
            break;
        case ConsoleKey.Escape:
            shouldExit = true;
            break;
        default:
            shouldExit = true;
            break;
    }

    // Clear the characters at the previous position
    Console.SetCursorPosition(lastX, lastY);
    for (int i = 0; i < player.Length; i++)
    {
        Console.Write(" ");
    }

    // Keep player position within the bounds of the Terminal window
    playerX = (playerX < 0) ? 0 : (playerX >= width ? width : playerX);
    playerY = (playerY < 0) ? 0 : (playerY >= height ? height : playerY);

    // Draw the player at the new location
    Console.SetCursorPosition(playerX, playerY);
    Console.Write(player);
}

// Clears the console, displays the food and player
void InitializeGame()
{
    Console.Clear();
    ShowFood();
    Console.SetCursorPosition(0, 0);
    Console.Write(player);
}