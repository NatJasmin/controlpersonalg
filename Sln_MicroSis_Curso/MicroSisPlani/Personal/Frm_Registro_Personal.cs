
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

namespace MicroSisPlani.Personal
{
    public partial class Frm_Registro_Personal : Form
    {
        public Frm_Registro_Personal()
        {
            InitializeComponent();
        }

           
      
        private void Frm_Registro_Personal_Load(object sender, EventArgs e)
        {
            txt_IdPersona.Text = RN_Utilitario.RN_NroDoc(2);
            Listar_Roles();
            Listar_Distritos();
          
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

                obj.RN_Registar_Personal(per);

                Actualizar_SiguienteNumero(2);

                MessageBox.Show("Datos guardados");
            }
            catch (Exception ex)
            {

            }

        }

       /* private void Pic_persona_Click(Object sender , EventArgs e)
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
        }*/

        private void btn_aceptar_Click(object sender, EventArgs e)
        {
            Registrar_Personal();
        }

        private double GenerarNextId(string numero)
        {
            double newnum = Convert.ToDouble(numero)+1;
            return newnum;
        }
        private void Actualizar_SiguienteNumero(int idtipo)
        {
            string xnum = BD_Utilitario.BD_Leer_Solo_Numero(idtipo);
            string xnuevonum = Convert.ToString(GenerarNextId(xnum));
            int td = xnuevonum.Length;
            string NuevoCorrelativo = "";

            if(xnuevonum.Length <5)
            {
                if(td==1)
                {
                    NuevoCorrelativo = "0000" + xnuevonum;
                }
                if (td == 2)
                {
                    NuevoCorrelativo = "000" + xnuevonum;
                }
                if (td == 3)
                {
                    NuevoCorrelativo = "00" + xnuevonum;
                }
                if (td == 4)
                {
                    NuevoCorrelativo = "0" + xnuevonum;
                }
            }

            BD_Utilitario.BD_ActualizarNro(idtipo, NuevoCorrelativo);
        }

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
            }
        }
    }

