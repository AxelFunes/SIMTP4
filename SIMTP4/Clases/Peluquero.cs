using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMTP4.Clases
{
    public class Peluquero
    {
        //private string nombre;
        private double demoraMinima;
        private double demoraMaxima;
        private float porcentajeAtencion;
        private string estado;
        private double tiempoAtencion;
        private double finAtencion;
        //private List<Cliente> ;

        /*public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }
        */
        public double DemoraMinima
        {
            get { return demoraMinima; }
            set { demoraMinima = value; }
        }

        public double DemoraMaxima
        {
            get { return demoraMaxima; }
            set { demoraMaxima = value; }
        }
        public string Estado
        {
            get { return estado; }
            set { estado = value; }
        }

        public float PorcentajeAtencion
        {
            get { return porcentajeAtencion; }
            set { porcentajeAtencion = value; }
        }
        public double TiempoAtencion
        {
            get { return tiempoAtencion; }
            set { tiempoAtencion = value; }
        }
        public double FinAtencion
        {
            get { return finAtencion; }
            set { finAtencion = value; }
        }
    }
}
