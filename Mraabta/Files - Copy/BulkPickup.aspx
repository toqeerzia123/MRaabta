<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BulkPickup.aspx.cs" MasterPageFile="~/BtsMasterPage.Master" Inherits="MRaabta.Files.BulkPickup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="padding-top:20px;padding-left:20px">
        <asp:FileUpload ID="file" runat="server" />
        <asp:Button runat="server" ID="UploadButton" Text="Upload" OnClick="UploadButton_Click" />
    </div>
</asp:Content>