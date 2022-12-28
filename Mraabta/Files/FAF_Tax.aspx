<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="FAF_Tax.aspx.cs" Inherits="MRaabta.Files.FAF_Tax" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function isNumberKey(evt) {

            var count = 1;
            var charCode = (evt.which) ? evt.which : event.keyCode
            if ((charCode != 46 && charCode > 31)
                && (charCode < 48 || charCode > 57 || charCode == 110 || charCode == 109 || charCode == 45 || charCode == 9)) {
                return false;
            }
            else {

                if (charCode == 110) {
                    count++;
                }
                if (count > 1) {
                    return false;
                }
            }

            return true;
        }

        function DecimalNumber(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 46)
                return false;
            else {
                var len = document.getElementById('<%=txt_Tax.ClientID%>').value.length;
                var index = document.getElementById('<%=txt_Tax.ClientID%>').value.indexOf('.');
                if (len == 0 && (index < 0 || index > 0) && charCode == 46) {
                    return false;
                }
                if (index < 0 && len > 1) {
                    if ((charCode > 48 || charCode < 57) && charCode != 46) {
                        return false;
                    }
                    else if (index < 0 && charCode == 46) {
                        return true;
                    }
                }
                if (index > 0 && charCode == 46) {
                    return false;
                }
                if (index > 0) {
                    var CharAfterdot = (len + 1) - index;
                    if (CharAfterdot > 4) {
                        return false;
                    }
                }

            }
            return true;
        }

        function NumberWithHyphen(evt) {

            var count = 1;
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 45 && charCode > 31
                && (charCode < 48 || charCode > 57 || charCode == 110 || charCode == 109 || charCode == 9)) {
                return false;
            }
            else {
                if (charCode == 110) {
                    count++;
                }
                if (count > 1) {
                    return false;
                }
            }

            return true;
        }

        function onlyAlphabets(e, t) {
            var count = 1;
            var charCode = (e.which) ? e.which : event.keyCode

            if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || (charCode == 32)) {
                count++;
            }
            else {
                if (count == 1) {
                    return false;
                }
            }
            return true;
        }

        function ClearAll() {
            var Service_Type = document.getElementById('<%=dd_ServiceType.ClientID %>');
            var Tax = document.getElementById('<%=txt_Tax.ClientID %>');
            var Start_Date = document.getElementById('<%=dd_From_Date.ClientID %>');
            var End_Date = document.getElementById('<%=dd_To_Date.ClientID %>');
            var Message = document.getElementById('<%=lbl_Message.ClientID %>');

            Service_Type.value = "0";
            Tax.value = null;
            Start_Date.value = null;
            End_Date.value = null;
            //Message.textContent = null;
        }

    </script>
    <br />
    <style>
        body {
            font-family: Calibri;
        }

        ul.tab {
            list-style-type: none;
            margin: 0;
            padding: 0;
            overflow: hidden;
            border: 1px solid #ccc;
            background-color: #f1f1f1;
        }

            /* Float the list items side by side */
            ul.tab li {
                float: left;
            }

                /* Style the links inside the list items */
                ul.tab li a {
                    display: inline-block;
                    color: black;
                    text-align: center;
                    padding: 14px 16px;
                    text-decoration: none;
                    transition: 0.3s;
                    font-size: 17px;
                }

                    /* Change background color of links on hover */
                    ul.tab li a:hover {
                        background-color: #ddd;
                    }

                    /* Create an active/current tablink class */
                    ul.tab li a:focus, .active {
                        background-color: #ccc;
                    }

        /* Style the tab content */
        .tabcontent {
            display: none;
            padding: 6px 12px;
            border: 1px solid #ccc;
            border-top: none;
        }
    </style>
    <div id="BasicInfo">
        <fieldset>
            <legend></legend>
            <asp:Panel ID="panel1" runat="server">
                <table style="font-family: Calibri; font-size: medium; padding-bottom: 0px !important; padding-top: 0px !important; width: 97%"
                    class="input-form">
                    <tr style="text-align: center;">
                        <center style="font-size: large; font-weight: bold;">
                            FUEL ADJUSTMENT FACTOR (FAF Percentage)
                        </center>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_Message" runat="server" Font-Size="Medium"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
                        <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 45px!important;">Product
                        </td>
                        <td class="input-field" style="width: 18% !important;">
                            <asp:DropDownList ID="dd_ServiceType" runat="server" AppendDataBoundItems="true" Width="100%">
                                <asp:ListItem Value="0">Select Product</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="space" style="width: 3% !important; margin: 0px 0px 0px 0px !important;"></td>
                        <td class="field" style="width: 4% !important; text-align: right !important; padding-right: 62px!important;">FAF
                        </td>
                        <td class="input-field" style="width: 12% !important;">
                            <asp:TextBox ID="txt_Tax" runat="server" MaxLength="7" autocomplete="off" onkeypress="return DecimalNumber(event);"></asp:TextBox>
                        </td>
                        <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
                    </tr>
                    <tr>
                        <td class="space" style="width: 6% !important; margin: 0px 0px 0px 0px !important;"></td>
                        <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px!important;">Effective From
                        </td>
                        <td class="input-field" style="width: 17% !important;">
                            <asp:TextBox ID="dd_From_Date" runat="server" autocomplete="off" onkeypress="return NumberWithHyphen(event);" MaxLength="10"></asp:TextBox>
                            <Ajax1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="dd_From_Date"
                                Format="yyyy-MM-dd"></Ajax1:CalendarExtender>
                        </td>
                        <td class="space" style="width: 3% !important; margin: 0px 0px 0px 0px !important;"></td>
                        <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 10px!important;">Effective To
                        </td>
                        <td class="input-field" style="width: 17% !important;">
                            <asp:TextBox ID="dd_To_Date" runat="server" autocomplete="off" onkeypress="return NumberWithHyphen(event);" MaxLength="10"></asp:TextBox>
                            <Ajax1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="dd_To_Date"
                                Format="yyyy-MM-dd"></Ajax1:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="field" style="width: 30% !important; text-align: right !important; padding-right: 5px!important;"></td>
                        <td class="input-field" style="width: 40% !important;" colspan="2">
                            <asp:Button ID="btn_reset" runat="server" Text="Reset" CssClass="button" OnClientClick="ClearAll();"
                                Width="80px" />
                            &nbsp;
                            <asp:Button ID="btn_save" runat="server" Text="Save" CssClass="button" OnClick="btn_save_Click"
                                Width="80px" />
                        </td>
                        <asp:HiddenField ID="hf_Status" runat="server" />
                    </tr>
                </table>
            </asp:Panel>
            <div style="height: 25px;">
                <asp:ImageButton ID="CSV_btn" Visible="true" runat="server" Style="float: right" ImageUrl="~/images/ExcelFile.png"
                    OnClick="CSV_btn_onclick" Width="40px" ToolTip="Get Downloadable Data"></asp:ImageButton>
            </div>
            <br />
            <fieldset id="FAF_Tax_DBT_Details" runat="server">
                <legend>FAF Tax Details</legend>
                <div runat="server" style="width: 60%; padding-left: 1%; font-weight: bold; font-size: medium;">
                    <asp:RadioButtonList ID="RBL_Status" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="RBL_Status_SelectedIndexChanged" CssClass="input-field">
                        <asp:ListItem Text="Active" Value="1" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="In Active" Value="0"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
                <br />
                <br />
                <asp:Panel ID="CC_Panel" runat="server" ScrollBars="Both">
                    <asp:Repeater runat="server" ID="rp_FAF_Tax_DBT" OnItemDataBound="rp_FAF_Tax_DBT_ItemDataBound">
                        <HeaderTemplate>
                            <table class="mGrid">
                                <tr>
                                    <th>S. No.
                                    </th>
                                    <th style="display: none">FAF Tax ID
                                    </th>
                                    <th style="display: none">Service Type ID
                                    </th>
                                    <th>Product
                                    </th>
                                    <th>Effective From
                                    </th>
                                    <th>Effective To
                                    </th>
                                    <th>Tax
                                    </th>
                                    <th>Status
                                    </th>
                                    <th>Created By
                                    </th>
                                    <th>Created On
                                    </th>
                                    <th>Modified By
                                    </th>
                                    <th>Modified On
                                    </th>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td style="text-align: center">
                                    <asp:Label ID="lblRowNumber" runat="server"></asp:Label>
                                </td>
                                <td style="text-align: center; display: none">
                                    <asp:HiddenField ID="FAF_Tax_ID" runat="server" Value='<%# DataBinder.Eval(Container, "DataItem.FAF_Tax_ID") %>'></asp:HiddenField>
                                </td>
                                <td style="text-align: center; display: none">
                                    <asp:HiddenField ID="ServiceType_ID" runat="server" Value='<%# DataBinder.Eval(Container, "DataItem.ServiceTypeID") %>'></asp:HiddenField>
                                </td>
                                <td style="text-align: center">
                                    <asp:Label ID="ServiceType" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ServiceType") %>'></asp:Label>
                                </td>
                                <td style="text-align: center">
                                    <asp:Label ID="EffectiveFrom" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.EffectiveFrom") %>'></asp:Label>
                                </td>
                                <td style="text-align: center">
                                    <asp:Label ID="EffectiveTo" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.EffectiveTo") %>'></asp:Label>
                                </td>
                                <td style="text-align: center">
                                    <asp:Label ID="Tax" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Tax") %>'></asp:Label>
                                </td>
                                <td style="text-align: center">
                                    <asp:Label ID="Status" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Status") %>'></asp:Label>
                                </td>
                                <td style="text-align: center">
                                    <asp:Label ID="CreatedBy" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CreatedBy") %>'></asp:Label>
                                </td>
                                <td style="text-align: center">
                                    <asp:Label ID="CreatedOn" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CreatedOn") %>'></asp:Label>
                                </td>
                                <td style="text-align: center">
                                    <asp:Label ID="ModifiedBy" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ModifiedBy") %>'></asp:Label>
                                </td>
                                <td style="text-align: center">
                                    <asp:Label ID="ModifiedOn" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ModifiedOn") %>'></asp:Label>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </asp:Panel>
            </fieldset>
        </fieldset>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
