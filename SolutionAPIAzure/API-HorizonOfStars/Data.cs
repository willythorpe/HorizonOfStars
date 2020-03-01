using MySql.Data.MySqlClient;
using FirebirdSql.Data.FirebirdClient;

namespace API_HorizonOfStars
{
    public class Data
    {
            public MySqlConnection conexaoMySql(string conexao)
            {
                MySqlConnection conn = new MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[conexao].ConnectionString);

                return conn;
            }

            public MySqlCommand comandoMySql(MySqlConnection conn, string comando)
            {
                MySqlCommand cmd = new MySqlCommand(comando, conn);

                return cmd;
            }

            public FbConnection conexaoFirebase(string conexao)
            {
                FbConnection conn = new FbConnection(System.Configuration.ConfigurationManager.ConnectionStrings[conexao].ConnectionString);
                return conn;
            }

    }
}