using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.ComponentModel;




namespace DataMininglab3
{

    public sealed partial class MainPage : Page
    {
        private static int DIMENSION = 150;
        private static int border = 0;
        private static int width = 4;

        private List<List<double>> TrainingSet = new List<List<double>>();
        private List<List<double>>  ResultSet = new List<List<double>>();

        private Random rnd = new Random();
        private SOM sm;
       
      
        private static DispatcherTimer dTimer;

        public MainPage()
        {
            this.InitializeComponent();



            GenerateTrainingSet();

            sm = new SOM(DIMENSION);
            ResultSet = sm.resultLL;//sm.DumpWeights(ResultSet);

            

            // таймер обновления экрана 5fps
            dTimer = new DispatcherTimer();
            dTimer.Tick += DrawdispatcherTimer_Tick;
            dTimer.Interval = new TimeSpan(0, 0, 0,0,200);

            dTimer.Start();


            // Пускай работает в другом треде
            BackgroundWorker bw = new BackgroundWorker();

            bw.DoWork += EpochCaller;
            bw.RunWorkerAsync();
        }

        // то что работает в другом треде
        private void EpochCaller(object sender, DoWorkEventArgs e)
        {
            while (!sm.done)
            {
                sm.Epoch(TrainingSet);
            }
        }


        // косвенный вызов перерисовки
        private void DrawdispatcherTimer_Tick(object sender, object e)
        {
            
            Iterations_Label.Text = "Итерация -"+sm.getIteration().ToString();
            //front_canvas.Invalidate(); 
            result_canvas.Invalidate();
            
        }
   // Генерация тестовых наборов
        private void GenerateTrainingSet()
        {
            for (int x = 0; x < DIMENSION*DIMENSION; x++)
            {
                List<double> tmp = new List<double>();
                
                if(x<((DIMENSION * DIMENSION) /10))
                {
                    tmp.Add(1);
                    tmp.Add(0);
                    tmp.Add(0);
                }
                else if(x < (2*(DIMENSION * DIMENSION) / 10))
                {
                    tmp.Add(0);
                    tmp.Add(1);
                    tmp.Add(0);
                }
                else if (x < (3 * (DIMENSION * DIMENSION) / 10))
                {
                    tmp.Add(0);
                    tmp.Add(1);
                    tmp.Add(1);
                }
                else if (x < (4 * (DIMENSION * DIMENSION) / 10))
                {
                    tmp.Add(0);
                    tmp.Add(1);
                    tmp.Add(0.5);
                }
                else if (x < (5 * (DIMENSION * DIMENSION) / 10))
                {
                    tmp.Add(0);
                    tmp.Add(0);
                    tmp.Add(1);
                }
                else if (x < (6 * (DIMENSION * DIMENSION) / 10))
                {
                    tmp.Add(0.9);
                    tmp.Add(0.3);
                    tmp.Add(0);
                }
                else if (x < (7 * (DIMENSION * DIMENSION) / 10))
                {
                    tmp.Add(0.1);
                    tmp.Add(0.7);
                    tmp.Add(0.2);
                }
                else if (x < (8 * (DIMENSION * DIMENSION) / 10))
                {
                    tmp.Add(0.5);
                    tmp.Add(0.22);
                    tmp.Add(0);
                }
                else if (x < (9 * (DIMENSION * DIMENSION) / 10))
                {
                    tmp.Add(0.5);
                    tmp.Add(0.5);
                    tmp.Add(0.3);
                }
                else
                {
                    tmp.Add(1);
                    tmp.Add(0);
                    tmp.Add(1);
                }


                TrainingSet.Add(tmp);
                tmp = new List<double>(3);
                tmp.Add(0);
                tmp.Add(0);
                tmp.Add(0);
                ResultSet.Add(tmp);
            }
        }

        private void GenerateTrainingSet2()
        {
            for (int x = 0; x < DIMENSION * DIMENSION; x++)
            {
                List<double> tmp = new List<double>();

                int choice = rnd.Next(10);

                switch (choice)
                {
                    case 0:
                        tmp.Add(1);
                        tmp.Add(0);
                        tmp.Add(0);
                        break;
                    case 1:
                        tmp.Add(0);
                        tmp.Add(1);
                        tmp.Add(0);
                        break;
                    case 2:
                        tmp.Add(0);
                        tmp.Add(1);
                        tmp.Add(1);
                        break;
                    case 3:
                        tmp.Add(1);
                        tmp.Add(0.5);
                        tmp.Add(0.2);
                        break;
                    case 4:
                        tmp.Add(0);
                        tmp.Add(0);
                        tmp.Add(1);
                        break;
                    case 5:
                        tmp.Add(0);
                        tmp.Add(1);
                        tmp.Add(0);
                        break;
                    case 6:
                        tmp.Add(0);
                        tmp.Add(1);
                        tmp.Add(0.1);
                        break;
                    case 7:
                        tmp.Add(0);
                        tmp.Add(1);
                        tmp.Add(0.5);
                        break;
                    case 8:
                        tmp.Add(0.3);
                        tmp.Add(0);
                        tmp.Add(0);
                        break;
                    case 9:
                        tmp.Add(0);
                        tmp.Add(1);
                        tmp.Add(0.7);
                        break;

                }


                TrainingSet.Add(tmp);
                tmp = new List<double>(3);
                tmp.Add(0);
                tmp.Add(0);
                tmp.Add(0);
                ResultSet.Add(tmp);
            }
        }

        //отрисовка тестового набора
        private void canvas_Draw(
    Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender,
    Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs args)
        {
            for (int x = 0; x < DIMENSION; x++)
            {
                for (int y = 0; y < DIMENSION; y++)
                {

                    
                    args.DrawingSession.DrawRectangle(x* width, y* width, width, width, Color.FromArgb(254,(byte)(Convert.ToInt32(TrainingSet[x *  DIMENSION + y ][0] * 254) ),
                                                                                     (byte)(Convert.ToInt32(TrainingSet[x * DIMENSION + y][1] * 254) ),
                                                                                     (byte)(Convert.ToInt32(TrainingSet[x * DIMENSION + y][2] * 254))
                                                                                     ),width);
                }
            }

        }

        //отрисовка весов карты
        private void canvas_Draw_Result(
     Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender,
     Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs args)
        {
           
            if (ResultSet != null)
            {
                for (int x = 0; x < DIMENSION; x++)
                {
                    for (int y = 0; y < DIMENSION; y++)
                    {


                        args.DrawingSession.DrawRectangle(x * width, y * width, width, width, Color.FromArgb(254, (byte)(Convert.ToInt32(ResultSet[x * DIMENSION + y][0] * 254) ),
                                                                                         (byte)(Convert.ToInt32(ResultSet[x * DIMENSION + y][1] * 254) ),
                                                                                         (byte)(Convert.ToInt32(ResultSet[x * DIMENSION + y][2] * 254) )
                                                                                         ), width);
                    }
                }
            }

        }


        // убить за собой Win2D
        void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            front_canvas.RemoveFromVisualTree();
            front_canvas = null;

            result_canvas.RemoveFromVisualTree();
            result_canvas = null;
        }

        
    }
}
