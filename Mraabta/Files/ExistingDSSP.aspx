<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="ExistingDSSP.aspx.cs" Inherits="MRaabta.Files.ExistingDSSP" %>

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
    </style>


    <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
        <tr>
            <td colspan="12" align="center" class="head_column">
                <h3 style="text-align: center">Existing DSSP
                </h3>
            </td>
        </tr>
    </table>

    <table style="margin-left: 5px; font-size: medium; color: black; padding-bottom: 0px; width: 100%; margin-top: 3px">


        <tr class="">
            <asp:Label runat="server" ID="statuslbl" Font-Size="Medium" ForeColor="Red"></asp:Label>

            <div runat="server" id="datediv">

                <td class="field" style="width: 3% !important; text-align: left !important; padding-right: 10px !important;">
                    <b>Date:</b>
                </td>
                <td style="width: 8%; text-align: left;">
                    <asp:TextBox ID="CreationDate" runat="server" placeholder="" Width="180px" AutoPostBack="false"></asp:TextBox>
                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                        Width="25px" />
                    <Ajax1:CalendarExtender ID="CalendarExtender2" TargetControlID="CreationDate" runat="server"
                        Format="yyyy-MM-dd" PopupButtonID="ImageButton1"></Ajax1:CalendarExtender>
                </td>
            </div>

            <div runat="server" id="NumberDiv">
                <td class="field" style="width: 5% !important; text-align: left !important; padding-right: 10px !important;">
                    <b>DSSP Number:</b>
                </td>
                <td style="width: 6%; text-align: left;">
                    <asp:TextBox ID="DSSPIdTxtbox" runat="server" MaxLength="20" Width="180px"> </asp:TextBox>
                </td>
            </div>



            <td style="width: 10%">
                <b>Search By:</b>
                <br />
                <asp:RadioButton runat="server" ID="NumberRadio" GroupName="SearchBy" Text="DSSP No." AutoPostBack="true" OnCheckedChanged="searchBy_CheckedChanged" />
                <br />
                <asp:RadioButton runat="server" ID="DateRadio" GroupName="SearchBy" Text="Date" AutoPostBack="true" OnCheckedChanged="searchBy_CheckedChanged" />
            </td>
            <td style="width: 25%">
                <asp:Button runat="server" CssClass="button" ID="Search" Text="Search" OnClick="Search_Click" />
            </td>
        </tr>



    </table>



    <div style="overflow-y: scroll; height: 600px; /*width: 74%; */width: 99.5%; float: left; margin-left: 5px; margin-top: 3px">
        <asp:GridView runat="server" ID="ConsignmentTable" Width="100%" CellPadding="0" CellSpacing="0" GridLines="Both" ForeColor="Black" AutoGenerateColumns="false" BorderColor="Black" HeaderStyle-HorizontalAlign="Center" BorderStyle="Solid" BorderWidth="1px" CssClass=" ">
            <Columns>
                <asp:TemplateField HeaderText="S.No." HeaderStyle-Font-Size="11" ItemStyle-Font-Size="Small" ItemStyle-Width="2%" HeaderStyle-ForeColor="Black" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true">
                    <ItemTemplate>
                        <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />

                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="DSSPNumber" HeaderStyle-Font-Size="11" ItemStyle-Font-Size="Small" HeaderText="DSSP No." ItemStyle-Width="5%" HeaderStyle-ForeColor="Black" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true" />
                <asp:BoundField DataField="zone" HeaderStyle-Font-Size="11" ItemStyle-Font-Size="Small" HeaderText="Zone" ItemStyle-Width="5%" HeaderStyle-ForeColor="Black" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true" />
                <asp:BoundField DataField="branch" HeaderStyle-Font-Size="11" ItemStyle-Font-Size="Small" ItemStyle-Width="7%" HeaderText="branch" HeaderStyle-ForeColor="Black" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true" />
                <asp:BoundField DataField="expressCenter" HeaderStyle-Font-Size="11" ItemStyle-Font-Size="Small" HeaderText="Express Center" ItemStyle-Width="7%" HeaderStyle-ForeColor="Black" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true" HeaderStyle-VerticalAlign="Middle" />
                <asp:BoundField DataField="BookingCode" HeaderStyle-Font-Size="11" ItemStyle-Font-Size="Small" HeaderText="Booking Code" ItemStyle-Width="7%" HeaderStyle-ForeColor="Black" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true" />
                <asp:BoundField DataField="CNCount" HeaderStyle-Font-Size="11" ItemStyle-Font-Size="Small" HeaderText="CN Amount" ItemStyle-Width="7%" HeaderStyle-ForeColor="Black" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true" />
                <asp:BoundField DataField="TotalAmount" HeaderStyle-ForeColor="Black" HeaderStyle-Font-Size="11" ItemStyle-Font-Size="Small" ItemStyle-Width="7%" HeaderText="Total Amount" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true" />
                <asp:BoundField DataField="CollectAmount" HeaderStyle-ForeColor="Black" HeaderStyle-Font-Size="11" ItemStyle-Font-Size="Small" ItemStyle-Width="7%" HeaderText="Collect Amount" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true" />
                <asp:BoundField DataField="BookingShift" HeaderStyle-ForeColor="Black" HeaderStyle-Font-Size="11" ItemStyle-Font-Size="Small" ItemStyle-Width="7%" HeaderText="Shift" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true" />
                <asp:BoundField DataField="CreatedOn" HeaderStyle-ForeColor="Black" HeaderStyle-Font-Size="11" ItemStyle-Font-Size="Small" ItemStyle-Width="7%" HeaderText="DATE" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true" />
                <asp:HyperLinkField HeaderText="" DataNavigateUrlFields="DSSPNumber" DataTextField="DSSPNumber" DataNavigateUrlFormatString="~/Files/DSSP.aspx?DSSPNo={0}" HeaderStyle-Width="5%" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" Target="_blank" />
            </Columns>
        </asp:GridView>
    </div>
    <br />
    <div>
    </div>

</asp:Content>
