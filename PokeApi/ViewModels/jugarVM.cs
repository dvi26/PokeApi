using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ApiMaui.Resources;
using DTO;
using ENT;
using PokeApi.Models;
using Services;

namespace PokeApi.ViewModels
{
    public class jugarVM : INotifyPropertyChanged
    {
        #region Atributos
        private int contador = 5;
        private clsPregunta preguntaActual;
        private int puntuacionTotal;
        private int rondasTotales = 0;
        private DelegateCommand insertarCommand;
        private DelegateCommand empezarCommand;
        private DelegateCommand reiniciarCommand;
        private DelegateCommand rankingCommand;
        private bool menuReglas = true;
        private bool vistaJuego = false;
        private bool vistaFinal = false;
        private IDispatcherTimer timer;
        private string nombre;
        private string haInsertado;
        private string color;
        private bool visibilidadRanking = false;

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
            set
            {
                if (PreguntaActual != null && PreguntaActual.PokemonSeleccionado == null)
                {
                    PreguntaActual.PokemonSeleccionado = value;
                    asignarPuntuacion();
                    pasarRonda();
                }
            }
        }

        public string Color
        {
            get { return color; }
            set { color = value; NotifyPropertyChanged("Color"); }
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

        public DelegateCommand EmpezarCommand
        {
            get
            {
                return empezarCommand;
            }
        }
        public DelegateCommand ReiniciarCommand
        {
            get
            {
                return reiniciarCommand;
            }
        }
        public DelegateCommand RankingCommand
        {
            get
            {
                return rankingCommand;
            }
        }
        public bool MenuReglas
        {
            get { return menuReglas; }
            private set { menuReglas = value; NotifyPropertyChanged("MenuReglas"); }
        }
        public bool VistaJuego
        {
            get { return vistaJuego; }
            private set { vistaJuego = value; NotifyPropertyChanged("VistaJuego"); }
        }
        public bool VistaFinal
        {
            get { return vistaFinal; }
            private set { vistaFinal = value; NotifyPropertyChanged("VistaFinal"); }
        }
        public string Nombre
        {
            get { return nombre; }
            //set publico para que funcione el canExecute
            set
            {
                nombre = value;
                NotifyPropertyChanged("Nombre");
                insertarCommand.RaiseCanExecuteChanged();
            }
        }
        public string HaInsertado
        {
            get { return haInsertado; }
            private set { haInsertado = value; NotifyPropertyChanged("HaInsertado"); }
        }
        public bool VisibilidadRanking
        {
            get { return visibilidadRanking; }
            private set { visibilidadRanking = value; NotifyPropertyChanged("VisibilidadRanking"); }
        }
        #endregion

        #region Constructor
        public jugarVM()
        {
            // Carga la primera pregunta antes de arrancar el timer
            cargarPregunta();

            empezarCommand = new DelegateCommand(empezarJuegoCommand);
            insertarCommand = new DelegateCommand(insertarPuntuacionCommand, activarBoton);
            reiniciarCommand = new DelegateCommand(reiniciarJuego);
            rankingCommand = new DelegateCommand(irARanking);

            timer = Application.Current.Dispatcher.CreateTimer();
            timer.Interval = TimeSpan.FromSeconds(1.5);
            timer.Tick += handlerJuego;
        }
        #endregion

        #region Funciones

        /// <summary>
        /// Muestra la vista del juego, ademas hace que comience el timer/juego
        /// </summary>
        private async void empezarJuegoCommand()
        {
            MenuReglas = false;
            VistaJuego = true;
            timer.Start();
        }

        /// <summary>
        /// Timer que cada 1.5 segundos carga una pregunta.
        /// </summary>
        /// <returns></returns>
        private async void handlerJuego(object sender, EventArgs e)
        {
            Contador -= 1;

            if (Contador <= 0)
            {
                pasarRonda();
            }
        }

        /// <summary>
        /// Asigna la puntuación total al jugador dependiendo de si la respuesta es correcta o no.
        /// </summary>
        private void asignarPuntuacion()
        {
            if (PreguntaActual.EsCorrecto)
            {
                PuntuacionTotal += Contador;
                Color = "#388E3C"; 
            }
            else
            {
                PuntuacionTotal -= 1;
                Color = "#D32F2F"; 
            }
        }

        /// <summary>
        /// Pasa a la siguiente ronda, reinicia el contador y carga una nueva pregunta. Si se han jugado menos de 20 rondas, continúa el juego; si no, finaliza el juego.
        /// </summary>
        private async void pasarRonda()
        {
            timer.Stop();
            await Task.Delay(2000);
            Color = "#FFFFFF"; 
            timer.Start();
            Contador = 5;
            RondasTotales++;
            if (RondasTotales <= 20)
            {
                await cargarPregunta();
            }
            else
            {
                timer.Stop();
                VistaJuego = false;
                VistaFinal = true;
            }
        }

        /// <summary>
        /// Carga una pregunta aleatoria con 4 pokemons, uno de ellos es el correcto.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Inserta la puntuación del jugador en la base de datos a través de la API.
        /// </summary>
        /// <returns></returns>
        private async void insertarPuntuacionCommand()
        {
            clsPuntuacion nuevaPuntuacion = new clsPuntuacion();
            nuevaPuntuacion.NombreJugador = Nombre;
            if (PuntuacionTotal < 0)
            {
                //Para que no sea negativa
                nuevaPuntuacion.Puntuacion = 0;
            }
            else
            {
                nuevaPuntuacion.Puntuacion = PuntuacionTotal;
            }

            int filasAfectadas = 0;
            try
            {
                filasAfectadas = await ManejadoraPuntuacionApi.AgregarPuntuacionAsync(nuevaPuntuacion);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("404"))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", $"No se pudo insertar la puntuacion: {ex.Message}", "Aceptar");
                }
                else if (ex.Message.Contains("400"))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", $"Solicitud incorrecta al insertar la puntuacion: {ex.Message}", "Aceptar");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", $"Error: {ex.Message}", "Aceptar");
                }
            }
            if (filasAfectadas > 0)
            {
                HaInsertado = "Puntuación insertada correctamente";
                VisibilidadRanking = true;
            }
            else
            {
                haInsertado = "No se pudo insertar la puntuación, intente de nuevo.";
            }
        }

        /// <summary>
        /// Verifica si el botón de insertar puntuación debe estar activo o no, basado en si el nombre no es nulo.
        /// </summary>
        /// <returns></returns>
        private bool activarBoton()
        {
            bool res;
            if (!string.IsNullOrEmpty(Nombre))
            {
                res = true;
            }
            else
            {
                res = false;
            }
            return res;
        }

        /// <summary>
        /// Reinicia el juego, reseteando las puntuaciones y rondas totales, y volviendo al menú de reglas.
        /// </summary>
        private async void reiniciarJuego()
        {
            await cargarPregunta();
            Nombre = "";
            PuntuacionTotal = 0;
            RondasTotales = 0;
            VistaJuego = false;
            VistaFinal = false;
            MenuReglas = true;
        }

        /// <summary>
        /// Navega a la vista de ranking de puntuaciones.
        /// </summary>
        private async void irARanking()
        {
            await Shell.Current.GoToAsync("///topScores");
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
