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
            int minDistance = Int32.MaxValue;
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

                if (gpu != "GAME_OVER")
                {
                    var reg = playerIdx == 0 ? reg0 :
                        playerIdx == 1 ? reg1 : reg2;

                    var distance = gpu.IndexOf('#', reg) - reg;
                    
                    distance = distance <= 0 ? Int32.MaxValue : distance;

                    minDistance = Math.Min(distance, minDistance);

                }
            }

            string output = minDistance == 3 ? "DOWN" :
                minDistance == 2 ? "LEFT" :
                minDistance == 1 ? "UP" :
                "RIGHT";

            Console.WriteLine(output);
        }
    }
}