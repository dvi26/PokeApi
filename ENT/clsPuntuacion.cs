using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENT
{
    public class clsPuntuacion
    {
        #region Atributos
        private int id;
        private string nombreJugador;
        private int puntuacion;
        #endregion

        #region Propiedades
        public int Id
        {
            get { return id; }
        }
        public string NombreJugador
        {
            get { return nombreJugador; }
            set { nombreJugador = value; }
        }
        public int Puntuacion
        {
            get { return puntuacion; }
            set { puntuacion = value; }
        }
        #endregion

        #region Constructores
        public clsPuntuacion(int id, string nombreJugador, int puntuacion)
        {
            this.id = id;
            this.nombreJugador = nombreJugador;
            this.puntuacion = puntuacion;
        }
        public clsPuntuacion()
        {

        }
        #endregion
    }
}
