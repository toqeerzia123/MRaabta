<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="Staff_entry.aspx.cs" Inherits="MRaabta.Files.Staff_entry" %>
<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div>
        <table>
            <tr>
                <td><b>Staff Code</b></td>
                <td style="padding-top:1em;padding-left:1em""><asp:TextBox runat ="server" ID="txt_stfcode"></asp:TextBox></td>
                <td><asp:Label runat="server" ID="l1"></asp:Label></td>
            </tr>
            <tr>
                <td><b>Staff Name</b></td>
            <td style="padding-top:1em;padding-left:1em""><asp:TextBox runat="server" ID="txt_stfname"></asp:TextBox></td>
                <td><asp:Label runat="server" ID="Label1"></asp:Label></td>
            </tr>
            <tr>
                <td><b>Designation</b></td>
            <td style="padding-top:1em;padding-left:1em""><asp:TextBox runat="server" ID="txt_designation"></asp:TextBox></td>
                <td><asp:Label runat="server" ID="Label6"></asp:Label></td>
            </tr>
            <tr>
                <td><b>CNIC Number</b></td>
            <td style="padding-top:1em;padding-left:1em""><asp:TextBox runat="server" ID="txt_cnic"></asp:TextBox></td>
                <td><asp:Label runat="server" ID="Label2"></asp:Label></td>
            </tr>
        <tr>
            <td><b>Phone No.</b></td>
        <td style="padding-top:1em;padding-left:1em""><asp:TextBox runat="server" ID="txt_phone"></asp:TextBox></td>
            <td><asp:Label runat="server" ID="Label3"></asp:Label></td>
        </tr>
            <tr>
                <td><b>Zone</b></td>
                <td style="padding-top:1em;padding-left:1em""><asp:DropDownList runat="server" ID="dd_zone" AutoPostBack="true" OnSelectedIndexChanged="dd_zone_SelectedIndexChanged1"></asp:DropDownList></td>
                <td><asp:Label runat="server" ID="Label4"></asp:Label></td>
            </tr>
            <tr>
                <td><b>Branch</td>
                <td style="padding-top:1em;padding-left:1em""><asp:DropDownList runat="server" ID="dd_branch"></asp:DropDownList></td>
                <td><asp:Label runat="server" ID="Label5"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <asp:Button runat="server" ID="btn_save" CssClass="button" Text="Save" OnClick="btn_save_Click" />
                </td>
                <td><asp:Label runat="server" ID="result"></asp:Label></td>
                
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
</asp:Content>