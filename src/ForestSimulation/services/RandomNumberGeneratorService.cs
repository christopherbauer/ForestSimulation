using System;

namespace ForestSimulation.services
{
    public class RandomNumberGeneratorService : INumberGeneratorService
    {
        private static Random _random;

        public RandomNumberGeneratorService(Random random)
        {
            _random = random;
        }

        public int GetNextRandomOfBound(int bound)
        {
            return _random.Next(bound);
        }

        public int GetNextRandomInRange(int lowBound, int highBound)
        {

            return _random.Next(lowBound, highBound);
        }
    }
}