using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestionCobro
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensaje.Text = HANAConnection.Instance.GetConnection().State.ToString();
            }
            catch (Exception ex) {
                lblMensaje.Text = ex.Message;
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                object flag =HANAConnection.Excalar(string.Format("Select count(*) from {0}SB1LD_EPG_PRO{0}.{0}OUSR{0} where {0}USER_CODE{0}='{1}' and {0}U_MBC_TRANSF{0}='{2}'", (char)34, txtUsuario.Text, txtClave.Text));
                if (flag.ToString().Equals("1"))
                {
                    if (txtUsuario.Text.Equals("manager"))
                    {
                        string ousr = (string)HANAConnection.Excalar(string.Format("Select {0}firstName{0}||' '||ifnull({0}middleName{0},'')|| ' '||{0}lastName{0} from {0}SB1LD_EPG_PRO{0}.{0}OUSR{0}  T0  INNER JOIN {0}SB1LD_EPG_PRO{0}.{0}OHEM{0} T1 ON T0.{0}USERID{0} = T1.{0}userId{0} where USER_CODE='{1}'", (char)34, txtUsuario.Text, txtClave.Text));
                        Session["user"] = ousr ;
                        string dpto = (string)HANAConnection.Excalar(string.Format("Select ifnull(T2.{0}Name{0},'') from {0}SB1LD_EPG_PRO{0}.{0}OUSR{0}  T0  INNER JOIN {0}SB1LD_EPG_PRO{0}.{0}OHEM{0} T1 ON T0.{0}USERID{0} = T1.{0}userId{0} inner join {0}SB1LD_EPG_PRO{0}.{0}OUDP{0} T2 on  T1.{0}dept{0}=T2.{0}Code{0}  where USER_CODE='{1}'", (char)34, txtUsuario.Text, txtClave.Text));
                        Session["dpto"] = dpto;
                    }
                    else
                    {
                        string ousr = (string)HANAConnection.Excalar(string.Format("Select {0}firstName{0}||' '||ifnull({0}middleName{0},'')|| ' '||{0}lastName{0} from {0}SB1LD_EPG_PRO{0}.{0}OUSR{0}  T0  INNER JOIN {0}SB1LD_EPG_PRO{0}.{0}OHEM{0} T1 ON T0.{0}USERID{0} = T1.{0}userId{0} where USER_CODE='{1}'", (char)34, txtUsuario.Text, txtClave.Text));
                        string dpto = (string)HANAConnection.Excalar(string.Format("Select ifnull(T2.{0}Name{0},'') from {0}SB1LD_EPG_PRO{0}.{0}OUSR{0}  T0  INNER JOIN {0}SB1LD_EPG_PRO{0}.{0}OHEM{0} T1 ON T0.{0}USERID{0} = T1.{0}userId{0} inner join {0}SB1LD_EPG_PRO{0}.{0}OUDP{0} T2 on  T1.{0}dept{0}=T2.{0}Code{0}  where USER_CODE='{1}'", (char)34, txtUsuario.Text, txtClave.Text));
                        Session["user"] = ousr;
                    Session["dpto"] = dpto;
                    }
                    string osql = String.Format("Insert into Bitacora(Usuario,Accion) values('{0}','{1}')",Session["user"].ToString(),"Inicio de Sesión");
                    ConexionSQL.DML(osql);
                    Response.Redirect("default.aspx");

                }
                else
                {
                    lblMensaje.Text = "El usuario o clave digitados no corresponde.";
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = ex.Message;
            }
        }
    }
}