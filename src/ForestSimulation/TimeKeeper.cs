using System;
using System.Linq;
using ForestSimulation.services;

namespace ForestSimulation
{
    public class TimeKeeper
    {
        public int PreviousYearsMaulingCount { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        private IForest _forest;
        private INumberGeneratorService _numberGeneratorService;

        public IForest Forest
        {
            get { return _forest; }
            set { _forest = value; }
        }

        public void StartTrackingForest(IForest forest, INumberGeneratorService numberGeneratorService = null)
        {
            Forest = forest;
            _numberGeneratorService = numberGeneratorService ?? new RandomNumberGeneratorService(new Random());
            Year = 0;
            Month = 1;
            Forest.InitializeForest();
        }

        public void Tick()
        {
            Month++;
            foreach (var forestObject in Forest.ForestObjects.ToList())
            {
                forestObject.Tick(Forest);
            }
            if (Month%12 == 0)
            {
                var lumberCount = Forest.Lumber;
                if (lumberCount < Forest.LumberJacks.Count)
                {
                    if (Forest.LumberJacks.Count > 1)
                    {
                        Forest.Remove(Forest.LumberJacks[_numberGeneratorService.GetNextRandomOfBound(Forest.LumberJacks.Count)]);
                    }
                }
                else if (lumberCount >= Forest.LumberJacks.Count)
                {
                    GenerateLumberJacksFor(lumberCount);
                }

                if (Forest.Maulings == PreviousYearsMaulingCount)
                {
                    Forest.GenerateBear();
                }
                else if (PreviousYearsMaulingCount < Forest.Maulings)
                {
                    if(Forest.Bears.Count>0)
                        Forest.Remove(Forest.Bears[_numberGeneratorService.GetNextRandomOfBound(Forest.Bears.Count)]);
                }
            }
            if (Month <= 12) return;
            Year++;
            Month = 1;
            PreviousYearsMaulingCount = Forest.Maulings - AllPreviousYearsMaulings;
            AllPreviousYearsMaulings += PreviousYearsMaulingCount;
            Forest.Lumber = 0;
        }

        public int AllPreviousYearsMaulings { get; set; }

        private void GenerateLumberJacksFor(int lumberCount)
        {
            if (lumberCount < 10)
            {
                Forest.GenerateLumberJack();
                return;
            }
            for (var i = 0; i < Math.Floor((decimal)lumberCount / 10); i++)
            {
                Forest.GenerateLumberJack();
            }
        }
    }
}