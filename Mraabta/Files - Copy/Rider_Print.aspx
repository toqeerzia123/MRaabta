<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Rider_Print.aspx.cs" Inherits="MRaabta.Files.Rider_Print" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style>
        .outpiece
        {
            width: 100%;
            font-family: Calibri;
            font-size: small;
            border-collapse: collapse;
        }
        
        .outpiece th
        {
            border-bottom: 2px Solid Black;
            border-top: 2px Solid Black;
        }
        
        .outpiece td
        {
            border-bottom: 1px Solid Black;
            border-top: 1px Solid Black;
        }
        .auto-style1
        {
            width: 39%;
        }
        .auto-style2
        {
            width: 36%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div style="width: 700px;">
            <table style="width: 100%; text-align: center; font-family: Calibri; font-size: small;">
                <tr>
                    <td style="width: 20%">
                        <img src="../images/OCS_Transparent.png" height="60px" alt="logo" width="157px" />
                    </td>
                    <td style="width: 60%; text-align: center;">
                        <h2>
                            RIDER INFORMATION</h2>
                    </td>
                    <td style="width: 20%; vertical-align: top; text-align: right">
                        <b>Print Time</b><asp:Label ID="lbl_Dd" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <table style="border: thin solid
                #003366; width: 100%; font-family: Calibri; font-size: small; border-collapse: collapse;">
                <tr>
                    <td colspan="5" style="text-align: center; font-size: medium; border: 2px
                Solid Black; background-color: #C0C0C0">
                        <b>Bio Data</b>
                    </td>
                </tr>
                <tr>
                    <td colspan="1" style="border-right-style: solid; border-right-width: 1px; border-right-color: #000000;
                        border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;
                        border-left-width: 2px; border-left-style: Solid;" class="auto-style1">
                        <b>Ride Code: </b>
                        <asp:Label ID="lbl_RiderCode" runat="server"></asp:Label>
                    </td>
                    <td colspan="2" style="border-right-style: solid; border-right-width: 1px; border-right-color: #000000;
                        border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;
                        border-left-width: 2px; border-left-style: Solid;">
                    </td>
                    <td colspan="2" style="border-right-style: solid; border-right-width: 2px; border-right-color: #000000;
                        border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;">
                        <b>EntryDate: </b>
                        <asp:Label ID="lbl_EDate" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <table style="border: thin solid
                #003366; width: 100%; font-family: Calibri; font-size: small; border-collapse: collapse;">
                            <td style="border-right-style: solid; border-right-width: 1px; border-right-color: #000000;
                                border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #000000;
                                width: 20%;">
                                <b>First Name: </b>
                                <asp:Label ID="lbl_FName" runat="server"></asp:Label>
                            </td>
                            <td colspan="2" style="border-right-style: solid; border-right-width: 1px; border-right-color: #000000;
                                border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #000000;
                                width: 30%;">
                                <b>Middle Name : </b>
                                <asp:Label ID="lbl_MName" runat="server"></asp:Label>
                            </td>
                            <td colspan="3" style="border-right-style: solid; border-right-width: 1px; border-right-color: #000000;
                                border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #000000;
                                border-left-width: 2px; border-left-style: Solid; width: 20%;">
                                <b>Last Name : </b>
                                <asp:Label ID="lbl_LName" runat="server"></asp:Label>
                            </td>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="5" style="border-right-style: solid; border-right-width: 1px; border-right-color: #000000;
                        border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;
                        border-left-width: 2px; border-left-style: Solid;" nowrap>
                        <b>Address: </b>
                        <asp:Label ID="lbl_Address" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="1" style="border-right: 2px solid #000000; border-bottom: 2px solid #000000;"
                        class="auto-style1">
                        <b>Phone No: </b>
                        <asp:Label ID="lbl_Phone" runat="server"></asp:Label>
                    </td>
                    <td colspan="2" style="border-right: 2px solid #000000; border-bottom: 2px solid #000000;">
                        <b>CNIC: </b>
                        <asp:Label ID="lbl_CNIC" runat="server"></asp:Label>
                    </td>
                    <td colspan="1" style="border-right: 2px solid #000000; border-bottom: 2px solid #000000;"
                        class="auto-style1">
                        <b>Status : </b>
                    </td>
                    <td colspan="1" style="border-right: 2px solid #000000; border-bottom: 2px solid #000000;"
                        class="auto-style1">
                        <asp:Label ID="lbl_Status" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="1" style="border-right: 2px solid #000000; border-bottom: 2px solid #000000;"
                        class="auto-style1">
                        <b>Rider Type: </b>
                        <asp:Label ID="lbl_RiderType" runat="server"></asp:Label>
                    </td>
                    <td colspan="1" style="border-right: 1px solid #000000; border-bottom: 2px solid #000000;
                        border-left-width: 2px; border-left-style: Solid;" class="auto-style1">
                        <b>Duty Type: </b>
                        <asp:Label ID="lbl_RiderDutyType" runat="server"></asp:Label>
                    </td>
                    <td colspan="3" style="border-right-style: solid; border-right-width: 1px; border-right-color: #000000;
                        border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;
                        border-left-width: 2px; border-left-style: Solid; width: 50%;">
                        <b>Shift Type: </b>
                        <asp:Label ID="lbl_RShiftType" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="1" style="border-right: 1px solid #000000; border-bottom: 2px solid #000000;
                        border-left-width: 2px; border-left-style: Solid;" class="auto-style1">
                        <b>Express Center: </b>
                        <asp:Label ID="lbl_Ec" runat="server"></asp:Label>
                    </td>
                    <td colspan="1" style="border-right: 1px solid #000000; border-bottom: 2px solid #000000;
                        border-left-width: 2px; border-left-style: Solid;" class="auto-style1">
                        <b>Department: </b>
                        <asp:Label ID="lbl_dept" runat="server"></asp:Label>
                    </td>
                    <td colspan="1" style="border-right: 1px solid #000000; border-bottom: 2px solid #000000;
                        border-left-width: 2px; border-left-style: Solid;" class="auto-style2">
                        <b>Branch Name: </b>
                        <asp:Label ID="lbl_BranchName" runat="server"></asp:Label>
                    </td>
                    <td colspan="2" style="border-right-style: solid; border-right-width: 1px; border-right-color: #000000;
                        border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;
                        border-left-width: 2px; border-left-style: Solid; width: 50%;">
                        <b>Zone Name: </b>
                        <asp:Label ID="lbl_ZoneName" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <br />
            <table style="border: thin solid
                #003366; width: 100%; font-family: Calibri; font-size: small; border-collapse: collapse;">
                <tr>
                    <td colspan="5" style="text-align: center; font-size: medium; border: 2px
                Solid Black; background-color: #C0C0C0">
                        <b>Route Name</b>
                    </td>
                </tr>
                <tr>
                    <td style="border-right-style: solid; border-right-width: 1px; border-right-color: #000000;
                        border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #000000;
                        width: 20%;">
                        <b>RouteCode: </b>
                        <asp:Label ID="lbl_RouteCode" runat="server"></asp:Label>
                    </td>
                    <td colspan="3" style="border-right-style: solid; border-right-width: 1px; border-right-color: #000000;
                        border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #000000;
                        width: 20%;">
                        <b>Route Name: </b>
                        <asp:Label ID="lbl_RouteName" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <br />
            <table style="border: thin solid
                #003366; width: 100%; font-family: Calibri; font-size: small; border-collapse: collapse;">
                <tr>
                    <td colspan="5" style="text-align: center; font-size: medium; border: 2px
                Solid Black; background-color: #C0C0C0">
                        <b>HR Information</b>
                    </td>
                </tr>
                <tr>
                    <td style="border-right-style: solid; border-right-width: 1px; border-right-color: #000000;
                        border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #000000;
                        width: 20%;">
                        <b>HR Code: </b>
                        <asp:Label ID="lbl_HrCode" runat="server"></asp:Label>
                    </td>
                    <td style="border-right-style: solid; border-right-width: 1px; border-right-color: #000000;
                        border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #000000;
                        width: 20%;">
                        <b>Date of Joining: </b>
                        <asp:Label ID="lbl_DOJ" runat="server"></asp:Label>
                    </td>
                    <td style="border-right-style: solid; border-right-width: 1px; border-right-color: #000000;
                        border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #000000;
                        border-left-width: 2px; border-left-style: Solid; width: 20%;">
                        <b>Date of Birth: </b>
                        <asp:Label ID="lbl_DOB" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <table style="border: thin solid
                #003366; width: 100%; font-family: Calibri; font-size: small; border-collapse: collapse;">
                <tr>
                    <td colspan="5" style="text-align: center; font-size: medium; border: 2px
                Solid Black; background-color: #C0C0C0">
                        <b>Deactivation Information</b>
                    </td>
                </tr>
                <tr>
                    <td style="border-right-style: solid; border-right-width: 1px; border-right-color: #000000;
                        border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #000000;
                        width: 20%;">
                        <b>Type Of separation: </b>
                        <asp:Label ID="lbl_sep" runat="server"></asp:Label>
                    </td>
                    <td style="border-right-style: solid; border-right-width: 1px; border-right-color: #000000;
                        border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #000000;
                        width: 20%;">
                        <b>Date of separation: </b>
                        <asp:Label ID="lbl_sepDate" runat="server"></asp:Label>
                    </td>
                    <td style="border-right-style: solid; border-right-width: 1px; border-right-color: #000000;
                        border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #000000;
                        border-left-width: 2px; border-left-style: Solid; width: 20%;">
                        <b>Reamarks: </b>
                        <asp:Label ID="lbl_remarks" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <table style="border: thin solid
                #003366; width: 50%; font-family: Calibri; font-size: small;">
                <tr>
                    <td colspan="1" style="text-align: center; font-size: medium;">
                    </td>
                    <td colspan="2" style="border-style: none none solid none; border-width: 2px; border-color: inherit;
                        text-align: center; border-bottom-style: solid; border-bottom-width: 2px;">
                    </td>
                    <td colspan="1" style="text-align: center; font-size: medium;">
                    </td>
                    <td colspan="2" style="border-color: Black; text-align: center; font-size: medium;
                        border-bottom-style: solid; border-bottom-width: 2px;">
                    </td>
                </tr>
                <tr>
                    <td colspan="1" style="text-align: center; font-size: medium;">
                    </td>
                    <td colspan="2" style="border-style: none; border-width: 2px; border-color: inherit;
                        text-align: center; border-bottom-width: 2px;">
                        Approved By
                    </td>
                    <td colspan="1" style="text-align: center; font-size: medium;">
                    </td>
                    <td colspan="2" style="border-color: Black; text-align: center; font-size: medium;
                        border-bottom-width: 2px;">
                        Received By
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</html>
