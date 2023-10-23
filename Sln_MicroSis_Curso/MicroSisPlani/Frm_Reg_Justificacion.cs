using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MicroSisPlani.Msm_Forms;
using Prj_Capa_Datos;
using Prj_Capa_Entidad;
using Prj_Capa_Negocio;


namespace MicroSisPlani
{
    public partial class Frm_Reg_Justificacion : Form
    {
        public Frm_Reg_Justificacion()
        {
            InitializeComponent();
        }

        private void Frm_Reg_Justificacion_Load(object sender, EventArgs e)
        {
           // txt_idjusti.Text = RN_Utilitario.RN_NroDoc(3);
        }

        private void pnl_titulo_MouseMove(object sender, MouseEventArgs e)
        {
            Utilitarios ui = new Utilitarios();
            if (e.Button == MouseButtons.Left)
            {
                ui.Mover_formulario(this);
            }
        }

        private void btn_cancelar_Click(object sender, EventArgs e)
        {
            this.Tag = "";
            this.Close();
        }

        private bool ValidarCampos()
        {
            Frm_Advertencia ver = new Msm_Forms.Frm_Advertencia();
            Frm_Filtro fil = new Msm_Forms.Frm_Filtro();

            if (txt_IdPersona.Text.Trim().Length <2)
            {
                fil.Show();
                ver.Lbl_Msm1.Text = "Falta el ID del personal";
                ver.ShowDialog();
                fil.Hide();
                txt_IdPersona.Focus();
                return false;
            }
            if (cbo_motivJusti.SelectedIndex==-1)
            {
                fil.Show();
                ver.Lbl_Msm1.Text = "Selecciona el motivo de tu Justificación";
                ver.ShowDialog();
                fil.Hide();
                cbo_motivJusti.Focus();
                return false;
            }
            if (txt_DetalleJusti.Text.Trim().Length==0)
            {
                fil.Show();
                ver.Lbl_Msm1.Text = "Agrega un pequeño resumen de tu Justificación";
                ver.ShowDialog();
                fil.Hide();
                txt_DetalleJusti.Focus();
                return false;
            }
            return true;
        }

        
        private void Registrar_Justificacion()
        {
            RN_Justificacion obj = new RN_Justificacion();
            EN_Justificacion jus = new EN_Justificacion();


            Frm_Advertencia ver = new Msm_Forms.Frm_Advertencia();
            Frm_Filtro fil = new Msm_Forms.Frm_Filtro();
            Frm_Msm_Bueno ok = new Frm_Msm_Bueno();


            try
            {
                jus.IdJusti = txt_idjusti.Text;
                jus.Id_Personal = txt_IdPersona.Text;
                jus.PrincipalMotivo = cbo_motivJusti.Text;
                jus.Detalle = txt_DetalleJusti.Text;
                jus.Fecha = Dtp_FechaJusti.Value;

                obj.RN_registrar_Justificacion(jus);
                
                if(BD_Justificacion.seguardo== true)
                {
                    Actualizar_SiguienteNumero(3);
                    fil.Show();
                    ok.Lbl_msm1.Text = "La solicitud de Justificacion fue registrada , Espere Aprobacion";
                    ok.ShowDialog();
                    fil.Hide();

                    this.Tag = "A";
                    this.Close();
                }
            }
            catch(Exception ex)
            {
                fil.Show();
                MessageBox.Show("Revisa el error : " + ex.Message, "Guardar Justificacion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                fil.Hide();            
            }

        }


        private void Actualizar_Justificacion()
        {
            RN_Justificacion obj = new RN_Justificacion();
            EN_Justificacion jus = new EN_Justificacion();


            Frm_Advertencia ver = new Msm_Forms.Frm_Advertencia();
            Frm_Filtro fil = new Msm_Forms.Frm_Filtro();
            Frm_Msm_Bueno ok = new Frm_Msm_Bueno();


            try
            {
                jus.IdJusti = txt_idjusti.Text;
                jus.PrincipalMotivo = cbo_motivJusti.Text;
                jus.Detalle = txt_DetalleJusti.Text;
                jus.Fecha = Dtp_FechaJusti.Value;

                obj.RN_Actualizar_Justificacion(jus);

                if (BD_Justificacion.seedito == true)
                {
                    
                    fil.Show();
                    ok.Lbl_msm1.Text = "La solicitud de Justificacion fue actualizada , Espere Aprobacion";
                    ok.ShowDialog();
                    fil.Hide();

                    this.Tag = "A";
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                fil.Show();
                MessageBox.Show("Revisa el error : " + ex.Message, "Guardar Justificacion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                fil.Hide();
            }

        }



        private double GenerarNextId(string numero)
        {
            double newnum = Convert.ToDouble(numero) + 1;
            return newnum;
        }
        private void Actualizar_SiguienteNumero(int idtipo)
        {
            string xnum = BD_Utilitario.BD_Leer_Solo_Numero(idtipo);
            string xnuevonum = Convert.ToString(GenerarNextId(xnum));
            int td = xnuevonum.Length;
            string NuevoCorrelativo = "";

            if (xnuevonum.Length < 5)
            {
                if (td == 1)
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

        private void btn_aceptar_Click(object sender, EventArgs e)
        {
            if(ValidarCampos()==true)
            {
                if(editar == true)
                {
                    Actualizar_Justificacion();
                }
                else
                {
                    Registrar_Justificacion();
                }

            }
        }

        bool editar = false;

        public void Buscar_Justificacion_paraEditar(string idjusti)
        {
            try
            {
                RN_Justificacion obj = new RN_Justificacion();
                DataTable Dato = new DataTable();

                Dato = obj.RN_BuscarJustificacion_porValor(idjusti);
                
                if(Dato.Rows.Count > 0)
                {
                    txt_idjusti.Text= Convert.ToString( Dato.Rows[0]["Id_justi"]);
                    txt_IdPersona.Text= Convert.ToString(Dato.Rows[0]["Id_Pernl"]);
                    txt_nompersona.Text= Convert.ToString(Dato.Rows[0]["Nombre_Completo"]);
                    cbo_motivJusti.Text=Convert.ToString(Dato.Rows[0]["PrincipalMotivo"]);
                    txt_DetalleJusti.Text= Convert.ToString(Dato.Rows[0]["Detalle_Justi"]);
                    Dtp_FechaJusti.Value = Convert.ToDateTime(Dato.Rows[0]["FechaJusti"]);
                    editar = true;
                    btn_aceptar.Enabled = true;
                        
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar los datos: "+ex.Message, "Advertencia de Seguridad ",MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
