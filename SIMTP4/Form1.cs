using SIMTP4.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SIMTP4.Form1.Peluquero;

namespace SIMTP4
{
    public partial class Form1 : Form
    {
        public Peluquero aprendiz = new Peluquero();
        public Peluquero veteranoA = new Peluquero(); 
        public Peluquero veteranoB = new Peluquero();
        //Clases
        public class Peluquero
        {
            public enum Nombre { Aprendiz, VeteranoA, VeteranoB };
            public string nombre;
            public enum Estado { Libre, Ocupado };
            public Estado estado;
            public double? finTiempoAtencion;
            public double demoraMinima;
            public double demoraMaxima;
            public double tiempoAtencion;
        }
        public class Cliente
        {
            public int numero;
            public enum Estado { Esperando_Atencion, Siendo_Atendido, Destruido };
            public Estado estado;
            public string Peluquero;           
            public double? TiempoEspera;
            public enum EstaEnCola { Si, No }
            public EstaEnCola esta_en_cola;
        }
        //public Random random_peluquero = new Random();

        double random_peluquero;
        double random_llegada;
        double random_atencionAprendiz;
        double random_atencionVetA;
        double random_atencionVetB;

        string peluquero;
        private int simulaciones;
        private int dias;
        List<Cliente> listCliente = new List<Cliente>();
        List<Peluquero> ListPeluquero = new List<Peluquero>();

        //Variables para Generar la simulacion de colas
        string Evento;
        string Evento_Anterior;
        double? Reloj = 0;
        double Reloj_Anterior;

        //tiempo entre llegadas
        private int demoraMinima;
        private int demoraMaxima;
        private double HoraProxLlegada;
        private Random rnd = new Random();

        //simulacion
        private Peluquero peluqueroSeleccionado;
        private double proximaLlegada;
        private double? menorTiempoFin;
        private double? Menor_Hora_Proximo_Evento;

        //Variables para Llegada de Proximo Cliente
        double randomDemora;
        private double randomDemoraPeluquero;
        double Hora_Llegada_Matricula;
        double Random_Renovacion;
        double Hora_Llegada_Renovacion;
        double Tiempo_Entre_Llegada;
        string Tipo_Cliente;
        double Proxima_Llegada;
        double Proxima_Llegada_Anterior;

        //Variables para Fin de Atencion
        double Random_Tiempo_Atencion;
        double Tiempo_Atencion;
        double? Fin_Manuel = 0;

        //Zona Matricula
        double? Fin_Tomas = 0;
        double? Fin_Alicia = 0;

        //Zona Renovacion
        double? Fin_Lucia = 0;
        double? Fin_Maria = 0;


        //Variables Para Servidores
        int colaAprendiz;
        int colaVeteranoA;
        int colaVeteranoB;
        string estadoAprendiz;
        string estadoVeteranoA;
        string estadoVeteranoB;
        double? finAprendiz;
        double? finVeteranoA;
        double? finVeteranoB;
        private int contadorCliente= 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void btn_Simular_Click(object sender, EventArgs e)
        {/*
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

            int dias= Convert.ToDouble(txtTiempoSimulacion.Text);
            double desde = Convert.ToDouble(txt_desde.Text);
            double hasta = Convert.ToDouble(txt_hasta.Text);

            simulacion(dias, desde, hasta);*/
            if (txtTiempoSimulacion.Text != "") //faltan validaciones
            {

                dias = Convert.ToInt32(txtTiempoSimulacion.Text);//cant de dias a simular
                //Variable_3 = true; Ver que hace

                Simulacion_Cero();
                for (int i = 0; i < simulaciones; i++)
                {
                    Comenzar();
                    cargarGrilla();
                }
                /*
                if (Variable_3 == true)
                {
                    foreach (var item in listCliente)
                    {
                        dgv_Clientes.Rows.Add(item.numero, item.estado, item.Servidor, item.Tipo_Cliente, item.TiempoEspera, item.esta_en_cola);
                    }
                }*/

            }
            else
            {
                MessageBox.Show("Coloque valores en la celda Simular");
            }

        }

