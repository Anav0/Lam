using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projekt.Classes;

namespace Projekt
{
    public class ModelEvaluation
    {

        public float TruePositive { get; set; }

        public float TrueNegative { get; set; }

        public float FalsePositive { get; set; }

        public float FalseNegative { get; set; }

        public float FalseNegativeRate { get; set; }

        public float FalsePositiveRate { get; set; }


        public float Recall { get; set; }

        public float Precision { get; set; }

        public float Measure { get; set; }

        public float CalculateRecall(SessionsGroup group)
        {
            float recall = 0;
            
            recall = (float)group.RobotCount / (group.CorrectlyAsRobot + group.WronglyAsHuman);
            TruePositive = group.CorrectlyAsRobot;
            FalseNegative = group.WronglyAsHuman;

            FalseNegativeRate = 1 - recall;
            Recall = recall;

            return recall;
        }

        public float CalculatePrecision(SessionsGroup group)
        {
            float precision = 0;

            Precision = precision = (float)group.RobotCount / (group.CorrectlyAsRobot + group.WronglyAsRobot);

            FalsePositive = group.WronglyAsRobot;
            FalsePositiveRate = 1 - precision;
            return precision;
        }

        public float CalculateMeasure()
        {
            return Measure = Recall + Precision;
        }
    }
}
