using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestionCobro
{
    public partial class master : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["User"] == null || Session["dpto"]==null) {

                Response.Redirect("login.aspx");
            }
        }
        protected void lnkbtnCerrarSesion_Click(object sender, EventArgs e)
        {
            try
            {
                string osql = String.Format("Insert into Bitacora(Usuario,Accion) values('{0}','{1}')", Session["user"].ToString(), "Fin de Sesión");
                Session.RemoveAll();
                Session.Abandon();
                Session.Clear();
                ConexionSQL.DML(osql);
                Response.Redirect("login.aspx");
            }
            catch (Exception ex)
            {
                Response.Redirect("login.aspx");
            }
        }
        public  void Mensaje(String mensaje)
        {
                string mensajePersonalizado = "Este es un mensaje personalizado desde el code-behind";
                string script = $"imprimirEnConsola('{mensaje}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ImprimirConsola", script, true);
        }
    }
}