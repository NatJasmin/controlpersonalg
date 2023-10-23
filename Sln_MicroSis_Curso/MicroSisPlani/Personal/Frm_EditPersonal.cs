
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Prj_Capa_Negocio;
using Prj_Capa_Datos;
using Prj_Capa_Entidad;
using System.IO;

namespace MicroSisPlani.Personal
{
    public partial class Frm_EditPersonal : Form
    {
        public Frm_EditPersonal()
        {
            InitializeComponent();
        }

           
      
        private void Frm_EditPersonal_Load(object sender, EventArgs e)
        {
            
            Listar_Roles();
            Listar_Distritos();
            Buscar_Personal_paraEditar(this.Tag.ToString());
          
        }


        private void Listar_Roles ()
        {
            RN_Rol obj = new RN_Rol();
            DataTable dato = new DataTable();

            dato = obj.RN_Listar_todos_Roles();
            if (dato.Rows.Count >0)
            {
                var cbo = cbo_rol;

                cbo.DataSource = dato;
                cbo.DisplayMember = "NomRol";
                cbo.ValueMember = "Id_rol";
                cbo.SelectedIndex = -1;
            }
        }

        private void Listar_Distritos()
        {
            RN_Distrito obj = new RN_Distrito();
            DataTable dato = new DataTable();

            dato = obj.RN_Listar_Distritos();
            if(dato.Rows.Count >0)
            {
                var cbo = cbo_Distrito;

                cbo.DataSource = dato;
                cbo.DisplayMember = "Distrito";
                cbo.ValueMember = "Id_Distrito";
                cbo.SelectedIndex = -1;
            }



        }


        private void Buscar_Personal_paraEditar(string idper)
        {
            RN_Personal obj = new RN_Personal();
            DataTable data = new DataTable();
            string sex = "";

            data = obj.RN_Buscar_personal_porValor(idper);
            if (data.Rows.Count >0)
            {
                txt_IdPersona.Text = Convert.ToString(data.Rows[0]["Id_Pernl"]);
                txt_Dni.Text = Convert.ToString(data.Rows[0]["CI"]);
                txt_nombres.Text = Convert.ToString(data.Rows[0]["Nombre_Completo"]);
                txt_direccion.Text = Convert.ToString(data.Rows[0]["Domicilio"]);
                txt_correo.Text = Convert.ToString(data.Rows[0]["Correo"]);
                txt_NroCelular.Text = Convert.ToString(data.Rows[0]["Celular"]);
                dtp_fechaNaci.Value = Convert.ToDateTime(data.Rows[0]["Fec_Naci"]);
                sex= Convert.ToString(data.Rows[0]["Sexo"]);
                if(sex =="M")
                {
                    cbo_sexo.SelectedIndex = 0;
                }
                else if (sex =="F")
                {
                    cbo_sexo.SelectedIndex = 1;
                }
                cbo_rol.SelectedValue = data.Rows[0]["Id_rol"];
                cbo_Distrito.SelectedValue =data.Rows[0]["Id_Distrito"];
                txt_IdPersona.Text = Convert.ToString(data.Rows[0]["Id_Pernl"]);
                
                
                
                xfoto = Convert.ToString(data.Rows[0]["Foto"]);
                if(File.Exists(xfoto)==false)
                {
                    xfoto = Application.StartupPath + @"\user.png";
                    Pic_persona.Load(Application.StartupPath + @"\user.png");
                }
                else
                {
                    Pic_persona.Load(xfoto);
                }
            }
        }


        private void btn_Salir_Click(object sender, EventArgs e)
        {
            this.Tag = "";
            this.Close();

        }
        private void btn_cancelar_Click(object sender,EventArgs e )
        {
            this.Tag = "";
            this.Close();
        }

        string xfoto = "";
      
        private void Registrar_Personal()
        {
            RN_Personal obj = new RN_Personal();
            EN_Persona per=new EN_Persona();

            try
            {
                per.Idpersonal = txt_IdPersona.Text;
                per.Dni = txt_Dni.Text;
                per.Nombres = txt_nombres.Text;
                per.FechaNaci = dtp_fechaNaci.Value;
                if(cbo_sexo.SelectedIndex==0)
                {
                    per.Sexo = "M";
                }
                else if (cbo_sexo.SelectedIndex==1)
                {
                    per.Sexo = "F";
                }
                per.Direccion =txt_direccion.Text;
                per.Correo=txt_correo.Text;
                per.Celular = Convert.ToInt32(txt_NroCelular.Text);
                per.IdRol = cbo_rol.SelectedValue.ToString();
                per.xImagen = xfoto;
                per.IdDistrito = cbo_Distrito.SelectedValue.ToString();

                obj.RN_actualizar_Personal(per);

                MessageBox.Show("Datos Actualizados");

                this.Tag = "A";
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Pic_persona_Click(Object sender , EventArgs e)
        {

            var filepath = string.Empty;
            try
            {
                if(openFileDialog1.ShowDialog()==DialogResult.OK)
                {
                    xfoto = openFileDialog1.FileName;
                    Pic_persona.Load(xfoto);
                }
                else
                {
                    xfoto = Application.StartupPath + @"\user.png";
                    Pic_persona.Load(Application.StartupPath + @"\user.png");
                }
            }
            catch (Exception ex)
            {
                xfoto = Application.StartupPath + @"\user.png";
                Pic_persona.Load(Application.StartupPath + @"\user.png");
            }
        }

        private void btn_aceptar_Click(object sender, EventArgs e)
        {
            Registrar_Personal();
        }

       
        /*
        private void Pic_persona_Click(object sender, EventArgs e)
        {

                var filepath = string.Empty;
                try
                {
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        xfoto = openFileDialog1.FileName;
                        Pic_persona.Load(xfoto);
                    }
                    else
                    {
                        xfoto = Application.StartupPath + @"\user.png";
                        Pic_persona.Load(Application.StartupPath + @"\user.png");
                    }
                }
                catch (Exception ex)
                {
                    xfoto = Application.StartupPath + @"\user.png";
                    Pic_persona.Load(Application.StartupPath + @"\user.png");
                }
            }*/
        }
    }

