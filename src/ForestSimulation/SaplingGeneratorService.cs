using ForestSimulation.ForestObject;

namespace ForestSimulation
{
    public class SaplingGeneratorService : ISaplingGeneratorService
    {

        public void CreateSapling(IForest forest, int x, int y)
        {
            forest.AddForestObject(new Tree(x, y, this));
        }
    }
}