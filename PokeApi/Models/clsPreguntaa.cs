using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using PokeApi.ViewModels;

namespace PokeApi.Models
{
    public class clsPregunta
    {
        private clsPokemon pokemonCorrecto;
        private ObservableCollection<clsPokemon> listaPokemons;
        private clsPokemon pokemonSeleccionado;
        private int puntos;

        public int Puntos
        {
            get { return puntos; }
        }
        public clsPokemon PokemonCorrecto
        {
            get { return pokemonCorrecto; }
            //set { pokemonCorrecto = value; }
        }
        public ObservableCollection<clsPokemon> ListaPokemons
        {
            get { return listaPokemons; }
            //set { listaPokemons = value; }
        }
        public clsPokemon PokemonSeleccionado
        {
            get { return pokemonSeleccionado; }
            set { pokemonSeleccionado = value; }
        }
        public clsPregunta(clsPokemon pokemonCorrecto, ObservableCollection<clsPokemon> listaPokemons)
        {
            this.pokemonCorrecto = pokemonCorrecto;
            this.listaPokemons = listaPokemons;
        }
        public clsPregunta()
        {

        }
        public void comprobarCorrecto(int segundosRestantes)
        {
            if (pokemonSeleccionado != null)
            {
                if (pokemonSeleccionado.Id == pokemonCorrecto.Id)
                {
                    this.puntos += segundosRestantes;
                }
                else
                {
                    puntos = -1;
                }
            }
        }
    }
}

