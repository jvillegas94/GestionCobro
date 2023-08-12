using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestionCobro
{
    public partial class Devoluciones : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CargarRutas();
                txtFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");
                CargarDatos(txtFecha.Text);
            }
        }
        protected void CargarDatos(String fecha) {
            grvDevolucionesEscaneadas.DataSource = HANAConnection.DQL(String.Format("Select *,\"Code\" \"NoSeq\" from \"SB1LD_RTR_PRO\".\"@DEVOLUCIONES\" where \"U_FechaRegistro\"='{0}'",fecha));
            grvDevolucionesEscaneadas.DataBind();
        }
        protected void CargarRutas()
        {
            ddlRutas.DataSource = HANAConnection.DQL("Select distinct \"SlpName\" from \"SB1LD_EPG_PRO\".VW_Devoluciones");
            ddlRutas.DataTextField = "SlpName";
            ddlRutas.DataValueField = "SlpName";
            ddlRutas.DataBind();
            if (Session["RutaSeleccionada"] != null) {
                ddlRutas.Text = Session["RutaSeleccionada"].ToString();
            }
        }

        protected void txtFecha_TextChanged(object sender, EventArgs e)
        {
            CargarDatos(txtFecha.Text);
        }

        protected void txtEscanear_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = HANAConnection.DQL(String.Format("Select * from \"SB1LD_EPG_PRO\".VW_Devoluciones where \"Codigo\"='{0}'", txtEscanear.Text));
            if (dt.Rows.Count > 0)
            {
                String SlpName = dt.Rows[0]["SlpName"].ToString();
                Session["RutaSeleccionada"] = SlpName;
                if (SlpName.Equals(ddlRutas.SelectedValue))
                {
                    String NumAtCard = dt.Rows[0]["NumAtCard"].ToString();
                    int Total = Convert.ToInt32(HANAConnection.Excalar(String.Format("Select count(*) from \"SB1LD_RTR_PRO\".\"@DEVOLUCIONES\" where \"U_NumAtCard\"='{0}' and \"U_FechaRegistro\"='{1}'", NumAtCard,txtFecha.Text)));
                    if (Total == 0)
                    {
                        String Empresa = dt.Rows[0]["Empresa"].ToString();
                        String CardCode = dt.Rows[0]["CardCode"].ToString();
                        String CardName = dt.Rows[0]["CardName"].ToString();
                        Double Cantidad = Convert.ToDouble(dt.Rows[0]["Cantidad"].ToString());
                        int NoSeq = Convert.ToInt32(HANAConnection.Excalar("SELECT ifnull(max(TO_NUMBER(\"Code\")),0)+1 \"NoSeq\" FROM \"SB1LD_RTR_PRO\".\"@DEVOLUCIONES\""));
                        int i = HANAConnection.DML(String.Format("insert into \"SB1LD_RTR_PRO\".\"@DEVOLUCIONES\" values({0},{0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}')", NoSeq, NumAtCard, CardCode, CardName, Cantidad, SlpName, Empresa,txtFecha.Text));
                        if (i > 0)
                        {
                            CargarDatos(txtFecha.Text);
                            txtEscanear.Text = "";
                            txtEscanear.Focus();
                        }
                    }
                    else
                    {
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Documento asignado anteriormente')", true);
                        MostrarNotificacionToast("Documento asignado anteriormente", "error");
                        txtEscanear.Text = "";
                        txtEscanear.Focus();
                    }
                }
                else {
                    String NumAtCard = dt.Rows[0]["NumAtCard"].ToString();
                    int Total = Convert.ToInt32(HANAConnection.Excalar(String.Format("Select count(*) from \"SB1LD_RTR_PRO\".\"@DEVOLUCIONES\" where \"U_NumAtCard\"='{0}' and \"U_FechaRegistro\"='{1}'", NumAtCard, txtFecha.Text)));
                    if (Total == 0)
                    {
                        String Empresa = dt.Rows[0]["Empresa"].ToString();
                        String CardCode = dt.Rows[0]["CardCode"].ToString();
                        String CardName = dt.Rows[0]["CardName"].ToString();
                        Double Cantidad = Convert.ToDouble(dt.Rows[0]["Cantidad"].ToString());
                        int NoSeq = Convert.ToInt32(HANAConnection.Excalar("SELECT ifnull(max(TO_NUMBER(\"Code\")),0)+1 \"NoSeq\" FROM \"SB1LD_RTR_PRO\".\"@DEVOLUCIONES\""));
                        int i = HANAConnection.DML(String.Format("insert into \"SB1LD_RTR_PRO\".\"@DEVOLUCIONES\" values({0},{0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}')", NoSeq, NumAtCard, CardCode, CardName, Cantidad, SlpName, Empresa, txtFecha.Text));
                        if (i > 0)
                        {
                            CargarDatos(txtFecha.Text);
                            txtEscanear.Text = "";
                            txtEscanear.Focus();
                        }
                    }
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('La Ruta no coincide')", true);
                        MostrarNotificacionToast($"Ruta cambiada a {SlpName}", "success");
                    ddlRutas.Text = SlpName;
                    txtEscanear.Text = "";
                    txtEscanear.Focus();
                }
            }
        }
        private void MostrarNotificacionToast(string mensaje, string tipo)
        {
            string script = $@"
    Toastify({{ 
        text: '{mensaje}',
        duration: 3000,
        position: 'toast.POSITION.TOP_CENTER', // Cambia la posición a 'top'
        gravity: 'top', // Cambia la gravedad a 'center'
        backgroundColor: {(tipo == "success" ? "'#4caf50'" : "'#f44336'")}
    }}).showToast();";

            ScriptManager.RegisterStartupScript(this, GetType(), "MostrarNotificacion", script, true);


            // Registra el script en el cliente
        }
        protected void imgbtn_Click(object sender, ImageClickEventArgs e)
        {
        }

        protected void grvDevolucionesEscaneadas_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void grvDevolucionesEscaneadas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Eliminar")) {
                int i = HANAConnection.DML(String.Format("delete from \"SB1LD_RTR_PRO\".\"@DEVOLUCIONES\" where \"U_NumAtCard\"='{0}' and \"U_FechaRegistro\"='{1}'",e.CommandArgument,txtFecha.Text));
                if (i > 0) {

                    CargarDatos(txtFecha.Text);
                    txtEscanear.Text = "";
                    txtEscanear.Focus();
                }
            
            }
           // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", String.Format("alert('{0}')", e.CommandArgument), true);
        }
    }
    }