using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace GestionCobro
{
    public class Configuracion
    {
        public string Fecha { get; set; }
        public Rutas Rutas { get; set; }
        public static Configuracion get(String Ruta)
        {
            StreamReader r = new StreamReader(Ruta);
            string json = r.ReadToEnd();
            r.Close();
            return JsonConvert.DeserializeObject<Configuracion>(json);
        }
    }

    public class Rutas
    {
        public string _01 { get; set; }
        public string _04 { get; set; }
        public string _14 { get; set; }
        public string _18 { get; set; }
        public string _22 { get; set; }
        public string _26 { get; set; }
    }
}