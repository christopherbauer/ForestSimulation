using System;
using System.Collections.Generic;
using System.Linq;
using ForestSimulation.services;

namespace ForestSimulation.ForestObject
{
    public class LumberJack : ForestObjectBase, ILumberJack
    {
        private readonly INumberGeneratorService _numberGeneratorService;

        public LumberJack(int x, int y, INumberGeneratorService numberGeneratorService = null) : base(x, y)
        {
            _numberGeneratorService = numberGeneratorService ?? new RandomNumberGeneratorService(new Random());
        }

        public void Tick(IForest forest)
        {
            var moves = 0;
            var gridContents = forest.GridContents(Location);
            while (moves < 3 && CheckGridAtLocationFor<ITree>(gridContents))
            {
                var spotPossibilities = GetAdjacentSpots(forest.Bound);
                var desiredSpot = spotPossibilities[_numberGeneratorService.GetNextRandomOfBound(spotPossibilities.Count - 1)];
                Location = desiredSpot;
                moves++;
                gridContents = forest.GridContents(Location);
            }
            
            if (gridContents == null) return;
            
            var trees = gridContents.Where(o => o is ITree).ToList();
            if (trees.Any())
            {
                CutDown(forest, (ITree) trees.First());
            }
        }

        public void CutDown(IForest forest, ITree tree)
        {
            switch (tree.Age)
            {
                case TreeAge.Tree:
                    forest.Lumber += 1;
                    forest.Remove(tree);
                    break;
                case TreeAge.ElderTree:
                    forest.Lumber += 2;
                    forest.Remove(tree);
                    break;
            }
        }
    }
}