<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PC_ClosingBalances_QA_Print.aspx.cs" Inherits="MRaabta.Files.PC_ClosingBalances_QA_Print" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .mGrid {
            width: 99%;
            background-color: #fff;
            margin: 1px 5px 0px 5px;
            /*  border: solid 1px #525252; */
            border-collapse: collapse;
            font-size: 11px;
            font-family: Tahoma;
        }

            .mGrid th {
                background: #000;
                border-left: 1px solid #525252;
                color: #fff;
                font-size: 12px;
                padding: 10px 8px;
                text-align: center;
                text-transform: uppercase !important;
                white-space: nowrap;
            }

            .mGrid td {
                border: solid 1px #c1c1c1;
                white-space: nowrap;
                padding: 5px;
            }

        * {
            padding: 0;
            margin: 0;
        }

        td {
            text-align: center;
            vertical-align: middle;
            padding-left: 10px;
            border-color: Black;
        }

        .alignment {
            text-align: center;
            vertical-align: middle;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hf_from" runat="server" />
        <div style="width: 100%; clear: both; font-size: large;">
            <center>
                <label>PC Closing Balance</label>
            </center>
        </div>
        <div class="tbl-large">
            <asp:Repeater ID="rp_data" runat="server" OnItemDataBound="rp_data_ItemDataBound">
                <HeaderTemplate>
                    <table class="mGrid">
                        <tr>
                            <th rowspan="2">Seq#
                            </th>
                            <th rowspan="2">Zone
                            </th>
                            <th rowspan="2">Branch
                            </th>
                            <th colspan="2">
                                <asp:Label ID="lbl_company1" runat="server"></asp:Label>
                            </th>
                            <th colspan="2">
                                <asp:Label ID="lbl_company2" runat="server"></asp:Label>
                            </th>
                        </tr>
                        <tr>
                            <th>Cash In Hand
                            </th>
                            <th>Petty Cash
                            </th>
                            <th>Cash In Hand
                            </th>
                            <th>Petty Cash
                            </th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td style="text-align: center;">
                            <%# DataBinder.Eval(Container.DataItem, "SNO") %>
                        </td>
                        <td>
                            <%# DataBinder.Eval(Container.DataItem, "Zone") %>
                        </td>
                        <td>
                            <asp:HyperLink ID="hl_BranchName" runat="server" Target="_blank" Text='<%# DataBinder.Eval(Container.DataItem, "BranchName") %>'></asp:HyperLink>
                            <%--<%# DataBinder.Eval(Container.DataItem, "BranchName") %>--%>
                        </td>
                        <td style="text-align: right;">
                            <%# DataBinder.Eval(Container.DataItem, "C1CashInHand") %>
                        </td>
                        <td style="text-align: right;">
                            <%# DataBinder.Eval(Container.DataItem, "C1PettyCash") %>
                        </td>
                        <td style="text-align: right;">
                            <%# DataBinder.Eval(Container.DataItem, "C2CashInHand") %>
                        </td>
                        <td style="text-align: right;">
                            <%# DataBinder.Eval(Container.DataItem, "C2PettyCash") %>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </form>
</body>
</html>