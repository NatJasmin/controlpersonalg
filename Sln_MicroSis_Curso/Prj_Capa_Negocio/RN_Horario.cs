using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Prj_Capa_Datos;
using Prj_Capa_Entidad;

namespace Prj_Capa_Negocio
{
  public  class RN_Horario
    {
        public void RN_actualizarHorario(EN_Horario P)
        {
            BD_Horario obj= new BD_Horario();
            obj.BD_actualizarHorario(P);
        }

        public DataTable RN_Leer_Horarios()
        {
            BD_Horario obj = new BD_Horario();
            return obj.BD_Leer_Horarios();
        }


    }
}
