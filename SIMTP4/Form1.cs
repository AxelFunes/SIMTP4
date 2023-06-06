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
using static SIMTP4.Form1.Cliente;

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
            //public enum Nombre { Aprendiz, VeteranoA, VeteranoB };
            public string nombre;
            public string estado;
            public double? finTiempoAtencion;
            public double demoraMinima;
            public double demoraMaxima;
            public double tiempoAtencion;
            public int cola;
            public int idCliente;
        }
        public class Cliente
        {
            public int numero;
            //public enum Estado { Esperando_Atencion, Siendo_Atendido, Destruido };
            public string estado;
            public string Peluquero;
            public double? TiempoEspera;
            public double? horaLlegada;
        }
        //public Random random_peluquero = new Random();

        double random_peluquero;
        double random_llegada;
        double random_atencionAprendiz;
        double random_atencionVetA;
        double random_atencionVetB;

        string peluquero;
        Cliente clientePelu = new Cliente();
        private int simulaciones;
        private int dias;
        List<Cliente> listCliente = new List<Cliente>();
        List<Peluquero> ListPeluquero = new List<Peluquero>();

        //Punto B
        int cantClientesEnCola;
        int cantMasAltaDeClientesCola;
        //Variables para Generar la simulacion de colas
        string Evento;
        string Evento_Anterior;
        double? Reloj = 0;
        double Reloj_Anterior;

        //tiempo entre llegadas
        private int demoraMinima;
        private int demoraMaxima;
        private double demoraCliente;
        private Random rnd = new Random();

        //simulacion
        private Peluquero peluqueroSeleccionado;
        private double proximaLlegada = 0;
        private double? menorTiempoFin;
        private double? Menor_Hora_Proximo_Evento;
        private string nombrePeluquero;
        private double acumOcupacionAprendiz;
        private int idClienteAtendido;
        private int contadorCliente;
        private string supera8hs;

        private double desde;
        private double hasta;
        private int contadorIteraciones = 0;

        //Variables para Llegada de Proximo Cliente
        double randomDemoraCliente;
        double randomDemoraPeluquero;
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
        double? finAprendiz;
        double? finVeteranoA;
        double? finVeteranoB;
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

                desde = Convert.ToDouble(txt_desde.Text);
                hasta = Convert.ToDouble(txt_hasta.Text);

                for (int i = 0; i < dias; i++)
                {
                    
                    Simulacion_Cero();
                    while (Reloj < 480)
                    {
                        Comenzar();
                    }
                    while (cantClientesEnCola > 0)
                    {
                        Comenzar();
                    }
                    Reloj = 0;
                    //contadorIteraciones = 0;
                    //Comenzar();
                    //cargarGrilla();
                }
                dgv_simulaciones.Rows.Add("Ultima Iteracion", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", Math.Round(acumOcupacionAprendiz, 4), cantMasAltaDeClientesCola);
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
            //ObtenerPeluquero();
            Elegir_Menor_Para_Proximo_Evento();
            Reloj = Menor_Hora_Proximo_Evento;
            //CancelacionCliente();
            //TotalCLientesCola();
            if (Reloj < 480)
            {
                if (Evento == "Llegada Cliente")
                {

                    nombrePeluquero = ObtenerPeluquero();
                    foreach (var peluquero in ListPeluquero)
                    {
                        if (peluquero.nombre == nombrePeluquero)
                        {
                            AsignarClienteAPeluquero(peluquero);
                            break;
                        }

                    }
                    Calcular_Tiempo_Entre_Llegada();

                    //CancelacionCliente();
                    //cargarGrilla();
                    contadorCliente++;
                }
                else /*if (Evento == "Fin Atencion")*/
                {

                    foreach (var peluquero in ListPeluquero)
                    {
                        if (peluquero.finTiempoAtencion == Menor_Hora_Proximo_Evento)
                        {

                            if (peluquero.cola > 0)
                            {
                                peluquero.cola--;
                                peluquero.estado = "Libre";
                                peluquero.tiempoAtencion = 0;
                                peluquero.finTiempoAtencion = null;

                                foreach (var cliente in listCliente)
                                {
                                    //if (cliente.estado == "Siendo Atendido" && peluquero.idCliente == cliente.numero)
                                    //{
                                    //    cliente.estado = "Cancelado";
                                    //}
                                    if (cliente.estado == "Siendo Atendido" && cliente.Peluquero == peluquero.nombre)
                                    {
                                        cliente.estado = "Desocupado";
                                        peluquero.estado = "Libre";
                                        idClienteAtendido = peluquero.idCliente;
                                    }
                                    if (cliente.estado == "Esperando Atencion" && cliente.Peluquero == peluquero.nombre && peluquero.estado == "Libre")
                                    {
                                        cliente.estado = "Siendo Atendido";
                                        peluquero.estado = "Ocupado";
                                        Tiempo_Atencion = CalcularDemora(peluquero.demoraMinima, peluquero.demoraMaxima, randomDemoraPeluquero);
                                        cliente.TiempoEspera = 0;
                                        peluquero.tiempoAtencion = Tiempo_Atencion;
                                        peluquero.finTiempoAtencion = (Tiempo_Atencion + Reloj);
                                        peluquero.idCliente = contadorCliente;
                                        if (nombrePeluquero == "Aprendiz")
                                        {
                                            random_atencionAprendiz = randomDemoraPeluquero;
                                            finAprendiz = Tiempo_Atencion + Reloj;
                                            acumOcupacionAprendiz += Tiempo_Atencion;
                                        }
                                        else if (nombrePeluquero == "Veterano A")
                                        {
                                            random_atencionVetA = randomDemoraPeluquero;
                                            finVeteranoA = Tiempo_Atencion + Reloj;
                                        }
                                        else { random_atencionVetB = randomDemoraPeluquero; finVeteranoB = Tiempo_Atencion + Reloj; }
                                        //AsignarClienteAPeluquero(peluquero);
                                        break;
                                    }
                                }

                                //clientePelu = listCliente.First(x => x.Peluquero == peluquero.nombre && x.estado == "Esperando Atencion");
                                //clientePelu.estado = "Siendo Atendido";
                                idClienteAtendido = peluquero.idCliente;
                                break;
                            }
                            else
                            {
                                peluquero.tiempoAtencion = 0;
                                peluquero.finTiempoAtencion = null;
                                peluquero.estado = "Libre";
                                foreach (var cliente in listCliente)
                                {
                                    if (cliente.estado == "Siendo Atendido" && cliente.Peluquero == peluquero.nombre)
                                    {
                                        cliente.estado = "Desocupado";
                                        peluquero.estado = "Libre";
                                        idClienteAtendido = peluquero.idCliente;
                                    }break;
                                }
                                idClienteAtendido = peluquero.idCliente;
                                break;
                            }
                        }
                    }
                    //cargarGrilla();
                }

                //cargarGrilla();
            }
            else if (Reloj > 480)
            {
                supera8hs = "Si";
                foreach (var peluquero in ListPeluquero)
                {
                    if (peluquero.finTiempoAtencion == Menor_Hora_Proximo_Evento)
                    {
                        if (peluquero.cola > 0)
                        {
                            peluquero.cola--;
                            //peluquero.estado = "Libre";
                            //peluquero.estado = "Ocupado";
                            peluquero.tiempoAtencion = 0;
                            peluquero.finTiempoAtencion = null;

                            foreach (var cliente in listCliente)
                            {
                                //if (cliente.estado == "Siendo Atendido" && peluquero.idCliente == cliente.numero)
                                //{
                                //    cliente.estado = "Cancelado";
                                //}
                                if (cliente.estado == "Siendo Atendido" && cliente.Peluquero == peluquero.nombre)
                                {
                                    cliente.estado = "Desocupado";
                                    peluquero.estado = "Libre";
                                    idClienteAtendido = peluquero.idCliente;
                                }
                                if (cliente.estado == "Esperando Atencion" && cliente.Peluquero == peluquero.nombre)
                                {
                                    cliente.estado = "Siendo Atendido";
                                    peluquero.estado = "Ocupado";
                                    Tiempo_Atencion = CalcularDemora(peluquero.demoraMinima, peluquero.demoraMaxima, randomDemoraPeluquero);
                                    cliente.TiempoEspera = 0;
                                    peluquero.tiempoAtencion = Tiempo_Atencion;
                                    peluquero.finTiempoAtencion = (Tiempo_Atencion + Reloj);
                                    peluquero.idCliente = contadorCliente;
                                    if (nombrePeluquero == "Aprendiz")
                                    {
                                        random_atencionAprendiz = randomDemoraPeluquero;
                                        finAprendiz = Tiempo_Atencion + Reloj;
                                        acumOcupacionAprendiz += Tiempo_Atencion;
                                    }
                                    else if (nombrePeluquero == "Veterano A")
                                    {
                                        random_atencionVetA = randomDemoraPeluquero;
                                        finVeteranoA = Tiempo_Atencion + Reloj;
                                    }
                                    else { random_atencionVetB = randomDemoraPeluquero; finVeteranoB = Tiempo_Atencion + Reloj; }
                                    //AsignarClienteAPeluquero(peluquero);
                                    //break;
                                }
                            }
                            //clientePelu = listCliente.First(x => x.Peluquero == peluquero.nombre && x.estado == "Esperando Atencion");
                            //clientePelu.estado = "Siendo Atendido";
                            idClienteAtendido = peluquero.idCliente;
                            //break;
                        }
                        else
                        {
                            peluquero.tiempoAtencion = 0;
                            peluquero.finTiempoAtencion = null;
                            peluquero.estado = "Libre";
                            idClienteAtendido = peluquero.idCliente;
                            break;
                        }
                    }

                }

                //cargarGrilla();
            }
            if (Reloj >= desde)
            {
                contadorIteraciones++;
                if (contadorIteraciones<= hasta)
                {
                    cargarGrilla();
                }
                
            }
            
            CancelacionCliente();
            TotalCLientesCola();
        }
        public void TotalCLientesCola()
        {
            cantClientesEnCola = 0;
            foreach (var peluca in ListPeluquero)
            {
                cantClientesEnCola += peluca.cola;
                if (cantClientesEnCola > cantMasAltaDeClientesCola)
                {
                    cantMasAltaDeClientesCola = cantClientesEnCola;
                }
            }
        }

        public void CancelacionCliente()
        {
            string nomPeluca = "";
            foreach (var cliente in listCliente)
            {
                if (cliente.estado == "Esperando Atencion")
                {
                    cliente.TiempoEspera = (Reloj - cliente.horaLlegada);
                }

                else if (cliente.TiempoEspera >= 30 && cliente.estado == "Esperando Atencion")
                {
                    cliente.estado = "Cancelado";
                    nomPeluca = cliente.Peluquero;
                    foreach (var peluquero in ListPeluquero)
                    {
                        if (peluquero.nombre == nomPeluca) // && peluquero.cola > 0)
                        {
                            peluquero.cola--;
                        }
                        break;
                    }
                }
                //if (cliente.estado == "Cancelado")
                //{
                //    //listCliente.Remove(cliente);
                //    //listCliente.RemoveAt(cliente.numero);
                //}
            }
        }
        public void Elegir_Menor_Para_Proximo_Evento()
        {
            menorTiempoFin = ListPeluquero.Min(x => x.finTiempoAtencion);

            if (menorTiempoFin != null)
            {
                //if (proximaLlegada > menorTiempoFin)
                if (menorTiempoFin < proximaLlegada || Reloj > 480)
                {
                    Menor_Hora_Proximo_Evento = menorTiempoFin;
                    Evento = "Fin Atencion";
                }
                else
                {
                    Menor_Hora_Proximo_Evento = proximaLlegada;
                    Evento = "Llegada Cliente";
                }
            }
            else
            {
                Menor_Hora_Proximo_Evento = proximaLlegada;
                Evento = "Llegada Cliente";
            }


        }

        private void Simulacion_Cero()
        {
            Menor_Hora_Proximo_Evento = 0;
            menorTiempoFin = 0;
            proximaLlegada = 0;
            contadorIteraciones = 0;

            supera8hs = "No";
            contadorCliente = 1;
            idClienteAtendido = 1;
            acumOcupacionAprendiz = 0;
            listCliente.Clear();
            //cantClientesEnCola=0;
            //ObtenerPeluquero();
            calcularPrimerLlegada();

            //Proxima_Llegada = Tiempo_Entre_Llegada;
            //Proxima_Llegada_Anterior = Tiempo_Entre_Llegada;

            foreach (var item in ListPeluquero)
            {
                item.estado = "Libre";
                item.finTiempoAtencion = null;
                item.cola = 0;
                item.idCliente = 0;
            }


            Evento = "Inicializacion";

            Menor_Hora_Proximo_Evento = proximaLlegada;

            cargarGrilla();
            Evento = "Llegada Cliente";
        }

        private String ObtenerPeluquero()
        {
            double prob0 = Convert.ToDouble(txtProbAprendiz.Text) / 100;
            double prob1 = Convert.ToDouble(txtProbVetA.Text) / 100;
            random_peluquero = rnd.NextDouble();
            string nombrePeluquero = "";
            if (random_peluquero < prob0)
            {
                nombrePeluquero = "Aprendiz";
                return nombrePeluquero;
            }
            else
            {
                if (random_peluquero < (prob0 + prob1))
                {
                    nombrePeluquero = "Veterano A";
                    return nombrePeluquero;
                }
                else
                {
                    nombrePeluquero = "Veterano B";
                    return nombrePeluquero;
                }
            }
        }

        private double CalcularDemora(double tiempominimo, double tiempomaximo, double random)
        {
            return (tiempominimo + random * (tiempomaximo - tiempominimo));
        }

        public void calcularPrimerLlegada()
        {
            demoraMinima = Convert.ToInt32(txtDemMinCliente.Text);   //del cliente
            demoraMaxima = Convert.ToInt32(txtDemMaxCliente.Text);
            randomDemoraCliente = rnd.NextDouble();
            //randomDemoraPeluquero = rnd.NextDouble();
            //Random_Matricula = rnd.NextDouble();
            demoraCliente = CalcularDemora(demoraMinima, demoraMaxima, randomDemoraCliente);
            proximaLlegada = (double)(demoraCliente + Reloj);

            //contadorCliente++;
        }
        private void Calcular_Tiempo_Entre_Llegada()
        {
            //string nombrePeluquero = "";
            //nombrePeluquero = ObtenerPeluquero();
            demoraMinima = Convert.ToInt32(txtDemMinCliente.Text);   //del cliente
            demoraMaxima = Convert.ToInt32(txtDemMaxCliente.Text);
            randomDemoraCliente = rnd.NextDouble();
            //randomDemoraPeluquero = rnd.NextDouble();
            demoraCliente = CalcularDemora(demoraMinima, demoraMaxima, randomDemoraCliente);
            proximaLlegada = (double)(demoraCliente + Reloj);
            //contadorCliente++;
            //AsignarClienteAPeluquero(peluqueroSeleccionado, contadorCliente);
        }
        public void AsignarClienteAPeluquero(Peluquero seleccionado)
        {
            randomDemoraPeluquero = rnd.NextDouble();
            if (seleccionado.estado == "Libre")
            {
                seleccionado.estado = "Ocupado";
                Tiempo_Atencion = CalcularDemora(seleccionado.demoraMinima, seleccionado.demoraMaxima, randomDemoraPeluquero);

                listCliente.Add(new Cliente { numero = contadorCliente, estado = "Siendo Atendido", Peluquero = seleccionado.nombre, TiempoEspera = 0, horaLlegada = Reloj });
                seleccionado.tiempoAtencion = Tiempo_Atencion;
                seleccionado.finTiempoAtencion = (Tiempo_Atencion + Reloj);
                seleccionado.idCliente = contadorCliente;
                if (nombrePeluquero == "Aprendiz")
                {
                    random_atencionAprendiz = randomDemoraPeluquero;
                    finAprendiz = Tiempo_Atencion + Reloj;
                    acumOcupacionAprendiz += Tiempo_Atencion;
                }
                else if (nombrePeluquero == "Veterano A")
                {
                    random_atencionVetA = randomDemoraPeluquero;
                    finVeteranoA = Tiempo_Atencion + Reloj;
                }
                else { random_atencionVetB = randomDemoraPeluquero; finVeteranoB = Tiempo_Atencion + Reloj; }
            }
            else
            {
                seleccionado.cola++;
                listCliente.Add(new Cliente { numero = contadorCliente, estado = "Esperando Atencion", Peluquero = seleccionado.nombre, TiempoEspera = 0, horaLlegada = Reloj });
            }

        }
        public void cargarGrilla()
        {
            if (Evento == "Inicializacion")
            {
                dgv_simulaciones.Rows.Add("Inicializacion", "0", supera8hs, "-", "-", "-"
               , "-", "-");
            }
            else if (Evento == "Llegada Cliente")
            {
                dgv_simulaciones.Rows.Add(Evento + "(" + contadorCliente + ")", Math.Round((double)Reloj, 4), supera8hs, Math.Round(random_peluquero, 4), nombrePeluquero, Math.Round(random_llegada, 4)
               , demoraCliente, Math.Round(proximaLlegada, 4), "", "", Math.Round(random_atencionAprendiz, 4), ObtenerTiempoAtencion("Aprendiz"), finAprendiz, Math.Round(random_atencionVetA, 4), ObtenerTiempoAtencion("Veterano A"), finVeteranoA, Math.Round(random_atencionVetB, 4), ObtenerTiempoAtencion("Veterano B"), finVeteranoB, ObtenerEstadoPeluquero("Aprendiz"), ObtenerColaPeluquero("Aprendiz"), ObtenerEstadoPeluquero("Veterano A"), ObtenerColaPeluquero("Veterano A"), ObtenerEstadoPeluquero("Veterano B"), ObtenerColaPeluquero("Veterano B"), Math.Round(acumOcupacionAprendiz, 4), cantMasAltaDeClientesCola);
            }
            else
            {
                dgv_simulaciones.Rows.Add(Evento, Math.Round((double)menorTiempoFin, 4), supera8hs, "-", "-", "-"
               , "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", ObtenerEstadoPeluquero("Aprendiz"), ObtenerColaPeluquero("Aprendiz"), ObtenerEstadoPeluquero("Veterano A"), ObtenerColaPeluquero("Veterano A"), ObtenerEstadoPeluquero("Veterano B"), ObtenerColaPeluquero("Veterano B"), Math.Round(acumOcupacionAprendiz,4), cantMasAltaDeClientesCola);
            }

        }
        public double ObtenerTiempoAtencion(string nombre)
        {
            Peluquero seleccionado = new Peluquero();
            seleccionado = ListPeluquero.Find(x => x.nombre == nombre);
            return Math.Round(seleccionado.tiempoAtencion, 4);
        }
        public string ObtenerEstadoPeluquero(string nombre)
        {
            Peluquero seleccionado = new Peluquero();
            seleccionado = ListPeluquero.Find(x => x.nombre == nombre);
            return seleccionado.estado;
        }
        public int ObtenerColaPeluquero(string nombre)
        {
            Peluquero seleccionado = new Peluquero();
            seleccionado = ListPeluquero.Find(x => x.nombre == nombre);
            return seleccionado.cola;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ListPeluquero.Add(new Peluquero { nombre = "Aprendiz", estado = "Libre", finTiempoAtencion = null, demoraMinima = Convert.ToInt32(txtDemMinAprendiz.Text), demoraMaxima = Convert.ToInt32(txtDemMaxAprendiz.Text) });
            ListPeluquero.Add(new Peluquero { nombre = "Veterano A", estado = "Libre", finTiempoAtencion = null, demoraMinima = Convert.ToInt32(txtDemMinVetA.Text), demoraMaxima = Convert.ToInt32(txtDemMaxVetA.Text) });
            ListPeluquero.Add(new Peluquero { nombre = "Veterano B", estado = "Libre", finTiempoAtencion = null, demoraMinima = Convert.ToInt32(txtDemMinVetB.Text), demoraMaxima = Convert.ToInt32(txtDemMaxVetB.Text) });
        }
    }
}