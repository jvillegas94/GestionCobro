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
                    CargarDatos();
                    CargarFacturacion();
                    this.Label1.Text = Session["dpto"].ToString();
                }
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
            DataTable dt = HANAConnection.DQL("SELECT \"BD\", \"NoFact\", TO_VARCHAR(\"Fecha\", 'dd/MM/yyyy') AS \"Fecha\", \"CardCode\", \"CardName\", \"DocTotal\", (\"BD\" || \"NoFact\") AS \"Filtro\" " +
                                   "FROM \"SB1LD_EPG_PRO\".\"VW_Facturas\" " +
                                   "WHERE DAYS_BETWEEN(\"Fecha\", CURRENT_DATE) <= 7");

            if (dt.Rows.Count > 0)
            {
                FacturasPendiente.DataSource = dt;
                FacturasPendiente.DataBind();
            }


        }
        protected void CargarFacturacion()
        {
            DataTable dt = ConexionSQL.consultaDataTable("Select * from [GestionCobroBD].[dbo].ControlFacturas where CuentasPorCobrar is null", "ControlFacturas");

            if (dt.Rows.Count > 0)
            {
                grvFacturacion.DataSource = dt;
                grvFacturacion.DataBind();
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
                                string tipo = "Tipo";  // Define el valor del tipo adecuado
                                int noDocumento = Convert.ToInt32(row["NoFact"]);
                                DateTime fecha = DateTime.ParseExact(row["Fecha"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                string cardCode = row["CardCode"].ToString();
                                string cardName = row["CardName"].ToString();
                                decimal total = Convert.ToDecimal(row["DocTotal"]);
                                DateTime despacho = fecha; // Define el valor de despacho adecuado
                                DateTime facturacion = DateTime.Now; // Define el valor de facturación adecuado

                                string insertQuery = $@"
                                                INSERT INTO [GestionCobroBD].[dbo].[ControlFacturas]([Empresa],[Tipo],[NoDocumento],[Fecha],[CardCode],[CardName],[Total],[Despacho]
                                                   ,[Facturacion],Usuario)
                                                VALUES('{empresa}','{tipo}',{noDocumento},'{fecha:yyyy-MM-dd}','{cardCode}','{cardName}',{total},'{despacho:yyyy-MM-dd HH:mm:ss}','{facturacion:yyyy-MM-dd HH:mm:ss}','{Session["user"].ToString()}');";
                                string checkIfExistsQuery = $@"SELECT isnull(Usuario,'') FROM [GestionCobroBD].[dbo].[ControlFacturas] WHERE [Empresa] = '{empresa}' AND [Tipo] = '{tipo}' AND [NoDocumento] = {noDocumento}";
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

        protected void grvFacturacion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            FacturasPendiente.PageIndex = e.NewPageIndex;
            CargarFacturacion();
        }
    }
}