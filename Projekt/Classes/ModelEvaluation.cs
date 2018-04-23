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

        public float Measure { get; set; }

        #endregion

        #region Public methods

        public float CalculateRecall(TestedGroup group)
        {
            float recall = (float)group.CorrectlyAsRobot / (group.CorrectlyAsRobot + group.WronglyAsHuman);
            TruePositive = group.CorrectlyAsRobot;
            FalseNegative = group.WronglyAsHuman;

            FalseNegativeRate = 1 - recall;
            Recall = recall;

            return recall;
        }

        public float CalculatePrecision(TestedGroup group)
        {
            float precision;

            Precision = precision = (float)group.CorrectlyAsRobot / (group.CorrectlyAsRobot + group.WronglyAsRobot);

            FalsePositive = group.WronglyAsRobot;
            FalsePositiveRate = 1 - precision;
            return precision;
        }

        public float CalculateMeasure()
        {
            return Measure = Recall + Precision;
        }

        #endregion
    }
}
