using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestionCobro
{
    public partial class liquidar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    if (Session["User"] != null)
                    {
                        TipoPago();
                        TipoDocumento();
                        txtNumFact.Focus();
                        txtLiquidador.Text = Session["User"].ToString();
                        Liquidaciones();

                        PnlManual.Visible = true;
                        PnlAutomatico.Visible = false;
                        btnBuscar.Visible = true;
                    }
                    else
                    {
                        Response.Redirect("login.aspx");
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(string.Format("<script>alert('{0}');</script>", ex.Message));
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            String NumFact = "", Empresa = "", Tipo = "";
            if (chkBoxAutomatico.Checked)
            {
                if (txtAutomatico.Text.Length > 0)
                {
                    NumFact = txtAutomatico.Text.Substring(4, txtAutomatico.Text.Length - 4);
                    String Em = txtAutomatico.Text.Substring(0, 2);
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
                    String TI = txtAutomatico.Text.Substring(2, 2);
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
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Debe escanear el código');", true);
                }
            }
            else
            {
                NumFact = txtNumFact.Text;
                Empresa = RadioButtonList1.SelectedValue;
                Tipo = RadioButtonList2.SelectedValue;

            }


            Liquidacion liquidacion = new Liquidacion(NumFact,Empresa,Tipo);
            if (!liquidacion.Ruta1.Equals("")) {
                if (!chkBoxAutomatico.Checked)
                {
                    lblEmpresa.Text = liquidacion.Empresa1;
                    lblFact.Text = liquidacion.DocNum1;
                    lblTipo.Text = liquidacion.Tipo;
                }
                else
                {
                    lblEmpresa.Text = Empresa;
                    lblFact.Text = NumFact;
                    lblTipo.Text = Tipo;
                }
                lblCliente.Text =liquidacion.CardName1;
                lblTotal.Text = liquidacion.Total1;
                lblResult.Text = "";
                    ddlTipoDocumento.Focus();
            }
            else {
                lblResult.Text = String.Format("No se ha registrado la factura {0}",txtNumFact.Text);
                lblResult.ForeColor = Color.Red;
            }
        }
        protected void TipoPago()
        {
            
            DataTable dt=HANAConnection.DQL(string.Format("Select b.{0}Descr{0} from {0}SB1LD_EPG_PRO{0}.CUFD a inner join {0}SB1LD_EPG_PRO{0}.UFD1 b on a.{0}FieldID{0} = b.{0}FieldID{0} where {0}AliasID{0} = 'TipoPago' and a.{0}TableID{0} = '@LOG_COBRO'", (char)34));
            if (dt.Rows.Count > 0)
            {
                ddlTipoPago.DataSource = dt;
                ddlTipoPago.DataTextField = "Descr";
                ddlTipoPago.DataValueField = "Descr";
                ddlTipoPago.DataBind();
            }
        }
        protected void TipoDocumento()
        {
            
            DataTable dt=HANAConnection.DQL(string.Format("Select b.{0}Descr{0} from {0}SB1LD_EPG_PRO{0}.CUFD a inner join {0}SB1LD_EPG_PRO{0}.UFD1 b on a.{0}FieldID{0} = b.{0}FieldID{0} where {0}AliasID{0} = 'TipoDocumento' and b.{0}TableID{0} = '@LOG_COBRO'", (char)34));
            if (dt.Rows.Count > 0)
            {
                ddlTipoDocumento.DataSource = dt;
                ddlTipoDocumento.DataTextField = "Descr";
                ddlTipoDocumento.DataValueField = "Descr";
                ddlTipoDocumento.DataBind();
            }
        }

        protected void btnAplicar_Click(object sender, EventArgs e)
        {
            ActualizarCodigos(lblFact.Text,lblEmpresa.Text,lblTipo.Text);
        }
        protected void ActualizarCodigos(String NumFact,String Empresa,String Tipo)
        {
            try
            {
                if (lblFact.Text.Equals(""))
                {
                    lblResult.Text = "Debe ingresar un número de factura válido";
                    lblResult.ForeColor=Color.Red;
                    txtNumFact.Focus();
                }
                else
                {
                    string TipoPago = ddlTipoPago.SelectedValue;
                    string monto = txtMonto.Text;
                    string fecha = txtFechaRecibo.Text;
                    string NumRecibo = txtNumeroRecibo.Text;
                    string Original = "";
                    string Tramite = "";
                    if (!ddlTipoDocumento.SelectedValue.Equals("Recibo")) {
                        TipoPago = "";
                        monto = "0";
                        fecha = "";
                        NumRecibo = "0";
                        if (ddlTipoDocumento.SelectedValue.Equals("Devuelve Original"))
                        {
                            Original = "Si";
                            Tramite = "";
                        }
                        if (ddlTipoDocumento.SelectedValue.Equals("Devuelve Trámite"))
                        {
                            Original = "";
                            Tramite = "Si";
                        }

                    }
                    
                    StringBuilder osql = new StringBuilder();
                        string FechaLiquidacion = DateTime.Now.ToString("yyyy/MM/dd");
                        string HoraLiquidacion = DateTime.Now.ToString("HH:mm:ss");
                    Liquidacion liquidacion = new Liquidacion(lblFact.Text,lblEmpresa.Text,lblTipo.Text);
                    string EstadoDoc = (string)HANAConnection.Excalar(string.Format("Select Max(ifnull({0}U_Estado{0},'N/A')){0}Estado{0} from {0}SB1LD_EPG_PRO{0}.{0}@LOG_COBRO{0} where {0}U_DocNum{0}='{1}' and {0}U_Empresa{0}='{2}' and {0}U_Tipo{0}='{3}'", (char)34, lblFact.Text, lblEmpresa.Text, lblTipo.Text));
                    if (EstadoDoc.Equals("Abierta")|| EstadoDoc.Equals("N/A"))
                    {
                        lblResult.Text = "No puede liquidar un documento de una ruta y periodo que se encuentra abierta";
                        lblResult.ForeColor = Color.Red;
                    }
                    else
                    {
                        string VaDenuevo = "0";
                        if (chkVaDeNuevo.Checked) {
                            VaDenuevo = "1";
                        }
                        string osql1 = "";
                        if (ddlTipoDocumento.SelectedValue == "Recibo")
                        {
                            
                            osql1 = String.Format("Insert into Bitacora(Usuario,Accion,FechaInicio,FechaFin,Ruta,Code,Nombre,FechaRegistro,HoraRegistro,DocNum,Empresa,Asignador,TipoDocumento,TipoPago,ReciboNum,MontoRecibo,Liquidador,FechaLiquidacion,HoraLiquidacion,FechaRecibo,Estado,Monto) values('{0}','{1}','{2}','{3}','{4}','{5}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}')", Session["user"].ToString(), "Liquidación de Documento", liquidacion.FechaInicio1, liquidacion.FechaFin1, liquidacion.Ruta1, liquidacion.Codigo1, liquidacion.FechaRegistro, liquidacion.HoraRegistro, lblFact.Text, lblEmpresa.Text, liquidacion.Asignador, ddlTipoDocumento.SelectedValue, TipoPago, NumRecibo, monto, txtLiquidador.Text, FechaLiquidacion, HoraLiquidacion, fecha, liquidacion.EstadoAsignacion, liquidacion.Monto);
                        }
                        else
                        {
                            osql1 = String.Format("Insert into Bitacora(Usuario,Accion,FechaInicio,FechaFin,Ruta,Code,Nombre,FechaRegistro,HoraRegistro,DocNum,Empresa,Asignador,TipoDocumento,TipoPago,ReciboNum,MontoRecibo,Liquidador,FechaLiquidacion,HoraLiquidacion,FechaRecibo,Estado,Monto) values('{0}','{1}','{2}','{3}','{4}','{5}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}')", Session["user"].ToString(), "No se Liquida "+ddlTipoDocumento.SelectedValue, liquidacion.FechaInicio1, liquidacion.FechaFin1, liquidacion.Ruta1, liquidacion.Codigo1, liquidacion.FechaRegistro, liquidacion.HoraRegistro, lblFact.Text, lblEmpresa.Text, liquidacion.Asignador, "", TipoPago, NumRecibo, monto, txtLiquidador.Text, FechaLiquidacion, HoraLiquidacion, fecha, liquidacion.EstadoAsignacion, liquidacion.Monto);
                        }
                        if (ddlTipoDocumento.SelectedValue == "Recibo")
                        {
                            osql.Append(("Update a set "));
                            osql.Append(string.Format(" {0}U_TipoDocumento{0}='{1}',{0}U_TipoPago{0}='{2}',{0}U_ReciboNum{0}='{3}',{0}U_MontoRecibo{0}='{4}',{0}U_FechaLiquidacion{0}='{5}',{0}U_HoraLiquidacion{0}='{6}',{0}U_Liquidador{0}='{7}',{0}U_FechaRecibo{0}='{8}',{0}U_Original{0}='{9}',{0}U_Tramite{0}='{10}',{0}U_VaDenuevo{0}='{11}'", (char)34, ddlTipoDocumento.SelectedValue, TipoPago, NumRecibo, monto, FechaLiquidacion, HoraLiquidacion, txtLiquidador.Text, fecha, Original, Tramite, VaDenuevo));
                            osql.Append(string.Format(" from {0}SB1LD_EPG_PRO{0}.{0}@LOG_COBRO{0} a", (char)34));
                            osql.Append(string.Format(" where {0}U_DocNum{0}='{1}' and {0}U_Empresa{0}='{2}' and {0}U_Tipo{0}='{3}'", (char)34, lblFact.Text, lblEmpresa.Text,lblTipo.Text));
                        }
                        else
                        {
                            osql.Append(("Update a set "));
                            osql.Append(string.Format(" {0}U_TipoDocumento{0}='{1}',{0}U_TipoPago{0}='{2}',{0}U_ReciboNum{0}='{3}',{0}U_MontoRecibo{0}='{4}',{0}U_FechaLiquidacion{0}='{5}',{0}U_HoraLiquidacion{0}='{6}',{0}U_Liquidador{0}='{7}',{0}U_FechaRecibo{0}='{8}',{0}U_Original{0}='{9}',{0}U_Tramite{0}='{10}',{0}U_VaDenuevo{0}='{11}'", (char)34, "", TipoPago, NumRecibo, monto, FechaLiquidacion, HoraLiquidacion, txtLiquidador.Text, fecha, Original, Tramite, VaDenuevo));
                            osql.Append(string.Format(" from {0}SB1LD_EPG_PRO{0}.{0}@LOG_COBRO{0} a", (char)34));
                            osql.Append(string.Format(" where {0}U_DocNum{0}='{1}' and {0}U_Empresa{0}='{2}' and {0}U_Tipo{0}='{3}'", (char)34, lblFact.Text,lblEmpresa.Text,lblTipo.Text));
                        }
                        int i = HANAConnection.DML(osql.ToString());
                        if (i >= 0)
                        {
                        ConexionSQL.DML(osql1);
                            lblResult.Text = "Liquidacion efectuada correctamente";
                            lblResult.ForeColor = Color.Green;
                            if (!chkBoxAutomatico.Checked)
                            {
                                txtNumFact.Text = "";
                                txtNumFact.Focus();
                            }
                            else {

                                txtAutomatico.Text = "";
                                txtAutomatico.Focus();
                            }
                            Liquidaciones();
                        }
                        else
                        {
                            lblResult.Text = "Ha ocurrido un error";
                            lblResult.ForeColor = Color.Red;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                lblResult.Text = ex.Message;
                lblResult.ForeColor = Color.Red;
            }
        }

        protected void ddlTipoDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!ddlTipoDocumento.SelectedValue.Equals("Recibo"))
            {
                ddlTipoPago.Enabled = false;
                txtNumeroRecibo.Enabled = false;
                txtFechaRecibo.Enabled = false;
                txtMonto.Enabled = false;
                btnAplicar.Focus();
                chkVaDeNuevo.Visible = true;
            }
            else
            {
                ddlTipoPago.Enabled = true;
                txtNumeroRecibo.Enabled = true;
                txtFechaRecibo.Enabled = true;
                txtMonto.Enabled = true;
                txtFechaRecibo.Focus();
                chkVaDeNuevo.Visible = false;
            }
        }
        protected void Liquidaciones()
        {
            StringBuilder osql = new StringBuilder();
            osql.Append(String.Format("Select TO_CHAR({0}U_ReciboNum{0}){0}Recibo{0},{0}U_MontoRecibo{0} {0}Monto{0},TO_Char({0}U_FechaRecibo{0},'dd/MM/yyyy'){0}Fecha{0},TO_CHAR({0}U_DocNum{0}){0}Documento{0},{0}U_Empresa{0} {0}Empresa{0},{0}U_Tipo{0} {0}Tipo{0} from {0}SB1LD_EPG_PRO{0}.{0}@LOG_COBRO{0}", (char)34));
            osql.Append(String.Format(" where {0}U_ReciboNum{0}>0", (char)34));
            osql.Append(String.Format(" order by {0}U_FechaLiquidacion{0} desc,{0}U_HoraLiquidacion{0} desc", (char)34));
            DataTable dt=HANAConnection.DQL(osql.ToString());
            if (dt.Rows.Count > 0)
            {
                dtgLiquidaciones.DataSource=dt;
                dtgLiquidaciones.DataBind();
            }
        }
        protected void btnCancelarLiquidacion_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder osql = new StringBuilder();
                osql.Append(String.Format("Update a set {0}U_ReciboNum{0}='0',{0}U_MontoRecibo{0}='0',{0}U_TipoDocumento{0}='',{0}U_TipoPago{0}='',{0}U_FechaLiquidacion{0}='',{0}U_HoraLiquidacion{0}='',{0}U_Liquidador{0}='',{0}U_FechaRecibo{0}='',{0}U_Tramite{0}='',{0}U_Original{0}=''", (char)34));
                osql.Append(String.Format(" from {0}SB1LD_EPG_PRO{0}.{0}@LOG_COBRO{0} a", (char)34));
                osql.Append(String.Format(" where {0}U_DocNum{0}='{1}' and {0}U_Empresa{0}='{2}' and {0}U_Tipo{0}='{3}'", (char)34,lblFact.Text, lblEmpresa.Text,lblTipo.Text));
                Liquidacion liquidacion = new Liquidacion(lblFact.Text, lblEmpresa.Text,lblTipo.Text);
                int i = HANAConnection.DML(osql.ToString());
                if (i > 0)
                {
                    string osql1 = String.Format("Insert into Bitacora(Usuario,Accion,FechaInicio,FechaFin,Ruta,Code,Nombre,FechaRegistro,HoraRegistro,DocNum,Empresa,Asignador,TipoDocumento,TipoPago,ReciboNum,MontoRecibo,Liquidador,FechaLiquidacion,HoraLiquidacion,FechaRecibo,Estado,Monto) values('{0}','{1}','{2}','{3}','{4}','{5}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}')", Session["user"].ToString(), "Cancelar Liquidacion", liquidacion.FechaInicio1, liquidacion.FechaFin1, liquidacion.Ruta1, liquidacion.Codigo1, liquidacion.FechaRegistro, liquidacion.HoraRegistro, liquidacion.DocNum1, liquidacion.Empresa1, liquidacion.Asignador, liquidacion.TipoDocumento, liquidacion.TipoPago, liquidacion.NumRecibo1, liquidacion.MontoRecibo, liquidacion.Liquidador, liquidacion.FechaLiquidacion, liquidacion.HoraLiquidacion, liquidacion.FechaLiquidacion, liquidacion.EstadoAsignacion, liquidacion.Monto);
                    ConexionSQL.DML(osql1);
                    lblResult.Text = "Liquidacion cancelada correctamente";
                    lblResult.ForeColor = Color.Green;
                    txtNumFact.Text = "";
                    txtNumFact.Focus();
                    Liquidaciones();
                }
                else
                {
                        lblResult.Text = "Ha ocurrido un error";
                        lblResult.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblResult.Text = ex.Message;
                lblResult.ForeColor = Color.Red;
            }
        }

        protected void dtgLiquidaciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            try
            {
                string comando = e.CommandName;
                if (!comando.Equals("Page"))
                {
                    int index = Convert.ToInt32(e.CommandArgument);
                    GridViewRow row = dtgLiquidaciones.Rows[index];
                    string filename = Server.HtmlDecode(row.Cells[3].Text);
                    string Empresa = Server.HtmlDecode(row.Cells[4].Text);
                    string tipo = Server.HtmlDecode(row.Cells[5].Text);
                    StringBuilder osql = new StringBuilder();
                    osql.Append(String.Format("Update a set {0}U_ReciboNum{0}='0',{0}U_MontoRecibo{0}='0',{0}U_TipoDocumento{0}='',{0}U_TipoPago{0}='',{0}U_FechaLiquidacion{0}='',{0}U_HoraLiquidacion{0}='',{0}U_Liquidador{0}='',{0}U_FechaRecibo{0}='',{0}U_Tramite{0}='',{0}U_Original{0}=''", (char)34));
                    osql.Append(String.Format(" from {0}SB1LD_EPG_PRO{0}.{0}@LOG_COBRO{0} a", (char)34));
                    osql.Append(String.Format(" where {0}U_DocNum{0}='{1}' and {0}U_Empresa{0}='{2}' and {0}U_Tipo{0}='{3}'", (char)34, filename,Empresa,tipo));
                    Liquidacion liquidacion = new Liquidacion(filename,Empresa,tipo);
                    int i = HANAConnection.DML(osql.ToString());
                    if (i > 0)
                    {
                    string osql1 = String.Format("Insert into Bitacora(Usuario,Accion,FechaInicio,FechaFin,Ruta,Code,Nombre,FechaRegistro,HoraRegistro,DocNum,Empresa,Asignador,TipoDocumento,TipoPago,ReciboNum,MontoRecibo,Liquidador,FechaLiquidacion,HoraLiquidacion,FechaRecibo,Estado,Monto) values('{0}','{1}','{2}','{3}','{4}','{5}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}')", Session["user"].ToString(), "Cancelar Liquidacion", liquidacion.FechaInicio1, liquidacion.FechaFin1, liquidacion.Ruta1, liquidacion.Codigo1, liquidacion.FechaRegistro, liquidacion.HoraRegistro, liquidacion.DocNum1, liquidacion.Empresa1, liquidacion.Asignador, liquidacion.TipoDocumento, liquidacion.TipoPago, liquidacion.NumRecibo1, liquidacion.MontoRecibo, liquidacion.Liquidador, liquidacion.FechaLiquidacion, liquidacion.HoraLiquidacion, liquidacion.FechaLiquidacion, liquidacion.EstadoAsignacion, liquidacion.Monto);
                    ConexionSQL.DML(osql1);
                        lblResult.Text = "Liquidacion cancelada correctamente";
                        lblResult.ForeColor = Color.Green;
                        txtBuscar.Text = "";
                        txtBuscar.Focus();
                        Liquidaciones();
                    }
                    else
                    {
                        lblResult.Text = "Ha ocurrido un error";
                        lblResult.ForeColor = Color.Red;
                    }
                }
            }
            catch (Exception ex)
            {
                lblResult.Text = ex.Message;
                lblResult.ForeColor = Color.Red;
            }
        }

        protected void dtgLiquidaciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            dtgLiquidaciones.PageIndex = e.NewPageIndex;
           Liquidaciones();
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            StringBuilder osql = new StringBuilder();
            osql.Append(String.Format("Select TO_CHAR({0}U_ReciboNum{0}){0}Recibo{0},{0}U_MontoRecibo{0} {0}Monto{0},TO_Char({0}U_FechaRecibo{0},'dd/MM/yyyy'){0}Fecha{0},TO_CHAR({0}U_DocNum{0}){0}Documento{0},{0}U_Empresa{0} {0}Empresa{0},{0}U_Tipo{0} {0}Tipo{0} from {0}SB1LD_EPG_PRO{0}.{0}@LOG_COBRO{0}", (char)34));
            osql.Append(String.Format(" where {0}U_DocNum{0}={1} or {0}U_ReciboNum{0}='{1}'", (char)34,txtBuscar.Text));
            osql.Append(String.Format(" order by {0}U_FechaLiquidacion{0} desc,{0}U_HoraLiquidacion{0} desc", (char)34));
            DataTable dt=HANAConnection.DQL(osql.ToString());
            if (dt.Rows.Count > 0)
            {
                dtgLiquidaciones.DataSource = dt;
                dtgLiquidaciones.DataBind();
                txtBuscar.Text = "";
                txtBuscar.Focus();
            }
            else
            {
                lblResult.Text = "No se encuentra ninguna coincidencia";
                lblResult.ForeColor = Color.Red;
            }
        }

        protected void chkBoxAutomatico_CheckedChanged(object sender, EventArgs e)
        {

            if (chkBoxAutomatico.Checked)
            {
                PnlManual.Visible = false;
                PnlAutomatico.Visible = true;
                btnBuscar.Visible = false;
            }
            else
            {
                PnlManual.Visible = true;
                PnlAutomatico.Visible = false;
                btnBuscar.Visible = true;
            }
        }

        protected void txtAutomatico_TextChanged(object sender, EventArgs e)
        {
            btnBuscar_Click(sender,e);
            txtAutomatico.Text = "";
            txtAutomatico.Focus();
        }
    }
}