using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestionCobro
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["User"] != null)
                { string user = Session["user"].ToString();
                    if (user.Equals("Depto TI | Empagro"))
                    {
                        Bitacora.Visible = true;

                    }
                    else { Bitacora.Visible = false; }
                }
                else
                {
                    Response.Redirect("login.aspx");
                }
            }
        }
        protected void LinkButton_Command(object sender, CommandEventArgs e)
        {

        }


    }
}