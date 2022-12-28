<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="NCIReport.aspx.cs" EnableEventValidation="false" Inherits="MRaabta.Files.NCIReport" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/jquery-3.5.1.min.js") %>"></script>

    <style>
        .label {
            /*float: left;*/
            font-weight: bold;
            text-align: right;
            padding: 0 10px 0 0;
            line-height: 22px;
        }

        .field {
            /*float: left;*/
            width: 150px;
        }

        .filter {
            margin: 0 0 10px 5px;
            background: #eee;
            padding: 10px;
            width: 100%;
        }

        .mGrid th {
            background: #f27031 !important;
        }
    </style>

    <style>
        /* Center the loader */

        .outer_box {
            background: #444 none repeat scroll 0 0;
            height: 151%;
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
            top: 43%
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
    <script type="text/javascript">
        function isNumberKey(evt) {

            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {

                return false;
            }
            return true;
        }


        function downloadCSV(csv, filename) {
            var csvFile;
            var downloadLink;
            csvFile = new Blob([csv], { type: "text/csv" });
            downloadLink = document.createElement("a");
            downloadLink.download = filename;
            downloadLink.href = window.URL.createObjectURL(csvFile);
            downloadLink.style.display = "none";
            document.body.appendChild(downloadLink);
            downloadLink.click();
        }

        function exportTableToCSV(table, filename) {
            var csv = [];
            document.getElementById('Div_GV_CSV').style.display = 'block';
            var rows = document.querySelectorAll(`${table} tr`);
            debugger;
            for (var i = 0; i < rows.length; i++) {
                var row = [], cols = rows[i].querySelectorAll("td, th");

                for (var j = 0; j < cols.length; j++) {
                    if (j == 6) {
                        row.push(cols[j].innerText.replaceAll(',', '-'));
                    } else {
                        if (cols[j].outerHTML == "<td>&nbsp;</td>") {
                            row.push("");
                        } else {
                            row.push(cols[j].innerText);
                        }
                    }
                }
                csv.push(row.join(","));
            }
            document.getElementById('Div_GV_CSV').style.display = 'none';

            // Download CSV file
            downloadCSV(csv.join("\n"), filename);
        }

    </script>

    <div>
        <fieldset style="height: 398px;">
            <legend>NCI Report</legend>
            <div class="filter">
                <table>
                    <tr>
                        <td class="label">From Date
                        </td>
                        <td class="field">
                            <asp:TextBox ID="startDate" runat="server" CssClass="med-field" MaxLength="10" Width="100px"></asp:TextBox>
                            <Ajax1:CalendarExtender ID="Calendar1" runat="server" TargetControlID="startDate"
                                Format="dd/MM/yyyy"></Ajax1:CalendarExtender>
                        </td>
                        <td class="label">To Date
                        </td>
                        <td class="field">
                            <asp:TextBox ID="endDate" runat="server" CssClass="med-field" MaxLength="10" Width="100px"></asp:TextBox>
                            <Ajax1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="endDate"
                                Format="dd/MM/yyyy"></Ajax1:CalendarExtender>
                        </td>
                        <td class="label">Consignment Number
                        </td>
                        <td class="field">
                            <asp:TextBox ID="txt_cn" runat="server" onkeypress="return isNumberKey(event);"></asp:TextBox>
                        </td>
                        <td class="label">Account No
                        </td>
                        <td class="field">
                            <asp:TextBox ID="txt_Acc" runat="server" MaxLength="20"></asp:TextBox>
                        </td>
                        <td class="label">Pending Ref. Status
                        </td>
                        <td class="field" style="width: 200px">
                            <asp:DropDownList ID="dd_calltrack" runat="server" AppendDataBoundItems="false"
                                Style="width: 142px;">
                            </asp:DropDownList>
                        </td>

                        <td class="label">COD/NON COD
                        </td>
                        <td class="field" style="width: 200px">
                            <asp:DropDownList ID="cod_dropdown" runat="server" AppendDataBoundItems="false"
                                Style="width: 142px;">
                                <asp:ListItem Text="NON COD" Value="0"></asp:ListItem>
                                <asp:ListItem Text="COD" Value="1"></asp:ListItem>
                                <asp:ListItem Text="RETAIL COD" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                        </td>

                        <td class="label">Report Type
                        </td>
                        <td class="field">
                            <asp:DropDownList ID="report_type" runat="server" AppendDataBoundItems="false"
                                Style="width: 100px;">
                            </asp:DropDownList>
                            <%--    <asp:RadioButton ID="rb_origin" runat="server" Text="Initiate Request" AutoPostBack="true" GroupName="gp" Checked="true" />
                            <asp:RadioButton ID="rb_dst" runat="server" Text="Receive Request" AutoPostBack="true" GroupName="gp" />--%>
                        </td>
                        <td class="label">
                            <asp:Button ID="btn_save" runat="server" Text="Search" CssClass="button" OnClick="btn_save_Click"
                                OnClientClick="javascript:document.getElementById('ContentPlaceHolder1_loaders').style.display = 'block';" />
                        </td>
                        <td class="label">
                            <asp:ImageButton ID="excelicon" runat="server" ImageUrl="~/images/ExcelFile.png" Width="48"
                                OnClientClick="exportTableToCSV('#ContentPlaceHolder1_Gv_CSV','NCIReport.csv')" />
                        </td>
                    </tr>
                </table>
            </div>

            <div id="loaders" runat="server" class="outer_box" style="display: none;">
                <div id="loader" runat="server" class="loader"></div>
            </div>
            <asp:Label ID="Errorid" runat="server" Font-Bold="true" Font-Size="Medium" ForeColor="Red"></asp:Label>
            <div>
                <asp:GridView ID="GV_Histroy" runat="server" AutoGenerateColumns="FALSE" CssClass="mGrid"
                    OnRowDataBound="GridView1_RowDataBound" AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE"
                    BorderStyle="None" AllowPaging="TRUE" PageSize="200" BorderWidth="1px" OnPageIndexChanging="GridView2_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField HeaderText="S.No." ItemStyle-Width="2%">
                            <ItemTemplate>
                                <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="LOGSTATUS" HeaderText="LOG STATUS" />
                        <asp:TemplateField HeaderText="CN" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <asp:HyperLink ID="lbl_CN" Target="_blank" runat="server" Text='<%# Bind("CONSIGNMENTNUMBER", "{0:N0}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="SHIPPERNAME" HeaderText="SHIPPER NAME" />
                        <asp:BoundField DataField="ACCOUNTNO" HeaderText="ACCOUNT NO" />
                        <asp:BoundField DataField="REASON" HeaderText="REASON" />
                        <asp:BoundField DataField="CALLSTATUS" HeaderText="CALL STATUS" />
                        <%-- <asp:BoundField DataField="COMMENT" HeaderText="COMMENT" />--%>
                        <asp:BoundField DataField="ORIGIN" HeaderText="ORIGIN" />
                        <asp:BoundField DataField="DST" HeaderText="DESTINATION" />
                        <asp:BoundField DataField="CREATEDON" HeaderText="CREATED ON" />
                        <%--<asp:BoundField DataField="CREATEDBY" HeaderText="CREATED BY" />--%>
                    </Columns>
                </asp:GridView>

                <div style="display: none" id="Div_GV_CSV">
                    <asp:GridView ID="Gv_CSV" runat="server" AutoGenerateColumns="FALSE"
                        AllowPaging="false">
                        <Columns>
                            <asp:TemplateField HeaderText="S.No." ItemStyle-Width="2%">
                                <ItemTemplate>
                                    <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="LOGSTATUS" HeaderText="LOG STATUS" />
                            <asp:BoundField DataField="CONSIGNMENTNUMBER" HeaderText="CN" />
                            <asp:BoundField DataField="SHIPPERNAME" HeaderText="SHIPPER NAME" />
                            <asp:BoundField DataField="ACCOUNTNO" HeaderText="ACCOUNT NO" />
                            <asp:BoundField DataField="REASON" HeaderText="REASON" />
                            <asp:BoundField DataField="CALLSTATUS" HeaderText="CALL STATUS" />
                            <asp:BoundField DataField="ORIGIN" HeaderText="ORIGIN" />
                            <asp:BoundField DataField="DST" HeaderText="DESTINATION" />
                            <asp:BoundField DataField="CREATEDON" HeaderText="CREATED ON" />
                        </Columns>
                    </asp:GridView>
                </div>

            </div>
        </fieldset>
    </div>
</asp:Content>
