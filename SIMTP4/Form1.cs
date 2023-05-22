using SIMTP4.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIMTP4
{
    public partial class Form1 : Form
    {
        public Peluquero aprendiz = new Peluquero();
        public Peluquero veteranoA = new Peluquero(); 
        public Peluquero veteranoB = new Peluquero();
        //public Random random_peluquero = new Random();

        double random_peluquero;
        double random_llegada;
        double random_atencionAprendiz;
        double random_atencionVetA;
        double random_atencionVetB;

        string peluquero;


        public Form1()
        {
            InitializeComponent();
        }

        private void btn_Simular_Click(object sender, EventArgs e)
        {
            double demMinAprendiz = Convert.ToDouble(txtDemMinAprendiz.Text);
            double demMaxAprendiz = Convert.ToDouble(txtDemMaxAprendiz.Text);
            double demMinVetA = Convert.ToDouble(txtDemMinVetA.Text);
            double demMaxVetA = Convert.ToDouble(txtDemMaxVetA.Text);
            double demMinVetB = Convert.ToDouble(txtDemMinVetB.Text);
            double demMaxVetB = Convert.ToDouble(txtDemMaxVetB.Text);
            aprendiz.DemoraMinima = demMinAprendiz;
            aprendiz.DemoraMaxima = demMaxAprendiz;
            veteranoA.DemoraMinima = demMinVetA;
            veteranoA.DemoraMaxima= demMaxVetA;
            veteranoB.DemoraMinima= demMinVetB;
            veteranoB.DemoraMaxima = demMaxVetB;

            double dias= Convert.ToDouble(txtTiempoSimulacion.Text);
            double desde = Convert.ToDouble(txt_desde.Text);
            double hasta = Convert.ToDouble(txt_hasta.Text);

            simulacion(dias, desde, hasta);


        }

        public void simulacion(double dias, double desde, double hasta)
        {
            
            double reloj = 0;
            double minutos = dias * 480;
            double horadesde = desde * 60;
            

            /*
            for (double i = reloj; i <= minutos; i++)
            {
                pide = "NO";

                random_demanda = rnd.NextDouble();
                BuscarDemanda();

                if (llegada == i && semanas != 0)
                {
                    yaPidio = false;
                    cantidadFalladas = 0;

                    random_falla1 = rnd.NextDouble();
                    random_falla2 = rnd.NextDouble();
                    random_falla3 = rnd.NextDouble();
                    random_falla4 = rnd.NextDouble();
                    random_falla5 = rnd.NextDouble();
                    random_falla6 = rnd.NextDouble();
                    BuscarFalla(random_falla1, random_falla2, random_falla3, random_falla4, random_falla5, random_falla6);
                    stock_Final = stock_Inicial + 6 - cantidadFalladas;

                }
                else
                {
                    stock_Final = stock_Inicial - demanda;
                    if (stock_Final <= pRenovacion && yaPidio == false)
                    {
                        if (stock_Final < 0)
                        {
                            agotamiento = Math.Abs(stock_Final);
                            stock_Final = 0;
                            costoAgotamiento = agotamiento * cAgotamiento;
                        }
                        else { costoAgotamiento = 0; } //reseteo para el acumulado
                        pide = "SI";
                        costoPedido = cPedido;

                        yaPidio = true;
                        random_demora = rnd.NextDouble();
                        BuscarDemora();
                        llegada = i + demora;

                    }
                    else //VEER
                    {
                        costoPedido = 0; //reseteo para el acumulado
                        if (stock_Final < 0)
                        {
                            agotamiento = Math.Abs(stock_Final);
                            stock_Final = 0;
                            costoAgotamiento = agotamiento * cAgotamiento;
                        }
                        else { costoAgotamiento = 0; } //reseteo para el acumulado
                    }

                    cantidadFalladas = 0; //reseteo para el acumulado
                }
                semanas = i;

                costoTenencia = stock_Final * cTenencia;
                costoTotal = costoTenencia + costoPedido + costoAgotamiento;
                costoAcumulado += costoTotal;

                if (semanas >= desde && semanas <= hasta)
                {
                    cargarGrilla(semanas);

                }
                if (semanas == experimentos && hasta != experimentos)
                {
                    cargarGrilla(semanas);
                }
                stock_Inicial = stock_Final;
            }*/

        }

        public void GenerarTiempoProximaLlegada()
        {
            /*
            RNDLlegadas = generadorLlegadas.NextDouble();
            TiempoLlegadas = calcularDemora(txt, fila.RNDLlegadas);
            ProximaLlegada = fila.Reloj + fila.TiempoLlegadas;
            */
        }
        public double calcularDemora(double a, double b, double rnd) //de atencion Y/O llegada
        {
            double unif = a + rnd * (b - a);
            return unif;
        }

        public void BuscarPeluquero()
        {
            double prob0 = Convert.ToDouble(txtProbAprendiz.Text) / 100;
            double prob1 = Convert.ToDouble(txtProbVetA.Text) / 100;
            


            if (random_peluquero < prob0)
            {
                peluquero = "Aprendiz";
            }
            else
            {
                if (random_peluquero < (prob0 + prob1))
                {
                    peluquero = "Veterano A";
                }
                else
                { 
                        peluquero = "Veterano B";
                                       
                }
            }

        }


        public void llegadaCliente()
        {

        }
        public void finAtencionAprendiz()
        {

        }
        public void finAtencionVetA()
        {

        }
        public void finAtencionVetB()
        {

        }


    }
}
