using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Player
{
    static void Main(string[] args)
    {

        List<string> allPlayerInputs = Enumerable.Range(0, (int)Math.Pow(4,8))
            .Select(i=> i.ToString("x").PadLeft(4, '0')
                    .Replace("0", "UU")
                    .Replace("1", "UD")
                    .Replace("2", "UL")
                    .Replace("3", "UR")
                    .Replace("4", "DU")
                    .Replace("5", "DD")
                    .Replace("6", "DL")
                    .Replace("7", "DR")
                    .Replace("8", "LU")
                    .Replace("9", "LD")
                    .Replace("a", "LL")
                    .Replace("b", "LR")
                    .Replace("c", "RU")
                    .Replace("d", "RD")
                    .Replace("e", "RL")
                    .Replace("f", "RR")).ToList();

        int playerIdx = int.Parse(Console.ReadLine());
        int nbGames = int.Parse(Console.ReadLine());

        // game loop
        while (true)
        {
            for (int i = 0; i < 3; i++)
            {
                string scoreInfo = Console.ReadLine();
            }
            List<GameInputs> gameInputs = new List<GameInputs>();
            for (int i = 0; i < nbGames; i++)
            {
                gameInputs.Add(new GameInputs(Console.ReadLine()));
            }

            double min = Double.MaxValue;
            char input = 'U';

            var sw = new Stopwatch();
            sw.Start();

            foreach (var playerInputs in allPlayerInputs)
            {
                var score = SimulateAll(gameInputs, playerIdx, playerInputs);

                if(score < min)
                {
                    min = score;
                    input = playerInputs[0];
                }
            }

            sw.Stop();

            Console.Error.WriteLine(sw.ElapsedMilliseconds);

            Console.WriteLine(
                input == 'U' ? "UP" :
                input == 'D' ? "DOWN" :
                input == 'L' ? "LEFT" :
                "RIGHT"
                );
        }
    }

    private const int _gameTurnWeight = 1;
    private const int _hurdleDistanceWeight = 10;
    private static double SimulateHurdleRace(GameInputs inputs, int playerIdx, string playerInputs)
    {
        if (inputs.GPU == "GAME_OVER") return 0;

        int position = inputs.Reg[playerIdx];

        for (int i = inputs.Reg[playerIdx + 3]; i < playerInputs.Length; i++)
        {
            if (position >= inputs.GPU.Length - 1)
            {
                return i * _gameTurnWeight;
            }

            var nextFenceDistance = inputs.GPU.IndexOf('#', position) - position;
            nextFenceDistance = nextFenceDistance <= 0 ? Int32.MaxValue : nextFenceDistance;

            if (playerInputs[i] == 'U')
            {
                position += 2;
                if (nextFenceDistance == 2)
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

                if (travelDistance >= nextFenceDistance)
                {
                    travelDistance = nextFenceDistance;
                    i += 3;
                }

                position += travelDistance;
            }
        }

        if (position >= inputs.GPU.Length) position = inputs.GPU.Length - 1;

        return (inputs.GPU.Length - position - 1) * _hurdleDistanceWeight + playerInputs.Length * _gameTurnWeight;
    }

    private const int _archeryDistanceWeight = 10;
    private static double SimulateArchery(GameInputs inputs, int playerIdx, string playerInputs)
    {
        if (inputs.GPU == "GAME_OVER") return 0;

        int X = inputs.Reg[playerIdx * 2];
        int Y = inputs.Reg[playerIdx * 2 + 1];

        for (int i = 0; i < inputs.GPU.Length && i < playerInputs.Length; i++)
        {
            switch (playerInputs[i])
            {
                case 'U':
                    Y -= inputs.GPU[i] - 48;
                    break;
                case 'D':
                    Y += inputs.GPU[i] - 48;
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
        if (inputs.GPU == "GAME_OVER") return 0;

        int distance = inputs.Reg[playerIdx];
        int risk = inputs.Reg[playerIdx * 2];

        if (risk < 0)
        {
            return 0; //does not matter
        }

        int indexOfInput = inputs.GPU.IndexOf(playerInputs[0]);

        int deltaDistance =
            indexOfInput == 0 ? 1 :
            indexOfInput == 3 ? 3 :
            2;
        risk +=
            indexOfInput == 0 ? -1 :
            indexOfInput == 3 ? 2 :
            1;

        risk = risk < 0 ? 0 : risk;

        if (risk >= 5)
        {
            return _rollerStunnedWeight - deltaDistance;
        }
        else
        {
            return risk - deltaDistance;
        }
    }

    private const int _maxDivingPoints = 120;
    private static double SimulateDiving(GameInputs inputs, int playerIdx, string playerInputs)
    {
        if (inputs.GPU == "GAME_OVER") return 0;

        int points = inputs.Reg[playerIdx];
        int combo = inputs.Reg[playerIdx * 2];

        for (int i = 0; i < inputs.GPU.Length && i < playerInputs.Length; i++)
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

    private static double SimulateAll(List<GameInputs> inputs, int playerIdx, string playerInputs)
    {
        double[] points = new double[4]
        {
            SimulateHurdleRace(inputs[0], playerIdx, playerInputs),
            SimulateArchery(inputs[1], playerIdx, playerInputs),
            SimulateRoller(inputs[2], playerIdx, playerInputs),
            SimulateDiving(inputs[3], playerIdx, playerInputs),
        };

        return points.Sum();
    }

    //private const int _genomeLength = 10;
    //private const int _populationSize = 100;
    //private const double _survivialRate = 0.5;
    //private static string GeneticAlghoritm(List<GameInputs> inputs, int playerIdx)
    //{
    //    var population = InitialPopulation();
    //}

    //private const string _genes = "UDLR";
    //private static List<string> InitialPopulation()
    //{
    //    Random rand = new Random();
    //    return Enumerable.Range(0, _populationSize)
    //        .Select(x =>
    //            string.Join("",Enumerable.Range(0, _genomeLength).Select(g => _genes[rand.Next() % 4])
    //        )
    //    ).ToList();
    //}
}

class GameInputs
{
    public GameInputs(string inputs)
    {
        var splited = inputs.Split(" ");
        GPU = splited[0];
        Reg = splited.Skip(1).Select(c => Int32.Parse(c)).ToList();
    }

    public string GPU { get; private set; }
    public IReadOnlyList<int> Reg { get; private set; }
}