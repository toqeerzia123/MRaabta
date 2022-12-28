<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="RiderEntryForm_.aspx.cs" Inherits="MRaabta.Files.RiderEntryForm_" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        input
        {
            padding: 3px;
        }
    </style>
    <script type="text/javascript">
        function myfunction(input) {

            //if (input.length != 15) {
            var format_and_pos = function (char, backspace) {
                var start = 0;
                var end = 0;
                var pos = 0;
                var separator = "-";
                var value = input.value;

                debugger;

                if (char !== false) {
                    start = input.selectionStart;
                    end = input.selectionEnd;

                    if (backspace && start > 0) // handle backspace onkeydown
                    {
                        start--;

                        if (value[start] == separator) { start--; }
                    }
                    // To be able to replace the selection if there is one
                    value = value.substring(0, start) + char + value.substring(end);

                    pos = start + char.length; // caret position
                }

                var d = 0; // digit count
                var dd = 0; // total
                var gi = 0; // group index
                var newV = "";
                var groups = /^\D*3[47]/.test(value) ? // check for American Express
                    [4, 6, 5] : [5, 7, 1];

                for (var i = 0; i < value.length; i++) {
                    if (/\D/.test(value[i])) {
                        if (start > i) { pos--; }
                    }
                    else {
                        if (d === groups[gi]) {
                            newV += separator;
                            d = 0;
                            gi++;

                            if (start >= i) { pos++; }
                        }
                        newV += value[i];
                        d++;
                        dd++;
                    }
                   <%-- if (13 == dd) // return
                    {

                        if (navigator.userAgent.indexOf("Firefox") != -1) {
                            window.setTimeout(function () {
                                document.getElementById('<%= txt_SalesGroup.ClientID %>').focus();
                            }, 0);
                        }
                        else if ((navigator.userAgent.indexOf("MSIE") != -1) || (!!document.documentMode == true)) //IF IE > 10
                        {
                            setTimeout(function () { document.getElementById('<%= txt_SalesGroup.ClientID %>').focus(); }, 10);
                        }

                }--%>
                    if (d === groups[gi] && groups.length === gi + 1) // max length
                    { break; }

                }
                input.value = newV;

                if (char !== false) { input.setSelectionRange(pos, pos); }
            };

            input.addEventListener('keypress', function (e) {
                var code = e.charCode || e.keyCode || e.which;

                // Check for tab and arrow keys (needed in Firefox)
                if (code !== 9 && (code < 37 || code > 40) &&
                    // and CTRL+C / CTRL+V
                    !(e.ctrlKey && (code === 99 || code === 118))) {
                    e.preventDefault();

                    var char = String.fromCharCode(code);

                    // if the character is non-digit
                    // OR
                    // if the value already contains 15/16 digits and there is no selection
                    // -> return false (the character is not inserted)

                    if (/\D/.test(char) || (this.selectionStart === this.selectionEnd &&
                        this.value.replace(/\D/g, '').length >=
                        (/^\D*3[47]/.test(this.value) ? 15 : 16))) // 15 digits if Amex
                    {
                        return false;
                    }
                    format_and_pos(char);
                }
            });


        };

    </script>
    <br />
    <div style="text-align: right; padding-right: 5px">
        <asp:Label ID="lbl_error" runat="server" Font-Size="Large" />
    </div>
    <asp:LinkButton Text="Edit" PostBackUrl="~/Files/RiderDeactivate_Form.aspx" CausesValidation="false"
        runat="server" />
    <br />
    <fieldset>
        <legend style="font-size: medium;">Rider Entry Form</legend>
        <table style="font-size: Small; padding-bottom: 0px; width: 100%; padding-top: 0px !important">
            <tr>
                <td class="field" style="width: 12%; text-align: right; padding-right: 10px;">
                    <b>First Name </b>
                </td>
                <td style="width: 15%; text-align: left;">
                    <asp:TextBox ID="txt_FirstName" runat="server" MaxLength="50" Width="160px"></asp:TextBox>
                </td>
                <td class="field" style="width: 12%; text-align: right; padding-right: 10px;">
                    <b>Middle Name</b>
                </td>
                <td style="width: 15%; text-align: left;">
                    <asp:TextBox ID="txt_MiddleName" runat="server" MaxLength="50" Width="160px"></asp:TextBox>
                </td>
                <td class="field" style="width: 12%; text-align: right; padding-right: 10px;">
                    <b>Last Name</b>
                </td>
                <td style="width: 15%; text-align: left;">
                    <asp:TextBox ID="txt_LastName" runat="server" MaxLength="50" Width="160px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="field" style="width: 12%; text-align: right; padding-right: 10px;">
                    <b>CNIC </b>
                </td>
                <td style="width: 15%; text-align: left;">
                    <asp:TextBox ID="txt_CNIC" runat="server" oninput="myfunction(this);" MaxLength="12"
                        Width="160px"></asp:TextBox>
                </td>
                <td class="field" style="width: 12%; text-align: right; padding-right: 10px;">
                    <b>Rider Type </b>
                </td>
                <td style="width: 15%; text-align: left;">
                    <asp:DropDownList ID="ddl_cid" runat="server" Width="145px" Height="30px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="field" style="width: 12%; text-align: right; padding-right: 10px;">
                    <b>Address </b>
                </td>
                <td style="width: 15%; text-align: left;">
                    <asp:TextBox ID="txt_address" runat="server" MaxLength="50" Width="160px"></asp:TextBox>
                </td>
                <td class="field" style="width: 12%; text-align: right; padding-right: 10px;">
                    <b>Phone No </b>
                </td>
                <td style="width: 15%; text-align: left;">
                    <asp:TextBox ID="txt_phoneNo" runat="server" MaxLength="12" Width="160px"></asp:TextBox>
                </td>
                <td class="field" style="width: 12%; text-align: right; padding-right: 10px;">
                    <b>Duty Type </b>
                </td>
                <td style="width: 15%; text-align: left;">
                    <asp:DropDownList ID="ddl_dutyType" runat="server" Width="145px" Height="30px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="field" style="width: 12%; text-align: right; padding-right: 10px;">
                    <b>Email </b>
                </td>
                <td style="width: 15%; text-align: left;">
                    <asp:TextBox ID="txt_email" runat="server" MaxLength="12" Width="160px"></asp:TextBox>
                </td>
                <td class="field" style="width: 12%; text-align: right; padding-right: 10px;">
                </td>
                <td style="width: 15%; text-align: left;">
                    <asp:TextBox ID="txt_lastrouteCode" Visible="false" runat="server" MaxLength="12"
                        Width="160px"></asp:TextBox>
                </td>
                <td class="field" style="width: 12%; text-align: right; padding-right: 10px;">
                    <b>Department </b>
                </td>
                <td style="width: 15%; text-align: left;">
                    <asp:DropDownList ID="ddl_dept" runat="server" Width="120px" Height="30px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; /* margin-top: auto; */vertical-align: top; padding-right: 10px;
                    padding-top: 9px;">
                    <b>Zone Id</b>
                </td>
                <td style="width: 20%; text-align: left;">
                    <asp:DropDownList ID="ddl_zoneId" runat="server" Width="120px" Height="30px" OnSelectedIndexChanged="ddl_zoneId_SelectedIndexChanged"
                        AutoPostBack="true">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfv1" runat="server" ForeColor="Red" ControlToValidate="ddl_zoneId"
                        InitialValue="Select Zone" ErrorMessage="Please select Zone" />
                </td>
                <td style="text-align: right; /* margin-top: auto; */vertical-align: top; padding-right: 10px;
                    padding-top: 9px;">
                    <b>Branch Id </b>
                </td>
                <td style="width: 20%; text-align: left;">
                    <asp:DropDownList ID="ddl_branchId" runat="server" Width="120px" Height="31px" AutoPostBack="true"
                        OnSelectedIndexChanged="ddl_branchId_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ForeColor="Red"
                        ControlToValidate="ddl_branchId" InitialValue="Select Branch" ErrorMessage="Please select Branch" />
                </td>
                <td style="text-align: right; /* margin-top: auto; */vertical-align: top; padding-right: 10px;
                    padding-top: 9px;">
                    <b>E Center </b>
                </td>
                <td style="text-align: left;">
                    <asp:DropDownList ID="ddl_expressCenterId" runat="server" Width="120px" Height="30px"
                        OnSelectedIndexChanged="ddl_expressCenterId_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="field" style="width: 12%; text-align: right; padding-right: 10px;">
                    <b>Shift </b>
                </td>
                <td style="width: 15%; text-align: left;">
                    <asp:DropDownList ID="ddl_Shift" runat="server" Width="145px" Height="30px">
                    </asp:DropDownList>
                </td>
                <td class="field" style="width: 12%; text-align: right; padding-right: 10px;">
                    <b>Rider Code</b>
                </td>
                <td style="width: 15%; text-align: left;">
                    <asp:TextBox ID="txt_riderCode" runat="server" MaxLength="12" Width="160px" AutoPostBack="True"
                        OnTextChanged="txt_riderCode_TextChanged"></asp:TextBox>
                </td>
                <td>
                    <asp:HyperLink ID="lb_riderlink" runat="server" Target="_blank"></asp:HyperLink>
                </td>
            </tr>
        </table>
    </fieldset>
    <br />
    <div>
        <fieldset>
            <legend>Rider Entry Form</legend>
            <table style="font-size: Small; padding-bottom: 0px; width: 100%; padding-top: 0px !important">
                <tr>
                    <td class="field" style="width: 12%; text-align: right; padding-right: 10px;">
                        <b>Route Code </b>
                    </td>
                    <td style="width: 15%; text-align: left;">
                        <asp:TextBox ID="txt_routeCode" runat="server" MaxLength="12" Width="160px" 
                            AutoPostBack="True" ontextchanged="txt_routeCode_TextChanged"></asp:TextBox>
                    </td>
                    <td class="field" style="width: 12%; text-align: right; padding-right: 10px;">
                    </td>
                    <td style="width: 15%; text-align: left;">
                        <asp:DropDownList ID="ddl_city" runat="server" Width="145px" Height="30px">
                        </asp:DropDownList>
                    </td>
                    <td class="field" style="width: 12%; text-align: right; padding-right: 10px;">
                        <b>Route Name </b>
                    </td>
                    <td style="width: 15%; text-align: left;">
                        <asp:TextBox ID="txt_routeName" runat="server" MaxLength="25" Width="160px"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <div>
        <fieldset>
            <legend>HR Section </legend>
            <table style="font-size: Small; padding-bottom: 0px; width: 100%; padding-top: 0px !important">
                <tr>
                    <td class="field" style="width: 12%; text-align: right; padding-right: 10px;">
                        <b>HRS Code </b>
                    </td>
                    <td style="width: 15%; text-align: left;">
                        <asp:TextBox ID="txt_hrs_Code" runat="server" MaxLength="12" Width="160px"></asp:TextBox>
                    </td>
                    <td class="field" style="width: 12%; text-align: right; padding-right: 10px;">
                        <b>Date of Joining</b>
                    </td>
                    <td style="width: 15%; text-align: left;">
                        <asp:TextBox ID="txt_DOj" runat="server" MaxLength="25" Width="160px"></asp:TextBox>
                        <Ajax1:CalendarExtender ID="CalendarExtender1" runat="server" Format="yyyy-MM-dd"
                            PopupButtonID="txt_DOj" TargetControlID="txt_DOj">
                        </Ajax1:CalendarExtender>
                    </td>
                    <td class="field" style="width: 12%; text-align: right; padding-right: 10px;">
                        <b>Date of Birth </b>
                    </td>
                    <td style="width: 15%; text-align: left;">
                        <asp:TextBox ID="txt_DOB" runat="server" MaxLength="25" Width="160px"></asp:TextBox>
                        <Ajax1:CalendarExtender ID="ceLoanTakenDate" runat="server" Format="yyyy-MM-dd" PopupButtonID="txt_DOB"
                            TargetControlID="txt_DOB">
                        </Ajax1:CalendarExtender>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <br />
    <table class="input-form" style="width: 97%">
        <tr>
            <td class="field">
                <asp:Button Text="Save" ID="btn_save" OnClick="btn_save_Click" runat="server" CssClass="button1"
                    Width="60px" />
            </td>
        </tr>
    </table>
</asp:Content>
