<%@ Page Title="Nuevo" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="nuevo.aspx.cs" Inherits="Gestion_Cobro.nuevo" %>
<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
</asp:Content>
<asp:Content ID="body" ContentPlaceHolderID="Body" runat="server">
    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <asp:Panel runat="server" ID="pnlBuscar" DefaultButton="btnBuscar">
    <div class="form-inline">
  <div class="form-group">
     <div class="inputWithIcon">
  <asp:TextBox runat="server" ID="txtFechaInicio" type="date" />
  <i class="fa fa-calendar-minus-o fa-lg fa-fw" aria-hidden="true"></i>
</div>
      </div>
  <div class=" form-group">
    <div class="inputWithIcon">
  <asp:TextBox runat="server" ID="txtFechaFin" type="date" />
  <i class="fa fa-calendar-plus-o fa-lg fa-fw" aria-hidden="true"></i>
</div>
      </div>
  <div class=" form-group">
    <div class="inputWithIcon">
  <asp:DropDownList runat="server" ID="ddlRuta"></asp:DropDownList>
  <i class="fa fa-map fa-lg fa-fw" aria-hidden="true"></i>
</div>
      </div>
  <div class=" form-group">
    <div class="inputWithIcon">
  <asp:Button runat="server" ID="btnBuscar" Text="Aplicar" OnClick="btnBuscar_Click"></asp:Button>
  <i class="fa fa-check fa-lg fa-fw" aria-hidden="true"></i>
</div>
      </div>

    </div>
                </asp:Panel>
            </div>
        </div>
    <div class="row">
        <div class="col-lg-5 col-md-5 col-sm-5 col-xs-5">
            
            <section class=" panel">
              <header class="panel-heading panel-info">
                Documentos Pendientes<asp:TextBox runat="server" ID="txtBuscarPendientes" AutoPostBack="true" OnTextChanged="txtBuscarPendientes_TextChanged" placeholder="N° Doc"/>
              </header>
              <div class="panel-body">
                  <asp:Panel runat="server" ScrollBars="Horizontal" Width="100%">
            <asp:GridView ID="grvPendientes" runat="server" CellPadding="4" CssClass="myGridClass" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" Font-Size="Small" Width="100%" PagerStyle-CssClass="myPagerClass" AllowPaging="True" OnPageIndexChanging="grvPendientes_PageIndexChanging" OnRowCommand="grvPendientes_RowCommand" PageSize="17">
                            <Columns>
                                <asp:TemplateField HeaderText="Asignar">
                     <ItemTemplate>
                        <asp:CheckBox ID="chkAccept" runat="server" EnableViewState="true" Checked="false"/>
                     </ItemTemplate>
                </asp:TemplateField>
                                <asp:BoundField DataField="DocNum" HeaderText="N° Doc" />
                                <asp:BoundField DataField="Tipo" HeaderText="Tipo Doc" />
                                <asp:BoundField DataField="CardName" HeaderText="Cliente" />
                                <asp:BoundField DataField="Total" DataFormatString="{0:c}" HeaderText="Monto" />
                            </Columns>

<PagerStyle CssClass=" pagination"></PagerStyle>
                        </asp:GridView>
            </asp:Panel>
</div>
                </section>
            </div>
        <div class="col-lg-2 col-md-2 col-sm-2 col-xs-2" style="vertical-align:middle">
            <div class="inputWithIcon">
  <asp:Button runat="server" ID="Button1" Text="Asignar" CssClass="btn btn-info btn-lg btn-block" OnClick="Button1_Click"></asp:Button>
  <i class="fa fa-arrow-circle-right fa-lg fa-fw" aria-hidden="true"></i>
</div>
<div class="inputWithIcon">
  <asp:Button runat="server" ID="Button2" Text="Eliminar" CssClass="btn btn-danger btn-lg btn-block" OnClick="Button1_Click"></asp:Button>
  <i class="fa fa-arrow-circle-left fa-lg fa-fw" aria-hidden="true"></i>
</div>
            </div>
        <div class="col-lg-5 col-md-5 col-sm-5 col-xs-5">
            <section class=" panel">
              <header class="panel-heading panel-info">
                Documentos Asignados
              </header>
              <div class="panel-body">
                  <asp:Panel runat="server" ScrollBars="Horizontal" Width="100%">
            <asp:GridView ID="GridView1" runat="server" CellPadding="4" CssClass="myGridClass" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" Font-Size="Small" Width="100%" PagerStyle-CssClass="myPagerClass" AllowPaging="True" OnPageIndexChanging="grvPendientes_PageIndexChanging" OnRowCommand="grvPendientes_RowCommand" PageSize="10">
                            <Columns>
                                <asp:TemplateField HeaderText="Asignar">
                     <ItemTemplate>
                        <asp:CheckBox ID="chkAccept" runat="server" EnableViewState="true" Checked="false"/>
                     </ItemTemplate>
                </asp:TemplateField>
                                <asp:BoundField DataField="DocNum" HeaderText="N° Doc" />
                                <asp:BoundField DataField="Tipo" HeaderText="Tipo Doc" />
                                <asp:BoundField DataField="CardName" HeaderText="Cliente" />
                                <asp:BoundField DataField="Total" DataFormatString="{0:c}" HeaderText="Monto" />
                            </Columns>

<PagerStyle CssClass=" pagination"></PagerStyle>
                        </asp:GridView>
            </asp:Panel>
</div>
                </section>
            </div>
        </div>
</asp:Content>
