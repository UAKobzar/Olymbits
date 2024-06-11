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

    private const int _gameTurnWeight = 1;
    private const int _hurdleDistanceWeight = 3;
    private static double SimulateHurdleRace(GameInputs inputs, int playerIdx, string playerInputs)
    {
        if (inputs.GPU == "GAME_OVER") return 0;

        int position = inputs.Reg[playerIdx];

        for (int i = inputs.Reg[playerIdx + 3]; i < playerInputs.Length; i++)
        {
            if(position >= inputs.GPU.Length - 1)
            {
                return i * _gameTurnWeight;
            }

            var nextFenceDistance = inputs.GPU.IndexOf('#', position);

            if (playerInputs[i] == 'U')
            {
                position += 2;
                if(nextFenceDistance == 2)
                {
                    i += 3; //stunned
                }
            }
            else
            {
                int travelDistance = 
                    playerInputs[i] == 'L' ? 1 :
                    playerInputs[i] == 'D' ? 2 : 
                    3;

                if(travelDistance >= nextFenceDistance)
                {
                    travelDistance = nextFenceDistance;
                    i += 3;
                }

                position += travelDistance;
            }
        }

        return (inputs.GPU.Length - position - 1) * _hurdleDistanceWeight;
    }

    private const int _archeryDistanceWeight = 3;
    private static double SimulateArchery(GameInputs inputs, int playerIdx, string playerInputs)
    {
        int X = inputs.Reg[playerIdx * 2];
        int Y = inputs.Reg[playerIdx * 2 + 1];

        for (int i = 0; i < inputs.GPU.Length || i < playerInputs.Length; i++)
        {
            switch (playerInputs[i])
            {
                case 'U':
                    Y += inputs.GPU[i] - 48;
                    break;
                case 'D':
                    Y -= inputs.GPU[i] - 48;
                    break;
                case 'L':
                    X -= inputs.GPU[i] - 48;
                    break;
                case 'R':
                    X += inputs.GPU[i] - 48;
                    break;
                default:
                    break;
            }
        }

        return Math.Sqrt(Y * Y + X * X) * _archeryDistanceWeight;
    }

    private const int _rollerStunnedWeight = 10;
    private static double SimulateRoller(GameInputs inputs, int playerIdx, string playerInputs)
    {
        int distance = inputs.Reg[playerIdx];
        int risk = inputs.Reg[playerIdx * 2];

        if(risk < 0)
        {
            return 0; //does not matter
        }

        int indexOfInput = inputs.GPU[playerInputs[0]];

        int deltaDistance = 
            indexOfInput == 0 ? 1 :
            indexOfInput == 3 ? 3 :
            2;
        risk += 
            indexOfInput == 0 ? -1 :
            indexOfInput == 3 ? 2 :
            1;

        risk = risk < 0 ? 0 : risk;

        if(risk >= 5)
        {
            return _rollerStunnedWeight - deltaDistance;
        }
        else
        {
            return risk - deltaDistance;
        }
    }

    private const int _maxDivingPoints = 55;
    private static double SimulateDiving(GameInputs inputs, int playerIdx, string playerInputs)
    {
        int points = inputs.Reg[playerIdx];
        int combo = inputs.Reg[playerIdx * 2];

        for (int i = 0; i < inputs.GPU.Length || i < playerInputs.Length; i++)
        {
            if (inputs.GPU[i] == playerInputs[i])
            {
                combo++;
                points += combo;
            }
            else
            {
                combo = 0;
            }
        }

        return _maxDivingPoints - points;
    }
}

class GameInputs
{
    public GameInputs(string inputs)
    {
        var splited = inputs.Split(" ");
        GPU = splited[0];
        Reg = splited.Skip(1).Select(c=>Int32.Parse(c)).ToList();
    }

    public string GPU { get; private set; }
    public IReadOnlyList<int> Reg { get; private set; }
}