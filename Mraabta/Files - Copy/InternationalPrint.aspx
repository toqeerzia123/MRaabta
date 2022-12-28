<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InternationalPrint.aspx.cs" Inherits="MRaabta.Files.InternationalPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 720px;">
        <%--<table style="width: 720px;" cellpadding="0" cellspacing="0">
            <tr>
                <td style="width: 635px;">
                    <table style="width: 634px; border-collapse: collapse; font-size: x-small; font-family: Calibri;"
                        cellpadding="0px">
                        <tr>
                            <td style="width: 317px;">
                                <table style="width: 317px; border-collapse: collapse; font-size: x-small; font-family: Calibri;">
                                    <tr>
                                        <td style="width: 130px; height: 50px; border-top: 2px Solid Black; border-left: 2px Solid Black;
                                            border-bottom: 1px Solid Black; border-right: 1px Solid Black; vertical-align: top;">
                                            <div style="width: 100%; height: 15px; border-bottom: 1px Solid Black; margin-left: -1px;
                                                padding-left: 2px; vertical-align: middle;">
                                                SHIPPER'S ACCOUNT
                                            </div>
                                        </td>
                                        <td style="width: 58px; height: 50px; border-top: 2px Solid Black; border-left: 1px Solid Black;
                                            border-bottom: 1px Solid Black; border-right: 2px Solid Black; vertical-align: top;">
                                            <div style="width: 100%; height: 15px; border-bottom: 1px Solid Black; margin-left: -1px;
                                                padding-left: 2px; vertical-align: middle;">
                                                ORIGIN
                                            </div>
                                        </td>
                                        <td style="width: 129px; border-bottom: 2px Solid Black;">
                                            <img src="../images/OCS_Transparent.png" height="50px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 130px; height: 50px; border-top: 1px Solid Black; border-left: 2px Solid Black;
                                            border-bottom: 1px Solid Black; border-right: 1px Solid Black; vertical-align: top;">
                                            <div style="width: 100%; height: 15px; border-bottom: 1px Solid Black; margin-left: -1px;
                                                padding-left: 2px; vertical-align: middle;">
                                                SECTION CODE
                                            </div>
                                        </td>
                                        <td style="width: 187px; height: 50px; border-right: 2px Solid Black; border-bottom: 1px Solid Black;"
                                            colspan="2">
                                            <div style="width: 100%; height: 14px; border-bottom: 1px Solid Black; margin-left: -1px;
                                                padding-left: 2px; vertical-align: middle;">
                                                SERVICE (PLEASE CHECK)
                                            </div>
                                            <table style="width: 100%; height: 35px; border-collapse: collapse;">
                                                <tr>
                                                    <td style="border-right: 1px Solid Black; width: 25%; text-align: center;">
                                                        DOC
                                                    </td>
                                                    <td style="border-right: 1px Solid Black; width: 25%">
                                                    </td>
                                                    <td style="border-right: 1px Solid Black; width: 25%">
                                                    </td>
                                                    <td style="width: 25%; text-align: center;">
                                                        SPS
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="width: 100%; border-left: 2px Solid Black; border-right: 1px Solid Black;
                                            height: 125px; vertical-align: top;">
                                            <div style="width: 100%; height: 14px; margin-left: -1px; padding-left: 2px; vertical-align: middle;">
                                                FROM (SHIPPER)
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 130px; height: 34px; border-bottom: 1px Solid Black; border-left: 2px Solid Black;">
                                        </td>
                                        <td style="width: 187px; height: 34px; border: 1px Solid Black; vertical-align: top;"
                                            colspan="2">
                                            <div style="width: 100%; height: 14px; margin-left: -1px; padding-left: 2px; vertical-align: top;">
                                                PHONE
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="width: 100%; border-left: 2px Solid Black; border-right: 1px Solid Black;
                                            border-bottom: 1px Solid Black; height: 34px; vertical-align: top;">
                                            <div style="width: 100%; height: 14px; margin-left: -1px; padding-left: 2px; vertical-align: middle;">
                                                SHIPPER'S NAME/DEPT. (SIGNATURE)
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="width: 100%; border-left: 2px Solid Black; border-right: 1px Solid Black;
                                            border-bottom: 1px Solid Black; height: 34px; vertical-align: top;">
                                            <div style="width: 100%; height: 14px; margin-left: -1px; padding-left: 2px; vertical-align: middle;">
                                                SHIPPER'S REFERENCE
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 317px;">
                                <table style="width: 317px; border-collapse: collapse; font-size: x-small; font-family: Calibri;">
                                    <tr>
                                        <td colspan="3" style="width: 100%; height: 105px; border-bottom: 2px Solid Black;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="width: 100%; border-left: 1px Solid Black; border-right: 1px Solid Black;
                                            height: 125px; vertical-align: top;">
                                            <div style="width: 100%; height: 14px; margin-left: -1px; padding-left: 2px; vertical-align: middle;">
                                                TO (CONSIGNEE)
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 130px; height: 34px; border-bottom: 1px Solid Black; border-left: 1px Solid Black;">
                                        </td>
                                        <td style="width: 187px; height: 34px; border: 1px Solid Black; vertical-align: top;"
                                            colspan="2">
                                            <div style="width: 100%; height: 14px; margin-left: -1px; padding-left: 2px; vertical-align: top;">
                                                PHONE
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="width: 100%; border-left: 1px Solid Black; border-right: 1px Solid Black;
                                            border-bottom: 1px Solid Black; height: 34px; vertical-align: top;">
                                            <div style="width: 100%; height: 14px; margin-left: -1px; padding-left: 2px; vertical-align: middle;">
                                                ATTN
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="width: 100%; border-left: 1px Solid Black; border-right: 1px Solid Black;
                                            border-bottom: 1px Solid Black; height: 34px; vertical-align: top;">
                                            <div style="width: 100%; height: 14px; margin-left: -1px; padding-left: 2px; vertical-align: middle;">
                                                OTHER: (SPECIAL)
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 633px; height: 73px; vertical-align: top; border-bottom: 1px Solid Black;"
                                colspan="2">
                                <div style="border-left: 2px Solid Black; width: 632px; height: 73px; border-right: 1px Solid Black;">
                                    <div style="width: 100%; height: 15px; border-bottom: 1px Solid Black; margin-left: -1px;
                                        padding-left: 2px; vertical-align: middle; text-align: center;">
                                        DESCRIPTION OF CONTENTS
                                    </div>
                                    <div style="width: 100%; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle;
                                        text-align: left; font-size: x-small;">
                                        Please attach a commercial invoice of SPS
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 317px; height: 50px; border-bottom: 1px Solid Black; border-right: 1px Solid Black;
                                vertical-align: top;">
                                <table style="width: 317px; height: 50px; border-collapse: collapse; border-left: 2px Solid Black;"
                                    cellpadding="0px;">
                                    <tr>
                                        <td style="text-align: center; vertical-align: top; height: 15px;">
                                            <div style="width: 148px; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle;
                                                text-align: Center; font-size: x-small;">
                                                DECLARED VALUE FOR CUSTOMS
                                            </div>
                                        </td>
                                        <td style="text-align: center; vertical-align: top; height: 15px;">
                                            <div style="width: 148px; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle;
                                                text-align: Center; font-size: x-small;">
                                                INSURANCE
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center; vertical-align: middle; height: 34px;">
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 34px;">
                                            YES/NO
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 316px; height: 50px; border-bottom: 1px Solid Black; border-right: 1px Solid Black;
                                vertical-align: top;">
                                <table style="width: 316px; height: 50px; border-collapse: collapse;" cellpadding="0px;">
                                    <tr>
                                        <td style="text-align: center; vertical-align: top; height: 15px;">
                                            <div style="width: 148px; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle;
                                                text-align: left; font-size: x-small;">
                                                DIMENSIONS (CM)
                                            </div>
                                        </td>
                                        <td style="text-align: center; vertical-align: top; height: 15px;">
                                            <div style="width: 148px; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle;
                                                text-align: Center; font-size: x-small;">
                                                CHARGEABLE WEIGHT
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center; vertical-align: middle; height: 34px;">
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 34px;">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 317px; height: 50px; border-bottom: 1px Solid Black; border-right: 1px Solid Black;
                                vertical-align: top;">
                                <table style="width: 317px; height: 50px; border-collapse: collapse; border-left: 2px Solid Black;"
                                    cellpadding="0px;">
                                    <tr style="height: 15px">
                                        <td style="text-align: center; vertical-align: top; height: 15px;" colspan="3">
                                            <div style="width: 74px; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle;
                                                text-align: Center; font-size: x-small;">
                                                DATE
                                            </div>
                                        </td>
                                        <td style="text-align: center; vertical-align: top; height: 15px; border-right: 1px Solid Black;"
                                            colspan="4">
                                            <div style="width: 74px; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle;
                                                text-align: Center; font-size: x-small;">
                                                TIME
                                            </div>
                                        </td>
                                        <td style="text-align: center; vertical-align: top; height: 15px;" colspan="3">
                                            <div style="width: 148px; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle;
                                                text-align: Center; font-size: x-small;">
                                                PICKED-UP FOR
                                                <img src="../images/OCS_Transparent.png" height="15px" />
                                                BY
                                            </div>
                                        </td>
                                    </tr>
                                    <tr style="height: 15px">
                                        <td style="text-align: right; vertical-align: middle; height: 15px;">
                                            /
                                        </td>
                                        <td style="text-align: right; vertical-align: middle; height: 15px;">
                                            /
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 15px;">
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 15px;">
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 15px;">
                                            :
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 15px;">
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 15px; border-right: 1px Solid Black;">
                                            AM
                                        </td>
                                        <td style="text-align: center; vertical-align: top; height: 15px;" colspan="3">
                                        </td>
                                    </tr>
                                    <tr style="height: 15px">
                                        <td style="text-align: center; vertical-align: middle; height: 15px; width: 24px;">
                                            M
                                        </td>
                                        <td style="text-align: Center; vertical-align: middle; height: 15px; width: 24px;">
                                            D
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 15px; width: 24px;">
                                            Y
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 15px; width: 24px;">
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 15px; width: 24px;">
                                            :
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 15px; width: 24px;">
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 15px; border-right: 1px Solid Black;">
                                            PM
                                        </td>
                                        <td style="text-align: center; vertical-align: top; height: 15px;" colspan="3">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 316px; height: 50px; border-bottom: 1px Solid Black; border-right: 1px Solid Black;
                                vertical-align: top;">
                                <table style="width: 316px; height: 50px; border-collapse: collapse;" cellpadding="0px;">
                                    <tr style="height: 15px">
                                        <td style="text-align: center; vertical-align: top; height: 15px;" colspan="3">
                                            <div style="width: 74px; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle;
                                                text-align: Center; font-size: x-small;">
                                                DATE
                                            </div>
                                        </td>
                                        <td style="text-align: center; vertical-align: top; height: 15px; border-right: 1px Solid Black;"
                                            colspan="4">
                                            <div style="width: 74px; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle;
                                                text-align: Center; font-size: x-small;">
                                                TIME
                                            </div>
                                        </td>
                                        <td style="text-align: center; vertical-align: top; height: 15px;" colspan="3">
                                            <div style="width: 148px; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle;
                                                text-align: Center; font-size: x-small;">
                                                CONSIGNEE'S SIGNATURE
                                            </div>
                                        </td>
                                    </tr>
                                    <tr style="height: 15px">
                                        <td style="text-align: right; vertical-align: middle; height: 15px;">
                                            /
                                        </td>
                                        <td style="text-align: right; vertical-align: middle; height: 15px;">
                                            /
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 15px;">
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 15px;">
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 15px;">
                                            :
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 15px;">
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 15px; border-right: 1px Solid Black;">
                                            AM
                                        </td>
                                        <td style="text-align: center; vertical-align: top; height: 15px;" colspan="3">
                                        </td>
                                    </tr>
                                    <tr style="height: 15px">
                                        <td style="text-align: center; vertical-align: middle; height: 15px; width: 24px;">
                                            M
                                        </td>
                                        <td style="text-align: Center; vertical-align: middle; height: 15px; width: 24px;">
                                            D
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 15px; width: 24px;">
                                            Y
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 15px; width: 24px;">
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 15px; width: 24px;">
                                            :
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 15px; width: 24px;">
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 15px; border-right: 1px Solid Black;">
                                            PM
                                        </td>
                                        <td style="text-align: center; vertical-align: top; height: 15px;" colspan="3">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 85px; vertical-align: top;">
                    <table cellpadding="0" style="border-collapse: collapse; font-size: x-small; font-family: Calibri;">
                        <tr>
                            <td style="height: 53px; width: 85px; border-top: 2px Solid Black; border-left: 2px Solid Black;
                                border-right: 2px Solid Black; border-bottom: 1px Solid Black; vertical-align: top;">
                                <div style="width: 100%; height: 15px; border-bottom: 1px Solid Black; margin-left: -1px;
                                    padding-left: 2px; vertical-align: middle;">
                                    DESTINATION
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 50px; width: 85px; border-top: 2px Solid Black; border-left: 2px Solid Black;
                                border-right: 2px Solid Black; border-bottom: 1px Solid Black; vertical-align: top;">
                                <div style="width: 100%; height: 15px; border-bottom: 1px Solid Black; margin-left: -1px;
                                    padding-left: 2px; vertical-align: middle;">
                                    PIECES
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 50px; width: 85px; border-top: 2px Solid Black; border-right: 2px Solid Black;
                                border-left: 1px Solid Black; border-bottom: 1px Solid Black; vertical-align: top;">
                                <div style="width: 100%; height: 15px; border-bottom: 1px Solid Black; margin-left: -1px;
                                    padding-left: 2px; vertical-align: middle;">
                                    WEIGHT
                                </div>
                                <div style="width: 100%; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle;">
                                    (1) ADX/APX
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 50px; width: 85px; border-top: 2px Solid Black; border-right: 2px Solid Black;
                                border-left: 1px Solid Black; border-bottom: 1px Solid Black; vertical-align: top;">
                                <div style="width: 100%; height: 15px; border-bottom: 1px Solid Black; margin-left: -1px;
                                    padding-left: 2px; vertical-align: middle;">
                                    WEIGHT CHARGE
                                </div>
                                <div style="width: 100%; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle;">
                                    (2) HDLG
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 182.5px; width: 85px; border-top: 2px Solid Black; border-right: 2px Solid Black;
                                border-left: 1px Solid Black; border-bottom: 1px Solid Black; vertical-align: top;">
                                <div style="width: 100%; height: 18px; border-bottom: 1px Solid Black; margin-left: -1px;
                                    padding-left: 2px; vertical-align: middle;">
                                    SURCHARGE
                                </div>
                                <table cellpadding="0" cellspacing="0" style="width: 84px; border-collapse: collapse;">
                                    <tr style="height: 23.5px;">
                                        <td style="width: 20px; border-right: 1px Solid Black; border-bottom: 1px Solid Black;">
                                        </td>
                                        <td style="width: 60px; border-bottom: 1px Solid Black;">
                                        </td>
                                    </tr>
                                    <tr style="height: 23.5px;">
                                        <td style="width: 20px; border-right: 1px Solid Black; border-bottom: 1px Solid Black;">
                                        </td>
                                        <td style="width: 60px; border-bottom: 1px Solid Black;">
                                        </td>
                                    </tr>
                                    <tr style="height: 23.5px;">
                                        <td style="width: 20px; border-right: 1px Solid Black; border-bottom: 1px Solid Black;">
                                            (3)
                                        </td>
                                        <td style="width: 60px; border-bottom: 1px Solid Black;">
                                            GST
                                        </td>
                                    </tr>
                                    <tr style="height: 23.5px;">
                                        <td style="width: 20px; border-right: 1px Solid Black; border-bottom: 1px Solid Black;">
                                        </td>
                                        <td style="width: 60px; border-bottom: 1px Solid Black;">
                                        </td>
                                    </tr>
                                    <tr style="height: 23.5px;">
                                        <td style="width: 20px; border-right: 1px Solid Black; border-bottom: 1px Solid Black;">
                                            (4)
                                        </td>
                                        <td style="width: 60px; border-bottom: 1px Solid Black;">
                                            OTHERS
                                        </td>
                                    </tr>
                                    <tr style="height: 23.5px;">
                                        <td style="width: 20px; border-right: 1px Solid Black; border-bottom: 1px Solid Black;">
                                        </td>
                                        <td style="width: 60px; border-bottom: 1px Solid Black;">
                                        </td>
                                    </tr>
                                    <tr style="height: 23.5px;">
                                        <td style="width: 20px; border-right: 1px Solid Black;">
                                        </td>
                                        <td style="width: 60px;">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 50px; width: 85px; border-top: 2px Solid Black; border-right: 2px Solid Black;
                                border-left: 1px Solid Black; border-bottom: 1px Solid Black; vertical-align: top;">
                                <div style="width: 100%; height: 15px; border-bottom: 1px Solid Black; margin-left: -1px;
                                    padding-left: 2px; vertical-align: middle;">
                                    COLLECT CHARGE
                                </div>
                                <div style="width: 100%; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle;">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 72px; width: 85px; border-top: 2px Solid Black; border-right: 2px Solid Black;
                                border-left: 1px Solid Black; border-bottom: 1px Solid Black; vertical-align: top;">
                                <div style="width: 100%; height: 30px; border-bottom: 1px Solid Black; margin-left: -1px;
                                    padding-left: 2px; vertical-align: middle;">
                                    TOTAL CHARGE (1+2+3+4)
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <hr style="border: 1px dashed black;" />
        <table style="width: 720px;" cellpadding="0" cellspacing="0">
            <tr>
                <td style="width: 635px;">
                    <table style="width: 634px; border-collapse: collapse; font-size: x-small; font-family: Calibri;"
                        cellpadding="0px">
                        <tr>
                            <td style="width: 317px;">
                                <table style="width: 317px; border-collapse: collapse; font-size: x-small; font-family: Calibri;">
                                    <tr>
                                        <td style="width: 130px; height: 50px; border-top: 2px Solid Black; border-left: 2px Solid Black;
                                            border-bottom: 1px Solid Black; border-right: 1px Solid Black; vertical-align: top;">
                                            <div style="width: 100%; height: 15px; border-bottom: 1px Solid Black; margin-left: -1px;
                                                padding-left: 2px; vertical-align: middle;">
                                                SHIPPER'S ACCOUNT
                                            </div>
                                        </td>
                                        <td style="width: 58px; height: 50px; border-top: 2px Solid Black; border-left: 1px Solid Black;
                                            border-bottom: 1px Solid Black; border-right: 2px Solid Black; vertical-align: top;">
                                            <div style="width: 100%; height: 15px; border-bottom: 1px Solid Black; margin-left: -1px;
                                                padding-left: 2px; vertical-align: middle;">
                                                ORIGIN
                                            </div>
                                        </td>
                                        <td style="width: 129px; border-bottom: 2px Solid Black;">
                                            <img src="../images/OCS_Transparent.png" height="50px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 130px; height: 50px; border-top: 1px Solid Black; border-left: 2px Solid Black;
                                            border-bottom: 1px Solid Black; border-right: 1px Solid Black; vertical-align: top;">
                                            <div style="width: 100%; height: 15px; border-bottom: 1px Solid Black; margin-left: -1px;
                                                padding-left: 2px; vertical-align: middle;">
                                                SECTION CODE
                                            </div>
                                        </td>
                                        <td style="width: 187px; height: 50px; border-right: 2px Solid Black; border-bottom: 1px Solid Black;"
                                            colspan="2">
                                            <div style="width: 100%; height: 14px; border-bottom: 1px Solid Black; margin-left: -1px;
                                                padding-left: 2px; vertical-align: middle;">
                                                SERVICE (PLEASE CHECK)
                                            </div>
                                            <table style="width: 100%; height: 35px; border-collapse: collapse;">
                                                <tr>
                                                    <td style="border-right: 1px Solid Black; width: 25%; text-align: center;">
                                                        DOC
                                                    </td>
                                                    <td style="border-right: 1px Solid Black; width: 25%">
                                                    </td>
                                                    <td style="border-right: 1px Solid Black; width: 25%">
                                                    </td>
                                                    <td style="width: 25%; text-align: center;">
                                                        SPS
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="width: 100%; border-left: 2px Solid Black; border-right: 1px Solid Black;
                                            height: 125px; vertical-align: top;">
                                            <div style="width: 100%; height: 14px; margin-left: -1px; padding-left: 2px; vertical-align: middle;">
                                                FROM (SHIPPER)
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 130px; height: 34px; border-bottom: 1px Solid Black; border-left: 2px Solid Black;">
                                        </td>
                                        <td style="width: 187px; height: 34px; border: 1px Solid Black; vertical-align: top;"
                                            colspan="2">
                                            <div style="width: 100%; height: 14px; margin-left: -1px; padding-left: 2px; vertical-align: top;">
                                                PHONE
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="width: 100%; border-left: 2px Solid Black; border-right: 1px Solid Black;
                                            border-bottom: 1px Solid Black; height: 34px; vertical-align: top;">
                                            <div style="width: 100%; height: 14px; margin-left: -1px; padding-left: 2px; vertical-align: middle;">
                                                SHIPPER'S NAME/DEPT. (SIGNATURE)
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="width: 100%; border-left: 2px Solid Black; border-right: 1px Solid Black;
                                            border-bottom: 1px Solid Black; height: 34px; vertical-align: top;">
                                            <div style="width: 100%; height: 14px; margin-left: -1px; padding-left: 2px; vertical-align: middle;">
                                                SHIPPER'S REFERENCE
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 317px;">
                                <table style="width: 317px; border-collapse: collapse; font-size: x-small; font-family: Calibri;">
                                    <tr>
                                        <td colspan="3" style="width: 100%; height: 105px; border-bottom: 2px Solid Black;
                                            text-align: center; font-size: large; font-family: IDAutomationHC39M;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="width: 100%; border-left: 1px Solid Black; border-right: 1px Solid Black;
                                            height: 125px; vertical-align: top;">
                                            <div style="width: 100%; height: 14px; margin-left: -1px; padding-left: 2px; vertical-align: middle;">
                                                TO (CONSIGNEE)
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 130px; height: 34px; border-bottom: 1px Solid Black; border-left: 1px Solid Black;">
                                        </td>
                                        <td style="width: 187px; height: 34px; border: 1px Solid Black; vertical-align: top;"
                                            colspan="2">
                                            <div style="width: 100%; height: 14px; margin-left: -1px; padding-left: 2px; vertical-align: top;">
                                                PHONE
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="width: 100%; border-left: 1px Solid Black; border-right: 1px Solid Black;
                                            border-bottom: 1px Solid Black; height: 34px; vertical-align: top;">
                                            <div style="width: 100%; height: 14px; margin-left: -1px; padding-left: 2px; vertical-align: middle;">
                                                ATTN
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="width: 100%; border-left: 1px Solid Black; border-right: 1px Solid Black;
                                            border-bottom: 1px Solid Black; height: 34px; vertical-align: top;">
                                            <div style="width: 100%; height: 14px; margin-left: -1px; padding-left: 2px; vertical-align: middle;">
                                                OTHER: (SPECIAL)
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 633px; height: 73px; vertical-align: top; border-bottom: 1px Solid Black;"
                                colspan="2">
                                <div style="border-left: 2px Solid Black; width: 632px; height: 73px; border-right: 1px Solid Black;">
                                    <div style="width: 100%; height: 15px; border-bottom: 1px Solid Black; margin-left: -1px;
                                        padding-left: 2px; vertical-align: middle; text-align: center;">
                                        DESCRIPTION OF CONTENTS
                                    </div>
                                    <div style="width: 100%; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle;
                                        text-align: left; font-size: x-small;">
                                        Please attach a commercial invoice of SPS
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 317px; height: 50px; border-bottom: 1px Solid Black; border-right: 1px Solid Black;
                                vertical-align: top;">
                                <table style="width: 317px; height: 50px; border-collapse: collapse; border-left: 2px Solid Black;"
                                    cellpadding="0px;">
                                    <tr>
                                        <td style="text-align: center; vertical-align: top; height: 15px;">
                                            <div style="width: 148px; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle;
                                                text-align: Center; font-size: x-small;">
                                                DECLARED VALUE FOR CUSTOMS
                                            </div>
                                        </td>
                                        <td style="text-align: center; vertical-align: top; height: 15px;">
                                            <div style="width: 148px; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle;
                                                text-align: Center; font-size: x-small;">
                                                INSURANCE
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center; vertical-align: middle; height: 34px;">
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 34px;">
                                            YES/NO
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 316px; height: 50px; border-bottom: 1px Solid Black; border-right: 1px Solid Black;
                                vertical-align: top;">
                                <table style="width: 316px; height: 50px; border-collapse: collapse;" cellpadding="0px;">
                                    <tr>
                                        <td style="text-align: center; vertical-align: top; height: 15px;">
                                            <div style="width: 148px; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle;
                                                text-align: left; font-size: x-small;">
                                                DIMENSIONS (CM)
                                            </div>
                                        </td>
                                        <td style="text-align: center; vertical-align: top; height: 15px;">
                                            <div style="width: 148px; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle;
                                                text-align: Center; font-size: x-small;">
                                                CHARGEABLE WEIGHT
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center; vertical-align: middle; height: 34px;">
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 34px;">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 317px; height: 50px; border-bottom: 1px Solid Black; border-right: 1px Solid Black;
                                vertical-align: top;">
                                <table style="width: 317px; height: 50px; border-collapse: collapse; border-left: 2px Solid Black;"
                                    cellpadding="0px;">
                                    <tr style="height: 15px">
                                        <td style="text-align: center; vertical-align: top; height: 15px;" colspan="3">
                                            <div style="width: 74px; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle;
                                                text-align: Center; font-size: x-small;">
                                                DATE
                                            </div>
                                        </td>
                                        <td style="text-align: center; vertical-align: top; height: 15px; border-right: 1px Solid Black;"
                                            colspan="4">
                                            <div style="width: 74px; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle;
                                                text-align: Center; font-size: x-small;">
                                                TIME
                                            </div>
                                        </td>
                                        <td style="text-align: center; vertical-align: top; height: 15px;" colspan="3">
                                            <div style="width: 148px; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle;
                                                text-align: Center; font-size: x-small;">
                                                PICKED-UP FOR
                                                <img src="../images/OCS_Transparent.png" height="15px" />
                                                BY
                                            </div>
                                        </td>
                                    </tr>
                                    <tr style="height: 15px">
                                        <td style="text-align: right; vertical-align: middle; height: 15px;">
                                            /
                                        </td>
                                        <td style="text-align: right; vertical-align: middle; height: 15px;">
                                            /
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 15px;">
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 15px;">
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 15px;">
                                            :
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 15px;">
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 15px; border-right: 1px Solid Black;">
                                            AM
                                        </td>
                                        <td style="text-align: center; vertical-align: top; height: 15px;" colspan="3">
                                        </td>
                                    </tr>
                                    <tr style="height: 15px">
                                        <td style="text-align: center; vertical-align: middle; height: 15px; width: 24px;">
                                            M
                                        </td>
                                        <td style="text-align: Center; vertical-align: middle; height: 15px; width: 24px;">
                                            D
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 15px; width: 24px;">
                                            Y
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 15px; width: 24px;">
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 15px; width: 24px;">
                                            :
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 15px; width: 24px;">
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 15px; border-right: 1px Solid Black;">
                                            PM
                                        </td>
                                        <td style="text-align: center; vertical-align: top; height: 15px;" colspan="3">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 316px; height: 50px; border-bottom: 1px Solid Black; border-right: 1px Solid Black;
                                vertical-align: top;">
                                <table style="width: 316px; height: 50px; border-collapse: collapse;" cellpadding="0px;">
                                    <tr style="height: 15px">
                                        <td style="text-align: center; vertical-align: top; height: 15px;" colspan="3">
                                            <div style="width: 74px; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle;
                                                text-align: Center; font-size: x-small;">
                                                DATE
                                            </div>
                                        </td>
                                        <td style="text-align: center; vertical-align: top; height: 15px; border-right: 1px Solid Black;"
                                            colspan="4">
                                            <div style="width: 74px; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle;
                                                text-align: Center; font-size: x-small;">
                                                TIME
                                            </div>
                                        </td>
                                        <td style="text-align: center; vertical-align: top; height: 15px;" colspan="3">
                                            <div style="width: 148px; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle;
                                                text-align: Center; font-size: x-small;">
                                                CONSIGNEE'S SIGNATURE
                                            </div>
                                        </td>
                                    </tr>
                                    <tr style="height: 15px">
                                        <td style="text-align: right; vertical-align: middle; height: 15px;">
                                            /
                                        </td>
                                        <td style="text-align: right; vertical-align: middle; height: 15px;">
                                            /
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 15px;">
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 15px;">
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 15px;">
                                            :
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 15px;">
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 15px; border-right: 1px Solid Black;">
                                            AM
                                        </td>
                                        <td style="text-align: center; vertical-align: top; height: 15px;" colspan="3">
                                        </td>
                                    </tr>
                                    <tr style="height: 15px">
                                        <td style="text-align: center; vertical-align: middle; height: 15px; width: 24px;">
                                            M
                                        </td>
                                        <td style="text-align: Center; vertical-align: middle; height: 15px; width: 24px;">
                                            D
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 15px; width: 24px;">
                                            Y
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 15px; width: 24px;">
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 15px; width: 24px;">
                                            :
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 15px; width: 24px;">
                                        </td>
                                        <td style="text-align: center; vertical-align: middle; height: 15px; border-right: 1px Solid Black;">
                                            PM
                                        </td>
                                        <td style="text-align: center; vertical-align: top; height: 15px;" colspan="3">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 85px; vertical-align: top;">
                    <table cellpadding="0" style="border-collapse: collapse; font-size: x-small; font-family: Calibri;">
                        <tr>
                            <td style="height: 53px; width: 85px; border-top: 2px Solid Black; border-left: 2px Solid Black;
                                border-right: 2px Solid Black; border-bottom: 1px Solid Black; vertical-align: top;">
                                <div style="width: 100%; height: 15px; border-bottom: 1px Solid Black; margin-left: -1px;
                                    padding-left: 2px; vertical-align: middle;">
                                    DESTINATION
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 50px; width: 85px; border-top: 2px Solid Black; border-left: 2px Solid Black;
                                border-right: 2px Solid Black; border-bottom: 1px Solid Black; vertical-align: top;">
                                <div style="width: 100%; height: 15px; border-bottom: 1px Solid Black; margin-left: -1px;
                                    padding-left: 2px; vertical-align: middle;">
                                    PIECES
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 50px; width: 85px; border-top: 2px Solid Black; border-right: 2px Solid Black;
                                border-left: 1px Solid Black; border-bottom: 1px Solid Black; vertical-align: top;">
                                <div style="width: 100%; height: 15px; border-bottom: 1px Solid Black; margin-left: -1px;
                                    padding-left: 2px; vertical-align: middle;">
                                    WEIGHT
                                </div>
                                <div style="width: 100%; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle;">
                                    (1) ADX/APX
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 50px; width: 85px; border-top: 2px Solid Black; border-right: 2px Solid Black;
                                border-left: 1px Solid Black; border-bottom: 1px Solid Black; vertical-align: top;">
                                <div style="width: 100%; height: 15px; border-bottom: 1px Solid Black; margin-left: -1px;
                                    padding-left: 2px; vertical-align: middle;">
                                    WEIGHT CHARGE
                                </div>
                                <div style="width: 100%; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle;">
                                    (2) HDLG
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 182.5px; width: 85px; border-top: 2px Solid Black; border-right: 2px Solid Black;
                                border-left: 1px Solid Black; border-bottom: 1px Solid Black; vertical-align: top;">
                                <div style="width: 100%; height: 18px; border-bottom: 1px Solid Black; margin-left: -1px;
                                    padding-left: 2px; vertical-align: middle;">
                                    SURCHARGE
                                </div>
                                <table cellpadding="0" cellspacing="0" style="width: 84px; border-collapse: collapse;">
                                    <tr style="height: 23.5px;">
                                        <td style="width: 20px; border-right: 1px Solid Black; border-bottom: 1px Solid Black;">
                                        </td>
                                        <td style="width: 60px; border-bottom: 1px Solid Black;">
                                        </td>
                                    </tr>
                                    <tr style="height: 23.5px;">
                                        <td style="width: 20px; border-right: 1px Solid Black; border-bottom: 1px Solid Black;">
                                        </td>
                                        <td style="width: 60px; border-bottom: 1px Solid Black;">
                                        </td>
                                    </tr>
                                    <tr style="height: 23.5px;">
                                        <td style="width: 20px; border-right: 1px Solid Black; border-bottom: 1px Solid Black;">
                                            (3)
                                        </td>
                                        <td style="width: 60px; border-bottom: 1px Solid Black;">
                                            GST
                                        </td>
                                    </tr>
                                    <tr style="height: 23.5px;">
                                        <td style="width: 20px; border-right: 1px Solid Black; border-bottom: 1px Solid Black;">
                                        </td>
                                        <td style="width: 60px; border-bottom: 1px Solid Black;">
                                        </td>
                                    </tr>
                                    <tr style="height: 23.5px;">
                                        <td style="width: 20px; border-right: 1px Solid Black; border-bottom: 1px Solid Black;">
                                            (4)
                                        </td>
                                        <td style="width: 60px; border-bottom: 1px Solid Black;">
                                            OTHERS
                                        </td>
                                    </tr>
                                    <tr style="height: 23.5px;">
                                        <td style="width: 20px; border-right: 1px Solid Black; border-bottom: 1px Solid Black;">
                                        </td>
                                        <td style="width: 60px; border-bottom: 1px Solid Black;">
                                        </td>
                                    </tr>
                                    <tr style="height: 23.5px;">
                                        <td style="width: 20px; border-right: 1px Solid Black;">
                                        </td>
                                        <td style="width: 60px;">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 50px; width: 85px; border-top: 2px Solid Black; border-right: 2px Solid Black;
                                border-left: 1px Solid Black; border-bottom: 1px Solid Black; vertical-align: top;">
                                <div style="width: 100%; height: 15px; border-bottom: 1px Solid Black; margin-left: -1px;
                                    padding-left: 2px; vertical-align: middle;">
                                    COLLECT CHARGE
                                </div>
                                <div style="width: 100%; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle;">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 72px; width: 85px; border-top: 2px Solid Black; border-right: 2px Solid Black;
                                border-left: 1px Solid Black; border-bottom: 1px Solid Black; vertical-align: top;">
                                <div style="width: 100%; height: 30px; border-bottom: 1px Solid Black; margin-left: -1px;
                                    padding-left: 2px; vertical-align: middle;">
                                    TOTAL CHARGE (1+2+3+4)
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>--%>
        <asp:PlaceHolder ID="ph1" runat="server"></asp:PlaceHolder>
    </div>
    </form>
</body>
</html>
