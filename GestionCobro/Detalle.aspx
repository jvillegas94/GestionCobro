<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="Detalle.aspx.cs" Inherits="GestionCobro.Detalle" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder4" runat="server">
<li><i class="fa fa-home"></i><a href="default.aspx">Inicio</a></li>
              <li><i class="fa fa-beer"></i><a href="Resumen.aspx">Resumen</a></li>
              <li><i class="fa fa-info"></i>Detalle Asignación</li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
    <div class="row">
     <div class="col-lg-6 col-md-6">
         </div>
    </div>
    <div class="row">
     <div class="col-lg-6 col-md-6" runat="server" id="Asignados">
            <div class="panel panel-default">
              <div class="panel-heading">
                <table style="width:100%"><tr><td><strong>Asignados</strong></td><td>
                        <asp:ImageButton CssClass="form-control" runat="server" ID="ibtnAsignados" CommandName="Asignados" ImageUrl="~/img/XLS.png" Width="60px" OnClick="xptExcel_Click"/>
                                              </td><td><h2><i class="fa fa-flag-o red"></i><strong><asp:Label runat="server" ID="lblRuta"></asp:Label></strong></h2></td>
                  <td style="text-align:right"><i class="fa fa-arrow-circle-right"></i><asp:Label runat="server" ID="lblDesde"></asp:Label>
                  <i class="fa fa-arrow-circle-left"></i><asp:Label runat="server" ID="lblHasta"></asp:Label></td></tr></table>
              </div>
              <div class="panel-body">
                  <asp:Panel runat="server" ScrollBars="Both" Width="100%" Height="250px">
            <asp:GridView ID="grvAsignados" runat="server" CellPadding="4" CssClass="table table-advance table-hover" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" Font-Size="Small" Width="100%" PagerStyle-CssClass="myPagerClass">
                            <Columns>
                                <asp:BoundField DataField="Empresa" HeaderText="Empresa" />
                                <asp:BoundField DataField="DocNum" HeaderText="DocNum" />
                                <asp:BoundField DataField="CardName" HeaderText="Cliente" />
                                <asp:BoundField DataField="Total" DataFormatString="{0:c}" HeaderText="Monto" />
                            </Columns>
<PagerStyle CssClass=" pagination"></PagerStyle>
                        </asp:GridView>
                </asp:Panel>
              </div>

            </div>

          </div>
     <div class="col-lg-6 col-md-6" runat="server" id="Tramitados">
            <div class="panel panel-default">
              <div class="panel-heading">
                <table style="width:100%"><tr><td><strong>Tramitados</strong></td><td>
                        <asp:ImageButton CssClass="form-control" runat="server" ID="ibtnTramitados" CommandName="Tramitados" ImageUrl="~/img/XLS.png" Width="60px" OnClick="xptExcel_Click"/></td><td><h2><i class="fa fa-flag-o red"></i><strong><%:Session["Ruta"]%></strong></h2></td>
                  <td style="text-align:right"><i class="fa fa-arrow-circle-right"></i><%:Session["Desde"]%>
                  <i class="fa fa-arrow-circle-left"></i><%:Session["Hasta"]%></td></tr></table>
              </div>
              <div class="panel-body">
                  <asp:Panel runat="server" ScrollBars="Both" Width="100%" Height="250px">
            <asp:GridView ID="grvTramitados" runat="server" CellPadding="4" CssClass="table table-advance table-hover" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" Font-Size="Small" Width="100%" PagerStyle-CssClass="myPagerClass">
                            <Columns>
                                <asp:BoundField DataField="Empresa" HeaderText="Empresa" />
                                <asp:BoundField DataField="DocNum" HeaderText="DocNum" />
                                <asp:BoundField DataField="CardName" HeaderText="Cliente" />
                                <asp:BoundField DataField="Total" DataFormatString="{0:c}" HeaderText="Monto" />
                            </Columns>
