using ENT;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ManejadoraPuntuacionApi
    {
        private static string urlBase = "https://apipuntuacionpkm.azurewebsites.net/Api/Puntuacion";

        /// <summary>
        /// Obtiene un listado completo de puntuaciones desde la API ordenado de mayor a menor
        /// </summary>
        /// <returns>Lista de puntuaciones</returns>
        public static async Task<List<clsPuntuacion>> getListadoPuntuacionesAsync()
        {
            Uri requestUri = new Uri($"{urlBase}");

            List<clsPuntuacion> puntuaciones = new List<clsPuntuacion>();

  
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(requestUri);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        puntuaciones = JsonConvert.DeserializeObject<List<clsPuntuacion>>(jsonResponse);
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        throw new Exception("404");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        throw new Exception("400");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener la lista de puntuaciones", ex);
                }
            }

            return puntuaciones;
        }
        /// <summary>
        /// Añade una nueva puntuacion a través de la API
        /// </summary>
        /// <param name="nuevaPuntuacion"></param>
        /// <returns>Código de estado HTTP de la respuesta</returns>
        public static async Task<int> AgregarPuntuacionAsync(clsPuntuacion nuevaPuntuacion)
        {
            Uri requestUri = new Uri($"{urlBase}");
            int filasAfectadas = 0;

            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    string jsonData = JsonConvert.SerializeObject(nuevaPuntuacion);
                    HttpContent content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await httpClient.PostAsync(requestUri, content);
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    //filasAfectadas = (int)response.StatusCode;

                    if (response.IsSuccessStatusCode)
                    {
                        filasAfectadas = Int32.Parse(jsonResponse);
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        throw new Exception("404");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        throw new Exception("400");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al agregar la persona", ex);
                }
                return filasAfectadas;
            }
        }
    }
}
