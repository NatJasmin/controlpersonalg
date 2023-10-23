using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Prj_Capa_Entidad;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;


namespace Prj_Capa_Datos
{
 public class BD_Asistencia : Cls_Conexion
    {
        public static bool entrada = false;
        public void BD_Registrar_entrada(EN_Asistencia asi)
        {
            MySqlConnection cn = new MySqlConnection(Conectar());
            MySqlCommand cmd = new MySqlCommand("sp_Registrar_Entrada", cn);
            try
            {
                cmd.CommandTimeout = 20;
                cmd.CommandType = CommandType.StoredProcedure;
                //parametros
                cmd.Parameters.AddWithValue("xId_asis", asi.IdAsistencia);
                cmd.Parameters.AddWithValue("xId_Pernl", asi.Id_Personal);
                cmd.Parameters.AddWithValue("xNombre_dia", asi.Nombre_Dia);
                cmd.Parameters.AddWithValue("xHoIngreso", asi.HoraIngre);
                cmd.Parameters.AddWithValue("xjustifi", asi.Justificacion);
                cmd.Parameters.AddWithValue("xtardanza", asi.Tardanza);

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();

                entrada = true;
                
            }
            catch (Exception ex)
            {
                entrada = false;
                if(cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
                MessageBox.Show("hay problemas : " + ex.Message, "Marcar Entrada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            
        }


        public static bool salida = false;
        public void BD_Registrar_salida(EN_Asistencia asi)
        {
            MySqlConnection cn = new MySqlConnection(Conectar());
            MySqlCommand cmd = new MySqlCommand("sp_registrar_salida", cn);
            try
            {
                cmd.CommandTimeout = 20;
                cmd.CommandType = CommandType.StoredProcedure;
                //parametros
                cmd.Parameters.AddWithValue("xId_asis", asi.IdAsistencia);
                cmd.Parameters.AddWithValue("xId_Pernl", asi.Id_Personal);
                cmd.Parameters.AddWithValue("xHoSalida", asi.HoraSalida);
                cmd.Parameters.AddWithValue("xtlHo_traba", asi.TotalHoras);
               

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();

                salida = true;

            }
            catch (Exception ex)
            {
                salida = false;
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
                MessageBox.Show("hay problemas : " + ex.Message, "Marcar Entrada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


        }
        public bool BD_verificar_si_Personal_YaMarco_su_Asistencia(string idper)
        {
            bool retornocarro = false;
            Int32 resultado = 0;

            MySqlConnection cn = new MySqlConnection();
            MySqlCommand cmd = new MySqlCommand();

            cn.ConnectionString = Conectar();

            cmd.CommandText = " sp_verificar_si_Personal_YaMarco_su_Asistencia";
            cmd.Connection = cn;
            cmd.CommandTimeout = 20;
            cmd.CommandType = CommandType.StoredProcedure;
            //parametros
            cmd.Parameters.AddWithValue("xidperso", idper);

            try
            {
                cn.Open();
                resultado = Convert.ToInt32(cmd.ExecuteScalar());
                if(resultado > 0)
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
                entrada = false;
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
                MessageBox.Show("hay problemas : " + ex.Message, "Marcar Entrada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return retornocarro; 
        }


        public bool BD_verificar_si_Personal_YaMarco_su_Falta(string idper)
        {
            bool retornocarro = false;
            Int32 resultado = 0;

            MySqlConnection cn = new MySqlConnection();
            MySqlCommand cmd = new MySqlCommand();

            cn.ConnectionString = Conectar();

            cmd.CommandText = "sp_verificar_si_Personal_YaMarco_su_Falta";
            cmd.Connection = cn;
            cmd.CommandTimeout = 20;
            cmd.CommandType = CommandType.StoredProcedure;
            //parametros
            cmd.Parameters.AddWithValue("xidperso", idper);

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
                entrada = false;
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
                MessageBox.Show("hay problemas : " + ex.Message, "Marcar Entrada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return retornocarro;
        }



        public bool BD_verificar_si_Personal_YaMarco_su_Entrada(string idper)
        {
            bool retornocarro = false;
            Int32 resultado = 0;

            MySqlConnection cn = new MySqlConnection();
            MySqlCommand cmd = new MySqlCommand();

            cn.ConnectionString = Conectar();

            cmd.CommandText = "sp_verificar_si_Personal_YaMarco_su_Entrada";
            cmd.Connection = cn;
            cmd.CommandTimeout = 20;
            cmd.CommandType = CommandType.StoredProcedure;
            //parametros
            cmd.Parameters.AddWithValue("xidperso", idper);

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
                entrada = false;
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
                MessageBox.Show("hay problemas : " + ex.Message, "Marcar Entrada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return retornocarro;
        }

        public void BD_Registrar_Falta(string idasis, string idper, string justi, string nomdia)
        {
            MySqlConnection cn = new MySqlConnection(Conectar());
            MySqlCommand cmd = new MySqlCommand("sp_registrar_Falta", cn);
            try
            {
                cmd.CommandTimeout = 20;
                cmd.CommandType = CommandType.StoredProcedure;
                //parametros
                cmd.Parameters.AddWithValue("_Id_Asis",idasis);
                cmd.Parameters.AddWithValue("_Id_Personal", idper);
                cmd.Parameters.AddWithValue("_justificacion", justi);
                cmd.Parameters.AddWithValue("_nomdia",nomdia);
                

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();

                entrada = true;

            }
            catch (Exception ex)
            {
                entrada = false;
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
                MessageBox.Show("hay problemas : " + ex.Message, "Marcar Entrada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


        }

        public DataTable BD_Buscar_Asistencia_deEntrada(String idperso)
        {
            MySqlConnection xcn = new MySqlConnection();
            try
            {
                xcn.ConnectionString = Conectar();
                MySqlDataAdapter da = new MySqlDataAdapter("sp_Listar_Asistencia_RecienCreada", xcn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("xidper", idperso);
                DataTable Dato = new DataTable();
                da.Fill(Dato);
                da = null;
                return Dato;
            }
            catch(Exception ex)
            {
                if(xcn.State== ConnectionState.Open)
                {
                    xcn.Close();
                }
                MessageBox.Show("Algo malo pasó:"+ex.Message,"Advertencia de Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }

        public DataTable BD_Listar_Todas_Asistencias()
        {
            MySqlConnection cn = new MySqlConnection();
            try
            {
                cn.ConnectionString = Conectar();
                MySqlDataAdapter da = new MySqlDataAdapter("Sp_listar_Todas_Asistencias", cn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable Dato = new DataTable();

                da.Fill(Dato);
                da = null;
                return Dato;
            }
            catch(Exception ex)
            {
                if(cn.State==ConnectionState.Open)
                {
                    cn.Close();
                   
                }
                MessageBox.Show("Algo malo pasó:" + ex.Message, "Advertencia de Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }


        public DataTable BD_Buscador_de_Asistencias(string xvalor)
        {
            MySqlConnection cn = new MySqlConnection();
            try
            {
                cn.ConnectionString = Conectar();
                MySqlDataAdapter da = new MySqlDataAdapter("Sp_Buscador_de_Asistencias", cn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("xValor", xvalor);
                DataTable Dato = new DataTable();

                da.Fill(Dato);
                da = null;
                return Dato;
            }
            catch (Exception ex)
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();

                }
                MessageBox.Show("Algo malo pasó:" + ex.Message, "Advertencia de Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }



        public void BD_Eliminar_asistencia(string idasi)
        {
            MySqlConnection cn = new MySqlConnection(Conectar());
            MySqlCommand cmd = new MySqlCommand("sp_eliminar_asistencia", cn);
            try
            {
                cmd.CommandTimeout = 20;
                cmd.CommandType = CommandType.StoredProcedure;
                //parametros
                cmd.Parameters.AddWithValue("_idasis", idasi);
                //cmd.Parameters.AddWithValue("xId_Pernl", asi.Id_Personal);
                //cmd.Parameters.AddWithValue("xHoSalida", asi.HoraSalida);
                //cmd.Parameters.AddWithValue("xtlHo_traba", asi.TotalHoras);


                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();

                salida = true;

            }
            catch (Exception ex)
            {
                salida = false;
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
                MessageBox.Show("hay problemas : " + ex.Message, "Marcar Entrada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


        }

        public DataTable BD_Listar_Asistencia_deldia(DateTime xdia)
        {
            MySqlConnection cn = new MySqlConnection();
            try
            {
                cn.ConnectionString = Conectar();
                MySqlDataAdapter da = new MySqlDataAdapter("sp_Listar_Asistencia_deldia", cn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("_fechahoy", xdia);
                DataTable Dato = new DataTable();

                da.Fill(Dato);
                da = null;
                return Dato;
            }
            catch (Exception ex)
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();

                }
                MessageBox.Show("Algo malo pasó:" + ex.Message, "Advertencia de Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }
    }

}
