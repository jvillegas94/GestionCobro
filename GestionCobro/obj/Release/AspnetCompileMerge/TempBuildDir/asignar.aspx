<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="asignar.aspx.cs" Inherits="GestionCobro.asignar" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style>
.vl {
  border-left: 6px solid green;
  height: 50px;
  display:block;
}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <asp:Panel runat="server" DefaultButton="btnAsignar">
            <section class="panel">
              <header class="panel-heading">
                Nueva Asignacion? &nbsp;
                     <asp:Label runat="server" ID="txtAsignador"  Width="250px" Enabled="false"></asp:Label><asp:Button runat="server" ID="btnActualizar" Text="Actualizar" Visible="false" OnClick="btnActualizar_Click" CssClass="btn btn-danger" />
              </header>
              <div class="panel-body">
                <div class="form-inline" role="form">
                  <div class="form-group" style="text-align:center">
                      <asp:Label runat="server" ID="lblDesde" Text="Desde"/><br/>
                     <asp:TextBox runat="server" type="date" ID="txtDesde" CssClass="form-control" placeholder="Fecha"></asp:TextBox>
                  </div>
                  <div class="form-group" style="text-align:center">
                      <asp:Label runat="server" ID="lblHasta" Text="Hasta"/><br/>
                     <asp:TextBox runat="server" type="date" ID="txtHasta" CssClass="form-control" placeholder="Fecha"></asp:TextBox>
                  </div>
                  <div class="form-group" style="text-align:center">
                      <asp:Label runat="server" ID="Label2" Text="Ruta"/><br/>
                    <asp:DropDownList CssClass="form-control" runat="server" ID="ddlRuta" placeholder="Ruta"  ForeColor="Gray" >
                        </asp:DropDownList></div>
                  <div class="form-group" style="text-align:center">
                      <asp:ImageButton OnClick="imgFind_Click" runat="server" ID="imgFind" ImageUrl="~/img/find.png" Width="25px" />
                      </div>
                    
                    <div class="vl form-group"></div>
                  <div class="form-group" style="text-align:center">
                      <asp:Label runat="server" ID="Label5" Text="Omitir Ruta?"/><br/>
                      <asp:CheckBox runat="server" ID="chkOmitir" Checked="false" />
                  </div>
                    <div class="vl form-group"></div>
                  <div class="form-group" style="text-align:center">
                      <asp:Label runat="server" ID="Label7" Text="Registro Automático"/><br/>
                      <asp:CheckBox runat="server" ID="chkBoxAutomatico" Checked="false" AutoPostBack="true" OnCheckedChanged="chkBoxAutomatico_CheckedChanged" />
                  </div>
                    <div class="vl form-group"></div>
                  <div class="form-group" style="text-align:center">
                      <asp:Label runat="server" ID="Label6" Text="Trámite?"/><br/>
                      <asp:CheckBox runat="server" ID="chkTramite" Checked="false" AutoPostBack="true" OnCheckedChanged="chkTramite_CheckedChanged" />
                  </div>
                    <div class="vl form-group"></div>
                    <asp:Panel runat="server" CssClass="form-group" ID="PnlManual">
                  <div class="form-group">
                      <asp:Label runat="server" ID="lblEmpresa" Text="Empresa"/><br/>
                    <asp:DropDownList CssClass="form-control" runat="server" ID="ddlEmpresa" placeholder="Empresa"  ForeColor="Gray" >
                            <asp:ListItem Selected="True">Empagro</asp:ListItem>
                            <asp:ListItem>Rosa</asp:ListItem>
                            <asp:ListItem>Don Beto</asp:ListItem>
                        </asp:DropDownList>
                      </div>
                  <div class="form-group">
                      <asp:Label runat="server" ID="Label4" Text="Tipo"/><br/>
                    <asp:DropDownList CssClass="form-control" runat="server" ID="ddlTipo" placeholder="Empresa"  ForeColor="Gray" >
                            <asp:ListItem Selected="True">FC</asp:ListItem>
                            <asp:ListItem>NC</asp:ListItem>
                        </asp:DropDownList>
                  </div>
                  <div class="form-group" style="text-align:center">
                      <asp:Label runat="server" ID="Label3" Text="N° Fact"/><br/>
                     <asp:TextBox runat="server" type="Text" ID="txtNumFact" CssClass="form-control" placeholder="N° Fact."></asp:TextBox>
                  </div>
                        </asp:Panel>
                    <div class="vl form-group"></div>
                    <asp:Panel runat="server" CssClass="form-group" ID="PnlAutomatico">
                  <div class="form-group" style="text-align:center">
                      <asp:Label runat="server" ID="Label10" Text="Código de Barras"/><br/>
                     <asp:TextBox runat="server" type="Text" ID="txtAutomatico" CssClass="form-control" placeholder="N° Fact." AutoPostBack="true" OnTextChanged="txtAutomatico_TextChanged"></asp:TextBox>
                  </div>
                        </asp:Panel>
                  <div class="form-group" style="text-align:center">
                      <asp:Label runat="server" ID="Label1" Text=""/><br/>
                    <asp:Button runat="server" ID="btnAsignar" Text="Asignar" CssClass="btn btn-primary" OnClick="btnAsignar_Click" />
                      </div>
                </div>

              </div>
            </section>
                </asp:Panel>

    </div>
        </div>
    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <asp:Panel runat="server" ID="pnlBuscar" DefaultButton="ImageButton1">
              <div class="panel-body">
                <div class="form-inline" role="form">
                  <div class="form-group">
            <asp:TextBox runat="server" ID="txtBuscar" CssClass="form-control" placeholder="Buscar" Width="100px"/>
                <asp:ImageButton OnClick="ImageButton1_Click" runat="server" ID="ImageButton1" ImageUrl="~/img/find.png" Width="25px" /><br/>
                        <asp:Label runat="server" ID="lblResult" Text="" ForeColor="black"></asp:Label>
                      </div>
                    </div>
                  </div>
            </asp:Panel>
            </div>
        </div>
    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <asp:Panel runat="server" ScrollBars="Horizontal" Width="100%">
            <asp:GridView ID="dtgFacturas" runat="server" CellPadding="4" CssClass="table table-striped table-advance table-hover" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" Font-Size="Small" Width="100%" PagerStyle-CssClass="myPagerClass" AllowPaging="True" OnPageIndexChanging="dtgFacturas_PageIndexChanging" OnRowCommand="dtgFacturas_RowCommand" PageSize="5">
                            <Columns>
                                <asp:BoundField DataField="DocNum" HeaderText="N° Doc" />
                                <asp:BoundField DataField="Tipo" HeaderText="Tipo Doc" />
                                <asp:BoundField DataField="U_Ruta" HeaderText="Ruta" />
                                <asp:BoundField DataField="CardName" HeaderText="Cliente" />
                                <asp:BoundField DataField="Total" DataFormatString="{0:c}" HeaderText="Monto" />
                                <asp:BoundField DataField="Desde" HeaderText="Desde" />
                                <asp:BoundField DataField="Hasta" HeaderText="Hasta" />
                                <asp:BoundField DataField="Empresa" HeaderText="Empresa" />
                                <asp:ButtonField ButtonType="Image" ItemStyle-VerticalAlign="Middle" ImageUrl="~/img/delete.png" CommandName="Eliminar">
                                <ControlStyle Width="25px" />
                                <ItemStyle VerticalAlign="Middle" Width="35px" />
                                </asp:ButtonField>
                            </Columns>

