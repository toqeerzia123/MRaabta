<%@ Page Title="" Language="C#" MasterPageFile="~/BtsMasterPage.Master" AutoEventWireup="true" CodeBehind="Zonewise_Timestamping.aspx.cs" Inherits="MRaabta.Files.Zonewise_Timestamping" %>

 <%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css"> 
        @media print
        {
          #noprintdiv
          {
              display: none !important;
              
          }  
          
          
          
            
        }
        
    
    </style>
</asp:Content>



<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<style>

.form-group label {
    float: left;
    font-weight: bold;
    width: 10%;
}

.col-md-6 {
    padding: 0 0 15px;
    width: 45%;
}
            

</style>
<table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
                <tr>
                    <td colspan="12" align="center" class="head_column">
                       <h3> Zone Wise Time Stamping</h3>
                    </td>
                </tr>            
            </table>

    <div id="Reportperameter" runat="server">
        <div class="box-content input-form">
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group input-group">
                        <label class="input-group-addon control-label">
                            From:</label>
                        <%--<asp:TextBox runat="server" ID="txt_date_from" CssClass="form-control datepicker" />--%>

                        <asp:TextBox ID="txt_date_from" runat="server" CssClass="med-field" MaxLength="10"></asp:TextBox>
                    <Ajax1:CalendarExtender ID="CalendarExtender3" TargetControlID="txt_date_from" runat="server"
                        Format="dd-MM-yyyy" PopupButtonID="Image1"> 
                    </Ajax1:CalendarExtender>   
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group input-group">
                        <label class="input-group-addon control-label">
                            To:</label>
                       <%--<asp:TextBox runat="server" ID="txt_date_to" CssClass="form-control datepicker" />--%>


                        <asp:TextBox ID="txt_date_to" runat="server" CssClass="med-field" MaxLength="10"></asp:TextBox>
                    <Ajax1:CalendarExtender ID="CalendarExtender4" TargetControlID="txt_date_to" runat="server"
                        Format="dd-MM-yyyy" PopupButtonID="Image1"> 
                    </Ajax1:CalendarExtender>   
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group input-group">
                        <label class="input-group-addon control-label">
                            Zone:</label>
                        <asp:DropDownList ID="dpd_zone" runat="server" CssClass="form-control" AutoPostBack="true" onselectedindexchanged="dpd_branch_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group input-group">
                        <label class="input-group-addon control-label">
                            Branch:</label>
                        <asp:DropDownList ID="dpd_branch" runat="server" CssClass="form-control" 
                            >
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <asp:Label Text="" runat="server" ID="lbl_error" Visible="false" />
            <asp:Button ID="btn_view" Text="View Report" runat="server"  CssClass="button1" onclick="btn_view_Click" />
        </div>
    </div>
    <div id="ViewReport" runat="server" visible="false">

        
    
        <table class="left_tbl top" width="100%" >
            <tr>
                <td colspan="5" align="center">
                    <b>Individual Time Sheet</b>
                </td>
            </tr>
            <tr>
                <td colspan="5" align="left">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td width="90px">
                    <b>From:</b>
                </td>
                <td width="90px;">
                    <asp:Label ID="lbl_from" runat="server" />
                </td>
                <td width="110px">
                    <b>To:</b>
                </td>
                <td width="110px">
                    <asp:Label ID="lbl_To" runat="server" />
                </td>
            </tr>
            <tr>
                <td width="90px">
                    <b>Employee No:</b>
                </td>
                <td width="90px">
                    <asp:Label ID="lbl_empno" runat="server" />
                </td>
                <td width="110px">
                    <b>Empyloyee Name:</b>
                </td>
                <td width="110px">
                    <asp:Label ID="lbl_empname" runat="server" />
                </td>
            </tr>

            <tr>
                <td width="90px">
                    <b>Zone:</b>
                </td>
                <td width="90px">
                    <asp:Label ID="lbl_zone" runat="server" />
                </td>
                <td width="110px">
                    <b>Branch:</b>
                </td>
                <td width="110px">
                    <asp:Label ID="lbl_branch" runat="server" />
                </td>
            </tr>
        </table>

        <table class="right_tbl top">
            <tr>
                <td>
                    <fieldset style="border:0;">
                        <table>
                            <tr>
                                <td colspan="2" align="center" style="padding: 2px;">
                                    <b>Name</b>
                                </td>
                            </tr>
                            <tr>
                                <td width="120px" style="padding: 2px;">
                                    <b>Department</b>
                                </td>
                                <td width="120px" style="padding: 2px; text-align: left;">
                                    <asp:Label ID="lbl_department" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td width="120px" style="padding: 2px;">
                                    <b>DESIGNATION</b>
                                </td>
                                <td width="120px" style="padding: 2px; text-align: left;">
                                    <asp:Label ID="lbl_designation" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td width="120px" style="padding: 2px;">
                                    <b>EMPLOYEE TYPE</b>
                                </td>
                                <td width="120px" style="padding: 2px; text-align: left;">
                                    <asp:Label ID="lbl_employee_type" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
            
            
        </table>
        <br />

        <span class="tbl-large">
            <div>
            <%--    <table width="100%" class="mGrid">
            <thead>
                <tr>
                    <th>
                        <b>AttendanceDate</b>
                    </th>
                    <th>
                        <b>Shift</b>
                    </th>
                    <th>
                        <b>DateIn</b>
                    </th>
                    <th>
                        <b>TimeIn</b>
                    </th>
                    <th>
                        <b>DateOut</b>
                    </th>
                    <th>
                        <b>TimeOut</b>
                    </th>
                    <th>
                        <b>WorkHrs</b>
                    </th>
                    <th>
                        <b>Remarks</b>
                    </th>
                </tr>
            </thead>--%>
                    <asp:GridView runat="server" ID="rpt_time" /> 
            <%--<asp:Repeater runat="server" ID="rpt_time">
                <ItemTemplate>
                    <tr>
                        <td>
                            <%# Eval("DATE") %>
                        </td>
                        <td>
                            <%# Eval("Shift") %>
                        </td>
                        <td>
                            <%# Eval("DateIn") %>
                        </td>
                        <td>
                            <%# Eval("LastIn") %>
                        </td>
                        <td>
                            <%# Eval("DateOut") %>
                        </td>
                        <td>
                            <%# Eval("Firstout") %>
                        </td>
                        <td>
                            <%# Eval("WorkHrs")%>
                        </td>
                        <td>
                            <%# Eval("Remarks")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>--%>
            </div>
        </span>

        <br />
        <table class="tbl_footer">
            <thead>
                <tr>
                    <td align="center" class="bold">
                        Physical
                    </td>
                    <td align="center" class="bold">
                        Absent
                    </td>
                    <%--<td align="center">
                        Working Hrs
                    </td>--%>
                    <td align="center" class="bold">
                        Missing Out
                    </td>
                    <td align="center" class="bold">
                        Payables Days
                    </td>
                </tr>
                <tr style="font-size: 14px;">
                    <td align="center">
                        <asp:Label Text="" ID="lbl_Physical" runat="server" />
                    </td>
                    <td align="center">
                        <asp:Label Text="" ID="lbl_Absent" runat="server" />
                    </td>
                    <%--<td align="center">
                        <asp:Label Text="" ID="lbl_WorkingHrs" runat="server" />
                    </td>--%>
                    <td align="center">
                        <asp:Label Text="" ID="lbl_missing" runat="server" />
                    </td>
                    <td align="center">
                        <asp:Label Text="" ID="lbl_PayDays" runat="server" />
                    </td>
                </tr>
            </thead>
        </table>
        <div style="float: right; position: relative; right: 4%; margin: 50px 0px 28px;">
            <table>
                <tr>
                    <td>
                        ________________
                    </td>
                </tr>
                <tr>
                    <td>
                        Signature Authority
                    </td>
                </tr>
            </table>
        </div>
        <div style="clear: both;">
        </div>
    </div>


</asp:Content>

