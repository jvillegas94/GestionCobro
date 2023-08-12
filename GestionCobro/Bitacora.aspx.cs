using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace GestionCobro
{
    public partial class Bitacora : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["User"] != null)
                {
                    string desde = DateTime.Now.ToString("yyyy-MM-dd");
                    string hasta = DateTime.Now.ToString("yyyy-MM-dd");
                    txtFechaInicio.Text = desde;
                    txtFechaFin.Text = hasta;
                    InfoBitacora();
                }
            else
            {
                Response.Redirect("login.aspx");
            }
            }
        }

        private void InfoBitacora()
        {
            string osql = String.Format("Select * from Vw_Bitacora where Fecha between '{0}' and '{1}'", Convert.ToDateTime(txtFechaInicio.Text).ToString("dd/MM/yyyy"), Convert.ToDateTime(txtFechaFin.Text).ToString("dd/MM/yyyy"));
            dtgBitacora.DataSource = ConexionSQL.consultaDataTable(osql, "Vw_Bitacora");
            dtgBitacora.DataBind();
        }
        private DataTable Detalle()
        {
            string osql = String.Format("Select * from Bitacora where Convert(varchar,Fecha,103) between '{0}' and '{1}'", Convert.ToDateTime(txtFechaInicio.Text).ToString("dd/MM/yyyy"), Convert.ToDateTime(txtFechaFin.Text).ToString("dd/MM/yyyy"));
            return  ConexionSQL.consultaDataTable(osql, "Vw_Bitacora");
        }

        protected void dtgBitacora_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dtgBitacora.PageIndex = e.NewPageIndex;
            InfoBitacora();
        }

        protected void dtgBitacora_RowCommand(object sender, GridViewCommandEventArgs e)
        {
           //No necesito ejecutar ningun comando por el momento
        }

        private void ExportToExcel(string nameReport, GridView wControl)
        {
            HttpResponse response = Response;
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            System.Web.UI.Page pageToRender = new System.Web.UI.Page();
            HtmlForm form = new HtmlForm();
            form.Controls.Add(wControl);
            pageToRender.Controls.Add(form);
            response.Clear();
            response.Buffer = true;
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", "attachment;filename=" + nameReport + ".xls");
            response.Charset = "UTF-8";
            response.ContentEncoding = Encoding.Default;
            pageToRender.RenderControl(htw);
            response.Write(sw.ToString());
            response.End();
        }

        protected void xptExcel_Click1(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridView data = new GridView();
                data.DataSource = Detalle();
                data.DataBind();
                if (data.Rows.Count > 0)
                {
                    ExportToExcel(String.Format("Bitacora del {0} al {1} ", Convert.ToDateTime(txtFechaInicio.Text).ToString("dd-MM-yyyy"), Convert.ToDateTime(txtFechaFin.Text).ToString("dd-MM-yyyy")), data);
                }
                else
                {
                    lblResult.Text = "No hay informacion que exportar para la fecha seleccionada";
                }
            }
            catch (Exception ex)
            {
                lblResult.Text = ex.Message;
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            InfoBitacora();
        }
    }
}