<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Rider.aspx.cs" Inherits="MRaabta.Files.Rider" MasterPageFile="~/BtsMasterPage.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../Bts_Css/Mazen.css" rel="stylesheet" />

        <link href="../mazen/css/sweetalert.css" rel="stylesheet" />
    <script src="../mazen/js/sweetalert-dev.js"></script>
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
            <span id="ContentPlaceHolder1_Errorid" style="color:Red;font-weight:bold;"></span>
            <legend style="font-size: medium;"><b>Add New Riders</b></legend>
        <%--<div class="col-lg-12 col-md-12 col-sm-12 screen-name">

            <div style="float: left;">
                <h3>Riders </h3>
            </div>
        </div>--%>
        <div class="col-lg-12 col-md-12 col-sm-12">
            <table>
                <tr>
                    <td style="width: 140px">
                        <b>Rider Code</b>
                    </td>
                    
                    <%--  <td style="width: 170px;">&nbsp;
                    </td>--%>
                    <td style="width: 100px">
                        <b>First Name</b>
                    </td>
                    
                    <td style="width: 100px">
                        <b>Last Name</b>
                    </td>
                    
                    <td style="width: 100px">
                        <b>CNIC</b>
                    </td>
                    
                    
                    
                </tr>
                <tr>
                    <td style="width: 170px">
                        <asp:TextBox ID="txtRiderCode" MaxLength="20"   runat="server">


                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Please Enter Rider Code" ControlToValidate="txtRiderCode" ForeColor="Red"></asp:RequiredFieldValidator>

                    </td>
                    <td style="width: 170px">
                        <asp:TextBox ID="txtFirstName" MaxLength="20"  Enabled="true" runat="server" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter First Name" ControlToValidate="txtFirstName" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                    <td style="width: 170px">
                        <asp:TextBox ID="txtLastName" MaxLength="20"  Enabled="true" runat="server" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Enter Last Name" ControlToValidate="txtLastName" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                    <td style="width: 170px">
                        <asp:TextBox ID="txtCNIC" MaxLength="20"  Enabled="true" runat="server" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please Enter CNIC" ControlToValidate="txtCNIC" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                    
                </tr>
                <tr>
                    <td style="width: 100px">
                        <b>Email</b>
                    </td>
                    <td style="width: 100px">
                        <b>Address</b>
                    </td>
                    <td style="width: 100px">
                        <b>Phone No</b>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    
                    <td style="width: 170px">
                        <asp:TextBox ID="txtEmail" MaxLength="20"   Enabled="true" runat="server" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" Display="Dynamic" runat="server" ErrorMessage="Please Enter Email" ControlToValidate="txtEmail" ForeColor="Red"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail" Display="Dynamic"
                                        ForeColor="Red" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                                        ErrorMessage="Invalid email address" />
                        </td>
                    <td style="width: 170px">
                        <asp:TextBox ID="txtAddress" MaxLength="50"  Enabled="true" runat="server" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" Display="Dynamic" runat="server" ErrorMessage="Please Enter Address" ControlToValidate="txtAddress" ForeColor="Red"></asp:RequiredFieldValidator>
                    
                    </td>
                    
                    <td style="width: 170px">
                        <asp:TextBox ID="txtPhoneNo" MaxLength="12"  Enabled="true" runat="server" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" Display="Dynamic" runat="server" ErrorMessage="Please Enter Phone No" ControlToValidate="txtPhoneNo" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>

                    <%--<td style="width: 140px">&nbsp;

                    </td>
                    <td style="width: 140px">&nbsp;

                    </td>
                    <td style="width: 140px">&nbsp;

                    </td>
                    <td style="width: 140px">&nbsp;

                    </td>
                    <td style="width: 140px">&nbsp;

                    </td>--%>
                    <%--<div style="margin: 18px 80px; float: right;">
    <asp:Button ID="Button1" CssClass="btn btn-success" runat="server" OnClick="btnSubmit_Click" Text="Save" />
<asp:Button ID="btnReset" CssClass="btn btn-danger" runat="server" OnClick="btnReset_Click" Text="Reset" />

</div>--%>

                    

                </tr>
      <%--<div class="btn_div" style="margin: -70px 75px;" >
                </div>--%>
            </table>
                               <asp:Button ID="btnSubmit" CssClass="btn btn-success" runat="server" OnClick="btnSubmit_Click" Text="Save" />