        private void Comenzar()
        {
            ObtenerPeluquero();
            Elegir_Menor_Para_Proximo_Evento();
            Reloj = Menor_Hora_Proximo_Evento;

            if (Menor_Hora_Proximo_Evento < 480)
            {
                if (Evento=="Llegada Cliente")
                {
                    foreach (var item in ListPeluquero)
                    {
                        if (item.nombre == peluqueroSeleccionado.nombre)
                        {
                            item.estado = Peluquero.Estado.Ocupado;
                            
                        }
                    }
                }
                else if (Evento=="Fin Atencion")
                {

                }
                
                
            }
        }
        public void Elegir_Menor_Para_Proximo_Evento()
        {

            menorTiempoFin = ListPeluquero.Min(x => x.finTiempoAtencion);

            if (menorTiempoFin != null)
            {
                if (Proxima_Llegada > menorTiempoFin)
                {
                    Menor_Hora_Proximo_Evento = menorTiempoFin;
                    Evento = "Fin Atencion";
                }
                else
                {
                    Menor_Hora_Proximo_Evento = Proxima_Llegada;
                    Evento = "Llegada Cliente";
                }
            }
            else
            {
                Menor_Hora_Proximo_Evento = Proxima_Llegada;
                Evento = "Llegada Cliente";
            }


        }

        private void Simulacion_Cero()
        {
            listCliente.Clear();
            //ObtenerPeluquero();
            Calcular_Tiempo_Entre_Llegada();

            //Proxima_Llegada = Tiempo_Entre_Llegada;
            //Proxima_Llegada_Anterior = Tiempo_Entre_Llegada;

            foreach (var item in ListPeluquero)
            {
                item.estado = Peluquero.Estado.Libre;
                item.finTiempoAtencion = null;
            }


            Evento = "Llegada Cliente";

            Menor_Hora_Proximo_Evento = Proxima_Llegada;

            estadoAprendiz = "Libre";
            estadoVeteranoA = "Libre";
            estadoVeteranoB = "Libre";
            
            finAprendiz = 0;
            finVeteranoA = 0;
            finVeteranoB = 0;

            colaAprendiz = 0;
            colaVeteranoA = 0;
            colaVeteranoB = 0;


            //cargarGrilla();
        }

        private void ObtenerPeluquero()
        {
            double prob0 = Convert.ToDouble(txtProbAprendiz.Text) / 100;
            double prob1 = Convert.ToDouble(txtProbVetA.Text) / 100;
            random_peluquero = rnd.NextDouble();
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
            
            peluqueroSeleccionado = ListPeluquero.Find(x => x.nombre == peluquero);
        }

        private double CalcularDemora(int tiempominimo,int tiempomaximo,double random)
        {
            return (demoraMinima + randomDemora * (demoraMaxima - demoraMinima));
        }

        private void Calcular_Tiempo_Entre_Llegada()
        {
            ObtenerPeluquero();
            demoraMinima = 2;   //del cliente
            demoraMaxima = 12;
            randomDemora = rnd.NextDouble();
            randomDemoraPeluquero = rnd.NextDouble();
            //Random_Matricula = rnd.NextDouble();
            HoraProxLlegada = CalcularDemora(demoraMinima, demoraMaxima, randomDemora);
            proximaLlegada = (double)(HoraProxLlegada + Reloj);
            contadorCliente++;
            if (peluqueroSeleccionado.estado == Estado.Libre) 
            {
                peluqueroSeleccionado.estado = Estado.Ocupado;
                Tiempo_Atencion = CalcularDemora(peluqueroSeleccionado.demoraMinima,peluqueroSeleccionado.demoraMaxima, randomDemoraPeluquero);
            }
            listCliente.Add(new Cliente { numero = contadorCliente, estado = Cliente.Estado.Esperando_Atencion, Peluquero = peluquero, TiempoEspera = 0, esta_en_cola = Cliente.EstaEnCola.No });
            
        }
        public void cargarGrilla()
        {
            dgv_simulaciones.Rows.Add(Evento, Math.Round((double)Reloj, 4), Math.Round(Random_Matricula, 4), Math.Round(Hora_Llegada_Matricula, 4), Math.Round(Random_Renovacion, 4), Math.Round(Hora_Llegada_Renovacion, 4)
                , Math.Round(Proxima_Llegada, 4), Math.Round(Random_Tiempo_Atencion, 4), Fin_Tomas, Fin_Alicia, Fin_Lucia, Fin_Maria, Fin_Manuel
                , Cola_Matricula, Estado_Tomas, Estado_Alicia, Cola_Renovacion, Estado_Lucia, Estado_Maria, Estado_Manuel);
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
