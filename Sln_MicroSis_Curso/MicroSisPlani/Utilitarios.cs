﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MicroSisPlani
{
    class Utilitarios
    {

        [DllImport ("user32.dll", EntryPoint="ReleaseCapture")]
        public static extern void ReleaseCapture();
        [DllImport ("User32.dll", EntryPoint= "SendMessage")]

        public static extern void  SendMessage(System.IntPtr hwnd, int wMsg, int wParam, int Lparam);

        public void Mover_formulario(Form f)
        {
            ReleaseCapture();
            SendMessage(f.Handle, 0x112, 0xf012, 0);
        }

      

    }
}
