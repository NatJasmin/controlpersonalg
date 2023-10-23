using DPFP.Gui.Verification;
using DPFP;
using MicroSisPlani.Msm_Forms;
using Prj_Capa_Datos;
using Prj_Capa_Entidad;
using Prj_Capa_Negocio;
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
using Prj_Capa_Entidad;
using Prj_Capa_Negocio;
using System.IO;



namespace MicroSisPlani
{
    public partial class Frm_Marcar_Asis_Manual : Form
    {
        public Frm_Marcar_Asis_Manual()
        {
            InitializeComponent();
        }

        private void Frm_Marcar_Asis_Manual_Load(object sender, EventArgs e)
        {

            Cargar_Horarios();
            txt_dni_Buscar.Focus();
        }

        private void btn_buscar_Click(object sender, EventArgs e)
        {
                RN_Asistencia objAsis = new RN_Asistencia();
                RN_Personal objper = new RN_Personal();
                DataTable dataper = new DataTable();
                DataTable dataasis = new DataTable();
                EN_Asistencia asis = new EN_Asistencia();

                Frm_Filtro fil = new Frm_Filtro();
                Frm_Advertencia ver = new Frm_Advertencia();
                Frm_Msm_Bueno ok = new Frm_Msm_Bueno();

                byte[] huellapersona;
                string xidPersona = "";
                string xrutaFoto = "";
                string xnombrepersona = "";
                string xdnipersona = "";

                bool TerminaBucle = false;
                int Nro_Rows_Data = 0;
                int NroVeces = 0;



                try
                {
                    dataper = objper.RN_Buscar_personal_porValor(txt_dni_Buscar.Text.Trim());
                    if (dataper.Rows.Count == 0) { return; }
                    Nro_Rows_Data = dataper.Rows.Count;
                    var datoPer = dataper.Rows[0];

                    xidPersona = Convert.ToString(datoPer["Id_Pernl"]);

                    xrutaFoto = Convert.ToString(datoPer["Foto"]);
                     lbl_nombresocio.Text = Convert.ToString(datoPer["Nombre_Completo"]);
                     Lbl_Idperso.Text = Convert.ToString(datoPer["Id_Pernl"]);
                     lbl_Dni.Text = Convert.ToString(datoPer["CI"]);

                                if (File.Exists(xrutaFoto) == true) { picSocio.Load(xrutaFoto.Trim()); } else { picSocio.Load(Application.StartupPath + @"\user.png"); }
                                if (objAsis.RN_verificar_si_Personal_YaMarco_su_Asistencia(Lbl_Idperso.Text) == true)
                                {
                                    fil.Show();
                                    ver.Lbl_Msm1.Text = "El personal ya marcó su asistencia , No insitir";
                                    ver.ShowDialog();
                                    fil.Hide();
                                    TerminaBucle = true;
                                    return;
                                }
                                if (objAsis.RN_verificar_si_Personal_YaMarco_su_Entrada(Lbl_Idperso.Text) == true)
                                {
                                    //marca salida
                                    Frm_Sinox sino = new Frm_Sinox();
                                    TerminaBucle = true;
                                    fil.Show();
                                    sino.Lbl_msm1.Text = "El usuario ya tiene un registro de entrada, te gustaria marcar su salida?";
                                    sino.ShowDialog();
                                    fil.Hide();

                                    if (sino.Tag.ToString() == "Si")
                                    {
                                        dataasis = objAsis.RN_Buscar_Asistencia_deEntrada(Lbl_Idperso.Text);
                                        if (dataasis.Rows.Count == 0) return;
                                        lbl_IdAsis.Text = Convert.ToString(dataasis.Rows[0]["Id_asis"]);

                                        asis.IdAsistencia = lbl_IdAsis.Text;
                                        asis.Id_Personal = lbl_IdAsis.Text;
                                        asis.HoraSalida = lbl_hora.Text;
                                        asis.TotalHoras = 8;

                                        objAsis.RN_Registrar_salida(asis);

                                        if (BD_Asistencia.salida == true)
                                        {
                                            lbl_msm.BackColor = Color.YellowGreen;
                                            lbl_msm.ForeColor = Color.White;
                                            lbl_msm.Text = "La salida del Personal fue registrado exitosamente";
                                            TocarAudio();
                                            xVerificationControl.Enabled = false;
                                            pnl_Msm.Visible = true;
                                            lbl_Cont.Text = "10";
                                            tmr_Conta.Enabled = true;

                                            TerminaBucle = true;
                                        }




                                    }
                                }
                                else
                                {
                                    if (VerificarHoraLimite() == false) { fil.Show(); ver.Lbl_Msm1.Text = "Tu Hora de entrada ya caducó"; ver.ShowDialog(); fil.Hide(); return; }
                                    //calcula tardnaza
                                    Calcular_Minutos_tarde();
                                    lbl_IdAsis.Text = RN_Utilitario.RN_NroDoc(1);
                                    String NomDia = DateTime.Now.ToString("dddd");
                                    //registro de asistencia
                                    asis.IdAsistencia = lbl_IdAsis.Text;
                                    asis.Id_Personal = Lbl_Idperso.Text;
                                    asis.Nombre_Dia = NomDia;
                                    asis.HoraIngre = lbl_hora.Text;
                                    asis.Justificacion = "Pendiente";
                                    asis.Tardanza = Convert.ToInt32(lbl_totaltarde.Text);

                                    objAsis.RN_Registrar_entrada(asis);

                                    if (BD_Asistencia.entrada == true)
                                    {
                                        Actualizar_SiguienteNumero(1);

                                        fil.Show();
                                        ok.Lbl_msm1.Text = "La asistencia se guardó correctamente";
                                        ok.ShowDialog();
                                        fil.Hide();

                                        TerminaBucle = true;

                                    }
                                }
                            
                            else
                            {
                                NroVeces += 1;
                            }


                        
                        else
                        {
                            NroVeces += 1;
                        }
                    
                    //fin del foreach
                    if (NroVeces == Nro_Rows_Data)
                    {
                        fil.Show();
                        ver.Lbl_Msm1.Text = "La huella no existe en la BD";
                        ver.ShowDialog();
                        fil.Hide();
                        return;

                    }
                }
                catch (Exception ex)
                {
                    string sms = ex.Message;
                }
        }


