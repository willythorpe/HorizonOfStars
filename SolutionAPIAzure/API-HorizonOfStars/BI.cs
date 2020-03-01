using System;

namespace API_HorizonOfStars
{
    public class BI
    {
        public int setBusinessInteligence(string nmName, string nmStarship, string nmCapacity, int nmResupply)
        {
            int resultado = 0;
            string dtAgora = String.Format("{0:dd-MM-yyyy HH:mm:ss}", HrBrasilia()); //DateTime.Now

            var conMySql = new Data().conexaoMySql("ConHorizon");
            var comando = new Data().comandoMySql(conMySql, "INSERT INTO horizonofstars (nmName, nmStarship, nmCapacity, nmResupply, dtDate) VALUES('" + nmName + "','" + nmStarship + "','" + nmCapacity + "','" + nmResupply + "','" + dtAgora + "')");

            try
            {
                conMySql.Open();
                comando.ExecuteNonQuery();
                conMySql.Close();

                resultado = 1;

            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                conMySql.Close();
            }

            return resultado;
        }

        public DateTime HrBrasilia()
        {
            DateTime dateTime = DateTime.UtcNow;
            TimeZoneInfo hrBrasilia = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime, hrBrasilia);
        }

    }
}