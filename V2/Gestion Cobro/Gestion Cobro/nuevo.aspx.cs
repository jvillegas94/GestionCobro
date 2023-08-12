using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Cobro
{
    public partial class nuevo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) {
                Rutas();
            }
        }
        protected void Rutas() {
            HANAConnection conn = new HANAConnection();

            ddlRuta.DataSource=conn.DQL(String.Format("Select {0}SlpName{0} from {0}SB1LD_EPG_PRO{0}.OSLP where {0}SlpName{0} like 'RUTA%' union all Select {0}SlpName{0} from {0}SB1LD_BET_PRO{0}.OSLP where {0}SlpName{0} like 'RUTA%' ", (char)34));
            ddlRuta.DataTextField = "SlpName";
            ddlRuta.DataValueField = "SlpName";
            ddlRuta.DataBind();

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            Pendientes();
        }
        protected void Pendientes()
        {
            HANAConnection conn = new HANAConnection();
            String osql = string.Format("Select {0}CardName{0},{0}DocNum{0},{0}DocDate{0},{0}Total{0},{0}Tipo{0},TO_CHAR({0}DocDueDate{0},'dd/MM/yyyy'){0}Vencimiento{0},Days_Between({0}DocDueDate{0},'{1}'){0}DiasVencido{0},{0}Empresa{0},{0}SlpName{0} from {0}SB1LD_EPG_PRO{0}.{0}VwQQINV{0} where {0}Total{0}>0 and Days_Between({0}DocDueDate{0},'{1}')>0 and {0}DocStatus{0}='O' and {0}DocNum{0} not in (Select TO_CHAR({0}U_DocNum{0}) from {0}SB1LD_EPG_PRO{0}.{0}@LOG_COBRO{0}) and {0}Grupo{0} not in ('Cliente Incobrable') and {0}SlpName{0} not in ('OFICINA') and {0}SlpName{0} in ('{2}')  order by {0}DiasVencido{0} desc", (char)34, txtFechaFin.Text, ddlRuta.SelectedValue);
            grvPendientes.DataSource = conn.DQL(osql);
            grvPendientes.DataBind();
        }
        protected void Pendientes(string No)
        {
            HANAConnection conn = new HANAConnection();
            String osql = string.Format("Select {0}CardName{0},{0}DocNum{0},{0}DocDate{0},{0}Total{0},{0}Tipo{0},TO_CHAR({0}DocDueDate{0},'dd/MM/yyyy'){0}Vencimiento{0},Days_Between({0}DocDueDate{0},'{1}'){0}DiasVencido{0},{0}Empresa{0},{0}SlpName{0} from {0}SB1LD_EPG_PRO{0}.{0}VwQQINV{0} where {0}Total{0}>0 and Days_Between({0}DocDueDate{0},'{1}')>0 and {0}DocStatus{0}='O' and {0}DocNum{0} not in (Select TO_CHAR({0}U_DocNum{0}) from {0}SB1LD_EPG_PRO{0}.{0}@LOG_COBRO{0}) and {0}Grupo{0} not in ('Cliente Incobrable') and {0}SlpName{0} not in ('OFICINA') and {0}SlpName{0} in ('{2}') and {0}DocNum{0} like '%{3}%'  order by {0}DiasVencido{0} desc", (char)34, txtFechaFin.Text, ddlRuta.SelectedValue,txtBuscarPendientes.Text);
            grvPendientes.DataSource = conn.DQL(osql);
            grvPendientes.DataBind();
        }

        protected void grvPendientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvPendientes.PageIndex = e.NewPageIndex;
            Pendientes();
        }

        protected void grvPendientes_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {

        }

        protected void txtBuscarPendientes_TextChanged(object sender, EventArgs e)
        {
            Pendientes(txtBuscarPendientes.Text);
        }
    }
}