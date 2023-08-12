<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="ControlFacturas.aspx.cs" Inherits="GestionCobro.ControlFacturas" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
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
    
  <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/css/bootstrap.min.css">
  <script src="https://cdn.jsdelivr.net/npm/jquery@3.6.4/dist/jquery.slim.min.js"></script>
  <script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js"></script>
  <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.bundle.min.js"></script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder4" runat="server">
    <li><i class="fa fa-home"></i><a href="default.aspx">Inicio</a></li>
              <li><i class="fa fa-exclamation-circle"></i>Control Factura(<asp:Label ID="Label1" runat="server" Text=""></asp:Label>)</li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
   <div class="row">
  <div class="col-sm-4 col-lg-4">
      <div class="card">
  <div class="card-header  bg-primary text-white">Despacho
  </div>
  <div class="card-body">
      <asp:Panel runat="server" ScrollBars="Horizontal" Width="100%">
      <asp:GridView style="font-size:10px" ID="FacturasPendiente" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="10" OnPageIndexChanging="FacturasPendiente_PageIndexChanging" CssClass="table table-striped table-advance table-hover" ForeColor="#333333" GridLines="None" Font-Size="Smaller" Width="100%">
                <PagerStyle CssClass="pagination-ys" />
                <Columns>
        <asp:BoundField DataField="BD" HeaderText="Empresa" />
        <asp:BoundField DataField="NoFact" HeaderText="N° Fact" />
        <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
        <asp:BoundField DataField="CardCode" HeaderText="Código" />
        <asp:BoundField DataField="CardName" HeaderText="Nombre" />
        <asp:BoundField DataField="DocTotal" HeaderText="Total" DataFormatString="{0:C2}" />
    </Columns>
            </asp:GridView></asp:Panel>
  </div>
  <div class="card-footer">Total: </div>
</div>
  </div>
  <div class="col-sm-4 col-lg-4">
      <div class="card">
  <div class="card-header bg-secondary text-white">Facturación<br/>
      <div class="form-inline">
          <div class="form-group">
    <asp:CheckBox AutoPostBack="true" OnCheckedChanged="ValidarCheck" ToolTip="fact" CssClass="form-check-input" ID="chkNoDelDiaFact" runat="server" />
    <asp:TextBox CssClass="form-control" ID="txtEscanear" runat="server" OnTextChanged="txtEscanear_TextChanged" AutoPostBack="true"></asp:TextBox>
</div>

          </div>
      </div>
  <div class="card-body"><asp:Panel runat="server" ScrollBars="Horizontal" Width="100%">
      <asp:GridView style="font-size:10px" ID="grvFacturacion" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="10" OnPageIndexChanging="grvFacturacion_PageIndexChanging" CssClass="table table-striped table-advance table-hover" ForeColor="#333333" GridLines="None" Font-Size="Smaller" Width="100%">
                <PagerStyle CssClass="pagination-ys" />
                <Columns>
        <asp:BoundField DataField="Empresa" HeaderText="Empresa" />
        <asp:BoundField DataField="NoDocumento" HeaderText="N° Fact" />
        <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
        <asp:BoundField DataField="CardCode" HeaderText="Código" />
        <asp:BoundField DataField="CardName" HeaderText="Nombre" />
        <asp:BoundField DataField="Total" HeaderText="Total" DataFormatString="{0:C2}" />
    </Columns>
            </asp:GridView></asp:Panel></div>
  <div class="card-footer">Total:</div>
</div>
      </div>
  <div runat="server" id="CxC" class="col-sm-4 col-lg-4">
      <div class="card">
  <div class="card-header  bg-success text-white">Cuentas por Cobrar<br/>
      <div class="form-inline">
          <div class="form-group">
    <div class="form-check">
        <asp:CheckBox AutoPostBack="true" OnCheckedChanged="ValidarCheck" ToolTip="cxc" CssClass="form-check-input" ID="chkNoDelDiaCxC" runat="server" />
    </div>
    <asp:TextBox CssClass="form-control" ID="txtEscanearCxC" runat="server"></asp:TextBox>
</div>

          </div></div>
  <div class="card-body"></div>
  <div class="card-footer">Total</div>
</div>
  </div>
</div>
</asp:Content>
<asp:Content ID="Content11" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
