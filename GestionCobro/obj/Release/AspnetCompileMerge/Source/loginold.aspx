<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="loginold.aspx.cs" Inherits="GestionCobro.login" %>

<!DOCTYPE html>
<html lang="en">

<head>
  <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <meta name="description" content="Control de Despacho | EMPAGRO">
  <meta name="author" content="Ing Luis Miguel Bolaños">
  <meta name="keyword" content="Empagro, Registro, Facturas, Control, Despachol">
  <link rel="shortcut icon" href="img/icon/icon.png">
    <link rel="icon" href="img/icon/icon.png">
    
  <title>Gestion Cobro - Empagro</title>

  <!-- Bootstrap CSS -->
  <link href="css/bootstrap.min.css" rel="stylesheet">
  <!-- bootstrap theme -->
  <link href="css/bootstrap-theme.css" rel="stylesheet">
  <!--external css-->
  <!-- font icon -->
  <link href="css/elegant-icons-style.css" rel="stylesheet" />
  <link href="css/font-awesome.css" rel="stylesheet" />
  <!-- Custom styles -->
  <link href="css/style.css" rel="stylesheet">
  <link href="css/style-responsive.css" rel="stylesheet" />

  <!-- HTML5 shim and Respond.js IE8 support of HTML5 -->
  <!--[if lt IE 9]>
    <script src="js/html5shiv.js"></script>
    <script src="js/respond.min.js"></script>
    <![endif]-->

    <!-- =======================================================
      Theme Name: NiceAdmin
      Theme URL: https://bootstrapmade.com/nice-admin-bootstrap-admin-html-template/
      Author: BootstrapMade
      Author URL: https://bootstrapmade.com
    ======================================================= -->
</head>

<body class="login-img3-body">

  <div class="container">

    <form class="login-form" runat="server">
      <div class="login-wrap">
        <p class="login-img"><i class="icon_lock_alt"></i></p>
        <div class="input-group">
          <span class="input-group-addon"><i class="icon_profile"></i></span>
            <asp:TextBox runat="server" TextMode="SingleLine" ID="txtUsuario" placeholder="Usuario" CssClass="form-control" autofocus="autofocus"/>
        </div>
        <div class="input-group">
          <span class="input-group-addon"><i class="icon_key_alt"></i></span>
            <asp:TextBox runat="server" TextMode="Password" ID="txtClave" placeholder="Clave" CssClass="form-control"/>
        </div>
          <asp:Label runat="server" ID="lblMensaje"/>
          <asp:Button runat="server" ID="btnLogin" CssClass="btn btn-primary btn-lg btn-block" Text="Iniciar" OnClick="btnLogin_Click"/>
      </div>
    
    </form>
      <div class="text-right footer">
        <div class="credits">
          <!--
            All the links in the footer should remain intact.
            You can delete the links only if you purchased the pro version.
            Licensing information: https://bootstrapmade.com/license/
            Purchase the pro version form: https://bootstrapmade.com/buy/?theme=NiceAdmin
          -->
          Elaborado por <a href="#">Ing. Luis Miguel Bolaños Mejias</a>
        </div>
      </div>
    
  </div>


</body>

</html>

