namespace ForestSimulation.ForestObject
{
    public interface ITree : IForestObject
    {
        TreeAge Age { get; set; }
        Point Location { get; set; }
        void Tick(IForest forest);
    }
}