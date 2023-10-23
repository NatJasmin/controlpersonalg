using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Prj_Capa_Datos;





namespace Prj_Capa_Negocio
{
  public class RN_Utilitario
    {
        public static string RN_NroDoc(int idtipo)
        {
            return BD_Utilitario.BD_NroDoc(idtipo);
        }

        public static void BD_ActulizarNro(int idtipo, string numero)
        {
            BD_Utilitario.BD_ActualizarNro(idtipo, numero);
        }

        public static string BD_Leer_Solo_Numero(int idtipo)
        {
            return BD_Utilitario.BD_Leer_Solo_Numero(idtipo);
        }

        public static string RN_Listar_TipoRobot(int idtipo)
        {
            return BD_Utilitario.BD_Listar_TipoRobot(idtipo);
        }

        public void RN_Actualizar_TipoRobot(int idtipo, string serie)
        {
            BD_Utilitario obj= new BD_Utilitario();
            obj.Bd_Actualizar_TipoRobot(idtipo, serie);
        }



    }
}
