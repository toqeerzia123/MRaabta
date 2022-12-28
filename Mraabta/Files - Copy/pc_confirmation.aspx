<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="pc_confirmation.aspx.cs" Inherits="MRaabta.Files.pc_confirmation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
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
        <style>
            .search
            {
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
            
            .search a
            {
                color: #fff;
                text-decoration: none;
            }
            .width
            {
            }
            .style1
            {
                text-align: left;
                width: 688px;
            }
        </style>
        <table class="input-form" style="width: 95%;">
            <tr>
                <td class="field">
                    <h3 class="style1">
                        Confirmation:
                    </h3>
                </td>
            </tr>
            <tr >
                <td>
                    <asp:Label ID="lbl_message" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
            <td colspan="2">
              <asp:Button ID="btn" Visible="false" runat="server" Text="Click To View & Edit" 
                    Width="130" CssClass="button1" onclick="btn_Click"
                         />
                         &nbsp;
                          <asp:Button ID="Button1" Visible="false" runat="server" Text="Add Next Voucher" 
                    Width="130" CssClass="button1" onclick="btn_nextClick"
                         />
            </td>
            </tr>
        </table>
    </div>
</asp:Content>
