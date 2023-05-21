using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMTP4.Clases
{
    public class Cliente
    {
        private string estado;
        private int tiempoEnEspera;
        

        public string Estado
        {
            get { return estado; }
            set { estado = value; }
        }

        public int TiempoEnEspera
        {
            get { return tiempoEnEspera; }
            set { tiempoEnEspera = value; }
        }

    }
}
