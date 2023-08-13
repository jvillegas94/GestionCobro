<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="Bitacora.aspx.cs" Inherits="GestionCobro.Bitacora" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder4" runat="server">
    <li><i class="fa fa-home"></i><a href="default.aspx">Inicio</a></li>
              <li><i class="fa fa-bullseye"></i>Bitacora Liquidación</li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
        <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <asp:Panel runat="server" DefaultButton="btnBuscar">
            <section class="panel">
              <header class="panel-heading">
                Parámetros de búsqueda &nbsp;
              </header>
              <div class="panel-body">
                <div class="form-inline" role="form">
                  <div class="form-group">
                    <asp:TextBox runat="server" ID="txtFechaInicio" type="date" placeholder="FechaInicio" autofocus="autofocus"></asp:TextBox>
                    </div>
                  <div class="form-group">
                    <asp:TextBox runat="server" ID="txtFechaFin" type="date" placeholder="FechaFin"></asp:TextBox>
                    </div>
                  <div class="form-group">
                <asp:Button runat="server" ID="btnBuscar" Text="Buscar" OnClick="btnBuscar_Click"/>
                      </div>
                  </div>
                  </div>
                </section>
                </asp:Panel><br />
            <asp:Label runat="server" ID="lblResult"></asp:Label>
            </div>
            </div>
    
        <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <asp:Panel runat="server" ScrollBars="Horizontal" Width="100%">
            <asp:GridView ID="dtgBitacora" runat="server" CellPadding="4" CssClass="table table-striped table-advance table-hover" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" Font-Size="Small" Width="100%" PagerStyle-CssClass="myPagerClass" AllowPaging="True" OnPageIndexChanging="dtgBitacora_PageIndexChanging" OnRowCommand="dtgBitacora_RowCommand" PageSize="20">
                            <Columns>
                                <asp:BoundField DataField="Usuario" HeaderText="Usuario" />
                                <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                                <asp:BoundField DataField="Inicio de Sesión" HeaderText="Inicio de Sesión" />
                                <asp:BoundField DataField="Fin de Sesión" HeaderText="Fin de Sesión" />
                                <asp:BoundField DataField="Asignar más documentos" HeaderText="Asignar más documentos" />
                                <asp:BoundField DataField="Eliminar Asignacion" HeaderText="Eliminar Asignacion" />
                                <asp:BoundField DataField="Nuevo Documento Insertado" HeaderText="Nuevo Documento Insertado" />
                                <asp:BoundField DataField="Liquidación de Documento" HeaderText="Liquidación de Documento" />
                                <asp:BoundField DataField="Cancelar Liquidacion" HeaderText="Cancelar Liquidacion" />
                                <asp:BoundField DataField="Cerrar Asignación" HeaderText="Cerrar Asignación" />
                            </Columns>

<PagerStyle CssClass=" pagination"></PagerStyle>
                        </asp:GridView>
            </asp:Panel>
            </div>
            </div>
                        <asp:ImageButton CssClass="form-control" runat="server" ID="xptExcel" ImageUrl="~/img/XLS.png" Width="60px" OnClick="xptExcel_Click1"/>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
