using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Memoria_juego
{
    internal class ClsConexion
    {


        static string servidor = "DRH";//nombre del servidor de la base de datos
        static string BaseD = "JUEGO_MEMORIA";//nombre de la base de datos

        string cadenaC = "Data Source=" + servidor + ";Initial Catalog=" + BaseD + ";Integrated Security=True";//se concatena para tener una cadena completa
        // hacemos un metodo public de tipo sql para poder conectarnos
        public SqlConnection Conectar()
        {

            SqlConnection Mconexion = new SqlConnection(cadenaC);// cremos un nuevo espacio en memoria con la cadena de coneccion

            Mconexion.Open(); // aperturamos la coneccion
            
            return Mconexion;// se retorna ya que se necesita devolver un valor tipo sql

        }
    }
}
