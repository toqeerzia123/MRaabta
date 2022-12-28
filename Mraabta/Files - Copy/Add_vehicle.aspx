<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="Add_vehicle.aspx.cs" Inherits="MRaabta.Files.Add_vehicle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        *td {
            padding-top: 1em;
            padding-top: 1em
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div>
        <table>
            <tr>
                <td>
                    <asp:Label runat="server" id="l1" Font-Bold="true" Text="Vehicle Name:"></asp:Label>
                </td>
                <td style="padding-top:1em;padding-left:1em">
                    <asp:TextBox runat="server" ID="txt_name" Height="22px"></asp:TextBox>
                </td>
                <td><asp:Label runat="server" ID="errname" Visible="false"></asp:Label></td>
               
            </tr>
            <tr>
                <td style="padding-top:1em"><asp:Label runat="server" ID="l2" Font-Bold="true" Text="Vehicle Description:"></asp:Label></td>
                <td style="padding-top:1em;padding-left:1em">
                    <asp:TextBox runat="server" id="txt_des" TextMode="MultiLine"></asp:TextBox>
                </td>

            </tr>
            <tr>
                <td style="padding-top:1em"><asp:Label runat="server" ID="Label1" Font-Bold="true" Text="Zone:"></asp:Label></td>
                <td style="padding-top:1em;padding-left:1em"><asp:DropDownList runat="server" ID="dd_zone" AutoPostBack="true" OnSelectedIndexChanged="dd_zone_SelectedIndexChanged"></asp:DropDownList></td>
           <td><asp:Label runat="server" ID="errzone" Visible="false"></asp:Label></td>
                  </tr>
            <tr>
                <td style="padding-top:1em"><asp:Label runat="server" ID="Label2" Font-Bold="true" Text="Branch:"></asp:Label></td>
                <td style="padding-top:1em;padding-left:1em""><asp:DropDownList runat="server" ID="dd_br"></asp:DropDownList></td>
            <td><asp:Label runat="server" ID="errbr" Visible="false"></asp:Label></td>
                 </tr>
            <tr>
                <td style="padding-top:1em">
                    <asp:Label runat="server" Text="Vehicle Type:" Font-Bold="true" ID="l3"></asp:Label>
                </td>
                <td style="padding-top:1em;padding-left:1em""><asp:DropDownList runat="server" ID="dd_vt"></asp:DropDownList></td>
            <td><asp:Label runat="server" ID="errvt" Visible="false"></asp:Label></td>
            </tr>
            <tr>
                <td style="padding-top:1em"><asp:Label ID="l4" runat="server" Text="Vehicle Mainatain:" Font-Bold="true"></asp:Label></td>
            <td style="padding-top:1em;padding-left:1em""><asp:DropDownList runat="server" ID="dd_vm">
                <%--<asp:ListItem Text="Select Vehicle" Value=""></asp:ListItem>
                <asp:ListItem Text="M&P Van" Value="01"></asp:ListItem>
                <asp:ListItem Text="Contractual Van" Value="02"></asp:ListItem>
                <asp:ListItem Text="Outsource Van" Value="03"></asp:ListItem>--%>
                </asp:DropDownList></td>
            <td><asp:Label runat="server" ID="errvm" Visible="false"></asp:Label></td>
                 </tr>
            <tr>
                 <td ><asp:Label runat="server"  Font-Bold="true" id="l5" Text="Status:" ></asp:Label></td>
                <td style="padding-top:1em;padding-left:1em"">
                    <asp:RadioButton runat="server" id="rb_a" Text="Active" Checked="true" GroupName="g1" />
                    <asp:RadioButton runat="server" ID="rb_inac" Text="Inactive"  GroupName="g1" />
                </td>
           <td><asp:Label runat="server" ID="errstatus" Visible="false"></asp:Label></td>
                  </tr>
            <tr>
                <td style="padding-top:1em">
                <asp:Button runat="server" CssClass="button" ID="btn_save" Text="Save" OnClick="btn_save_Click" /></td>
            </tr>
            <tr><asp:Label runat="server" ID="result"></asp:Label></tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
</asp:Content>