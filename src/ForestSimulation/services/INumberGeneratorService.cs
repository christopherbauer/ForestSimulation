namespace ForestSimulation.services
{
    public interface INumberGeneratorService
    {
        int GetNextRandomOfBound(int bound);
        int GetNextRandomInRange(int lowBound, int highBound);
    }
}