using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Prj_Capa_Datos;
using Prj_Capa_Negocio;
using MicroSisPlani.Msm_Forms;


namespace MicroSisPlani
{
    public partial class Frm_Login : Form
    {
        public Frm_Login()
        {
            InitializeComponent();
        }

        private void Frm_Login_Load(object sender, EventArgs e)
        {

        }

        private void btn_Aceptar_Click(object sender, EventArgs e)
        {
            Verificar_Acceso();
        }

        private bool ValidarTexbox()
        {
            Frm_Filtro fil = new Frm_Filtro();
            if(txt_usu.Text.Trim().Length==0)
            {
                fil.Show();
                MessageBox.Show("ingresa tu usuario","Login",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                fil.Hide();
                txt_usu.Focus();
                return false;
            }
            if (txt_usu.Text.Trim().Length == 0)
            {
                fil.Show();
                MessageBox.Show("ingresa tu usuario", "Login", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                fil.Hide();
                txt_pass.Focus();
                return false;
            }
            return true;
        }

        private void Verificar_Acceso()
        {
            RN_Usuario obj = new RN_Usuario();
            DataTable dt = new DataTable();

            int veces = 0;

            if (ValidarTexbox() == false) return;
            string usu, pass;

            usu = txt_usu.Text;
            pass = txt_pass.Text;

            if(obj.RN_Verificar_Acceso(usu,pass)==true)
            {
                Cls_Libreria.Usuario = usu;

                dt = obj.RN_Leer_Datos_Usuario(usu);

                if(dt.Rows.Count >0)
                {
                    DataRow dr= dt.Rows[0];
                    Cls_Libreria.IdRol = Convert.ToString(dr["Id_Usu"]);
                    Cls_Libreria.Apellidos = dr["Nombre_Completo"].ToString();
                    Cls_Libreria.IdRol = Convert.ToString(dr["Id_Rol"]);
                    Cls_Libreria.Rol = dr["NomRol"].ToString();
                    Cls_Libreria.Foto = dr["Avatar"].ToString();


                }
                this.Hide();
                Frm_Principal pri = new Frm_Principal();

                pri.Show();
                pri.Cargar_Datos_Usuario();
                pri.Verificar_Robot_De_Faltas();


            }
            else
            {
                txt_pass.Text = "";
                txt_usu.Text = "";
                MessageBox.Show("Usuario o Contraseña no son validas:", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txt_usu.Focus();
                veces += 1;

                if(veces ==3)
                {
                    MessageBox.Show("El nro maximo de intentos fue superado", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Application.Exit();
                }
            }


        }

        private void txt_usu_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter) 
            {
                txt_pass.Focus();
            }
        }

        private void txt_pass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_Aceptar_Click(sender, e);
            }
        }

        private void btn_Salir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pnl_titulo_MouseMove(object sender, MouseEventArgs e)
        {
            Utilitarios ui = new Utilitarios();
            if(e.Button==MouseButtons.Left)
            {
                ui.Mover_formulario(this);
            }
        }
    }
}
