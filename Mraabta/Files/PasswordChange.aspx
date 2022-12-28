<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.master" CodeBehind="PasswordChange.aspx.cs" Inherits="MRaabta.Files.PasswordChange" %>


<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
        <tr>
            <td colspan="12" align="center" class="head_column">
                <h3> Password Change </h3>
            </td>
        </tr>            
    </table>

    <asp:Label ID="error" runat="server" CssClass="error_msg"></asp:Label>

    <table class="input-form" style="width:97%">
        <tr>
            <td class="field">Name: </td>
            <td class="input-field">
                <asp:Label ID="txt_name" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="field">Salary Code: </td>
            <td class="input-field">
                <asp:Label ID="txt_salary_code" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="field">Last Date Change: </td>
            <td class="input-field">
                <asp:Label ID="txt_last_date" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="field">Password Expire Date: </td>
            <td class="input-field">
                <asp:Label ID="txt_expire_date" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="field">Old Password: </td>
            <td class="input-field">
                <asp:TextBox ID="txt_old_password" TextMode="Password" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="field">New Password: </td>
            <td class="input-field">
                <asp:TextBox ID="txt_new_password" TextMode="Password" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="field">
                <asp:Button ID="btn_save" runat="server" Text="Save" CssClass="button1" OnClick="btn_save_Click" />  
            </td>
        </tr>
    </table> 

</asp:Content>
