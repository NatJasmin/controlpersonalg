using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prj_Capa_Entidad;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Prj_Capa_Datos
{
    public class BD_Justificacion : Cls_Conexion
    {
        public static bool seguardo = false;
        public static bool seedito = false;
        public static bool seelimino = false;
        public void Bd_registrar_Justificacion(EN_Justificacion jus)
        {
            MySqlConnection cn = new MySqlConnection(Conectar());
            MySqlCommand cmd = new MySqlCommand("sp_registrar_justificacion", cn);
            try
            {
                cmd.CommandTimeout = 20;
                cmd.CommandType = CommandType.StoredProcedure;

                //parametros
                cmd.Parameters.AddWithValue("_Id_Justi", jus.IdJusti);
                cmd.Parameters.AddWithValue("_Id_Personal", jus.Id_Personal);
                cmd.Parameters.AddWithValue("_Principalmoti", jus.PrincipalMotivo);
                cmd.Parameters.AddWithValue("_Detalle", jus.Detalle);
                cmd.Parameters.AddWithValue("_FechaJusti", jus.Fecha);


                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();

                seguardo = true;
            }
            catch (Exception ex)
            {
                seguardo=false;
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close() ;
                }
                MessageBox.Show("Algo malo pasó: " + ex.Message, "Advertencia de Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        public void Bd_Actualizar_Justificacion(EN_Justificacion jus)
        {
            MySqlConnection cn = new MySqlConnection(Conectar());
            MySqlCommand cmd = new MySqlCommand("sp_Actualizar_justificacion", cn);
            try
            {
                cmd.CommandTimeout = 20;
                cmd.CommandType = CommandType.StoredProcedure;

                //parametros
                cmd.Parameters.AddWithValue("_Id_Justi", jus.IdJusti);
                cmd.Parameters.AddWithValue("_Principalmoti", jus.PrincipalMotivo);
                cmd.Parameters.AddWithValue("_Detalle", jus.Detalle);
                cmd.Parameters.AddWithValue("_FechaJusti", jus.Fecha);


                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();

                seedito = true;
            }
            catch (Exception ex)
            {
                seedito = false;
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
                MessageBox.Show("Algo malo pasó: " + ex.Message, "Advertencia de Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        public DataTable BD_Cargar_todos_Justificacion()
        {
            MySqlConnection xcn = new MySqlConnection();
            try
            {
                xcn.ConnectionString = Conectar();
                MySqlDataAdapter da = new MySqlDataAdapter("sp_Listar_Justificaciones", xcn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable Dato = new DataTable();
                da.Fill(Dato);
                da = null;
                return Dato;
            }
            catch (Exception ex)
            {
                if(xcn.State == ConnectionState.Open)
                {
                    xcn.Close();
                }
                MessageBox.Show("Algo malo pasó " + ex.Message, "Advertencia de seguridad", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;//fin de 2
        }



        public DataTable BD_BuscarJustificacion_porValor(string xdato)
        {
            MySqlConnection cn = new MySqlConnection();
            try
            {
                cn.ConnectionString = Conectar();
                MySqlDataAdapter da = new MySqlDataAdapter("sp_Buscador_Justificaciones", cn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("_valor", xdato);
                DataTable Dato = new DataTable();

                da.Fill(Dato);
                da = null;
                return Dato;
            }
            catch (Exception ex)
            {
                if (cn.State == ConnectionState.Open)
                cn.Close();
                cn = null;
                
                MessageBox.Show("Algo malo pasó " + ex.Message, "Advertencia de seguridad", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }

        public void Bd_Eliminar_Justificacion(string idjusti)
        {
            MySqlConnection cn = new MySqlConnection(Conectar());
            MySqlCommand cmd = new MySqlCommand("Sp_EliminarJustificacion", cn);
            try
            {
                cmd.CommandTimeout = 20;
                cmd.CommandType = CommandType.StoredProcedure;

                //parametros
                cmd.Parameters.AddWithValue("_idjusti",idjusti);
                
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();

                seelimino = true;
            }
            catch (Exception ex)
            {
                seelimino = false;
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
                MessageBox.Show("Algo malo pasó: " + ex.Message, "Advertencia de Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        public void Bd_Aprobar_Desaprobar_Justificacion(string idjusti, string estadojus)
        {
            MySqlConnection cn = new MySqlConnection(Conectar());
            MySqlCommand cmd = new MySqlCommand("sp_aprobarJustificacion", cn);
            try
            {
                cmd.CommandTimeout = 20;
                cmd.CommandType = CommandType.StoredProcedure;

                //parametros
                cmd.Parameters.AddWithValue("_idjusti", idjusti);
                cmd.Parameters.AddWithValue("_estadoJusti", estadojus);

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();

                seelimino = true;
            }
            catch (Exception ex)
            {
                seelimino = false;
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
                MessageBox.Show("Algo malo pasó: " + ex.Message, "Advertencia de Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        public bool BD_verificar_si_Personal_TieneJustificacion(string idper)
        {
            bool retornocarro = false;
            Int32 resultado = 0;

            MySqlConnection cn = new MySqlConnection();
            MySqlCommand cmd = new MySqlCommand();

            cn.ConnectionString = Conectar();

            cmd.CommandText = "SP_VerificarJustificacion_Aprobada";
            cmd.Connection = cn;
            cmd.CommandTimeout = 20;
            cmd.CommandType = CommandType.StoredProcedure;
            //parametros
            cmd.Parameters.AddWithValue("_Id_Personal", idper);

            try
            {
                cn.Open();
                resultado = Convert.ToInt32(cmd.ExecuteScalar());
                if (resultado > 0)
                {
                    retornocarro = true;
                }
                else
                {
                    retornocarro = false;
                }
                cmd.Parameters.Clear();
                cmd.Dispose();
                cmd = null;
                cn.Close();
                cn = null;

            }
            catch (Exception ex)
            {
               
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
                MessageBox.Show("hay problemas : " + ex.Message, "Marcar Entrada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return retornocarro;
        }








    }
}
