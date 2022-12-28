<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="Outbound_Datashuffle.aspx.cs" Inherits="MRaabta.Files.Outbound_Datashuffle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset>
        <legend>Data Shuffle </legend>
        <p>
            Process in order to shuffle Card Consignment Data
        </p>
        <div align="left">
            <asp:Button ID="Btn_process" runat="server" Text="Process" CssClass="button" OnClick="Btn_Search_Click" />
        </div>
    </fieldset>
</asp:Content>
