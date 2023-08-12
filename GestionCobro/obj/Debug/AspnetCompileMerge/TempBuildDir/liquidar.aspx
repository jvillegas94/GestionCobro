<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="liquidar.aspx.cs" Inherits="GestionCobro.liquidar" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style>
    .invoice-box {
        max-width: 800px;
        margin: auto;
        padding: 30px;
        border: 1px solid #eee;
        box-shadow: 0 0 10px rgba(0, 0, 0, .15);
        font-size: 16px;
        line-height: 24px;
        font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif;
        color: #555;
    }
    
    .invoice-box table {
        width: 100%;
        line-height: inherit;
        text-align: left;
    }
    
    .invoice-box table td {
        padding: 5px;
        vertical-align: top;
    }
    
    .invoice-box table tr td:nth-child(2) {
        text-align: right;
    }
    
    .invoice-box table tr.top table td {
        padding-bottom: 20px;
    }
    
    .invoice-box table tr.top table td.title {
        font-size: 45px;
        line-height: 45px;
        color: #333;
    }
    
    .invoice-box table tr.information table td {
        padding-bottom: 40px;
    }
    
    .invoice-box table tr.heading td {
        background: #eee;
        border-bottom: 1px solid #ddd;
        font-weight: bold;
    }
    
    .invoice-box table tr.details td {
        padding-bottom: 20px;
    }
    
    .invoice-box table tr.item td{
        border-bottom: 1px solid #eee;
    }
    
    .invoice-box table tr.item.last td {
        border-bottom: none;
    }
    
    .invoice-box table tr.total td:nth-child(2) {
        border-top: 2px solid #eee;
        font-weight: bold;
    }
    
    @media only screen and (max-width: 600px) {
        .invoice-box table tr.top table td {
            width: 100%;
            display: block;
            text-align: center;
        }
        
        .invoice-box table tr.information table td {
            width: 100%;
            display: block;
            text-align: center;
        }
    }
    
    /** RTL **/
    .rtl {
        direction: rtl;
        font-family: Tahoma, 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif;
    }
    
    .rtl table {
        text-align: right;
    }
    
    .rtl table tr td:nth-child(2) {
        text-align: left;
    }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
    <div class="row">
        <div class="col-lg-2 col-md-2 col-sm-2">
            <asp:Panel runat="server" ID="pnlBuscar" DefaultButton="btnBuscar">
            <section class="panel">
              <header class="panel-heading">
                Nueva Liquidación? &nbsp;
                     <asp:Label runat="server" ID="txtLiquidador"  Width="250px" Enabled="false"></asp:Label>
              </header>
              <div class="panel-body">
                <div class="form-inline" role="form">
                  <div class="form-group">
                      <asp:Label runat="server" ID="Label3" Text="N° Fact"/><br/>
                     <asp:TextBox runat="server" type="Text" ID="txtNumFact" CssClass="form-control" placeholder="N° Fact."></asp:TextBox>
                  </div>
                  <div class="form-group">
                      <asp:RadioButtonList ID="RadioButtonList1" runat="server">
                          <asp:ListItem Text="Empagro" 
        Value="Empagro" 
        Selected="True" />
                          <asp:ListItem Text="Rosa" 
        Value="Rosa" 
        Selected="False" />
                          <asp:ListItem Text="Don Beto" 
        Value="Don Beto" 
        Selected="False" />
                      </asp:RadioButtonList>
                  </div>
                  <div class="form-group">
                      <asp:Label runat="server" ID="Label1" Text=""/><br/>
                    <asp:Button runat="server" ID="btnBuscar" Text="Buscar" CssClass="btn btn-primary" OnClick="btnBuscar_Click"/>
                      </div>
                </div>

              </div>
            </section>
                </asp:Panel>
    </div>
        
        <div class="col-lg-10 col-md-10 col-sm-10">
             <asp:Panel runat="server" ID="Panel1" DefaultButton="btnAplicar">
              <div class="invoice-box">
        <table cellpadding="0" cellspacing="0">
            <tr class="top">
                <td colspan="2">
                    <table>
                        <tr>
                            <td class="title">
                                <asp:Label runat="server" ID="lblEmpresa"/>
                            </td>
                            
                            <td>
                                Documento #: <asp:Label runat="server" ID="lblFact"/><br>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            
            <tr class="information">
                <td colspan="2">
                    <table>
                        <tr>
                            <td>
                                Tipo de Documento<br/>
                                <asp:DropDownList runat="server" ID="ddlTipoDocumento" BackColor="Transparent" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoDocumento_SelectedIndexChanged" />
                            </td>
                            
                            <td>
                                <asp:Label runat="server" ID="lblCliente"/>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            
            <tr class="heading">
                <td>
                    Tipo de Pago
                </td>
                
                <td>
                    <asp:DropDownList runat="server" ID="ddlTipoPago" BackColor="Transparent" />
                </td>
            </tr>
            
            <tr class="details">
                <td>
                <div class="form-inline" role="form">
                  <div class="form-group">
                   <asp:TextBox runat="server" ID="txtFechaRecibo" CssClass="form-control" Type="Date"/>
                       </div>
                  <div class="form-group"><asp:TextBox runat="server" ID="txtNumeroRecibo" CssClass="form-control" placeholder="N° Recibo" />
                      </div>
                    </div>
                </td>
                
                <td>
                   <asp:TextBox runat="server" ID="txtMonto" CssClass="form-control" placeholder="Monto" />
                </td>
            </tr>
            <tr class="total">
                <td>
                    <asp:Button runat="server" ID="btnAplicar" Text="Aplicar" CssClass="btn btn-info" OnClick="btnAplicar_Click" />
                    <asp:Button runat="server" ID="btnCancelarLiquidacion" Text="Cancelar liquidación" CssClass="btn btn-danger" OnClick="btnCancelarLiquidacion_Click" />
                </td>
                
                <td>
                   Total: <asp:Label runat="server" ID="lblTotal"/>
                </td>
            </tr>
        </table>
    </div>
                </asp:Panel>
            </div>
        </div>
    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <asp:Label runat="server" ID="lblResult"/>
            </div>
    </div>
    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <asp:Panel runat="server" ID="Panel2" DefaultButton="ImageButton1">
              <div class="panel-body">
                <div class="form-inline" role="form">
                  <div class="form-group">
            <asp:TextBox runat="server" ID="txtBuscar" CssClass="form-control" placeholder="Buscar" Width="100px"/>
                <asp:ImageButton OnClick="ImageButton1_Click" runat="server" ID="ImageButton1" ImageUrl="~/img/find.png" Width="25px" /><br/>
                        <asp:Label runat="server" ID="Label2" Text="" ForeColor="black"></asp:Label>
                      </div>
                    </div>
                  </div>
            </asp:Panel>
            </div>
        </div>
    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <asp:Panel runat="server" ScrollBars="Horizontal" Width="100%">
            <asp:GridView ID="dtgLiquidaciones" runat="server" PageSize="5" CellPadding="4" CssClass="table table-striped table-advance table-hover" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" Font-Size="Small" Width="100%" AllowPaging="true" OnPageIndexChanging="dtgLiquidaciones_PageIndexChanging" PagerStyle-CssClass="myPagerClass" OnRowCommand="dtgLiquidaciones_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="Recibo" HeaderText="Recibo" />
                                <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                                <asp:BoundField DataField="Monto" DataFormatString="{0:c}" HeaderText="Monto" />
                                <asp:BoundField DataField="Documento" DataFormatString="{0:c}" HeaderText="Documento" />
                                <asp:ButtonField ButtonType="Image" ItemStyle-VerticalAlign="Middle" ImageUrl="~/img/Delete.png" CommandName="Agregar">
                                <ControlStyle Width="25px" />
                                <ItemStyle VerticalAlign="Middle" Width="35px" />
                                </asp:ButtonField>
                            </Columns>
<PagerStyle CssClass=" pagination"></PagerStyle>
                        </asp:GridView>
                </asp:Panel>
            </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder4" runat="server">
<li><i class="fa fa-home"></i><a href="default.aspx">Inicio</a></li>
              <li><i class="fa fa-cloud-download"></i>Liquidar Ruta</li>
</asp:Content>
