<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="Manage_InternationalHisotry.aspx.cs" Inherits="MRaabta.Files.Manage_InternationalHisotry" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .med-field
        {
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function isNumberKey(evt) {

            var count = 1;
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31
                && (charCode < 48 || charCode > 57 || charCode == 110 || charCode == 109 || charCode == 45 || charCode == 9)) {
                return false;
            }
            else {

                if (charCode == 110 || charCode == 46) {
                    count++;
                }
                if (count > 1) {
                    return false
                }
            }

            return true;
        }

        function onlyAlphabets(e, t) {
            try {
                if (window.event) {
                    var charCode = window.event.keyCode;
                }
                else if (e) {
                    var charCode = e.which;
                }
                else { return true; }
                if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || (charCode == 32))
                    return true;
                else
                    return false;
            }
            catch (err) {
                alert(err.Description);
            }
        }
    </script>
    <table class="input-form boxbg" style="width: 95%">
        <tr>
            <td class="field">
                Consignment Number:
            </td>
            <td class="input-field">
                <asp:TextBox ID="txt_consignmentno" runat="server" CssClass="med-field" onkeypress="return isNumberKey(event);"></asp:TextBox>
            </td>
            <td style="float: left; margin: 0px 0px 0px 20px;">
            </td>
            <td>
                <asp:Button ID="submit" runat="server" Text="Search" CssClass="button1" OnClick="Btn_Load_Click" />
            </td>
        </tr>
    </table>
    <br />
    <br />
    <fieldset>
        <legend>Tracking Details</legend>
        <table class="input-form boxbg" style="width: 95%">
            <tr>
                <td class="field">
                    Consignment Number:
                </td>
                <td class="input-field">
                    <asp:TextBox ID="txt_1" runat="server" CssClass="med-field" Enabled="false"></asp:TextBox>
                    <asp:HiddenField ID="hd_rd1" runat="server" />
                </td>
                <td style="float: left; margin: 0px 0px 0px 20px;">
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td class="field">
                    Reference Number:
                </td>
                <td class="input-field">
                    <asp:TextBox ID="txt_referenceno" runat="server" CssClass="med-field" AutoPostBack="true"
                        onkeypress="return isNumberKey(event);" OnTextChanged="txt_referenceno_TextChanged"></asp:TextBox>
                </td>
                <td style="float: left; margin: 0px 0px 0px 20px;">
                </td>
                <td>
                </td>
            </tr>
             <tr>
                <td class="field">
                    Couries:
                </td>
                <td class="input-field">
                    <asp:DropDownList ID="dd_couries" runat="server" CssClass="med-field"></asp:DropDownList>
                </td>
                <td style="float: left; margin: 0px 0px 0px 20px;">
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td class="field">
                    Carrier :
                </td>
                <td class="input-field">
                    <asp:TextBox ID="txt_Carier" runat="server" CssClass="med-field"></asp:TextBox>
                </td>
                <td style="float: left; margin: 0px 0px 0px 20px;">
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td class="field">
                    Current time
                </td>
                <td class="input-field">
                    <telerik:RadDateTimePicker ID="rd_1" runat="server" MinDate="1/12/2018">
                    </telerik:RadDateTimePicker>
                </td>
                <td style="float: left; margin: 0px 0px 0px 20px;">
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td class="field">
                    Current Location
                </td>
                <td class="input-field">
                    <asp:TextBox ID="txt_CurrentLocation" runat="server" CssClass="med-field" onkeypress="return onlyAlphabets(event,this);"></asp:TextBox>
                </td>
                <td style="float: left; margin: 0px 0px 0px 20px;">
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td class="field">
                    Tracking Details
                </td>
                <td class="input-field">
                    <asp:TextBox ID="txt_TrackingDetails" runat="server" CssClass="med-field" TextMode="MultiLine"
                        Height="70px" Width="257px" onkeypress="return onlyAlphabets(event,this);"></asp:TextBox>
                </td>
                <td style="float: left; margin: 0px 0px 0px 20px;">
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td class="field">
                </td>
                <td class="field" colspan="2">
                    <div style="width: 600px">
                        <asp:Button ID="btn_Insert" runat="server" Text="Insert" CssClass="button1" OnClick="btn_Insert_Click" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btn_update" runat="server" Text="update" CssClass="button1" Enabled="false"
                            OnClick="btn_update_Click" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btn_Cancel" runat="server" Text="cancel" CssClass="button1" OnClick="btn_Cancel_Click" />
                    </div>
                </td>
            </tr>
        </table>
        <br />
        <fieldset>
            <legend>Tracking Details</legend>
            <asp:Repeater runat="server" ID="Repeater1" OnItemCommand="Repeater1_ItemCommand"
                OnItemDataBound="Repeater1_ItemDataBound">
                <HeaderTemplate>
                    <table class="mGrid">
                        <tr>
                            <th>
                            </th>
                            <th>
                            </th>
                            <th>
                                Consignment Number
                            </th>
                            <th>
                                Reference Number
                            </th>
                            <th>
                                Transaction Time
                            </th>
                            <th>
                                Current Location
                            </th>
                            <th>
                                Tracking Details
                            </th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:LinkButton ID="lb_1" runat="server" Text="Edit" CommandName="Edit"></asp:LinkButton>
                        </td>
                        <td>
                            <asp:LinkButton ID="lb_2" runat="server" Text="Delete" CommandName="Delete"></asp:LinkButton>
                        </td>
                        <td>
                            <asp:Label ID="lbl_ConsignmentNumber" runat="server" Text='<%# Eval("consignmentnumber") %>'></asp:Label>
                            <asp:HiddenField ID="hd_id" runat="server" Value='<%# Eval("ID") %>' />
                        </td>
                        <td>
                            <asp:Label ID="lbl_referenceNumber" runat="server" Text='<%# Eval("ReferenceNumber") %>'></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_TransactionTime" runat="server" Text='<%# Eval("transactionDate") %>'></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_CurrentLocation" runat="server" Text='<%# Eval("CurrentLocation") %>'></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_Details" runat="server" Text='<%# Eval("Details") %>'></asp:Label>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </fieldset>
    </fieldset>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
