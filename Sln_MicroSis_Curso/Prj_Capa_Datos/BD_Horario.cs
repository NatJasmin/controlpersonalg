using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using Prj_Capa_Entidad;
using System.Windows.Forms;

namespace Prj_Capa_Datos
{
 public class BD_Horario : Cls_Conexion
    {
        public static bool seguardo = false;
        public void BD_actualizarHorario(EN_Horario P)
        {
            MySqlConnection cn = new MySqlConnection();
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                cn.ConnectionString = Conectar();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_actualizar_horario";
                cmd.Connection = cn;
                cmd.CommandTimeout = 20;
                //parametros
                cmd.Parameters.AddWithValue("xId_Hor", P.Idhora);
                cmd.Parameters.AddWithValue("xHoEntrda", P.HoEntrada);
                cmd.Parameters.AddWithValue("xMiTolerancia", P.HoTole);
                cmd.Parameters.AddWithValue("xHoraLimite", P.HoLimite);
                cmd.Parameters.AddWithValue("xHoSalida", P.HoSalida);

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();

                cmd.Dispose();
                cmd = null;
                cn.Dispose();
                cn = null;
                seguardo = true;

            }
            catch (Exception ex)
            {
                seguardo = false;
                MessageBox.Show("Hay error al editar" + ex.Message, "Informe del sistema", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                if (cn.State == ConnectionState.Open) cn.Close();
                cmd.Dispose();
                cmd = null;
                cn.Dispose();
                cn = null;

            }
        }
        public DataTable BD_Leer_Horarios()
        {
            MySqlConnection Cn = new MySqlConnection();
            try
            {
                Cn.ConnectionString = Conectar();
                MySqlDataAdapter Da = new MySqlDataAdapter("sp_listarHorario", Cn);
                Da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable Datos = new DataTable();
                Da.Fill(Datos);
                Da = null;
                return Datos;

            }
            catch(Exception ex)
            {
                MessageBox.Show("Hay error al consultar horario" + ex.Message, "Informe del sistema", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                if (Cn.State == ConnectionState.Open) Cn.Close();
                Cn.Dispose();
                Cn = null;
                return null;

            }
        }


       






    }
}
