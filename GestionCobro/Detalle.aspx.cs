using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace GestionCobro
{
    public partial class Detalle : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["User"] != null) {
                    if (Session["Ruta"] != null)
                    {
                        lblRuta.Text = Session["Ruta"].ToString();
                        lblDesde.Text = Session["Desde"].ToString();
                        lblHasta.Text = Session["Hasta"].ToString();
                        grvAsignados.DataSource=DetalleRuta();
                        grvTramitados.DataSource = DetalleTramitados();
                        grvOriginales.DataSource = DetalleOriginales();
                        grvRecibos.DataSource = DetalleRecibos();
                        grvPendientes.DataSource = DetallePendientes();
                        grvAsignados.DataBind();
                        grvTramitados.DataBind();
                        grvOriginales.DataBind();
                        grvRecibos.DataBind();
                        grvPendientes.DataBind();
                        if (grvAsignados.Rows.Count == 0)
                        {
                            Asignados.Visible = false;
                        }
                        if (grvOriginales.Rows.Count == 0)
                        {
                            Originales.Visible = false;
                        }
                        if (grvPendientes.Rows.Count == 0)
                        {
                            Pendientes.Visible = false;
                        }
                        if (grvRecibos.Rows.Count == 0)
                        {
                            Recibos.Visible = false;
                        }
                        if (grvTramitados.Rows.Count == 0)
                        {
                            Tramitados.Visible = false;
                        }
                    }
                    else
                    {
                        Response.Redirect("Resumen.aspx");
                    }
                }
                else
                {
                    Response.Redirect("login.aspx");
                }
            }
        }

        private System.Data.DataTable DetalleRuta()
        {
            StringBuilder osql = new StringBuilder();
            osql.Append(string.Format("Select {0}Empresa{0},{0}Tipo{0}||'-'||TO_CHAR({0}U_DocNum{0}){0}DocNum{0},{0}CardName{0},case {0}Tipo{0} when 'FC' then {0}Total{0} else {0}Total{0}*-1 end {0}Total{0} ", (char)34));
            osql.Append(string.Format("from {0}SB1LD_EPG_PRO{0}.{0}@LOG_COBRO{0} a ", (char)34));
            osql.Append(string.Format("inner join {0}SB1LD_EPG_PRO{0}.{0}VwQQINV{0} b on a.{0}U_DocNum{0}=b.{0}DocNum{0} and a.{0}U_Empresa{0}=b.{0}Empresa{0} ", (char)34));
            osql.Append(string.Format("where {0}U_Ruta{0}='{1}' and TO_CHAR({0}U_FechaInicio{0},'dd/MM/yyyy')='{2}' and TO_CHAR({0}U_FechaFin{0},'dd/MM/yyyy')='{3}' ", (char)34, lblRuta.Text, lblDesde.Text, lblHasta.Text));
            return HANAConnection.DQL(osql.ToString());
        }

        private System.Data.DataTable DetalleTramitados()
        {
            StringBuilder osql = new StringBuilder();
            osql.Append(("Select  "));
            osql.Append(string.Format("{0}Empresa{0}, ", (char)34));
            osql.Append(string.Format("{0}Tipo{0}||'-'||TO_CHAR({0}U_DocNum{0}){0}DocNum{0}, ", (char)34));
            osql.Append(string.Format("{0}CardName{0}, ", (char)34));
            osql.Append(string.Format("case {0}Tipo{0} when 'FC' then {0}Total{0} else {0}Total{0}*-1 end {0}Total{0}  ", (char)34));
            osql.Append(string.Format("from {0}SB1LD_EPG_PRO{0}.{0}@LOG_COBRO{0} a  ", (char)34));
            osql.Append(string.Format("inner join {0}SB1LD_EPG_PRO{0}.{0}VwQQINV{0} b on a.{0}U_DocNum{0}=b.{0}DocNum{0} and a.{0}U_Empresa{0}=b.{0}Empresa{0}  ", (char)34));
            osql.Append(string.Format("where {0}U_Ruta{0}='{1}' and TO_CHAR({0}U_FechaInicio{0},'dd/MM/yyyy')='{2}'  ", (char)34, lblRuta.Text, lblDesde.Text));
            osql.Append(string.Format("and TO_CHAR({0}U_FechaFin{0},'dd/MM/yyyy')='{1}' and {0}U_Tramite{0}='Si' ", (char)34, lblHasta.Text));
            return HANAConnection.DQL(osql.ToString());
        }
        private System.Data.DataTable DetalleOriginales()
        {
            StringBuilder osql = new StringBuilder();
            osql.Append(("Select  "));
            osql.Append(string.Format("{0}Empresa{0}, ", (char)34));
            osql.Append(string.Format("{0}Tipo{0}||'-'||TO_CHAR({0}U_DocNum{0}){0}DocNum{0}, ", (char)34));
            osql.Append(string.Format("{0}CardName{0}, ", (char)34));
            osql.Append(string.Format("case {0}Tipo{0} when 'FC' then {0}Total{0} else {0}Total{0}*-1 end {0}Total{0}  ", (char)34));
            osql.Append(string.Format("from {0}SB1LD_EPG_PRO{0}.{0}@LOG_COBRO{0} a  ", (char)34));
            osql.Append(string.Format("inner join {0}SB1LD_EPG_PRO{0}.{0}VwQQINV{0} b on a.{0}U_DocNum{0}=b.{0}DocNum{0} and a.{0}U_Empresa{0}=b.{0}Empresa{0}  ", (char)34));
            osql.Append(string.Format("where {0}U_Ruta{0}='{1}' and TO_CHAR({0}U_FechaInicio{0},'dd/MM/yyyy')='{2}'  ", (char)34, lblRuta.Text, lblDesde.Text));
            osql.Append(string.Format("and TO_CHAR({0}U_FechaFin{0},'dd/MM/yyyy')='{1}' and {0}U_Original{0}='Si' ", (char)34, lblHasta.Text));
            return HANAConnection.DQL(osql.ToString());
        }
        private System.Data.DataTable DetalleRecibos()
        {
            StringBuilder osql = new StringBuilder();
            osql.Append(("Select  distinct "));
            osql.Append(string.Format("TO_CHAR({0}U_ReciboNum{0}){0}DocNum{0}, ", (char)34));
            osql.Append(string.Format("{0}CardName{0}, ", (char)34));
            osql.Append(string.Format("{0}U_MontoRecibo{0} {0}Total{0}  ", (char)34));
            osql.Append(string.Format("from {0}SB1LD_EPG_PRO{0}.{0}@LOG_COBRO{0} a  ", (char)34));
            osql.Append(string.Format("inner join {0}SB1LD_EPG_PRO{0}.{0}VwQQINV{0} b on a.{0}U_DocNum{0}=b.{0}DocNum{0} and a.{0}U_Empresa{0}=b.{0}Empresa{0}  ", (char)34));
            osql.Append(string.Format("where {0}U_Ruta{0}='{1}' and TO_CHAR({0}U_FechaInicio{0},'dd/MM/yyyy')='{2}'  ", (char)34, lblRuta.Text, lblDesde.Text));
            osql.Append(string.Format("and TO_CHAR({0}U_FechaFin{0},'dd/MM/yyyy')='{1}' and length({0}U_TipoDocumento{0})='6' ", (char)34, lblHasta.Text));
            return HANAConnection.DQL(osql.ToString());
        }

        private System.Data.DataTable DetallePendientes()
        {
            StringBuilder osql = new StringBuilder();
            osql.Append(("Select  "));
            osql.Append(string.Format("{0}Empresa{0}, ", (char)34));
            osql.Append(string.Format("{0}Tipo{0}||'-'||TO_CHAR({0}U_DocNum{0}){0}DocNum{0}, ", (char)34));
            osql.Append(string.Format("{0}CardName{0}, ", (char)34));
            osql.Append(string.Format("case {0}Tipo{0} when 'FC' then {0}Total{0} else {0}Total{0}*-1 end {0}Total{0}  ", (char)34));
            osql.Append(string.Format("from {0}SB1LD_EPG_PRO{0}.{0}@LOG_COBRO{0} a  ", (char)34));
            osql.Append(string.Format("inner join {0}SB1LD_EPG_PRO{0}.{0}VwQQINV{0} b on a.{0}U_DocNum{0}=b.{0}DocNum{0} and a.{0}U_Empresa{0}=b.{0}Empresa{0}  ", (char)34));
            osql.Append(string.Format("where {0}U_Ruta{0}='{1}' and TO_CHAR({0}U_FechaInicio{0},'dd/MM/yyyy')='{2}'  ", (char)34, lblRuta.Text, lblDesde.Text));
            osql.Append(string.Format("and TO_CHAR({0}U_FechaFin{0},'dd/MM/yyyy')='{1}' and (length({0}U_TipoDocumento{0})=0  and ifnull({0}U_Tramite{0},'') in ('','0') and ifnull({0}U_Original{0},'') in ('','0') ) and {0}Total{0}>0 ", (char)34, lblHasta.Text));
            return HANAConnection.DQL(osql.ToString());
        }

        private void ExportToExcel(System.Data.DataTable dt,String nombre)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition",String.Format( "attachment;filename={0}.xls",nombre));
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages
                GridView gridView = new GridView();
                gridView.AllowPaging = false;
                gridView.DataSource = dt;
                gridView.DataBind();

                gridView.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in gridView.HeaderRow.Cells)
                {
                    cell.BackColor = gridView.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in gridView.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = Color.ForestGreen;
                        }
                        else
                        {
                            cell.BackColor = Color.FloralWhite;
                        }
                        cell.CssClass = "textmode";
                    }
                }

                gridView.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }
        protected void xptExcel_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            string nombre = String.Format("{0} {1} {2} al {3}",btn.CommandName,lblRuta.Text,lblDesde.Text,lblHasta.Text);
            switch (btn.CommandName)
            {
                case "Asignados":
            ExportToExcel(DetalleRuta(),nombre);
                    break;
                case "Tramitados":
            ExportToExcel(DetalleTramitados(), nombre);
                    break;
                case "Originales":
            ExportToExcel(DetalleOriginales(), nombre);
                    break;
                case "Recibos":
                    ExportToExcel(DetalleRecibos(), nombre);
                    break;
                case "Pendientes":
                    ExportToExcel(DetallePendientes(), nombre);
                    break;

            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }
    }
}