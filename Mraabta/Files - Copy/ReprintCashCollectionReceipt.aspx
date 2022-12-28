<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="ReprintCashCollectionReceipt.aspx.cs" Inherits="MRaabta.Files.ReprintCashCollectionReceipt" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <script type="text/javascript">
        function isNumberKeyWithDecimal(evt) {
            var status = false;
            var charCode = (evt.which) ? evt.which : event.keyCode

            if (charCode > 31 && (charCode < 48 || charCode > 57))
                status = false;
            if (charCode == 46)
                status = true;
            if (charCode > 47 && charCode < 58)
                status = true;
            return status;
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }


    </script>
    <style>
        .table th {
            border: 1px solid black;
            margin: 0;
            padding: 0;
        }

        .table td {
            border: 1px solid black;
            margin: 0;
            padding: 0;
        }

        tr.spaceUnder > td {
            padding-bottom: 2px;
        }

        .div_header {
            width: 100%;
            height: 23px;
            font-size: 14px;
            margin-bottom: 1px;
        }

            .div_header div {
                float: left;
                background-color: gray;
                width: 16.4%;
                font-size: 14px;
                border: 1px solid #000;
                color: black;
            }


        .bottom div {
            float: left;
            width: 16.4%;
            height: 36px;
            border: 0.5px solid #000;
            color: black;
        }

        .saveBtn {
            margin-top: 5px;
            float: right;
            margin: 8px 25px 10px 0px;
        }  
/* Center the loader */

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
    
    .loader {
      border: 16px solid #f3f3f3;
      border-radius: 50%;
      border-top: 16px solid #3498db;
      width: 120px;
      height: 120px;
      -webkit-animation: spin 2s linear infinite;
      animation: spin 2s linear infinite;
      left: 43%;
      position:relative;
      top:43%
    }

    @-webkit-keyframes spin {
      0% { -webkit-transform: rotate(0deg); }
      100% { -webkit-transform: rotate(360deg); }
    }

    @keyframes spin {
      0% { transform: rotate(0deg); }
      100% { transform: rotate(360deg); }
    }
</style>

    <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
        <tr>
            <td colspan="12" align="center" class="head_column">
                <h3 style="text-align: center">Re Print Cash Collection Voucher
                </h3>
            </td>
        </tr>
    </table>

    <fieldset style="border: 1px solid black;margin-top: 4px;">
    <table style="margin-left: 5px; font-size: medium; color: black; padding-bottom: 0px; width: 100%; margin-top: 3px">


        <tr class="">
            <asp:Label runat="server" ID="statuslbl" Font-Size="Medium" ForeColor="Red"></asp:Label>

            <div runat="server" id="DateDiv" visible="false">

                <td class="field" style="width: 4% !important; text-align: left !important; padding-right: 10px !important;">
                    <b>Date:</b>
                </td>
                <td style="width: 5%; text-align: left;">
                    <asp:TextBox ID="CreationDate" runat="server" placeholder="" Width="180px" AutoPostBack="false"></asp:TextBox>
                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                        Width="25px" />
                    <Ajax1:CalendarExtender ID="CalendarExtender2" TargetControlID="CreationDate" runat="server"
                        Format="yyyy-MM-dd" PopupButtonID="ImageButton1"></Ajax1:CalendarExtender>
                </td>
            </div>

            <div runat="server" id="NumberDiv" visible="true">
                <td class="field" style="width: 5% !important; text-align: left !important; padding-right: 10px !important;">
                    <b>RR Number:</b>
                </td>
                <td style="width: 5%; text-align: left;">
                    <asp:TextBox ID="RRNumberTxt" runat="server" MaxLength="20" Width="180px"> </asp:TextBox>
                </td>
            </div>


              <div runat="server" id="CashierDiv" visible="false">
                <td class="field" style="width: 5% !important; text-align: left !important; padding-right: 10px !important;">
                    <b>Cashier User Id:</b>
                </td>
                <td style="width: 5%; text-align: left;">
                    <asp:TextBox ID="cashierIdTxt" runat="server" MaxLength="100" Width="180px"> </asp:TextBox>
                </td>
            </div>


            <td style="width: 10%">
                <b>Search By:</b>
                <br />
                <asp:RadioButton runat="server" ID="NumberRadio" Checked="true" GroupName="SearchBy" Text="Voucher No." AutoPostBack="true" OnCheckedChanged="searchBy_CheckedChanged" />
                <br />
                <asp:RadioButton runat="server" ID="DateRadio" GroupName="SearchBy" Text="Date" AutoPostBack="true" OnCheckedChanged="searchBy_CheckedChanged" />
                   <br />
                <asp:RadioButton runat="server" ID="CashierIdRadio" GroupName="SearchBy" Text="Cashier Id" AutoPostBack="true" OnCheckedChanged="searchBy_CheckedChanged" />
            </td>
            <td style="width: 25%">
                <asp:Button runat="server" CssClass="button" ID="Search" Text="Search" OnClick="Search_Click"     OnClientClick="javascript:document.getElementById('ContentPlaceHolder1_loaders').style.display = 'block';" />
           
            </td>
        </tr>
    </table>
    </fieldset>
    <div id="loaders" runat="server" class="outer_box" style="display: none;">
            <div id="loader" runat="server" class="loader">
            </div>
        </div>

    <div style="overflow-y: scroll; height: 600px; /*width: 74%; */width: 99.5%; float: left; margin-left: 5px; margin-top: 3px">
        <asp:GridView runat="server" ID="ConsignmentTable" Width="100%" CellPadding="0" CellSpacing="0" GridLines="Both" ForeColor="Black" AutoGenerateColumns="false" BorderColor="Black" HeaderStyle-HorizontalAlign="Center" BorderStyle="Solid" BorderWidth="1px" CssClass=" ">
            <Columns>
                <asp:TemplateField HeaderText="S.No." HeaderStyle-Font-Size="11" ItemStyle-Font-Size="Small" ItemStyle-Width="2%" HeaderStyle-ForeColor="Black" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true">
                    <ItemTemplate>
                        <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />

                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="VoucherNo" HeaderStyle-Font-Size="11" ItemStyle-Font-Size="Small" HeaderText="Voucher No." ItemStyle-Width="5%" HeaderStyle-ForeColor="Black" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true" />
                <asp:BoundField DataField="DSSP" HeaderStyle-Font-Size="11" ItemStyle-Font-Size="Small" HeaderText="DSSP No." ItemStyle-Width="5%" HeaderStyle-ForeColor="Black" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true" />
                <asp:BoundField DataField="EC" HeaderStyle-Font-Size="11" ItemStyle-Font-Size="Small" HeaderText="Express Center" ItemStyle-Width="5%" HeaderStyle-ForeColor="Black" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true" />
                <asp:BoundField DataField="Branch" HeaderStyle-Font-Size="11" ItemStyle-Font-Size="Small" ItemStyle-Width="7%" HeaderText="branch" HeaderStyle-ForeColor="Black" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true" />
                <asp:BoundField DataField="Amount" HeaderStyle-ForeColor="Black" HeaderStyle-Font-Size="11" ItemStyle-Font-Size="Small" ItemStyle-Width="7%" HeaderText="Amount" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true" />
                <asp:HyperLinkField HeaderText="" DataNavigateUrlFields="DSSP,BookingCode" Text="Print" DataNavigateUrlFormatString="~/Files/ReceiptVoucherDSSP.aspx?DSSP={0}&BookingCode={1}" HeaderStyle-Width="5%" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" Target="_blank" />
            </Columns>
        </asp:GridView>
    </div>
    <br />
    <div>
    </div>

</asp:Content>
