using System.Collections.Generic;

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

        #endregion
    }
}