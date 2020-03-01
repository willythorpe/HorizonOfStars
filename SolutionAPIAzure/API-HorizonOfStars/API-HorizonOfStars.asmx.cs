using System;
using System.Web.Services;

namespace API_HorizonOfStars
{
    /// <summary>
    /// Descrição resumida de API_HorizonOfStars
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que esse serviço da web seja chamado a partir do script, usando ASP.NET AJAX, remova os comentários da linha a seguir. 
    // [System.Web.Script.Services.ScriptService]
    public class API_HorizonOfStars : System.Web.Services.WebService
    {
        [WebMethod]
        public string GetAllPlanets()
        {
            try { 
                Swapi sw = new Swapi();
                return sw.GetAllPlanets().ToString();
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        [WebMethod]
        public string GetAllStarships()
        {
            try
            {
                Swapi sw = new Swapi();
                return sw.GetAllSpaceships().ToString();
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        [WebMethod]
        public string GetFilterStarship(string nmNameSpaceship, int nuOPPlanet)
        {
            try
            {
                Swapi sw = new Swapi();
                return sw.GetRequestQueryStarship(nmNameSpaceship, nuOPPlanet);
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        [WebMethod]
        public int GetCalcMathMGLT(int nuOPPlanet)
        {
            try {
            CalcMath cm = new CalcMath();
            return cm.calcMathMGLT(nuOPPlanet);
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        [WebMethod]
        public int calcStopResupply(int nuChargeFullSpaceShip, int nuOPPlanet)
        {
            int nuTimesResupply = 0;

            try {         

                if (nuChargeFullSpaceShip > 0 && nuOPPlanet > 0)
                {
                    CalcMath cm = new CalcMath();
                    nuTimesResupply = cm.calcStopResupply(nuChargeFullSpaceShip, nuOPPlanet);
                }            
            }
            catch (Exception e)
            {
                throw (e);
            }

            return nuTimesResupply;
        }

        [WebMethod]

        public int insertBI(string nmName, string nmStarship, string nmCapacity, int nmResupply)
        {
            int resultBI = 0;

            try
            {
                if (nmName != null && nmStarship != null)
                {
                    BI bi = new BI();
                    resultBI = bi.setBusinessInteligence(nmName, nmStarship, nmCapacity, nmResupply);
                }
            }
            catch (Exception e)
            {
                throw (e);
            }

            return resultBI;
        }

    }

}
