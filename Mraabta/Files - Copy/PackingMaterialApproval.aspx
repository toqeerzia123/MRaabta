<%@ Page Language="C#" Title="Packing Material Approval" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="PackingMaterialApproval.aspx.cs" Inherits="MRaabta.Files.PackingMaterialApproval" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../Bts_Css/Mazen.css" rel="stylesheet" />
    <asp:UpdatePanel runat="server" ID="panel1">
        <contenttemplate>
        <div class="row main-body newPanel">
            <fieldset class="fieldsetSmall">
               <span id="ContentPlaceHolder1_Errorid" style="color:Red;font-weight:bold;"></span>
               <legend style="font-size: medium;"><b>Packing Material Approval</b></legend>
               <table style="width:100%">
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
                        <%--<asp:DropDownList  ID="ddlRequestNo" Enabled="true" OnSelectedIndexChanged="ddlRequestNo_SelectedIndexChanged" AutoPostBack="true" runat="server" />--%>
                    </td>
                     <td style="width: 170px">
                        <asp:TextBox ID="txtCustomer" Enabled="false" runat="server" />
                    </td>
                   <td style="width: 140px">
                        <asp:TextBox ID="ddlLocation" Enabled="false" runat="server" />
                    </td>
                     <td style="width: 140px" >
                        <asp:TextBox Columns="40"  TextMode="multiline" ID="txtAddress" Enabled="false" runat="server" />
                    </td>
                     <td style="width: 100px;"><asp:Button ID="btnSubmit" CssClass="btn btn-success" runat="server" OnClick="btnSubmit_Click" Text="Approved" /></td>
                </tr>
                 
               </table>
              
               <fieldset class="fieldsetSmall">
                  <span id="ContentPlaceHolder1_Errorid" style="color:Red;font-weight:bold;"></span>
                  <legend style="font-size: medium;"><b>Selected Packing Items</b></legend>
                  <div class="col-lg-12 col-md-12 col-sm-12">
                       <asp:GridView ID="grdPR"  runat="server" AutoGenerateColumns="False" AllowPaging="True" OnRowDataBound="grdPR_RowDataBound"
            OnPageIndexChanging="grdPR_PageIndexChanging"
            class="" Font-Size="Medium" CellPadding="4" ForeColor="#333333" GridLines="None" AllowSorting="True" Font-Bold="True" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Width="100%" EnableTheming="False" BorderColor="#DA7A4D" BorderStyle="Solid" BorderWidth="5px">
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:TemplateField HeaderText="S.NO" Visible="true">
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

                <asp:TemplateField HeaderText="Tariff Each Item	" Visible="true">
                    <ItemTemplate>
                        <asp:Label ID="lblRate" runat="server" Text='<%# Bind("RATE") %>'>  
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="CompanyCost" Visible="true">
                    <ItemTemplate>
                        <asp:Label ID="lblCompanyCost" runat="server" Text='<%# Bind("COMPANYCOST") %>'>  
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>


                <asp:TemplateField HeaderText="Approved Quantity" Visible="true">
                    <ItemTemplate>
                         <center><asp:TextBox Width="100px" ID="txtApprovedQuantity" Text='<%# Bind("RequestQuantity") %>' onKeypress="if (event.keyCode < 46 || event.keyCode > 57 || event.keyCode == 47) event.returnValue = false;" runat="server" MaxLength="3" ></asp:TextBox> </center>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1"  runat="server" ErrorMessage="Required" ControlToValidate="txtApprovedQuantity" ForeColor="Red"  style="display:inline-block"></asp:RequiredFieldValidator>
                        <%--<asp:Label ID="lblApprovedQuantity" runat="server" Text='<%# Bind("ApprovedQuantity") %>'>  --%>
                       <%-- </asp:Label>--%>
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
            <RowStyle BackColor="#fbefe9" ForeColor="#333333"  Font-Bold="false" HorizontalAlign="Center" />
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
    </contenttemplate>
    </asp:UpdatePanel>
</asp:Content>

