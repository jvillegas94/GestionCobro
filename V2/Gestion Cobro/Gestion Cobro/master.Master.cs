using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Cobro
{
    public partial class master : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                lblPage.Text = Page.Title;
                switch (Page.Title)
                {
                    case "Inicio":
                        Inicio.Attributes.Add("class", "active");
                        break;
                    case "Nuevo":
                        Nuevo.Attributes.Add("class", "active");
                        break;
                }
                if (Session["Usuario"] != null)
                {
                    lnkIniciar.Visible = false;
                    LinkButton1.Visible = true;
                    lblUsuario.Text = String.Format("Bienvenido(a) {0}", Session["Usuario"].ToString());
                    if (Session["Usuario"].ToString().Equals("manager") || Session["Usuario"].ToString().Equals("HSIBAJA"))
                    {
                        Bitacora.Visible = true;
                        Resumen.Visible = true;
                        Nuevo.Visible = true;
                        Liquidación.Visible = true;
                        Buscar.Visible = true;
                    }
                    else
                    {
                        Bitacora.Visible = false;
                        Resumen.Visible = true;
                        Nuevo.Visible = true;
                        Liquidación.Visible = true;
                        Buscar.Visible = true;
                    }
                }
                else {
                    LinkButton1.Visible = false;
                    lnkIniciar.Visible = true;
                    lblUsuario.Text = "";
                }

            }
        }

        protected void lnkIniciar_Click(object sender, EventArgs e)
        {

        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            mp1.Show();
            txtUsuario.Focus();
        }

        protected void btnIniciar_Click(object sender, EventArgs e)
        {
            LinkButton1.Visible = true;
            lnkIniciar.Visible = false;
            Session["Usuario"] = txtUsuario.Text;
            Response.Redirect("default.aspx");
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            LinkButton1.Visible = false;
            lnkIniciar.Visible = true;
            Session.Clear();
            Session.Abandon();
            Response.Redirect("default.aspx");
        }
    }
}