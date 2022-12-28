<%@ Page Title="" Language="C#" MasterPageFile="~/BtsMasterPage.Master" AutoEventWireup="true" CodeBehind="PackingMaterialRate.aspx.cs" Inherits="MRaabta.Files.PackingMaterialRate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../Bts_Css/Mazen.css" rel="stylesheet" />
    <asp:UpdatePanel runat="server" ID="panel1">
        <ContentTemplate>
            <div class="row main-body newPanel">
                <fieldset class="fieldsetSmall">
                    <span id="ContentPlaceHolder1_Errorid" style="color: Red; font-weight: bold;"></span>
                    <legend style="font-size: medium;"><b>Packing Material Invoice</b></legend>
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 140px">
                                <b>Request Number</b>
                            </td>
                            <td style="width: 140px">
                                <b>Customer</b>
                            </td>
                            <td style="width: 140px">
                                <b>Location</b>
                            </td>
                            <td style="width: 140px">
                                <b>Address</b>
                            </td>
                        </tr>
                        <tr>
                            
                            <td style="width: 170px">
                                <asp:TextBox ID="txtRequestNumber" Enabled="false" runat="server" />
                            </td>

                        
                            
                            <td style="width: 170px">
                                <asp:TextBox ID="txtCustomer" Enabled="false" runat="server" />
                            </td>
                        
                            
                            <td style="width: 140px">
                                <asp:TextBox ID="ddlLocation" Enabled="false" runat="server" />
                            </td>

                       
                            
                            <td style="width: 140px" >
                                <asp:TextBox Columns="40" TextMode="multiline" ID="txtAddress" Enabled="false" runat="server" />
                            </td>
                        </tr>
                    </table>
                    <fieldset class="fieldsetSmall">
                        <span id="ContentPlaceHolder1_Errorid" style="color: Red; font-weight: bold;"></span>
                        <legend style="font-size: medium;"><b>Packing Items Rates</b></legend>
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:GridView ID="grdPR" runat="server" AutoGenerateColumns="False" AllowPaging="True" OnRowDataBound="grdPR_RowDataBound"
                                OnPageIndexChanging="grdPR_PageIndexChanging" ShowFooter="true"
                                class="" Font-Size="Medium" CellPadding="4" ForeColor="#333333" GridLines="None" AllowSorting="True" Font-Bold="True" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Width="100%" EnableTheming="False" BorderColor="#DA7A4D" BorderStyle="Solid" BorderWidth="5px">
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:TemplateField HeaderText="SNO" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSNO" runat="server" Text='<%# Bind("SNO") %>'>  
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Item Name" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblItemID" runat="server" Text='<%# Bind("ItemID") %>'>  
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Item Name" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblItemName" runat="server" Text='<%# Bind("Name") %>'>  
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Item Size" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblItemSize" runat="server" Text='<%# Bind("Size") %>'>  
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Requested Quantity" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRequestedQuantity" runat="server" Text='<%# Bind("RequestQuantity") %>'>  
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Approved Quantity" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblApprovedQuantity" runat="server" Text='<%# Bind("ApprovedQuantity") %>'>  
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Issued Quantity" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIssuedQuantity" Enabled="true" runat="server" Text='<%# Bind("IssuedQuantity") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rate" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRate" Enabled="true" runat="server" Text='<%# Bind("Rate") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="GST" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGST" Enabled="true" runat="server" Text='<%# Bind("GST") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total Price" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotalPrice" Enabled="true" runat="server" Text='<%# Bind("TotalPrice") %>' />

                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="GST Amount" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGSTAmount" Enabled="true" runat="server" Text='<%# Bind("GSTAmount") %>' />

                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total Amount" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotalAmount" Enabled="true" runat="server" Text='<%# Bind("TotalAmount") %>' />

                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDetailID" runat="server" Visible="true" Text='<%# Bind("PackingRequestDetailID") %>'> 
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EditRowStyle BackColor="#999999" />
                                <FooterStyle BackColor="#DA7A4D" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#DA7A4D" Font-Bold="True" ForeColor="White" BorderColor="White" BorderStyle="Solid" BorderWidth="1px" />
                                <PagerStyle BackColor="#DA7A4D" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#fbefe9" ForeColor="#333333" Font-Bold="false" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="false" ForeColor="#333333" />
                                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                            </asp:GridView>
                        </div>
                    </fieldset>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table width="100%">
        <tr>
            <td style="width: 100px;" align="right">
                <asp:Button ID="IssueSubmit"  CssClass="btn btn-success" runat="server" OnClick="IssueSubmit_Click" Text="Save" />
            </td>
            <td style="width: 100px;">
                <asp:Button ID="btnPost"  CssClass="btn btn-warning" Visible="false" runat="server" OnClick="btnPost_Click" Text="Post" />
                <asp:Button ID="btnInvoice"  CssClass="btn btn-info" Visible="false" runat="server" OnClick="btnInvoice_Click" Text="View Invoice" />
            </td>
            

        </tr>
    </table>
</asp:Content>

