<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="Devoluciones.aspx.cs" Inherits="GestionCobro.Devoluciones" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style>
        .pagination-ys {
    /*display: inline-block;*/
    padding-left: 0;
    margin: 20px 0;
    border-radius: 4px;
}

.pagination-ys table > tbody > tr > td {
    display: inline;
}

.pagination-ys table > tbody > tr > td > a,
.pagination-ys table > tbody > tr > td > span {
    position: relative;
    float: left;
    padding: 8px 12px;
    line-height: 1.42857143;
    text-decoration: none;
    color: #dd4814;
    background-color: #ffffff;
    border: 1px solid #dddddd;
    margin-left: -1px;
}

.pagination-ys table > tbody > tr > td > span {
    position: relative;
    float: left;
    padding: 8px 12px;
    line-height: 1.42857143;
    text-decoration: none;    
    margin-left: -1px;
    z-index: 2;
    color: #aea79f;
    background-color: #f5f5f5;
    border-color: #dddddd;
    cursor: default;
}

.pagination-ys table > tbody > tr > td:first-child > a,
.pagination-ys table > tbody > tr > td:first-child > span {
    margin-left: 0;
    border-bottom-left-radius: 4px;
    border-top-left-radius: 4px;
}

.pagination-ys table > tbody > tr > td:last-child > a,
.pagination-ys table > tbody > tr > td:last-child > span {
    border-bottom-right-radius: 4px;
    border-top-right-radius: 4px;
}

.pagination-ys table > tbody > tr > td > a:hover,
.pagination-ys table > tbody > tr > td > span:hover,
.pagination-ys table > tbody > tr > td > a:focus,
.pagination-ys table > tbody > tr > td > span:focus {
    color: #97310e;
    background-color: #eeeeee;
    border-color: #dddddd;
}
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder4" runat="server">
    <li><i class="fa fa-home"></i><a href="default.aspx">Inicio</a></li>
              <li><i class="fa fa-bell"></i>Devoluciones</li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <div class="form-inline">
                <div class="form-group">
                <asp:TextBox runat="server" ID="txtFecha" AutoPostBack="true" OnTextChanged="txtFecha_TextChanged" TextMode="Date" placeholder="Fecha" CssClass="form-control bg-warning"/>
                    </div>
                <div class="form-group">
                <asp:DropDownList runat="server" ID="ddlRutas" CssClass="form-control bg-info"/>
                    </div>
                <div class="form-group">
                <asp:TextBox runat="server" AutoPostBack="true"  OnTextChanged="txtEscanear_TextChanged" ID="txtEscanear" placeholder="Escanear" CssClass="form-control bg-primary"/>
                    </div>
                </div>
            </div>
            </div>
        <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <asp:GridView ID="grvDevolucionesEscaneadas" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="20"  CssClass="table table-striped table-advance table-hover" ForeColor="#333333" GridLines="None" Font-Size="Smaller" Width="100%" OnRowCommand="grvDevolucionesEscaneadas_RowCommand" DataKeyNames="U_NumAtCard">
                <Columns>
                            <asp:BoundField DataField="NoSeq" HeaderText="NoSeq" />
                            <asp:BoundField DataField="U_Empresa" HeaderText="Empresa" />
                            <asp:BoundField DataField="U_NumAtCard" HeaderText="Referencia" />
                            <asp:BoundField DataField="U_CardCode" HeaderText="Código Cliente" />
                            <asp:BoundField DataField="U_CardName" HeaderText="Nombre Cliente" />
                            <asp:BoundField DataField="U_Cantidad" HeaderText="Cantidad" DataFormatString="{0:F2}" />
                            <asp:BoundField DataField="U_Ruta" HeaderText="Ruta" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton CommandName="Eliminar" CommandArgument='<%# Eval("U_NumAtCard") %>' OnClick="imgbtn_Click" Width="35px" ID="imgbtn" runat="server" ImageUrl="~/img/delete.png"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                 </Columns>
            </asp:GridView>
            </div>
            </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