        public void TocarAudio()
        {
            string ruta;
            ruta = Application.StartupPath;
            System.Media.SoundPlayer son;
            son = new System.Media.SoundPlayer(ruta + @"\timbre1.wav");
            son.Play();
        }

        private void Cargar_Horarios()
        {
            RN_Horario obj = new RN_Horario();
            DataTable data = new DataTable();

            data = obj.RN_Leer_Horarios();

            if (data.Rows.Count == 0) return;
            //lbl_idHorario.Text = Convert.ToString(data.Rows[0]["Id_Hor"]);
            dtp_horaIngre.Value = Convert.ToDateTime(data.Rows[0]["HoEntrda"]);
            Lbl_HoraEntrada.Text = dtp_horaIngre.Value.Hour.ToString() + ":" + dtp_horaIngre.Value.Minute.ToString();
            dtp_horaSalida.Value = Convert.ToDateTime(data.Rows[0]["HoSalida"]);
            dtp_hora_tolercia.Value = Convert.ToDateTime(data.Rows[0]["MiTolerancia"]);
            Dtp_Hora_Limite.Value = Convert.ToDateTime(data.Rows[0]["HoraLimite"]);
        }

        private void btn_Salir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pnl_titulo_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.Button==MouseButtons.Left)
            {
                Utilitarios ui= new Utilitarios();
                ui.Mover_formulario(this);
            }
        }

        private void lbl_msm_Click(object sender, EventArgs e)
        {

        }
    }
}