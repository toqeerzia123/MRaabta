<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CIH_HBL_GLprint.aspx.cs" Inherits="MRaabta.Files.CIH_HBL_GLprint" %>
 

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1 {
            font-size: small;
            font-weight: bold;
        }

        .style2 {
            font-size: x-small;
        }

        .style3 {
            font-size: x-small;
            width: 45%;
        }

        .style4 {
            width: 35%;
        }

        .style5 {
            font-size: x-small;
            width: 28%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hf_from" runat="server" />
        <asp:HiddenField ID="hf_branch" runat="server" />
        <asp:HiddenField ID="hf_to" runat="server" />
        <asp:HiddenField ID="hf_ec" runat="server" />
        <asp:HiddenField ID="hf_status" runat="server" />
        <asp:HiddenField ID="hf_company" runat="server" />
        <div class="aspNetHidden">
        </div>
        <div class="aspNetHidden">
        </div>
        <div>
            <table border="0" width="100%">
                <tr>
                    <td width="25%">
                        <img src="../images/OCS_Transparent.png" style="width: 103px; height: 47px" />
                    </td>
                    <td align="center" colspan="2" class="style4">
                        <font color="#000000" face="verdana" size="1">Print Date &amp; Time:
                        <asp:Label ID="lbl_currentdate" runat="server"></asp:Label></font>
                    </td>
                    <td align="right" width="25%">
                        <font color="#000000" face="verdana" size="1">Created User :
                        <asp:Label ID="lbl_user" runat="server"></asp:Label></font>
                    </td>
                </tr>
                <tr>
                    <td width="25%">&nbsp;
                    </td>
                    <td align="center" colspan="2" class="style4">&nbsp;
                    </td>
                    <td align="right" width="25%">&nbsp;
                    </td>
                </tr>
            </table>
            <table border="0" width="100%">
                <tr>
                    <td width="25%">&nbsp;
                    </td>
                    <td align="center" colspan="2" width="50%">
                        <font color="#000000" face="verdana" size="1"><b style="font-size: small">
                            <asp:Label runat="server" Text="M&P Express Logistics Pvt. Ltd." ID="lbl_company"></asp:Label></b></font>
                    </td>
                    <td align="right" width="25%">
                        <font color="#000000" face="verdana" size="1"></font>
                    </td>
                </tr>
                <tr>
                    <td width="25%">&nbsp;
                    </td>
                    <td align="center" colspan="2" width="50%">
                        <font color="#000000" face="verdana"><span class="style1">Cash In Hand General Ledger (HBL Connect)</span></font>
                    </td>
                    <td align="right" width="25%">&nbsp;
                    </td>
                </tr>
            </table>
            <table border="0" width="100%">
                <tr>
                    <td width="25%" class="style2">
                        <font color="#000000" face="verdana" size="1" class="style2"><b>From Date :  </b>
                            <asp:Label ID="lbl_from" runat="server"></asp:Label></font>
                    </td>
                    <td align="center" colspan="2" class="style5">&nbsp;
                    </td>
                    <td align="right" width="25%">
                        <font color="#000000" face="verdana" size="1" class="style2"><b>To Date : </b>
                            <asp:Label ID="lbl_to" runat="server"></asp:Label></font>
                    </td>
                </tr>
                <tr>
                    <td width="25%" class="style2">
                        <font color="#000000" face="verdana" size="1" class="style2"><b>Branch :  </b>
                            <asp:Label ID="lbl_branch" runat="server"></asp:Label></font>
                    </td>
                    <td align="center" colspan="2" class="style5">&nbsp;
                    </td>
                    <td align="right" width="25%">
                        <font color="#000000" face="verdana" size="1" class="style2"><b runat="server" id="opening">Opening Balance:</b>
                            <asp:Label ID="lbl_opening_balnc" runat="server"></asp:Label></font> 
                    </td>
                </tr> 
            </table>
            <br />
            <hr style="color: Black; background-color: Black;" />
            <span id="Span1" class="tbl-large">
                <asp:GridView ID="GridView" runat="server" AutoGenerateColumns="false" CssClass="mGrid"
                    AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None" AllowPaging="false"
                    BorderWidth="1px" Width="100%">
                    <HeaderStyle ForeColor="#000000" Font-Size="Small" BackColor="#cccccc" Font-Names="verdana" />
                    <RowStyle ForeColor="#000000" Font-Size="X-Small" Font-Names="verdana" />
                    <Columns>
                        <asp:TemplateField HeaderText="S.No" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="center">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                      <asp:BoundField HeaderText="Branch" DataField="Branch" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="10%"></asp:BoundField> 
                        <asp:BoundField HeaderText="Date" DataField="date" ItemStyle-HorizontalAlign="Center"
                            ItemStyle-Width="10%"></asp:BoundField>
                        <asp:BoundField HeaderText="Description" DataField="cashtype" ItemStyle-HorizontalAlign="Center"
                            ItemStyle-Width="30%"></asp:BoundField>
                          <asp:BoundField HeaderText="Deposit Slip" DataField="DepositSlipNo" ItemStyle-HorizontalAlign="Right"
                        ItemStyle-Width="20%"></asp:BoundField>
                        <asp:BoundField HeaderText="Debit" DataField="dnote" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="right"
                            ItemStyle-Width="10%"></asp:BoundField>
                        <asp:BoundField HeaderText="Credit" DataField="cnote" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="right"
                            ItemStyle-Width="10%"></asp:BoundField>
                        <asp:BoundField HeaderText="Balance" DataField="balance" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="right"
                            ItemStyle-Width="10%"></asp:BoundField>
                    </Columns>
                </asp:GridView>
                <table border="0" width="100%">
                </table>
                <br />
                <table border="0" width="100%">
                    <tr>
                        <td width="25%" class="style2">
                            <font color="#000000" face="verdana" size="1" class="style2"><b></b></font>
                        </td>
                        <td align="center" colspan="2" class="style3">&nbsp;
                        </td>
                        <td align="right" width="25%">
                            <font color="#000000" face="verdana" size="1" class="style2"><b>
                                <b runat="server" id="closing">Closing Balance: </b>
                                <asp:Label ID="lbl_closing_balnc" runat="server" Style="font-size: x-small"></asp:Label></font><br />
                            <font color="#000000" face="verdana" size="1" class="style2"><b></td>
                    </tr>
                    <tr>
                        <td width="25%" class="style2" colspan="2">
                            <font color="#000000" face="verdana" size="1" class="style2"><b></b>
                                <asp:Label ID="lbl_amount_eng" runat="server" Style="font-size: x-small"></asp:Label></font>
                        </td>
                        <td align="right" width="25%">
                            <b></b>
                        </td>
                    </tr>
                </table>
                <table border="0" width="100%">
                    <tr>
                        <td>
                            <br />
                            <br />
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td width="50%" class="style2">
                            <font color="#000000" face="verdana" size="1" class="style2"><b style="text-align: left"></b></font>
                        </td>
                        <td></td>
                        <td align="right" width="50%">
                            <font color="#000000" face="verdana" size="1" class="style2"><b>Checked By:_______________________
                            </b></font>
                        </td>
                    </tr>
                </table>
            </span>
            <div class="page" style="page-break-before: always;">
            </div>
            <table border="0" width="100%">
                <tr>
                    <td align="center" width="100%">&nbsp;
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>