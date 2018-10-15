namespace ForestSimulation.ForestObject
{
    public interface IForestObject
    {
        Point Location { get; set; }
        void Tick(IForest forest);
    }
}