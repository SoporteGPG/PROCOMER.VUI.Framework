using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;


namespace TokenServices
{
    public class cls_Token
    {         


        #region  tareas

        public async Task<Boolean> m_ValidarToken(string pm_Ambiente, string pm_Token)
            {
                Boolean result = false;
                string mensaje = "";
                string Url = string.Empty;

                if (pm_Ambiente == "PROD")
                { Url = "https://bus.procomer.go.cr:7778/JwtApp/api/user/user"; }
                else
                { Url = "https://wsdev.procomer.go.cr:7778/JwtApp/api/user/user"; }

                try
                {

                    var client = new HttpClient();
                    //client.BaseAddress = new Uri(Parametros.UrlBase + Parametros.MetodoIngresarCVO + Parametros.Key);
                    client.BaseAddress = new Uri(Url);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));


                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", pm_Token);
                    //string json = await client.GetAsync(DES_Url);

                    var response = await client.GetAsync(Url);

                    string validacion = "";
                    validacion = response.ToString();

                    if (response.ReasonPhrase.ToString() == "OK")
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }

                    return result;
                }
                catch (Exception ex)
                {

                    mensaje = "Datos no obtenidos";
                    return result;
                }
            }

            public async Task<string> m_Api_ObtenerToken(string pm_Ambiente, string pm_Usuario, string pm_Clave)
            {
                string Url = string.Empty;
                string Token = string.Empty;

                if (pm_Ambiente == "PROD")
                { Url = "https://bus.procomer.go.cr:7778/JwtApp/api/login/loginById"; }
                else
                { Url = "https://wsdev.procomer.go.cr:7778/JwtApp/api/login/loginById"; }

                try
                {

                    var client = new HttpClient();
                    //client.BaseAddress = new Uri(Parametros.UrlBase + Parametros.MetodoIngresarCVO + Parametros.Key);
                    client.BaseAddress = new Uri(Url);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
                    //client.DefaultRequestHeaders.Add("token", Token);

                    cls_ObtenerToken parametros = new cls_ObtenerToken
                    {
                        IdInstitucion = pm_Usuario,
                        NombreServicio = pm_Clave
                    };


                    var jsonTask = JsonConvert.SerializeObject(parametros);

                    var response = await client.PostAsync(string.Empty, new StringContent(jsonTask, Encoding.UTF8, "application/json"));

                    var resultJSON = await response.Content.ReadAsStringAsync();
                    var resultvf = JsonConvert.DeserializeObject<string>(resultJSON);

                    Token = resultvf.ToString();
                    Token = response.RequestMessage.ToString();

                    if (response.ReasonPhrase == "Unauthorized")
                    {
                        Token = "Unauthorized";

                    }
                    else
                    {
                        Token = resultvf.ToString();
                    }

                    return Token;

                }
                catch (Exception ex)
                {

                    return "Error" + ex;
                }
            }
        #endregion

        #region funciones
        public string ObtenerToken(string pm_Usuario, string pm_Clave, string pm_Ambiente)
            {
                string Token = "vacio";
                string mensaje;
                bool validez;
                try
                {
                    SqlCommand command = new SqlCommand();
                    cls_ConexionToken objConexion = new cls_ConexionToken();
                    SqlDataReader dr = null;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "SP_Verificar";
                    command.Connection = objConexion.m_ObtenerConexionAmbiente();
                    command.Parameters.AddWithValue("@Usuario", pm_Usuario);
                    command.Parameters.AddWithValue("@Contraseña", pm_Clave);
                    command.ExecuteNonQuery();
                    dr = command.ExecuteReader();
                    dr.Read();
                    mensaje = dr["Mensaje"].ToString();
                    validez = Convert.ToBoolean(dr["valido"].ToString());

                    objConexion.m_ObtenerConexionAmbiente().Close();

                    if (mensaje.Equals("Credenciales válidas.") && validez == true)
                    {
                        if (pm_Ambiente == "PROD")
                        {
                            pm_Usuario = "191";
                            pm_Clave = "VUI";
                            var Task_Token = Task.Run(() => m_Api_ObtenerToken(pm_Ambiente, pm_Usuario, pm_Clave));
                            Task_Token.Wait();
                            Token = Task_Token.Result.ToString();
                        }
                        else
                        { Token = "Pruebas"; }

                    }
                    else
                    {
                        Token = "Credenciales No válidas";
                    }
                    return Token;
                }
                catch (Exception ex)
                {
                    return "Error:" + ex;
                }
            }

            public string ValidarToken(string pm_Ambiente, string pm_Token)
            {
                string mensaje = string.Empty;
                Boolean Token = false;
                try
                {
                    if (pm_Ambiente == "PROD")
                    {
                        var Task_Token = Task.Run(() => m_ValidarToken(pm_Ambiente, pm_Token));
                        Task_Token.Wait();
                        Token = Task_Token.Result;
                        if (Token != false)
                        {
                            mensaje = Token.ToString();
                        }
                    }
                    else
                    if (pm_Token == "Pruebas")
                    { Token = true; }

                    mensaje = Token.ToString();
                    return mensaje;
                }

                catch (Exception ex)
                {

                    mensaje = "Error: " + ex;
                    return mensaje;
                }
            }
        #endregion

        #region clase
        class cls_ObtenerToken
            {
                public string IdInstitucion { get; set; }
                public string NombreServicio { get; set; }
            }

        public class cls_ConexionToken
        {
            private string connectionString;
            private SqlConnection conexion;

            public cls_ConexionToken()
            {
                this.connectionString = ConfigurationManager.ConnectionStrings["ConexionValidar"].ConnectionString;
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
        #endregion

    }

}
