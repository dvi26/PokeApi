using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using ENT;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Services
{
    public class ManejadoraPokeApi
    {
        private static string urlBase = "https://pokeapi.co/api/v2/pokemon/";

        /// <summary>
        /// Obtiene un Pokémon aleatorio desde la API de PokeAPI, recive la id del Pokémon como parámetro.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Pokemon aleatorio desde la api</returns>
        /// <exception cref="Exception"></exception>
        public static async Task<clsPokemon> GetPokemonRandomAsync(int id)
        {
            using HttpClient httpClient = new HttpClient();

            HttpResponseMessage response = await httpClient.GetAsync(urlBase + $"{id}");

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new Exception("404: Pokémon no encontrado");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception("400: Solicitud incorrecta");
                }
                throw new Exception("Otro error");
            }
            else
            {
                string json = await response.Content.ReadAsStringAsync();
                JObject data = JObject.Parse(json);

                string nombre = (string)data["name"];

                string foto = "";

                // Intenta dream_world
                //foto = (string?)data["sprites"]?["other"]?["dream_world"]?["front_default"];

                // Si no hay, intenta official-artwork
                if (string.IsNullOrEmpty(foto))
                {
                    foto = (string?)data["sprites"]?["other"]?["official-artwork"]?["front_default"];
                }

                // Si aún no hay, intenta front_default
                if (string.IsNullOrEmpty(foto))
                {
                    foto = (string?)data["sprites"]?["front_default"];
                }

                // Si nada funciona, usa una imagen por defecto o vacía
                if (string.IsNullOrEmpty(foto))
                {
                    foto = "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/0.png";
                }

                clsPokemon pokemonAleatorio = new clsPokemon(id, char.ToUpper(nombre[0]) + nombre.Substring(1), foto);

                return pokemonAleatorio;
            }
        }
    }
}



