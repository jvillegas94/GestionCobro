using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestionCobro
{
    public partial class Resumen : System.Web.UI.Page
    {
        string Desde="", Hasta = "", Ruta = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["User"] != null)
                {
                    ResumenFacturacion();
                }
                else {
                    Response.Redirect("login.aspx");
                }
            }
        }
        protected void ResumenFacturacion()
        {
            try
            {
                DataTable dt=HANAConnection.DQL(string.Format("Select * from {0}SB1LD_EPG_PRO{0}.{0}Vw_ResumenLiquidacion{0} where {0}DocsPendientes{0}>0", (char)34));
                dtgRuta.DataSource = dt;
                dtgRuta.DataBind();
            } catch (Exception ex) {
                //No es necesario
            }

        }

        protected void dtgRuta_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string comando = e.CommandName;
                if (!comando.Equals("Page"))
                {
                        int index = Convert.ToInt32(e.CommandArgument);
                    if (comando.Equals("Agregar"))
                    {
                        string desde = Server.HtmlDecode(dtgRuta.Rows[index].Cells[1].Text);
                        string hasta = Server.HtmlDecode(dtgRuta.Rows[index].Cells[2].Text);
                        string ruta = Server.HtmlDecode(dtgRuta.Rows[index].Cells[0].Text);
                        string osql = String.Format("Insert into Bitacora(Usuario,Accion,FechaInicio,FechaFin,Ruta) values('{0}','{1}','{2}','{3}','{4}')", Session["user"].ToString(), "Asignar más documentos", desde, hasta, ruta);
                        ConexionSQL.DML(osql);
                        Response.Redirect("asignar.aspx?Desde=" + desde + "&Hasta=" + hasta + "&Ruta=" + ruta);
                    }
                    if (comando.Equals("Cerrar"))
                    {

                        Desde = Server.HtmlDecode(dtgRuta.Rows[index].Cells[1].Text);
                        Hasta = Server.HtmlDecode(dtgRuta.Rows[index].Cells[2].Text);
                        Ruta= Server.HtmlDecode(dtgRuta.Rows[index].Cells[0].Text);
                        int x = PorReasignar(Ruta);
                        if ( x> 0)
                        {
                            lblDocumentosporreasignar.Text = x.ToString();
                            lblRutaCerrar.Text = Ruta;
                            ModalPopupExtender1.Show();
                        }
                        else
                        {
                            lblDesde.Text = Desde;
                            lblHasta.Text = Hasta;
                            lblRuta.Text = Ruta;
                            mpePopUp.Show();
                        }
                    }
                    if (comando.Equals("Info"))
                    {
                        Desde = Server.HtmlDecode(dtgRuta.Rows[index].Cells[1].Text);
                        Hasta = Server.HtmlDecode(dtgRuta.Rows[index].Cells[2].Text);
                        Ruta = Server.HtmlDecode(dtgRuta.Rows[index].Cells[0].Text);
                        Session["Desde"] = Desde;
                        Session["Hasta"] = Hasta;
                        Session["Ruta"] = Ruta;
                        Response.Redirect("Detalle.aspx");
                    }
                    }
            }
            catch (Exception ex)
            {
                //
            }
        }

        protected void dtgRuta_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string Estado = e.Row.Cells[10].Text;
            string Pendientes = e.Row.Cells[4].Text;
            if (Estado.Equals("Cerrada"))
            {

                e.Row.Cells[11].Enabled = false;
                e.Row.Cells[10].Enabled = false;
                if (!Pendientes.Equals("0"))
                {
                    e.Row.BackColor = ColorTranslator.FromHtml("#F5A9A9");
                }

            }
            if (Estado.Equals("Abierta"))
            {
                e.Row.Cells[11].Enabled = true;
                e.Row.Cells[10].Enabled = true;
                e.Row.BackColor = ColorTranslator.FromHtml("#A9F5D0");
            }
        }

        protected void btnOpenPopUp_Click(object sender, EventArgs e)
        {
            lblDesde.Text = Desde;
            lblHasta.Text = Hasta;
            lblRuta.Text = Ruta;
            mpePopUp.Show();
        }

        private void Cancelar() {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Update a ");
            stringBuilder.Append(string.Format("set {0}U_Estado{0}='Cerrada'", (char)34));
            stringBuilder.Append(string.Format("from {0}SB1LD_EPG_PRO{0}.{0}@LOG_COBRO{0} a ", (char)34));
            stringBuilder.Append(string.Format("where {0}U_FechaInicio{0}='{1}' and {0}U_FechaFin{0}='{2}' and {0}U_Ruta{0}='{3}'", (char)34, Convert.ToDateTime(lblDesde.Text).ToString("yyyy-MM-dd"), Convert.ToDateTime(lblHasta.Text).ToString("yyyy-MM-dd"), lblRuta.Text));
            string osql = String.Format("Insert into Bitacora(Usuario,Accion,FechaInicio,FechaFin,Ruta) values('{0}','{1}','{2}','{3}','{4}')", Session["user"].ToString(), "Cerrar Asignación", lblDesde.Text, lblHasta.Text, lblRuta.Text);
            int i = HANAConnection.DML(stringBuilder.ToString());
            if (i > 0)
            {
            ConexionSQL.DML(osql);
                lblResult.Text = "Asignacion Cerrada Correctamente";
                lblResult.ForeColor = Color.Green;
                ResumenFacturacion();
            }
            else
            {
                lblResult.Text = "Ha ocurrido un error";
                lblResult.ForeColor = Color.Red;
            }

        }

        protected int Reasignar(string ruta)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Update a ");
            stringBuilder.Append(string.Format(" set a.{0}U_FechaInicio{0}={0}Desde{0},", (char)34));
            stringBuilder.Append(string.Format(" a.{0}U_FechaFin{0}={0}Hasta{0},", (char)34));
            stringBuilder.Append(string.Format(" a.{0}U_TipoDocumento{0}='',", (char)34));
            stringBuilder.Append(string.Format(" a.{0}U_TipoPago{0}='',", (char)34));
            stringBuilder.Append(string.Format(" a.{0}U_ReciboNum{0}=0,", (char)34));
            stringBuilder.Append(string.Format(" a.{0}U_MontoRecibo{0}=0,", (char)34));
            stringBuilder.Append(string.Format("  a.{0}U_FechaLiquidacion{0}='',", (char)34));
            stringBuilder.Append(string.Format(" a.{0}U_HoraLiquidacion{0}='',", (char)34));
            stringBuilder.Append(string.Format("  a.{0}U_Liquidador{0}='',", (char)34));
            stringBuilder.Append(string.Format("  a.{0}U_FechaRecibo{0}='',", (char)34));
            stringBuilder.Append(string.Format("  a.{0}U_Estado{0}='Abierta',", (char)34));
            stringBuilder.Append(string.Format("  a.{0}U_MontoDoc{0}=c.{0}Total{0},", (char)34));
            stringBuilder.Append(string.Format("  a.{0}U_Original{0}='',", (char)34));
            stringBuilder.Append(string.Format("  a.{0}U_Tramite{0}='',", (char)34));
            stringBuilder.Append(string.Format("  a.{0}U_VaDenuevo{0}='0'", (char)34));
            stringBuilder.Append(string.Format(" from {0}SB1LD_EPG_PRO{0}.{0}@LOG_COBRO{0} a", (char)34));
            stringBuilder.Append(string.Format(" inner join {0}SB1LD_EPG_PRO{0}.{0}VwQQINV{0} c on a.{0}U_Empresa{0}=c.{0}Empresa{0} ", (char)34));
            stringBuilder.Append(string.Format(" and a.{0}U_DocNum{0}=c.{0}DocNum{0}", (char)34));
            stringBuilder.Append(" inner join ( Select ");
            stringBuilder.Append(string.Format(" {0}U_Ruta{0},", (char)34));
            stringBuilder.Append(string.Format(" TO_CHAR(max({0}U_FechaInicio{0}),'yyyy/MM/dd'){0}Desde{0},", (char)34));
            stringBuilder.Append(string.Format(" TO_CHAR(max({0}U_FechaFin{0}),'yyyy/MM/dd'){0}Hasta{0} ", (char)34));
            stringBuilder.Append(string.Format(" from {0}SB1LD_EPG_PRO{0}.{0}@LOG_COBRO{0} a ", (char)34));
            stringBuilder.Append(string.Format(" where {0}U_Estado{0}='Abierta' ", (char)34));
            stringBuilder.Append(string.Format(" group by {0}U_Ruta{0} )b on a.{0}U_Ruta{0}=b.{0}U_Ruta{0} ", (char)34));
            stringBuilder.Append(string.Format(" where {0}U_VaDenuevo{0}='1' ", (char)34));
            stringBuilder.Append(string.Format(" and a.{0}U_Ruta{0}='{1}'", (char)34,ruta));
            int i = HANAConnection.DML(stringBuilder.ToString());
            return i;
            }

        protected int PorReasignar(string ruta)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(string.Format("Select TO_CHAR(Count(*)){0}Total{0} from {0}SB1LD_EPG_PRO{0}.{0}@LOG_COBRO{0} a", (char)34));
            stringBuilder.Append(string.Format(" inner join {0}SB1LD_EPG_PRO{0}.{0}VwQQINV{0} c on a.{0}U_Empresa{0}=c.{0}Empresa{0} ", (char)34));
            stringBuilder.Append(string.Format(" and a.{0}U_DocNum{0}=c.{0}DocNum{0}", (char)34));
            stringBuilder.Append(" inner join ( Select ");
            stringBuilder.Append(string.Format(" {0}U_Ruta{0},", (char)34));
            stringBuilder.Append(string.Format(" TO_CHAR(max({0}U_FechaInicio{0}),'yyyy/MM/dd'){0}Desde{0},", (char)34));
            stringBuilder.Append(string.Format(" TO_CHAR(max({0}U_FechaFin{0}),'yyyy/MM/dd'){0}Hasta{0} ", (char)34));
            stringBuilder.Append(string.Format(" from {0}SB1LD_EPG_PRO{0}.{0}@LOG_COBRO{0} a ", (char)34));
            stringBuilder.Append(string.Format(" where {0}U_Estado{0}='Abierta' ", (char)34));
            stringBuilder.Append(string.Format(" group by {0}U_Ruta{0} )b on a.{0}U_Ruta{0}=b.{0}U_Ruta{0} ", (char)34));
            stringBuilder.Append(string.Format(" where {0}U_VaDenuevo{0}='1' ", (char)34));
            stringBuilder.Append(string.Format(" and a.{0}U_Ruta{0}='{1}'", (char)34, ruta));
            string EstadoDoc = (string)HANAConnection.Excalar(stringBuilder.ToString());
            return Convert.ToInt32(EstadoDoc);
        }
        protected void dtgRuta_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dtgRuta.PageIndex = e.NewPageIndex;
            ResumenFacturacion();
        }

        protected void btnCancelReasignar_Click(object sender, EventArgs e)
        {
            ModalPopupExtender1.Hide();

        }

        protected void btnOKReasignar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Reasignar(lblRutaCerrar.Text) > 0)
                {
                    lblResult.Text = "Documentos reasignados correctamente";
                    lblResult.ForeColor = Color.Green;
                    ResumenFacturacion();

                }
                else
                {
                    lblResult.Text = "Ha ocurrido un error";
                    lblResult.ForeColor = Color.Red;
                }
                ModalPopupExtender1.Hide();
            }
            catch (Exception ex)
            {
                lblResult.Text = ex.Message;
                lblResult.ForeColor = Color.Red;
            }
        }

        protected void LinkButton1_Command(object sender, CommandEventArgs e)
        {

        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            Cancelar();
            mpePopUp.Hide();

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            mpePopUp.Hide();

        }
    }
}