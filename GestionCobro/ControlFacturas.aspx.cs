using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestionCobro
{
    public partial class ControlFacturas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["dpto"] != null)
                {
                    txtDesde.Text = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
                    txtHasta.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    CargarFacturacion();
                    CargarDatos();
                    this.Label1.Text = Session["dpto"].ToString();
                    if (Session["dpto"].ToString().Equals("VENTAS"))
                    {

                        this.pnlFacturacion.Enabled = true;
                        this.pnlCuentasporCobrar.Enabled = false;
                    }
                    else if (Session["dpto"].ToString().Equals("TI")) {
                   
                        this.pnlFacturacion.Enabled = true;
                        this.pnlCuentasporCobrar.Enabled = true;
                    }
                    else
                    {

                        this.pnlFacturacion.Enabled = false;
                        this.pnlCuentasporCobrar.Enabled = true;

                    }
                }
            }
        }
        private void ExportToExcel(System.Data.DataTable dt, String nombre)
        {
            try
            {
                if (dt.Rows.Count > 0)
                {
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", ($"attachment;filename={nombre}_{DateTime.Now.ToString("yyyyMMdd hh:mm:ss")}.xls"));
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
                else
                {

                    MostrarNotificacionToast($"No hay registros que exportar", "error");
                    this.txtEscanear.Text = "";
                    this.txtEscanear.Focus();
                }
            }
            catch (Exception ex)
            {

                MostrarNotificacionToast($"{ex.Message}", "error");
                this.txtEscanear.Text = "";
                this.txtEscanear.Focus();
            }
        }
        protected void ValidarCheck(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            switch (chk.ToolTip)
            {
                case "cxc":
                    txtEscanearCxC.Focus();
                    break;
                case "fact":
                    txtEscanear.Focus();
                    break;
            }
        }

        protected void FacturasPendiente_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            FacturasPendiente.PageIndex = e.NewPageIndex;
            CargarDatos();
        }
        protected void CargarDatos()
        {
            string desdeString = txtDesde.Text;
            string hastaString = txtHasta.Text;
            DateTime desde, hasta;
            if (DateTime.TryParseExact(desdeString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out desde) &&
    DateTime.TryParseExact(hastaString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out hasta))
            {
                // Las conversiones fueron exitosas, ahora puedes comparar las fechas.
                if (desde <= hasta)
                {
                    DataTable dt = HANAConnection.DQL("SELECT \"BD\", \"NoFact\", TO_VARCHAR(\"Fecha\", 'dd/MM/yyyy') AS \"Fecha\", \"CardCode\", \"CardName\", \"DocTotal\", (\"BD\" || \"NoFact\") AS \"Filtro\" " +
                                   "FROM \"SB1LD_EPG_PRO\".\"VW_Facturas\" " +
                                   $"WHERE \"Fecha\" between '{desdeString}' and '{hastaString}'");
                    DataTable dt1 = ConexionSQL.consultaDataTable("Select Empresa,NoDocumento from [GestionCobroBD].[dbo].ControlFacturas", "Facturas");

                    // Crear un HashSet para almacenar las combinaciones Empresa y NoDocumento de dt1
                    HashSet<Tuple<string, int>> dt1Set = new HashSet<Tuple<string, int>>();

                    // Llenar el HashSet con las combinaciones Empresa y NoDocumento de dt1
                    foreach (DataRow row in dt1.Rows)
                    {
                        string empresa = row["Empresa"].ToString();
                        int noDocumento = Convert.ToInt32(row["NoDocumento"]);
                        dt1Set.Add(new Tuple<string, int>(empresa, noDocumento));
                    }

                    // Iterar en reverso por el DataTable y eliminar las filas que coinciden
                    for (int i = dt.Rows.Count - 1; i >= 0; i--)
                    {
                        string empresa = dt.Rows[i]["BD"].ToString();
                        int noDocumento = Convert.ToInt32(dt.Rows[i]["NoFact"]);

                        if (dt1Set.Contains(new Tuple<string, int>(empresa, noDocumento)))
                        {
                            dt.Rows.RemoveAt(i);
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        FacturasPendiente.DataSource = dt;
                        FacturasPendiente.DataBind();
                    }
                    else
                    {

                        FacturasPendiente.DataSource = null;
                        FacturasPendiente.DataBind();
                    }
                    DateTime fechaDespacho = DateTime.Now.AddDays(-7);
                    string fechaDespachoFormateada = fechaDespacho.ToString("dd/MM/yyyy");
                    lblFechaDespacho.Text = $"{desdeString} al {hastaString}";
                }
                else
                {
                    // Desde es mayor o igual que Hasta, el rango no es válido.
                    // Puedes mostrar un mensaje de error o tomar la acción correspondiente.
                    // Por ejemplo:
                    // Response.Write("La fecha Desde debe ser menor que la fecha Hasta.");

                    MostrarNotificacionToast($"El rango de fechas no es válido", "error");
                }
            }
            else
            {
                MostrarNotificacionToast($"Los valores deben ser fechas válidas", "error");
            }
        }
        protected void CargarFacturacion()
        {
            DataTable dt = ConexionSQL.consultaDataTable("Select * from [GestionCobroBD].[dbo].ControlFacturas where CuentasPorCobrar is null", "ControlFacturas");

            DataTable dt1 = ConexionSQL.consultaDataTable("Select * from [GestionCobroBD].[dbo].ControlFacturas where CuentasPorCobrar is not null", "ControlFacturas");

            if (dt.Rows.Count > 0)
            {
                grvFacturacion.DataSource = dt;
                grvFacturacion.DataBind();
                this.lblTotalFacturacion.Text = $"{dt.Rows.Count}";
            }
            else
            {
                grvFacturacion.DataSource = null;
                grvFacturacion.DataBind();
                this.lblTotalFacturacion.Text = $"0";
            }

            if (dt1.Rows.Count > 0)
            {
                this.grvCuentasPorCobrar.DataSource = dt1;
                grvCuentasPorCobrar.DataBind();
                this.lblTotalCuentasporCobrar.Text = $"{dt1.Rows.Count}";
            }
            else
            {
                grvCuentasPorCobrar.DataSource = null;
                grvCuentasPorCobrar.DataBind();
                this.lblTotalCuentasporCobrar.Text = $"0";
            }


        }

        protected void txtEscanear_TextChanged(object sender, EventArgs e)
        {
            CargarFactura(txtEscanear.Text);
        }
        protected void CargarFactura(String Filtro)
        {
            try
            {
                String NumeroFact = "", Empresa = "", Tipo = "";
                DataTable dt = new DataTable();
                if (Filtro.Length > 0)
                {
                    NumeroFact = Filtro.Substring(4, Filtro.Length - 4);
                    String Em = Filtro.Substring(0, 2);
                    switch (Em)
                    {
                        case "00":
                            Empresa = "Empagro";
                            break;
                        case "01":
                            Empresa = "Rosa";
                            break;
                        case "02":
                            Empresa = "Don Beto";
                            break;
                    }
                    String TI = Filtro.Substring(2, 2);
                    switch (TI)
                    {
                        case "00":
                            Tipo = "FC";
                            break;
                        case "01":
                            Tipo = "NC";
                            break;
                    }


                    String depto = Session["dpto"].ToString();
                    if (depto == "VENTAS" || depto == "TI")
                    {
                        if (!chkNoDelDiaFact.Checked)
                        {
                            dt = HANAConnection.DQL("SELECT \"BD\", \"NoFact\", TO_VARCHAR(\"Fecha\", 'dd/MM/yyyy') AS \"Fecha\", \"CardCode\", \"CardName\", \"DocTotal\", \"Condiciones\",\"Estado\", (\"BD\" || \"NoFact\") AS \"Filtro\" " + "FROM \"SB1LD_EPG_PRO\".\"VW_Facturas\" " +
                                                   $"WHERE DAYS_BETWEEN(\"Fecha\", CURRENT_DATE) <= 7 and \"BD\"='{Empresa}' and \"NoFact\"='{NumeroFact}';");
                        }
                        else
                        {
                            dt = HANAConnection.DQL("SELECT \"BD\", \"NoFact\", TO_VARCHAR(\"Fecha\", 'dd/MM/yyyy') AS \"Fecha\", \"CardCode\", \"CardName\", \"DocTotal\", \"Condiciones\",\"Estado\", (\"BD\" || \"NoFact\") AS \"Filtro\" " + "FROM \"SB1LD_EPG_PRO\".\"VW_Facturas\" " +
                                                   $"WHERE \"BD\"='{Empresa}' and \"NoFact\"='{NumeroFact}';");
                        }
                        if (dt.Rows.Count == 1)
                        {
                            DataRow row = dt.Rows[0];
                            if (row["Condiciones"].ToString().Equals("Contado")) {

                                MostrarNotificacionToast($"Este documento es de contado", "error");
                                this.txtEscanear.Text = "";
                                this.txtEscanear.Focus();
                            }
                            else if (!row["Estado"].Equals("Normal")) {
                                MostrarNotificacionToast($"Este documento se encuentra {row["Estado"]}", "error");
                                this.txtEscanear.Text = "";
                                this.txtEscanear.Focus();
                            }
                            else
                            {
                                string empresa = row["BD"].ToString();
                                int noDocumento = Convert.ToInt32(row["NoFact"]);
                                DateTime fecha = DateTime.ParseExact(row["Fecha"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                string cardCode = row["CardCode"].ToString();
                                string cardName = row["CardName"].ToString();
                                decimal total = Convert.ToDecimal(row["DocTotal"]);
                                DateTime despacho = fecha; // Define el valor de despacho adecuado
                                DateTime facturacion = DateTime.Now; // Define el valor de facturación adecuado

                                string insertQuery = $@"set dateformat ymd;INSERT INTO [GestionCobroBD].[dbo].[ControlFacturas]([Empresa],[Tipo],[NoDocumento],[Fecha],[CardCode],[CardName],[Total],[Despacho],[Facturacion],UsuarioFacturacion) VALUES('{empresa}','{Tipo}',{noDocumento},'{fecha:yyyy-MM-dd}','{cardCode}','{cardName}','{total.ToString().Replace(",",".")}','{despacho:yyyy-MM-dd}','{facturacion:yyyy-MM-dd HH:mm:ss}','{Session["user"].ToString()}');";
                                string script = $"{insertQuery.Replace("'", "\\'")}";

                                string checkIfExistsQuery = $@"SELECT isnull(UsuarioFacturacion,'') FROM [GestionCobroBD].[dbo].[ControlFacturas] WHERE [Empresa] = '{empresa}' AND [Tipo] = '{Tipo}' AND [NoDocumento] = {noDocumento}";
                                //MostrarNotificacionToast(insertQuery,"success");
                               /* master masterPage = this.Master as master;

                                if (masterPage != null)
                                {
                                    // Llamar al método MostrarMensaje en la página maestra
                                    masterPage.Mensaje(script);
                                }*/
                                String flag = (ConexionSQL.ConsultaUnica(checkIfExistsQuery));
                                if (flag.Length > 0)
                                {

                                    MostrarNotificacionToast($"Este documento ya fue escaneado por {flag}", "error");
                                    this.txtEscanear.Text = "";
                                    this.txtEscanear.Focus();
                                }
                                else
                                {
                                    int i = ConexionSQL.DML(insertQuery);
                                    if (i > 0)
                                    {
                                        CargarFacturacion();
                                        CargarDatos();
                                        MostrarNotificacionToast($"Documento cargado correctamente", "success");
                                        this.txtEscanear.Text = "";
                                        this.txtEscanear.Focus();
                                    }
                                    else
                                    {

                                        MostrarNotificacionToast($"Ha ocurrido un error al cargar el documento", "error");
                                        this.txtEscanear.Text = "";
                                        this.txtEscanear.Focus();
                                    }
                                }

                            }
                        }
                        else
                        {

                            MostrarNotificacionToast($"No se encuentra ninguna coincidencia", "error");
                            this.txtEscanear.Text = "";
                            this.txtEscanear.Focus();
                        }
                    } 

                }
            }
            catch (Exception ex) {

                MostrarNotificacionToast(ex.Message, "error");
                this.txtEscanear.Text = "";
                this.txtEscanear.Focus();
            }
        }
        protected void CargarCuentasPorCobrar(String Filtro)
        {
            try
            {
                String NumeroFact = "", Empresa = "", Tipo = "";
                DataTable dt = new DataTable();
                if (Filtro.Length > 0)
                {
                    NumeroFact = Filtro.Substring(4, Filtro.Length - 4);
                    String Em = Filtro.Substring(0, 2);
                    switch (Em)
                    {
                        case "00":
                            Empresa = "Empagro";
                            break;
                        case "01":
                            Empresa = "Rosa";
                            break;
                        case "02":
                            Empresa = "Don Beto";
                            break;
                    }
                    String TI = Filtro.Substring(2, 2);
                    switch (TI)
                    {
                        case "00":
                            Tipo = "FC";
                            break;
                        case "01":
                            Tipo = "NC";
                            break;
                    }
                    String osql = $"Select * from [GestionCobroBD].[dbo].[ControlFacturas] where Empresa='{Empresa}' and NoDocumento='{NumeroFact}' and CuentasPorCobrar is null";
                    dt = ConexionSQL.consultaDataTable(osql, "ControlDespacho");
                    if (dt.Rows.Count> 0)
                    {
                        osql = $@"Update a set CuentasPorCobrar=getdate(),UsuarioCuentasporCobrar='{Session["user"].ToString()}' from [GestionCobroBD].[dbo].[ControlFacturas] a where Empresa='{Empresa}' and NoDocumento={NumeroFact}; ";
                        int i = ConexionSQL.DML(osql);
                        if (i > 0)
                        {
                            CargarFacturacion();
                            MostrarNotificacionToast($"Documento cargado correctamente", "success");
                            this.txtEscanearCxC.Text = "";
                            this.txtEscanearCxC.Focus();
                        }
                        else {
                            MostrarNotificacionToast($"Ha ocurrido un error al cargar el documento", "error");
                            this.txtEscanear.Text = "";
                            this.txtEscanear.Focus();
                        }

                    }
                    else {
                         MostrarNotificacionToast($"No se encuentra ninguna coincidencia", "error");
                        this.txtEscanearCxC.Text = "";
                        this.txtEscanearCxC.Focus();
                    }
                }
            }
            catch (Exception ex) {

                MostrarNotificacionToast($"{ex.Message}", "error");
                this.txtEscanearCxC.Text = "";
                this.txtEscanearCxC.Focus();
            }
        }
    private void MostrarNotificacionToast(string mensaje, string tipo)
        {
            string script = $@"
    Toastify({{ 
        text: '{mensaje}',
        duration: 3000,
        close: true,
        gravity: 'top', // `top` or `bottom`
        position: 'left', // `left`, `center` or `right`
        stopOnFocus: true,
        backgroundColor: {(tipo == "success" ? "'#4caf50'" : "'#f44336'")}
    }}).showToast();";

            ScriptManager.RegisterStartupScript(this, GetType(), "MostrarNotificacion", script, true);


            // Registra el script en el cliente
        }

        protected void grvFacturacion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            FacturasPendiente.PageIndex = e.NewPageIndex;
            CargarFacturacion();
        }

        protected void btnEliminar_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btnEliminar = (ImageButton)sender;
            GridViewRow row = (GridViewRow)btnEliminar.NamingContainer;

            string empresa = row.Cells[0].Text; // Obtén la Empresa de la primera celda
            int noDocumento = Convert.ToInt32(row.Cells[1].Text); // Obtén el NoDocumento de la segunda celda

            String osql = $"Delete from [GestionCobroBD].[dbo].[ControlFacturas] where Empresa='{empresa}' and NoDocumento='{noDocumento}'";
            int i =ConexionSQL.DML(osql);
            if (i > 0) {
                MostrarNotificacionToast($"Documento eliminado correctamente", "success");
                this.txtEscanear.Text = "";
                this.txtEscanear.Focus();
            }
                CargarFacturacion();
                CargarDatos();
        }

        protected void txtEscanearCxC_TextChanged(object sender, EventArgs e)
        {
            CargarCuentasPorCobrar(txtEscanearCxC.Text);
        }

        protected void btnEliminarCxC_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btnEliminar = (ImageButton)sender;
            GridViewRow row = (GridViewRow)btnEliminar.NamingContainer;

            string empresa = row.Cells[0].Text; // Obtén la Empresa de la primera celda
            int noDocumento = Convert.ToInt32(row.Cells[1].Text); // Obtén el NoDocumento de la segunda celda

            String osql = $"Update a set CuentasPorCobrar=null from [GestionCobroBD].[dbo].[ControlFacturas] a where Empresa='{empresa}' and NoDocumento='{noDocumento}'";
            int i = ConexionSQL.DML(osql);
            if (i > 0)
            {
                MostrarNotificacionToast($"Documento eliminado correctamente", "success");
                this.txtEscanearCxC.Text = "";
                this.txtEscanearCxC.Focus();
            }
            CargarFacturacion();

        }

        protected void grvCuentasPorCobrar_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvCuentasPorCobrar.PageIndex = e.NewPageIndex;
            CargarFacturacion();

        }

        protected void ibtnAsignados_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton imgbtn = (ImageButton)sender;
            DataTable despacho = new DataTable();
            DataTable facturacion = new DataTable();
            DataTable CuentasporCobrar = new DataTable();
            switch (imgbtn.CommandName)
            {
                case "Despacho":
            despacho = HANAConnection.DQL("SELECT \"BD\", \"NoFact\", TO_VARCHAR(\"Fecha\", 'dd/MM/yyyy') AS \"Fecha\", \"CardCode\", \"CardName\", \"DocTotal\", (\"BD\" || \"NoFact\") AS \"Filtro\" " +
                                   "FROM \"SB1LD_EPG_PRO\".\"VW_Facturas\" " +
                                   "WHERE DAYS_BETWEEN(\"Fecha\", CURRENT_DATE) <= 7");
            ExportToExcel(despacho,"Despacho");
                    break;
                case "Facturacion":
                    String osql = $"Select * from [GestionCobroBD].[dbo].[ControlFacturas] where CuentasPorCobrar is null";
                    facturacion = ConexionSQL.consultaDataTable(osql, "Facturacion");
                    ExportToExcel(facturacion, "Facturacion");
                    break;
                case "CxC":
            String osql1 = $"Select * from [GestionCobroBD].[dbo].[ControlFacturas] where CuentasPorCobrar is not null";
            CuentasporCobrar = ConexionSQL.consultaDataTable(osql1, "CuentasPorCobrar");
                    ExportToExcel(CuentasporCobrar, "CxC");
                    break;
            }
        }

        protected void txtDesde_TextChanged(object sender, EventArgs e)
        {
            CargarDatos();
        }
    }
}