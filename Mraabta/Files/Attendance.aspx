<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Attendance.aspx.cs" Inherits="MRaabta.Files.Attendance" MasterPageFile="~/BtsMasterPage.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../Bts_Css/Mazen.css" rel="stylesheet" />
      
    <link href="../mazen/css/sweetalert.css" rel="stylesheet" />
    <script src="../mazen/js/sweetalert-dev.js"></script>
     <script type="text/javascript">

         function ShowCal() {

            //swal('saasdsad');
         }
     
     </script>
      <style>
         #overlay {
    position: fixed;
    z-index: 99;
    top: 0px;
    left: 0px;
    background-color: #f8f8f8;
    width: 100%;
    height: 100%;
    filter: Alpha(Opacity=90);
    opacity: 0.9;
    -moz-opacity: 0.9;
}            
#theprogress {
    background-color: #fff;
    border:1px solid #ccc;
    padding:10px;
    width: 300px;
    
    line-height:30px;
    text-align: center;
    filter: Alpha(Opacity=100);
    opacity: 1;
    -moz-opacity: 1;
}
#modalprogress {
    position: absolute;
    top: 40%;
    left: 50%;
    margin: -11px 0 0 -150px;
    color: #990000;
    font-weight:bold;
    font-size:14px;
}




    </style>
     <asp:UpdateProgress ID="prgLoadingStatus" runat="server" DynamicLayout="true">
    <ProgressTemplate>
        <div id="overlay">
            <div id="modalprogress">
                <div id="theprogress">
                    <asp:Image ID="imgWaitIcon" runat="server" ImageAlign="AbsMiddle" ImageUrl="../mazen/images/wait.gif" />
                    Please wait...
                </div>
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>  
    <asp:UpdatePanel runat="server" ID="panel1">
        
        <ContentTemplate >
    <div class="row main-body newPanel">
        <fieldset class="fieldsetSmall">
            <span id="ContentPlaceHolder1_Errorid" style="color: Red; font-weight: bold;"></span>
            <legend style="font-size: medium;"><b>Attendance</b></legend>
           
            <div class="col-lg-12 col-md-12 col-sm-12">
                <table style="width: auto;">
                    <tr>
                        <td style="width: 50px">
                            <b>Search by Rider code</b>
                            <%--<b>Rider</b>--%>
                        </td>
                        <td style="width: 100px">
                            
                        </td>
                        <td style="width: 50px">
                            
                            <b>Date</b>
                           <%-- <b>Status</b>--%>
                        </td>
                    
                    </tr>
                    <tr>

                        <td style="width: 170px">
                            <asp:TextBox ID="txtSearchRider" MaxLength="20" runat="server" style=" margin-top: 0px; " />
                            <%--<asp:DropDownList ID="ddlRiderCode" OnSelectedIndexChanged="ddlRiderCode_SelectedIndexChanged" AutoPostBack="true"  CssClass="form-control" runat="server" Style="width: 250px;">
                            </asp:DropDownList>--%>

                        </td>

                        <td style="width: 170px">
                            <asp:Button ID="btnSearch" CssClass="btn btn-success" runat="server" OnClick="btnSearch_Click" OnClientClick="" Text="Search" style="margin-bottom: 10px;" />
                        </td>
                        <td style="width: 170px">
                                                        <asp:TextBox ID="txtDate" Enabled="false" runat="server" TextMode="Date" Style="width: 225px; margin-bottom: 10px; height: 33px" />

                           <%-- <asp:DropDownList ID="ddlAttendanceStatusID" Style="width: 250px;" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlAttendanceStatusID_SelectedIndexChanged" runat="server">
                            </asp:DropDownList>--%>

                        </td>

                        <td style="width: 170px" >
                                                        <asp:Button ID="btnGetAllAttendance"    OnClientClick="ShowCal()"  CssClass="btn btn-warning" runat="server" OnClick="btnGetAllAttendance_Click" Text="Get All Attendance" style="margin-bottom: 10px;" />

                            
                            <%--<asp:DropDownList ID="ddlAlternativeRiderCode" Style="width: 250px;" CssClass="form-control" runat="server">
                            </asp:DropDownList>--%>

                        </td>
                      


                    </tr>
                </table>
                
                
            
        <%--</fieldset>
        <fieldset class="fieldsetSmall">
                <span id="ContentPlaceHolder1_Errorid" style="color: Red; font-weight: bold;"></span>
                <legend style="font-size: medium;"><b>Attendance</b></legend>--%>
                <%--<div class="col-lg-12 col-md-12 col-sm-12">
                    <table>
                        <tr>
                            <td style="width: 80px">
                                <b>Search Rider</b>
                            </td>
                        </tr>
                        <tr>
                            
                            <td style="width: 170px">
                                
                            </td>
                            <td>
                                
                            </td>
                        </tr>

                    </table>
                </div>
            <br />--%>
            <div class="col-lg-12 col-md-12 col-sm-12">
                <asp:UpdatePanel ID="grdUp1" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="grdPR" runat="server" AutoGenerateEditButton="True" DataKeyNames="AttendanceID" AutoGenerateColumns="false"
                            AllowPaging="true" OnRowCancelingEdit="grdPR_RowCancelingEdit" OnRowDataBound="grdPR_RowDataBound"
                            OnRowEditing="grdPR_RowEditing"
                            OnRowUpdating="grdPR_RowUpdating"
                            OnPageIndexChanging="OnPageIndexChanging" AutoGenerateDeleteButton="false" PageSize="50"
                            class="table table-bordered table-hover" Font-Size="Medium" CellPadding="4" ForeColor="#333333" GridLines="None" AllowSorting="true" BorderColor="#DA7A4D" Width="100%"
                            BorderStyle="Solid" BorderWidth="5px">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <%-- <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnCancel" runat="server" Text="Remove" CommandName="CmdCancel" CommandArgument='<%#Bind("AttendanceID") %>'>
                            </asp:LinkButton>
                             <asp:LinkButton ID="btnEdit" runat="server" Text="Edit" CommandName="CmdEdit" CommandArgument='<%#Bind("AttendanceID") %>'>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                                <%--<asp:BoundField ItemStyle-Width="150px" DataField="AttendanceID" HeaderText="Attendance ID" />--%>
                                <asp:TemplateField Visible="false" HeaderText="Attendance ID">
                                    <EditItemTemplate>
                                        <asp:Label ID="lblEAttendanceID" runat="server" Text='<%# Bind("AttendanceID") %>'>  
                                        </asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblAttendanceID" runat="server" Text='<%# Bind("AttendanceID") %>'>  
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Attendance Date">
                                    <EditItemTemplate>
                                        <asp:Label ID="lblEAttendanceDate" runat="server" Text='<%# Bind("AttendanceDate","{0:dd/MM/yyyy}") %>'>  
                                        </asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblAttendanceDate" runat="server" Text='<%# Bind("AttendanceDate","{0:dd/MM/yyyy}") %>'>  
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                  <asp:TemplateField HeaderText="Route Code" Visible="true">
                                    <EditItemTemplate>
                                        <asp:Label ID="lblErouteCode" runat="server" Text='<%# Bind("routeCode") %>'>  
                                        </asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblrouteCode" runat="server" Text='<%# Bind("routeCode") %>'>  
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rider Code" Visible="true">
                                    <EditItemTemplate>
                                        <asp:Label ID="lblERiderCode" runat="server" Text='<%# Bind("RiderCode") %>'>  
                                        </asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRiderCode" runat="server" Text='<%# Bind("RiderCode") %>'>  
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rider">
                                    <EditItemTemplate>
                                        <asp:Label ID="lblERider" runat="server" Text='<%# Bind("Rider") %>'>  
                                        </asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRider" runat="server" Text='<%# Bind("Rider") %>'>  
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%--<asp:BoundField ItemStyle-Width="150px" DataField="AttendanceDate" HeaderText="Attendance Date" dataformatstring="{0:d}" />--%>
                                <%--<asp:BoundField ItemStyle-Width="150px" DataField="Rider" HeaderText="Rider" />--%>
                                <%--<asp:BoundField ItemStyle-Width="150px" DataField="AttendanceStatus" HeaderText="Attendance Status" />--%>
                                <asp:TemplateField HeaderText="Attendance Status">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlgAttendanceStatus" AutoPostBack="true" OnSelectedIndexChanged="ddlgAttendanceStatus_SelectedIndexChanged" Width="150px" Height="25px" runat="server">
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblAttendanceStatus" runat="server" Text='<%# Bind("AttendanceStatus") %>'>  
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:BoundField ItemStyle-Width="150px" DataField="AlternateRider" HeaderText="Alternate Rider" />--%>
                                <asp:TemplateField HeaderText="Alternate Rider">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlgAlternateRider" Width="150px" Height="25px" runat="server">
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblAlternateRider" runat="server" Text='<%# Bind("AlternateRider") %>'>  
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                            <EditRowStyle BackColor="#82aae8" />
                            <FooterStyle BackColor="#F27031" Font-Bold="True" ForeColor="#ffffff" />
                            <HeaderStyle BackColor="#DA7A4D" Font-Bold="True" ForeColor="#ffffff" HorizontalAlign="Left" />
                            <PagerStyle BackColor="#DA7A4D" ForeColor="#ffffff" Font-Bold="true" HorizontalAlign="Center" />
                            <RowStyle BackColor="#fbefe9" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#F5F7FB" />
                            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                            <SortedDescendingCellStyle BackColor="#E9EBEF" />
                            <SortedDescendingHeaderStyle BackColor="#4870BE" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
            </fieldset>

            
    </div>
            
            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>
