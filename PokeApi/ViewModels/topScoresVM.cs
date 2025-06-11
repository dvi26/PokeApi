using System.ComponentModel;
using System.Runtime.CompilerServices;
using ApiMaui.Resources;
using ENT;
using Services;

namespace PokeApi.ViewModels
{
    public class topScoresVM : INotifyPropertyChanged
    {
        #region Atributos
        private List<clsPuntuacion> puntuaciones;
        /// ActivityIndicator para mostrar el estado de carga
        private bool activityIndicator;
        private DelegateCommand juegoCommand;
        #endregion

        #region Propiedades
        public List<clsPuntuacion> Puntuaciones
        {
            get { return puntuaciones; }
            private set { puntuaciones = value; NotifyPropertyChanged("Puntuaciones"); }
        }
        public bool ActivityIndicator
        {
            get { return activityIndicator; }
            set { activityIndicator = value; NotifyPropertyChanged("ActivityIndicator"); }
        }
        public DelegateCommand JuegoCommand
        {
            get { return juegoCommand; }
        }
        #endregion

        #region Constructor
        public topScoresVM()
        {
            cargarListado();
            juegoCommand = new DelegateCommand(irAJuego);
        }
        #endregion

        #region Funciones
        /// <summary>
        /// Funcion que trae y carga el listado desde la Api
        /// </summary>
        /// <returns></returns>
        private async Task cargarListado()
        {
            try
            {
                ActivityIndicator = true;
                Puntuaciones = await ManejadoraPuntuacionApi.getListadoPuntuacionesAsync();
                ActivityIndicator = false;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("404"))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", $"No se encontraron puntuaciones: {ex.Message}", "Aceptar");
                }
                else if (ex.Message.Contains("400"))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", $"Solicitud incorrecta al cargar las puntuaciones: {ex.Message}", "Aceptar");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", $"Error al cargar el listado de puntuaciones: {ex.Message}", "Aceptar");
                }
            }
        }
        /// <summary>
        /// Funcion que se encargará de recargar el listado en el OnAppearing del codigo behind
        /// </summary>
        /// <returns></returns>
        public async Task recargarListado()
        {
            await cargarListado();
        }

        /// <summary>
        /// Navega a la vista del juego.
        /// </summary>
        private async void irAJuego()
        {
            await Shell.Current.GoToAsync("///jugar");
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
