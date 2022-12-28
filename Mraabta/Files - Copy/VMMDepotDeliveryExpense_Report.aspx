<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="VMMDepotDeliveryExpense_Report.aspx.cs" Inherits="MRaabta.Files.VMMDepotDeliveryExpense_Report" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        .datebox
        {
            background: url(images/calendar_1.png) no-repeat;
            background-position: 2px 3px !important;
            padding: 0px 0px 0px 25px;
            width: 150px;
            height: 20px;
            border: 1px solid #CCC;
            -moz-border-radius: 5px;
            -webkit-border-radius: 5px;
            border-radius: 5px;
            -moz-box-shadow: 0 1px 1px #ccc inset, 0 1px 0 #fff;
            -webkit-box-shadow: 0 1px 1px #CCC inset, 0 1px 0 #FFF;
        }
        
        .datebox:focus
        {
            background-color: #FFF;
            border-color: #CCC;
            outline: none;
            -moz-box-shadow: 0 0 0 1px #e8c291 inset;
            -webkit-box-shadow: 0 0 0 1px #E8C291 inset;
            box-shadow: 0 0 0 1px #CCC inset;
        }
        
        .txtbox
        {
            padding: 0px 0px 0px 2px;
            width: 170px;
            height: 20px;
            border: 1px solid #CCC;
            -moz-border-radius: 5px;
            -webkit-border-radius: 5px;
            border-radius: 5px;
            -moz-box-shadow: 0 1px 1px #ccc inset, 0 1px 0 #fff;
            -webkit-box-shadow: 0 1px 1px #CCC inset, 0 1px 0 #FFF;
        }
        
        .txtbox:focus
        {
            background-color: #FFF;
            border-color: #ccc;
            outline: none;
            -moz-box-shadow: 0 0 0 1px #e8c291 inset;
            -webkit-box-shadow: 0 0 0 1px #E8C291 inset;
            box-shadow: 0 0 0 1px #ccc inset;
        }
        
        .ddlbox
        {
            border: 1px solid #CCC;
            border-radius: 5px;
            width: 180px;
            height: 20px;
        }
        
        .buttonload
        {
            background-color: #ccc; /* Green background */
            color: white; /* White text */
            padding: 12px 24px; /* Some padding */
            font-size: 16px; /* Set a font-size */
        }
        
        .fa
        {
            margin-left: -12px;
            margin-right: 8px;
        }
        
        .time
        {
            width: 120px;
            height: 20px;
        }
        
        .gridview
        {
            background-color: #00427c;
            color: White; /* font-size: x-small;*/
            font-family: Calibri;
        }
        
        .grid
        {
            /*  font-size: x-small;*/
            font-family: Calibri;
        }
        
        .abc table
        {
            /*font-size: x-small;*/
            font-family: Calibri;
        }
    </style>
    <div class="heading" id="heading" runat="server">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <b>
                    <asp:Label ID="Errorid" runat="server" ForeColor="Red"></asp:Label></b>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <link href="../Styles/StyleSheet.css" rel="stylesheet" type="text/css" />
    <fieldset>
        <legend>DEPOT DELIVERY EXPENSE</legend>
        <%--       <asp:UpdatePanel ID="mypanel" runat="server">
            <ContentTemplate>--%>
        <table width="600px">
            <tr>
                <td class="cell-two-mini" style="font-size: x-small; width: 85px;" colspan="1">
                    <b>From Date</b>
                </td>
                <td class="cell-two-mini" style="width: 50px;" colspan="1">
                    <asp:TextBox ID="txt_fromdate" runat="server" CssClass="datebox" MaxLength="10"></asp:TextBox>
                    <Ajax1:CalendarExtender ID="CalendarExtender3" TargetControlID="txt_fromdate" runat="server"
                        Format="dd/MM/yyyy" PopupButtonID="Image1">
                    </Ajax1:CalendarExtender>
                    <br />
                    <asp:RequiredFieldValidator runat="server" Width="110px" ID="RequiredFieldValidator3"
                        ControlToValidate="txt_fromdate" ForeColor="Red" ErrorMessage="Please Enter Date!" />
                </td>
                <td class="cell-two-mini" style="font-size: x-small; width: 85px;" colspan="1">
                    <b>To Date</b>
                </td>
                <td class="cell-two-mini" style="width: 50px;" colspan="1">
                    <asp:TextBox ID="txt_todate" runat="server" CssClass="datebox" MaxLength="10"></asp:TextBox>
                    <Ajax1:CalendarExtender ID="CalendarExtender4" TargetControlID="txt_todate" runat="server"
                        Format="dd/MM/yyyy" PopupButtonID="Image1">
                    </Ajax1:CalendarExtender>
                    <br />
                    <asp:RequiredFieldValidator runat="server" Width="110px" ID="RequiredFieldValidator4"
                        ControlToValidate="txt_todate" ForeColor="Red" ErrorMessage="Please Enter Date!" />
                </td>
            </tr>
            <tr>
                <td class="cell-two-mini" style="font-size: x-small; width: 35px;" colspan="1">
                    <b>Zone</b>
                </td>
                <td class="cell-two-mini" width="50px" colspan="1">
                    <asp:DropDownList runat="server" Visible="true" CssClass="ddlbox" ID="ddl_zone" AppendDataBoundItems="true"
                        OnSelectedIndexChanged="ddl_zone_SelectedIndexChanged" AutoPostBack="true">
                    </asp:DropDownList>
                </td>
            
                <td class="cell-two-mini" style="font-size: x-small; width: 35px;" colspan="1">
                    <b>Vehicle</b>
                </td>
                <td class="cell-two-mini" width="50px" colspan="1">
                    <asp:DropDownList runat="server" Visible="true" CssClass="ddlbox" ID="dd_van" AppendDataBoundItems="true">
                        <asp:ListItem Value="0">Select Vehicle</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <table width="600px">
            <%--            <tr>
                 <td style="font-size: x-small;" width="30px"colspan="1">
                            <b>Division</b>
                        </td>
                        <td class="cell-two-mini" width="80px" colspan="1">
                            <asp:DropDownList ID="dd_ItemLevel1" runat="server" CssClass="ddlbox" AppendDataBoundItems="true"
                                AutoPostBack="true">
                                <asp:ListItem Value="0"> Select Division </asp:ListItem>
                                <asp:ListItem Value="1"> PHARMA </asp:ListItem>
                                <asp:ListItem Value="2"> CONSUMER </asp:ListItem>
                                <asp:ListItem Value="3"> TELECOM </asp:ListItem>
                                <asp:ListItem Value="4"> HCU </asp:ListItem>
                                <asp:ListItem Value="5"> STATIONARY </asp:ListItem>
                            </asp:DropDownList>
                        </td>
            </tr>--%>
            <tr colspan="2">
                <td style="height: 10px;">
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btn_Submit" CssClass="button" runat="server" OnClick="btn_Submit_Click"
                        Text="Show Record" />
                    <%--     <asp:Button ID="ButtonExportExcel" runat="server" CssClass="button" Text="Export To Excel"
                        Width="125" OnClick="ButtonExportExcel_Click" />--%>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <%--        <div class="abc">--%>
        <asp:GridView ID="gvwData" AutoGenerateColumns="false" GridLines="Both" runat="server"
            AlternatingRowStyle-CssClass="alt" Width="100%" OnRowCreated="OnRowCreated" OnRowDataBound="OnRowDataBound"
            OnDataBound="OnDataBound" HeaderStyle-CssClass="gridview">
            <Columns>
                <asp:BoundField DataField="EID" ItemStyle-HorizontalAlign="Center" HeaderText="EXPENSE ID"
                    ItemStyle-CssClass="grid" ItemStyle-Width="50px" />
                <asp:BoundField DataField="EXPENSE_DATE" HeaderText="EXPENSE DATE" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-CssClass="grid" ItemStyle-Width="100px" />
                <%-- <asp:BoundField DataField="DSR" HeaderText="DRIVER" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="grid"
                    ItemStyle-Width="180px" />--%>
                <asp:BoundField DataField="VEHICLE" HeaderText="VEHICLE" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-CssClass="grid" ItemStyle-Width="80px" />
                <asp:BoundField DataField="MMC" HeaderText="MAIN CATEGORY" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-CssClass="grid" ItemStyle-Width="200px" />
                <asp:BoundField DataField="MSC" HeaderText="SUB CATEGORY" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-CssClass="grid" ItemStyle-Width="200px" />
                <asp:BoundField DataField="NO." ItemStyle-HorizontalAlign="Center" HeaderText="BILL NO"
                    ItemStyle-CssClass="grid" ItemStyle-Width="70px" />
                <asp:BoundField DataField="DATE" ItemStyle-HorizontalAlign="Center" HeaderText="BILL DATE"
                    ItemStyle-CssClass="grid" ItemStyle-Width="70px" />
                <asp:BoundField DataField="AMOUNT" ItemStyle-HorizontalAlign="Center" HeaderText="AMOUNT"
                    ItemStyle-CssClass="grid" ItemStyle-Width="70px" />
                <asp:BoundField DataField="workorder" ItemStyle-HorizontalAlign="Center" HeaderText="WORK ORDER NUMBER"
                    ItemStyle-CssClass="grid" ItemStyle-Width="70px" />
                <asp:BoundField DataField="MTR_READING" ItemStyle-HorizontalAlign="Center" HeaderText="VEHCILE METER READING"
                    ItemStyle-CssClass="grid" ItemStyle-Width="80px" />
            </Columns>
            <EditRowStyle BackColor="#2461BF" />
            <FooterStyle BackColor="#507CD1" ForeColor="White" />
            <HeaderStyle BackColor="#ee701b" ForeColor="White" BorderColor="White" />
            <PagerStyle BackColor="#f5f5f5" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#f5f5f5" />
        </asp:GridView>
        <asp:HiddenField runat="server" ID="hf_gv_count" />
        <%--             <asp:Repeater ID="rp_log" runat="server" OnItemDataBound="rptItem_ItemDataBound" OnItemCreated="rpitem_itemcreated" >
              <HeaderTemplate>
                        <table style="width: 100%;">
                            <tr>
                                <td class="heading-cell" style="width: 100px">
                                    DATE
                                </td>
                                <td class="heading-cell" style="width: 100px">
                                    DEPOT
                                </td>
                                <td class="heading-cell" style="width: 100px">
                                    DIVISION
                                </td>
                                <td class="heading-cell" style="width: 100px">
                                    LOC/UPC
                                </td>
                                <td class="heading-cell" style="width: 100px">
                                    DSR
                                </td>
                                <td class="heading-cell" style="width: 100px">
                                    DRIVER
                                </td>
                                <td class="heading-cell" style="width: 100px">
                                    DP DATE
                                </td>
                                <td class="heading-cell" style="width: 100px">
                                    DP TIME
                                </td>
                                <td class="heading-cell" style="width: 100px">
                                    AR DATE
                                </td>
                                <td class="heading-cell" style="width: 100px">
                                    AR TIME
                                </td>
                                <td class="heading-cell" style="width: 100px">
                                    DP METER
                                </td>
                                <td class="heading-cell" style="width: 100px">
                                    AR METER
                                </td>
                                <td class="heading-cell" style="width: 100px">
                                    TOTAL KM
                                </td>
                                <td class="heading-cell" style="width: 100px">
                                    VALUE
                                </td>
                                <td class="heading-cell" style="width: 100px">
                                    CUST. COUNT
                                </td>
                                <td class="heading-cell" style="width: 100px">
                                    CM COUNT
                                </td>
                                <td class="heading-cell" style="width: 100px">
                                    SR RETURN
                                </td>
                                <td class="heading-cell" style="width: 100px">
                                    CUST. COUNT
                                </td>
                                <td class="heading-cell" style="width: 100px">
                                    CM COUNT
                                </td>
                            </tr>
                    </HeaderTemplate>
                 <ItemTemplate>
                        <tr>
                            <td class="grid-cell">
                                <asp:Label ID="lbl_docdate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DOC_DATE")%>'></asp:Label>
                            </td>
                            <td class="grid-cell">
                                <asp:Label ID="lbl_name" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "NAME")%>'></asp:Label>
                            </td>
                            <td class="grid-cell">
                                <asp:Label ID="lbl_division" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DIVISION")%>'></asp:Label>
                            </td>
                            <td class="grid-cell">
                                <asp:Label ID="lbl_loc_upc" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LOC_UPC")%>'></asp:Label>
                            </td>
                            <td class="grid-cell">
                                <asp:Label ID="lbl_dsr_name" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DSR_NAME")%>'></asp:Label>
                            </td>
                            <td class="grid-cell">
                                <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DRIVER_NAME")%>'></asp:Label>
                            </td>
                            <td class="grid-cell">
                                <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DDATE")%>'></asp:Label>
                            </td>
                            <td class="grid-cell">
                                <asp:Label ID="Label3" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DTIME")%>'></asp:Label>
                            </td>
                            <td class="grid-cell">
                                <asp:Label ID="Label4" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ADATE")%>'></asp:Label>
                            </td>
                            <td class="grid-cell">
                                <asp:Label ID="Label5" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ATIME")%>'></asp:Label>
                            </td>
                            <td class="grid-cell">
                                <asp:Label ID="Label6" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DREADING")%>'></asp:Label>
                            </td>
                            <td class="grid-cell">
                                <asp:Label ID="Label7" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "AREADING")%>'></asp:Label>
                            </td>
                            <td class="grid-cell">
                                <asp:Label ID="Label8" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "TOTAL_KM")%>'></asp:Label>
                            </td>
                            <td class="grid-cell">
                                <asp:Label ID="Label9" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CM_INV_AMT")%>'></asp:Label>
                            </td>
                            <td class="grid-cell">
                                <asp:Label ID="Label10" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CM_CUST_CNT")%>'></asp:Label>
                            </td>
                            <td class="grid-cell">
                                <asp:Label ID="Label11" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CM_INV_CNT")%>'></asp:Label>
                            </td>
                            <td class="grid-cell">
                                <asp:Label ID="Label12" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SR_INV_AMT")%>'></asp:Label>
                            </td>
                            <td class="grid-cell">
                                <asp:Label ID="Label13" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SR_CUST_CNT")%>'></asp:Label>
                            </td>
                            <td class="grid-cell">
                                <asp:Label ID="Label14" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SR_INV_CNT")%>'></asp:Label>
                            </td>
                        </tr>
                        <asp:Literal ID="lt_Subtotal" runat="server" Visible="false"></asp:Literal>
                    </ItemTemplate>
               <FooterTemplate>
                        <asp:Literal ID="lt_Grandtotal" runat="server"></asp:Literal>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>--%>
        <%--            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ButtonExportExcel" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>--%>
    </fieldset>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
