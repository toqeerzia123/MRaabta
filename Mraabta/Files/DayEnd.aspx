<%@ Page Title="" Language="C#" MasterPageFile="~/BtsMasterPage.Master" AutoEventWireup="true" CodeBehind="DayEnd.aspx.cs" Inherits="MRaabta.Files.DayEnd" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script>
        function Loader() {
            document.getElementById('<%=div2.ClientID %>').style.display = "block !important";
            this.disabled = true;
            this.value = 'Please Wait...';
        }
    </script>
    <style>
        .input-form tr
        {
            float: none;
            margin: 0 0 10px;
            width: 100%;
        }
        .outer_box
        {
            background: #444 none repeat scroll 0 0;
            height: 101%;
            left: 0;
            opacity: 0.9;
            position: absolute;
            top: -1%;
            width: 100%;
        }
        
        
        .pop_div
        {
            background: #eee none repeat scroll 0 0;
            border-radius: 10px;
            height: 100px;
            left: 48%;
            position: relative;
            top: 40%;
            width: 257px;
        }
        
        .btn_ok
        {
            background: #000 none repeat scroll 0 0;
            border: 0 none;
            color: #fff;
            left: -18px;
            padding: 1px 14px;
            position: relative;
            top: 67%;
        }
        
        .btn_cancel
        {
            background: #000 none repeat scroll 0 0;
            border: 0 none;
            color: #fff;
            left: 22%;
            padding: 1px 14px;
            position: relative;
            top: 42%;
        }
        
        .pop_div > span
        {
            float: left;
            line-height: 40px;
            text-align: center;
            width: 100%;
        }
        .tbl-large div
        {
            position: static;
        }
        
        .outer_box img
        {
            left: 42%;
            position: relative;
            top: 40%;
        }
    </style>
    <div id="div2" runat="server" class="outer_box" style="display: none; background: #FFFFFF none repeat scroll 0 0;">
        <img src="../images/Loading_Movie-02.gif" />
    </div>
    <div id="divDialogue" runat="server" class="outer_box" style="display: none;">
        <div class="pop_div">
            <table style="width: 100% !important;">
                <tr width="100%">
                    <td style="float: left; margin-top: 12px; text-align: center; width: 228px;">
                        <asp:Label ID="lbl_error" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr width="100%">
                    <td style="float: left; margin-left: 30px; margin-top: 8px; text-align: center !important;">
                        <asp:Button ID="btn_cancelDialogue" runat="server" Text="Cancel" CssClass="button"
                            OnClick="btn_cancelDialogue_Click" />
                    </td>
                    <td style="float: left; margin-top: 8px; text-align: center !important; width: 50% !important;">
                        <asp:Button ID="btn_okDialogue" runat="server" Text="OK" CssClass="button" OnClick="btn_okDialogue_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="div1" runat="server" class="outer_box" style="display: none;">
        <div class="pop_div">
            <table style="width: 100% !important;">
                <tr width="100%">
                    <td style="float: left; margin-top: 12px; text-align: center; width: 228px;">
                        <asp:Label ID="lbl_error2" runat="server" Text="Some CNs are short received. Click OK to Continue."></asp:Label>
                    </td>
                </tr>
                <tr width="100%">
                    <td style="float: left; margin-left: 30px; margin-top: 8px; text-align: center !important;">
                        <asp:Button ID="btn_cancel2" runat="server" Text="Cancel" CssClass="button" OnClick="btn_cancel2Dialogue_Click" />
                    </td>
                    <td style="float: left; margin-top: 8px; text-align: center !important; width: 50% !important;">
                        <asp:Button ID="btn_ok2" runat="server" Text="OK" CssClass="button" OnClick="btn_ok2Dialogue_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <table style="font-family: Calibri; font-size: medium; padding-bottom: 0px !important;
        padding-top: 0px !important" class="input-form">
        <tr>
            <td colspan="4">
                <asp:Label ID="Errorid" runat="server" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="field">
                Date
            </td>
            <td class="field">
                <asp:TextBox ID="txt_date" runat="server" Width="100px"></asp:TextBox>
                <Ajax1:CalendarExtender ID="calendar1" runat="server" Format="dd-MM-yyyy" TargetControlID="txt_date">
                </Ajax1:CalendarExtender>
            </td>
            <td class="space">
            </td>
            <td class="input-field">
                <asp:Button ID="btn_search" runat="server" Text="Search" CssClass="button" Width="100px"
                    OnClick="btn_search_Click" />
                &nbsp;
                <asp:Button ID="btn_dayEnd" runat="server" CssClass="button" Text="DAY END" OnClick="btn_dayEnd_Click"
                    OnClientClick="Loader()" UseSubmitBehavior="false" Width="100px" />
            </td>
        </tr>
        <tr>
            <td colspan="4" style="width: 100%">
                <span class="tbl-large">
                    <asp:PlaceHolder ID="ph1" runat="server"></asp:PlaceHolder>
                </span>
            </td>
        </tr>
    </table>
</asp:Content>
