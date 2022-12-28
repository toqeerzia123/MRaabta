<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="Add_cat.aspx.cs" Inherits="MRaabta.Files.Add_cat" %>


<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <table>
            <tr>
                <td>
                    <asp:RadioButton runat="server" AutoPostBack="true" OnCheckedChanged="rb_newcat_CheckedChanged" ID="rb_newcat" Text="Add New Category" GroupName="g1" /></td>
                <td>
                    <asp:RadioButton runat="server" AutoPostBack="true" OnCheckedChanged="rb_newscat_CheckedChanged" ID="rb_newscat" Text="Add New Sub Category" GroupName="g1" /></td>
                <td>
                    <asp:RadioButton runat="server" AutoPostBack="true" OnCheckedChanged="rb_assgn_CheckedChanged" ID="rb_assgn" Text="Assign Sub Category" GroupName="g1" /></td>
            </tr>
        </table>
        <table>

            <tr id="tr1" runat="server" visible="false">
                <td><b>Category Name</b></td>
                <td style="padding-top: 1em; padding-left: 1em">
                    <asp:TextBox runat="server" ID="txt_cname"></asp:TextBox></td>
            </tr>

            <tr id="tr2" runat="server" visible="false">
                <td><b>Select Category</b></td>
                <td style="padding-top: 1em; padding-left: 1em">
                    <asp:DropDownList Width="200px" runat="server" ID="dd_cat"></asp:DropDownList></td>
            </tr>
            <tr id="tr3" runat="server" visible="false">
                <td><b>Sub CategoryName</b></td>
                <td style="padding-top: 1em; padding-left: 1em">
                    <asp:TextBox runat="server" ID="txt_scname"></asp:TextBox></td>
            </tr>
            <tr>
                <td>
                    <table id="tb1" runat="server" visible="false">
                        <tr>
                            <td><b>Select Main Category</b></td>
                            <td>
                                <asp:DropDownList runat="server" ID="dd_asgmcat" Width="200px"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td><b>Select Sub Category</b></td>
                            <td>
                                <asp:DropDownList runat="server" ID="dd_asgscat" Width="200px"></asp:DropDownList></td>
                        </tr>
                    </table>
                </td>
            </tr>



            <tr visible="false">
                <td style="padding-top: 1em; padding-left: 1em">
                    <asp:Button runat="server" ID="btn_save" OnClick="btn_save_Click" Text="Save" /></td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="lb_result"></asp:Label></td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>