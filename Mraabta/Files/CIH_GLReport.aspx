<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="CIH_GLReport.aspx.cs" Inherits="MRaabta.Files.CIH_GLReport" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript">
        function AddValidation() {

            if (document.getElementById("<%=txt_frmdate.ClientID%>").value == "") {
                alert("Please Select From Date");
                return false;
            }

            if (document.getElementById("<%=txt_todate.ClientID%>").value == "") {
                alert("Please Select To Date");
                return false;

            } 
            return true;
        }
    </script>
    <style>
        .chkbox {
        }
    </style>
    <div>
        <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
            <tr>
                <td colspan="12" align="center" class="head_column">
                    <h3>General Ledger
                    </h3>
                </td>
            </tr>
        </table>
        <style>
            .input-form tr {
                float: none;
                margin: 0 0 10px;
                width: 100%;
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


            .pop_div {
                background: #eee none repeat scroll 0 0;
                border-radius: 10px;
                height: 100px;
                left: 48%;
                position: relative;
                top: 40%;
                width: 257px;
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

            .outer_box img {
                left: 42%;
                position: relative;
                top: 40%;
            }
        </style>
        <style>
            .search {
                float: right;
                width: 10%;
                background: #5f5a8d;
                padding: 3px;
                position: relative;
                right: 99px;
                margin: 0px 0px 15px;
                top: 7px;
                text-align: center;
            }

                .search a {
                    color: #fff;
                    text-decoration: none;
                }

            .width {
            }
        </style>
        <div id="div2" runat="server" class="outer_box" style="display: none; background: #FFFFFF none repeat scroll 0 0;">
            <img src="../images/Loading_Movie-02.gif" />
        </div>
        <table class="input-form" style="width: 95%;">
            <tr>
                <td class="field">From Date:
                </td>
                <td class="input-field">
                    <asp:TextBox ID="txt_frmdate" runat="server" CssClass="med-field" MaxLength="10"
                        Width="190"></asp:TextBox>
                    <Ajax1:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_frmdate" runat="server"
                        Format="dd/MM/yyyy" PopupButtonID="Image1"></Ajax1:CalendarExtender>
                </td>
                <td class="field"></td>
                <td class="field">To Date
                </td>
                <td class="input-field">
                    <asp:TextBox ID="txt_todate" runat="server" CssClass="med-field" MaxLength="10" Width="190"></asp:TextBox>
                    <Ajax1:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_todate" runat="server"
                        Format="dd/MM/yyyy" PopupButtonID="Image1"></Ajax1:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td class="field">Zone:
                </td>
                <td class="input-field">
                    <asp:DropDownList ID="dd_zone" runat="server" CssClass="dropdown width" Width="200px"
                        AutoPostBack="true" OnSelectedIndexChanged="dd_zone_Changed">
                    </asp:DropDownList>
                </td>
                <td class="field"></td>
                <td class="field">Branch:
                </td>
                <td class="input-field">

                    <asp:ListBox ID="dd_branch" runat="server" SelectionMode="Multiple" CssClass="dropdown width" Width="200"></asp:ListBox>
                    <br />
                    All<asp:CheckBox runat="server" ID="branchAllChkbox" CssClass="chkbox" OnCheckedChanged="branchAllChkbox_CheckedChanged" AutoPostBack="true" />
                </td>
                <td class="field"></td>

                <td colspan="2">
                    <asp:Button ID="btn_add" runat="server" Text="Generate Data" Width="130" CssClass="button1"
                        OnClick="btn_add_Click" OnClientClick="return AddValidation();" />
                    <br /><br />
                    <asp:Button ID="btn_addhbl" runat="server" Text="Generate Data (With HBL Konnect)" Width="200" CssClass="button1"
                        OnClick="btn_addhbl_Click" OnClientClick="return AddValidation();" />
                </td>
            </tr>
            <tr>
                <td class="field">Company:
                </td>
                <td class="input-field">
                    <asp:DropDownList ID="dd_company" runat="server" CssClass="dropdown width" Width="200">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="field">
                    <%--  Express Center:--%>
                </td>
                <td class="input-field">
                    <%-- <asp:ListBox  SelectionMode="Multiple"  ID="dd_ec" runat="server" CssClass=" dropdown width" Width="200" Rows="4"></asp:ListBox>
                    --%>
                </td>
            </tr>
            <tr>
                <td colspan="6" valign="middle">
                    <div style="text-align: center;">
                        <asp:Label runat="server" ID="lbl_error" Style="color: #333333; font-size: medium; font-weight: 700; text-align: center"></asp:Label>
                    </div>
                </td>
            </tr>
        </table>
        <span id="Span1" class="tbl-large">
            <%--<asp:GridView ID="GridView" runat="server" AutoGenerateColumns="false" CssClass="mGrid"
                AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None" AllowPaging="false"
                BorderWidth="1px" OnRowCommand="GridView1_RowCommand">
                <Columns>
                    <asp:BoundField HeaderText="S.No" DataField="SerialNo" ItemStyle-HorizontalAlign="left"
                        ItemStyle-Width="20%"></asp:BoundField>
                    <asp:BoundField HeaderText="Branch" DataField="branch" ItemStyle-HorizontalAlign="left"
                        ItemStyle-Width="30%"></asp:BoundField>
                    <%--   <asp:BoundField HeaderText="Express Center" DataField="ec" ItemStyle-HorizontalAlign="left"
                        ItemStyle-Width="30%"></asp:BoundField>--%>
            <%--   <asp:BoundField HeaderText="Transactions Count" DataField="entries" ItemStyle-HorizontalAlign="right"
                        ItemStyle-Width="20%"></asp:BoundField>
                    <asp:ButtonField ButtonType="Button" Text="View & Print" ItemStyle-Width="20%" runat="server"
                        CommandName="view" />
                </Columns>
            </asp:GridView>--%>
    </div>
</asp:Content>
