<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="GestionCobro._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
        <div class="row">
            <a href="asignar.aspx">
 <div class="col-lg-3 col-md-3 col-sm-12 col-xs-12" runat="server" visible="true">
            <div class="info-box blue-bg">
              <i class="fa fa-cloud-upload"></i>
              <div class="count">Asignar</div>
              <div class="title">Ruta</div>
            </div>
            <!--/.info-box-->
          </div>
          <!--/.col--></a>
            <a href="liquidar.aspx">
 <div id="Asignar" class="col-lg-3 col-md-3 col-sm-12 col-xs-12" runat="server" visible="true">
            <div class="info-box green-bg">
              <i class="fa fa-cloud-download"></i>
              <div class="count">Liquidar</div>
              <div class="title">Ruta</div>
            </div>
            <!--/.info-box-->
          </div>
          <!--/.col-->
    </a>
            <a href="Resumen.aspx">
 <div id="Resumen" class="col-lg-3 col-md-3 col-sm-12 col-xs-12" runat="server" visible="true">
            <div class="info-box red-bg">
              <i class="fa fa-beer"></i>
              <div class="count">Resumen</div>
              <div class="title">Asignaciones</div>
            </div>
            <!--/.info-box-->
          </div>
          <!--/.col-->
    </a>
            <a href="Bitacora.aspx">
 <div id="Bitacora" class="col-lg-3 col-md-3 col-sm-12 col-xs-12" runat="server" visible="false">
            <div class="info-box orange-bg">
              <i class="fa fa-bullseye"></i>
              <div class="count">Bitacora</div>
              <div class="title">Liquidacion</div>
            </div>
            <!--/.info-box-->
          </div>
          <!--/.col-->
    </a>
            </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder4" runat="server">
<li><i class="fa fa-home"></i><a href="default.aspx">Inicio</a></li>
              <li><i class="fa fa-briefcase"></i>Menu Principal</li>
</asp:Content>