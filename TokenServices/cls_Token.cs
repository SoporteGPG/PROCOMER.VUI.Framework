using Newtonsoft.Json;
using PROCOMER.VUI.Conexion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TokenServices
{
    public class cls_Token
    {
        public Boolean m_ValidarTokenOtro(string pm_Ambiente, string pm_Token)
        {
            Boolean result = false;
            string mensaje = "";
            //string DES_Url = "http://20.253.152.143:7778/JwtApp/api/user/user";
            string DES_Url = "https://wsdev.procomer.go.cr:7778/JwtApp/api/user/user";

            string PROD_Url = "";
            //se ejecuta como un api
            //ejecuta el método http://20.253.152.143:7778/JwtApp/api/user/user

            try
            {
                if (pm_Ambiente == "PROD")
                {
                    string validacion = "";

                    if (validacion != "")
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }

                }
                else
                {
                    string validacion = "";
                    if (validacion != "")
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }


                return result;
            }
            catch (Exception ex)
            {
                mensaje = "Datos no obtenidos";

                return result;
            }
        }

        public async Task<Boolean> m_ValidarToken(/*string pm_Ambiente, */string pm_Token)
        {
            Boolean result = false;
            string mensaje = "";
            //string DES_Url = "http://20.253.152.143:7778/JwtApp/api/user/user";
            string DES_Url = "https://wsdev.procomer.go.cr:7778/JwtApp/api/user/user";
            string PROD_Url = "https://bus.procomer.go.cr:7778/JwtApp/api/user/user";
            //se ejecuta como un api
            //ejecuta el método http://20.253.152.143:7778/JwtApp/api/user/user

            try
            {
                //if (pm_Ambiente == "PROD")
                //{
                    var client = new HttpClient();
                    //client.BaseAddress = new Uri(Parametros.UrlBase + Parametros.MetodoIngresarCVO + Parametros.Key);
                    client.BaseAddress = new Uri(PROD_Url);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    //var jsonTask = JsonConvert.SerializeObject(parametros);
                    // Use the access token to call a protected web API.

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", pm_Token);
                    //string json = await client.GetAsync(DES_Url);

                    var response = await client.GetAsync(DES_Url);

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

                //}
                //else
                //{
                //    var client = new HttpClient();
                //    //client.BaseAddress = new Uri(Parametros.UrlBase + Parametros.MetodoIngresarCVO + Parametros.Key);
                //    client.BaseAddress = new Uri(DES_Url);
                //    client.DefaultRequestHeaders.Accept.Clear();
                //    client.DefaultRequestHeaders.Accept.Add(
                //        new MediaTypeWithQualityHeaderValue("application/json"));

                //    //var jsonTask = JsonConvert.SerializeObject(parametros);
                //    // Use the access token to call a protected web API.

                //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", pm_Token);
                //    //string json = await client.GetAsync(DES_Url);

                //    var response = await client.GetAsync(DES_Url);

                //    string validacion = "";
                //    validacion = response.ToString();

                //    if (response.ReasonPhrase.ToString() == "OK")
                //    {
                //        result = true;
                //    }
                //    else
                //    {
                //        result = false;
                //    }
                //}


                return result;
            }
            catch (Exception ex)
            {


                mensaje = "Datos no obtenidos";

                return result;
            }
        }

        public async Task<string> m_Api_ObtenerToken(/*string pm_Ambiente, */string pm_Usuario, string pm_Clave)
        {
            string Token = "";
            string mensaje = "";
            //string DES_Url = "http://20.253.152.143:7778/JwtApp/api/login/loginById";
            string DES_Url = "https://wsdev.procomer.go.cr:7778/JwtApp/api/login/loginById";
            string PROD_Url = "https://bus.procomer.go.cr:7778/JwtApp/api/login/loginById";
            cls_Token result = new cls_Token();
            try
            {
                //pm_Usuario = "";
                //pm_Clave= "";
                //if (pm_Ambiente == "PROD")
                //{

                    var client = new HttpClient();
                    //client.BaseAddress = new Uri(Parametros.UrlBase + Parametros.MetodoIngresarCVO + Parametros.Key);
                    client.BaseAddress = new Uri(PROD_Url);
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

                    var response = await client.PostAsync(string.Empty, new StringContent(jsonTask, Encoding.UTF8, "ContentTypeJSON"));

                    var resultJSON = await response.Content.ReadAsStringAsync();
                    var resultvf = JsonConvert.DeserializeObject<string>(resultJSON);

                    Token = resultvf.ToString();
                    Token = response.RequestMessage.ToString();
                    mensaje = "Datos obtenidos";
                    if (response.ReasonPhrase == "Unauthorized")
                    {
                        Token = "Unauthorized";

                    }
                    else
                    {
                        //Token = response.Content.ToString();
                        Token = resultvf.ToString();
                    }
                //}
                //else
                //{
                    //var client = new HttpClient();
                    ////client.BaseAddress = new Uri(Parametros.UrlBase + Parametros.MetodoIngresarCVO + Parametros.Key);
                    //client.BaseAddress = new Uri(DES_Url);
                    //client.DefaultRequestHeaders.Accept.Clear();
                    //client.DefaultRequestHeaders.Accept.Add(
                    //    new MediaTypeWithQualityHeaderValue("application/json"));
                    ////client.DefaultRequestHeaders.Add("token", Token);



                    //cls_ObtenerToken parametros = new cls_ObtenerToken
                    //{
                    //    IdInstitucion = pm_Usuario,
                    //    NombreServicio = pm_Clave
                    //};


                    //var jsonTask = JsonConvert.SerializeObject(parametros);

                    //var response = await client.PostAsync(string.Empty, new StringContent(jsonTask, Encoding.UTF8, "ContentTypeJSON"));

                    //var resultJSON = await response.Content.ReadAsStringAsync();
                    //var resultvf = JsonConvert.DeserializeObject<string>(resultJSON);

                    //Token = resultvf.ToString();

                    //mensaje = "Datos obtenidos";
                    //if (response.ReasonPhrase == "Unauthorized")
                    //{
                    //    Token = "Unauthorized";

                    //}
                    //else
                    //{
                    //    Token = resultvf.ToString();
                    //}
                //}
                return Token;

            }
            catch (Exception ex)
            {


                mensaje = "Datos no obtenidos";

                return pm_Usuario;
            }
        }
        public string ObtenerToken(string pm_Usuario, string pm_Clave)
        {
            string Token = "";
            string mensaje;
            bool validez;
            try
            {
                SqlCommand command = new SqlCommand();
                cls_Conexion objConexion = new cls_Conexion();
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
                    pm_Usuario = "191";
                    pm_Clave = "VUI";
                    var Task_Token = Task.Run(() => m_Api_ObtenerToken(/*pm_Ambiente, */pm_Usuario, pm_Clave));
                    Task_Token.Wait();

                    Token = Task_Token.Result;
                    return Token;
                }
                else
                {
                    return Token;
                }
            }
            catch (Exception ex)
            {
                return Token;
            }
        }

        public Boolean ValidarToken(/*string pm_Ambiente, */string pm_Token)
        {
            string mensaje = "";

            try
            {
                var Task_Token = Task.Run(() => m_ValidarToken(/*pm_Ambiente, */pm_Token));
                Task_Token.Wait();

                Boolean Token = Task_Token.Result;

                if (Token != false)
                {
                    mensaje = "Datos obtenidos";
                }
                else
                {
                    mensaje = "Datos no obtenidos";
                }
                return Token;
            }

            catch (Exception ex)
            {
                Boolean Token = false;
                mensaje = "Error: " + ex;
                return Token;
            }
        }

    }
}
