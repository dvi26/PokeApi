using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using PokeApi.ViewModels;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace PokeApi.Models
{
    public class clsPregunta
    {
        private clsPokemon pokemonCorrecto;
        private ObservableCollection<clsPokemon> listaPokemons;
        private clsPokemon pokemonSeleccionado;
        //private int puntos;
        private bool esCorrecto;

        /*
        public int Puntos
        {
            get { return puntos; }
        }*/
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
            set { pokemonSeleccionado = value; comprobarCorrecto(); }
        }
        public bool EsCorrecto
        {
            get { return esCorrecto; }
        }
        public clsPregunta(clsPokemon pokemonCorrecto, ObservableCollection<clsPokemon> listaPokemons)
        {
            this.pokemonCorrecto = pokemonCorrecto;
            this.listaPokemons = listaPokemons;
        }
        public clsPregunta()
        {

        }
        /*
        public void comprobarCorrecto(int segundosRestantes)
        {
            if (pokemonSeleccionado != null)
            {
                if (pokemonSeleccionado.Id == pokemonCorrecto.Id)
                {
                    puntos += segundosRestantes;
                }
                else
                {
                    puntos = -1;
                }
            }
        }*/
        private void comprobarCorrecto()
        {
            if (pokemonSeleccionado != null)
            {
                if (pokemonSeleccionado.Id == pokemonCorrecto.Id)
                {
                    esCorrecto = true;
                }
                else
                {
                    esCorrecto = false;
                }
            }
        }
    }
}

