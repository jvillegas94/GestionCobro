using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
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
                if (Session["user"].ToString().Equals("Depto TI | Empagro"))
                {
                    txtFecha.Enabled = true;
                }
                else
                {
                    txtFecha.Enabled = false;
                }
                String pfecha = Configuracion.get(Server.MapPath("Archivos/configuracion.json")).Fecha;
                txtFecha.Text = pfecha;
                GetList();
                CargarDatos(txtBuscar.Text);
                CargarProcesadas();
            }
            else {

                txtBuscar.Focus();
            }
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            CargarDatos(txtBuscar.Text);
        }

        protected void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            CargarDatos(txtBuscar.Text);

        }
        protected void CargarDatos(String Filtro)
        {
            String NumeroFact = "", Empresa = "", Tipo = "";
            DataTable dt = new DataTable();
            if (!chkManual.Checked)
            {
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
                }
                if (Empresa.Length != 0)
                {
                    var count = lista.Where(x => x != null && x.BD == Empresa && x.NoFact == NumeroFact).Count();
                    if (count == 0)
                    {
                        dt = HANAConnection.DQL(String.Format("select \"BD\",\"NoFact\", TO_VARCHAR(\"Fecha\", 'dd/MM/yyyy')\"Fecha\", \"CardCode\", \"CardName\", \"DocTotal\",(\"BD\"||\"NoFact\")\"Filtro\" from \"SB1LD_EPG_PRO\".\"VW_Facturas\" where \"Fecha\" > '{2}' and \"BD\"='{0}' and \"NoFact\"='{1}' ", Empresa, NumeroFact, txtFecha.Text));
                        if (dt.Rows.Count > 0)
                        {
                            FacturasProcesadas fp = new FacturasProcesadas();
                            fp.BD = Empresa;
                            fp.Monto = Convert.ToDouble(dt.Rows[0]["DocTotal"].ToString());
                            fp.NoFact = NumeroFact;
                            fp.Socio = dt.Rows[0]["CardName"].ToString();
                            lista.Add(fp);
                        }
                        Filtro = "";
                            dt = HANAConnection.DQL(String.Format("select \"BD\",\"NoFact\", TO_VARCHAR(\"Fecha\", 'dd/MM/yyyy')\"Fecha\", \"CardCode\", \"CardName\", \"DocTotal\",(\"BD\"||\"NoFact\")\"Filtro\" from \"SB1LD_EPG_PRO\".\"VW_Facturas\" where \"Fecha\" > '{1}' and \"BD\"||TO_VARCHAR(\"Fecha\", 'dd/MM/yyyy')||\"CardCode\"||\"CardName\"||\"DocTotal\"||\"NoFact\" like '%{0}%'", Filtro, txtFecha.Text));
                }
                        else
                        {
                        Filtro = "";
                            dt = HANAConnection.DQL(String.Format("select \"BD\",\"NoFact\", TO_VARCHAR(\"Fecha\", 'dd/MM/yyyy')\"Fecha\", \"CardCode\", \"CardName\", \"DocTotal\",(\"BD\"||\"NoFact\")\"Filtro\" from \"SB1LD_EPG_PRO\".\"VW_Facturas\" where \"Fecha\" > '{1}' and \"BD\"||TO_VARCHAR(\"Fecha\", 'dd/MM/yyyy')||\"CardCode\"||\"CardName\"||\"DocTotal\"||\"NoFact\" like '%{0}%'", Filtro, txtFecha.Text));
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Documento procesado anteriormente')", true);
                        }
                GuardarListaAsync();
                CargarProcesadas();
                txtBuscar.Text = "";
                }
                else
                {
                    dt = HANAConnection.DQL(String.Format("select \"BD\",\"NoFact\", TO_VARCHAR(\"Fecha\", 'dd/MM/yyyy')\"Fecha\", \"CardCode\", \"CardName\", \"DocTotal\",(\"BD\"||\"NoFact\")\"Filtro\" from \"SB1LD_EPG_PRO\".\"VW_Facturas\" where \"Fecha\" > '{1}' and \"BD\"||TO_VARCHAR(\"Fecha\", 'dd/MM/yyyy')||\"CardCode\"||\"CardName\"||\"DocTotal\"||\"NoFact\" like '%{0}%'", Filtro, txtFecha.Text));
                }
            }
            else
            {
                if (Filtro.Length > 0)
                {
                    dt = HANAConnection.DQL(String.Format("select \"BD\",\"NoFact\", TO_VARCHAR(\"Fecha\", 'dd/MM/yyyy')\"Fecha\", \"CardCode\", \"CardName\", \"DocTotal\",(\"BD\"||\"NoFact\")\"Filtro\" from \"SB1LD_EPG_PRO\".\"VW_Facturas\" where \"Fecha\" > '{1}' and \"BD\"||TO_VARCHAR(\"Fecha\", 'dd/MM/yyyy')||\"CardCode\"||\"CardName\"||\"DocTotal\"||\"NoFact\" like '%{0}%'", Filtro, txtFecha.Text));
                }
                else
                {
                    dt = HANAConnection.DQL(String.Format("select \"BD\",\"NoFact\", TO_VARCHAR(\"Fecha\", 'dd/MM/yyyy')\"Fecha\", \"CardCode\", \"CardName\", \"DocTotal\",(\"BD\"||\"NoFact\")\"Filtro\" from \"SB1LD_EPG_PRO\".\"VW_Facturas\" where \"Fecha\" > '{0}'", txtFecha.Text));
                }
            }
            
            DataView view = new DataView(dt);
            StringBuilder sb = new StringBuilder();
            bool first = true;
            if (lista != null)
            {
                foreach (FacturasProcesadas fp in lista)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        sb.Append(" AND ");
                    }

                    sb.AppendFormat("Filtro<>'{0}{1}'", fp.BD, fp.NoFact);
                }
            }
            view.RowFilter = sb.ToString();
            FacturasPendiente.DataSource = view;
            FacturasPendiente.DataBind();
            txtBuscar.Focus();
        }

        protected  void GuardarListaAsync() {
            var jsonString = JsonConvert.SerializeObject(lista, Formatting.Indented);
            File.WriteAllText(Server.MapPath("Archivos/FacturasProcesadas.json"), jsonString);


        }
        private static List<FacturasProcesadas> lista;
        protected void GetList() {
            StreamReader r = new StreamReader(Server.MapPath("Archivos/FacturasProcesadas.json"));
            string json = r.ReadToEnd();
            r.Close();
            List<FacturasProcesadas> items = JsonConvert.DeserializeObject<List<FacturasProcesadas>>(json);
            lista=items;
            if (lista == null) {
                lista = new List<FacturasProcesadas>();
            }

        }
        protected void CargarProcesadas()
        {
            try
            {
                grvProcesadas.DataSource = lista;
                grvProcesadas.DataBind();
            }
            catch (Exception ex) { }
        }

        protected void FacturasPendiente_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            FacturasPendiente.PageIndex = e.NewPageIndex;
            CargarDatos(txtBuscar.Text);

        }

        protected void grvProcesadas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvProcesadas.PageIndex = e.NewPageIndex;
            CargarProcesadas();
        }

        protected void txtBuscarProcesadas_TextChanged(object sender, EventArgs e)
        {
            CargarProcesadas();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            DataTable dt = HANAConnection.DQL(String.Format("select \"BD\",\"NoFact\", TO_VARCHAR(\"Fecha\", 'dd/MM/yyyy')\"Fecha\", \"CardCode\", \"CardName\", \"DocTotal\" from \"SB1LD_EPG_PRO\".\"VW_Facturas\" where \"Fecha\" > '{0}'", txtFecha.Text));
            ExportToExcel(dt, String.Format("Pendientes al {0}", txtFecha.Text));
        }
        private void ExportToExcel(System.Data.DataTable dt, String nombre)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", String.Format("attachment;filename={0}.xls", nombre));
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
                            cell.BackColor = Color.DarkGray;
                            cell.ForeColor = Color.White;
                        }
                        else
                        {
                            cell.BackColor = Color.White;
                            cell.ForeColor = Color.Black;
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
        protected void txtFecha_TextChanged(object sender, EventArgs e)
        {
            Configuracion conf = new Configuracion();
            conf.Fecha = txtFecha.Text;
            var jsonString = JsonConvert.SerializeObject(conf, Formatting.Indented);
            File.WriteAllText(Server.MapPath("Archivos/configuracion.json"), jsonString);
        }
    }
}