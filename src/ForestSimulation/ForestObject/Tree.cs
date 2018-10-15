using ForestSimulation.services;

namespace ForestSimulation.ForestObject
{
    public class Tree : ForestObjectBase, ITree
    {
        private readonly ISaplingGeneratorService _saplingGeneratorService;

        public Tree(int x, int y, ISaplingGeneratorService saplingGeneratorService, TreeAge treeAge = TreeAge.Sapling, INumberGeneratorService numberGeneratorService = null) : base(x, y, numberGeneratorService)
        {
            Age = treeAge;
            _saplingGeneratorService = saplingGeneratorService;
        }

        public TreeAge Age { get; set; }

        private int _monthsOld;

        public void Tick(IForest forest)
        {
            _monthsOld++;
            
            if (Age == TreeAge.Sapling)
            {
                if (_monthsOld > 12)
                {
                    Age = TreeAge.Tree;
                }
                else
                {
                    return;
                }
            }

            if (Age == TreeAge.Tree && _monthsOld > 120)
            {
                Age = TreeAge.ElderTree;
            }


            if ((Age == TreeAge.Tree && NumberGeneratorService.GetNextRandomInRange(1, 10) == 1) || //Flat 10% chance
                Age == TreeAge.ElderTree && NumberGeneratorService.GetNextRandomInRange(1, 10) <= 2) //Flat 20% chance
            {
                var assigned = false;
                var spotPossibilities = GetAdjacentSpots(forest.Bound);
                while (!assigned && spotPossibilities.Count > 0)
                {
                    var spotIndex = NumberGeneratorService.GetNextRandomOfBound(spotPossibilities.Count);
                    var checkSpot = spotPossibilities[spotIndex];
                    spotPossibilities.RemoveAt(spotIndex);

                    if (forest.IsOpenSpot(checkSpot.X, checkSpot.Y))
                    {
                        assigned = true;
                        _saplingGeneratorService.CreateSapling(forest, checkSpot.X, checkSpot.Y);
                    }
                }
            }
        }
    }
}