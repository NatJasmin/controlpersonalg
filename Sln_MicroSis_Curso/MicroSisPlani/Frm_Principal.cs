using MicroSisPlani.Personal;
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
using MicroSisPlani.Msm_Forms;
using System.IO;



namespace MicroSisPlani
{
    public partial class Frm_Principal : Form
    {
        public Frm_Principal()
        {
            InitializeComponent();
        }

        private void Frm_Principal_Load(object sender, EventArgs e)
        {
            Configurar_ListView_Per();
            ConfiguraListview_Justifi();
            P_Cargar_Todos_Personal();
            ConfiguraListView_Asis();
            J_Cargar_todas_Justificaciones();

            Cargar_Horarios();
            A_Cargar_todas_Asistencias();
                       
        }
        public void Cargar_Datos_Usuario()
        {
            try
            {
                Frm_Filtro fil = new Frm_Filtro();

                fil.Show();
                MessageBox.Show("Bienvenida(o): " + Cls_Libreria.Apellidos, "Bienvenida(o) al Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                fil.Hide();
                Lbl_NomUsu.Text = Cls_Libreria.Apellidos;
                lbl_rolNom.Text = Cls_Libreria.Rol;

                if (Cls_Libreria.Foto.Trim().Length == 0 | Cls_Libreria.Foto == null) return;
                if(File.Exists(Cls_Libreria.Foto)==true)
                {
                    pic_user.Load(Cls_Libreria.Foto);
                }
                else
                {
                    pic_user.Image = Properties.Resources.user;
                }
            }
            catch(Exception ex)
            {

            }
        }

        public void Cargar_Horarios()
        {
            RN_Horario obj = new RN_Horario();
            DataTable data = new DataTable();

            data= obj.RN_Leer_Horarios();

            if (data.Rows.Count == 0) return;
            lbl_idHorario.Text = Convert.ToString(data.Rows[0]["Id_Hor"]);
            dtp_horaIngre.Value = Convert.ToDateTime(data.Rows[0]["HoEntrda"]);
            dtp_horaSalida.Value= Convert.ToDateTime(data.Rows[0]["HoSalida"]);
            dtp_hora_tolercia.Value = Convert.ToDateTime(data.Rows[0]["MiTolerancia"]);
            Dtp_Hora_Limite.Value = Convert.ToDateTime(data.Rows[0]["HoraLimite"]);
        }

        public void Verificar_Robot_De_Faltas()
        {
            string tipoRobot = "";
            tipoRobot = RN_Utilitario.RN_Listar_TipoRobot(4);

            if(tipoRobot.Trim()=="Si")
            {
                
                rdb_ActivarRobot.Checked = true;
                pnl_falta.Visible=true;
                timerTempo.Start();
            }
            else if(tipoRobot.Trim()=="No")
            {
                rdb_Desact_Robot.Checked = true;
                timerTempo.Stop();
                timerFalta.Stop();
            
            }
        }
        
        private void pnl_titu_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button ==MouseButtons.Left )
            {
                Utilitarios u = new Utilitarios();
                u.Mover_formulario(this);
            }
        }

        private void btn_Salir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_mini_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void lsv_person_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        #region "personal"

        private void Configurar_ListView_Per()
        {
            var lis = lsv_person;

            lis.Items.Clear();
            lis.View = View.Details;
            lis.GridLines = false;
            lis.FullRowSelect = true;
            lis.Scrollable = true;
            lis.HideSelection = false;

            //columnas
            lis.Columns.Add("Id Persona", 0, HorizontalAlignment.Left);
            lis.Columns.Add("Ci", 95, HorizontalAlignment.Left);
            lis.Columns.Add("Nombres del Socio", 316, HorizontalAlignment.Left);
            lis.Columns.Add("Direccion", 0, HorizontalAlignment.Left);
            lis.Columns.Add("Correo", 0, HorizontalAlignment.Left);
            lis.Columns.Add("Sex", 0, HorizontalAlignment.Left);
            lis.Columns.Add("Fe Nacim. ", 110, HorizontalAlignment.Center);
            lis.Columns.Add("Nro Celular", 120, HorizontalAlignment.Left);
            lis.Columns.Add("Rol", 100, HorizontalAlignment.Left);
            lis.Columns.Add("Distrito", 0, HorizontalAlignment.Left);
            lis.Columns.Add("Estado", 100, HorizontalAlignment.Left);

        }

        private void Llenar_ListView(DataTable data)
        {
            lsv_person.Items.Clear();
            for (int i=0; i < data.Rows.Count;i++)
            {
                DataRow dr = data.Rows[i];
                ListViewItem list = new ListViewItem(dr["Id_Pernl"].ToString());//cabecera listview
                list.SubItems.Add(dr["CI"].ToString());
                list.SubItems.Add(dr["Nombre_Completo"].ToString());
                list.SubItems.Add(dr["Domicilio"].ToString());
                list.SubItems.Add(dr["Correo"].ToString());
                list.SubItems.Add(dr["Sexo"].ToString());
                list.SubItems.Add(dr["Fec_Naci"].ToString());
                list.SubItems.Add(dr["Celular"].ToString());
                list.SubItems.Add(dr["NomRol"].ToString());
                list.SubItems.Add(dr["Distrito"].ToString());
                list.SubItems.Add(dr["Estado_Per"].ToString());
                lsv_person.Items.Add(list); // sino ponemos esto el listView nunca se llenara

            }
            Lbl_total.Text = Convert.ToString(lsv_person.Items.Count);
        }




