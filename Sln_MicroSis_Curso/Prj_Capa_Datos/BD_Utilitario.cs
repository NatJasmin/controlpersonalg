using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.ComponentModel;

namespace Prj_Capa_Datos
{
 public     class BD_Utilitario : Cls_Conexion 
    {

  public static string BD_NroDoc (int idtipo)
        {

            MySqlConnection cn = new MySqlConnection();
            try
            {
                cn.ConnectionString = Conectar2();
                MySqlCommand cmd = new MySqlCommand("Sp_Listado_tipo", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("_id_tipo", idtipo);
                string NroDoc = "";

                cn.Open();
                NroDoc = Convert.ToString(cmd.ExecuteScalar());
                return NroDoc;
            }
            catch (Exception ex)
            {
                if(cn.State == ConnectionState.Open)
                {
                    cn.Clone();
                }
                MessageBox.Show("no se pudo leer + "+ ex.Message , "Advertencia",MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
            return "";
        }

        public static void BD_ActualizarNro(int idtipo,string numero)
            {
                MySqlConnection cn = new MySqlConnection(Conectar2());
                MySqlCommand cmd = new MySqlCommand("sp_Actualiza_Tipo_Doc", cn);
            try
            {
                cmd.CommandTimeout = 20;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("_idtipo", idtipo);
                cmd.Parameters.AddWithValue("_numero", numero);


                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            catch (Exception ex)
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Clone();
                }
                MessageBox.Show("no se pudo leer + " + ex.Message, "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }

        }
        public static string BD_Leer_Solo_Numero(int idtipo)
        {
            MySqlConnection cn = new MySqlConnection();
            try
            {
                cn.ConnectionString=Conectar2();
                MySqlCommand cmd = new MySqlCommand("Sp_Listar_Solo_Numero", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("_idtipo", idtipo);

                string xNumero;

                cn.Open();
                xNumero = Convert.ToString( cmd.ExecuteScalar());
                return xNumero;
            }
            catch (Exception ex)
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Clone();
                }
                MessageBox.Show("no se pudo leer + " + ex.Message, "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
            return "";

        }

        public static string BD_Listar_TipoRobot(int idtipo)
        {
            MySqlConnection cn = new MySqlConnection();
            try
            {
                cn.ConnectionString = Conectar2();
                MySqlCommand cmd = new MySqlCommand("sp_listarRobot", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("_idtipo",idtipo);

                string tiporobot;

                cn.Open();
                tiporobot = Convert.ToString(cmd.ExecuteScalar());
                cn.Close();

                return tiporobot;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Advertencia de seguridad ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (cn.State == ConnectionState.Open)
                    cn.Close();
                    cn.Dispose();
                    cn = null;
                    return null;
            }

        }
        public static bool falta = false;

        public void Bd_Actualizar_TipoRobot(int idtipo, string serie)
        {
            MySqlConnection cn = new MySqlConnection(Conectar());
            MySqlCommand cmd = new MySqlCommand("sp_editarRobot",cn);
            try
            {
                cmd.CommandTimeout = 20;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("_idtipo",idtipo);
                cmd.Parameters.AddWithValue("_tipoRobot",serie);

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();

                falta = true;
            }
            catch (Exception ex)
            {
                falta = false;
                MessageBox.Show("Algo salió mal: " + ex.Message, "Advertencia de seguridad ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (cn.State == ConnectionState.Open)
                    cn.Close();
            }

        }





    }
}
      
