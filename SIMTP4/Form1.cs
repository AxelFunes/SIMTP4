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
            public enum Nombre { VeteranoA, VeteranoB, Aprendiz };
            public Nombre nombre;
            public enum Estado { Libre, Ocupado };
            public Estado estado;
            public double finTiempoAtencion;
            public double tiempoAtencion;
            public int cola;
            public int demoraMinima;
            public int demoraMaxima;
        }
        public class Cliente
        {
            public int numero;
            public enum Estado { EsperandoAtencion, SiendoAtendido, Cancelado, Desocupado };
            public Estado estado;
            public string Peluquero;
            public double? TiempoEspera;
            public enum EstaEnCola { Si, No }
            public EstaEnCola esta_en_cola;
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
        List<Cliente> listaCliente = new List<Cliente>();
        List<Peluquero> listaPeluquero = new List<Peluquero>();
        string estadoVetA = "";
        string estadoVetB = "";
        string estadoAprendiz = "";



        //Punto B
        int cantClientesEnCola;
        int cantMasAltaDeClientesCola;
        //Variables para Generar la simulacion de colas
        string Evento;
        string Evento_Anterior;
        double Reloj = 0;
        double Reloj_Anterior;

        //tiempo entre llegadas
        private int demoraMinima;
        private int demoraMaxima;
        private double demoraCliente;
        private Random rnd = new Random();

        //simulacion
        private Peluquero peluqueroSeleccionado;
        private double Menor_Hora_Proximo_Evento;
        private string nombrePeluquero;
        private double acumOcupacionAprendiz;
        private int contadorCliente;
        private string supera8hs;
        private double menorTiempoFin;
        private int contadorClienteAtendido;

        private double desde;
        private double hasta;
        private int contadorIteraciones = 0;
        private int contadorIteracionesTotales = 0;

        //Variables para Llegada de Proximo Cliente
        double randomDemoraCliente;
        double randomDemoraPeluquero;
        double Tiempo_Entre_Llegada;
        double Proxima_Llegada;

        //Variables para Fin de Atencion
        double Random_Tiempo_Atencion;
        double Tiempo_Atencion;
        double? finAtencionAprendiz;
        double? finAtencionVeteranoA;
        double? finAtencionVeteranoB;
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
            listaCliente.Clear();
            dgv_simulaciones.Rows.Clear();
            try
            {
                if (txtTiempoSimulacion.Text != "") //faltan validaciones
                {

                    dias = Convert.ToInt32(txtTiempoSimulacion.Text);//cant de dias a simular
                                                                     //Variable_3 = true; Ver que hace

                    desde = Convert.ToDouble(txt_desde.Text);
                    hasta = Convert.ToDouble(txt_hasta.Text);

                    for (int i = 0; i < dias; i++)
                    {
                        Simulacion_Cero();
                        contadorIteracionesTotales++;
                        if (Reloj < 480 && contadorIteracionesTotales <= 100000)
                        {
                            while (Reloj < 480)
                            {
                                Comenzar();
                                contadorIteracionesTotales++;
                            }
                        }
                        //while (Reloj < 480)
                        //{
                        //    Comenzar();
                        //    contadorIteraciones++;
                        //}
                        else if (Reloj > 480 && contadorIteraciones <= 100000)
                        {
                            while (cantClientesEnCola > 0)
                            {
                                finAtencionOchoHoras();
                                contadorIteracionesTotales++;
                            }
                        }

                        //while (cantClientesEnCola > 0)
                        //{
                        //    Comenzar();
                        //    contadorIteraciones++;
                        //}
                        Reloj = 0;
                        //contadorIteraciones = 0;
                        //Comenzar();
                        //cargarGrilla();
                        //listaCliente.Clear();

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
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }

        private void Comenzar()
        {
            //ObtenerPeluquero();
            //Elegir_Menor_Para_Proximo_Evento();
            Reloj = Menor_Hora_Proximo_Evento;
            //CancelacionCliente();
            //TotalCLientesCola();
            if (Reloj < 480)
            {
                if (Evento == "Llegada Cliente")
                {

                    nombrePeluquero = ObtenerPeluquero();
                    foreach (var peluquero in listaPeluquero)
                    {
                        if (peluquero.nombre.ToString() == nombrePeluquero)
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
                if (Evento == "Fin Atencion")
                {
                    foreach (var peluquero in listaPeluquero)
                    {
                        if (peluquero.finTiempoAtencion == Menor_Hora_Proximo_Evento)
                        {
                            if (peluquero.cola > 0)
                            {
                                peluquero.cola--;
                                peluquero.estado = Peluquero.Estado.Libre;
                                peluquero.tiempoAtencion = 0;
                                peluquero.finTiempoAtencion = 0;

                                foreach (var cliente in listaCliente)
                                {
                                    //if (cliente.estado == "Siendo Atendido" && peluquero.idCliente == cliente.numero)
                                    //{
                                    //    cliente.estado = "Cancelado";
                                    //}

                                    if (cliente.estado == Cliente.Estado.EsperandoAtencion)
                                    {
                                        cliente.TiempoEspera = (Reloj - cliente.horaLlegada);
                                    }

                                    if (cliente.TiempoEspera >= 30 && cliente.estado == Cliente.Estado.EsperandoAtencion && peluquero.nombre.ToString()== cliente.Peluquero)
                                    {
                                        cliente.estado = Cliente.Estado.Cancelado;
                                        peluquero.cola--;
                                    }
                                    if (cliente.estado == Cliente.Estado.EsperandoAtencion && cliente.Peluquero == peluquero.nombre.ToString() && peluquero.estado == Peluquero.Estado.Libre)
                                    {
                                        cliente.estado = Cliente.Estado.SiendoAtendido;
                                        peluquero.estado = Peluquero.Estado.Ocupado;
                                        Tiempo_Atencion = CalcularDemora(peluquero.demoraMinima, peluquero.demoraMaxima, randomDemoraPeluquero);
                                        cliente.TiempoEspera = 0;
                                        peluquero.tiempoAtencion = Tiempo_Atencion;
                                        peluquero.finTiempoAtencion = (Tiempo_Atencion + Reloj);

                                        if (nombrePeluquero == "Aprendiz")
                                        {
                                            random_atencionAprendiz = randomDemoraPeluquero;
                                            finAtencionAprendiz = Tiempo_Atencion + Reloj;
                                            acumOcupacionAprendiz += Tiempo_Atencion;
                                        }
                                        else if (nombrePeluquero == "VeteranoA")
                                        {
                                            random_atencionVetA = randomDemoraPeluquero;
                                            finAtencionVeteranoA = Tiempo_Atencion + Reloj;
                                        }
                                        else { random_atencionVetB = randomDemoraPeluquero; finAtencionVeteranoB = Tiempo_Atencion + Reloj; }
                                        //AsignarClienteAPeluquero(peluquero);

                                    }
                                    if (cliente.estado == Cliente.Estado.SiendoAtendido && cliente.Peluquero == peluquero.nombre.ToString())
                                    {
                                        //listaCliente.Remove(cliente);
                                        cliente.estado = Cliente.Estado.Desocupado;
                                        contadorClienteAtendido = cliente.numero;
                                        //peluquero.estado = Peluquero.Estado.Libre;
                                        //idClienteAtendido = peluquero.idCliente;

                                    }
                                    break;
                                }

                                //clientePelu = listCliente.First(x => x.Peluquero == peluquero.nombre && x.estado == "Esperando Atencion");
                                //clientePelu.estado = "Siendo Atendido";
                                //idClienteAtendido = peluquero.idCliente;
                                //break;
                            }
                            else
                            {
                                peluquero.tiempoAtencion = 0;
                                peluquero.finTiempoAtencion = 0;
                                peluquero.estado = Peluquero.Estado.Libre;
                                foreach (var cliente in listaCliente)
                                {
                                    if (cliente.estado == Cliente.Estado.SiendoAtendido && cliente.Peluquero == peluquero.nombre.ToString())
                                    {
                                        //listaCliente.Remove(cliente);
                                        cliente.estado = Cliente.Estado.Desocupado;
                                        peluquero.estado = Peluquero.Estado.Libre;
                                        contadorClienteAtendido = cliente.numero;
                                        //idClienteAtendido = peluquero.idCliente;
                                    }
                                    break;
                                }
                                //idClienteAtendido = peluquero.idCliente;
                                //break;
                            }
                        }
                    }
                    //cargarGrilla();
                }

                //cargarGrilla();
                //}
                //if (Reloj >= desde)
                //{
                //    contadorIteraciones++;
                //    if (contadorIteraciones <= hasta)
                //    {
                //        cargarGrilla();
                //    }

                //}
                cargarGrilla();
                Elegir_Menor_Para_Proximo_Evento();
                CancelacionCliente();
                TotalCLientesCola();
            }
        }

        public void finAtencionOchoHoras()
        {
            supera8hs = "Si";
            foreach (var peluquero in listaPeluquero)
            {
                if (peluquero.finTiempoAtencion == Menor_Hora_Proximo_Evento)
                {
                    if (peluquero.cola > 0)
                    {
                        peluquero.cola--;
                        peluquero.estado = Peluquero.Estado.Libre;
                        //peluquero.estado = "Ocupado";
                        peluquero.tiempoAtencion = 0;
                        peluquero.finTiempoAtencion = 0;

                        foreach (var cliente in listaCliente)
                        {
                            //if (cliente.estado == "Siendo Atendido" && peluquero.idCliente == cliente.numero)
                            //{
                            //    cliente.estado = "Cancelado";
                            //}
                            if (cliente.estado == Cliente.Estado.SiendoAtendido && cliente.Peluquero == peluquero.nombre.ToString())
                            {
                                cliente.estado = Cliente.Estado.Desocupado;
                                peluquero.estado = Peluquero.Estado.Libre;
                                //listaCliente.Remove(cliente);
                                //idClienteAtendido = peluquero.idCliente;
                            }
                            if (cliente.estado == Cliente.Estado.EsperandoAtencion && cliente.Peluquero == peluquero.nombre.ToString())
                            {
                                cliente.estado = Cliente.Estado.SiendoAtendido;
                                peluquero.estado = Peluquero.Estado.Ocupado;
                                Tiempo_Atencion = CalcularDemora(peluquero.demoraMinima, peluquero.demoraMaxima, randomDemoraPeluquero);
                                cliente.TiempoEspera = 0;
                                peluquero.tiempoAtencion = Tiempo_Atencion;
                                peluquero.finTiempoAtencion = (Tiempo_Atencion + Reloj);
                                //peluquero.idCliente = contadorCliente;
                                if (nombrePeluquero == "Aprendiz")
                                {
                                    random_atencionAprendiz = randomDemoraPeluquero;
                                    finAtencionAprendiz = Tiempo_Atencion + Reloj;
                                    acumOcupacionAprendiz += Tiempo_Atencion;
                                }
                                else if (nombrePeluquero == "VeteranoA")
                                {
                                    random_atencionVetA = randomDemoraPeluquero;
                                    finAtencionVeteranoA = Tiempo_Atencion + Reloj;
                                }
                                else { random_atencionVetB = randomDemoraPeluquero; finAtencionVeteranoB = Tiempo_Atencion + Reloj; }
                                //AsignarClienteAPeluquero(peluquero);
                                break;
                            }
                        }
                        //clientePelu = listCliente.First(x => x.Peluquero == peluquero.nombre && x.estado == "Esperando Atencion");
                        //clientePelu.estado = "Siendo Atendido";
                        //idClienteAtendido = peluquero.idCliente;
                        //break;
                    }
                    else
                    {
                        peluquero.tiempoAtencion = 0;
                        peluquero.finTiempoAtencion = 0;
                        peluquero.estado = Peluquero.Estado.Libre;
                        //idClienteAtendido = peluquero.idCliente;
                        break;
                    }
                }

            }

            //cargarGrilla();

            //if (Reloj >= desde)
            //{
            //    contadorIteraciones++;
            //    if (contadorIteraciones <= hasta)
            //    {
            //        cargarGrilla();
            //    }

            //}
            cargarGrilla();
            Elegir_Menor_Para_Proximo_Evento();
            CancelacionCliente();
            TotalCLientesCola();
        }


        public void TotalCLientesCola()
        {
            cantClientesEnCola = 0;
            foreach (var peluca in listaPeluquero)
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
            listaCliente.RemoveAll(x=> x.estado==Cliente.Estado.Cancelado || x.estado==Cliente.Estado.Desocupado);
            foreach (var cliente in listaCliente)
            {
                if (cliente.estado == Cliente.Estado.EsperandoAtencion)
                {
                    cliente.TiempoEspera = (Reloj - cliente.horaLlegada);
                }

                else if (cliente.TiempoEspera >= 30 && cliente.estado == Cliente.Estado.EsperandoAtencion)
                {
                    nomPeluca = cliente.Peluquero;
                    cliente.estado = Cliente.Estado.Cancelado;
                    foreach (var peluquero in listaPeluquero)
                    {
                        if (peluquero.nombre.ToString() == nomPeluca) // && peluquero.cola > 0)
                        {
                            peluquero.cola--;
                        }
                        break;
                    }
                    //listaCliente.Remove(cliente);
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
            menorTiempoFin = listaPeluquero.Min(x => x.finTiempoAtencion);

            if (menorTiempoFin != 0)
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
            Menor_Hora_Proximo_Evento = 0;
            menorTiempoFin = 0;
            Proxima_Llegada = 0;
            contadorIteraciones = 1;

            supera8hs = "No";
            contadorCliente = 1;

            acumOcupacionAprendiz = 0;

            listaCliente.Clear();
            //cantClientesEnCola=0;
            //ObtenerPeluquero();
            calcularPrimerLlegada();

            //Proxima_Llegada = Tiempo_Entre_Llegada;
            //Proxima_Llegada_Anterior = Tiempo_Entre_Llegada;

            foreach (var item in listaPeluquero)
            {
                item.estado = Peluquero.Estado.Libre;
                item.finTiempoAtencion = 0;
                item.cola = 0;
                item.tiempoAtencion = 0;

            }


            Evento = "Inicializacion";

            Menor_Hora_Proximo_Evento = Proxima_Llegada;

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
                    nombrePeluquero = "VeteranoA";
                    return nombrePeluquero;
                }
                else
                {
                    nombrePeluquero = "VeteranoB";
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
            Proxima_Llegada = (double)(demoraCliente + Reloj);
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
            Proxima_Llegada = (double)(demoraCliente + Reloj);
            //contadorCliente++;
            //AsignarClienteAPeluquero(peluqueroSeleccionado, contadorCliente);
        }
        public void AsignarClienteAPeluquero(Peluquero seleccionado)
        {
            randomDemoraPeluquero = rnd.NextDouble();
            if (seleccionado.estado.ToString() == "Libre")
            {
                seleccionado.estado = Peluquero.Estado.Ocupado;
                Tiempo_Atencion = CalcularDemora(seleccionado.demoraMinima, seleccionado.demoraMaxima, randomDemoraPeluquero);
                Math.Round(Tiempo_Atencion, 4);
                listaCliente.Add(new Cliente { estado = Cliente.Estado.SiendoAtendido, numero = contadorCliente, Peluquero = seleccionado.nombre.ToString(), TiempoEspera = 0, horaLlegada = Reloj });
                seleccionado.tiempoAtencion = Tiempo_Atencion;
                seleccionado.finTiempoAtencion = (Tiempo_Atencion + Reloj);
                if (nombrePeluquero == "Aprendiz")
                {
                    random_atencionAprendiz = randomDemoraPeluquero;
                    finAtencionAprendiz = Tiempo_Atencion + Reloj;
                    acumOcupacionAprendiz += Tiempo_Atencion;
                }
                else if (nombrePeluquero == "VeteranoA")
                {
                    random_atencionVetA = randomDemoraPeluquero;
                    finAtencionVeteranoA = Tiempo_Atencion + Reloj;
                }
                else { random_atencionVetB = randomDemoraPeluquero; finAtencionVeteranoB = Tiempo_Atencion + Reloj; }
            }
            else
            {
                seleccionado.cola++;
                listaCliente.Add(new Cliente { numero = contadorCliente, estado = Cliente.Estado.EsperandoAtencion, Peluquero = seleccionado.nombre.ToString(), TiempoEspera = 0, horaLlegada = Reloj });
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
               , demoraCliente, Math.Round(Proxima_Llegada, 4), "", "", Math.Round(random_atencionAprendiz, 4), ObtenerTiempoAtencion("Aprendiz"), finAtencionAprendiz, Math.Round(random_atencionVetA, 4), ObtenerTiempoAtencion("VeteranoA"), finAtencionVeteranoA, Math.Round(random_atencionVetB, 4), ObtenerTiempoAtencion("VeteranoB"), finAtencionVeteranoB, ObtenerEstadoPeluquero("Aprendiz"), ObtenerColaPeluquero("Aprendiz"), ObtenerEstadoPeluquero("VeteranoA"), ObtenerColaPeluquero("VeteranoA"), ObtenerEstadoPeluquero("VeteranoB"), ObtenerColaPeluquero("VeteranoB"), Math.Round(acumOcupacionAprendiz, 4), cantMasAltaDeClientesCola);
            }
            else
            {
                dgv_simulaciones.Rows.Add(Evento, Math.Round((double)menorTiempoFin, 4), supera8hs, "-", "-", "-"
               , "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", ObtenerEstadoPeluquero("Aprendiz"), ObtenerColaPeluquero("Aprendiz"), ObtenerEstadoPeluquero("VeteranoA"), ObtenerColaPeluquero("VeteranoA"), ObtenerEstadoPeluquero("VeteranoB"), ObtenerColaPeluquero("VeteranoB"), Math.Round(acumOcupacionAprendiz, 4), cantMasAltaDeClientesCola);
            }

        }
        public double ObtenerTiempoAtencion(string nombre)
        {
            Peluquero seleccionado = new Peluquero();
            seleccionado = listaPeluquero.Find(x => x.nombre.ToString() == nombre);
            return seleccionado.tiempoAtencion;
        }
        public string ObtenerEstadoPeluquero(string nombre)
        {
            Peluquero seleccionado = new Peluquero();
            seleccionado = listaPeluquero.Find(x => x.nombre.ToString() == nombre);
            return seleccionado.estado.ToString();
        }
        public int ObtenerColaPeluquero(string nombre)
        {
            Peluquero seleccionado = new Peluquero();
            seleccionado = listaPeluquero.Find(x => x.nombre.ToString() == nombre);
            return seleccionado.cola;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listaPeluquero.Add(new Peluquero { nombre = Peluquero.Nombre.Aprendiz, estado = Peluquero.Estado.Libre, finTiempoAtencion = 0, demoraMinima = Convert.ToInt32(txtDemMinAprendiz.Text), demoraMaxima = Convert.ToInt32(txtDemMaxAprendiz.Text) });
            listaPeluquero.Add(new Peluquero { nombre = Peluquero.Nombre.VeteranoA, estado = Peluquero.Estado.Libre, finTiempoAtencion = 0, demoraMinima = Convert.ToInt32(txtDemMinVetA.Text), demoraMaxima = Convert.ToInt32(txtDemMaxVetA.Text) });
            listaPeluquero.Add(new Peluquero { nombre = Peluquero.Nombre.VeteranoB, estado = Peluquero.Estado.Libre, finTiempoAtencion = 0, demoraMinima = Convert.ToInt32(txtDemMinVetB.Text), demoraMaxima = Convert.ToInt32(txtDemMaxVetB.Text) });
        }
    }
}
