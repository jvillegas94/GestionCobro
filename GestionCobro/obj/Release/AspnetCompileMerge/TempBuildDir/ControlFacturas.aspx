<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="ControlFacturas.aspx.cs" Inherits="GestionCobro.ControlFacturas" %>
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
              <li><i class="fa fa-exclamation-circle"></i>Control Factura</li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:Timer ID="Timer1" runat="server" Enabled="true" OnTick="Timer1_Tick" Interval="300000"></asp:Timer>
    <div class="form-inline">
            <asp:Button runat="server" ID="btnGuardar" OnClick="btnGuardar_Click" CssClass="btn btn-sm btn-default" Text="Exportar" Editable="false"/>
        <asp:TextBox runat="server" ID="txtFecha" Enabled="false" TextMode="Date" CssClass="form-control bg-warning sm-input" AutoPostBack="true" OnTextChanged="txtFecha_TextChanged"/>

    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
   <contenttemplate>
        <div class="row">
        <div class="col-lg-6 col-md-6 col-sm-6">
            <div class="form-inline">
            <asp:CheckBox ID="chkManual" runat="server" Text="Manual" Checked="false" />  
            <asp:TextBox ID="txtBuscar" runat="server" AutoPostBack="true" OnTextChanged="txtBuscar_TextChanged" CssClass=" form-control" Width="180px" autofocus="true"></asp:TextBox>
                </div>
            <asp:GridView ID="FacturasPendiente" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="20" OnPageIndexChanging="FacturasPendiente_PageIndexChanging" CssClass="table table-striped table-advance table-hover" ForeColor="#333333" GridLines="None" Font-Size="Smaller" Width="100%">
                <PagerStyle CssClass="pagination-ys" />
                <Columns>
        <asp:BoundField DataField="BD" HeaderText="Empresa" />
        <asp:BoundField DataField="NoFact" HeaderText="N° Fact" />
        <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
        <asp:BoundField DataField="CardCode" HeaderText="Código" />
        <asp:BoundField DataField="CardName" HeaderText="Nombre" />
        <asp:BoundField DataField="DocTotal" HeaderText="Total" DataFormatString="{0:C2}" />
    </Columns>
            </asp:GridView>
            </div>
        <div class="col-lg-6 col-md-6 col-sm-6">
            <asp:GridView ID="grvProcesadas" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="20" OnPageIndexChanging="grvProcesadas_PageIndexChanging" CssClass="table table-striped table-advance table-hover" ForeColor="#333333" GridLines="None" Font-Size="Smaller" Width="100%">
                <PagerStyle CssClass="pagination-ys" />
                <Columns>
        <asp:BoundField DataField="BD" HeaderText="Empresa" />
        <asp:BoundField DataField="NoFact" HeaderText="N° Fact" />
        <asp:BoundField DataField="Socio" HeaderText="Nombre" />
        <asp:BoundField DataField="Monto" HeaderText="Total" DataFormatString="{0:C2}" />
    </Columns>
            </asp:GridView>
            </div>
            </div>
       </contenttemplate>
        
   <triggers>
      <asp:asyncpostbacktrigger controlid="txtBuscar" eventname="TextChanged"></asp:asyncpostbacktrigger>
      <asp:asyncpostbacktrigger controlid="Timer1" eventname="Tick"></asp:asyncpostbacktrigger>
   </triggers>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
