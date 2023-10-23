
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Prj_Capa_Entidad;
using Prj_Capa_Negocio;
using DPFP;
using System.IO;
using Prj_Capa_Datos;
using MicroSisPlani.Msm_Forms;
using System.Xml.Serialization;

namespace MicroSisPlani
{
    public partial class Frm_Marcar_Asistencia : Form
    {
        public Frm_Marcar_Asistencia()
        {
            InitializeComponent();

        }

        private DPFP.Verification.Verification verificar;
        private DPFP.Verification.Verification.Result resultado;

        

        private void Frm_Marcar_Asistencia_Load(object sender, EventArgs e)
        {
            verificar = new DPFP.Verification.Verification();
            resultado = new DPFP.Verification.Verification.Result();
            Cargar_Horarios();
        }

        private void Cargar_Horarios()
        {
            RN_Horario obj = new RN_Horario();
            DataTable data = new DataTable();

            data = obj.RN_Leer_Horarios();

            if (data.Rows.Count == 0) return;
            //lbl_idHorario.Text = Convert.ToString(data.Rows[0]["Id_Hor"]);
            dtp_horaIngre.Value = Convert.ToDateTime(data.Rows[0]["HoEntrda"]);
            Lbl_HoraEntrada.Text=dtp_horaIngre.Value.Hour.ToString() + ":" + dtp_horaIngre.Value.Minute.ToString();
            dtp_horaSalida.Value = Convert.ToDateTime(data.Rows[0]["HoSalida"]);
            dtp_hora_tolercia.Value = Convert.ToDateTime(data.Rows[0]["MiTolerancia"]);
            Dtp_Hora_Limite.Value = Convert.ToDateTime(data.Rows[0]["HoraLimite"]);
        }

        private void Calcular_Minutos_tarde()
        {
            int horaIn = dtp_horaIngre.Value.Hour;
            int minuIn = dtp_horaIngre.Value.Minute;

            int MinuTole = dtp_hora_tolercia.Value.Minute;
            int HoraTole = dtp_hora_tolercia.Value.Hour;

            int HoraNow = DateTime.Now.Hour;
            int MinuNow = DateTime.Now.Minute;

            int TotalMinutos = MinuTole + minuIn;
            int TotalTardanza = 0;

            if(HoraNow == horaIn & MinuNow>TotalMinutos)
            {
                lbl_totaltarde.Text ="0";
            }
            else if (HoraNow == horaIn & MinuNow > TotalMinutos)
            {
                TotalTardanza = MinuNow - TotalMinutos;
                lbl_totaltarde.Text=TotalTardanza.ToString();
            }
            else if (HoraNow > horaIn)
            {
                int horaTarde = 0;

                horaTarde=HoraNow-horaIn;
                int horaMinuto = 0;
                
                horaMinuto = 60*horaTarde;
                

                int NuevominTarde = 0;
                NuevominTarde = horaMinuto-TotalMinutos;

                TotalTardanza = MinuNow + NuevominTarde;
                lbl_totaltarde.Text = TotalTardanza.ToString();
            }
        }

        private void btn_Salir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Calcular_Minutos_tarde();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lbl_hora.Text = DateTime.Now.ToString("hh:mm:ss");
        }

        private void xVerificationControl_Load(object sender, EventArgs e)
        {

        }

