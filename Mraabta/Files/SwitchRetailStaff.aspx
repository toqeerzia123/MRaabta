<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="SwitchRetailStaff.aspx.cs" Inherits="MRaabta.Files.SwitchRetailStaff" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <script type="text/javascript">


         function isNumberKey(evt) {
             var charCode = (evt.which) ? evt.which : event.keyCode
             if (charCode > 31 && (charCode < 48 || charCode > 57))
                 return false;
             return true;
         }

         function isNumberKeyWithDecimal(evt) {
             var status = false;
             var charCode = (evt.which) ? evt.which : event.keyCode

             if (charCode > 31 && (charCode < 48 || charCode > 57))
                 status = false;
             if (charCode == 46)
                 status = true;
             if (charCode > 47 && charCode < 58)
                 status = true;
             return status;
         }


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
    <style>
        input
        {
            padding: 3px;
        }
           tr.spaceUnder>td {
                    padding-bottom: 6px;
                }
    </style>
      <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
        <tr>
            <td colspan="12" align="center" class="head_column">
                <h3 style="text-align: center">
                   Retail Staff
                </h3>
            </td>
        </tr>
    </table>
            
   
    <fieldset style="border: solid; border-width: thin; height: auto; border-color: #a8a8a8;"
        class="ml-2 mr-2">

        <legend id="Legend5" visible="true" style="width: auto; font-size: 16px; font-weight: bold;color: #1f497d;">Switch Retail Staff</legend>

        <table style="margin-left:10px;font-size: medium; padding-bottom: 0px; width: 100%;">
           

            <tr class="spaceUnder">
                <td class="field" style="width: 8% !important; text-align: left !important; padding-right: 10px !important;">
                    <b>Zone</b>
                </td>
                                <td style="width: 12%; text-align: left;">

                    <asp:DropDownList ID="ddl_zoneId" runat="server" Width="200px" Height="30px" OnSelectedIndexChanged="ddl_zoneId_SelectedIndexChanged"
                        AutoPostBack="true">
                    </asp:DropDownList>
                </td>
                <td style="width: 8% !important; text-align: left !important; padding-right: 10px !important;">
                    <b>Branch </b>
                </td>
                                
                <td style="width: 12%; text-align: left;">
                    <asp:DropDownList ID="dd_Branch" runat="server" Width="180px" Height="30px" OnSelectedIndexChanged="ddl_branchId_SelectedIndexChanged"
                        AutoPostBack="true">
                    </asp:DropDownList>
                </td>
                 <td style="width: 8% !important; text-align: left !important; padding-right: 10px !important;">
                    <b>Express Center </b>
                </td>
                <td style="width: 12%; text-align: left;">
                  <asp:DropDownList ID="dd_eclist" runat="server" Width="180px" Height="30px"
                        AutoPostBack="true">
                    </asp:DropDownList>
                </td>

            </tr>
           <tr class="spaceUnder">
                 <td style="width: 8% !important; text-align: left !important; padding-right: 10px !important;">
                    <b>Username </b>
                </td>
                 <td style="width: 12%; text-align: left;">
                     <asp:DropDownList ID="UsernameDropdown" runat="server" Width="200px" Height="30px" OnTextChanged="ddl_Username_SelectedIndexChanged" 
                        AutoPostBack="true">
                    </asp:DropDownList>
                </td>
               <td style="width: 8% !important; text-align: left !important; padding-right: 10px !important;">
                    <b>Employee Code </b>
                </td>
                <td style="width: 12%; text-align: left;">
                    <asp:TextBox ID="EmpCodeTxtBox" runat="server" MaxLength="100" Width="180px"> </asp:TextBox>
                </td>
                
               
                 <td style="width: 8% !important; text-align: left !important; padding-right: 10px !important;">
                    <b>Shift</b>
                </td>
                <td style="width: 12%; text-align: left;" >
                    <asp:DropDownList runat="server" ID="shiftDropdown">
                        <asp:ListItem Selected="True" Text="A" Value="A"></asp:ListItem>
                        <asp:ListItem Text="B" Value="B"></asp:ListItem>
                        <asp:ListItem Text="C" Value="C"></asp:ListItem>
                    </asp:DropDownList> 
                </td>
            </tr>
            
            <tr class="spaceUnder">
                
              
            </tr>
            
            
            <tr class="spaceUnder">
                <td>

                </td>
                <td>

                </td>
                 <td  style="width:8%;padding-top:10px">
                    <asp:Button ID="btn_save" runat="server" Text="Save" CssClass="button" OnClick="btn_Save_Staff_Click" onclientclick="return confirm('Are you sure you want to add this employee?');"
                         Width="95px" />
                </td>
                <td style="width:12%;padding-top:10px">
                    <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" CssClass="button" UseSubmitBehavior="false"
                        OnClick="btn_Cancel_Click" Width="95px" />
                </td>
                <td>

                </td>

            </tr>
           
        </table>
    </fieldset>
    <br />

      <%--<asp:GridView ID="RetailStaffGrid" runat="server" AutoGenerateColumns="false"  
                    CssClass="mGrid" AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None" 
                    AllowPaging="false" BorderWidth="1px"  OnPageIndexChanging="GridView2_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField HeaderText="S.No." ItemStyle-Width="4%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />

                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField ItemStyle-Width="8%" HeaderStyle-Width="10px" HeaderText="Zone" DataField="Zone" ItemStyle-HorizontalAlign="Center"
                            ></asp:BoundField>
                        <asp:BoundField ItemStyle-Width="8%" HeaderStyle-Width="100px" HeaderText="Branch" DataField="Branch" ItemStyle-HorizontalAlign="Center"
                            ></asp:BoundField>
                        <asp:BoundField HeaderStyle-Width="100px" HeaderText="Username" DataField="employeeUsername" ItemStyle-HorizontalAlign="Center"
                            ItemStyle-Width="15%" ></asp:BoundField>
                        <asp:BoundField  HeaderStyle-Width="100px" HeaderText="Employee Code" DataField="UserId" ItemStyle-HorizontalAlign="Center"
                            ItemStyle-Width="10%"></asp:BoundField>
                            
                        <asp:BoundField  HeaderStyle-Width="100px" HeaderText="Booking Code" DataField="BookingStaff" ItemStyle-HorizontalAlign="Center"
                            ItemStyle-Width="10%"></asp:BoundField>
                        <asp:BoundField  HeaderStyle-Width="100px" HeaderText="Express Center" DataField="ExpressCenterCode" ItemStyle-HorizontalAlign="Center"
                            ItemStyle-Width="10%"></asp:BoundField>
                       
                   
                    </Columns>
                </asp:GridView>--%>

    <div style="width: auto; height: 480px; overflow: scroll">   
        <asp:GridView ID="RetailStaffGrid" runat="server" AutoGenerateColumns="False" CellPadding="6" 
            CssClass="mGrid" AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None"
OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating">  
            <Columns>  
                <asp:TemplateField HeaderText="S.No." ItemStyle-Width="4%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />

                            </ItemTemplate>
                        </asp:TemplateField>
                <asp:TemplateField HeaderText="Zone" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="11%" >  
                    <ItemTemplate>  
                        <asp:Label ID="lbl_Zone" runat="server" Text='<%#Eval("Zone") %>'></asp:Label>  
                    </ItemTemplate>  
                    <EditItemTemplate> 
                        <asp:DropDownList ID="Grid_Zone_Dropdown" runat="server" AutoPostBack="true" Visible="true"  OnSelectedIndexChanged="ddl_zone_grid_IndexChange"  >
                </asp:DropDownList>

                    </EditItemTemplate>  
                </asp:TemplateField>  
                <asp:TemplateField HeaderText="Branch" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="13%">  
                    <ItemTemplate>  
                        <asp:Label ID="lbl_Branch" runat="server" Text='<%#Eval("Branch") %>'></asp:Label>  
                    </ItemTemplate>  
                    <EditItemTemplate>  
                         <asp:DropDownList ID="Grid_Branch_Dropdown" runat="server" AutoPostBack="true" Visible="true"  OnSelectedIndexChanged="ddl_branch_grid_IndexChange"  >
                </asp:DropDownList>
                    </EditItemTemplate>  
                </asp:TemplateField>  
               <%-- <asp:TemplateField HeaderText="Booking staff" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="12%">  
                    <ItemTemplate>  
                        <asp:Label ID="lbl_BookingStaff" runat="server" Text='<%#Eval("BookingStaff") %>'></asp:Label>  
                    </ItemTemplate>  
                    <EditItemTemplate>  
                        <asp:TextBox ID="txt_BookingStaff" runat="server" Height="20px" MaxLength="20" Text='<%#Eval("BookingStaff") %>'></asp:TextBox>  
                    </EditItemTemplate>  
                </asp:TemplateField> --%>

                <asp:BoundField  HeaderStyle-Width="100px" ReadOnly="True" HeaderText="Employee Code" DataField="UserId" ItemStyle-HorizontalAlign="Center"
                            ItemStyle-Width="12%"></asp:BoundField>
                <asp:BoundField HeaderStyle-Width="100px" ReadOnly="True" HeaderText="Username" DataField="employeeUsername" ItemStyle-HorizontalAlign="Center"
                            ItemStyle-Width="18%" ></asp:BoundField>

                 <asp:TemplateField HeaderText="Express Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="28%">  
                    <ItemTemplate>  
                        <asp:Label ID="lbl_ExpressCenterCode" runat="server" Text='<%#Eval("ExpressCenterCode") %>'></asp:Label>  
                    </ItemTemplate>  
                    <EditItemTemplate>  
                        <asp:DropDownList ID="Grid_EC_Dropdown" runat="server" AutoPostBack="true" Visible="true"  >
                </asp:DropDownList> 
                    </EditItemTemplate>  
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Shift" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="28%">  
                    <ItemTemplate>  
                        <asp:Label ID="lbl_shift" runat="server" Text='<%#Eval("shift") %>'></asp:Label>  
                    </ItemTemplate>  
                    <EditItemTemplate>  
                        <asp:DropDownList ID="Grid_shift_Dropdown" runat="server" AutoPostBack="true" Visible="true"  >
                            <asp:ListItem Selected="True" Text="A" Value="A"></asp:ListItem>
                            <asp:ListItem Text="B" Value="B"></asp:ListItem>
                            <asp:ListItem Text="C" Value="C"></asp:ListItem>
                        </asp:DropDownList> 
                    </EditItemTemplate>  
                </asp:TemplateField> 
                <asp:TemplateField>  
                    <ItemTemplate>  
                        <asp:Button ID="btn_Edit" runat="server" Text="Edit"  CssClass="button"  CommandName="Edit" Width="120px" ItemStyle-HorizontalAlign="Center"/>  
                    </ItemTemplate>  
                    <EditItemTemplate>  
                        <asp:Button ID="btn_Update" runat="server" Text="Update"  CssClass="button"  Width="70px" onclientclick="return confirm('Are you sure you want to update this employee?');"  CommandName="Update"/>  
                        <asp:Button ID="btn_Cancel" runat="server" Text="Cancel"  CssClass="button"   Width="70px" CommandName="Cancel"/>  
                    </EditItemTemplate>  
                </asp:TemplateField>  
            </Columns>  
            <%--<HeaderStyle BackColor="#663300" ForeColor="#ffffff"/>  
            <RowStyle BackColor="#e7ceb6"/>  --%>
        </asp:GridView>  
      
    </div>  
    </asp:Content>
