using System;
using System.Configuration;
using System.Data.SqlClient;

namespace PROCOMER.VUI.Conexion
{
    public class cls_Conexion
    {
        private string connectionString;
        private SqlConnection conexion;

        public cls_Conexion()
        {
            this.connectionString = ConfigurationManager.ConnectionStrings["Cadena"].ConnectionString;
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

        // Note: The second m_ObtenerConexionAmbiente method is essentially the same as the first.
        // You may consider removing one of them unless you have a specific reason for having both.

    }
}
