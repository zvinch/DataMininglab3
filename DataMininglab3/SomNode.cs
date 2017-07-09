using System;
using System.Collections.Generic;
namespace DataMininglab3
{
    class SomNode
    {
        public List<double> weights = new List<double>();
        public double X;
        public double Y;
        private static Random rnd = new Random();

        public SomNode(double x, double y, double dimensions)
        {
            X = x;
            Y = y;
            for (int i = 0; i < dimensions; i++)
            {
                weights.Add(0);
            }

        }
        

        // просто евклидово расстояние для параметров
        public double GetDistanceEuclidean(List<double> InputVector)
        {
            double dist = 0;
            for (int i = 0; i < weights.Count; ++i)
            {
                dist += (InputVector[i] - weights[i]) * (InputVector[i] - weights[i]);
            }


            return dist;
        }
        // правило обновления [http://users.ics.aalto.fi/jhollmen/dippa/node20.html#SECTION00523100000000000000]
        public void AdjustWeights(List<double> target, double LearningRate, double influence)
        {
            for( int i = 0; i < target.Count; i++)
            {

                
                weights[i] += LearningRate * influence * (target[i] - weights[i]);
            }
        }


    };
}