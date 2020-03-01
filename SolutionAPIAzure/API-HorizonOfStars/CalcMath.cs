using System;

namespace API_HorizonOfStars
{
    public class CalcMath
    {
        public int calcMathMGLT(int nuOPPlanet)
        {
            int nuMGLT = 0; 

            int nuOPEarth = 365;
            int nuMGLTEarth = 1; // MGLT distance Earth and Sun

            if (nuOPPlanet > 0)
            {
                nuMGLT = (nuOPPlanet * 100) / nuOPEarth * nuMGLTEarth;
            }

            return (int)Math.Round((float)nuMGLT) / 100;
        }

        public int calcStopResupply(int nuChargeFullSpaceShip, int nuOPPlanet)
        {
            int nuStopResupply = 0;
            int nuMGLT = 0;

            if (nuChargeFullSpaceShip > 0 && nuOPPlanet > 0)
            {
                nuMGLT = calcMathMGLT(nuOPPlanet);
                float db = ((float)nuMGLT / (float)nuChargeFullSpaceShip);
                decimal inteiro = decimal.Ceiling((decimal)db);
                nuStopResupply = (int)Math.Round(inteiro);
            }

            return nuStopResupply;
        }
    }
}