using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Player
{
    static void Main(string[] args)
    {
        int playerIdx = int.Parse(Console.ReadLine());
        int nbGames = int.Parse(Console.ReadLine());

        // game loop
        while (true)
        {
            for (int i = 0; i < 3; i++)
            {
                string scoreInfo = Console.ReadLine();
            }
            for (int i = 0; i < nbGames; i++)
            {
                string[] inputs = Console.ReadLine().Split(' ');
                string gpu = inputs[0];
                int reg0 = int.Parse(inputs[1]);
                int reg1 = int.Parse(inputs[2]);
                int reg2 = int.Parse(inputs[3]);
                int reg3 = int.Parse(inputs[4]);
                int reg4 = int.Parse(inputs[5]);
                int reg5 = int.Parse(inputs[6]);
                int reg6 = int.Parse(inputs[7]);

                if(gpu != "GAME_OVER")
                {
                    int distanceToFence = gpu.IndexOf('#', reg0) - reg0;

                    string output = distanceToFence == 3 ? "DOWN" :
                        distanceToFence == 2 ? "LEFT" :
                        distanceToFence == 1 ? "UP" :
                        "RIGHT";

                    Console.WriteLine(output);
                }
                else
                {
                    Console.WriteLine("UP");
                }
            }
        }
    }
}