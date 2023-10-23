using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prj_Capa_Datos;
using Prj_Capa_Entidad;
using System.Data;

namespace Prj_Capa_Negocio
{
    public class RN_Asistencia
    {
        public bool RN_verificar_si_Personal_YaMarco_su_Asistencia(string idper)
        {
            BD_Asistencia obj= new BD_Asistencia();
            return obj.BD_verificar_si_Personal_YaMarco_su_Asistencia(idper);
        }

        public bool RN_verificar_si_Personal_YaMarco_su_Falta(string idper)
        {
            BD_Asistencia obj = new BD_Asistencia();
            return obj.BD_verificar_si_Personal_YaMarco_su_Falta(idper);
        }

        public bool RN_verificar_si_Personal_YaMarco_su_Entrada(string idper)
        {
            BD_Asistencia obj = new BD_Asistencia();
            return obj.BD_verificar_si_Personal_YaMarco_su_Entrada(idper);
        }

        public void RN_Registrar_Falta(string idasis, string idper, string justi, string nomdia)
        {
            BD_Asistencia obj = new BD_Asistencia();
            obj.BD_Registrar_Falta(idasis,idper,justi,nomdia);
        }

        public void RN_Registrar_entrada(EN_Asistencia asi)
        {
            BD_Asistencia obj = new BD_Asistencia();
            obj.BD_Registrar_entrada(asi);
        }

        public DataTable RN_Buscar_Asistencia_deEntrada(String idperso)
        {
            BD_Asistencia obj = new BD_Asistencia();
            return obj.BD_Buscar_Asistencia_deEntrada(idperso);
        }

        public void RN_Registrar_salida(EN_Asistencia asi)
        {
            BD_Asistencia obj = new BD_Asistencia();
            obj.BD_Registrar_salida(asi);

        }

        public DataTable RN_Listar_Todas_Asistencias()
        {
            BD_Asistencia obj = new BD_Asistencia();
            return obj.BD_Listar_Todas_Asistencias();
        }

        public DataTable RN_Buscador_de_Asistencias(string xvalor)
        {
            BD_Asistencia obj = new BD_Asistencia();
            return obj.BD_Buscador_de_Asistencias(xvalor);
        }

        public void RN_Eliminar_asistencia(string idasi)
        {
            BD_Asistencia obj = new BD_Asistencia();
            obj.BD_Eliminar_asistencia(idasi);

        }

        public DataTable RN_Listar_Asistencia_deldia(DateTime xdia)
        {
            BD_Asistencia obj = new BD_Asistencia();
            return obj.BD_Listar_Asistencia_deldia(xdia);
        }

    }
}