        private void xVerificationControl_OnComplete(object Control, FeatureSet FeatureSet, ref DPFP.Gui.EventHandlerStatus EventHandlerStatus)
        {
            RN_Asistencia objAsis = new RN_Asistencia();
            RN_Personal objper = new RN_Personal();
            DataTable dataper = new DataTable();
            DataTable dataasis = new DataTable();
            EN_Asistencia asis = new EN_Asistencia();

            Frm_Filtro fil = new Frm_Filtro();
            Frm_Advertencia ver= new Frm_Advertencia();
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
                dataper = objper.RN_Lista_Todo_personal();
                if(dataper.Rows.Count == 0) { return; }
                Nro_Rows_Data= dataper.Rows.Count;
                var datoPer = dataper.Rows[0];

                foreach (DataRow dt in dataper.Rows) 
                {
                    if (TerminaBucle == true){ return;}

                    huellapersona = (byte[])dt["FingerPrint"];
                    xidPersona = Convert.ToString(dt["Id_Pernl"]);

                    if(huellapersona.Length > 100)
                    {
                        DPFP.Template TemplateBD = new DPFP.Template();
                        TemplateBD.DeSerialize(huellapersona);
                        verificar.Verify(FeatureSet, TemplateBD, ref resultado);

                        if(resultado.Verified==true)
                        {
                            xrutaFoto = Convert.ToString(dt["Foto"]);
                            lbl_nombresocio.Text = Convert.ToString(dt["Nombre_Completo"]);
                            Lbl_Idperso.Text= Convert.ToString(dt["Id_Pernl"]);
                            lbl_Dni.Text = Convert.ToString(dt["CI"]);

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
                            if(objAsis.RN_verificar_si_Personal_YaMarco_su_Entrada(Lbl_Idperso.Text)==true)
                            {
                                //marca salida
                                Frm_Sinox sino = new Frm_Sinox();
                                TerminaBucle = true;
                                fil.Show();
                                sino.Lbl_msm1.Text = "El usuario ya tiene un registro de entrada, te gustaria marcar su salida?";
                                sino.ShowDialog();
                                fil.Hide();

                                if(sino.Tag.ToString()=="Si")
                                {
                                    dataasis = objAsis.RN_Buscar_Asistencia_deEntrada(Lbl_Idperso.Text);
                                    if (dataasis.Rows.Count == 0) return;
                                    lbl_IdAsis.Text = Convert.ToString( dataasis.Rows[0]["Id_asis"]);

                                    asis.IdAsistencia = lbl_IdAsis.Text;
                                    asis.Id_Personal= lbl_IdAsis.Text;
                                    asis.HoraSalida = lbl_hora.Text;
                                    asis.TotalHoras = 8;

                                    objAsis.RN_Registrar_salida(asis);

                                    if(BD_Asistencia.salida==true)
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

                                if(BD_Asistencia.entrada==true)
                                {
                                    Actualizar_SiguienteNumero(1);

                                    fil.Show();
                                    ok.Lbl_msm1.Text = "La asistencia se guardó correctamente";
                                    ok.ShowDialog();
                                    fil.Hide();

                                    TerminaBucle = true;

                                }
                            }
                        }
                        else
                        {
                            NroVeces += 1;
                        }


                    }
                    else
                    {
                        NroVeces += 1;
                    }
                }
                //fin del foreach
                if(NroVeces==Nro_Rows_Data)
                {
                    fil.Show();
                    ver.Lbl_Msm1.Text = "La huella no existe en la BD";
                    ver.ShowDialog();
                    fil.Hide();
                    return;

                }
            }
            catch(Exception ex)
            {
                string sms = ex.Message;
            }




        }

        public void TocarAudio()
        {
            string ruta;
            ruta = Application.StartupPath;
            System.Media.SoundPlayer son;
            son=new System.Media.SoundPlayer(ruta + @"\timbre1.wav");
            son.Play();
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


        private bool VerificarHoraLimite()
        {
            int xhourLimite=Dtp_Hora_Limite.Value.Hour;
            int xminuLimite=Dtp_Hora_Limite.Value.Minute;

            int horaCaptu = DateTime.Now.Hour;
            int minutoCaptu = DateTime.Now.Minute;

            if(xhourLimite==horaCaptu & xminuLimite < minutoCaptu)
            {
                
                return false;
            }
            else if (horaCaptu > xhourLimite)
            {
                return false;
            }
            else if (horaCaptu == xhourLimite & minutoCaptu < xminuLimite )
            {
                return true;
            }
            else if (horaCaptu < xhourLimite )
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        private int sec = 10;

        private void tmr_Conta_Tick(object sender, EventArgs e)
        {
            sec = -1;

            lbl_Cont.Text=sec.ToString();
            lbl_Cont.Refresh();

            if(sec==0)
            {
                LimpiarFormulario();
                sec = 10;
                tmr_Conta.Stop();
                lbl_Cont.Text = "10";
            }

        }



        private void LimpiarFormulario()
        {
            lbl_nombresocio.Text = "";
            lbl_totaltarde.Text = "0";
            lbl_TotalHotrabajda.Text = "0";
            lbl_Dni.Text = "";
            lbl_Cont.Text = "0";
            lbl_IdAsis.Text = "";
            Lbl_Idperso.Text = "";
            lbl_justifi.Text = "";
            lbl_msm.BackColor = Color.Transparent;
            lbl_msm.Text = "";
            picSocio.Image = null;
            xVerificationControl.Enabled = true;

        }



    }
}
