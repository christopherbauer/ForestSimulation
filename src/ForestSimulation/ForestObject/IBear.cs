using System.Collections.Generic;

namespace ForestSimulation.ForestObject
{
    public interface IBear : IForestObject
    {
        void Tick(IForest forest);
        void Maul(IForest forest, ILumberJack lumberJack);
        Point Location { get; set; }
        List<Point> GetAdjacentSpots(int bound);
    }
}