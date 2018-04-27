namespace Projekt
{
    public class ModelEvaluation
    {
        #region Public properties

        public float TruePositive { get; set; }

        public float TrueNegative { get; set; }

        public float FalsePositive { get; set; }

        public float FalseNegative { get; set; }

        public float FalseNegativeRate { get; set; }

        public float FalsePositiveRate { get; set; }


        public float Recall { get; set; }

        public float Precision { get; set; }

        public float Accuracy { get; set; }

        public float Measure { get; set; }

        #endregion

        #region Public methods

        public void EvaluateResults(TestedGroup group)
        {

            TruePositive = group.CorrectlyAsRobot;
            FalsePositive = group.WronglyAsRobot;

            TrueNegative = group.CorrectlyAsHuman;
            FalseNegative = group.WronglyAsHuman;

            Recall = (float) group.CorrectlyAsRobot / (group.CorrectlyAsRobot + group.WronglyAsHuman);
            Precision =  (float)group.CorrectlyAsRobot / (group.CorrectlyAsRobot + group.WronglyAsRobot);
            Accuracy = (TruePositive + TrueNegative) / (TruePositive + TrueNegative + FalsePositive + FalseNegative);
            Measure = Recall + Precision;

            FalsePositiveRate = 1 - Precision;
            FalseNegativeRate = 1 - Recall;
        }

        #endregion
    }
}