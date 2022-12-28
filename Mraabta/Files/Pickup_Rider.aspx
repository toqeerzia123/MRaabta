<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="Pickup_Rider.aspx.cs" Inherits="MRaabta.Files.Pickup_Rider" %>


<%@ Register Assembly="TimePicker" Namespace="MKB.TimePicker" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Scripts/jquery-3.5.1.min.js"></script>
    <script type="text/javascript">

        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }

        var uploadFile = (data) => {
            $.ajax({
                url: '<%=ResolveUrl("~/PickUp/UploadBulkPickup")%>',
                type: "POST",
                contentType: false,
                processData: false,
                data: data,
                success: function (result) {
                    alert(result);
                },
                error: function (err) {
                    alert(err.statusText);
                }
            });
        };

        $(function () {
            $('#btnUpload').click(function () {
                var fileUpload = $("#file").get(0);
                var file = fileUpload.files[0];
                var data = new FormData();
                data.append('file', file);
                uploadFile(data);
            });
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .button {
            border-style: none !important;
            border-color: inherit !important;
            border-width: 0 !important;
            background-color: #5f5a8d !important;
            border-radius: 5px !important;
            color: White !important;
            font-family: Calibri !important;
            font-size: small !important;
            padding: 3px 20px !important;
            cursor: pointer !important;
            height: 31px;
        }
    </style>
    <div style="float: left">
        <asp:Label ID="Errorid" runat="server" ForeColor="Red"></asp:Label>
    </div>
    <br />
    <fieldset style="width: 100%">
        <legend>Manual Pick Up</legend>
        <table width="100%">
            <tr>
                <td colspan="1">
                    <b>Account:</b>
                </td>
                <td colspan="1">
                    <asp:TextBox ID="txt_AccountNo" runat="server" AutoPostBack="true" OnTextChanged="txt_AccountNo_TextChanged"></asp:TextBox>
                </td>
                <td colspan="1"></td>
                <td colspan="1">
                    <asp:Label ID="lbl_AccountName" runat="server"></asp:Label>
                </td>
                <td colspan="1">
                    <asp:Label ID="lbl_AccountAddress" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="1">
                    <b>Origin</b>
                </td>
                <td colspan="1">
                    <asp:DropDownList ID="dd_Origin" runat="server" AutoPostBack="true" OnSelectedIndexChanged="dd_Origin_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td colspan="1"></td>
                <td colspan="1">
                    <b>Alternate Address:</b>
                </td>
                <td colspan="1">
                    <asp:TextBox ID="txt_alternateAddress" runat="server" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="1">
                    <b>Pick up Date</b>
                </td>
                <td colspan="1">
                    <asp:TextBox ID="txt_pickDate" runat="server"></asp:TextBox>
                </td>
                <td colspan="1"></td>
                <td colspan="1">
                    <b>Pick up Time</b>
                </td>
                <td colspan="1">
                    <cc1:TimeSelector ID="TimeSelector1" runat="server">
                    </cc1:TimeSelector>
                </td>
            </tr>
            <tr>
                <td colspan="1">
                    <b>Weight</b>
                </td>
                <td colspan="1">
                    <asp:TextBox ID="txt_weight" runat="server"></asp:TextBox>
                </td>
                <td colspan="1"></td>
                <td colspan="1">
                    <b>Pieces</b>
                </td>
                <td colspan="1">
                    <asp:TextBox ID="txt_pieces" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="1">
                    <b>Rider</b>
                </td>
                <td colspan="1">
                    <%-- <asp:DropDownList ID="dd_riders" runat="server" AutoPostBack="true" OnSelectedIndexChanged="dd_riders_SelectedIndexChanged">
                    </asp:DropDownList>--%>
                    <asp:TextBox ID="dd_riders" runat="server" AutoPostBack="true" OnTextChanged="dd_riders_TextChanged"
                        Visible="false"></asp:TextBox>
                    <asp:DropDownList ID="dd_presentRiders" runat="server" AutoPostBack="true" AppendDataBoundItems="true"
                        OnSelectedIndexChanged="dd_riders_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td colspan="1"></td>
                <td colspan="1">
                    <b>Rider Mobile No.</b>
                </td>
                <td colspan="1">
                    <asp:TextBox ID="txt_RiderPhone" runat="server" MaxLength="11" onkeypress="return isNumber(event)"></asp:TextBox>
                </td>
                <td colspan="1">
                    <asp:Label ID="lbl_riderName" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label Text="Type" runat="server" Font-Bold="true"/>
                </td>
                <td>
                    <asp:RadioButtonList ID="type" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="On Time" Value="OnTime" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Scheduled" Value="Scheduled"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td colspan="1"></td>
                <td colspan="1">&nbsp;
                </td>
                <td colspan="1"></td>
                <td colspan="1">
                    <asp:Button ID="btn_AddBooking" runat="server" Text="Save and Send SMS" CssClass="button"
                        OnClick="btn_AddBooking_Click" />
                </td>
                <td colspan="1"></td>
            </tr>
        </table>
    </fieldset>
    <br />
    <br />
    <fieldset style="width: 100%">
        <legend>Bulk Pick Up</legend>
        <div style="padding-top: 20px; padding-left: 20px">
            <input type="file" id="file" />
            <button type="button" id="btnUpload">Upload</button>
            <a href="<%=ResolveUrl("~/docs/data.xlsx") %>" style="margin-left: 20px" download>Download Sample File</a>
        </div>
    </fieldset>
</asp:Content>
