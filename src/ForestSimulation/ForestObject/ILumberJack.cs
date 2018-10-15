using System.Collections.Generic;

namespace ForestSimulation.ForestObject
{
    public interface ILumberJack : IForestObject
    {
        void Tick(IForest forest);
        void CutDown(IForest forest, ITree tree);
        List<Point> GetAdjacentSpots(int bound);
    }
}