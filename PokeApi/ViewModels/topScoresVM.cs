using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ENT;
using Services;

namespace PokeApi.ViewModels
{
    public class topScoresVM : INotifyPropertyChanged
    {
        private List<clsPuntuacion> puntuaciones;
        public List<clsPuntuacion> Puntuaciones
        {
            get { return puntuaciones; }
            private set { puntuaciones = value; NotifyPropertyChanged("Puntuaciones"); }
        }
        public topScoresVM()
        {
            cargarListado();
        }

        private async Task cargarListado()
        {
            try
            {
                Puntuaciones = await ManejadoraPuntuacionApi.getListadoPuntuacionesAsync();
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
        public async Task recargarListado()
        {
            await cargarListado();
        }

        #region NotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}
