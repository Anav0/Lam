using System.Collections.Generic;
using Projekt.GUI.UserControls;
using Projekt.ViewModels;

namespace Projekt
{
    public class TestedGroup : Group
    {
        public TestedGroup()
        {
            Sessions = new List<TestedSession>();
        }

        #region Public properties

        public new List<TestedSession> Sessions { get; }

        public DetectionType DetectionMethodSelected { get; set; }

        /// <summary>
        ///     Różnica między LogR i LogH wartości powinny być z przedziału 0,5 a 2
        /// </summary>
        public float Delta { get; set; } = 0.5f;

        public int RobotCount { get; set; }

        public int CorrectlyAsRobot { get; set; }

        public int CorrectlyAsHuman { get; set; }

        public int WronglyAsHuman { get; set; }

        public int WronglyAsRobot { get; set; }

        public int HumanCount { get; set; }

        public int UnknownCount { get; set; }

        public int SumOfflineDetections { get; set; }

        public int SumOnlineDetections { get; set; }

        #endregion

        #region Public methods

        public void AddSession(TestedSession session)
        {
            Sessions.Add(session);
            if (session.RealType == SessionTypes.Robot)
                RobotCount++;
            else if (session.RealType == SessionTypes.Human)
                HumanCount++;
            else
                UnknownCount++;
        }

        /// <summary>
        ///     Notuję informację o wynikach klasyfikacji
        /// </summary>
        /// <param name="session">Sprawdzana sesja</param>
        public void CalculateQuantities(TestedSession session)
        {
            if (session.RealType == SessionTypes.Human && session.PredictedType == session.RealType)
                CorrectlyAsHuman++;
            else if (session.RealType == SessionTypes.Robot && session.PredictedType == session.RealType)
                CorrectlyAsRobot++;
            else if (session.RealType == SessionTypes.Human &&
                     session.PredictedType == SessionTypes.Robot)
                WronglyAsRobot++;
            else if (session.RealType == SessionTypes.Robot &&
                     session.PredictedType == SessionTypes.Human)
                WronglyAsHuman++;

            if (session.DetectionMethodUsed == DetectionType.Offline)
                SumOfflineDetections++;
            else if (session.DetectionMethodUsed == DetectionType.Online) SumOnlineDetections++;
        }

        public ResultsControl AsResultsControl()
        {
            var evaluator = new ModelEvaluation();

            double k = Sessions[0].Kpercent;

            var incorrectHitsProcent = (float)(WronglyAsHuman + WronglyAsRobot) / Sessions.Count * 100;
            var correctHits = 100 - incorrectHitsProcent;
            evaluator.EvaluateResults(this);

            var resultsViewModel = new ResultsViewModel
            {
                FailurePercent = incorrectHitsProcent,
                SuccessPercent = correctHits,
                KPercent = k,
                Delta = Delta,
                SessionsCount = Sessions.Count,
                OnlineMethodUsed = SumOnlineDetections,
                OfflineMethodUsed = SumOfflineDetections,
                MethodSelected = DetectionMethodSelected,
                Recall = evaluator.Recall,
                Precision = evaluator.Precision,
                Measure = evaluator.Measure,
                Accuracy = evaluator.Accuracy,
                TrueNegative = evaluator.TrueNegative,
                TruePositive = evaluator.TruePositive,
                FalsePositive = evaluator.FalsePositive,
                FalseNegative = evaluator.FalseNegative,
                GivenName = GivenName
            };

            return new ResultsControl(resultsViewModel);

        }
        #endregion

        public void ClearResults()
        {
            CorrectlyAsRobot = 0;
            CorrectlyAsHuman = 0;
            WronglyAsRobot = 0;
            WronglyAsHuman = 0;
            HumanCount = 0;
            RobotCount = 0;
            SumOfflineDetections = 0;
            SumOnlineDetections = 0;

        }
    }
}