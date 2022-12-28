<%@ Page Language="C#" Title ="Packing Material Approval List" AutoEventWireup="true"  MasterPageFile="~/BtsMasterPage.Master"  CodeBehind="PackingMaterialApprovalList.aspx.cs" Inherits="MRaabta.Files.PackingMaterialApprovalList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../Bts_Css/Mazen.css" rel="stylesheet" />
<asp:UpdatePanel runat="server" ID="panel1">
    <ContentTemplate>
        <div class="row main-body newPanel">
            <fieldset class="fieldsetSmall">
               <span id="ContentPlaceHolder1_Errorid" style="color:Red;font-weight:bold;"></span>
               <legend style="font-size: medium;"><b>Packing Matreial Approval List</b></legend>
               <asp:GridView ID="grPML" runat="server" AutoGenerateColumns="false"  AllowPaging="True"
                       OnPageIndexChanging="grPML_PageIndexChanging" Font-Size="Medium" CellPadding="4" ForeColor="#333333" GridLines="None" Font-Bold="True" Width="100%" 
                          EnableTheming="False" BorderColor="#DA7A4D" BorderStyle="Solid" BorderWidth="5px"  EmptyDataText="No Request Generated">
                          <Columns>
                              <asp:TemplateField HeaderText="Packing Request ID" Visible="true">
                    <ItemTemplate>
                        
                        <asp:LinkButton ID="linkRequesID" runat="server" OnClick="linkRequesID_Click" Text='<%# Bind("PackingRequestID") %>'>  
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Account NO" Visible="true">
                    <ItemTemplate>
                        <asp:Label ID="lblAccountNO" runat="server" Text='<%# Bind("accountNo") %>'>  
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Account Name" Visible="true">
                    <ItemTemplate>
                        <asp:Label ID="lblAccountName" runat="server" Text='<%# Bind("name") %>'>  
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Location Name" Visible="true">
                    <ItemTemplate>
                        <asp:Label ID="lblLocationName" runat="server" Text='<%# Bind("LocationID") %>'>  
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Request Date" Visible="true">
                    <ItemTemplate>
                        <asp:Label ID="lblRequestDate" runat="server" Text='<%# Bind("RequestDate") %>'>  
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Approve Date" Visible="true">
                    <ItemTemplate>
                        <asp:Label ID="lblApproveDate" runat="server" Text='<%# Bind("ApprovedDate") %>'>  
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                              <asp:TemplateField HeaderText="Status" Visible="true">
                              <ItemTemplate>
                        <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("AppStatus") %>'>  
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
               
            </fieldset>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
