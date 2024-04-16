using System;
using System.Configuration;
using System.Data.SqlClient;

namespace PROCOMER.VUI.Conection
{
    public class cls_Conection
    {
        private string connectionString;
        private SqlConnection conexion;

        public cls_Conection()
        {
            this.connectionString = ConfigurationManager.ConnectionStrings["Conexion"].ConnectionString;
            conexion = new SqlConnection(connectionString);
        }

        public SqlConnection m_ObtenerConexionAmbiente(string pm_Ambiente = null)
        {
            try
            {
                if (conexion.State == System.Data.ConnectionState.Open)
                {
                    conexion.Close();
                }
                else
                {
                    conexion.Open();
                }
            }
            catch (Exception ex)
            {
                // Handle the exception, log, or rethrow if needed.
                return null;
            }

            return conexion;
        }

    }
}