        private void P_Cargar_Todos_Personal()
        {
            RN_Personal obj = new RN_Personal();
            DataTable dato = new DataTable();

            dato = obj.RN_Lista_Todo_personal();
            if (dato.Rows.Count >0)
            {
                Llenar_ListView(dato);

            }
            else
            {
                lsv_person.Items.Clear();
            }
        }

        private void P_Cargar_Todos_PersonaldeBaja()
        {
            RN_Personal obj = new RN_Personal();
            DataTable dato = new DataTable();

            dato = obj.RN_Listar_PersonaldeBaja();
            if (dato.Rows.Count > 0)
            {
                Llenar_ListView(dato);

            }
            else
            {
                lsv_person.Items.Clear();
            }
        }



        private void P_Buscar_Personal_porValor(String valor)
        {
            RN_Personal obj = new RN_Personal();
            DataTable Dato = new DataTable();

            Dato = obj.RN_Buscar_personal_porValor(valor);
            if (Dato.Rows.Count > 0)
            {
                Llenar_ListView(Dato);

            }
            else
            {
                lsv_person.Items.Clear();
            }
        }





        #endregion

        private void bt_personal_Click(object sender, EventArgs e)
        {
            elTab1.SelectedTabPageIndex = 1;
            elTabPage2.Visible=true;
            P_Cargar_Todos_Personal();
        }

        private void txt_Buscar_OnValueChanged(object sender, EventArgs e)
        {
            if (txt_Buscar.Text.Length> 2)
            {
                P_Buscar_Personal_porValor(txt_Buscar.Text);
            }
        }