<PagerStyle CssClass=" pagination"></PagerStyle>
                        </asp:GridView>
            </asp:Panel>
            </div>
        </div>
    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <asp:Panel runat="server" ScrollBars="Horizontal" Width="100%">
            <asp:GridView ID="grvPendientes" runat="server" CellPadding="4" CssClass="table table-striped table-advance table-hover" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" Font-Size="Small" Width="100%" PagerStyle-CssClass="myPagerClass" AllowPaging="True" OnPageIndexChanging="grvPendientes_PageIndexChanging" OnRowCommand="grvPendientes_RowCommand" PageSize="5">
                            <Columns>
                                <asp:BoundField DataField="CardName" HeaderText="Cliente" />
                                <asp:BoundField DataField="Empresa" HeaderText="Empresa" />
                                <asp:BoundField DataField="SlpName" HeaderText="Ruta" />
                                <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                                <asp:BoundField DataField="DocNum" HeaderText="N° Doc" />
                                <asp:BoundField DataField="DocDate" HeaderText="Fecha Doc" />
                                <asp:BoundField DataField="Total" HeaderText="Total"  DataFormatString="{0:c}" />
                                <asp:BoundField DataField="Vencimiento" HeaderText="Vencimiento" />
                                <asp:BoundField DataField="DiasVencido" HeaderText="Dias Vencido" />
                                <asp:ButtonField ButtonType="Image" ItemStyle-VerticalAlign="Middle" ImageUrl="~/img/Add.png" CommandName="Eliminar">
                                <ControlStyle Width="25px" />
                                <ItemStyle VerticalAlign="Middle" Width="35px" />
                                </asp:ButtonField>
                            </Columns>

<PagerStyle CssClass=" pagination"></PagerStyle>
                        </asp:GridView>
            </asp:Panel>
                        <asp:ImageButton CssClass="form-control" runat="server" ID="xptExcel" ImageUrl="~/img/XLS.png" Width="60px" OnClick="xptExcel_Click"/>
            </div>
        </div>
    <asp:Label ID="lblHidden" runat="server" Text=""></asp:Label>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <ajaxToolkit:ModalPopupExtender ID="mpePopUp" runat="server" TargetControlID="lblHidden" PopupControlID="divPopUp" BackgroundCssClass="modalBackground"></ajaxToolkit:ModalPopupExtender>
    <asp:Panel runat="server" DefaultButton="btnOK">
    <div  id="divPopUp" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                  <div class="modal-dialog">
                    <div class="modal-content">
                      <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h4 class="modal-title">Re-Asignación</h4>
                      </div>
                      <div class="modal-body">

                        Este documento (<asp:Label  Font-Bold="true" runat="server" ID="lblNumFact"></asp:Label>) ya se encuentra registrado, desea re-asignarlo<br />
                          Desde: <asp:Label Font-Bold="true" runat="server" ID="lblDesdeRA"></asp:Label><br />
                          Hasta: <asp:Label Font-Bold="true" runat="server" ID="lblHastaRA"></asp:Label><br />
                          Ruta: <asp:Label Font-Bold="true" runat="server" ID="lblRutaRA"></asp:Label><br />

                      </div>
                      <div class="modal-footer">
                          <asp:Button id="btnCancel" OnClick="btnCancel_Click" CssClass="btn btn-default" runat="server" text="Cancel" />
                          <asp:Button id="btnOk" OnClick="btnOk_Click" runat="server" text="Ok" CssClass="btn btn-success"/>
                      </div>
                    </div>
                  </div>
                </div>

    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder4" runat="server">
<li><i class="fa fa-home"></i><a href="default.aspx">Inicio</a></li>
              <li><i class="fa fa-cloud-upload"></i>Asignar Ruta</li>
</asp:Content>
