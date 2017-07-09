using System;
using System.Collections.Generic;

namespace DataMininglab3
{
    class SOM
    {
        List<SomNode> nodes =  new List<SomNode>();

        SomNode  Winning; // Текущая BMU

        double MapRadius; // начальный радиус

        double TimeConstant; // постоянная времени

        int Iterations = 10000 ; // Колличество итераций

        int CurrentIteration; // осталось итераций 

        double NeoghborhoodRadius; // радиус влияния BMU
        // сила притяжения к  BMU
        double influence; 

        double StartingLearningRate = 0.1; // начальный коэффицент обучения
        // текущий коэффицент обучения
        double CurrentLearningRate; // текущий коэффицент обучения

        public bool done;  // готово 

        int Dimension; // сторона квадрата

        int DepthDimension = 3; // глубина параметров

        public List<List<double>> resultLL = null;


        Random rnd = new Random();

        public SOM(int WIDTH)
        {
            CurrentIteration = Iterations;
            Dimension = WIDTH;
            for (int row = 0; row < Dimension; ++row)
            {
                for (int col = 0; col < Dimension; ++col)
                {
                    SomNode tmp = new SomNode(row, col, DepthDimension);
                    nodes.Add(tmp);

                    
                }
            }

            MapRadius =(2* Dimension) / 3;

            TimeConstant = CurrentIteration / Math.Log(MapRadius);
            makeResRef();
        }


        // Итерации от 0 до количества итераций
        public int getIteration()
        {
            return (Iterations - CurrentIteration);
        }


        // создаем ссылку для отображения
        public void makeResRef()
        {
            resultLL = new List<List<double>>();
            for (int i = 0; i < Dimension * Dimension; i++)
            {
                resultLL.Add(nodes[i].weights);
            }
        }


        // Одна итерация по тестовому набору
        public bool Epoch(List<List <double>> inpt)
        {
            if (done)
            {
                return true;
            }

            if (--CurrentIteration > 0)
            {
                int ThisList;

               switch((Iterations - CurrentIteration)) //растягиваем по углам первые итерации что бы было нагляднее при монотонном базовом весе
                {                                      
                    case 0:
                        ThisList = 0;
                        break;
                    case 1:
                        ThisList = Dimension;
                        break;
                    case 2:
                        ThisList = (Dimension*Dimension)/2;
                        break;
                    case 3:
                        ThisList = (Dimension * Dimension)-1;
                        break;
                    default:
                        ThisList = rnd.Next(inpt.Count);
                        break;
                }
                
                // Ищем наиболее похожего
                Winning = BMU(inpt[ThisList]);


                // Постепенно уменьшаем радиус []
                NeoghborhoodRadius = MapRadius * Math.Exp(-(double)(Iterations - CurrentIteration) / TimeConstant);


                // Притеягиваем соседей в радиусе к BMU
                for (int i = 0; i < nodes.Count; i++)
                {
                    double DistBMUSQ = (Winning.X - nodes[i].X) *
                                       (Winning.X - nodes[i].X) +
                                       (Winning.Y - nodes[i].Y) *
                                       (Winning.Y - nodes[i].Y);

                    double WidthSq = NeoghborhoodRadius * NeoghborhoodRadius;

                    if (DistBMUSQ < (WidthSq))
                    {
                        influence = Math.Exp(-(DistBMUSQ) / (2 * WidthSq));

                        nodes[i].AdjustWeights(inpt[ThisList], CurrentLearningRate, influence);
                    }

                   

                   
                }
                
                CurrentLearningRate = StartingLearningRate * Math.Exp(-(double)(Iterations - CurrentIteration) / CurrentIteration);
            } else
            {
                done = true;
            }
                return true;
        }

        // ищем наиболее похожего через минимальное расстояние по евклиду
        SomNode BMU(List<double> inpt)
        {
            SomNode winner = null;

            double LowestDistance = 1000000;

            for( int i = 0; i <nodes.Count; i++)
            {
                double dist = nodes[i].GetDistanceEuclidean(inpt);

                if(dist < LowestDistance)
                {
                    LowestDistance = dist;
                    winner = nodes[i];
                }
            }

            return winner;
        }
    }
}
