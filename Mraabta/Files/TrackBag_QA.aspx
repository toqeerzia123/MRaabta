<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="TrackBag_QA.aspx.cs" Inherits="MRaabta.Files.TrackBag_QA" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .outer_box
        {
            background: #444 none repeat scroll 0 0;
            height: 110%;
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
            height: 500px;
            left: 21%;
            position: relative;
            top: 15%;
            width: 1008px;
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
        .btninaspclass
        {
            font-weight: bold;
        }
        .tabsClass
        {
        }
    </style>
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
            document.getElementById("<%=txt_bagNo.ClientID %>").value = "";
            return false;
        }
        function altufaltu2() {
            debugger;
            document.getElementById('<%= div1.ClientID %>').style.visibility = "hidden"


        }
        function txtManChanged() {

            var gridView = document.getElementById("<%=gv_bagManifests.ClientID %>");
            var txtMan = document.getElementById("<%=txt_manifest.ClientID %>");
            var txtbag = document.getElementById("<%=txt_bagNo.ClientID %>");
            debugger;
            if (txtbag.value == "") {
                alert('Provide Bag Number');
                return false;
            }
            var tbody = gridView.getElementsByTagName("tbody")[0];
            var flag = 1;
            for (var i = 1; i < gridView.rows.length; i++) {
                var row = tbody.getElementsByTagName("tr")[i];
                if (row.cells.length == 1) {
                    return true;
                }
                var column = row.getElementsByTagName("td")[4];
                var txtDesc = row.cells[4].children[0];
                if (row.getElementsByTagName("td")[1].innerText.trim() == txtMan.value) {
                    flag = 0;

                    if (txtDesc.value == "EXCESS RECEIVED") {
                        alert('This Bag is already Excess Received');
                        return false;
                    }
                    txtMan.value = '';
                    var chk = row.getElementsByTagName("input");
                    chk[0].checked = true;






                    txtDesc.value = 'RECEIVED';

                    //browser handling start
                    if (navigator.userAgent.indexOf("Firefox") != -1) {
                        window.setTimeout(function () {
                            txtMan.focus();
                        }, 0);
                    }
                    else if ((navigator.userAgent.indexOf("MSIE") != -1) || (!!document.documentMode == true)) //IF IE > 10
                    {
                        setTimeout(function () { txtMan.focus(); }, 10);
                    }
                    //browser handling END
                    break;
                    //alert('3 ok');
                    //hdstatus.value = textbox.value;
                }
                //alert(weight);
            }

            if (flag == 1) {
                return true;
            }
            else {
                return false;
            }
            //return flag;
            //var hdstatus = column.getElementsByTagName('hd_Gstatus');

        }

        function chkManChanged() {
            debugger;
            var gridView = document.getElementById("<%=gv_bagManifests.ClientID %>");
            var tbody = gridView.getElementsByTagName("tbody")[0];

            for (var i = 1; i < gridView.rows.length; i++) {
                var row = tbody.getElementsByTagName("tr")[i];
                var column = row.getElementsByTagName("td")[4];
                var chk = row.getElementsByTagName("input");
                if (chk[0].type == "checkbox" & chk[0].checked) {

                    var txtDesc = row.cells[4].children[0];
                    if (txtDesc.value == 'EXCESS RECEIVED') {

                    }
                    else {
                        txtDesc.value = 'RECEIVED';
                    }

                }
                else {

                    var txtDesc = row.cells[4].children[0];
                    txtDesc.value = 'SHORT RECEIVED';

                }

            }
            return false;
        }

        function validateGrids() {
            debugger;
            var gridView = document.getElementById("<%=gv_bagManifests.ClientID %>");
            var gvCn = document.getElementById("<%=gv_outpieces.ClientID %>");
            var tbody = gridView.getElementsByTagName("tbody")[0];
            var shortCheck = "";
            for (var i = 1; i < gridView.rows.length; i++) {
                var row = tbody.getElementsByTagName("tr")[i];
                var chk = row.cells[0].children[0];
                var txtDesc = row.cells[4].children[0];
                if (txtDesc.value == "EXCESS RECEIVED") {

                }
                else if (!(chk.checked)) {

                    txtDesc.value = 'SHORT RECEIVED';
                    shortCheck = "0";
                }


            }
            var tbodycn = gvCn.getElementsByTagName("tbody")[0];
            var shortcnCheck = "";
            for (var i = 1; i < gvCn.rows.length; i++) {
                var row = tbodycn.getElementsByTagName("tr")[i];
                var chk = row.cells[0].children[0];
                var txtDesc = row.cells[4].children[0];
                if (txtDesc.value == "EXCESS RECEIVED") {

                }
                else if (!(chk.checked)) {

                    txtDesc.value = 'SHORT RECEIVED';
                    shortcnCheck = "0";
                }
            }
            if (shortcnCheck == "0" && shortCheck == "0") {
                return confirm("Some Bags and Out Pieces are short received. Do you want to continue?");
            }
            if (shortCheck == "0") {

                return confirm('Some Bags are short received. Continue?');
            }
            if (shortcnCheck == "0") {
                return confirm('Some Out Pieces are short received. Continue?');
            }

            return true;
        }
    </script>
    <script>
        function txtCNChanged() {
            
            var gridView = document.getElementById("<%=gv_outpieces.ClientID %>");
            var txtMan = document.getElementById("<%=txt_cnNo.ClientID %>");
            var txtbag = document.getElementById("<%=txt_bagNo.ClientID %>");
            if (txtMan.value.length > 15 || txtMan.value.length < 11) {
                alert('Consignment Number must be between 11 and 15 digits');
                txtMan.value = "";
                txtMan.focus();
                return false;
            }
            debugger;
            if (txtbag.value == "") {
                alert('Provide Bag Number');
                return false;
            }
            var tbody = gridView.getElementsByTagName("tbody")[0];
            var flag = 1;
            for (var i = 1; i < gridView.rows.length; i++) {
                var row = tbody.getElementsByTagName("tr")[i];
                if (row.cells.length == 1) {
                    return true;
                }
                var column = row.getElementsByTagName("td")[4];

                var txtDesc = row.cells[4].children[0];
                if (row.getElementsByTagName("td")[1].innerText.trim() == txtMan.value) {
                    flag = 0;
                    if (txtDesc.value == "EXCESS RECEIVED") {
                        alert('This CN is already Excess Received');
                        return false;
                    }

                    txtMan.value = '';
                    var chk = row.getElementsByTagName("input");
                    chk[0].checked = true;


                    debugger;



                    txtDesc.value = 'RECEIVED';

                    //browser handling start
                    if (navigator.userAgent.indexOf("Firefox") != -1) {
                        window.setTimeout(function () {
                            txtMan.focus();
                        }, 0);
                    }
                    else if ((navigator.userAgent.indexOf("MSIE") != -1) || (!!document.documentMode == true)) //IF IE > 10
                    {
                        setTimeout(function () { txtMan.focus(); }, 10);
                    }
                    //browser handling END
                    break;
                    //alert('3 ok');
                    //hdstatus.value = textbox.value;
                }
                //alert(weight);
            }

            if (flag == 1) {
                return true;
            }
            else {
                return false;
            }
            //return flag;
            //var hdstatus = column.getElementsByTagName('hd_Gstatus');

        }


        function chkCNChanged() {
            debugger;
            var gridView = document.getElementById("<%=gv_outpieces.ClientID %>");
            var tbody = gridView.getElementsByTagName("tbody")[0];

            for (var i = 1; i < gridView.rows.length; i++) {
                var row = tbody.getElementsByTagName("tr")[i];
                var column = row.getElementsByTagName("td")[4];
                var chk = row.getElementsByTagName("input");
                var txtDesc = row.cells[4].children[0];

                if (chk[0].type == "checkbox" & chk[0].checked) {


                    txtDesc.value = 'RECEIVED';
                }
                else {


                    txtDesc.value = 'SHORT RECEIVED';

                }

            }
            return false;
        }
    </script>
    <div id="div1" runat="server" class="outer_box" style="display: none;">
        <div class="pop_div">
            <table style="width: 100% !important;">
                <tr width="100%">
                    <td style="float: left; margin-top: 12px; text-align: center; width: 228px;">
                        <asp:Label ID="lbl_error2" runat="server" Text="Bag Number Does not Exist. Do you want to Continue."></asp:Label>
                    </td>
                </tr>
                <tr width="100%">
                    <td style="float: left; margin-left: 30px; margin-top: 8px; text-align: center !important;">
                        <input type="button" id="btn_cancel2" value="Cancel" class="button" onclick="altufaltu();" />
                    </td>
                    <td style="float: left; margin-top: 8px; text-align: center !important; width: 50% !important;">
                        <asp:Button ID="btn_ok2" runat="server" Text="OK" CssClass="button" OnClick="btn_ok2_Click"
                            OnClientClick="altufaltu2();" />
                        <%--OnClientClick="window.location.href='Manage_Manifest_new.aspx?mode=d'; return false;"--%>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <%--<div id="divDialogue" runat="server" class="outer_box" style="display: none;">
        <div class="pop_div">
            <table style="border: 2px solid red; width: 100% !important; height: 100% !important;">
                <tr style="height: 20px;">
                    <td style="width: 100% !important; height: 20px; text-align: right; vertical-align: top;"
                        colspan="2">
                        <asp:Button ID="btn_closePopUp" runat="server" Text="X" CssClass="btninaspclass"
                            OnClick="btn_closePopUp_Click" />
                    </td>
                </tr>
                <tr style="height: 20px;">
                    <td valign="top" style="width: 10%; font-weight: bold; margin-left: 20px;">
                        Manifest Number
                    </td>
                    <td valign="top">
                        <asp:TextBox ID="txt_manifestNumber" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="top" colspan="2">
                    </td>
                </tr>
                <tr style="height: 20px; margin-bottom: 5px; margin-right: 10px; text-align: right;">
                    <td valign="top" style="width: 10%; font-weight: bold; margin-left: 20px;">
                    </td>
                    <td valign="top" style="text-align: right; float: right; margin-right: 25px; margin-bottom: 20px;">
                        <asp:Button ID="btn_ok" runat="server" Text="OK" CssClass="button" OnClick="btn_ok_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </div>--%>
    <asp:Label ID="Errorid" runat="server" Font-Bold="true" Font-Size="Large"></asp:Label>
    <table style="font-family: Calibri; font-size: medium; padding-bottom: 0px !important;
        padding-top: 0px !important; width: 97%" class="input-form">
        <tr style="padding: 0px 0px 0px 0px !important;">
            <td colspan="10" style="padding-bottom: 1px !important; padding-top: 1px !important;">
                <h4 style="font-family: Calibri; margin: 0px !important;">
                    Bag Info</h4>
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important;">
                
            </td>
            <td class="input-field" style="width: 15%">
               
            </td>
            <td class="space" style="margin-right: 5% !important">
            </td>
            <td class="field" style="width: 10% !important;">
                
            </td>
            <td class="input-field" style="width: 15%">
                
            </td>
            <td class="space" style="margin-right: 5% !important">
            </td>
            <td class="field" style="width: 10% !important;">
                
            </td>
            <td class="input-field" style="width: 15% !important; text-align: right;">
                <asp:Button ID="btn_search" runat="server" Text="Search Debag" CssClass="button" OnClick="btn_search_Click" />
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important;">
                Bag No
                <asp:HiddenField ID="hd_debagWithoutBag" runat="server" />
            </td>
            <td class="input-field" style="width: 15%">
                <asp:TextBox ID="txt_bagNo" runat="server" Width="100%" AutoPostBack="true" OnTextChanged="txt_bagNo_TextChanged"></asp:TextBox>
            </td>
            <td class="space" style="margin-right: 5% !important">
            </td>
            <td class="field" style="width: 10% !important;">
                Origin
            </td>
            <td class="input-field" style="width: 15%">
                <asp:TextBox ID="txt_origin" runat="server" Width="100%" Visible="false"></asp:TextBox>
                <asp:DropDownList ID="dd_origin" runat="server" Width="100%" Enabled="false" AppendDataBoundItems="true">
                    <asp:ListItem></asp:ListItem>
                </asp:DropDownList>
                <asp:HiddenField ID="hd_origin" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important;">
                Destination
            </td>
            <td class="input-field" style="width: 15%">
                <asp:TextBox ID="txt_destination" runat="server" Width="100%" Visible="false"></asp:TextBox>
                <asp:DropDownList ID="dd_destination" runat="server" Width="100%" Enabled="false"
                    AppendDataBoundItems="true">
                    <asp:ListItem></asp:ListItem>
                </asp:DropDownList>
                <asp:HiddenField ID="hd_destination" runat="server" />
            </td>
            <td class="space" style="margin-right: 5% !important">
            </td>
            <td class="field" style="width: 10% !important;">
                Total Weight
            </td>
            <td class="input-field" style="width: 15%">
                <asp:TextBox ID="txt_totalWeight" runat="server" Width="100%"></asp:TextBox>
            </td>
            <td class="space" style="margin-right: 5% !important">
            </td>
            <td class="field" style="width: 10% !important;">
                Seal No
            </td>
            <td class="input-field" style="width: 15%">
                <asp:TextBox ID="txt_sealNo" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important;">
                Date
            </td>
            <td class="input-field" style="width: 15%">
                <asp:TextBox ID="txt_date" runat="server" Width="100%"></asp:TextBox>
                <Ajax1:CalendarExtender ID="calendar1" runat="server" TargetControlID="txt_date"
                    Format="dd/MM/yyyy">
                </Ajax1:CalendarExtender>
            </td>
            <td class="space" style="margin-right: 5% !important">
            </td>
            <td class="field" style="width: 10% !important;">
                Description
            </td>
            <td class="input-field" style="width: 49% !important;" colspan="4">
                <asp:TextBox ID="txt_description" runat="server" Width="100%" TextMode="MultiLine"
                    Rows="2"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table style="font-family: Calibri; font-size: medium; padding-bottom: 0px !important;
        padding-top: 0px !important; width: 97%" class="input-form">
        <tr style="padding: 0px 0px 0px 0px !important;">
            <td colspan="10" style="padding-bottom: 1px !important; padding-top: 1px !important;">
                <h4 style="font-family: Calibri; margin: 0px !important;">
                    Tracking Details</h4>
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important;">
                Arrived At
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:DropDownList ID="dd_arrivedAt" runat="server" Width="100%">
                </asp:DropDownList>
            </td>
            <td class="space" style="margin-right: 5% !important">
            </td>
            <td class="field" style="width: 10% !important;">
                Departure To
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:DropDownList ID="dd_departureTo" runat="server" AppendDataBoundItems="true"
                    Enabled="false" Width="100%">
                    <asp:ListItem Value="0">Select Destination</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important;">
                Status
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:RadioButtonList ID="rbtn_status" runat="server" RepeatColumns="2" AutoPostBack="true"
                    RepeatDirection="Horizontal" OnSelectedIndexChanged="rbtn_status_SelectedIndexChanged">
                    <asp:ListItem Value="0">Re-Routed</asp:ListItem>
                    <asp:ListItem Value="1">Final</asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <td class="space" style="margin-right: 5% !important">
            </td>
        </tr>
    </table>
    <Ajax1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" Height="300px"
        Style="margin-left: 4% !important; margin-right: 4% !important;">
        <Ajax1:TabPanel runat="server" HeaderText="Manifest" ID="TabPanel1" Width="90%" Height="300px"
            Style="overflow: scroll;">
            <HeaderTemplate>
                Manifests
            </HeaderTemplate>
            <ContentTemplate>
                <table style="width: 100% !important; height: 100% !important;">
                    <tr>
                        <td valign="top" style="width: 20%; font-weight: bold; margin-left: 20px;">
                            Manifest Number
                        </td>
                        <td valign="top" style="width: 80%">
                            <asp:TextBox ID="txt_manifest" onchange="if(!txtManChanged()) { return false;}" runat="server"
                                AutoPostBack="True" OnTextChanged="txt_manifest_TextChanged" onkeypress="return isNumberKey(event);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" style="text-align: right; width: 100%;" colspan="2">
                            <div class="tbl-large">
                                <asp:GridView ID="gv_bagManifests" runat="server" AutoGenerateColumns="False" CssClass="mGrid"
                                    BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" ShowHeaderWhenEmpty="True"
                                    Width="98%" EmptyDataText="No Data Available">
                                    <AlternatingRowStyle CssClass="alt" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Received">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chk_received" runat="server" AutoPostBack="false" OnCheckedChanged="chk_received_CheckedChanged"
                                                    onclick="chkManChanged();" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Manifest No." DataField="ManifestNo" />
                                        <asp:BoundField HeaderText="Origin" DataField="Origin" />
                                        <asp:BoundField HeaderText="Destination" DataField="Dest" />
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_statusDesc" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "StatusDesc") %>'
                                                    CssClass="newbox" Enabled="false"></asp:TextBox>
                                                <%--<asp:Label ID="lbl_gStatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "StatusDesc") %>' Enabled="false"></asp:Label>--%>
                                                <asp:HiddenField ID="hd_Gstatus" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "statusCode") %>' />
                                                <asp:HiddenField ID="hd_Gorigin" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "OriginID") %>' />
                                                <asp:HiddenField ID="hd_Gdestination" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "DestinationID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reason">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_remarks" CssClass="newbox" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Reason") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status" Visible="false">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="dd_status" runat="server">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
            </ContentTemplate>
        </Ajax1:TabPanel>
        <Ajax1:TabPanel ID="TabPanel2" runat="server" HeaderText="Consignments" Height="300px" Style="overflow: scroll;">
            <ContentTemplate>
                <table style="width: 100% !important; height: 100% !important;">
                    <tr>
                        <td valign="top" style="width: 20%; font-weight: bold; margin-left: 20px;">
                            Consignment Number
                        </td>
                        <td valign="top" style="width: 80%">
                            <asp:TextBox ID="txt_cnNo" runat="server" AutoPostBack="true" onchange="if(!txtCNChanged()) { return false;}"
                                OnTextChanged="txt_outpiece_TextChanged" onkeypress="return isNumberKey(event);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" style="text-align: right; width: 100%;" colspan="2">
                            <div class="tbl-large">
                                <asp:GridView ID="gv_outpieces" runat="server" AutoGenerateColumns="false" CssClass="mGrid"
                                    AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"
                                    ShowHeaderWhenEmpty="true" Width="98%" EmptyDataText="No Data Available">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chk_received" runat="server" onclick="chkCNChanged();" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="CN No." DataField="outpieceNumber" />
                                        <asp:BoundField HeaderText="Origin" DataField="originName" />
                                        <asp:BoundField HeaderText="Destination" DataField="destName" />
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_statusDesc" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "StatusDesc") %>'
                                                    CssClass="newbox" Enabled="false"></asp:TextBox>
                                                <asp:HiddenField ID="hd_Gstatus" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "statusCode") %>' />
                                                <asp:HiddenField ID="hd_Gorigin" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "origin") %>' />
                                                <asp:HiddenField ID="hd_Gdestination" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "destination") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:BoundField HeaderText="Description" DataField="Description" />--%>
                                        <asp:TemplateField HeaderText="Reason">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_remarks" CssClass="newbox" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "reason") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Weight" ItemStyle-Width="15px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_cnWeight" CssClass="newbox" Width="15px" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "weight") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Pieces" ItemStyle-Width="15px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_cnPieces" CssClass="newbox" runat="server" Width="15px" Text='<%# DataBinder.Eval(Container.DataItem, "pieces") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
            </ContentTemplate>
        </Ajax1:TabPanel>
    </Ajax1:TabContainer>
    <div style="width: 100%; text-align: center">
        <asp:Button ID="btn_reset" runat="server" Text="Reset" CssClass="button" OnClick="btn_reset_Click" />
        &nbsp;
        <asp:Button ID="btn_save" runat="server" Text="Save" CssClass="button" CommandName="first"
            OnClientClick="if(!validateGrids()) { return false;}" OnClick="btn_save_Click" />
        &nbsp;
        <asp:Button ID="btn_cancel" runat="server" Text="Cancel" CssClass="button" />
    </div>
</asp:Content>