        private void txt_Buscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txt_Buscar.Text.Length> 2)
                {
                    P_Buscar_Personal_porValor(txt_Buscar.Text);
                }
                else
                {
                    P_Cargar_Todos_Personal();
                }
            }
        }

        private void Bt_NewPerso_Click(object sender, EventArgs e)
        {
            Frm_Filtro fil = new Frm_Filtro();
            Frm_Registro_Personal per = new Frm_Registro_Personal();

            fil.Show();
            per.ShowDialog();
            fil.Hide();

            if (per.Tag.ToString()=="A")
            {
                P_Cargar_Todos_Personal();
            }
                
        }

        private void Btn_EditPerso_Click(object sender, EventArgs e)
        {
            Frm_Filtro fil = new Frm_Filtro();
            Frm_Advertencia ver = new Frm_Advertencia();
            Frm_EditPersonal per = new Frm_EditPersonal();
            string idper = "";

            if(lsv_person.SelectedItems.Count ==0)
            {
                fil.Show();
                ver.Lbl_Msm1.Text = "Por favor,Selecciona el Personal que quieras Editar ";
                ver.ShowDialog();
                fil.Hide();
                return;
            }
            else
            {
                var lis = lsv_person.SelectedItems[0];
                idper = lis.SubItems[0].Text;

                fil.Show();
                per.Tag = idper;
                per.ShowDialog();
                fil.Hide();

                if(per.Tag.ToString()=="A")
                {
                    P_Cargar_Todos_Personal();
                }
            }
        }

        private void btn_VerTodoPerso_Click(object sender, EventArgs e)
        {
            P_Cargar_Todos_Personal();
        }

        private void bt_nuevoPersonal_Click(object sender, EventArgs e)
        {
            Frm_Filtro fil = new Frm_Filtro();
            Frm_Registro_Personal per = new Frm_Registro_Personal();

            fil.Show();
            per.ShowDialog();
            fil.Hide();

            if (per.Tag.ToString() == "A")
            {
                P_Cargar_Todos_Personal();
            }
        }

        private void bt_editarPersonal_Click(object sender, EventArgs e)
        {
            Frm_Filtro fil = new Frm_Filtro();
            Frm_Advertencia ver = new Frm_Advertencia();
            Frm_EditPersonal per = new Frm_EditPersonal();
            string idper = "";

            if (lsv_person.SelectedItems.Count == 0)
            {
                fil.Show();
                ver.Lbl_Msm1.Text = "Por favor,Selecciona el Personal que quieras Editar ";
                ver.ShowDialog();
                fil.Hide();
                return;
            }
            else
            {
                var lis = lsv_person.SelectedItems[0];
                idper = lis.SubItems[0].Text;

                fil.Show();
                per.Tag = idper;
                per.ShowDialog();
                fil.Hide();

                if (per.Tag.ToString() == "A")
                {
                    P_Cargar_Todos_Personal();
                }
            }
        }

        private void bt_eliminarPermanenteTool_Click(object sender, EventArgs e)
        {
            Frm_Filtro fil = new Frm_Filtro();
            Frm_Advertencia ver = new Frm_Advertencia();
            Frm_Sino sino = new Frm_Sino();
            Frm_Msm_Bueno ok = new Frm_Msm_Bueno(); 

            if (lsv_person.SelectedItems.Count == 0)
            {
                fil.Show();
                ver.Lbl_Msm1.Text = "Por favor,Selecciona el Personal que deseas eliminar ";
                ver.ShowDialog();
                fil.Hide();
                return;
            }
            else 
            {
                string idper = "";
                var lis = lsv_person.SelectedItems[0];
                idper = lis.SubItems[0].Text;

                fil.Show();
                sino.Lbl_msm1.Text = "Estás seguro de Eliminar Personal";
                sino.ShowDialog();
                fil.Hide();

                if(sino.Tag.ToString()=="Si")
                {
                    RN_Personal obj = new RN_Personal();
                    obj.RN_EliminarPersonal(idper);

                    if(BD_Personal.sequito==true)
                    {
                        fil.Show();
                        ok.Lbl_msm1.Text = "El Personal fue Eliminado de Forma Permanente";
                        ok.ShowDialog();
                        fil.Hide();

                    

                    }
                }





            }
        }

        private void bt_darDeBajaTool_Click(object sender, EventArgs e)
        {
            Frm_Filtro fil = new Frm_Filtro();
            Frm_Advertencia ver = new Frm_Advertencia();
            Frm_Sino sino = new Frm_Sino();
            Frm_Msm_Bueno ok = new Frm_Msm_Bueno();

            if (lsv_person.SelectedItems.Count == 0)
            {
                fil.Show();
                ver.Lbl_Msm1.Text = "Por favor,Selecciona el Personal que deseas dar de baja ";
                ver.ShowDialog();
                fil.Hide();
                return;
            }
            else
            {
                string idper = "";
                var lis = lsv_person.SelectedItems[0];
                idper = lis.SubItems[0].Text;

                fil.Show();
                sino.Lbl_msm1.Text = "Estás seguro de dar de baja al personal";
                sino.ShowDialog();
                fil.Hide();

                if (sino.Tag.ToString() == "Si")
                {
                    RN_Personal obj = new RN_Personal();
                    obj.RN_DardeBaja_Personal(idper);

                    if (BD_Personal.sequito == true)
                    {
                        fil.Show();
                        ok.Lbl_msm1.Text = "El Personal fue dado de baja exitosamente";
                        ok.ShowDialog();
                        fil.Hide();

                    }
                }

            }
    }

        private void verPersonalDeBajaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            P_Cargar_Todos_PersonaldeBaja();
        }

        private void bt_mostrarTodoElPersonal_Click(object sender, EventArgs e)
        {
            P_Cargar_Todos_Personal();
        }

        private void bt_copiarNroDNI_Click(object sender, EventArgs e)
        {
            Frm_Filtro fil = new Frm_Filtro();
            Frm_Advertencia ver = new Frm_Advertencia();

            if (lsv_person.SelectedItems.Count == 0)
            {
                fil.Show();
                ver.Lbl_Msm1.Text = "Selecciona el Item que deseas copiar ";
                ver.ShowDialog();
                fil.Hide();
                return;
            }
            else
            {
                var lsv = lsv_person.SelectedItems[0];
                string xdni = lsv.SubItems[1].Text;

                Clipboard.Clear();
                Clipboard.SetText(xdni.Trim());
            }

        }

        private void Btn_Cerrar_TabPers_Click(object sender, EventArgs e)
        {
            elTab1.SelectedTabPageIndex = 0;
            elTabPage2.Visible = false;
            txt_Buscar.Text = "";
            P_Cargar_Todos_Personal();

        }

        private void bt_solicitarJustificacion_Click(object sender, EventArgs e)
        {


            Frm_Filtro fil = new Frm_Filtro();
            Frm_Advertencia ver = new Frm_Advertencia();
            Frm_Reg_Justificacion jus = new Frm_Reg_Justificacion();

            if (lsv_person.SelectedItems.Count == 0)
            {
                fil.Show();
                ver.Lbl_Msm1.Text = "Seleccione el Personal para solicitar su Justificación";
                ver.ShowDialog();
                fil.Hide();
                return;
            }
            else
            {
                var lsv = lsv_person.SelectedItems[0];
                string xidpersonal= lsv.SubItems[0].Text;
                string xnombre = lsv.SubItems[2].Text;

                fil.Show();
                jus.txt_IdPersona.Text = xidpersonal;
                jus.txt_nompersona.Text=xnombre;
                jus.txt_idjusti.Text = RN_Utilitario.RN_NroDoc(3);
                jus.ShowDialog();
                fil.Hide();
            }


            
        }

        private void elTabPage5_Click(object sender, EventArgs e)
        {

        }

        private void ConfiguraListview_Justifi()
        {
            var lis = lsv_justifi;
            lis.Columns.Clear();
            lis.Items.Clear();
            lis.View = View.Details;
            lis.GridLines = false;
            lis.FullRowSelect = true;
            lis.Scrollable = true;
            lis.HideSelection = false;

            lis.Columns.Add("IdJusti",0, HorizontalAlignment.Left);
            lis.Columns.Add("IdPerso",0, HorizontalAlignment.Left);
            lis.Columns.Add("Nombres del Personal",316, HorizontalAlignment.Left);
            lis.Columns.Add("Motivo",110, HorizontalAlignment.Left);
            lis.Columns.Add("Fecha",120, HorizontalAlignment.Left);
            lis.Columns.Add("Fecha",120, HorizontalAlignment.Left);
            lis.Columns.Add("Estado",120, HorizontalAlignment.Left);
            lis.Columns.Add("Detalle Justifi",0, HorizontalAlignment.Left);
        }


        private void llenar_dataJustificacion (DataTable dato)
        {
            lsv_justifi.Items.Clear();
            for(int i =0; i < dato.Rows.Count; i++)
            {
                DataRow dr = dato.Rows[i];
                ListViewItem list = new ListViewItem(dr["Id_Justi"].ToString());
                list.SubItems.Add(dr["Id_Pernl"].ToString());
                list.SubItems.Add(dr["Nombre_Completo"].ToString());
                list.SubItems.Add(dr["PrincipalMotivo"].ToString());
                list.SubItems.Add(dr["FechaEmi"].ToString());
                list.SubItems.Add(dr["FechaJusti"].ToString());
                list.SubItems.Add(dr["EstadoJus"].ToString());
                list.SubItems.Add(dr["Detalle_Justi"].ToString());

                lsv_justifi.Items.Add(list);
               
            }
            lbl_totaljusti.Text = lsv_justifi.Items.Count.ToString();
        }


        private void J_Cargar_todas_Justificaciones()
        {
            RN_Justificacion obj = new RN_Justificacion();
            DataTable dt = new DataTable();

            dt = obj.RN_Cargar_todos_Justificacion();
            if (dt.Rows.Count >0)
            {
                llenar_dataJustificacion(dt);
            }
            else
            {
                lsv_justifi.Items.Clear();
            }

        }

        private void J_Buscar_Justificaciones(string xvalor)
        {
            RN_Justificacion obj = new RN_Justificacion();
            DataTable dt = new DataTable();

            dt = obj.RN_BuscarJustificacion_porValor(xvalor);
            if (dt.Rows.Count > 0)
            {
                llenar_dataJustificacion(dt);
            }
            else
            {
                lsv_justifi.Items.Clear();
            }

        }

        private void bt_exploJusti_Click(object sender, EventArgs e)
        {
            elTab1.SelectedTabPageIndex = 4;
            elTabPage5.Visible = true;
            txt_buscarjusti.Focus();
            J_Cargar_todas_Justificaciones();
        }

        private void bt_cerrarjusti_Click(object sender, EventArgs e)
        {
            elTab1.SelectedTabPageIndex = 0;
            elTabPage5.Visible = false;
            txt_buscarjusti.Text = "";
        }

        private void lsv_justifi_MouseClick(object sender, MouseEventArgs e)
        {
            var lsv = lsv_justifi.SelectedItems[0];
            string detallejus = lsv.SubItems[7].Text;

            lbl_Detalle.Text = detallejus.Trim();
        }

        private void bt_editJusti_Click(object sender, EventArgs e)
        {
            Frm_Filtro fil = new Frm_Filtro();
            Frm_Advertencia ver = new Frm_Advertencia();
            Frm_Reg_Justificacion jus = new Frm_Reg_Justificacion();

            if(lsv_justifi.SelectedIndices.Count ==0)
            {
                fil.Show();
                MessageBox.Show("Selecciona un Item por favor ", "Advertencia de seguridad ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                fil.Hide();            
            }
            else
            {
                var lsv = lsv_justifi.SelectedItems[0];
                string xidjusti = lsv.SubItems[0].Text;

                fil.Show();
                jus.Buscar_Justificacion_paraEditar(xidjusti);
                jus.ShowDialog();
                fil.Hide();

                if(jus.Tag.ToString()=="A")
                {
                    J_Cargar_todas_Justificaciones();
                }
                   

            }
        }

        private void bt_mostrarJusti_Click(object sender, EventArgs e)
        {
            J_Cargar_todas_Justificaciones();
        }

        private void bt_ElimiJusti_Click(object sender, EventArgs e)
        {
            Frm_Filtro fil = new Frm_Filtro();
            Frm_Advertencia ver = new Frm_Advertencia();
            Frm_Sino sino = new Frm_Sino();
            RN_Justificacion obj = new RN_Justificacion();
            if (lsv_justifi.SelectedIndices.Count == 0)
            {
                fil.Show();
                MessageBox.Show("Selecciona un Item por favor que quieras eliminar  ", "Advertencia de seguridad ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                fil.Hide();
            }
            else
            {
                var lsv = lsv_justifi.SelectedItems[0];
                string xidjusti = lsv.SubItems[0].Text;

                fil.Show();
                sino.Lbl_msm1.Text = "Estas seguro de eliminarlo ?";
                sino.ShowDialog();
                fil.Hide();

                if (sino.Tag.ToString() == "Si")
                {
                    obj.RN_Eliminar_Justificacion(xidjusti);
                    if(BD_Justificacion.seelimino==true)
                    {

                        J_Cargar_todas_Justificaciones();
                    }
                }


            }
        }

        private void bt_aprobarJustificacion_Click(object sender, EventArgs e)
        {
            Frm_Filtro fil = new Frm_Filtro();
            Frm_Advertencia ver = new Frm_Advertencia();
            Frm_Sino sino = new Frm_Sino();
            RN_Justificacion obj = new RN_Justificacion();

            String xestadojus = "";
            if (lsv_justifi.SelectedIndices.Count == 0)
            {
                fil.Show();
                MessageBox.Show("Selecciona un Item por favor  ", "Advertencia de seguridad ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                fil.Hide();
            }
            else
            {
                var lsv = lsv_justifi.SelectedItems[0];
                string xidjusti = lsv.SubItems[0].Text;
                xestadojus = lsv.SubItems[6].Text;

                if (xestadojus.Trim()=="Aprobado")
                {
                    fil.Show();
                    ver.Lbl_Msm1.Text = "La justificacion seleccionada ya está aprobada";
                    ver.ShowDialog();
                    fil.Hide();
                    return;

                }

                fil.Show();
                sino.Lbl_msm1.Text = "Estas seguro de aprobar la justificacion ?";
                sino.ShowDialog();
                fil.Hide();

                if (sino.Tag.ToString() == "Si")
                {
                    obj.RN_Aprobar_Desaprobar_Justificacion(xidjusti,"Aprobado");
                    if (BD_Justificacion.seelimino == true)
                    {

                        J_Cargar_todas_Justificaciones();
                    }
                }


            }
        }

        private void bt_desaprobarJustificacion_Click(object sender, EventArgs e)
        {
            Frm_Filtro fil = new Frm_Filtro();
            Frm_Advertencia ver = new Frm_Advertencia();
            Frm_Sino sino = new Frm_Sino();
            RN_Justificacion obj = new RN_Justificacion();

            String xestadojus = "";
            if (lsv_justifi.SelectedIndices.Count == 0)
            {
                fil.Show();
                MessageBox.Show("Selecciona un Item por favor  ", "Advertencia de seguridad ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                fil.Hide();
            }
            else
            {
                var lsv = lsv_justifi.SelectedItems[0];
                string xidjusti = lsv.SubItems[0].Text;
                xestadojus = lsv.SubItems[6].Text;

                if (xestadojus.Trim() == "Pendiente")
                {
                    fil.Show();
                    ver.Lbl_Msm1.Text = "La justificacion seleccionada aun no fue aprobada";
                    ver.ShowDialog();
                    fil.Hide();
                    return;

                }

                fil.Show();
                sino.Lbl_msm1.Text = "Estas seguro de restablecer la justificacion a pendiente ?";
                sino.ShowDialog();
                fil.Hide();

                if (sino.Tag.ToString() == "Si")
                {
                    obj.RN_Aprobar_Desaprobar_Justificacion(xidjusti, "Pendiente");
                    if (BD_Justificacion.seelimino == true)
                    {

                        J_Cargar_todas_Justificaciones();
                    }
                }


            }
        }

        private void txt_buscarjusti_OnValueChanged(object sender, EventArgs e)
        {
            if(txt_buscarjusti.Text.Trim().Length > 2)
            {
                J_Buscar_Justificaciones(txt_buscarjusti.Text);

            }
        }

        private void txt_buscarjusti_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode== Keys.Enter)
            {
                if(txt_buscarjusti.Text.Trim().Length > 2)
                {
                    J_Buscar_Justificaciones(txt_buscarjusti.Text);
                }
                else
                {
                    J_Cargar_todas_Justificaciones();
                }
            }
        }

        private void bt_Config_Click(object sender, EventArgs e)
        {
            elTab1.SelectedTabPageIndex = 3;
            elTabPage4.Visible = true;
        }

        private void btn_SaveHorario_Click(object sender, EventArgs e)
        {
            Frm_Filtro fis = new Frm_Filtro();
            Frm_Msm_Bueno ok = new Frm_Msm_Bueno();
            Frm_Advertencia adver = new Frm_Advertencia();

            try
            {
                RN_Horario hor = new RN_Horario();
                EN_Horario por = new EN_Horario();

               

                por.Idhora = lbl_idHorario.Text;
                por.HoEntrada = dtp_horaIngre.Value;
                por.HoTole = dtp_hora_tolercia.Value;
                por.HoLimite = Dtp_Hora_Limite.Value;
                por.HoSalida = dtp_horaSalida.Value;

                hor.RN_actualizarHorario(por);

                if(BD_Horario.seguardo== true)
                {
                    fis.Show();
                    ok.Lbl_msm1.Text = "El horario fue actualizado";
                    ok.ShowDialog();
                    fis.Hide();

                    elTab1.SelectedTabPageIndex = 0;
                    elTabPage4.Visible = false;
                }
            }
            catch(Exception ex)
            {
                fis.Show();
                adver.Lbl_Msm1.Text = "Hay problemas: "+ ex.Message;
                adver.ShowDialog();
                fis.Hide();

            }
        }

        private void btn_Savedrobot_Click(object sender, EventArgs e)
        {
            RN_Utilitario uti = new RN_Utilitario();

            if(rdb_ActivarRobot.Checked == true)
            {
                uti.RN_Actualizar_TipoRobot(4,"Si");
                if(BD_Utilitario.falta==true)
                {
                    Frm_Msm_Bueno ok = new Frm_Msm_Bueno();
                    ok.Lbl_msm1.Text = "El robot fue actualizado";
                    ok.ShowDialog();
                    elTab1.SelectedTabPageIndex = 0;
                    elTabPage4.Visible = false;
                }
            }
            else if (rdb_Desact_Robot.Checked==true)
            {
                uti.RN_Actualizar_TipoRobot(4, "No");
                if (BD_Utilitario.falta == true)
                {
                    Frm_Msm_Bueno ok = new Frm_Msm_Bueno();
                    ok.Lbl_msm1.Text = "El robot fue actualizado";
                    ok.ShowDialog();
                    elTab1.SelectedTabPageIndex = 0;
                    elTabPage4.Visible = false;
                }
            }
        }

        private void bt_registrarHuellaDigital_Click(object sender, EventArgs e)
        {
            Frm_Filtro fil = new Frm_Filtro();
            Frm_Advertencia ver = new Frm_Advertencia();
            Frm_Regis_Huella per = new Frm_Regis_Huella();
            string idper = "";

            if (lsv_person.SelectedItems.Count == 0)
            {
                fil.Show();
                ver.Lbl_Msm1.Text = "Por favor,Selecciona el Personal que quieras Agregar su huella";
                ver.ShowDialog();
                fil.Hide();
                return;
            }
            else
            {
                var lis = lsv_person.SelectedItems[0];
                idper = lis.SubItems[0].Text;

                fil.Show();
                per.Tag = idper;
                per.ShowDialog();
                fil.Hide();

                if (per.Tag.ToString() == "A")
                {
                    P_Cargar_Todos_Personal();
                }
            }
        }

        private void btn_Asis_With_Huella_Click(object sender, EventArgs e)
        {
            Frm_Filtro fil = new Frm_Filtro();
            Frm_Marcar_Asistencia asi = new Frm_Marcar_Asistencia();

            fil.Show();
            asi.ShowDialog();
            fil.Hide();
        }

        private void bt_Explo_Asis_Click(object sender, EventArgs e)
        {
            elTab1.SelectedTabPageIndex = 2;
            elTabPage3.Visible = true;
        }


        private int temp1 = 0;
        private int segun1 = 0;

        private void timerTempo_Tick(object sender, EventArgs e)
        {
            temp1 += 1;
            lbl_temp.Text = segun1.ToString().PadLeft(2, Convert.ToChar("0")) + ":";
            lbl_temp.Text += temp1.ToString().PadLeft(2, Convert.ToChar("0"));
            lbl_temp.Refresh();

            if(temp1==30)
            {
                timerTempo.Stop();
                temp1 = 0;
                Empezar_Registro_deFaltas();
            }


        }
    
    
        private void Empezar_Registro_deFaltas()
        {
            RN_Asistencia objasis = new RN_Asistencia();
            Frm_Filtro fil = new Frm_Filtro();
            Frm_Advertencia adver = new Frm_Advertencia();
            Frm_Msm_Bueno ok = new Frm_Msm_Bueno();
            DataTable dataper = new DataTable();
            RN_Personal objper = new RN_Personal();
            RN_Justificacion objus = new RN_Justificacion();

            int HoLimite = Dtp_Hora_Limite.Value.Hour;
            int MiLimite = Dtp_Hora_Limite.Value.Minute;

            int horaCaptu = DateTime.Now.Hour;
            int minutoCaptu=DateTime.Now.Minute;

            string Dniper = "";
            int cant = 0;
            int TotalItem = 0;
            string xidPersona = "";
            string IdAsistencia = "";
            string xjustifi = "";
            string xnomDia = "";

            if(horaCaptu < HoLimite )
            {
                lbl_temp.Text = "Robot Activo, Aun es temprano para marcar en :  ";
                return;
            }
            if(horaCaptu==HoLimite && minutoCaptu > HoLimite)
            {
                dataper = objper.RN_Lista_Todo_personal();
                if (dataper.Rows.Count <= 0) return;

                TotalItem = dataper.Rows.Count;

                foreach (DataRow reg in dataper.Rows)
                {
                    Dniper = Convert.ToString(reg["CI"]);
                    xidPersona = Convert.ToString(reg["Id_Pernl"]);

                    if(objasis.RN_verificar_si_Personal_YaMarco_su_Asistencia(xidPersona.Trim())==false)
                    {
                        if(objasis.RN_verificar_si_Personal_YaMarco_su_Falta(xidPersona)==false)
                        {
                            RN_Asistencia objA = new RN_Asistencia();
                            EN_Asistencia asi = new EN_Asistencia();
                            IdAsistencia = RN_Utilitario.RN_NroDoc(1);

                            //verificar si personla tiee justifi por falta
                            if(objus.RN_verificar_si_Personal_TieneJustificacion(xidPersona)==true)
                            {
                                xjustifi = "Falta Justificada";
                            }
                            else
                            {
                                xjustifi = "Falta no justificada";
                            }
                            xnomDia = DateTime.Now.ToString("dddd");

                            objasis.RN_Registrar_Falta(IdAsistencia,xidPersona,xjustifi,xnomDia);
                            
                            if(BD_Asistencia.entrada==true)
                            {
                                Actualizar_SiguienteNumero(1);
                                cant += 1;
                            }
                        
                        }
                    }
                }//fin del for
                if(cant >1)
                {
                    timerFalta.Stop();
                    fil.Show();
                    ok.Lbl_msm1.Text = "Un total de: "+cant.ToString()+"/"+ TotalItem.ToString()+ "Faltas se han registrado";
                    ok.ShowDialog();
                    fil.Hide();

                    pnl_falta.Visible = false;
                
                }
                else
                {
                    timerFalta.Stop();
                    fil.Show();
                    ok.Lbl_msm1.Text = "El dia de hoy no faltó nadie al trabajo";
                    ok.ShowDialog();
                    fil.Hide();
                    pnl_falta.Visible = false;
                }
            }
            else
            {
                lbl_temp.Text = "Robot Activo, Aun es temprano para Marcar en: ";
                timerTempo.Start();
            }
        }
    
        private double Generar_Nextid(string numero)
        {
            double newnum = Convert.ToDouble(numero) + 1;
            return newnum;
        }

        private void Actualizar_SiguienteNumero(int idtipo)
        {
            string xnum = BD_Utilitario.BD_Leer_Solo_Numero(idtipo);
            string newxnum = Convert.ToString(Generar_Nextid(xnum));
            int td = newxnum.Length;
            string nuevoNumero = "";
            if(newxnum.Length <5)
            { 
            if(td==1)
            {
                nuevoNumero = "0000" + newxnum;

            }
            else if(td==2)
            {
                nuevoNumero = "000" + newxnum;
            }
            else if(td==3)
            {
                nuevoNumero = "00" + newxnum;
            }
            else if (td==4)
            {
                nuevoNumero = "0" + newxnum;
            }
            }

            BD_Utilitario.BD_ActualizarNro(idtipo, nuevoNumero);
        }

        private void Lbl_stop_Click(object sender, EventArgs e)
        {
            timerTempo.Stop();
        }

        private void ConfiguraListView_Asis()
        {
            var lis = lsv_asis;

            lis.Columns.Clear();
            lis.Items.Clear();
            lis.View = View.Details;
            lis.GridLines = false;
            lis.FullRowSelect = true;
            lis.Scrollable = true;
            lis.HideSelection = false;

            //columnas:
            lis.Columns.Add("Id Asis", 0, HorizontalAlignment.Left);
            lis.Columns.Add("Ci", 80, HorizontalAlignment.Left);
            lis.Columns.Add("Nombres del personal", 316, HorizontalAlignment.Left);
            lis.Columns.Add("Fecha", 90, HorizontalAlignment.Left);
            lis.Columns.Add("Dia", 80, HorizontalAlignment.Left);
            lis.Columns.Add("Ho ingreso", 90, HorizontalAlignment.Left);
            lis.Columns.Add("Tardanza", 70, HorizontalAlignment.Left);
            lis.Columns.Add("Ho Salida", 90, HorizontalAlignment.Left);
            lis.Columns.Add("Justificacion", 0, HorizontalAlignment.Left);
            lis.Columns.Add("Estado", 100, HorizontalAlignment.Left);


        }

        private void Llenar_ListView_Asistencia(DataTable dato)
        {
            lsv_asis.Items.Clear();
            for(int i=0 ; i < dato.Rows.Count;i++)
            {
                DataRow dr = dato.Rows[i];
                ListViewItem list = new ListViewItem(dr["Id_asis"].ToString());
                list.SubItems.Add(dr["CI"].ToString());
                list.SubItems.Add(dr["Nombre_Completo"].ToString());
                list.SubItems.Add(dr["FechaAsis"].ToString());
                list.SubItems.Add(dr["Nombre_dia"].ToString());
                list.SubItems.Add(dr["HoIngreso"].ToString());
                list.SubItems.Add(dr["Tardanzas"].ToString());
                list.SubItems.Add(dr["HoSalida"].ToString());
                list.SubItems.Add(dr["Justificacion"].ToString());
                list.SubItems.Add(dr["EstadoAsis"].ToString());

                lsv_asis.Items.Add(list);

            }
            lbl_totalasis.Text = Convert.ToString(lsv_asis.Items.Count);
        }


        public void A_Cargar_todas_Asistencias()
        {
            RN_Asistencia obj = new RN_Asistencia();
            DataTable dato = new DataTable();

            dato = obj.RN_Listar_Todas_Asistencias();
            if(dato.Rows.Count > 0)
            {
                Llenar_ListView_Asistencia(dato);
            }
            else
            {
                lsv_asis.Items.Clear();
            }

        }

        public void A_Buscador_de_Asistencias(string xvalor)
        {
            RN_Asistencia obj = new RN_Asistencia();
            DataTable dato = new DataTable();

            dato = obj.RN_Buscador_de_Asistencias(xvalor);
            if (dato.Rows.Count > 0)
            {
                Llenar_ListView_Asistencia(dato);
            }
            else
            {
                lsv_asis.Items.Clear();
            }

        }
        private void txt_buscarAsis_OnValueChanged(object sender, EventArgs e)
        {
            if(txt_buscarAsis.Text.Trim().Length >2)
            {
                A_Buscador_de_Asistencias(txt_buscarAsis.Text);
            }
        }

        private void txt_buscarAsis_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                if(txt_buscarAsis.Text.Trim().Length > 2)
                {
                    A_Buscador_de_Asistencias(txt_buscarAsis.Text);
                }
                else
                {
                    A_Cargar_todas_Asistencias();
                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Frm_Filtro fil = new Frm_Filtro();
            Frm_Marcar_Asistencia asi = new Frm_Marcar_Asistencia();

            fil.Show();
            asi.ShowDialog();
            fil.Hide();
        }


        private void btn_Asis_Manual_Click(object sender, EventArgs e)
        {

            Frm_Filtro fil = new Frm_Filtro();
            Frm_Marcar_Asis_Manual asis = new Frm_Marcar_Asis_Manual();

            fil.Show();
            asis.ShowDialog();
            fil.Hide();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Frm_Advertencia adver = new Frm_Advertencia();
            Frm_Sino sino = new Frm_Sino();
            Frm_Msm_Bueno ok= new Frm_Msm_Bueno();
            Frm_Filtro fis = new Frm_Filtro();
            RN_Asistencia obj = new RN_Asistencia();

            if(lsv_asis.SelectedIndices.Count==0)
            {
                fis.Show();
                adver.Lbl_Msm1.Text = "Seleccione el item que desea Eliminar";
                adver.ShowDialog();
                fis.Hide();
                return;
            }
            else
            {
                var lsv = lsv_asis.SelectedItems[0];
                string xidasis = lsv.SubItems[0].Text;

                sino.Lbl_msm1.Text = "Estas seguro de eliminar la J?"+ "\n\r"+"Recuerde que este proceso es bajo su entera responsabilidad ";
                fis.Show();
                sino.ShowDialog();
                fis.Hide();

                if(Convert.ToString(sino.Tag)=="Si")
                {
                    obj.RN_Eliminar_asistencia(xidasis);
                    if(BD_Asistencia.salida==true)
                    {
                        fis.Show();
                        ok.Lbl_msm1.Text = "Asistencia eliminada";
                        ok.ShowDialog();
                        fis.Hide();
                        A_Cargar_todas_Asistencias();
                    }
                }

            }
        }

        private void bt_vertodasasistencia_Click(object sender, EventArgs e)
        {
            A_Cargar_todas_Asistencias();
        }

        private void bt_copiarnrodnitoo_Click(object sender, EventArgs e)
        {
            Frm_Filtro fil = new Frm_Filtro();
            Frm_Advertencia ver = new Frm_Advertencia();

            if (lsv_asis.SelectedItems.Count == 0)
            {
                fil.Show();
                ver.Lbl_Msm1.Text = "Selecciona el Item que deseas copiar ";
                ver.ShowDialog();
                fil.Hide();
                return;
            }
            else
            {
                var lsv = lsv_asis.SelectedItems[0];
                string xdni = lsv.SubItems[1].Text;

                Clipboard.Clear();
                Clipboard.SetText(xdni.Trim());
            }
        }



        private void A_Buscador_de_Asistencia_deldia(DateTime xdia)
        {
            RN_Asistencia obj = new RN_Asistencia();
            DataTable dato = new DataTable();

            dato = obj.RN_Listar_Asistencia_deldia(xdia);
            if (dato.Rows.Count > 0)
            {
                Llenar_ListView_Asistencia(dato);
            }
            else
            {
                lsv_asis.Items.Clear();
            }

        }

        private void bt_verAsistenciasDelDiaT_Click(object sender, EventArgs e)
        {
            A_Buscador_de_Asistencia_deldia(dtp_fechadeldia.Value);
        }

        private void btn_cerrarEx_Asis_Click(object sender, EventArgs e)
        {

        }
    }
}