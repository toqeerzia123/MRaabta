<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ECSelection.aspx.cs" Inherits="MRaabta.ECSelection" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="description" content="Neon Admin Panel" />
    <meta name="author" content="" />
    <title>: : : M&P Express Logistics (Private) Limited : : :</title>
    <link rel="stylesheet" href="assets/js/jquery-ui/css/no-theme/jquery-ui-1.10.3.custom.min.css">
    <link rel="stylesheet" href="assets/css/font-icons/entypo/css/entypo.css">
    <link rel="stylesheet" href="http://fonts.googleapis.com/css?family=Noto+Sans:400,700,400italic">
    <link rel="stylesheet" href="assets/css/bootstrap.css">
    <link rel="stylesheet" href="assets/css/neon-core.css">
    <link rel="stylesheet" href="assets/css/neon-theme.css">
    <link rel="stylesheet" href="assets/css/neon-forms.css">
    <link rel="stylesheet" href="assets/css/custom.css">
    <script src="assets/js/jquery-1.11.0.min.js"></script>
    <!--[if lt IE 9]><script src="assets/js/ie8-responsive-file-warning.js"></script><![endif]-->
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
		<script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
		<script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js"></script>
	<![endif]-->
    <style>
        .txtArea
        {
            background: rgba(0, 0, 0, 0) none repeat scroll 0 0;
            border: 0 none;
            height: 26px;
            text-align: left;
            width: 265px;
        }
    </style>
</head>
<body class="page-body login-page login-form-fall" data-url="http://neon.dev">
    <!-- This is needed when you send requests via Ajax -->
    <script type="text/javascript">
        var baseurl = 'http://localhost:51598/';

        function ValidateEC() {
            var dd_ec = document.getElementById('<%= dd_ec.ClientID %>');
            if (dd_ec.options[dd_ec.options.selectedIndex].value == '0') {
                alert('Invalid Express Center');
                return false;
            }
            else {
                return true;
            }
        }
    </script>
    <style>
        .dd
        {
            background-color: #303640;
            color: White;
            border: 0px;
            height: 100%;
        }
        .btnExtra
        {
            text-align:center !important;
        }
    </style>
    <div class="login-container">
        <div class="login-header login-caret">
            <div class="login-content" style="width: 100%; background-color: #2a2a2f;">
                <a href="Loader.aspx" class="logo">
                    <img src="images/mnplogo.png" height="90" alt="" />
                </a>
                <p class="description">
                    <h2 style="color: #cacaca; font-weight: 100;">
                        M&P Express Logistics (Private) Limited.
                    </h2>
                </p>
                <!-- progress bar indicator -->
                <div class="login-progressbar-indicator">
                    <h3>
                        43%</h3>
                    <span>logging in...</span>
                </div>
            </div>
        </div>
        <div class="login-progressbar">
            <div>
            </div>
        </div>
        <div class="login-form">
            <div class="login-content">
                <div class="form-login-error">
                    <h3>
                        Invalid login</h3>
                    <p>
                        Please enter correct email and password!</p>
                </div>
                <form id="form1" runat="server">
                <div class="form-group" style="width: 100% !important;">
                    <div class="input-group" style="width: 100% !important; background-color: #303640 !important;
                        height: 100%">
                        <asp:DropDownList ID="dd_ec" runat="server" AppendDataBoundItems="true" CausesValidation="false"
                            CssClass="dd" Height="100%" Width="100%">
                            <asp:ListItem Value="0">Select Your Express Center</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group" style="text-align: center;">
                    <div style="float:left; width:45%; margin-right:10%;">
                        <Button ID="btn_back" class="btn btn-primary btn-block btn-login btnExtra" type="button"
                            Text="Back" Width="100%" OnClick="SendBack()" >Back</Button>
                    </div>
                    <div style="float:left; width:45%;">
                        <asp:Button ID="LoginButton" class="btn btn-primary btn-block btn-login btnExtra"
                            runat="server" Width="100%" Text="Proceed" OnClick="LoginButton_onClick" OnClientClick="return ValidateEC();" />
                    </div>
                </div>
                </form>
                <style>
                    td
                    {
                        border: 1px solid rgba(204, 204, 204, 0.1) !important;
                    }
                    th
                    {
                        border: 1px solid rgba(204, 204, 204, 0.1) !important;
                        background-color: rgba(235, 235, 235, 0) !important;
                    }
                    .icon-hover
                    {
                        cursor: pointer;
                    }
                </style>
                <script>
                    function copy(email, password) {
                        document.getElementById("email").value = email;
                        document.getElementById("password").value = password;
                    }
                    function SendBack() {
                    var  win =   window.open('login?', 'parent');
                        win.focus();
                    }
                </script>
                <%--<div class="panel panel-primary" style="background-color: rgba(255, 255, 255, 0);
                    border-color: rgba(235, 235, 235, 0.14);">
                    <div class="panel-heading" style="background-color: rgba(255, 255, 255, 0.16); border-color: rgba(204, 204, 204, 0.08);">
                        <div class="panel-title">
                            Demo account login credentials</div>
                    </div>
                     <div class="panel-body with-table">
        <table class="table table-bordered table-responsive">
            <thead>
                <tr>
                    <th>Email</th>
                    <th>Password</th>
                </tr>
            </thead>
        
            <tbody>
                <tr>
                    <td>Bilal</td>
                    <td>123</td>
                    <td>
                        <i class="entypo-target icon-hover tooltip-default" onclick="copy('teacher@example.com' , '1234')"
                             data-toggle="tooltip" data-placement="top" title="" data-original-title="copy"></i>
                    </td>
                </tr>
                <tr>
                    <td>Zohair</td>
                    <td>123</td>
                    <td>
                        <i class="entypo-target icon-hover tooltip-default" onclick="copy('student@example.com' , '1234')"
                             data-toggle="tooltip" data-placement="top" title="" data-original-title="copy"></i>
                    </td>
                </tr>
                <tr>
                    <td>Sunny</td>
                    <td>123</td>
                    <td>
                        <i class="entypo-target icon-hover tooltip-default" onclick="copy('parent@example.com' , '1234')"
                             data-toggle="tooltip" data-placement="top" title="" data-original-title="copy"></i>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
                </div>--%>
            </div>
        </div>
    </div>
    <!-- Bottom Scripts -->
    <script src="assets/js/gsap/main-gsap.js"></script>
    <script src="assets/js/jquery-ui/js/jquery-ui-1.10.3.minimal.min.js"></script>
    <script src="assets/js/bootstrap.js"></script>
    <script src="assets/js/joinable.js"></script>
    <script src="assets/js/resizeable.js"></script>
    <script src="assets/js/neon-api.js"></script>
    <script src="assets/js/jquery.validate.min.js"></script>
    <script src="assets/js/neon-login.js"></script>
    <script src="assets/js/neon-custom.js"></script>
    <script src="assets/js/neon-demo.js"></script>
</body>
</html>
