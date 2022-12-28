<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CardManifest_Print.aspx.cs" Inherits="MRaabta.Files.CardManifest_Print" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <style>
        .table
        {
            border-collapse: collapse;
        }
        
        .table,  .table td
        {
            border: 1px solid black;
        }
        .table th
        {
            text-align:center;
            border: 1px solid black;
            padding:5px 5px 5px 5px;
            }
    </style>
    <form id="form1" runat="server">
    <div style="width: 100%;">
        <asp:PlaceHolder ID="ph1" runat="server"></asp:PlaceHolder>
        <%--<table style="font-family: Calibri; font-size: small; border: medium solid rgb(0, 0, 0);
            margin: 0px; padding: 0px; width: 700px; position: relative; top: 10px;">
            <tr>
                <td>
                    <b>SN</b>
                </td>
                <td>
                    <b>SHIPPER</b>
                </td>
                <td>
                    <b>CONSIGNMENT NO.</b>
                </td>
                <td>
                    <b>CONSIGNEE</b>
                </td>
                <td>
                    <b>ACTUAL WEIGHT</b>
                </td>
            </tr>
            <tr>
                <td style="width: 10%;">
                    1
                </td>
                <td style="width: 27.5%;">
                    Ateeq
                </td>
                <td style="width: 15%;">
                    676627000100
                </td>
                <td style="width: 27.5%;">
                    Rabi
                </td>
                <td style="width: 15%;">
                    0.5 kg
                </td>
            </tr>
        </table>--%>
    </div>
    </form>
</body>
</html>
