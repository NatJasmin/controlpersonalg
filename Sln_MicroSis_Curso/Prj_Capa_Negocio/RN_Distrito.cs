﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Prj_Capa_Datos;

namespace Prj_Capa_Negocio
{
   public class RN_Distrito
    {
        public DataTable RN_Listar_Distritos()
        {
            BD_Distrito obj =new BD_Distrito();
            return obj.BD_Listar_Distritos();
        }

    }
}
