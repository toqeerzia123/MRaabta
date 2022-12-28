<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="CourierOutSecurityCheck.aspx.cs" Inherits="MRaabta.Files.CourierOutSecurityCheck" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <style>

.print
{
    background: #f27031 none repeat scroll 0 0;
    color: #fff;
    margin: 0 10px;
    padding: 5px;
    text-decoration: none;
    border-radius: 5px;
}

.print:hover
{
    text-shadow: 1px 1px 1px #000;
}

.mGrid td {
    padding: 8px 5px;
}
 .GreenLabel
        {
            font-size: 12px;
            padding: 5px;
            width: 120px;
            float: left;
    margin: 0 10px 10px 0;
    height: 30px;
        }
.consignment-success
{
    background: #00A651;
    color: #fff;
    
}
 
.consignment-cod
{
    background: #C3BDBC;
    color: black;
    
}

.consignment-danger
{
    background: #CC2424;
    color: #fff;
    margin: 0 10px 10px 0;
}

.consignment-remain
{
    background: #5B9BD5;
    color: #fff;
    margin: 0 10px 10px 0;
}
th,td{ 
width: 170px; 


}
.tdd{
    
}

</style>
        <asp:Panel ID="body1" runat="server">
            <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
                <tr>
                    <td colspan="12" align="center" class="head_column">
                       <h3>Courier Out Security Check</h3>
                    </td>
                </tr>            
            </table> 
            <div>
            <asp:Label ID="Errorid" runat="server" style="color: #CC2424; font-size:15px; font-weight:700" Visible="false"></asp:Label>
                
            </div>
            <table class="input-form" style="width:90%"> 
                <tr>                                      
                    <td class="field">Runsheet Date:</td>
                    <td class="input-field"  Width="100px" >
                       <div id="txtDate_" runat="server">
                            <asp:label ID="dd_start_date" runat="server"  MaxLength="10"
                                Width="120px" AutoPostBack="false"></asp:label>
                            <%--<asp:ImageButton ID="Popup_Button" runat="server" ImageUrl="~/images/Calender50px.png"
                                Width="20px" />
                            <Ajax1:CalendarExtender ID="CalenderStart" TargetControlID="dd_start_date" runat="server"
                                Format="yyyy-MM-dd" PopupButtonID="Popup_Button">
                            </Ajax1:CalendarExtender>--%>
                       </div>                  
                    </td>                    
                    <td class="field">SDO Code:</td>
                    <td class="input-field" Width="100px"  >
                        <asp:TextBox ID="dd_RunSheetNo" runat="server" CssClass="med-field"  Width="175px"></asp:TextBox>                            
                    </td>
                    
                    <td class="field">
                        <asp:Button ID="submit" runat="server" Text="Search" CssClass="button1" OnClick="submit_Click" /><%-- OnClick="Btn_Save_Click"--%>
                    </td>
                   
                </tr>
               <tr>
                    <td class="field">Runsheet No Scan:</td>
                    <td class="field">
                        <asp:TextBox ID="dd_R_NO" runat="server" CssClass="med-field" AutoPostBack="true" OnTextChanged="dd_R_NO_TextChanged" ></asp:TextBox>                            
                    </td>
                    <td  class="field tdd" style="height:10px;"></td>
                   <td class="field tdd" id="lbl_sdo" runat="server" visible="false">SDO Name:</td>
                   <td>
                       <asp:Label runat="server" ID="lbl_sdoname" Visible="false" ></asp:Label>
                   </td>
                </tr>
                <tr>
                    <td class="field">ConsignmentNo Scan:</td>
                    <td class="field">
                        <asp:TextBox ID="dd_CN_No" runat="server" CssClass="med-field" AutoPostBack="true" OnTextChanged="dd_CN_No_TextChanged" ></asp:TextBox>  
                     
                    </td>
                    <td  class="field tdd" style="height:10px;"></td>
                    <td class="field tdd" id="lbl_branch" runat="server" visible="false">Branch:</td>
                   <td>
                       <asp:Label runat="server" ID="lbl_branchname" Visible="false" ></asp:Label>
                   </td>
                </tr>
               
            </table>
             <asp:Panel ID="innerbody" runat="server">
             <div style="float:none;width:70%;overflow:hidden;">
                 <%--<asp:Label id="lbl_msg01" style="color: #00A651; font-size:15px; font-weight:700" class="ml-5" runat="server" Visible="true"> </asp:Label>--%>
                 <h4 style="padding: 3px; margin: 0 15px;">Last Scan</h4>
                 <asp:Panel ID="pnl_lbl" runat="server"  Style="border: 1px solid #000;
                        width: 145px; padding: 10px; margin: 0 15px;  height: 50px;  float:left" >
                    </asp:Panel>
                 <asp:Button ID="btnProceed" runat="server" Text="Save" CssClass="button1" OnClick="btnProceed_Click" style="float: right; margin-left: 2em" /><%-- OnClick="Btn_Save_Click"--%>
                 <asp:Button ID="btnrefresh2" runat="server" Text="Refresh" CssClass="button1" OnClick="btn_refresh_Click" style="float: right; "  Visible="false" Enabled="false"/>
                 
             </div>
            <div style="float:none;width:85%;overflow:hidden;">
                <span id="Table_1"  class="tbl-large"> 
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" DataKeyNames="RUNSHEETNUMBER"
                    CssClass="mGrid" AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None"
                    AllowPaging="false" BorderWidth="1px" style="margin: 0 0 0 10px;" OnRowDataBound="GridView_RowDataBound">
                    <Columns>
                    <asp:TemplateField ShowHeader="true">  
                        <ItemTemplate>  
                        <asp:CheckBox ID="chkid" runat="server"  AutoPostBack="true"  OnCheckedChanged="chkid_CheckedChanged"/>  
                        </ItemTemplate>  
                     </asp:TemplateField>  
                    <asp:TemplateField HeaderText = "S.No." ItemStyle-Width="2%">
                             <ItemTemplate>
                                 <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                             </ItemTemplate>
                         </asp:TemplateField>
                        <asp:BoundField HeaderText="RUNSHEET No#" DataField="RUNSHEETNUMBER" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>  
                        <asp:BoundField HeaderText="RUNSHEET Date" DataField="RUNSHEETDATE" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>
                        <asp:BoundField HeaderText="RUNSHEET Type" DataField="RunsheetType" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>                          
                        <asp:BoundField HeaderText="ROUTE Code" DataField="ROUTECODE" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>  
                        <asp:BoundField HeaderText="Route Name" DataField="RouteName" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>  
                        <asp:BoundField HeaderText="Route Type" DataField="RouteType" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>  
                        <asp:BoundField HeaderText="CN Count" DataField="CNCOUNT" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>  
                        <asp:BoundField HeaderText="COD CN Count" DataField="CODCNCOUNT" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>  
                        <asp:BoundField HeaderText="ColorLabel" DataField="ColorLabel" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" Visible="false"></asp:BoundField>  
                        <%--<asp:TemplateField HeaderText="Remarks">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txt_comments" AutoPostBack="true" CssClass="med-field" OnTextChanged="txt_comments_TextChanged" Text='<%#Eval("Remarks") %>' ></asp:TextBox>
                            </ItemTemplate>
                             <ControlStyle Width="300px"></ControlStyle>
                         </asp:TemplateField>--%>

                    </Columns>
                                                      
                </asp:GridView>	
                </span>
            </div>
                 <asp:Panel ID="child_head" runat="server">
                     <div>
                         <h2 id="con_head" style="width: 500px;float: left;margin: 0 45px;">COD Consignments</br></h2>
                     </div>
                     <div>
                          <h2 id="con_head2" style="width: 500px;float: left;">NON COD Consignments</br></h2>
                     </div>
            
            
        </asp:Panel>
                <asp:Panel runat="server" >
                    <asp:Panel ID="child_panel2" runat="server"  Style="border: 1px solid #000;
                        width: 500px; padding: 10px; margin: 0 15px;  height: 300px; overflow:scroll; float:left" >
                    </asp:Panel>
                    <asp:Panel ID="child_panel" runat="server"  Style="border: 1px solid #000;
                        width: 500px; padding: 10px; margin: 0 15px;  height: 300px; overflow:scroll; float:left">
                    </asp:Panel>
                    
                </asp:Panel>
            </asp:Panel>
     </asp:Panel>
    <asp:Panel ID="body2" runat="server">
        <div style="float:none;width:70%;overflow:hidden;">
                 <asp:Label id="lbl_msg" style="color: #00A651; font-size:15px; font-weight:700" class="ml-5" runat="server" Visible="false"> </asp:Label>
                 <asp:Label id="lbl_time" style="color: #00A651; font-size:15px; font-weight:700" runat="server"></asp:Label>
                 <asp:Button ID="btn_refresh" runat="server" Text="Refresh" CssClass="button1" OnClick="btn_refresh_Click" style="float: right;" /><%-- OnClick="Btn_Save_Click"--%>
                 <asp:Button ID="btn_print" runat="server" Text="Print" CssClass="button1" OnClientClick="print()"  style="float: right; margin-right: 10px;" />
                
             </div>
       <%--   <div style="float:none;width:85%;overflow:hidden;">
               <div>
                         <h2 id="con_head" style="width: 500px;float: left;margin: 0 45px;">Security Check Summary</br></h2>
                </div>
                <span id="Table_1"  class="tbl-large"> 
                <asp:GridView ID="gvw_data" runat="server" AutoGenerateColumns="false" DataKeyNames="SecurityScanId"
                    CssClass="mGrid" AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None"
                    AllowPaging="false" BorderWidth="1px" style="margin: 0 0 0 10px;">
                    <Columns>
                  
                        <asp:BoundField HeaderText="Log ID" DataField="SecurityScanId" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>  
                        <asp:BoundField HeaderText="Total Scanned Runsheet" DataField="TotalScannedRunsheet" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>
                        <asp:BoundField HeaderText="COD Count" DataField="CODCount" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>                          
                        <asp:BoundField HeaderText="DOM Count" DataField="DomesticCount" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>  
                        <asp:BoundField HeaderText="Missing Count" DataField="MissingRunsheet" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>  
                    </Columns>
                                                      
                </asp:GridView>	
                </span>
            </div>   --%>
        <div style="float:none;overflow:hidden;">
               <div>
                         <h2 id="con_head" style="width: 500px;float: left;margin: 0 45px;">Runsheet Summary</br></h2>
                </div>
                <span id="Table_1"  class="tbl-large"> 
                <asp:GridView ID="gvw_runsheet" runat="server" AutoGenerateColumns="false" DataKeyNames="RUNSHEETNUMBER"
                    CssClass="mGrid" AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None"
                    AllowPaging="false" BorderWidth="1px" style="margin: 0 0 0 10px;">
                    <Columns>
                  
                        <asp:BoundField HeaderText="Runsheet Number" DataField="RUNSHEETNUMBER" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>  
                        <asp:BoundField HeaderText="Runsheet Date" DataField="RUNSHEETDATE" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" ></asp:BoundField>
                        <asp:BoundField HeaderText="Runsheet Type" DataField="RunsheetType" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>                          
                        <asp:BoundField HeaderText="Route Code" DataField="ROUTECODE" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>  
                        <asp:BoundField HeaderText="Route Name" DataField="RouteName" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField> 
                        <asp:BoundField HeaderText="Route Type" DataField="RouteType" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>  
                        <asp:BoundField HeaderText="DOM Count" DataField="CNCOUNT" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>  
                        <asp:BoundField HeaderText="COD Count" DataField="CODCNCOUNT" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>  
                        <asp:BoundField HeaderText="Security Checked" DataField="SecurityChecked" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>  
                    </Columns>
                                                      
                </asp:GridView>	
                </span>
            </div>   
        <div style="float:none;overflow:hidden;">
               <div>
                         <h2 id="con_head" style="width: 500px;float: left;margin: 0 45px;">Security Check Summary</br></h2>
                </div>
                <span id="Table_1"  class="tbl-large"> 
                <asp:GridView ID="gvw_security" runat="server" AutoGenerateColumns="false" DataKeyNames="SecurityScanId"
                    CssClass="mGrid" AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None"
                    AllowPaging="false" BorderWidth="1px" style="margin: 0 0 0 10px; width: 60%">
                    <Columns>
                  
                        <asp:BoundField HeaderText="Log ID" DataField="SecurityScanId" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>  
                        <asp:BoundField HeaderText="Runsheet Number" DataField="RunSheetNumber" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>
                        <asp:BoundField HeaderText="Runsheet Date" DataField="RunsheetDate" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>                          
                        <asp:BoundField HeaderText="Route Code" DataField="RouteCode" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>  
                        <asp:BoundField HeaderText="SDO Code" DataField="sdocode" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField> 
                        <asp:BoundField HeaderText="Total DOM" DataField="TotalDOMCount" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>  
                        <asp:BoundField HeaderText="Total COD" DataField="TotalCodCount" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>  
                        <asp:BoundField HeaderText="Scanned DOM" DataField="ScannedDOM" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>  
                        <asp:BoundField HeaderText="Scanned COD" DataField="ScannedCOD" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>  
                        <asp:BoundField HeaderText="Remaining DOM" DataField="RemainingDOM" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>  
                        <asp:BoundField HeaderText="Remaining COD" DataField="RemainingCOD" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>  
                    </Columns>
                                                      
                </asp:GridView>	
                </span>
            </div> 
            <div>
                <div>
                         <h2 id="con_head" style="width: 500px;float: left;margin: 0 45px;">Shipments Missing on Runsheet</br></h2>
                </div>
                <asp:Panel ID="pnl_miss" runat="server"  Style="border: 1px solid #000;
                        width: 80%; padding: 10px; margin: 0 15px;  height: 250px; overflow:scroll; float:left" >
                    </asp:Panel>
            </div>
         <div>
                <div>
                         <h2 id="con_head_s" style="width: 500px;float: left;margin: 0 45px; padding-top: 20px;">COD/Handcarry Shipments not security checked</br></h2>
                </div>
                <asp:Panel ID="pnl_remain" runat="server"  Style="border: 1px solid #000;
                        width: 80%; padding: 10px; margin: 0 15px;  height: 250px; overflow:scroll; float:left" >
                    </asp:Panel>
            </div>
    </asp:Panel>
    <script  type="text/javascript">
        function print()
        {
            debugger;
            var mywindow = window.open("");

            mywindow.document.write('<html><head><title style=" text-align: center;">' + 'Courier Out Security Check' + '</title>');
            mywindow.document.write('</head><body style="font-family: "Times New Roman", Times, serif; font-size: 14px; width: 80%;"><div align="center">');
            mywindow.document.write('<h3 style="text-align: center">' + 'Courier Out Security Check' + '</h3>');
            
            mywindow.document.write('<div align="center"><table style = "padding: 10px; border: 1px solid black; font-size: 13px;" ><tr><td width="170px">' + 'Runsheet Date' + '</td>');
            mywindow.document.write('<td width="170px">' + 'SDO Code – SDO Name' + '</td>');
            mywindow.document.write('<td width="170px">' + 'Time Out' + '</td></tr>');
            mywindow.document.write('<tr><td width="170px">' + document.getElementById('<%= dd_start_date.ClientID %>').outerHTML + '</td>');
            mywindow.document.write('<td width="170px">' + document.getElementById('<%= lbl_sdoname.ClientID %>').outerHTML + '</td>');
            mywindow.document.write('<td width="170px">' + document.getElementById('<%= lbl_time.ClientID %>').outerText + '</tr></td></table></div>');
            mywindow.document.write('<div align="center" style="margin-top:25px">'+'Runsheets Summary'+'</div>');
            mywindow.document.write('<div>' + document.getElementById('<%= gvw_runsheet.ClientID %>').outerHTML + '</div>');

            mywindow.document.write('<div style="margin-top:25px"><div align="center">' + 'Security Check Summary' + '</div>');
            mywindow.document.write('<div>' + document.getElementById('<%= gvw_security.ClientID %>').outerHTML + '</div>');
            debugger;
            
            var divToPrint1 = document.getElementsByClassName('consignment-danger');
            var i = 0;
            if (divToPrint1.length != 0) {
                mywindow.document.write('</br><div align="center" style="margin-top:25px">' + 'Shipments missing on runsheet' + '</div>');
                mywindow.document.write('<div style="border: 1px solid #000; width: 80 %; padding: 10px; margin: 0 15px;  float: left">');
                for (i = 0; i < divToPrint1.length; i++) {
                    mywindow.document.write('<label style="font-size: 12px;padding: 5px;width: 120px;float: left; margin: 0 10px 10px 0;height: 20px; background: #CC2424; color: #000000	; margin: 0 10px 10px 0;">' + divToPrint1[i].outerText + '</label>');

                }
                mywindow.document.write('</div></div></br>');


            }
            
            var j = 0;
            var divToPrint2 = document.getElementsByClassName('consignment-remain');
            if (divToPrint2.length != 0) {
                mywindow.document.write('<div style="margin-top:50px">' + 'COD/Handcarry Shipments not security checked' + '</div>');
                mywindow.document.write('<div style="border: 1px solid #000; width: 80 %; padding: 10px; margin: 0 15px;  float: left">');
                for (j = 0; j < divToPrint2.length; j++) {
                    mywindow.document.write('<label style="font-size: 12px;padding: 5px;width: 120px;float: left; margin: 0 10px 10px 0;height: 20px; background: #5B9BD5; color: #000000	; margin: 0 10px 10px 0;">' + divToPrint2[j].outerText + '</label>');

                }
                mywindow.document.write('</div>');

            }
            
            mywindow.document.write('</div></body></html>');
            mywindow.print();
            <%--var divToPrint = document.getElementById('<%= GridView1.ClientID %>');
            debugger;
            var divToPrint1 = document.getElementById('<%= child_panel2.ClientID %>');
            newWin = window.open("");
            newWin.document.write("<br/>", divToPrint.outerHTML, "<br/>" + divToPrint1.outerText);
            newWin.print();--%>
            //newWin.close();

        }
    </script>

  

</asp:Content>



