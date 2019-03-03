using System;

/// <summary>
/// Torstein Bratbergs code library
/// </summary>
/// 
namespace MyLib
{
    public enum AngleType{
        AbsRad = 0,
        AbsDeg = 1,
        RelRad = 2, //Does this make sence without knowledge of boat?
        RelDeg = 3 //Does this make sence without knowledge of boat?

    }
    public class T_Angle
    {
        public float AngAbsRad { get; set; } //Make explicit setter that updates the others?
        public float AngAbsDeg { get; set; }
        public float AngRelRad { get; set; }
        public float AngRelDeg { get; set; }

        /// <summary>
        /// Constructor that enterprets angle argument based on supplied angleType. Relative angles not implemented
        /// </summary>
        public T_Angle(float angle, AngleType angleType)
        {
            switch (angleType)
            {
                case AngleType.AbsRad:
                    AngAbsRad = angle;
                    AngAbsDeg = (float) ( AngAbsRad / ( 2 * Math.PI) * 360f );
                    break;
                case AngleType.AbsDeg:
                    AngAbsDeg = angle;
                    AngAbsRad = (float)( AngAbsDeg / 360f * (2 * Math.PI) );
                    break;
                case AngleType.RelRad:
                    throw new Exception("T_Angle: RelAbs not implemented. Does it even make sence?");
                    break;
                case AngleType.RelDeg:
                    throw new Exception("T_Angle: RelDeg not implemented. Does it even make sence?");
                    break;
                default:
                    break;
            }
        }
    }
}
