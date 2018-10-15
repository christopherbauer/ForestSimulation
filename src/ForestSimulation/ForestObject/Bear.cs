using System.Collections.Generic;
using System.Linq;
using ForestSimulation.services;

namespace ForestSimulation.ForestObject
{
    public class Bear : ForestObjectBase, IBear
    {
        public Bear(int x, int y, INumberGeneratorService numberGeneratorService = null) : base(x, y, numberGeneratorService)
        {
        }

        public override void Tick(IForest forest)
        {
            var moves = 0;
            var gridContents = forest.GridContents(Location);
            while (moves < 5 && CheckGridAtLocationFor<ILumberJack>(gridContents))
            {
                var spotPossibilities = GetAdjacentSpots(forest.Bound);
                var desiredSpot = spotPossibilities[NumberGeneratorService.GetNextRandomOfBound(spotPossibilities.Count - 1)];
                Location = desiredSpot;
                moves++;
                gridContents = forest.GridContents(Location);
            }
            if (gridContents != null)
            {
                var lumberJacks = gridContents.Where(o => o is ILumberJack).ToList();
                if (lumberJacks.Any())
                {
                    Maul(forest, (ILumberJack) lumberJacks.First());
                }   
            }
        }

        public void Maul(IForest forest, ILumberJack lumberJack)
        {
            forest.Maulings += 1;
            forest.Remove(lumberJack);
        }
    }
}