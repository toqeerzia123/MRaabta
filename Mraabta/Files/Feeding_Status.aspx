<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Feeding_Status.aspx.cs" Inherits="MRaabta.Files.Feeding_Status" %>

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

    <%--    <style type="text/css" media="print">
        /*@media print {

            a[href]:after {
                content: none !important;
            }

            @page {
                margin-top: 5px;
           margin-bottom: 80px; 
                /*margin: 10px 37px 10px 37px;
            }

            body {
                padding-top: 10px;
                padding-bottom: 72px;
            }
        }*/
        @page {
            margin-top: 4mm;
            margin-bottom: 1mm;
        }

        body {
            padding-top: 10px;
            padding-bottom: 72px;
        }
    </style>--%>
    <%--    <style type="text/css" media="print">
        @page {
            size: auto; /* auto is the initial value */
            margin:10px 37px 10px 37px; /* this affects the margin in the printer settings */
             
        }
    </style>--%>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hf_Date" runat="server" />
        <asp:HiddenField ID="hf_Month" runat="server" />
        <asp:HiddenField ID="hf_Year" runat="server" />
        <asp:HiddenField ID="hf_Branch_Name" runat="server" />
        <div style="min-width: 800px;">
            <asp:Literal ID="lt_main" runat="server"></asp:Literal>
        </div>
        <div style="width: 100%; clear: both;">
            <center>
                <asp:Label ID="lbl_Message" runat="server" Font-Size="Medium"></asp:Label>
            </center>
        </div>
        <div style="width: 100%; clear: both;">
            <center>
                <asp:Label ID="lbl_report_name" Font-Size="Large" runat="server" CssClass="report_name"></asp:Label>
            </center>
        </div>
        <div style="width: 100%; clear: both;">
            <center>
                <asp:Label ID="lbl_total_record" Font-Size="Large" runat="server" CssClass="report_name"></asp:Label>
            </center>
        </div>
        <div style="float: left; width: 100%; clear: both;">
            <asp:ImageButton ID="btn_csv" runat="server" ImageUrl="~/images/csv.jpg" Width="48"
                OnClick="btnCSV_Click" Visible="false" ToolTip="Download CSV File" />
            <asp:ImageButton ID="btn_Excel" runat="server" ImageUrl="~/images/ExcelFile.png" Width="48"
                OnClick="btn_Excel_Click" Visible="false" ToolTip="Download Excel File" />
        </div>
        <br />
        <span id="Table_1" class="tbl-large" style="float: left; width: 75% !important;" runat="server">
            <asp:GridView ID="GridViewFeeding_Status" runat="server" AutoGenerateColumns="False"
                CssClass="mGrid" OnPageIndexChanging="GridViewFeeding_Status_OnPageIndexChanging"
                OnRowDataBound="GridViewFeeding_Status_RowDataBound" OnRowCreated="GridViewFeeding_Status_RowCreated"
                AlternatingRowStyle-CssClass="alt"
                BorderColor="#DEDFDE" BorderStyle="None" AllowPaging="True"
                BorderWidth="1px" PageSize="200" ShowFooter="true">
                <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                <Columns>
                    <asp:TemplateField HeaderText="S. No.">
                        <ItemTemplate>
                            <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="DATE" HeaderText="DATE"></asp:BoundField>
                    <asp:TemplateField HeaderText="RETAIL SALE">
                        <ItemTemplate>
                            <asp:Label ID="RETAIL_SALE" runat="server" Text='<%# Bind("RETAIL_SALE", "{0:N0}") %>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="TOTAL_RETAIL_SALE" runat="server" Font-Bold="true" Text='<%# Eval("Keyword") %>' />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="CASH RECOVERY">
                        <ItemTemplate>
                            <asp:Label ID="CASH_RECOVERY" runat="server" Text='<%# Bind("CASH_RECOVERY", "{0:N0}") %>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="TOTAL_CASH_RECOVERY" runat="server" Font-Bold="true" Text='<%# Eval("Keyword") %>' />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="COD">
                        <ItemTemplate>
                            <asp:Label ID="COD" runat="server" Text='<%# Bind("COD", "{0:N0}") %>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="TOTAL_COD" runat="server" Font-Bold="true" Text='<%# Eval("Keyword") %>' />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="MISC">
                        <ItemTemplate>
                            <asp:Label ID="MISC_RECEIPT" runat="server" Text='<%# Bind("MISC_RECEIPT", "{0:N0}") %>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="TOTAL_MISC_RECEIPT" runat="server" Font-Bold="true" Text='<%# Eval("Keyword") %>' />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="COD">
                        <ItemTemplate>
                            <asp:Label ID="COD_DEPOSITE" runat="server" Text='<%# Bind("COD_DEPOSITE", "{0:N0}") %>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="TOTAL_COD_DEPOSITE" runat="server" Font-Bold="true" Text='<%# Eval("Keyword") %>' />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="NON COD">
                        <ItemTemplate>
                            <asp:Label ID="DEPOSITE_NONCOD" runat="server" Text='<%# Bind("DEPOSITE_NONCOD", "{0:N0}") %>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="TOTAL_DEPOSITE_NONCOD" runat="server" Font-Bold="true" Text='<%# Eval("Keyword") %>' />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="DIFFERENCE">
                        <ItemTemplate>
                            <asp:Label ID="DIFFERENCE" runat="server" Text='<%# Bind("DIFFERENCE", "{0:N0}") %>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="TOTAL_DIFFERENCE" runat="server" Font-Bold="true" Text='<%# Eval("Keyword") %>' />
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </span>
    </form>
</body>
</html>
