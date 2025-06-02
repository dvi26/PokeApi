using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ApiMaui.Resources;
using DTO;
using PokeApi.Models;
using Services;

namespace PokeApi.ViewModels
{
    public class jugarVM : BindableObject,INotifyPropertyChanged
    {
        #region Atributos
        private int contador;
        private clsPregunta preguntaActual;
        private int puntuacionTotal;
        private clsPokemon pokemonSeleccionado;
        private int rondasTotales = 0;
        private DelegateCommand insertarCommand;
        #endregion

        #region Propiedades
              
        public int RondasTotales
        {
            get { return rondasTotales; }
            private set { rondasTotales = value; NotifyPropertyChanged("RondasTotales"); }
        }

        public int PuntuacionTotal
        {
            get { return puntuacionTotal; }
            private set { puntuacionTotal = value; NotifyPropertyChanged("PuntuacionTotal"); }
        }
        public clsPokemon PokemonSeleccionado
        {
            get { return pokemonSeleccionado; }
            set
            {
                pokemonSeleccionado = value;
                if (PreguntaActual != null && PreguntaActual.PokemonSeleccionado == null)
                {
                    PreguntaActual.PokemonSeleccionado = value;
                    PreguntaActual.comprobarCorrecto(Contador);
                    PuntuacionTotal += PreguntaActual.Puntos;
                }
                NotifyPropertyChanged("PokemonSeleccionado");
            }
        }

        public clsPregunta PreguntaActual
        {
            get { return preguntaActual; }
            private set { preguntaActual = value; NotifyPropertyChanged("PreguntaActual"); }
        }
        public int Contador
        {
            get { return contador; }
            private set
            {
                contador = value; NotifyPropertyChanged("Contador");
            }
        }

        public DelegateCommand InsertarCommand
        {
            get
            {
                return insertarCommand;
            }
        }
        #endregion

        #region Constructor
        public jugarVM()
        {
            _ = cuentaAtras();
            _ = cargarPregunta();
            //insertarCommand = new DelegateCommand(insertarPuntuacion(),activarBoton());
        }
        #endregion

        #region Funciones
        private async Task cuentaAtras()
        {
            bool res = true;
            RondasTotales++;
            Contador = 5;
            //Lo he puesto a 1.5 segundos para que se vea mejor el efecto
            if (RondasTotales <= 20)
            {
                Dispatcher.StartTimer(TimeSpan.FromSeconds(1.5), () =>
                {
                    Contador -= 1;
                    if (Contador <= 0)
                    {
                        _ = cuentaAtras();
                        _ = cargarPregunta();
                        res = false;
                    }
                    return res;
                });
            }
        }
        private async Task cargarPregunta()
        {
            int pokemonMax = 1025;
            int pokemonsMaxLista = 4;
            Random rnd = new Random();

            //HashSet para evitar duplicados
            HashSet<int> idsElegidos = new HashSet<int>();

            while (idsElegidos.Count < pokemonsMaxLista)
            {
                idsElegidos.Add(rnd.Next(1, pokemonMax + 1));
            }

            ObservableCollection<clsPokemon> listaPokemon = new ObservableCollection<clsPokemon>();

            foreach (int id in idsElegidos)
            {
                try
                {
                    clsPokemon pokemon = await ManejadoraPokeApi.GetPokemonRandomAsync(id);
                    listaPokemon.Add(pokemon);
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("404"))
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", $"No se encontro el pokemon: {ex.Message + id}", "Aceptar");
                    }
                    else if (ex.Message.Contains("400"))
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", $"Solicitud incorrecta al traer el pokemon: {ex.Message}", "Aceptar");
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", $"Error: {ex.Message}", "Aceptar");
                    }
                }      
            }

            int indiceCorrecto = rnd.Next(listaPokemon.Count);
            clsPokemon pokemonCorrecto = listaPokemon[indiceCorrecto];

            PreguntaActual = new clsPregunta(pokemonCorrecto, listaPokemon);
        }
        #endregion

        #region NotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
