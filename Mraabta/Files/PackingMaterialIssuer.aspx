<%@ Page Language="C#" Title="Packing Material Issuer" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="PackingMaterialIssuer.aspx.cs" Inherits="MRaabta.Files.PackingMaterialIssuer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../Bts_Css/Mazen.css" rel="stylesheet" />
    <script type="text/javascript">
        function isNumberKey(evt) {

            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {

                return false;
            }
            return true;
        }
        function isNumberKeydouble(evt) {
            debugger;
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                if (charCode == 46) {
                    return true;
                }
                return false;
            }
            return true;
        }
    </script>
    <asp:UpdatePanel runat="server" ID="panel1">
        <ContentTemplate>
            <div class="row main-body newPanel">
                <fieldset class="fieldsetSmall">
                    <span id="ContentPlaceHolder1_Errorid" style="color: Red; font-weight: bold;"></span>
                    <legend style="font-size: medium;"><b>Packing Material Issuer</b></legend>
                    <table style="width: 100%">
                        <tr>
                             <td style="width: 30px;"></td>
                            <td style="width: 150px">
                                <b>Request Number</b>
                            </td>
                            <td style="width: 150px">
                                <b>Customer</b>
                            </td>
                            <td style="width: 150px">
                                <b>Location</b>
                            </td>
                            <td style="width: 150px">
                                <b>Address</b>
                            </td>
                            <td style="width: 150px;display:none">
                                <b>Tariff Each Item</b>
                            </td>
                            <td style="width: 150px;display:none">
                                <b>Company Cost</b>
                            </td>
                        </tr>
                        <tr>
                             <td style="width: 30px;"></td>
                            <td style="width: 170px">
                                <asp:TextBox ID="txtRequestNumber" Enabled="false" runat="server" />
                            </td>
                            <td style="width: 170px">
                                <asp:TextBox ID="txtCustomer" Enabled="false" runat="server" />
                            </td>
                            <td style="width: 170px">
                                <asp:TextBox ID="ddlLocation" Enabled="false" runat="server" />
                            </td>
                            <td style="width: 170px">
                                <asp:TextBox Columns="40" TextMode="multiline" ID="txtAddress" Enabled="false" runat="server" />
                            </td>

                            
                            <td style="width: 140px;display:none">
                                <asp:TextBox ID="lblRate" Enabled="false" runat="server" />
                            </td>

                            
                            <td style="width: 140px;display:none">
                                <asp:TextBox ID="lblCompanyRate" Enabled="false" runat="server" />
                            </td>
                           
                        </tr>
                    </table>
                    <fieldset class="fieldsetSmall">
                        <span id="ContentPlaceHolder1_Errorid" style="color: Red; font-weight: bold;"></span>
                        <legend style="font-size: medium;"><b>Selected Packing Items</b></legend>
                        
                        <br />
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:GridView ID="grdPR" runat="server" AutoGenerateColumns="False" AllowPaging="True" OnRowDataBound="grdPR_RowDataBound"
                                OnPageIndexChanging="grdPR_PageIndexChanging"
                                class="" Font-Size="Medium" CellPadding="4" ForeColor="#333333" GridLines="None" AllowSorting="True" Font-Bold="True" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Width="100%" EnableTheming="False" BorderColor="#DA7A4D" BorderStyle="Solid" BorderWidth="5px">
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:TemplateField HeaderText="SNO" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSNO" runat="server" Text='<%# Bind("SNO") %>'>  
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
                                    <asp:TemplateField HeaderText="Tariff" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRate" runat="server" Text='<%# Bind("Rate") %>'>  
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Company Cost" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCompanyCost" runat="server" Text='<%# Bind("CompanyCost") %>'>  
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Issued Quantity" Visible="true">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtIssuedQuantity" Enabled="true" runat="server" onKeypress="if (event.keyCode < 46 || event.keyCode > 57 || event.keyCode == 47) event.returnValue = false;" runat="server" MaxLength="3" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required" ControlToValidate="txtIssuedQuantity" ForeColor="Red" Style="display: inline-block"></asp:RequiredFieldValidator>
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
                                <FooterStyle BackColor="#CC3399" Font-Bold="True" ForeColor="White" />
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
                        <br />
                        <table style="color:#333333;border-color:#DA7A4D;border-width:5px;border-style:Solid;font-size:Medium;font-weight:bold;text-decoration:none;width:100%;border-collapse:collapse;">
                            <tr style="color:White;background-color:#DA7A4D;border-color:White;border-width:1px;border-style:Solid;font-weight:bold;">
                                <td style="width: 140px"><b>Consignment No</b></td>
                                <td style="width: 140px"><b>Weight</b></td>
                                <td style="width: 140px"><b>Pieces</b></td>
                                <td style="width: 140px"><b>Remarks</b></td>
                                <td style="width: 140px"></td>
                            </tr>
                            <tr>
                                <td style="width: 140px">
                                    <asp:TextBox ID="txt_cn" runat="server" MaxLength="12" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                </td>
                                <td style="width: 140px">
                                    <asp:TextBox ID="txt_weight" runat="server" MaxLength="4" onkeypress="return isNumberKeydouble(event);" onchange="weightChange()"></asp:TextBox>
                                </td>
                                <td style="width: 140px">
                                    <asp:TextBox ID="txt_pieces" runat="server" MaxLength="3" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                </td>
                                <td style="width: 140px">
                                    <asp:TextBox ID="txt_remarks" runat="server" TextMode="multiline"></asp:TextBox>
                                </td>
                                <%--<td style="width: 140px">
                                    <aps:label id="lbltariff" runat="server" ></aps:label>
                                </td>
                                <td style="width: 140px">
                                    <aps:label id="lblcompanycost" runat="server" ></aps:label>
                                </td>--%>
                                <td style="width: 100px;">
                                    <asp:Button ID="IssueSubmit" CssClass="btn btn-success" runat="server" OnClick="IssueSubmit_Click" Text="Submit" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

