using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class clsPokemon
    {
        private int id;
        private string nombre;
        private string foto;

        public int Id
        {
            get { return id; }
            //set { id = value; }
        }
        public string Nombre
        {
            get { return nombre; }
            //set { nombre = value; }
        }
        public string Foto
        {
            get { return foto; }
            //set { foto = value; }
        }

        public clsPokemon(int id, string nombre, string foto)
        {
            this.id = id;
            this.nombre = nombre;
            this.foto = foto;
        }

        public clsPokemon()
        {
            
        }

    }
}
