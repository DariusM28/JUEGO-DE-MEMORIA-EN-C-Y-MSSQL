using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Data.SqlClient;

namespace Memoria_juego
{
  
    public partial class PanelJuego : Form
    {
        ClsConexion conex =new ClsConexion();//LLAMAMOS A LA CLASE
        Stopwatch crono = new Stopwatch();// ESTA CLASE NOS AYUDA A HACER UN CRONOMETRO 
        
        // DECLARAMSO LAS VARIABLES A UTILIZAR
        int errores = 0;
        int aciertos = 0;
        int TamanioColumnasFilas = 4;
        int Movimientos = 0;
        int CantidadDeCartasVolteadas = 0;
        List<string> CartasEnumeradas;
        List<string> CartasRevueltas;
        ArrayList CartasSeleccionadas;
        PictureBox CartaTemporal1;
        PictureBox CartaTemporal2;
        int CartaActual = 0;
        public PanelJuego()
        {
            InitializeComponent();
            iniciarJuego();//CUANDO EL FORMULARIO APARECE YA ESTA DISPONIBLE LAS CAJAS DE IMAGEN PARA INICIAR EL JUEGO
        }
        // ESTA FUNCION PERMITE QUE INICIE EL CRONOMETRO PARA MEDIR EL TIEMPO DEL JUGADOR
        public void IniciarTiempo()
        {
            crono.Start();
            TimerCronometro.Enabled = true;
        }
        // ESTA FUNCION PERMITE HACER UN LA INSERCION DE DATOS DE CADA JUGADOR
        public void AgregarDatos()
        {
            crono.Stop();
            string query = "INSERT INTO INFORMACION_JUGADOR(ALIAS,MOVIMIENTOS,ERRORES,ACIERTOS,TIEMPO)VALUES('" + label2.Text + "'," + lblRecord.Text + "," + labelErrores.Text + "," + labelAciertos.Text + ",'" + lblTiempo.Text + "');";
            SqlCommand comando = new SqlCommand(query, conex.Conectar());
            comando.ExecuteNonQuery();// EJECUTA LA SENTENCIA SQL PARA AGREGAR LOS DATOS EN LA TABLA
            // CERRAMOS ESTE FORMULARIO PARA QUE SE PUEDA INGRESAR OTRO JUGADOR AL FINALIZAR LA PARTIDA
            Form1 principal = new Form1();
            principal.Show();
            this.Close();
            this.Hide();
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            TimeSpan reloj = new TimeSpan(0, 0, 0, 0, (int)crono.ElapsedMilliseconds);// INICIALIZAMOS UNA VARIABLE CON LOS PARAMETROS DE DIAS,HORAS,MINUTOS,SEGUNDOS,MILISEGUNDOS
            string hh = reloj.Hours.ToString().Length < 2 ? "0" + reloj.Hours.ToString() : reloj.Hours.ToString();// MIENTRAS EL TIMER ESTE EN FUNCIION ASIGNAMOS Y MOSTRAMOS EL TIEMPO 
            string mm = reloj.Minutes.ToString().Length < 2 ? "0" + reloj.Minutes.ToString() : reloj.Minutes.ToString();
            string ss = reloj.Seconds.ToString().Length < 2 ? "0" + reloj.Seconds.ToString() : reloj.Seconds.ToString();

            lblTiempo.Text = hh + ":" + mm + ":" + ss;// CONCATENAMOS PARA MOSTRAR EL TIEMPO DEL RECORRIDO EN TIEMPO REAL
        }
        public void iniciarJuego()// HACEMOS UN METODO PUBLICO PARA INICIAR EL JUEGO
        {
            timer1.Enabled = false;
            timer1.Stop();
            lblRecord.Text = "0";
            CantidadDeCartasVolteadas = 0;
            Movimientos = 0;
            pjuego.Controls.Clear();
            CartasEnumeradas = new List<string>();
            CartasRevueltas = new List<string>();
            CartasSeleccionadas = new ArrayList();
            for (int i = 0; i < 8; i++)
            {
                CartasEnumeradas.Add(i.ToString());
                CartasEnumeradas.Add(i.ToString());//LA SEGUNDA LISTA TENDRA LOS MISMOS VALORES DE LA PRIMERA PERO EN DIFERENTE ORDEN Y REVUELTAS
            }
            var NumeroAleatorio = new Random();// HACEMOS QUE SE REVULCAN CON NUMEROS ALEATORIOS Y PASANDO AL SIGUIENTE EJEMPLO: 4,6,5,1,2,7,3,8
            var Resultado = CartasEnumeradas.OrderBy(item => NumeroAleatorio.Next());
            foreach (string ValorCarta in Resultado)
            {
                CartasRevueltas.Add(ValorCarta);// SE AÑADE EL NUMERO ALEATORIAO AL OBJETO
            }
            // EN RESUMEN CREMAOS UNA MATRIZ DE 4X4
            var tablaPanel = new TableLayoutPanel();
            tablaPanel.RowCount = TamanioColumnasFilas;
            tablaPanel.ColumnCount = TamanioColumnasFilas;
            for (int i = 0; i < TamanioColumnasFilas; i++)
            {
                // NOS ENCARGAMOS QUE EL TAMAÑO DE CADA CAJA DE IMAGEN SEA PROPORCIONAL AL TAMAÑO DEL PANEL
                var Porcentaje = 150f / (float)TamanioColumnasFilas - 10;
                tablaPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, Porcentaje));
                tablaPanel.RowStyles.Add(new RowStyle(SizeType.Percent, Porcentaje));
            }
            int contadorFichas = 1;
            // ASIGNAMOS LOS VALORES A CADA CAJA DE IMAGEN POR MEDIO DE UNA MATRIZ
            for (var i = 0; i < TamanioColumnasFilas; i++)
            {
                for (var j = 0; j < TamanioColumnasFilas; j++)
                {
                    var CartasJuego = new PictureBox();
                    CartasJuego.Name = string.Format("{0}", contadorFichas);
                    CartasJuego.Dock = DockStyle.Fill;
                    CartasJuego.SizeMode = PictureBoxSizeMode.StretchImage;
                    CartasJuego.Image = Properties.Resources.Girada;
                    CartasJuego.Cursor = Cursors.Hand;
                    CartasJuego.Click += btnCarta_Click;
                    tablaPanel.Controls.Add(CartasJuego, j, i);
                    contadorFichas++;
                }
            }
            tablaPanel.Dock = DockStyle.Fill;
            pjuego.Controls.Add(tablaPanel);


        }
       
        private void btnCarta_Click(object sender, EventArgs e)
        {
            IniciarTiempo();// CUANDO EL JUGADOR DE CLICK EN ALGUNA IMAGEN EL TIEMPO EMPEZARA A CORRER

            if (CartasSeleccionadas.Count < 2)
            {
                Movimientos++;//SE INCREMENTA CUANDO EL JUAGOR DA CLICK(CUENTA LOS CLICKS)
                lblRecord.Text = Convert.ToString(Movimientos);
                var CartasSeleccionadasUsuario = (PictureBox)sender;

                CartaActual = Convert.ToInt32(CartasRevueltas[Convert.ToInt32(CartasSeleccionadasUsuario.Name) - 1]);
                CartasSeleccionadasUsuario.Image = RecuperarImagen(CartaActual);
                CartasSeleccionadas.Add(CartasSeleccionadasUsuario);
                //  2 Veces se realizo el evento del click
                if (CartasSeleccionadas.Count == 2)
                {
                    CartaTemporal1 = (PictureBox)CartasSeleccionadas[0];
                    CartaTemporal2 = (PictureBox)CartasSeleccionadas[1];
                    int Carta1 = Convert.ToInt32(CartasRevueltas[Convert.ToInt32(CartaTemporal1.Name) - 1]);
                    int Carta2 = Convert.ToInt32(CartasRevueltas[Convert.ToInt32(CartaTemporal2.Name) - 1]);
                    // SI LAS CAJAS DE IMAGEN SON DISTINTAS AUMENTA UN ERROR
                    if (Carta1 != Carta2)
                    {
                        timer1.Enabled = true;
                        timer1.Start();
                        errores++;
                        labelErrores.Text= errores.ToString();  
                    }
                    else
                    {// EN CASO CONTRARIO SE AUMENTA LOS ACIERTOS
                        aciertos++;
                        labelAciertos.Text = aciertos.ToString();
                        CantidadDeCartasVolteadas++;
                        if (CantidadDeCartasVolteadas > 7)// COMO SON 8 IMAGENES CUANDO SUPERE O SEA MAYOR QUE 7 SE DETENDRA EL JUEGO Y EL TIEMPO PARA ALMACENAR LOS DATOS
                        {
                            crono.Stop();
                            MessageBox.Show("Se termino el juego sus datos has sido guardados","FIN DEL JUEGO",MessageBoxButtons.OK);
                            AgregarDatos();// LLAMOS A LA FUNCION PARA HACER LA INSERCION EN LA TABLA


                        }
                        CartaTemporal1.Enabled = false; CartaTemporal2.Enabled = false;// COMO LAS CARTAS FUERON ACERTADAS LAS BLOQUEMOS PARA QUE NO HAGA NADA MAS
                        CartasSeleccionadas.Clear();

                    }


                }
            }

        }
        // ESTA FUNCIOS NOS PERMITE OBTENER LA IMAGENS AGREGADAS EN RECUROS Y (A CADA IMAGEN LE CAMBIAMOS LA PROPIEDAD DE NOMBRE PARA DETENCTARLO TODOS TIENEN LA BASE DE IMG+UN NUMERO )
        public Bitmap RecuperarImagen(int NumeroImagen)
        {
            Bitmap TmpImg = new Bitmap(200, 100);
            switch (NumeroImagen)
            {
                case 0:
                    TmpImg = Properties.Resources.img11;
                    break;
                default:
                    TmpImg = (Bitmap)Properties.Resources.ResourceManager.GetObject("img" + NumeroImagen);// RECUPERAMOS LA IMAGEN GRACIAS AL MAPA DE BITS DESDE RECURSOS
                    break;
            }
            return TmpImg;

        }

       
          private void timer1_Tick_1(object sender, EventArgs e)
        {
            int TiempoVirarCarta = 1;
            if (TiempoVirarCarta == 1)
            {
                CartaTemporal1.Image = Properties.Resources.Girada;
                CartaTemporal2.Image = Properties.Resources.Girada;
                CartasSeleccionadas.Clear();
                TiempoVirarCarta = 0;
                timer1.Stop();

            }
        }
        // ESTE BOTON ES PARA SALIR DE LA PARTIDA PERO DEJAR REGISTRO DE LA PARTIDA
        private void button1_Click(object sender, EventArgs e)
        {
            AgregarDatos();
            MessageBox.Show("Se termino el juego sus datos has sido guardados", "FIN DEL JUEGO", MessageBoxButtons.OK);

        }
    }
}