<PagerStyle CssClass=" pagination"></PagerStyle>
                        </asp:GridView>
                </asp:Panel>
              </div>

            </div>

          </div>
     <div class="col-lg-6 col-md-6" runat="server" id="Originales">
            <div class="panel panel-default">
              <div class="panel-heading">
                <table style="width:100%"><tr><td><strong>Originales</strong></td><td>
                        <asp:ImageButton CssClass="form-control" runat="server" ID="ibtnOriginales" CommandName="Originales" ImageUrl="~/img/XLS.png" Width="60px" OnClick="xptExcel_Click"/></td><td><h2><i class="fa fa-flag-o red"></i><strong><%:Session["Ruta"]%></strong></h2></td>
                  <td style="text-align:right"><i class="fa fa-arrow-circle-right"></i><%:Session["Desde"]%>
                  <i class="fa fa-arrow-circle-left"></i><%:Session["Hasta"]%></td></tr></table>
              </div>
              <div class="panel-body">
                  <asp:Panel runat="server" ScrollBars="Both" Width="100%" Height="250px">
            <asp:GridView ID="grvOriginales" runat="server" CellPadding="4" CssClass="table table-advance table-hover" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" Font-Size="Small" Width="100%" PagerStyle-CssClass="myPagerClass">
                            <Columns>
                                <asp:BoundField DataField="Empresa" HeaderText="Empresa" />
                                <asp:BoundField DataField="DocNum" HeaderText="DocNum" />
                                <asp:BoundField DataField="CardName" HeaderText="Cliente" />
                                <asp:BoundField DataField="Total" DataFormatString="{0:c}" HeaderText="Monto" />
                            </Columns>
<PagerStyle CssClass=" pagination"></PagerStyle>
                        </asp:GridView>
                </asp:Panel>
              </div>

            </div>

          </div>
     <div class="col-lg-6 col-md-6" runat="server" id="Recibos">
            <div class="panel panel-default">
              <div class="panel-heading">
                <table style="width:100%"><tr><td><strong>Recibos</strong></td><td>
                        <asp:ImageButton CssClass="form-control" runat="server" ID="ibtnRecibos" CommandName="Recibos" ImageUrl="~/img/XLS.png" Width="60px" OnClick="xptExcel_Click"/></td><td><h2><i class="fa fa-flag-o red"></i><strong><%:Session["Ruta"]%></strong></h2></td>
                  <td style="text-align:right"><i class="fa fa-arrow-circle-right"></i><%:Session["Desde"]%>
                  <i class="fa fa-arrow-circle-left"></i><%:Session["Hasta"]%></td></tr></table>
              </div>
              <div class="panel-body">
                  <asp:Panel runat="server" ScrollBars="Both" Width="100%" Height="250px">
            <asp:GridView ID="grvRecibos" runat="server" CellPadding="4" CssClass="table table-advance table-hover" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" Font-Size="Small" Width="100%" PagerStyle-CssClass="myPagerClass">
                            <Columns>
                                <asp:BoundField DataField="DocNum" HeaderText="Numero Recibo" />
                                <asp:BoundField DataField="CardName" HeaderText="Cliente" />
                                <asp:BoundField DataField="Total" DataFormatString="{0:c}" HeaderText="Monto" />
                            </Columns>
<PagerStyle CssClass=" pagination"></PagerStyle>
                        </asp:GridView>
                </asp:Panel>
              </div>

            </div>

          </div>
     <div class="col-lg-6 col-md-6" runat="server" id="Pendientes">
            <div class="panel panel-default">
              <div class="panel-heading">
                <table style="width:100%"><tr><td><strong>Pendientes</strong></td><td>
                        <asp:ImageButton CssClass="form-control" runat="server" ID="ImageButton1" CommandName="Pendientes" ImageUrl="~/img/XLS.png" Width="60px" OnClick="xptExcel_Click"/></td><td><h2><i class="fa fa-flag-o red"></i><strong><%:Session["Ruta"]%></strong></h2></td>
                  <td style="text-align:right"><i class="fa fa-arrow-circle-right"></i><%:Session["Desde"]%>
                  <i class="fa fa-arrow-circle-left"></i><%:Session["Hasta"]%></td></tr></table>
              </div>
              <div class="panel-body">
                  <asp:Panel runat="server" ScrollBars="Both" Width="100%" Height="250px">
            <asp:GridView ID="grvPendientes" runat="server" CellPadding="4" CssClass="table table-advance table-hover" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" Font-Size="Small" Width="100%" PagerStyle-CssClass="myPagerClass">
                            <Columns>
                                <asp:BoundField DataField="Empresa" HeaderText="Empresa" />
                                <asp:BoundField DataField="DocNum" HeaderText="DocNum" />
                                <asp:BoundField DataField="CardName" HeaderText="Cliente" />
                                <asp:BoundField DataField="Total" DataFormatString="{0:c}" HeaderText="Monto" />
                            </Columns>
<PagerStyle CssClass=" pagination"></PagerStyle>
                        </asp:GridView>
                </asp:Panel>
              </div>

            </div>

          </div>
        </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
