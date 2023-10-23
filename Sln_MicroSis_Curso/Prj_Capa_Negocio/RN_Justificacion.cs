using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prj_Capa_Datos;
using Prj_Capa_Entidad;
using System.Data;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Prj_Capa_Negocio
{
    public class RN_Justificacion
    {
        public void RN_registrar_Justificacion(EN_Justificacion jus)
        {
            BD_Justificacion obj = new BD_Justificacion();
            obj.Bd_registrar_Justificacion(jus);
        }

        public DataTable RN_Cargar_todos_Justificacion()
        {
            BD_Justificacion obj = new BD_Justificacion();
            return obj.BD_Cargar_todos_Justificacion();
        }

        public DataTable RN_BuscarJustificacion_porValor(string xdato)
        {
            BD_Justificacion obj = new BD_Justificacion();
            return obj.BD_BuscarJustificacion_porValor(xdato);
        }


        public void RN_Actualizar_Justificacion(EN_Justificacion jus)
        {
            BD_Justificacion obj = new BD_Justificacion();
            obj.Bd_Actualizar_Justificacion(jus);
        }
        public void RN_Eliminar_Justificacion(string idjusti)
        {
            BD_Justificacion obj = new BD_Justificacion();
            obj.Bd_Eliminar_Justificacion(idjusti);
        }

        public void RN_Aprobar_Desaprobar_Justificacion(string idjusti, string estadojus)
        {
            BD_Justificacion obj = new BD_Justificacion();
            obj.Bd_Aprobar_Desaprobar_Justificacion(idjusti, estadojus);

        }

        public bool RN_verificar_si_Personal_TieneJustificacion(string idper)
        {
            BD_Justificacion obj = new BD_Justificacion();
            return obj.BD_verificar_si_Personal_TieneJustificacion(idper);
        }







    }
}
