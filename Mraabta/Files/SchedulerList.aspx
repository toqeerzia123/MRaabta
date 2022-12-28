﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SchedulerList.aspx.cs" Inherits="MRaabta.Files.SchedulerList" MasterPageFile="~/BtsMasterPage.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../Bts_Css/Mazen.css" rel="stylesheet" />
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
                    <span id="ContentPlaceHolder1_Errorid" style="color: Red; font-weight: bold;"></span>
                    <legend style="font-size: medium;"><b>Scheduler List</b></legend>
                    <div class="row main-body newPanel">

                        <%--<div class="col-lg-12 col-md-12 col-sm-12 screen-name">
            <div style="float: left;">
                <h3>Pickup Request List </h3>
            </div>
        </div>--%>
                        <div class="col-lg-12 col-md-12 col-sm-12">

                            <table>
                                <tr>
                                    <td>
                                        <b>Pickup Date</b>
                                    </td>

                                </tr>
                                <tr>

                                    <td style="width: 225px;">
                                        <asp:TextBox ID="txtAccountNo" TextMode="Date" Style="width: 230px; margin-top: 10px; height: 33px" runat="server">
                                        </asp:TextBox>
                                        <%--<asp:TextBox ID="" CssClass="form-control" runat="server" />--%>
                                    </td>



                                    <td>
                                        <div class="btn_div" style="">
                                            <asp:Button ID="btnSubmit" CssClass="btn btn-success" runat="server" OnClick="btnSubmit_Click" Text="Search" Style="margin-top: 9px;" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="btn_div" style="">
                                            <asp:Button ID="btnAdd" CssClass="btn btn-success" runat="server" OnClick="btnAdd_Click" Text="Add New Request" Style="margin-top: 9px;" />
                                        </div>
                                    </td>
                                </tr>
                            </table>

                        </div>
                    </div>
                    <br />
                    <%--    <div class="btn_div">
                    <asp:Button ID="btnAdd" CssClass="btn btn-success" runat="server" OnClick="btnAdd_Click" Text="Add New Request" />
                </div>--%>
                    <div>
                        <asp:GridView ID="grPML" runat="server" AutoGenerateColumns="false" AllowPaging="True" DataKeyNames="SchedulerID"
                            OnPageIndexChanging="grPML_PageIndexChanging" Font-Size="Medium" CellPadding="4" ForeColor="#333333" GridLines="None" Font-Bold="True" Width="100%"
                            EnableTheming="False" BorderColor="#DA7A4D" BorderStyle="Solid"
                            AutoGenerateDeleteButton="True" OnRowDeleting="GridView1_RowDeleting"
                            BorderWidth="5px" EmptyDataText="No Request Generated">
                            <Columns>
                                <asp:TemplateField HeaderText="Action" Visible="true">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkView" runat="server" OnClick="linkRequesID_Click" CommandArgument='<%#Eval("SchedulerID") %>'>Edit</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Scheduler ID" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblschedulerid" runat="server" Text='<%# Bind("SchedulerID") %>'>  
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Pickup Date" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPickupDate" runat="server" Text='<%# Bind("PickupDate","{0:MMM dd, yyyy}") %>'>  
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Location Name" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lbllocationName" runat="server" Text='<%# Bind("CustomerLocation") %>'>  
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Customer Name" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustomerName" runat="server" Text='<%# Bind("CustomerName") %>'>  
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="branch Name" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblbranchName" runat="server" Text='<%# Bind("branchName") %>'>  
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Product Name" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProductID" runat="server" Text='<%# Bind("ProductID") %>'>  
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Slot" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSlot" runat="server" Text='<%# Bind("Slot") %>'>  
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Service Name" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServiceID" runat="server" Text='<%# Bind("ServiceID") %>'>  
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Account No" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblaccountNo" runat="server" Text='<%# Bind("accountNo") %>'>  
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Route Code" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRouteCode" runat="server" Text='<%# Bind("RouteCode") %>'>  
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Schedule Type" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblScheduleType" runat="server" Text='<%# Bind("ScheduleType") %>'>  
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                            <EditRowStyle BackColor="#999999" BorderColor="#DA7A4D" BorderStyle="Solid" BorderWidth="1px" />
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


                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>