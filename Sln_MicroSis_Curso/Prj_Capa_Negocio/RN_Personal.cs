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
     public class RN_Personal
    {
        public void RN_Registar_Personal(EN_Persona per)
        {
            BD_Personal obj =new BD_Personal();
            obj.BD_Registar_Personal(per);
        }


        public void RN_actualizar_Personal(EN_Persona per)
        {
            BD_Personal obj = new BD_Personal();
            obj.BD_actualizar_Personal(per);
        }
        public DataTable RN_Lista_Todo_personal()
        {
            BD_Personal obj = new BD_Personal();
            return obj.BD_Lista_Todo_personal();
        }

        public DataTable RN_Buscar_personal_porValor(string valor)
        {
            BD_Personal obj = new BD_Personal();
            return obj.BD_Buscar_personal_porValor(valor);
        }

        public void RN_EliminarPersonal(string idper)
        {
            BD_Personal obj = new BD_Personal();
            obj.Bd_EliminarPersonal(idper);
        }

       public void RN_DardeBaja_Personal(string idper)
        {
            BD_Personal obj = new BD_Personal();
            obj.Bd_DardeBaja_Personal(idper);
        }

        public DataTable RN_Listar_PersonaldeBaja()
        {
            BD_Personal obj = new BD_Personal();
            return obj.BD_Listar_PersonaldeBaja();
        }

        public void RN_Registrar_Huella_Personal(string idper, object huella)
        {
            BD_Personal obj = new BD_Personal();
            obj.BD_Registrar_Huella_Personal(idper, huella);
        }




    }
}
