using System;
using System.Threading;
using ForestSimulation.services;

namespace ForestSimulation
{
    class Program
    {
        static void Main(string[] args)
        {
            var timeKeeper = new TimeKeeper();
            var randomNumberGeneratorService = new RandomNumberGeneratorService(new Random(123456));
            timeKeeper.StartTrackingForest(new Forest(20,randomNumberGeneratorService), randomNumberGeneratorService);

            for (var i = 0; i < 4800; i++)
            {
                timeKeeper.Tick();
                timeKeeper.Forest.Print();

                Console.WriteLine("Year: {0}     Month: {1}       Maulings: {2}", timeKeeper.Year, timeKeeper.Month,
                    timeKeeper.PreviousYearsMaulingCount);

                Thread.Sleep(50);
            }
            Console.ReadLine();
        }
    }
}
