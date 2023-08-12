<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="Resumen.aspx.cs" Inherits="GestionCobro.Resumen" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder4" runat="server">
<li><i class="fa fa-home"></i><a href="default.aspx">Inicio</a></li>
              <li><i class="fa fa-beer"></i>Resumen Asignaciones</li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <asp:Panel runat="server" ScrollBars="Horizontal" Width="100%">
            <asp:GridView ID="dtgRuta" runat="server" CellPadding="4" CssClass="table table-advance table-hover" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" Font-Size="Small" Width="100%" PagerStyle-CssClass="myPagerClass" OnPageIndexChanging="dtgRuta_PageIndexChanging" OnRowCommand="dtgRuta_RowCommand" OnRowDataBound="dtgRuta_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="Ruta" HeaderText="Ruta" />
                                <asp:BoundField DataField="Desde" HeaderText="Desde" />
                                <asp:BoundField DataField="Hasta" HeaderText="Hasta" />
                                <asp:BoundField DataField="Total" HeaderText="Total" />
                                <asp:BoundField DataField="DocsPendientes" HeaderText="Pendientes" />
                                <asp:BoundField DataField="Monto" DataFormatString="{0:c}" HeaderText="Monto" />
                                <asp:BoundField DataField="Tramite" DataFormatString="{0:c}" HeaderText="Tramite" />
                                <asp:BoundField DataField="Original" DataFormatString="{0:c}" HeaderText="Original" />
                                <asp:BoundField DataField="Recibo" DataFormatString="{0:c}" HeaderText="Recibo" />
                                <asp:BoundField DataField="Pendiente" DataFormatString="{0:c}" HeaderText="Pendiente" />
                                <asp:BoundField DataField="U_Estado" DataFormatString="{0:c}" HeaderText="Estado Asignación" />
                                <asp:ButtonField ButtonType="Image" ItemStyle-VerticalAlign="Middle" ImageUrl="~/img/locked.png" CommandName="Cerrar">
                                <ControlStyle Width="25px" />
                                <ItemStyle VerticalAlign="Middle" Width="35px" />
                                </asp:ButtonField>
                                <asp:ButtonField ButtonType="Image" ItemStyle-VerticalAlign="Middle" ImageUrl="~/img/Add.png" CommandName="Agregar">
                                <ControlStyle Width="25px" />
                                <ItemStyle VerticalAlign="Middle" Width="35px" />
                                </asp:ButtonField>
                                <asp:ButtonField ButtonType="Image" ItemStyle-VerticalAlign="Middle" ImageUrl="~/img/info.png" CommandName="Info">
                                <ControlStyle Width="25px" />
                                <ItemStyle VerticalAlign="Middle" Width="35px" />
                                </asp:ButtonField>
                            </Columns>
<PagerStyle CssClass=" pagination"></PagerStyle>
                        </asp:GridView>
                </asp:Panel><br/>
                        <asp:Label runat="server" ID="lblResult" Text="" ForeColor="black"></asp:Label>
            </div>
        </div>
     <asp:Label ID="lblHidden" runat="server" Text=""></asp:Label>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <ajaxToolkit:ModalPopupExtender ID="mpePopUp" runat="server" TargetControlID="lblHidden" PopupControlID="divPopUp" BackgroundCssClass="modalBackground"></ajaxToolkit:ModalPopupExtender>
    <div  id="divPopUp" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                  <div class="modal-dialog">
                    <div class="modal-content">
                      <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h4 class="modal-title">Cerrar Asignación</h4>
                      </div>
                      <div class="modal-body">

                        Esta seguro que desea cerrar la asignación<br />
                          Desde: <asp:Label Font-Bold="true" runat="server" ID="lblDesde"></asp:Label><br />
                          Hasta: <asp:Label Font-Bold="true" runat="server" ID="lblHasta"></asp:Label><br />
                          Ruta: <asp:Label Font-Bold="true" runat="server" ID="lblRuta"></asp:Label><br />

                      </div>
                      <div class="modal-footer">
                          <asp:Button id="btnCancel" OnClick="btnCancel_Click" CssClass="btn btn-default" runat="server" text="Cancel" />
                          <asp:Button id="btnOk" OnClick="btnOk_Click" runat="server" text="Ok" CssClass="btn btn-success"/>
                      </div>
                    </div>
                  </div>
                </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
