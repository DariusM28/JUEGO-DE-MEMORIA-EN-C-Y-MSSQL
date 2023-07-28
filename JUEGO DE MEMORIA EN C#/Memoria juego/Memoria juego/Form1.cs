using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace Memoria_juego
{
    public partial class Form1 : Form
    {
        ClsConexion conex = new ClsConexion();
        public Form1()
        {
            InitializeComponent();
            MostrarRecords();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            MostrarRecords();
        }

        public void MostrarRecords()
        {
            string consulta = "EXECUTE RECORDS";// CREAMOS UN PROCESO ALMACENADO EN UN QUERY Y SOLO LO EJECUTAMOS EN UNA VARIABLE STRING
            SqlCommand sqlCommand = new SqlCommand(consulta,conex.Conectar());// INICIALIZAMOS UNA VARIBLE TIPO SQLCOMANDO CON LOS PARAMETROS DE LA CONSULTA Y LA CONECCION INSTANCIADA
            SqlDataAdapter dato = new SqlDataAdapter(sqlCommand);// LA VARIABLE DATO TENDRA LOS RESUTADOS DE LA SENTENCIA SQL
            DataTable tabla = new DataTable();
            dato.Fill(tabla);// LENAMOS LOS DATOS EN UNA NUEVA TABLA
            dataGridView1.DataSource=tabla;// MOSTRAMOS LOS RESULTADOS DE LOS JUGADORES
            
        } 
        private void button1_Click(object sender, EventArgs e)
        {
            if (TexboxNombreJugador.Text == " "||TexboxNombreJugador.Text=="" )// hacemos una comparacion para que el alias no qude vacio o sin nombre
            {
               /* mostramos un mensaje*/      MessageBox.Show("Error No Se Ingreso Alias Para Jugar","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                
            }
            else
            {// si cumple las caracteristicas se puede proceder a abrir el otro formulario
                PanelJuego instacia = new PanelJuego(); // hacemos una intancia para poder compartir el alias a un label de forms 2
                instacia.label2.Text = TexboxNombreJugador.Text;
                instacia.Show();
                this.Hide();

            }

        }

       
    }
}
