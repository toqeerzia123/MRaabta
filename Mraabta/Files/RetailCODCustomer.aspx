<%@ Page Title="New Retail COD Customer" Language="C#" MasterPageFile="~/BtsMasterPage.master"
    AutoEventWireup="true" CodeBehind="RetailCODCustomer.aspx.cs"
    Inherits="MRaabta.Files.RetailCODCustomer" %>


<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .lable {
            font-weight: bold;
            width: 110px;
        }

        .outer_box {
            background: #444 none repeat scroll 0 0;
            height: 101%;
            left: 0;
            opacity: 0.9;
            position: absolute;
            top: -1%;
            width: 100%;
        }

        .loader {
            border: 16px solid #f3f3f3;
            border-radius: 50%;
            border-top: 16px solid #3498db;
            width: 120px;
            height: 120px;
            -webkit-animation: spin 2s linear infinite;
            animation: spin 2s linear infinite;
            left: 43%;
            position: relative;
            top: 43%
        }

        @-webkit-keyframes spin {
            0% {
                -webkit-transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
            }
        }

        @keyframes spin {
            0% {
                transform: rotate(0deg);
            }

            100% {
                transform: rotate(360deg);
            }
        }

        .txt_width {
            width: 85%;
        }
    </style>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>
    <script type="text/javascript" language="javascript">
        function isNumberKeydouble(evt) {
            //debugger;
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                if (charCode == 46) {
                    return true;
                }
                return false;
            }
            return true;
        }
    </script>
    <table style="width: 100%; margin: 0; padding: 0;">
        <tr>
            <td colspan="12" align="center" class="head_column">
                <h3>Retail COD Customer</h3>
            </td>
        </tr>
    </table>

    <div style="float: left; width: 100%">
        <fieldset style="/*! float: left; */ width: 70%; margin: 0 40px;">
            <legend>Basic Info</legend>
            <table style="width: 100%; margin: 0; padding: 0;">
                <tr>
                    <td class="lable">Customer Name</td>
                    <td>
                        <asp:TextBox ID="txt_customername" runat="server" CssClass="txt_width"></asp:TextBox>
                    </td>

                    <td class="lable">Mobile Number</td>
                    <td>
                        <asp:TextBox ID="txt_mobile" runat="server" CssClass="txt_width" onkeypress="return isNumberKeydouble();" AutoPostBack="true" OnTextChanged="txt_mobile_TextChanged"
                            onchange="changeFunction()" MaxLength="11" placeholder="XXXX-XXXXXXX"></asp:TextBox>
                        <script type="text/javascript" language="javascript">
                            function changeFunction() {
                                var num = document.getElementById("<%= txt_mobile.ClientID %>").value.length;

                                if (num < 11) {
                                    document.getElementById("<%= txt_mobile.ClientID %>").value = "";
                                    alert("Length of Mobile Number is not Correct.");
                                }
                                if (num > 11) {
                                    document.getElementById("<%= txt_mobile.ClientID %>").value = "";
                                    alert("Length of Mobile Number is not Correct.");
                                }
                            }
                            /*
                            $('#ContentPlaceHolder1_txt_mobile').keyup(function () {
                                var val = this.value.replace(/\D/g, '');
                                var newVal = '';
                                for (i = 0; i < 1; i++) {
                                    if (val.length > 4) {
                                        newVal += val.substr(0, 4) + '-';
                                        val = val.substr(4);
                                    }
                                }
                                newVal += val;
                                this.value = newVal;
                            });*/
                        </script>
                    </td>
                </tr>
                <tr>
                    <td class="lable">Email Id</td>
                    <td>
                        <asp:TextBox ID="txt_email" runat="server" CssClass="txt_width"></asp:TextBox>
                    </td>

                    <td class="lable">CNIC Number	</td>
                    <td>
                        <asp:TextBox ID="txt_cnic" runat="server" CssClass="txt_width" MaxLength="13" placeholder="XXXXX-XXXXXXX-X"
                            AutoPostBack="true" onkeypress="return isNumberKeydouble();" OnTextChanged="txt_cnic_TextChanged" onchange="changeFunctionNIC()"></asp:TextBox>

                        <script type="text/javascript" language="javascript">
                            function changeFunctionNIC() {
                                debugger;
                                var num = document.getElementById("<%= txt_cnic.ClientID %>").value.length;

                                if (num < 13) {
                                    document.getElementById("<%= txt_cnic.ClientID %>").value = "";
                                    alert("Length of CNIC Number is not Correct.");
                                }
                                if (num > 13) {
                                    document.getElementById("<%= txt_cnic.ClientID %>").value = "";
                                    alert("Length of CNIC Number is not Correct.");
                                }
                            }

                            /* $('#ContentPlaceHolder1_txt_cnic').keyup(function () {
                                 debugger;
                                 var val = this.value.replace(/\D/g, '');
                                 var newVal = '';
                                 for (i = 0; i < 1; i++) {
                                     if (val.length > 5) {
                                         newVal += val.substr(0, 5) + '-';
                                         val = val.substr(5);
                                     }
                                     if (val.length > 11) {
                                         newVal += val.substr(0, 1) + '-';
                                         val = val.substr(1);
                                     }
                                 }
                                 newVal += val;
                                 this.value = newVal;
                             }); */
                            /*
                            function autogen(evt) {
                                debugger;
                                evt = (evt) ? evt : window.event;
                                var charCode = (evt.which) ? evt.which : evt.keyCode;
                                var t = document.getElementById("ContentPlaceHolder1_txt_cnic").value + String.fromCharCode(charCode);
                                if (t.length <= 15) {
                                    //if ((t.length % 5 == 0 || t.length >= 15) && t.charAt(t.length - 0) != "-") {
                                        if ((t.length % 5 == 0)) {
                                        document.getElementById("ContentPlaceHolder1_txt_cnic").value = t.toUpperCase() + "-";
                                    }
                                    else {
                                        document.getElementById("ContentPlaceHolder1_txt_cnic").value = t.toUpperCase();
                                    }
                                }
                                return false;
                            }*/
                        </script>
                    </td>
                </tr>
                <tr>
                    <td class="lable">Address</td>
                    <td colspan="3">
                        <asp:TextBox ID="txt_address" runat="server" Columns="77" Rows="2" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </fieldset>

        <fieldset style="float: left; width: 70%; margin: 0 40px;">
            <legend>Bank Info</legend>
            <table style="width: 100%; margin: 0; padding: 0;">
                <tr>
                    <td class="lable">Bank Name</td>
                    <td width="37%">
                        <asp:DropDownList ID="dd_bank" runat="server" AppendDataBoundItems="true" CssClass="txt_width">
                            <asp:ListItem Value="0">Select Bank</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="lable">Bank Branch Name</td>
                    <td>
                        <asp:TextBox ID="txt_bankbranchname" runat="server" CssClass="txt_width"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="lable">Bank Branch Code</td>
                    <td>
                        <asp:TextBox ID="txt_bankbranchcode" runat="server" CssClass="txt_width"></asp:TextBox>
                    </td>

                    <td class="lable">Account Title</td>
                    <td>
                        <asp:TextBox ID="txt_accounttitle" runat="server" CssClass="txt_width"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <%--<td class="lable">Account Number</td>
                    <td>
                        <asp:TextBox ID="txt_accountno" runat="server" CssClass="txt_width"></asp:TextBox>
                    </td>--%>

                    <td class="lable">IBAN Number</td>
                    <td>
                        <asp:TextBox ID="txt_ibft" runat="server" CssClass="txt_width" MaxLength="24" onkeypress="return isNumberKeydouble__();" onchange="changeFunctionIBAN()"></asp:TextBox>
                        <script type="text/javascript" language="javascript">

                            function changeFunctionIBAN() {
                                debugger;
                                var num = document.getElementById("<%= txt_ibft.ClientID %>").value.length;

                                if (num < 24) {
                                    document.getElementById("<%= txt_ibft.ClientID %>").value = "";
                                    alert("Length of IBAN Number is not Correct.");
                                }
                                if (num > 24) {
                                    document.getElementById("<%= txt_ibft.ClientID %>").value = "";
                                    alert("Length of IBAN Number is not Correct.");
                                }
                            }
                            
                            function isNumberKeydouble__() {
                                var regex = new RegExp("^[a-zA-Z0-9]+$");
                                var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
                                if (!regex.test(key)) {
                                    event.preventDefault();
                                    return false;
                                }
                            }
                        </script>
                    </td>
                </tr>
                <%--<tr>
                    <td class="lable">NTN No</td>
                    <td>
                        <asp:TextBox ID="txt_ntn" runat="server" CssClass="txt_width"></asp:TextBox>
                    </td>
                </tr>--%>
            </table>
        </fieldset>
    </div>
    <asp:Label ID="ErrorLbl" runat="server" Style="color: red; font-size: 16px; float: left"></asp:Label>
    <br />
    <div style="margin: 10px 40px; float: left;">

        <asp:Button ID="btn" runat="server" Text="Save Data" CssClass="button" OnClick="Btn_Save" OnClientClick="javascript:document.getElementById('ContentPlaceHolder1_loaders').style.display = 'block';" />
    </div>
    <div id="loaders" runat="server" class="outer_box" style="display: none;">
        <div id="loader" runat="server" class="loader">
        </div>
    </div>
</asp:Content>
