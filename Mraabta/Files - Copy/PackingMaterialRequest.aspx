<%@ Page Language="C#" Title="Packing Material Request" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="PackingMaterialRequest.aspx.cs" Inherits="MRaabta.Files.PackingMaterialRequest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../Bts_Css/Mazen.css" rel="stylesheet" />
    <link href="../mazen/css/sweetalert.css" rel="stylesheet" />
    <script src="../mazen/js/sweetalert-dev.js"></script>
    <style>
        #overlay {
            position: fixed;
            z-index: 99;
            top: 0px;
            left: 0px;
            background-color: #f8f8f8;
            width: 100%;
            height: 100%;
            filter: Alpha(Opacity=90);
            opacity: 0.9;
            -moz-opacity: 0.9;
        }

        #theprogress {
            background-color: #fff;
            border: 1px solid #ccc;
            padding: 10px;
            width: 300px;
            line-height: 30px;
            text-align: center;
            filter: Alpha(Opacity=100);
            opacity: 1;
            -moz-opacity: 1;
        }

        #modalprogress {
            position: absolute;
            top: 40%;
            left: 50%;
            margin: -11px 0 0 -150px;
            color: #990000;
            font-weight: bold;
            font-size: 14px;
        }

        input[type="submit"]:disabled, input[disabled] :hover {
            color: -internal-light-dark(rgba(16, 16, 16, 0.3), white);
            background-color: #eeeeee !important;
            border-color: -internal-light-dark(rgba(118, 118, 118, 0.3), rgba(195, 195, 195, 0.3));
            /*background-color: -internal-light-dark(rgba(239, 239, 239, 0.3), rgba(19, 1, 1, 0.3));
    border-color: -internal-light-dark(rgba(118, 118, 118, 0.3), rgba(195, 195, 195, 0.3));*/
        }
    </style>
    <asp:UpdateProgress ID="prgLoadingStatus" runat="server" DynamicLayout="true">
        <ProgressTemplate>
            <div id="overlay">
                <div id="modalprogress">
                    <div id="theprogress">
                        <asp:Image ID="imgWaitIcon" runat="server" ImageAlign="AbsMiddle" ImageUrl="../mazen/images/wait.gif" />
                        Please wait...
                    </div>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel runat="server" ID="panel1">
        <ContentTemplate>
            <div class="row main-body newPanel">
                <fieldset class="fieldsetSmall">
                    <span id="ContentPlaceHolder1_Errorid-1" style="color: Red; font-weight: bold;"></span>
                    <legend style="font-size: medium;"><b>Packing Material Request</b></legend>
                    <table style="width: 100%; margin: 0px 20px;">
                        <tr>
                            <td style="width: 170px">
                                <b>Account Number</b>
                                <%-- <asp:CompareValidator ID="CompareValidator2" runat="server" 
     ControlToValidate="ddlAccountNo" ValueToCompare="-1" Operator="NotEqual"
     Type="Integer" ErrorMessage="Required" ForeColor="red" style="    display: inline-block;" />--%>
                            </td>
                            <td style="width: 170px">
                                <b>Location</b>
                            </td>
                            <td>
                                <b>Address</b>
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>

                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtAccountNumber" Width="200px" Enabled="true" runat="server" OnTextChanged="ddlAccountNo_SelectedIndexChanged" AutoPostBack="true" />
                                <asp:HiddenField ID="CustomerID" runat="server" />
                                <%-- <asp:DropDownList  ID="ddlAccountNo" Enabled="true" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAccountNo_SelectedIndexChanged"  />--%>
                            </td>
                            <td>
                                <asp:DropDownList Height="32px" Width="200px" ID="ddlLocation" Enabled="true" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged" />
                            </td>
                            <td style="width: 250px;">
                                <asp:TextBox TextMode="multiline" Columns="50" ID="txtAddress" Enabled="false" runat="server" />
                            </td>
                            <td style="width: 100px;">
                                <asp:Button ID="btnSubmit" CssClass="btn btn-success" runat="server" OnClick="btnSubmit_Click" Text="submit" /></td>
                            <td>
                                <asp:Button ID="btnReset" CssClass="btn btn-danger" runat="server" OnClick="btnReset_Click" Text="Reset" /></td>
                            <td>
                                <asp:Button ID="btnback" CssClass="btn btn-danger" runat="server" OnClick="btnBack_Click" Text="Back" /></td>
                        </tr>


                    </table>
                    <fieldset class="fieldsetSmall">
                        <span id="ContentPlaceHolder1_Errorid-2" style="color: Red; font-weight: bold;"></span>
                        <legend style="font-size: medium;"><b>Add Packing Item</b></legend>
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <table style="">

                                <tr>
                                    <td style="width: 127px">
                                        <b>Packing Item</b>
                                        <%--<asp:CompareValidator ID="CompareValidator1" runat="server" 
     ControlToValidate="ddlNeedPackingMaterialID" ValueToCompare="-1" Operator="NotEqual"
     Type="Integer" ErrorMessage="Required" ForeColor="red" style="    display: inline-block;" />--%>
                                    </td>
                                    <td style="width: 170px">
                                        <asp:DropDownList Height="32px" Width="150px" ID="ddlNeedPackingMaterialID" Enabled="true" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlNeedPackingMaterialID_SelectedIndexChanged" />
                                    </td>
                                    <td style="width: 50px">
                                        <b>Size</b>
                                    </td>
                                    <td style="width: 170px">
                                        <asp:DropDownList Height="32px" Width="150px" ID="ddlSize" Enabled="false" runat="server" />
                                    </td>
                                    <td style="width: 60px">
                                        <b>Quantity</b>
                                    </td>
                                    <td style="width: 140px">
                                        <asp:TextBox ID="txtQuantity" Enabled="false" runat="server" onKeypress="if (event.keyCode < 46 || event.keyCode > 57 || event.keyCode == 47) event.returnValue = false;" runat="server" MaxLength="3" />
                                    </td>
                                    <td style="width: 100px;">
                                        <asp:Button ID="btnAddItem" CssClass="btn btn-success" runat="server" OnClick="btnAddItem_Click" Text="Add Item" />

                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                            </table>
                            <div class="col-lg-12 col-md-12 col-sm-12">
                                <asp:GridView ID="grdPR" runat="server" OnRowDeleting="grdPR_RowDeleting" OnRowDataBound="grdPR_RowDataBound" 
                                    CssClass="Grid" AutoGenerateColumns="false" Font-Size="Medium" CellPadding="4" ForeColor="#333333"                                     
                                    GridLines="None" Font-Bold="True" Width="100%" EnableTheming="False" BorderColor="#DA7A4D" BorderStyle="Solid" 
                                    BorderWidth="5px" EmptyDataText="No Packing Material has been added.">
                                    <Columns>
                                        <asp:BoundField DataField="SNO" HeaderText="S. No." ItemStyle-Width="120">
                                            <ItemStyle Width="120px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Name" HeaderText="Item Name" ItemStyle-Width="120">
                                            <ItemStyle Width="120px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ItemID" HeaderText="Item ID" ItemStyle-Width="120">
                                            <ItemStyle Width="120px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Size" HeaderText="Size" ItemStyle-Width="120">
                                            <ItemStyle Width="120px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SizeID" HeaderText="Size ID" ItemStyle-Width="120">
                                            <ItemStyle Width="120px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RequestQuantity" HeaderText="Requested Quantity" ItemStyle-Width="120">
                                            <ItemStyle Width="120px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ApprovedQuantity" HeaderText="Approved Quantity" ItemStyle-Width="120">
                                            <ItemStyle Width="120px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IssuedQuantity" HeaderText="Issued Quantity" ItemStyle-Width="120">
                                            <ItemStyle Width="120px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TariffItem" HeaderText="Tariff Each Item" ItemStyle-Width="120">
                                            <ItemStyle Width="120px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CompanyCost" HeaderText="Company Cost" ItemStyle-Width="120">
                                            <ItemStyle Width="120px" />
                                        </asp:BoundField>
                                        <asp:CommandField ShowDeleteButton="True" ButtonType="Button" ControlStyle-Width="120" DeleteText="Remove">
                                            <ControlStyle Width="120px" CssClass="btn btn-danger" />
                                            <ItemStyle Width="120px" />
                                        </asp:CommandField>
                                    </Columns>
                                    <EditRowStyle BackColor="#DC3545" BorderColor="#DA7A4D" BorderStyle="Solid" BorderWidth="1px" />
                                    <FooterStyle BackColor="#CC3399" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#DA7A4D" Font-Bold="True" ForeColor="White" BorderColor="White" BorderStyle="Solid" BorderWidth="1px" />
                                    <PagerStyle BackColor="#DA7A4D" ForeColor="White" HorizontalAlign="Center" />
                                    <RowStyle BackColor="#fbefe9" ForeColor="#333333" Font-Bold="false" HorizontalAlign="Center" BorderColor="#DA7A4D" BorderStyle="Solid" BorderWidth="1px" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="false" ForeColor="#333333" />
                                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                </asp:GridView>


                            </div>
                        </div>
                    </fieldset>
                    <%--<fieldset class="fieldsetSmall">
                  <span id="ContentPlaceHolder1_Errorid" style="color:Red;font-weight:bold;"></span>
                  <legend style="font-size: medium;"><b>Packing Items Request List</b></legend>
                   <div class="col-lg-12 col-md-12 col-sm-12">
                       
                      <asp:GridView ID="grPML" runat="server" AutoGenerateColumns="false" OnRowCommand="grPML_RowCommand" OnRowDataBound="grPML_RowDataBound"
                       OnPageIndexChanging="grPML_PageIndexChanging" Font-Size="Medium" CellPadding="4" ForeColor="#333333" GridLines="None" Font-Bold="True" Width="100%" 
                          EnableTheming="False" BorderColor="#DA7A4D" BorderStyle="Solid" BorderWidth="5px"  EmptyDataText="No Request Generated">
                          <Columns>
                              <asp:TemplateField HeaderText="Packing Request ID" Visible="true">
                    <ItemTemplate>
                        <asp:Label ID="lblItemID" runat="server" Text='<%# Bind("PackingRequestID") %>'>  
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Item Name" Visible="true">
                    <ItemTemplate>
                        <asp:Label ID="lblItemName" runat="server" Text='<%# Bind("ItemID") %>'>  
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Requested Quantity" Visible="true">
                    <ItemTemplate>
                        <asp:Label ID="lblRequestedQuantity" runat="server" Text='<%# Bind("RequestQuantity") %>'>  
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
                          <EditRowStyle BackColor="#999999" BorderColor="#DA7A4D" BorderStyle="Solid" BorderWidth="1px" />
                          <FooterStyle BackColor="#CC3399" Font-Bold="True" ForeColor="White" />
                          <HeaderStyle BackColor="#DA7A4D" Font-Bold="True" ForeColor="White" BorderColor="White" BorderStyle="Solid" BorderWidth="1px" />
                          <PagerStyle BackColor="#DA7A4D" ForeColor="White" HorizontalAlign="Center" />
                          <RowStyle BackColor="#fbefe9" ForeColor="#333333"  Font-Bold="false" HorizontalAlign="Center" BorderColor="#DA7A4D" BorderStyle="Solid" BorderWidth="1px" />
                          <SelectedRowStyle BackColor="#E2DED6" Font-Bold="false" ForeColor="#333333" />
                          <SortedAscendingCellStyle BackColor="#E9E7E2" />
                          <SortedAscendingHeaderStyle BackColor="#506C8C" />
                          <SortedDescendingCellStyle BackColor="#FFFDF8" />
                         <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                    </asp:GridView>
                  </div>
               </fieldset>--%>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