<%--                                    <asp:Button ID="btnSubmit" CssClass="btn btn-success" runat="server" OnClick="btnSubmit_Click" Text="Save" />--%>

        </div>

       
    </fieldset>
     <fieldset class="fieldsetSmall">
            <span id="ContentPlaceHolder1_Errorid" style="color:Red;font-weight:bold;"></span>
            <legend style="font-size: medium;"><b>Search Rider</b></legend>
        <div class="col-lg-12 col-md-12 col-sm-12">
            <table>
                <tr>
                    <td style="width: 140px">
                        <b>Search Rider</b>
                    </td>
                </tr>
                <tr>
                    
                    <td style="width: 170px">
                        <asp:TextBox ID="txtSearchRider" runat="server" />
                    </td>
                </tr>

            </table>
            <div class="btn_div" style="margin: -52px 270px 0;float:left" >
                    <asp:Button ID="btnSearch" CssClass="btn btn-success" runat="server" OnClick="btnSearch_Click" Text="Search" />
                </div>
        </div>
         <br />
    <div class="col-lg-12 col-md-12 col-sm-12">
        <asp:UpdatePanel ID="grdUp1" runat="server">
            <ContentTemplate>
                <asp:GridView ID="grdPR" runat="server" AutoGenerateEditButton="True" DataKeyNames="RiderCode" AutoGenerateColumns="false"
                    AllowPaging="true" OnRowCancelingEdit="grdPR_RowCancelingEdit" OnRowDataBound="grdPR_RowDataBound"
                    OnRowDeleting="grdPR_RowDeleting" OnRowEditing="grdPR_RowEditing"
                    OnRowUpdating="grdPR_RowUpdating"
                    OnPageIndexChanging="OnPageIndexChanging" AutoGenerateDeleteButton="True" PageSize="10"
                    class="table table-bordered table-hover"  Font-Size="Medium" CellPadding="4" ForeColor="#333333" GridLines="None" AllowSorting="true" BorderColor="#DA7A4D" Width="100%" BorderStyle="Solid" BorderWidth="5px">
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
                        <asp:TemplateField HeaderText="Rider Code">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtERiderCode" MaxLength="20"  Enabled="false" runat="server" Text='<%# Bind("RiderCode") %>'>  
                                </asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblRiderCode" runat="server" Text='<%# Bind("RiderCode") %>'>  
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                          <asp:TemplateField HeaderText="First Name">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEFirstName" MaxLength="20"  runat="server" Text='<%# Bind("FirstName") %>'>  
                                </asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblFirstName"  runat="server" Text='<%# Bind("FirstName") %>'>  
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Last Name">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtELastName" MaxLength="20"  runat="server" Text='<%# Bind("LastName") %>'>  
                                </asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblLastName" runat="server" Text='<%# Bind("LastName") %>'>  
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="CNIC">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtECNIC" MaxLength="20" runat="server" Text='<%# Bind("CNIC") %>'>  
                                </asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCNIC" runat="server" Text='<%# Bind("CNIC") %>'>  
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Address">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEAddress" MaxLength="50"  runat="server" Text='<%# Bind("Address") %>'>  
                                </asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblAddress" runat="server" Text='<%# Bind("Address") %>'>  
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Email">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEEmail"  MaxLength="20"  runat="server" Text='<%# Bind("Email") %>'>  
                                </asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblEmail" runat="server" Text='<%# Bind("Email") %>'>  
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Phone No">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEPhoneNo" MaxLength="12"  runat="server" Text='<%# Bind("PhoneNo") %>'>  
                                </asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblPhoneNo" runat="server" Text='<%# Bind("PhoneNo") %>'>  
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                    <EditRowStyle BackColor="#82aae8" />
                    <FooterStyle BackColor="#F27031" Font-Bold="True" ForeColor="#333333" />
                   <HeaderStyle BackColor="#DA7A4D" Font-Bold="True" ForeColor="#ffffff" />
                    <PagerStyle BackColor="#DA7A4D" ForeColor="#ffffff" Font-Bold="true" HorizontalAlign="Center" />
                    <RowStyle BackColor="#EFF3FB" />
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

    
    </div> </ContentTemplate>
            </asp:UpdatePanel>
           
</asp:Content>