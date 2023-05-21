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
        
        double random_peluquero;
        double random_llegada;
        double random_atencionAprendiz;
        double random_atencionVetA;
        double random_atencionVetB;



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



        }

        public double calcularDemora(double a, double b, double rnd) //de atencion Y/O llegada
        {
            double unif = a + rnd * (b - a);
            return unif;
        }
    }
}
