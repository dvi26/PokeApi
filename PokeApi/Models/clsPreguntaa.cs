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
        #region Atributos
        private clsPokemon pokemonCorrecto;
        private ObservableCollection<clsPokemon> listaPokemons;
        private clsPokemon pokemonSeleccionado;
        private bool esCorrecto;
        #endregion

        #region Propiedades
        public clsPokemon PokemonCorrecto
        {
            get { return pokemonCorrecto; }
        }
        public ObservableCollection<clsPokemon> ListaPokemons
        {
            get { return listaPokemons; }
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
        #endregion

        #region Funciones
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
        #endregion
    }
}

