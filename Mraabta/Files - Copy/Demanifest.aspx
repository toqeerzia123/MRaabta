<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.master" CodeBehind="Demanifest.aspx.cs" Inherits="MRaabta.Files.Demanifest" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="div1" runat="server" class="outer_box" style="display: none;">
        <div class="pop_div">
            <table style="width: 100% !important;">
                <tr width="100%">
                    <td style="float: left; margin-top: 12px; text-align: center; width: 228px;">
                        <asp:Label ID="lbl_error2" runat="server" Text="Manifest Number does not Exist. Do you want to Continue."></asp:Label>
                    </td>
                </tr>
                <tr width="100%">
                    <td style="float: left; margin-left: 30px; margin-top: 8px; text-align: center !important;">
                        <input type="button" id="btn_cancel2" value="Cancel" class="button" onclick="altufaltu();" />
                    </td>
                    <td style="float: left; margin-top: 8px; text-align: center !important; width: 50% !important;">
                        <asp:Button ID="btn_ok2" runat="server" Text="OK" CssClass="button" OnClientClick="GoToManifestNew(); return false;" />
                        <%--OnClientClick="window.location.href='Manage_Manifest_new.aspx?mode=d'; return false;"--%>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <telerik:radajaxmanager id="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rd_1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rd_1" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:radajaxmanager>
    <telerik:radcodeblock id="RadCodeBlock1" runat="server">
        <script language="javascript" type="text/javascript">

            function isNumberKey(evt) {
                debugger;

                var count = 1;
                var charCode = (evt.which) ? evt.which : event.keyCode
                if (charCode != 46 && charCode > 31
                    && (charCode < 48 || charCode > 57 || charCode == 110 || charCode == 109 || charCode == 45 || charCode == 9)) {
                    return false;
                }
                else {

                    if (charCode == 110 || charCode == 46) {
                        count++;
                    }
                    if (count > 1) {
                        return false
                    }
                }

                return true;
            }
            function altufaltu() {
                debugger;
                document.getElementById('<%= div1.ClientID %>').style.visibility = "hidden"
                document.getElementById('<%= txt_manifestNumber.ClientID %>').value = "";
                return false;
            }
            function centerLoadingPanel() {
                centerElementOnScreen($get("<%= RadAjaxLoadingPanel1.ClientID %>"));
            }
            function centerElementOnScreen(element) {
                var scrollTop = document.body.scrollTop;
                var scrollLeft = document.body.scrollLeft;
                var viewPortHeight = document.body.clientHeight;
                var viewPortWidth = document.body.clientWidth;
                if (document.compatMode == "CSS1Compat") {
                    viewPortHeight = document.documentElement.clientHeight;
                    viewPortWidth = document.documentElement.clientWidth;
                    if (!$telerik.isSafari) {
                        scrollTop = document.documentElement.scrollTop;
                        scrollLeft = document.documentElement.scrollLeft;
                    }
                }
                var topOffset = Math.ceil(viewPortHeight / 2 - element.offsetHeight / 2);
                var leftOffset = Math.ceil(viewPortWidth / 2 - element.offsetWidth / 2);
                var top = scrollTop + topOffset - 40;
                var left = scrollLeft + leftOffset - 70;
                element.style.position = "absolute";
                element.style.top = top + "px";
                element.style.left = left + "px";
            }


            count = 0;
            function isNumberKey(evt) {

                var charCode = (evt.which) ? evt.which : event.keyCode
                if (charCode != 46 && charCode > 31
            && (charCode < 48 || charCode > 57 || charCode == 110 || charCode == 109 || charCode == 45 || charCode == 9)) {
                    return false;
                }
                else {

                    if (charCode == 110) {
                        count++;
                    }
                    if (count > 1) {
                        return false
                    }
                }

                return true;
            }

            function ConsignmentText() {
                var c = eventArgs.get_keyCode();
                if (c == 13) {
                    // eventArgs.set_cancel(true);
                    var textbox = $find("txtDescription");
                }
            }

            function radCombobox_onLoad(sender) {
                sender._oSelectItemOnBlur = sender._selectItemOnBlur;
                sender._selectItemOnBlur = function (e) {
                    if (!this.get_enableLoadOnDemand())
                        this._oSelectItemOnBlur(e);
                };
            }

            function GoToManifestNew() {
                var man = document.getElementById('<%= txt_manifestNumber.ClientID %>');
                window.location.href = 'Manage_Manifest_new.aspx?mode='.concat(man.value);
            }
            function PageRedirect() {
                window.open('Search_Demanifest.aspx', '_blank');
            }
        </script>
    </telerik:radcodeblock>
    <style>
        .outer_box
        {
            background: #444 none repeat scroll 0 0;
            height: 101%;
            left: 0;
            opacity: 0.9;
            position: absolute;
            top: -1%;
            width: 100%;
        }
        
        
        .pop_div
        {
            background: #eee none repeat scroll 0 0;
            border-radius: 10px;
            height: 100px;
            left: 48%;
            position: relative;
            top: 40%;
            width: 235px;
        }
        
        .btn_ok
        {
            background: #000 none repeat scroll 0 0;
            border: 0 none;
            color: #fff;
            left: -18px;
            padding: 1px 14px;
            position: relative;
            top: 67%;
        }
        
        .btn_cancel
        {
            background: #000 none repeat scroll 0 0;
            border: 0 none;
            color: #fff;
            left: 22%;
            padding: 1px 14px;
            position: relative;
            top: 42%;
        }
        
        .pop_div > span
        {
            float: left;
            line-height: 40px;
            text-align: center;
            width: 100%;
        }
        .tbl-large div
        {
            position: static;
        }
    </style>
    <asp:Panel ID="rd_1" runat="server">
        <div id="divDialogue" runat="server" class="outer_box" style="display: none;">
            <div class="pop_div">
                <table style="width: 100% !important;">
                    <tr width="100%">
                        <td style="float: left; margin-top: 12px; text-align: center; width: 228px;">
                            <asp:Label ID="lbl_error" runat="server" Text="Some CNs are short received. Click OK to Continue."></asp:Label>
                        </td>
                    </tr>
                    <tr width="100%">
                        <td style="float: left; margin-left: 30px; margin-top: 8px; text-align: center !important;">
                            <asp:Button ID="btn_cancelDialogue" runat="server" Text="Cancel" CssClass="button"
                                OnClick="btn_cancelDialogue_Click" />
                        </td>
                        <td style="float: left; margin-top: 8px; text-align: center !important; width: 50% !important;">
                            <asp:Button ID="btn_okDialogue" runat="server" Text="OK" CssClass="button" OnClick="btn_okDialogue_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <table style="font-family: Calibri; font-size: medium; padding-bottom: 0px !important;
            padding-top: 0px !important" class="input-form">
            <tr style="padding: 0px 0px 0px 0px !important;">
                <td colspan="5" style="padding-bottom: 1px !important; padding-top: 1px !important;
                    text-align: center !important; float: left; width: 100%">
                    <h4 style="font-family: Calibri; margin: 0px !important; font-variant: small-caps;
                        text-align: center !important; width: 100%">
                        Manifest Info</h4>
                </td>
            </tr>
            <tr>
                <td colspan="5" style="text-align: right; width: 100%; float: left;">
                    <asp:Button ID="btn_searchDemanifest" runat="server" CssClass="button" Text="Search Demanifests"
                        OnClientClick="PageRedirect(); return false;"></asp:Button>
                </td>
            </tr>
            <tr>
                <td class="field" style="width: 15% !important;">
                    Manifest Number
                </td>
                <td class="input-field" style="width: 15% !important;">
                    <asp:TextBox ID="txt_manifestNumber" runat="server" OnTextChanged="txt_manifestNumber_TextChanged"
                        AutoPostBack="true" onkeypress="return isNumberKey(event);"></asp:TextBox>
                </td>
                <td class="space">
                </td>
                <td class="field" style="width: 15% !important;">
                    Date
                </td>
                <td class="input-field" style="width: 15% !important;">
                    <asp:TextBox ID="txt_date" runat="server" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="field" style="width: 15% !important;">
                    Origin
                </td>
                <td class="input-field" style="width: 15% !important;">
                    <asp:TextBox ID="txt_origin" runat="server" Enabled="false"></asp:TextBox>
                    <asp:HiddenField ID="hd_origin" runat="server" />
                </td>
                <td class="space">
                </td>
                <td class="field" style="width: 15% !important;">
                    Destination
                </td>
                <td class="input-field" style="width: 15% !important;">
                    <asp:TextBox ID="txt_destination" runat="server" Enabled="false"></asp:TextBox>
                    <asp:HiddenField ID="hd_destination" runat="server" />
                </td>
            </tr>
        </table>
        <fieldset style="border-radius: 5px !important; font-weight: bold; font-size: medium;
            padding: 10px !important; margin: 18px !important; width: 95%;">
            <legend>Manifest Contents (Consignments)</legend>
            <table style="width: 100%">
                <tr>
                    <td style="width: 13%; font-weight: bold;">
                        CN Number
                    </td>
                    <td style="width: 20%;">
                        <asp:TextBox ID="txt_cnNumber" runat="server" OnTextChanged="txt_cnNumber_TextChanged"
                            AutoPostBack="true" onkeypress="return isNumberKey(event);"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
            <div style="width: 100%; height: 250px; overflow: scroll; text-align: center;">
                <span id="Table_1" class="tbl-large" style="width: 100%;">
                    <asp:Label ID="err_msg" runat="server" CssClass="error_msg"></asp:Label>
                    <asp:GridView ID="gv_consignments" runat="server" AutoGenerateColumns="false" CssClass="mGrid"
                        AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"
                        Width="95%" EmptyDataText="No Data Available" OnRowDataBound="gv_consignments_RowDataBound">
                        <RowStyle Font-Bold="false" />
                        <Columns>
                            <asp:TemplateField HeaderText="Received">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chk_received" runat="server" OnCheckedChanged="chk_received_CheckedChanged"
                                        AutoPostBack="true" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Consignment #" DataField="consignmentNumber" />
                            <asp:BoundField HeaderText="Status" DataField="DeManifestStateId" />
                            <asp:TemplateField HeaderText="Reason">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_reason" runat="server" CssClass="newbox" Text='<%# DataBinder.Eval(Container.DataItem, "Remarks") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:BoundField HeaderText="Reason" DataField="Remarks" />--%>
                            <asp:BoundField HeaderText="Origin" DataField="Origin" />
                            <asp:BoundField HeaderText="Destination" DataField="Dest" />
                            <asp:BoundField HeaderText="Consignment Type" DataField="ConType" />
                            <asp:BoundField HeaderText="Service Type" DataField="serviceTypeName" />
                            <asp:BoundField HeaderText="Weight" DataField="Weight" />
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hd_manifestID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "manifestID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </span>
                <asp:Button ID="btn_applyDefault" runat="server" Text="Apply Default Tariff" CssClass="button"
                    Visible="false" />
            </div>
        </fieldset>
        <div style="width: 100%; text-align: center">
            <asp:Button ID="btn_reset" runat="server" Text="Reset" CssClass="button" OnClick="btn_reset_Click" />
            &nbsp;
            <asp:Button ID="btn_save" runat="server" Text="Save" CssClass="button" OnClick="btn_save_Click" />
            &nbsp;
            <asp:Button ID="btn_print" runat="server" Text="Print Report" CssClass="button" Visible="false"
                OnClick="btn_print_Click" />
            &nbsp;
            <asp:Button ID="btn_cancel" runat="server" Text="Cancel" CssClass="button" OnClick="btn_cancel_Click" />
        </div>
    </asp:Panel>
    <telerik:radajaxloadingpanel id="RadAjaxLoadingPanel1" runat="server" skin="Web20"
        issticky="true" height="200px">
        <img src="~/ajax_loader.gif" alt="loading.." />
    </telerik:radajaxloadingpanel>
</asp:Content>
