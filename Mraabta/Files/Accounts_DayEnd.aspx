<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="Accounts_DayEnd.aspx.cs" Inherits="MRaabta.Files.Accounts_DayEnd" %>

<%@ Register Namespace="AjaxControlToolkit" TagPrefix="Ajax" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script>
        function altufaltu() {
            debugger;
            document.getElementById('<%= div1.ClientID %>').style.visibility = "hidden"

            return false;
        }
    </script>
    <style>
        .tableFull {
            width: 100%;
            font-family: Calibri;
        }

        .textField {
            font-weight: bold;
            text-align: right;
        }

        .tableFull tr {
            margin-bottom: 10px;
        }
    </style>
    <style>
        .outer_box {
            background: #444 none repeat scroll 0 0;
            height: 101%;
            left: 0;
            opacity: 0.8;
            position: absolute;
            top: -1%;
            width: 100%;
        }


        .pop_div {
            background: #eee none repeat scroll 0 0;
            border-radius: 10px;
            height: 110px;
            left: 48%;
            position: relative;
            top: 40%;
            width: 300px;
            opacity: 1;
        }

        .btn_ok {
            background: #000 none repeat scroll 0 0;
            border: 0 none;
            color: #fff;
            left: -18px;
            padding: 1px 14px;
            position: relative;
            top: 67%;
        }

        .btn_cancel {
            background: #000 none repeat scroll 0 0;
            border: 0 none;
            color: #fff;
            left: 22%;
            padding: 1px 14px;
            position: relative;
            top: 42%;
        }

        .pop_div > span {
            float: left;
            line-height: 40px;
            text-align: center;
            width: 100%;
        }

        .tbl-large div {
            position: static;
        }
    </style>
    <div id="div1" runat="server" class="outer_box" style="display: none;">
        <div class="pop_div">
            <table style="width: 100% !important;">
                <tr width="100%">
                    <td style="float: left; margin-top: 12px; text-align: center; width: 290px;">
                        <asp:Label ID="lbl_error2" runat="server" Text="No Previous Day Ends found for selected Documents.
            Day End Would be Performed for selected Date.
            Do you want to Proceed."></asp:Label>
                    </td>
                </tr>
                <tr width="100%">
                    <td style="float: left; margin-left: 50px; margin-top: 8px; text-align: center !important;">
                        <input type="button" id="btn_cancel2" value="Cancel" class="button" onclick="altufaltu();" />
                    </td>
                    <td style="float: left; margin-top: 8px; text-align: center !important; width: 50% !important;">
                        <asp:Button ID="btn_ok2" runat="server" Text="OK" CssClass="button"
                            onclick="btn_ok2_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <table class="tableFull">
        <tr>
            <td style="width: 10%"></td>
            <td style="width: 80%; text-align: center; font-variant: small-caps; font-size: larger;"
                colspan="6">
                <b>Accounts Day End</b>
            </td>
            <td style="width: 10%"></td>
        </tr>
        <tr>
            <td style="width: 20%"></td>
            <td class="textField" style="width: 10%">Zone
            </td>
            <td class="controlField" style="width: 10%">
                <asp:DropDownList ID="dd_zone" runat="server" Style="width: 100%" AutoPostBack="true"
                    OnSelectedIndexChanged="dd_zone_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td class="textField" style="width: 10%">Date
            </td>
            <td class="controlField" style="width: 10%">
                <asp:TextBox ID="txt_date" runat="server"></asp:TextBox>
                <Ajax:CalendarExtender ID="calendar1" runat="server" TargetControlID="txt_date" Format="yyyy-MM-dd"></Ajax:CalendarExtender>
            </td>
            <td class="textField" style="width: 10%">Doc Type
            </td>
            <td class="controlField" style="width: 10%">
                <asp:DropDownList ID="dd_docType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="dd_docType_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td style="width: 20%"></td>
        </tr>
        <tr>
            <td style="width: 20%"></td>
            <td class="textField" style="width: 10%"></td>
            <td class="controlField" style="width: 10%"></td>
            <td class="textField" style="width: 20%" colspan="2">
                <br />
                <br />
                <asp:Button ID="btn_dayEnd" runat="server" Text="DAY END" CssClass="button" Width="100%"
                    OnClick="btn_dayEnd_Click1" OnClientClick="return(confirm('Are you sure you want to perform Day Edn'));" />
            </td>
            <td class="textField" style="width: 10%"></td>
            <td class="controlField" style="width: 10%"></td>
            <td style="width: 20%"></td>
        </tr>
    </table>
    <table class="tableFull">
        <tr>
            <td style="width: 10%"></td>
            <td style="width: 80%; text-align: center; font-variant: small-caps; font-size: larger;"
                colspan="6">
                <br />
                <br />
                <b>Previous Day End</b>
            </td>
            <td style="width: 10%"></td>
        </tr>
        <tr>
            <td style="width: 20%"></td>
            <td class="textField" style="width: 10%">Year
            </td>
            <td style="width: 20%" colspan="2">
                <asp:DropDownList ID="dd_year" runat="server" AutoPostBack="true" OnSelectedIndexChanged="dd_year_SelectedIndexChanged"
                    AppendDataBoundItems="true">
                    <asp:ListItem Value="0">Select Year</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="textField" style="width: 10%">Month
            </td>
            <td style="width: 20%" colspan="2">
                <asp:DropDownList ID="dd_month" runat="server" AutoPostBack="true" OnSelectedIndexChanged="dd_month_SelectedIndexChanged">
                    <asp:ListItem Value="0">Select Month</asp:ListItem>
                    <asp:ListItem Value="01">January</asp:ListItem>
                    <asp:ListItem Value="02">February</asp:ListItem>
                    <asp:ListItem Value="03">March</asp:ListItem>
                    <asp:ListItem Value="04">April</asp:ListItem>
                    <asp:ListItem Value="05">May</asp:ListItem>
                    <asp:ListItem Value="06">June</asp:ListItem>
                    <asp:ListItem Value="07">July</asp:ListItem>
                    <asp:ListItem Value="08">August</asp:ListItem>
                    <asp:ListItem Value="09">September</asp:ListItem>
                    <asp:ListItem Value="10">October</asp:ListItem>
                    <asp:ListItem Value="11">November</asp:ListItem>
                    <asp:ListItem Value="12">December</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="width: 20%"></td>
        </tr>
        <tr>
            <td style="width: 10%"></td>
            <td style="width: 80%; text-align: center; padding-left: 10%; padding-right: 10%;"
                colspan="6">
                <asp:GridView ID="gv_dayends" runat="server" Width="100%" AutoGenerateColumns="false"
                    EmptyDataText="No Data Available">
                    <columns>
                        <asp:BoundField HeaderText="Zone" DataField="ZoneName" />
                        <asp:BoundField HeaderText="Document Type" DataField="Document" />
                        <asp:BoundField HeaderText="Date" DataField="DayEndDate" />
                    </columns>
                </asp:GridView>
            </td>
            <td style="width: 10%"></td>
        </tr>
    </table>
</asp:Content>
