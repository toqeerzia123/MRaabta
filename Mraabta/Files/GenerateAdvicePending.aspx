<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GenerateAdvicePending.aspx.cs" Inherits="MRaabta.Files.GenerateAdvicePending" MasterPageFile="~/BtsMasterPage.master" %>

<%@ Register Namespace="AjaxControlToolkit" TagPrefix="Ajax" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .mGrid th {
            background: #f27031 !important;
        }

        .label {
            float: left;
            font-weight: bold;
            width: 120px;
        }

        .field {
            float: left;
            width: 236px;
        }

        .blinking {
            animation: blinkingText 0.8s infinite;
            font-weight: bold;
        }

        @keyframes blinkingText {
            0% {
                color: white;
            }

            49% {
                color: #000;
            }

            50% {
                color: rwhiteed;
            }

            99% {
                color: white;
            }

            100% {
                color: #000;
            }
        }

        .main-tbl {
            background: #f26726 none repeat scroll 0 0;
            color: White;
        }

        legend {
            color: #f26726;
            font-weight: bold;
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

        .head_column_panel {
            float: left;
            width: 7%;
        }
    </style>
    <script type="text/javascript" src="../Scripts/jquery-3.5.1.min.js"></script>

    <script type="text/javascript">
        function isNumberKey(evt) {

            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {

                return false;
            }
            return true;
        }
        function BlockButton() {
            debugger;
            var Consignment = document.getElementById("<%= txt_cn.ClientID %>").value;
            if (Consignment.match(/[a-z]/i)) {
                alert('Please provide correct consignment number');
                return false;
            }
            document.getElementById("<%= btn_save.ClientID %>").style.visibility = "hidden";
            document.getElementById('ContentPlaceHolder1_loaders').style.display = 'block';
            return true;
        } 


         
    </script>
    <table class="mGrid_Table" width="100%" cellspacing="0" cellpadding="0">
        <tbody>
            <tr>
                <td colspan="12" class="head_column" align="center">
                    <h3>Generate Advice Pending
                    </h3>
                </td>
            </tr>
        </tbody>
    </table>
    <asp:Label ID="Errorid" runat="server" Font-Bold="true" Font-Size="Medium" ForeColor="Red"></asp:Label>
    <div class="main-tbl">
        <table>
            <tr>
                <td class="label" style="line-height: 20px;">Consignment No:
                </td>
                <td class="field">
                    <asp:TextBox ID="txt_cn" runat="server" AutoPostBack="true" OnTextChanged="txt_consignment_TextChanged"
                              onChange="javascript:document.getElementById('ContentPlaceHolder1_loaders2').style.display = 'block';"
                           
                        onkeypress="return isNumberKey(event);"></asp:TextBox>
                    

                </td>
                <td class="field">
                    <asp:Label ID="lbl_cod" runat="server" CssClass="blinking"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
     <div id="loaders2" runat="server" class="outer_box" style="display: none;">
            <div id="loader2" runat="server" class="loader">
            </div>
        </div>
    <div id="div_box" runat="server" visible="false">
        <fieldset>
            <legend>Consignment Track</legend>
            <table>
                <tr>
                    <td class="label">Consignment No:
                    </td>
                    <td class="field">
                        <asp:Label ID="lbl_cn" runat="server"></asp:Label>
                    </td>
                    <td class="label">Origin:
                    </td>
                    <td class="field">
                        <asp:Label ID="lbl_origin" runat="server"></asp:Label>
                        <asp:HiddenField ID="hd_origin" runat="server" />
                    </td>
                    <td class="label">Destination:
                    </td>
                    <td class="field">
                        <asp:Label ID="lbl_destination" runat="server"></asp:Label>
                        <asp:HiddenField ID="hd_destination" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="label">Account No:
                    </td>
                    <td class="field">
                        <asp:Label ID="lbl_account" runat="server"></asp:Label>
                    </td>
                    <td class="label">Shipper Name:
                    </td>
                    <td class="field">
                        <asp:Label ID="lbl_shippername" runat="server"></asp:Label>
                        <asp:HiddenField ID="hd_shippername" runat="server" />
                    </td>
                    <td class="label">Booking Date:
                    </td>
                    <td class="field">
                        <asp:Label ID="lbl_bookingdate" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="label">Consignee:
                    </td>
                    <td class="field">
                        <asp:Label ID="lbl_consignee" runat="server"></asp:Label>
                    </td>
                    <td class="label">Consignee Cell#:
                    </td>
                    <td class="field">
                        <asp:Label ID="lbl_consigneecell" runat="server"></asp:Label>
                    </td>
                    <td class="label">Service Type:
                    </td>
                    <td class="field">
                        <asp:Label ID="lbl_service" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="label">Consignee Address:
                    </td>
                    <td class="field" colspan="4" style="width: 596px;">
                        <asp:Label ID="lbl_consigneeaddress" runat="server"></asp:Label>
                    </td>
                    <td class="label">Current Status
                    </td>
                    <td class="field">
                        <asp:Label ID="lbl_currentstatus" runat="server"></asp:Label>
                        <asp:HiddenField ID="hd_currentstatus" runat="server"></asp:HiddenField>
                    </td>
                </tr>
            </table>
        </fieldset>
        <fieldset>
            <legend>Update Pending Info</legend>
            <table>
                <tr style="display: none;">
                    <td class="label">Consignment No:
                    </td>
                    <td class="field">
                        <asp:Label ID="lbl_cn1" runat="server"></asp:Label>
                    </td>
                    <td class="label">Origin:
                    </td>
                    <td class="field">
                        <asp:DropDownList ID="dd_origin" runat="server" Height="27px" Width="129px">
                        </asp:DropDownList>
                    </td>
                    <td class="label">Destination:
                    </td>
                    <td class="field">
                        <asp:DropDownList ID="ddl_Destination" runat="server" Height="27px" Width="129px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr style="display: none;">
                    <td class="label">Shipper Name:
                    </td>
                    <td class="field" colspan="3" style="width: 597px">
                        <asp:TextBox ID="txt_shipper" runat="server" Style="width: 497px" Enabled="false"></asp:TextBox>
                    </td>
                    <%--<td class="label">
                        Account No:
                    </td>
                    <td class="field">
                        <asp:TextBox ID="lbl_account" runat="server" Enabled="false"></asp:TextBox>
                        <asp:Label ID="lbl_cod" runat="server" CssClass="blinking"></asp:Label>
                    </td>--%>
                    <%--<td class="label">
                        Shipper Cell#:
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txt_shippercell" runat="server" onkeypress="return isNumberKey(event);"></asp:TextBox>
                    </td>--%>
                </tr>
                <tr>
                    <td class="label">Reason of Pending:
                    </td>
                    <td class="field">
                        <asp:DropDownList ID="dd_reason" runat="server" AppendDataBoundItems="false" Style="width: 142px;" 
                            AutoPostBack="true" OnSelectedIndexChanged="dd_reason_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="label">
                        <b>Standard Notes</b>
                    </td>
                    <td class="field">
                        <asp:DropDownList ID="dd_standardnote" runat="server" AppendDataBoundItems="false"
                            Style="width: 160px;">
                        </asp:DropDownList>
                    </td>
                    <td class="label">Phone Status:
                    </td>
                    <td class="field">
                        <asp:DropDownList ID="dd_callstatus" runat="server" AppendDataBoundItems="false"
                            Style="width: 142px;">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="label">Calling Time:
                    </td>
                    <td class="field">
                        <asp:DropDownList ID="dd_hour" runat="server" AppendDataBoundItems="false">
                        </asp:DropDownList>
                        <asp:DropDownList ID="dd_min" runat="server" AppendDataBoundItems="false">
                        </asp:DropDownList>
                    </td>
                    <td class="label">Consignee:
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txt_consignee" runat="server" />
                    </td>
                    <td class="label">Consignee Cell#:
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txt_consignee_cell" runat="server" onkeypress="return isNumberKey(event);" />
                    </td>
                </tr>
                <tr>
                    <td class="label">Pending Ref. Status:
                    </td>
                    <td class="field">
                        <asp:DropDownList ID="dd_calltrack" runat="server" AppendDataBoundItems="false" AutoPostBack="true"
                            OnSelectedIndexChanged="dd_calltrack_SelectedIndexChanged" Style="width: 142px;">
                        </asp:DropDownList>
                    </td>
                    <td class="label">Re-Attempt Remarks:
                    </td>
                    <td class="field">
                        <asp:DropDownList ID="dd_reattemptremarks" runat="server" AppendDataBoundItems="false"
                            Style="width: 142px;">
                        </asp:DropDownList>
                    </td>
                    <td class="label">Pending Ref. No:
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txt_ticketno" runat="server" Enabled="false" Style="border: 0; background: #fff;" />
                        <asp:HiddenField ID="txt_ticketalready" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="label">Consignee Address:
                    </td>
                    <td class="field" colspan="6" style="width: 864px;">
                        <asp:TextBox ID="txt_ConsigneeAddress" runat="server" Rows="1" Columns="105" TextMode="MultiLine" />
                    </td>
                </tr>
                <tr>
                    <td class="label">Additional Reamrks:
                    </td>
                    <td class="field" colspan="6" style="width: 864px;">
                        <asp:TextBox ID="txt_comment" runat="server" Rows="1" Columns="105" TextMode="MultiLine" />
                    </td>
                </tr>
                <tr id="cod_row" runat="server" visible="false">
                    <td class="label">
                        <b>Allocated To COD Customer</b>
                    </td>
                    <td class="field">
                        <asp:CheckBox ID="chk_cod" runat="server" Style="float: left; width: 10% !important;" />
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Button ID="btn_save" runat="server" Text="Save" CssClass="button" OnClick="btn_save_Click"
                            OnClientClick="return BlockButton();" />
                       
                    </td>
                    <td class="label">
                        <asp:Button ID="tvt_reset" runat="server" Text="Reset" CssClass="button" OnClick="ResetAll" />
                    </td>
                    <td class="label">
                        <asp:Button ID="btn_new_request" runat="server" Text="Re-Open Request" CssClass="button"
                            Visible="false" OnClick="btn_New_Request_Click" />
                    </td>
                </tr>
            </table>
        </fieldset>
        <div id="loaders" runat="server" class="outer_box" style="display: none;">
            <div id="loader" runat="server" class="loader">
            </div>
        </div>
        <fieldset>
            <legend>Pending Ticket Log</legend>
            <div id="div_grid" visible="false" runat="server" style="overflow: scroll; width: 1089px; padding: 0 0 10px;">
                <asp:GridView ID="GV_Histroy" runat="server" AutoGenerateColumns="FALSE" CssClass="mGrid"
                    AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None" AllowPaging="false"
                    BorderWidth="1px">
                    <Columns>
                        <asp:TemplateField HeaderText="S.No." ItemStyle-Width="2%">
                            <ItemTemplate>
                                <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="logstatus" HeaderText="log status" HeaderStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="TicketNo" HeaderText="Pending Ref.No" />
                        <asp:BoundField DataField="CONSIGNMENTNUMBER" HeaderText="CN" />
                        <asp:BoundField DataField="REASON" HeaderText="REASON" />
                        <asp:BoundField DataField="CALLSTATUS" HeaderText="PHONE CALL STATUS" />
                        <asp:BoundField DataField="PHONECALLTIME" HeaderText="PHONE CALL TIME" />
                        <asp:BoundField DataField="calltrack" HeaderText="CALL TRACK" />
                        <asp:BoundField DataField="REATTEMPT" HeaderText="RE ATTEMPT" />
                        <asp:BoundField DataField="COMMENT" HeaderText="COMMENT" />
                        <asp:BoundField DataField="ACCOUNTNO" HeaderText="ACCOUNT NO" />
                        <asp:BoundField DataField="SHIPPERNAME" HeaderText="SHIPPER NAME" />
                        <asp:BoundField DataField="CONSIGNEE" HeaderText="CONSIGNEE" />
                        <asp:BoundField DataField="CONSIGNEECELL" HeaderText="CONSIGNEE CELL" />
                        <asp:BoundField DataField="CONSIGNEEADDRESS" HeaderText="CONSIGNEE ADDRESS" />
                        <asp:BoundField DataField="ORIGIN" HeaderText="ORIGIN" />
                        <asp:BoundField DataField="DST" HeaderText="DESTINATION" />
                        <asp:BoundField DataField="CREATEDON" HeaderText="CREATED ON" />
                        <asp:BoundField DataField="CREATEDBY" HeaderText="CREATED BY" />
                    </Columns>
                </asp:GridView>
            </div>
        </fieldset>
    </div>
</asp:Content>
