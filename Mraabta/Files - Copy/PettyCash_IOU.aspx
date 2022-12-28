<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="PettyCash_IOU.aspx.cs" Inherits="MRaabta.Files.PettyCash_IOU" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">

        function isNumberKey(evt) {

            var count = 1;
            var charCode = (evt.which) ? evt.which : event.keyCode
            if ((charCode > 47 && charCode < 58)) {
                count++;
            }
            else {
                if (count == 1) {
                    return false;
                }
            }

            return true;
        }
    </script>
    <style type="text/css">
        .mGrid {
            margin: 0 0 0 4px !important;
        }

        .abc {
            width: 200px;
        }

        .topheader {
            background-color: #F26726;
            font-weight: bold;
            border: 1px solid #F26726;
            color: #fff;
            text-transform: uppercase;
        }

        .footercolor {
            background-color: lightblue;
        }

        .report_name {
            float: left;
            left: 1% important;
            text-align: left !important;
        }

        /* Center the loader */

        .outer_box {
            background: #444 none repeat scroll 0 0;
            height: 101%;
            left: 0;
            opacity: 0.9;
            position: absolute;
            top: -1%;
            width: 100%;
        }

        .loader {
            border: 16px solid #f3f3f3;
            border-radius: 50%;
            border-top: 16px solid #3498db;
            width: 120px;
            height: 120px;
            -webkit-animation: spin 2s linear infinite;
            animation: spin 2s linear infinite;
            left: 43%;
            position: relative;
            top: 43%;
        }

        @-webkit-keyframes spin {
            0% {
                -webkit-transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
            }
        }

        @keyframes spin {
            0% {
                transform: rotate(0deg);
            }

            100% {
                transform: rotate(360deg);
            }
        }
    </style>
    <div>
        <table cellpadding="0" cellspacing="0" width='95%' class="mainTable">
            <tr>
                <td colspan="12" align="center" class="head_column">
                    <h3>Petty Cash IOU Report</h3>
                </td>
            </tr>
            <tr>
                <td class="topheader" width="12%" id="Td_ReportType" runat="server" colspan="12">
                    <center>
                        <div>
                            <asp:RadioButtonList runat="server" AutoPostBack="true" OnSelectedIndexChanged="RBL_ReportCategory_SelectedIndexChanged" ID="RBL_ReportCategory" CssClass="med-field" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Date" Value="Date" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="IOU Number" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Employee Code" Value="1"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </center>
                </td>
            </tr>
        </table>
    </div>
    <div>
        <table cellpadding="0" cellspacing="0" width='100%' class="input-form">
            <%--border="2" style="border-color: black;"--%>
            <tbody>
                <tr>
                    <td>
                        <asp:Label ID="lbl_Message" runat="server" Font-Size="Medium"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td width="12%" id="Td_IOU_EC" runat="server">
                        <div class="field" style="width: 13.5%" id="lblIOUNO" runat="server">
                            IOU Number
                        </div>
                        <div class="input-field" style="width: 8%;" id="txtIOUONO" runat="server">
                            <asp:TextBox ID="txtIOUNumber" runat="server" AutoCompleteType="None" onkeypress="return isNumberKey(event);" CssClass="med-field" MaxLength="11"
                                Width="150px" AutoPostBack="false"></asp:TextBox>
                        </div>
                        <div class="field" id="lblEC" runat="server">
                            Employee Code
                        </div>
                        <div class="input-field" style="width: 8%; padding-left: 6px;" id="txtEC" runat="server">
                            <asp:TextBox ID="txtEmployeeCode" runat="server" AutoCompleteType="None" onkeypress="return isNumberKey(event);" CssClass="med-field" MaxLength="8"
                                Width="150px" AutoPostBack="false"></asp:TextBox>
                        </div>
                    </td>
                    <td width="12%" id="Td_Date" runat="server">
                        <div class="field" id="lbl_StartDate" runat="server">
                            From Date:
                        </div>
                        <div class="input-field" id="txtStartDate_" runat="server">
                            <asp:TextBox ID="txtStartDate" runat="server" AutoCompleteType="None" CssClass="med-field" MaxLength="10" Width="190px" AutoPostBack="false"></asp:TextBox>
                            <Ajax1:CalendarExtender ID="CalenderStart" TargetControlID="txtStartDate" runat="server" Format="yyyy-MM-dd"></Ajax1:CalendarExtender>
                        </div>
                        <div class="field" style="width: 13%;"></div>
                        <div class="field" id="lbl_EndDate" runat="server">
                            To Date:
                        </div>
                        <div class="input-field" id="txtEndDate_" runat="server">
                            <asp:TextBox ID="txtEndDate" runat="server" AutoCompleteType="None" CssClass="med-field" MaxLength="10" Width="190px" AutoPostBack="false"></asp:TextBox>
                            <Ajax1:CalendarExtender ID="CalendarEnd" TargetControlID="txtEndDate" runat="server" Format="yyyy-MM-dd"></Ajax1:CalendarExtender>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td width="12%" id="Td_Zone" runat="server">
                        <div class="field" id="lbl_Zone" runat="server">
                            Zone:
                        </div>
                        <div class="input-field" id="txt_Zone" runat="server">
                            <asp:DropDownList ID="ListBoxZones" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ListBoxZones_OnSelectedIndexChanged" CssClass="dropdown" Width="200px">
                            </asp:DropDownList>
                        </div>

                        <div class="field" id="lbl_Branch" runat="server">
                            Branch:
                        </div>
                        <div class="input-field" id="txt_Branch" runat="server">
                            <asp:DropDownList ID="ListBoxBranches" runat="server" AutoPostBack="false" CssClass="dropdown" Width="200px">
                            </asp:DropDownList>
                        </div>
                    </td>
                </tr>
                <%--<tr>
                    <td width="50%" id="Td_FlagType" runat="server">
                        <div class="field" id="lbl_Flag" runat="server">
                            Flag
                        </div>
                        <div class="abc" style="padding-left: 28%;" id="Div_FlagType" runat="server">
                            <asp:RadioButtonList runat="server" TextAlign="Right" ID="RBL_FlagType" CssClass="rcbInput" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Settled" Value="S" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Outstanding" Value="OS"></asp:ListItem>
                                <asp:ListItem Text="ALL" Value="ALL"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </td>
                </tr>--%>
                <tr>
                    <td>
                        <div style="padding-left: 100%;">
                            <asp:Button ID="Button1" runat="server" Text="Generate Data" CssClass="button" OnClick="btnSearchData_Click"
                                OnClientClick="javascript:document.getElementById('ContentPlaceHolder1_loaders').style.display = 'block';javascript:document.getElementById('Table_1').style.display = 'none';javascript:document.getElementById('ContentPlaceHolder1_report_name').style.display = 'none';javascript:document.getElementById('ContentPlaceHolder1_btn_excel').style.display = 'none';" />
                        </div>
                    </td>
                    <td class="head_column_panel" colspan="3"></td>
                </tr>
                <tr>
                    <asp:HiddenField ID="hf_StartDate" runat="server" />
                    <asp:HiddenField ID="hf_EndDate" runat="server" />
                    <asp:HiddenField ID="hf_ZoneID" runat="server" />
                    <asp:HiddenField ID="hf_BranchID" runat="server" />
                    <%--<asp:HiddenField ID="hf_FlagID" runat="server" />--%>
                    <asp:HiddenField ID="hf_IOUNumber" runat="server" />
                    <asp:HiddenField ID="hf_EmployeeCode" runat="server" />
                    <asp:HiddenField ID="hf_ReportType" runat="server" />
                    <asp:HiddenField ID="hf_UserID" runat="server" />
                </tr>
            </tbody>
        </table>
    </div>
    <div id="loaders" runat="server" class="outer_box" style="display: none;">
        <div id="loader" runat="server" class="loader">
        </div>
    </div>
    <div style="float: left; width: 80%; clear: both;">
        <asp:Label ID="lbl_report_name" Font-Size="Large" runat="server" CssClass="report_name"></asp:Label>
    </div>
    <div style="float: left; width: 80%; clear: both;">
        <asp:Label ID="lbl_total_record" Font-Size="Large" runat="server" CssClass="report_name"></asp:Label>
    </div>
    <div style="float: left; width: 20%; clear: both;">
        <asp:ImageButton ID="btn_HTML" runat="server" ImageUrl="~/images/ExcelFile.png" Width="48"
            OnClick="btn_HTML_Click" Visible="false" ToolTip="Get Downloadable Data" />
    </div>
    <div>
        <asp:Label ID="lbl_report_version" Font-Size="Large" runat="server"></asp:Label>
    </div>
    <asp:Literal ID="lbl_msg" runat="server"></asp:Literal>
    <asp:Label ID="error_msg" runat="server" CssClass="error_msg"></asp:Label>
    <span id="Table_1" class="tbl-large" style="float: left; width: 100%">
        <asp:GridView ID="GridViewPettyCash_IOUReport_Date" runat="server" AutoGenerateColumns="False"
            CssClass="mGrid" OnPageIndexChanging="GridViewPettyCash_IOUReport_Date_OnPageIndexChanging"
            AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None" AllowPaging="True"
            BorderWidth="1px" PageSize="200">
            <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
            <Columns>
                <asp:TemplateField HeaderText="S. No.">
                    <ItemTemplate>
                        <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="2%" />
                </asp:TemplateField>
                <asp:BoundField DataField="DateIOU" HeaderText="DATE IOU">
                    <ItemStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="Zone" HeaderText="ZONE">
                    <ItemStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="Branch" HeaderText="BRANCH">
                    <ItemStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="IOUNumber" HeaderText="IOU NUMBER">
                    <ItemStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="EmployeeCode" HeaderText="EMPLOYEE CODE">
                    <ItemStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="EmployeeName" HeaderText="EMPLOYEE NAME">
                    <ItemStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="OUTSTANDING AMOUNT">
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="7%" />
                    <ItemTemplate>
                        <asp:Label ID="OutstandingAmount" runat="server" Text='<%# Bind("OutstandingAmount", "{0:N2}") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="SETTLED AMOUNT">
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="7%" />
                    <ItemTemplate>
                        <asp:Label ID="SettledAmount" runat="server" Text='<%# Bind("SettledAmount", "{0:N2}") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="REMAINING AMOUNT">
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="7%" />
                    <ItemTemplate>
                        <asp:Label ID="RemainingAmount" runat="server" Text='<%# Bind("RemainingAmount", "{0:N2}") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Reason" HeaderText="REASON">
                    <ItemStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="Remarks" HeaderText="REMARKS">
                    <ItemStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="CreateOn" HeaderText="CREATE ON">
                    <ItemStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="CreateBy" HeaderText="CREATE BY">
                    <ItemStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="ModifyOn" HeaderText="MODIFY ON">
                    <ItemStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="ModifyBy" HeaderText="MODIFY BY">
                    <ItemStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
        <asp:GridView ID="GridViewPettyCash_IOUReport" runat="server" AutoGenerateColumns="False"
            CssClass="mGrid" OnPageIndexChanging="GridViewPettyCash_IOUReport_OnPageIndexChanging"
            AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None" AllowPaging="True"
            BorderWidth="1px" PageSize="200">
            <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
            <Columns>
                <asp:TemplateField HeaderText="S. No.">
                    <ItemTemplate>
                        <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="2%" />
                </asp:TemplateField>
                <asp:BoundField DataField="DateIOU" HeaderText="DATE IOU">
                    <ItemStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="Zone" HeaderText="ZONE">
                    <ItemStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="Branch" HeaderText="BRANCH">
                    <ItemStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="IOUNumber" HeaderText="IOU NUMBER">
                    <ItemStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="EmployeeCode" HeaderText="EMPLOYEE CODE">
                    <ItemStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="EmployeeName" HeaderText="EMPLOYEE NAME">
                    <ItemStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="Flag" HeaderText="FLAG">
                    <ItemStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="AMOUNT">
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="7%" />
                    <ItemTemplate>
                        <asp:Label ID="Amount" runat="server" Text='<%# Bind("Amount", "{0:N2}") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Reason" HeaderText="REASON">
                    <ItemStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="Remarks" HeaderText="REMARKS">
                    <ItemStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="CreateOn" HeaderText="CREATE ON">
                    <ItemStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="CreateBy" HeaderText="CREATE BY">
                    <ItemStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="ModifyOn" HeaderText="MODIFY ON">
                    <ItemStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="ModifyBy" HeaderText="MODIFY BY">
                    <ItemStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
    </span>
</asp:Content>