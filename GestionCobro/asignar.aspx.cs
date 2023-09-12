using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace GestionCobro
{
    public partial class asignar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["User"] != null)
                {
                    ddlRuta.DataSource = Ruta();
                    ddlRuta.DataTextField = "SlpName";
                    ddlRuta.DataValueField = "SlpName";
                    ddlRuta.DataBind();
                    string desde = Convert.ToDateTime(Request["Desde"]).ToString("yyyy-MM-dd");
                    string hasta = Convert.ToDateTime(Request["Hasta"]).ToString("yyyy-MM-dd");
                    string ruta = Request["ruta"];
                    if (desde.Equals("0001-01-01") && hasta.Equals("0001-01-01")&&ruta==null)
                    {
                        desde = Desde();
                        hasta = Hasta();
                    }
                    if (Session["User"].Equals("Luis Miguel Bolaños Mejias"))
                    {
                        btnActualizar.Visible = true;
                    }
                    else
                    {
                        btnActualizar.Visible = false;
                    }
                    txtDesde.Text = desde;
                    txtHasta.Text = hasta;
                    ddlRuta.SelectedValue = ruta;
                    dtgFacturas.DataSource = Asignaciones();
                    dtgFacturas.DataBind();
                    grvPendientes.DataSource = Pendientes();
                    grvPendientes.DataBind();
                    txtAsignador.Text = Session["User"].ToString();
                    txtNumFact.Text = "";
                    txtNumFact.Focus();
                    chkOmitir.Checked = false;
                    PnlManual.Visible = true;
                    PnlAutomatico.Visible = false;
                    btnAsignar.Visible = true;
                }
            else
            {
                Response.Redirect("login.aspx");
            }
            }
        }

        public DataTable Ruta()
        {
            DataTable dt=HANAConnection.DQL(String.Format("Select {0}SlpName{0} from {0}SB1LD_EPG_PRO{0}.OSLP where {0}SlpName{0} like 'RUTA%' union all Select {0}SlpName{0} from {0}SB1LD_BET1_PRO{0}.OSLP where {0}SlpName{0} like 'RUTA%' ", (char)34));
            return dt;

        }
        public string Desde()
        {
            object flag = HANAConnection.Excalar(string.Format("Select TO_CHAR({0}Desde{0},'yyyy-MM-dd'){0}Desde{0} from {0}SB1LD_EPG_PRO{0}.Vw_Semana", (char)34));
            return flag.ToString();
        }
        public string Hasta()
        {
            object flag = HANAConnection.Excalar(string.Format("Select TO_CHAR({0}Hasta{0},'yyyy-MM-dd'){0}Hasta{0} from {0}SB1LD_EPG_PRO{0}.Vw_Semana", (char)34));
            return flag.ToString();
        }

        protected void Registrar(String NumFact,String Tipo,String Empresa)
        {
            string osql = "";
            try
            {
                object flag = HANAConnection.Excalar(string.Format("Select count(*) from {0}SB1LD_EPG_PRO{0}.{0}@LOG_COBRO{0} where {0}U_DocNum{0} ={1} and {0}U_Empresa{0}='{2}'", (char)34, NumFact, Empresa));
                if (flag.ToString().Equals("0"))
                {
                    decimal qq = (decimal)HANAConnection.Excalar(string.Format("Select QQ from {0}SB1LD_EPG_PRO{0}.{0}VwQQINV{0} where {0}DocNum{0}='{1}' and {0}Empresa{0}='{2}'", (char)34, NumFact, Empresa));
                    string Monto = (string)HANAConnection.Excalar(string.Format("Select to_Char({0}Total{0}) from {0}SB1LD_EPG_PRO{0}.{0}VwQQINV{0} where {0}DocNum{0}='{1}' and {0}Empresa{0}='{2}'", (char)34, NumFact, Empresa));
                    string EstadoPeriodo = (string)HANAConnection.Excalar(string.Format("Select ifnull(Max(ifnull({0}U_Estado{0},'N/A')),'N/A'){0}Estado{0} from {0}SB1LD_EPG_PRO{0}.{0}@LOG_COBRO{0} where {0}U_FechaInicio{0}='{1}' and {0}U_FechaFin{0}='{2}' and {0}U_Ruta{0}='{3}'", (char)34, Convert.ToDateTime(txtDesde.Text).ToString("yyyy-MM-dd"), Convert.ToDateTime(txtHasta.Text).ToString("yyyy-MM-dd"), ddlRuta.SelectedValue));
                    if (EstadoPeriodo.Equals("Cerrada")) {

                        lblResult.Text = ("La ruta seleccionada ya se encuentra cerrada");
                        lblResult.ForeColor = Color.Red;
                        txtNumFact.Text = "";
                        txtNumFact.Focus();
                    }
                    else { 
                    if (qq.Equals(null))
                    {
                        lblResult.Text = "El numero de factura no existe para la empresa seleccionada";
                        lblResult.ForeColor = Color.Red;
                    }
                    if (!Monto.Equals("0.000000"))
                    {
                        string Vendedor = (string)HANAConnection.Excalar(string.Format("Select ({0}SlpName{0}) from {0}SB1LD_EPG_PRO{0}.{0}VwQQINV{0} where {0}DocNum{0}='{1}' and {0}Empresa{0}='{2}'", (char)34, NumFact, Empresa));
                            if (chkOmitir.Checked) {
                                Vendedor = ddlRuta.SelectedValue;
                            }
                        if (Vendedor.Equals(ddlRuta.SelectedValue))
                        {
                            string fecha = DateTime.Now.ToString("yyyy/MM/dd");
                            string hora = DateTime.Now.ToString("HH:mm:ss");
                            object code = HANAConnection.Excalar(string.Format("Select count(*)+1 from {0}SB1LD_EPG_PRO{0}.{0}@LOG_COBRO{0}", (char)34));

                                string tramite = ",''";
                                if (chkTramite.Checked)
                                {
                                    tramite = ",'Trámite'";
                                }
                                osql = string.Format("insert into {0}SB1LD_EPG_PRO{0}.{0}@LOG_COBRO{0} values('{1}','{1}',{2},'{3}','{4}','{5}','{6}','','',0,0,'{7}','{8}','','','{9}','','','Abierta','{10}','','','0','{11}'" + tramite + ")", (char)34, code.ToString(), NumFact, ddlRuta.SelectedValue, txtDesde.Text, txtHasta.Text, Empresa, fecha, hora, txtAsignador.Text, Monto, Tipo, tramite);
                                int i = HANAConnection.DML(osql);
                            if (i > 0)
                            {
                            string osql1 = String.Format("Insert into Bitacora(Usuario,Accion,FechaInicio,FechaFin,Ruta,Code,Nombre,FechaRegistro,HoraRegistro,DocNum,Empresa,Asignador,Monto,Estado) values('{0}','{1}','{2}','{3}','{4}','{5}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','Abierto')", Session["user"].ToString(), "Nuevo Documento Insertado", txtDesde.Text, txtHasta.Text, ddlRuta.SelectedValue, code.ToString(), fecha, hora, NumFact, Empresa, txtAsignador.Text, Monto);
                            ConexionSQL.DML(osql1);
                                lblResult.Text = "La factura " + NumFact + " ha sido registrada";
                                lblResult.ForeColor = Color.Green;
                                dtgFacturas.DataSource = Asignaciones();
                                dtgFacturas.DataBind();
                                grvPendientes.DataSource = Pendientes();
                                grvPendientes.DataBind();
                                txtNumFact.Text = "";
                                txtNumFact.Focus();
                                    chkOmitir.Checked = false;
                            }
                        }
                        else
                        {
                            lblResult.Text = String.Format("La Ruta seleccionada ({0}) no corresponde a la especificada en el documento {1}", ddlRuta.SelectedValue, Vendedor);
                            lblResult.ForeColor = Color.Red;
                            ddlRuta.Focus();
                            ddlRuta.SelectedValue = Vendedor;
                        }
                    }
                    else {
                        lblResult.Text = String.Format("La Factura {0} ya se encuentra cancelada", NumFact);
                        lblResult.ForeColor = Color.Red;
                        txtNumFact.Text = "";
                        txtNumFact.Focus();
                    }
                }
                }
                else
                {
                    /*lblResult.Text = "La factura " + txtNumFact.Text + " ya se encuentra registrada";
                    lblResult.ForeColor = Color.Blue;*/
                    mpePopUp.Show();
                    lblDesdeRA.Text = txtDesde.Text;
                    lblHastaRA.Text = txtHasta.Text;
                    lblRutaRA.Text = ddlRuta.Text;
                    lblNumFact.Text = NumFact;
                    txtNumFact.Text = "";
                    txtNumFact.Focus();
                }
            }
            catch (Exception ex)
            {
                lblResult.Text = ex.Message;
                lblResult.ForeColor = Color.Red;
            }
        }

        private DataTable Asignaciones()
        {
            String osql = string.Format("Select {0}U_Ruta{0},{0}Tipo{0},TO_CHAR({0}DocNum{0}){0}DocNum{0}, {0}CardName{0}, {0}Total{0}, TO_CHAR({0}U_FechaInicio{0}, 'dd/MM/yyyy'){0}Desde{0}, TO_CHAR({0}U_FechaFin{0}, 'dd/MM/yyyy'){0}Hasta{0}, {0}Empresa{0} from {0}SB1LD_EPG_PRO{0}.{0}@LOG_COBRO{0} a inner join {0}SB1LD_EPG_PRO{0}.{0}VwQQINV{0} b on a.{0}U_DocNum{0} = b.{0}DocNum{0} and a.{0}U_Empresa{0} = b.{0}Empresa{0}   and {0}U_Tipo{0}={0}Tipo{0} where {0}U_FechaInicio{0}='{1}' and {0}U_FechaFin{0}='{2}' and {0}Total{0}>0  order by {0}U_FechaRegistro{0} desc,{0}U_HoraRegistro{0} desc", (char)34, txtDesde.Text, txtHasta.Text);
            DataTable dt=HANAConnection.DQL(osql);
            return dt;
        }

        protected void btnAsignar_Click(object sender, EventArgs e)
        {
            String NumeroFact = "",Empresa="",Tipo="";

            if (chkBoxAutomatico.Checked) {
                if (txtAutomatico.Text.Length > 0)
                {
                    NumeroFact = txtAutomatico.Text.Substring(4, txtAutomatico.Text.Length - 4);
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

            } else {
                NumeroFact = txtNumFact.Text;
                Empresa = ddlEmpresa.SelectedValue;
                Tipo = ddlTipo.SelectedValue;
            }

            try
            {
                if (NumeroFact.Length > 0 && Empresa.Length > 0)
                {
                    string qty = (string)HANAConnection.Excalar(string.Format("Select TO_CHAR(Count(*)) from {0}SB1LD_EPG_PRO{0}.{0}VwQQINV{0} b where {0}DocNum{0} = '{1}' and {0}Empresa{0} = '{2}'", (char)34, NumeroFact, Empresa));
                    if (!qty.Equals("1"))
                    {
                        lblResult.Text = "Por favor especifique el tipo de documento";
                        lblResult.ForeColor = Color.Red;
                        Registrar(Tipo);
                    }
                    else
                    {
                        Registrar(NumeroFact,Tipo,Empresa);
                    }
                }
                else {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Debe digitar el numero de documento y seleccionar la empresa');", true);
                }
            }
            catch (Exception ex) {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('"+ex.Message+"');", true);
            }
        }
        private void Registrar(string selectedValue)
        {
            string osql = "";
            try
            {
                object flag = HANAConnection.Excalar(string.Format("Select count(*) from {0}SB1LD_EPG_PRO{0}.{0}@LOG_COBRO{0} where {0}U_DocNum{0} ={1} and {0}U_Empresa{0}='{2}' and {0}U_Tipo{0}='{3}'", (char)34, txtNumFact.Text, ddlEmpresa.SelectedValue,selectedValue));
                if (flag.ToString().Equals("0"))
                {
                    decimal qq= (decimal)HANAConnection.Excalar(string.Format("Select (QQ) from {0}SB1LD_EPG_PRO{0}.{0}VwQQINV{0} where {0}DocNum{0}='{1}' and {0}Empresa{0}='{2}' and {0}Tipo{0}='{3}'", (char)34, txtNumFact.Text, ddlEmpresa.SelectedValue, selectedValue));
                   // HanaDecimal qq = (HanaDecimal)pQQ;
                    string Monto = (string)HANAConnection.Excalar(string.Format("Select to_Char({0}Total{0}) from {0}SB1LD_EPG_PRO{0}.{0}VwQQINV{0} where {0}DocNum{0}='{1}' and {0}Empresa{0}='{2}' and {0}Tipo{0}='{3}'", (char)34, txtNumFact.Text, ddlEmpresa.SelectedValue, selectedValue));
                    string EstadoPeriodo = (string)HANAConnection.Excalar(string.Format("Select ifnull(Max(ifnull({0}U_Estado{0},'N/A')),'N/A'){0}Estado{0} from {0}SB1LD_EPG_PRO{0}.{0}@LOG_COBRO{0} where {0}U_FechaInicio{0}='{1}' and {0}U_FechaFin{0}='{2}' and {0}U_Ruta{0}='{3}'", (char)34, Convert.ToDateTime(txtDesde.Text).ToString("yyyy-MM-dd"), Convert.ToDateTime(txtHasta.Text).ToString("yyyy-MM-dd"), ddlRuta.SelectedValue));
                    if (EstadoPeriodo.Equals("Cerrada"))
                    {

                        lblResult.Text = ("La ruta seleccionada ya se encuentra cerrada");
                        lblResult.ForeColor = Color.Red;
                        txtNumFact.Text = "";
                        txtNumFact.Focus();
                    }
                    else
                    {
                        if (qq.Equals(null))
                        {
                            lblResult.Text = "El numero de documento no existe para la empresa seleccionada";
                            lblResult.ForeColor = Color.Red;
                        }
                        if (!Monto.Equals("0.000000"))
                        {
                            string Vendedor = (string)HANAConnection.Excalar(string.Format("Select ({0}SlpName{0}) from {0}SB1LD_EPG_PRO{0}.{0}VwQQINV{0} where {0}DocNum{0}='{1}' and {0}Empresa{0}='{2}' and {0}Tipo{0}='{3}'", (char)34, txtNumFact.Text, ddlEmpresa.SelectedValue, selectedValue));
                            if (chkOmitir.Checked)
                            {
                                Vendedor = ddlRuta.SelectedValue;
                            }
                            if (Vendedor.Equals(ddlRuta.SelectedValue))
                            {
                                string fecha = DateTime.Now.ToString("yyyy/MM/dd");
                                string hora = DateTime.Now.ToString("HH:mm:ss");
                                object code = HANAConnection.Excalar(string.Format("Select count(*)+1 from {0}SB1LD_EPG_PRO{0}.{0}@LOG_COBRO{0}", (char)34));
                                string tramite = ",''";
                                if (chkTramite.Checked)
                                {
                                    tramite = ",'Trámite'";
                                }
                                osql = string.Format("insert into {0}SB1LD_EPG_PRO{0}.{0}@LOG_COBRO{0} values('{1}','{1}',{2},'{3}','{4}','{5}','{6}','','',0,0,'{7}','{8}','','','{9}','','','Abierta','{10}','','','0','{11}'" + tramite + ")", (char)34, code.ToString(), txtNumFact.Text, ddlRuta.SelectedValue, txtDesde.Text, txtHasta.Text, ddlEmpresa.SelectedValue, fecha, hora, txtAsignador.Text, Monto, selectedValue, tramite);
                                int i = HANAConnection.DML(osql);
                                if (i > 0)
                                {
                                    string osql1 = String.Format("Insert into Bitacora(Usuario,Accion,FechaInicio,FechaFin,Ruta,Code,Nombre,FechaRegistro,HoraRegistro,DocNum,Empresa,Asignador,Monto,Estado) values('{0}','{1}','{2}','{3}','{4}','{5}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','Abierto')", Session["user"].ToString(), "Nuevo Documento Insertado", txtDesde.Text, txtHasta.Text, ddlRuta.SelectedValue, code.ToString(), fecha, hora, txtNumFact.Text, ddlEmpresa.SelectedValue, txtAsignador.Text, Monto);
                                    ConexionSQL.DML(osql1);
                                    lblResult.Text = "La factura " + txtNumFact.Text + " ha sido registrada";
                                    lblResult.ForeColor = Color.Green;
                                    dtgFacturas.DataSource = Asignaciones();
                                    dtgFacturas.DataBind();
                                    grvPendientes.DataSource = Pendientes();
                                    grvPendientes.DataBind();
                                    txtNumFact.Text = "";
                                    txtNumFact.Focus();
                                    chkOmitir.Checked = false;
                                }
                            }
                            else
                            {
                                lblResult.Text = String.Format("La Ruta seleccionada ({0}) no corresponde a la especificada en el documento {1}", ddlRuta.SelectedValue, Vendedor);
                                lblResult.ForeColor = Color.Red;
                                ddlRuta.Focus();
                                ddlRuta.SelectedValue = Vendedor;
                            }
                        }
                        else
                        {
                            lblResult.Text = String.Format("La Factura {0} ya se encuentra cancelada", txtNumFact.Text);
                            lblResult.ForeColor = Color.Red;
                            txtNumFact.Text = "";
                            txtNumFact.Focus();
                        }
                    }
                }
                else
                {
                    /*lblResult.Text = "La factura " + txtNumFact.Text + " ya se encuentra registrada";
                    lblResult.ForeColor = Color.Blue;*/
                    mpePopUp.Show();
                    lblDesdeRA.Text = txtDesde.Text;
                    lblHastaRA.Text = txtHasta.Text;
                    lblRutaRA.Text = ddlRuta.Text;
                    lblNumFact.Text = txtNumFact.Text;
                    txtNumFact.Text = "";
                    txtNumFact.Focus();
                }
            }
            catch (Exception ex)
            {
                lblResult.Text = ex.Message;
                lblResult.ForeColor = Color.Red;
            }
        }

        protected void dtgFacturas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            dtgFacturas.PageIndex = e.NewPageIndex;
            dtgFacturas.DataSource = Asignaciones();
            dtgFacturas.DataBind();
        }
        protected void ActualizarCodigos()
        {
            try
            {
                StringBuilder osql = new StringBuilder();
                osql.Append(string.Format("Update b set b.{0}Code{0}=a.{0}Num{0}, b.{0}Name{0} = a.{0}Num{0} from (", (char)34));
                osql.Append(string.Format("Select {0}Code{0}, {0}Name{0}, ROW_NUMBER() over(order by {0}U_DocNum{0}) as {0}Num{0}, {0}U_DocNum{0}, {0}U_Empresa{0} ", (char)34));
                osql.Append(string.Format("from {0}SB1LD_EPG_PRO{0}.{0}@LOG_COBRO{0} a ", (char)34));
                osql.Append(string.Format(")a inner join {0}SB1LD_EPG_PRO{0}.{0}@LOG_COBRO{0} b on a.{0}U_DocNum{0} = b.{0}U_DocNum{0} and a.{0}U_Empresa{0} = b.{0}U_Empresa{0}", (char)34));
                int i = HANAConnection.DML(osql.ToString());
                if (i >= 0)
                {
                    lblResult.Text = "Códigos actualizados correctamente";
                    lblResult.ForeColor = Color.Green;
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
            }
        }
        protected void dtgFacturas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string comando = e.CommandName;
                if (!comando.Equals("Page"))
                {
                    int index = Convert.ToInt32(e.CommandArgument);
                    GridViewRow row = dtgFacturas.Rows[index];
                    string filename = Server.HtmlDecode(row.Cells[0].Text);
                    string tipo = Server.HtmlDecode(row.Cells[1].Text);
                    string Empresa = Server.HtmlDecode(row.Cells[7].Text);
                    lblResult.Text = filename;
                    string EstadoDoc = HANAConnection.DQL(string.Format("Select Max(ifnull({0}U_Estado{0},'N/A')){0}Estado{0} from {0}SB1LD_EPG_PRO{0}.{0}@LOG_COBRO{0} where {0}U_DocNum{0}='{1}' and {0}U_Empresa{0}='{2}' and {0}U_Tipo{0}='{3}'", (char)34, filename, Empresa, tipo)).Rows[0][0].ToString();
                    if (EstadoDoc.Equals("Cerrada"))
                    {
                        lblResult.Text = "No puede eliminar un documento de una ruta y periodo que se encuentra cerrada";
                        lblResult.ForeColor = Color.Red;
                    }
                    else
                    {
                        
                        Liquidacion liquidacion = new Liquidacion(filename,Empresa,tipo);
                        int i = HANAConnection.DML(string.Format("delete from {0}SB1LD_EPG_PRO{0}.{0}@LOG_COBRO{0} where {0}U_DocNum{0} = '{1}' and {0}U_Empresa{0}='{2}' and {0}U_Estado{0} not in ('Cerrado') and {0}U_Tipo{0}='{3}'", (char)34, filename, Empresa, tipo));
                        if (i > 0)
                        {
                        string osql1 = String.Format("set dateformat dmy;Insert into Bitacora(Usuario,Accion,FechaInicio,FechaFin,Ruta,Code,Nombre,FechaRegistro,HoraRegistro,DocNum,Empresa,Asignador,TipoDocumento,TipoPago,ReciboNum,MontoRecibo,Liquidador,FechaLiquidacion,HoraLiquidacion,FechaRecibo,Estado,Monto) values('{0}','{1}','{2}','{3}','{4}','{5}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}')", Session["user"].ToString(), "Eliminar Asignacion", liquidacion.FechaInicio1, liquidacion.FechaFin1, liquidacion.Ruta1, liquidacion.Codigo1, liquidacion.FechaRegistro.Substring(0,10), liquidacion.HoraRegistro, liquidacion.DocNum1, liquidacion.Empresa1, liquidacion.Asignador, liquidacion.TipoDocumento, liquidacion.TipoPago, liquidacion.NumRecibo1, liquidacion.MontoRecibo, liquidacion.Liquidador, liquidacion.FechaLiquidacion, liquidacion.HoraLiquidacion, liquidacion.FechaLiquidacion, liquidacion.EstadoAsignacion, liquidacion.Monto);
                        ConexionSQL.DML(osql1);
                            lblResult.Text = "La factura " + filename + " ha sido eliminada";
                            dtgFacturas.DataSource = Asignaciones();
                            dtgFacturas.DataBind();
                            ActualizarCodigos();
                            lblResult.ForeColor = Color.Green;
                            txtBuscar.Text = "";
                            txtBuscar.Focus();
                            grvPendientes.DataSource = Pendientes();
                            grvPendientes.DataBind();
                        }
                        else
                        {
                            lblResult.Text = "Ha ocurrido un error al eliminar la factura " + filename;
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

        protected void imgFind_Click(object sender, ImageClickEventArgs e)
        {

            dtgFacturas.DataSource = Asignaciones();
            dtgFacturas.DataBind();
            grvPendientes.DataSource = Pendientes();
            grvPendientes.DataBind();
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                String osql = string.Format("Select {0}U_Ruta{0},{0}Tipo{0},TO_CHAR({0}DocNum{0}){0}DocNum{0}, {0}CardName{0}, {0}Total{0}, TO_CHAR({0}U_FechaInicio{0}, 'dd/MM/yyyy'){0}Desde{0}, TO_CHAR({0}U_FechaFin{0}, 'dd/MM/yyyy'){0}Hasta{0}, {0}Empresa{0} from {0}SB1LD_EPG_PRO{0}.{0}@LOG_COBRO{0} a inner join {0}SB1LD_EPG_PRO{0}.{0}VwQQINV{0} b on a.{0}U_DocNum{0} = b.{0}DocNum{0} and a.{0}U_Empresa{0} = b.{0}Empresa{0}  and {0}U_Tipo{0}={0}Tipo{0} where {0}U_DocNum{0}='{1}'  order by {0}U_FechaRegistro{0} desc,{0}U_HoraRegistro{0} desc", (char)34, txtBuscar.Text);
                DataTable dt=HANAConnection.DQL(osql);
                dtgFacturas.DataSource = dt;
                dtgFacturas.DataBind();
            }
            catch (Exception ex)
            {
                lblResult.Text = ex.Message;
                lblResult.ForeColor = Color.Red;
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                String osql = string.Format("Select {0}U_Ruta{0},{0}Tipo{0},TO_CHAR({0}DocNum{0}){0}DocNum{0}, {0}CardName{0}, {0}Total{0}, TO_CHAR({0}U_FechaInicio{0}, 'dd/MM/yyyy'){0}Desde{0}, TO_CHAR({0}U_FechaFin{0}, 'dd/MM/yyyy'){0}Hasta{0}, {0}Empresa{0} from {0}SB1LD_EPG_PRO{0}.{0}@LOG_COBRO{0} a inner join {0}SB1LD_EPG_PRO{0}.{0}VwQQINV{0} b on a.{0}U_DocNum{0} = b.{0}DocNum{0} and a.{0}U_Empresa{0} = b.{0}Empresa{0}  and {0}U_Tipo{0}={0}Tipo{0} where {0}U_DocNum{0}='{1}'  order by {0}U_FechaRegistro{0} desc,{0}U_HoraRegistro{0} desc", (char)34, txtBuscar.Text);
                DataTable dt=HANAConnection.DQL(osql);
                dtgFacturas.DataSource = dt;
                dtgFacturas.DataBind();
                grvPendientes.DataSource = Pendientes();
                grvPendientes.DataBind();
            }
            catch (Exception ex)
            {
                lblResult.Text = ex.Message;
                lblResult.ForeColor = Color.Red;
            }
        }

        protected void grvPendientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            grvPendientes.PageIndex = e.NewPageIndex;
            grvPendientes.DataSource = Pendientes();
            grvPendientes.DataBind();

        }

        protected void grvPendientes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string comando = e.CommandName;
                if (!comando.Equals("Page"))
                {
                    int index = Convert.ToInt32(e.CommandArgument);
                    GridViewRow row = grvPendientes.Rows[index];
                    string filename = Server.HtmlDecode(row.Cells[4].Text);
                    txtNumFact.Text = filename;
                    txtNumFact.Focus();
                    ddlEmpresa.SelectedValue = Server.HtmlDecode(row.Cells[1].Text);
                    ddlRuta.SelectedValue = Server.HtmlDecode(row.Cells[2].Text);
                    ddlTipo.SelectedValue = Server.HtmlDecode(row.Cells[3].Text);
                }
            }
            catch (Exception ex)
            {
                lblResult.Text = ex.Message;
                lblResult.ForeColor = Color.Red;
            }

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
        private DataTable Pendientes()
        {
            String osql = string.Format("Select {0}CardName{0},{0}DocNum{0},{0}DocDate{0},{0}Total{0},{0}Tipo{0},TO_CHAR({0}DocDueDate{0},'dd/MM/yyyy'){0}Vencimiento{0},Days_Between({0}DocDueDate{0},'{1}'){0}DiasVencido{0},{0}Empresa{0},{0}SlpName{0} from {0}SB1LD_EPG_PRO{0}.{0}VwQQINV{0} where {0}Total{0}>0 and Days_Between({0}DocDueDate{0},'{1}')>0 and {0}DocStatus{0}='O' and {0}DocNum{0} not in (Select TO_CHAR({0}U_DocNum{0}) from {0}SB1LD_EPG_PRO{0}.{0}@LOG_COBRO{0}) and {0}Grupo{0} not in ('Cliente Incobrable') and {0}SlpName{0} not in ('OFICINA') and {0}SlpName{0} in ('{2}')  order by {0}DiasVencido{0} desc", (char)34, txtHasta.Text,ddlRuta.SelectedValue);
            DataTable dt = HANAConnection.DQL(osql);
            return dt;
        }

        protected void xptExcel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridView data = new GridView();
                data.DataSource = Pendientes();
                data.DataBind();
                if (data.Rows.Count > 0)
                {
                    ExportToExcel("Pendientes al " + Convert.ToDateTime(txtHasta.Text).ToString("dd-MM-yyyy"), data);
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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            mpePopUp.Hide();
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            string osql = "";
            try {
                decimal qq = (decimal)HANAConnection.Excalar(string.Format("Select QQ from {0}SB1LD_EPG_PRO{0}.{0}VwQQINV{0} where {0}DocNum{0}='{1}' and {0}Empresa{0}='{2}'", (char)34, lblNumFact.Text, ddlEmpresa.SelectedValue));
                string Monto = (string)HANAConnection.Excalar(string.Format("Select to_Char({0}Total{0}) from {0}SB1LD_EPG_PRO{0}.{0}VwQQINV{0} where {0}DocNum{0}='{1}' and {0}Empresa{0}='{2}'", (char)34, lblNumFact.Text, ddlEmpresa.SelectedValue));
                string EstadoPeriodo = (string)HANAConnection.Excalar(string.Format("Select ifnull(Max(ifnull({0}U_Estado{0},'N/A')),'N/A'){0}Estado{0} from {0}SB1LD_EPG_PRO{0}.{0}@LOG_COBRO{0} where {0}U_FechaInicio{0}='{1}' and {0}U_FechaFin{0}='{2}' and {0}U_Ruta{0}='{3}'", (char)34, Convert.ToDateTime(txtDesde.Text).ToString("yyyy-MM-dd"), Convert.ToDateTime(txtHasta.Text).ToString("yyyy-MM-dd"), ddlRuta.SelectedValue));
                    if (qq.Equals(null))
                    {
                        lblResult.Text = "El numero de factura no existe para la empresa seleccionada";
                        lblResult.ForeColor = Color.Red;
                    }
                    if (!Monto.Equals("0.000000"))
                    {
                        string Vendedor = (string)HANAConnection.Excalar(string.Format("Select ({0}SlpName{0}) from {0}SB1LD_EPG_PRO{0}.{0}VwQQINV{0} where {0}DocNum{0}='{1}' and {0}Empresa{0}='{2}'", (char)34, lblNumFact.Text, ddlEmpresa.SelectedValue));
                        if (chkOmitir.Checked)
                        {
                            Vendedor = ddlRuta.SelectedValue;
                        }
                        if (Vendedor.Equals(ddlRuta.SelectedValue))
                        {
                            string fecha = DateTime.Now.ToString("yyyy/MM/dd");
                            string hora = DateTime.Now.ToString("HH:mm:ss");
                            object code = HANAConnection.Excalar(string.Format("Select count(*)+1 from {0}SB1LD_EPG_PRO{0}.{0}@LOG_COBRO{0}", (char)34));
                            //osql = string.Format("insert into {0}SB1LD_EPG_PRO{0}.{0}@LOG_COBRO{0} values('{1}','{1}',{2},'{3}','{4}','{5}','{6}','','',0,0,'{7}','{8}','','','{9}','','','Abierta','{10}','','')", (char)34, code.ToString(), txtNumFact.Text, ddlRuta.SelectedValue, txtDesde.Text, txtHasta.Text, ddlEmpresa.SelectedValue, fecha, hora, txtAsignador.Text, Monto);
                            StringBuilder osql2 = new StringBuilder();
                            osql2.Append("Update a "); 
                            osql2.Append("set  ");
                            osql2.Append(string.Format("{0}U_Ruta{0}='{1}', ",(char)34,Vendedor));
                            osql2.Append(string.Format("{0}U_FechaInicio{0}='{1}', ",(char)34,txtDesde.Text));
                            osql2.Append(string.Format("{0}U_FechaFin{0}='{1}', ", (char)34,txtHasta.Text));
                            osql2.Append(string.Format("{0}U_Empresa{0}='{1}', ", (char)34,ddlEmpresa.SelectedValue));
                            osql2.Append(string.Format("{0}U_TipoDocumento{0}='{1}', ", (char)34,""));
                            osql2.Append(string.Format("{0}U_TipoPago{0}='{1}', ", (char)34,""));
                            osql2.Append(string.Format("{0}U_ReciboNum{0}='{1}', ", (char)34,"0"));
                            osql2.Append(string.Format("{0}U_MontoRecibo{0}='{1}', ", (char)34,"0"));
                            osql2.Append(string.Format("{0}U_FechaRegistro{0}='{1}', ", (char)34,fecha));
                            osql2.Append(string.Format("{0}U_HoraRegistro{0}='{1}', ", (char)34,hora));
                            osql2.Append(string.Format("{0}U_FechaLiquidacion{0}='{1}', ", (char)34,""));
                            osql2.Append(string.Format("{0}U_HoraLiquidacion{0}='{1}', ", (char)34,""));
                            osql2.Append(string.Format("{0}U_Asignador{0}='{1}', ", (char)34,txtAsignador.Text));
                            osql2.Append(string.Format("{0}U_Liquidador{0}='{1}', ", (char)34,""));
                            osql2.Append(string.Format("{0}U_FechaRecibo{0}='{1}', ", (char)34,""));
                            osql2.Append(string.Format("{0}U_Estado{0}='{1}', ", (char)34,"Abierta"));
                            osql2.Append(string.Format("{0}U_MontoDoc{0}='{1}', ", (char)34,Monto));
                            osql2.Append(string.Format("{0}U_Original{0}='{1}', ", (char)34,"0"));
                            osql2.Append(string.Format("{0}U_Tramite{0}= '{1}' ", (char)34,""));
                            osql2.Append(string.Format("from {0}SB1LD_EPG_PRO{0}.{0}@LOG_COBRO{0} a  ", (char)34));
                            osql2.Append(string.Format("where {0}U_DocNum{0}= '{1}'  ", (char)34,lblNumFact.Text));
                            osql2.Append(string.Format("and {0}U_Empresa{0}= '{1}' ", (char)34,ddlEmpresa.SelectedValue));
                            int i = HANAConnection.DML(osql2.ToString());
                            if (i > 0)
                            {
                                string osql1 = String.Format("Insert into Bitacora(Usuario,Accion,FechaInicio,FechaFin,Ruta,Code,Nombre,FechaRegistro,HoraRegistro,DocNum,Empresa,Asignador,Monto,Estado) values('{0}','{1}','{2}','{3}','{4}','{5}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','Abierto')", Session["user"].ToString(), "Re-asignación de documento", txtDesde.Text, txtHasta.Text, ddlRuta.SelectedValue, code.ToString(), fecha, hora, lblNumFact.Text, ddlEmpresa.SelectedValue, txtAsignador.Text, Monto);
                                ConexionSQL.DML(osql1);
                                lblResult.Text = "La factura " + lblNumFact.Text + " ha sido re-asignada";
                                lblResult.ForeColor = Color.Green;
                                dtgFacturas.DataSource = Asignaciones();
                                dtgFacturas.DataBind();
                                grvPendientes.DataSource = Pendientes();
                                grvPendientes.DataBind();
                                txtNumFact.Text = "";
                                txtNumFact.Focus();
                                chkOmitir.Checked = false;
                            }
                        }
                        else
                        {
                            lblResult.Text = String.Format("La Ruta seleccionada ({0}) no corresponde a la especificada en el documento {1}", ddlRuta.SelectedValue, Vendedor);
                            lblResult.ForeColor = Color.Red;
                            ddlRuta.Focus();
                            ddlRuta.SelectedValue = Vendedor;
                            txtNumFact.Text = lblNumFact.Text;
                        }
                    }
                    else
                    {
                        lblResult.Text = String.Format("La Factura {0} ya se encuentra cancelada", lblNumFact.Text);
                        lblResult.ForeColor = Color.Red;
                        txtNumFact.Text = "";
                        txtNumFact.Focus();
                    }
                
            } catch (Exception ex)
            {
                lblResult.Text = ex.Message;
                lblResult.ForeColor = Color.Red;
                txtNumFact.Text = "";
                txtNumFact.Focus();
            }

        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            ActualizarCodigos();
        }

        protected void chkTramite_CheckedChanged(object sender, EventArgs e)
        {
        }

        protected void chkBoxAutomatico_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBoxAutomatico.Checked)
            {
                PnlManual.Visible = false;
                PnlAutomatico.Visible = true;
                btnAsignar.Visible = false;
            }
            else
            {
                PnlManual.Visible = true;
                PnlAutomatico.Visible = false;
                btnAsignar.Visible = true;
            }

        }

        protected void txtAutomatico_TextChanged(object sender, EventArgs e)
        {
            btnAsignar_Click(sender, e);
            txtAutomatico.Text = "";
            txtAutomatico.Focus();
        }
    }
}