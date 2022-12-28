<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="PickupRider.aspx.cs" Inherits="MRaabta.Files.PickupRider" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script src="../Js/FusionCharts.js" type="text/javascript"></script>
    <body>
        <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>

            <tr>
                <td colspan="12" align="center" class="head_column">
                    <h3>Pickup Rider</h3>
                </td>
            </tr>

            <tr style="height: 85px;">

                <td class="head_column_panel" width="10%" id="Td1" runat="server" style="padding-left: 20px">
                    <label for="exampleInputEmail1"><b>From Date</b></label>

                    <div class="row">
                        <div class="col-md-9 ">
                            <asp:TextBox ID="dd_start_date" runat="server" placeholder="From" Width="140px" AutoPostBack="false"></asp:TextBox>
                        </div>
                    </div>
                </td>

                <td style="width: 4%">
                    <div class="col-md-3 mt-1">
                        <asp:ImageButton ID="Popup_Button2" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                            Width="23px" Height="23px" />
                        <Ajax1:CalendarExtender ID="CalendarExtender2" TargetControlID="dd_start_date" runat="server"
                            Format="yyyy-MM-dd" PopupButtonID="Popup_Button2"></Ajax1:CalendarExtender>
                    </div>
                </td>

                <td class="head_column_panel" width="10%" id="Td2" runat="server" style="padding-left: 10px">
                    <label for="exampleInputEmail1"><b>End Date</b></label>
                    <div class="row">
                        <div class="col-md-9 ">
                            <asp:TextBox ID="dd_end_date" runat="server" placeholder="From" Width="140px" AutoPostBack="false"></asp:TextBox>
                        </div>
                    </div>
                </td>
                <td style="width: 4%">
                    <div class="col-md-3 mt-1">
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                            Width="23px" Height="23px" />
                        <Ajax1:CalendarExtender ID="CalendarExtender1" TargetControlID="dd_end_date" runat="server"
                            Format="yyyy-MM-dd" PopupButtonID="ImageButton1"></Ajax1:CalendarExtender>
                    </div>
                </td>
                <td class="head_column_panel" width="10%" style="padding-left: 00px">
                    <div><b>Zone</b> </div>
                    <div>
                        <asp:ListBox ID="dd_zone" runat="server" CssClass="dropdown" SelectionMode="Single" AppendDataBoundItems="true"
                            AutoPostBack="true" OnSelectedIndexChanged="branch_SelectedIndexChanged"></asp:ListBox>
                    </div>
                </td>

                <td class="head_column_panel" width="11%" style="padding-left: 20px">
                    <div><b>Select Branch</b> </div>
                    <div>
                        <asp:ListBox ID="dd_branch" runat="server" SelectionMode="Single" CssClass="dropdown"></asp:ListBox>
                    </div>
                </td>
                <%--    <td style="padding-left: 10px;width">
                    <asp:CheckBox ID="branch_chk" runat="server" Text="ALL" AutoPostBack="true" OnCheckedChanged="branch_chk_CheckedChanged" />
                </td>--%>

                <td class="head_column_panel" id="Div_Type" runat="server" width="15%" style="padding-left: 30px">
                    <div><b>Type</b></div>
                    <div>
                        <asp:RadioButton ID="rb_Pending" AutoPostBack="false" GroupName="Group_Type" runat="server" Checked="true" Text="Pending" />
                    </div>
                    <div>
                        <asp:RadioButton ID="rb_Booked" AutoPostBack="false" GroupName="Group_Type" runat="server" Text="Booked" />
                    </div>
                    <div>
                        <asp:RadioButton ID="rb_NoResponse" AutoPostBack="false" GroupName="Group_Type" runat="server" Text="No response" />
                    </div>
                    <div>
                        <asp:RadioButton ID="rb_refuse" AutoPostBack="false" GroupName="Group_Type" runat="server" Text="Refused" />
                    </div>
                </td>
                <%--
                <td class="head_column_panel" width="9%">
                    <div>Output Type</div>
                    <div>
                        <asp:DropDownList ID="type" runat="server" CssClass="dropdown">
                            <asp:ListItem Value="html" Selected="True">HTML</asp:ListItem>
                            <asp:ListItem Value="CSV">CSV</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </td>--%>
                <td class="head_column_panel">
                    <asp:Button ID="Button1" runat="server" Text="Show Data" CssClass="button" OnClick="Btn_Search_Click" />
                </td>
                <td class="head_column_panel" colspan="3"></td>
            </tr>
        </table>
        <br />

        <fieldset style="border: solid; border-width: thin; height: auto; border-color: #a8a8a8;"
            class="">

            <legend id="Legend5" visible="true" style="width: auto; font-size: 16px; font-weight: bold; color: #1f497d;">Pickup Requests</legend>

            <span id="" class="">


                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <asp:Label ID="lbl_total_record" runat="server" CssClass="lbl_rep" Font-Bold="true" Font-Size="12px"></asp:Label>
                </div>
                <asp:Label ID="error_msg" runat="server" CssClass="error_msg"></asp:Label>

                <asp:GridView ID="gg_CustomerLedger_Month" runat="server" AutoGenerateColumns="false" ItemStyle-HorizontalAlign="left" OnPageIndexChanging="GridView2_PageIndexChanging"
                    CssClass="mGrid" AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None" OnRowDataBound="GridView1_RowDataBound" Width="100%"
                    AllowPaging="true" PageSize="200" BorderWidth="1px">
                    <Columns>
                        <asp:TemplateField HeaderText="S.No." ItemStyle-Width="2%">
                            <ItemTemplate>
                                <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />

                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:BoundField DataField="ticketNumber" HeaderText="Ticket No." ItemStyle-Width="10" />

                        <asp:BoundField DataField="consigner" HeaderText="Consigner" ItemStyle-Width="10" />
                        <asp:BoundField DataField="consigneraddress" HeaderText="Consigner Address" ItemStyle-Width="8%" ControlStyle-Width="8%" HeaderStyle-Width="8" />
                        <asp:BoundField DataField="consignerCellNo" HeaderText="Consigner Phone" ItemStyle-Width="10" />
                        <asp:BoundField DataField="scheduledService" HeaderText="Scheduled Service" ItemStyle-Width="10" />
                        <asp:BoundField DataField="pickupScheduled" HeaderText="Pickup Scheduled" ItemStyle-Width="10" />
                        <asp:BoundField DataField="pickupTime" HeaderText="Pickup Time" ItemStyle-Width="10" />
                        <asp:BoundField DataField="Reason" HeaderText="Reason" ItemStyle-Width="10" />
                        <asp:BoundField DataField="pieces" HeaderText="Pieces" ItemStyle-Width="10" />
                        <asp:BoundField DataField="weight" HeaderText="Weight" ItemStyle-Width="10" />

                        <asp:TemplateField HeaderText="Call Status" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Label ID="Lbl_call_id" runat="server" Text='<%# Eval("callStatusCode") %>' Visible="false" />
                                <asp:Label ID="lbl_Id_CallStatus" runat="server" Text='<%# Eval("ticketNumber") %>' Visible="false" />
                                <asp:DropDownList ID="ddl_callStatus" Width="200px" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_callStatus_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Express Center" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Label ID="Lbl_exp" runat="server" Text='<%# Eval("expressCenterCode") %>' Visible="false" />
                                <asp:Label ID="lbl_Id_ticket" runat="server" Text='<%# Eval("ticketNumber") %>' Visible="false" />
                                <asp:DropDownList ID="ddl_expressCenter" Width="200px" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_expressCenter_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Rider" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Label ID="Lbl_riders" runat="server" Text='<%# Eval("RiderCode") %>' Visible="false" />
                                <asp:Label ID="lbl_Id" runat="server" Text='<%# Eval("ticketNumber") %>' Visible="false" />
                                <asp:DropDownList ID="ddl_rider" Width="200px" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_rider_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Remarks" ItemStyle-Width="8%">
                            <ItemTemplate>
                                <asp:HyperLink ID="lblshipment" Target="_blank" runat="server" Text='Details' ForeColor="Blue" />
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>

                <asp:GridView ID="GridviewBooked" runat="server" AutoGenerateColumns="false" ItemStyle-HorizontalAlign="left" OnPageIndexChanging="GridView2_PageIndexChanging"
                    CssClass="mGrid" AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None" Width="100%"
                    AllowPaging="true" PageSize="200" BorderWidth="1px">
                    <Columns>
                        <asp:TemplateField HeaderText="S.No." ItemStyle-Width="2%">
                            <ItemTemplate>
                                <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />

                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ticketNumber" HeaderText="Ticket No." ItemStyle-Width="10" />

                        <asp:BoundField DataField="consigner" HeaderText="Consigner" ItemStyle-Width="10" />
                        <asp:BoundField DataField="consigneraddress" HeaderText="Consigner Address" ItemStyle-Width="10" />
                        <asp:BoundField DataField="consignerCellNo" HeaderText="Consigner Phone" ItemStyle-Width="10" />
                        <asp:BoundField DataField="scheduledService" HeaderText="Scheduled Service" ItemStyle-Width="10" />
                        <asp:BoundField DataField="pickupScheduled" HeaderText="Pickup Scheduled" ItemStyle-Width="10" />
                        <asp:BoundField DataField="pickupTime" HeaderText="Pickup Time" ItemStyle-Width="10" />
                        <asp:BoundField DataField="pieces" HeaderText="Pieces" ItemStyle-Width="10" />
                        <asp:BoundField DataField="weight" HeaderText="Weight" ItemStyle-Width="10" />
                        <asp:BoundField DataField="expressCenter" HeaderText="expressCenter" ItemStyle-Width="10" />
                        <asp:BoundField DataField="ridername" HeaderText="Rider" ItemStyle-Width="10" />
                    </Columns>
                </asp:GridView>
            </span>
        </fieldset>
    </body>
</asp:Content>
