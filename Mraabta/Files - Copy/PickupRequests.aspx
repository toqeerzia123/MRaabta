<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="PickupRequests.aspx.cs" Inherits="MRaabta.Files.PickupRequests" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script>
    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>
    <script type="text/javascript">
        debugger;
        function successalert() {
            swal({
                title: 'Message!',
                text: 'No Record Found',
                type: 'error'
            });
        }
        function OpenSingleRequest(id) {
            alert(id);
            PageMethods.OpenDetails(id, onSucess, onError);

            function onSucess(result) {
                alert(result);
            }

            function onError(result) {
                alert('Cannot process your request at the moment, please try later.');
            }
        }

    </script>

    <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
        <tr>
            <td colspan="12" align="center" class="head_column">
                <h3>Pickup Requests
                </h3>
            </td>
        </tr>
    </table>
    <br />
    <table style="margin-left: 30px; font-size: medium; padding-bottom: 0px; width: 100%;">
        <tr>
            <td style="width: 8%;">
                <label for="exampleInputEmail1">From Date</label>
                <br />
                <div class="row">
                    <div class="col-md-8">
                        <asp:TextBox ID="Fromdate" runat="server" placeholder="From" Width="190px" AutoPostBack="false"></asp:TextBox>
                    </div>
                    <div class="col-md-3 mt-1">
                        <asp:ImageButton ID="Popup_Button1" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                            Width="23px" Height="23px" />
                        <Ajax1:CalendarExtender ID="CalendarExtender1" TargetControlID="Fromdate" runat="server"
                            Format="dd-MM-yyyy" PopupButtonID="Popup_Button1"></Ajax1:CalendarExtender>
                    </div>

                </div>

            </td>
            <td style="width: 8%">

                <label for="exampleInputEmail1">
                    To Date</label>
                <br />
                <div class="row">
                    <div class="col-md-8">
                        <asp:TextBox ID="Todate" runat="server" placeholder="To" Width="190px" AutoPostBack="false"></asp:TextBox>
                    </div>
                    <div class="col-md-3 mt-1">
                        <asp:ImageButton ID="Popup_Button2" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                            Width="23px" Height="23px" />
                        <Ajax1:CalendarExtender ID="CalendarExtender2" TargetControlID="Todate" runat="server"
                            Format="dd-MM-yyyy" PopupButtonID="Popup_Button2"></Ajax1:CalendarExtender>
                    </div>
                </div>
            </td>
            <td style="width: 25%">
                <br />
                <asp:Button ID="btn_save" runat="server" Text="Search" CssClass="button" OnClick="btn_Save_Staff_Click"
                    Width="95px" />
            </td>
        </tr>
        <asp:Label ID="status_lbl" runat="server"></asp:Label>
    </table>
    <br />
    <div style="width: auto; height: 400px; overflow: scroll">
        <asp:GridView ID="PickupGrid" runat="server" AutoGenerateColumns="False" CellPadding="6" OnRowDataBound="PickupGrid_RowDataBound"
            CssClass="mGrid" AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None">
            <Columns>
                <asp:TemplateField HeaderText="S.No." ItemStyle-Width="4%" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderStyle-Width="100px" ReadOnly="True" HeaderText="Pickup Id" DataField="Id" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="12%"></asp:BoundField>
                <asp:BoundField HeaderStyle-Width="100px" ReadOnly="True" HeaderText="Consigner Name" DataField="ConsignerName" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="12%"></asp:BoundField>
                <asp:BoundField HeaderStyle-Width="100px" ReadOnly="True" HeaderText="Consigner Address" DataField="ConsignerAddress" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="18%"></asp:BoundField>
                <asp:BoundField HeaderStyle-Width="100px" ReadOnly="True" HeaderText="Origin City" DataField="origin" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="18%"></asp:BoundField>
                <asp:BoundField HeaderStyle-Width="100px" ReadOnly="True" HeaderText="Destination City" DataField="destination" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="18%"></asp:BoundField>
                <asp:BoundField HeaderStyle-Width="100px" ReadOnly="True" HeaderText="Requested Date" DataField="createdOn" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="18%"></asp:BoundField>
                <asp:TemplateField HeaderText="" ItemStyle-Width="4%" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkBtnDetails" Text="Details" runat="server" CommandName="Select" CommandArgument='<%# Eval("Id") %>' OnClick="ShowDetails_Click" />
                        <%--<asp:Button ID="btnview" runat="server" Text="View Detail" OnClientClick='<%#String.Format("return OpenSingleRequest({0})", Eval("Id")) %>'></asp:Button>--%>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>


    <asp:Panel Style="border: 1px solid black;" runat="server" Visible="false" ID="DetailDiv">
        <fieldset style="height: auto; border-color: #a8a8a8;"
            class="">

            <hr style="border: 1px solid black; margin-left: 30px; margin-right: 30px;" />

            <table style="font-size: medium; padding-bottom: 0px; width: 100%; font-size: 12px; text-align: center;">
                <tr>
                    <td style="width: 15%">
                        <b>
                            <asp:Label ID="Label1" runat="server">Weight</asp:Label></b><br />
                        <asp:Label ID="lblweight" runat="server">1</asp:Label>
                    </td>
                    <td style="width: 15%; border-left: 1px solid black;">
                        <b>
                            <asp:Label ID="Label2" runat="server">Pieces</asp:Label></b><br />
                        <asp:Label ID="lblPieces" runat="server">3</asp:Label>
                    </td>
                    <td style="width: 15%; border-left: 1px solid black;">
                        <div>
                            <b>
                                <asp:Label ID="Label4" runat="server">Origin</asp:Label></b><br />
                            <asp:Label ID="lblOrigin" runat="server">3</asp:Label>
                        </div>
                    </td>
                    <td style="width: 15%; border-left: 1px solid black;">
                        <div>
                            <b>
                                <asp:Label ID="Label6" runat="server">Destination</asp:Label></b><br />
                            <asp:Label ID="lblDestination" runat="server">3</asp:Label>
                        </div>
                    </td>
                    <td style="width: 15%; border-left: 1px solid black;">
                        <div>
                            <b>
                                <asp:Label ID="Label8" runat="server">Service Type</asp:Label></b><br />
                            <asp:Label ID="lblServiceType" runat="server">3</asp:Label>
                        </div>
                    </td>
                </tr>
            </table>

            <hr style="border: 1px solid black; margin-left: 30px; margin-right: 30px;" />

            <table style="font-size: 12px">
                <tr>

                    <td style="width: 25%">
                        <fieldset style="height: auto; border-color: #a8a8a8; margin-left: 6em"
                            class="">
                            <legend id="Legend6" visible="true" style="margin-left: 1em; width: auto; font-size: 14px; font-weight: bold; color: #1f497d;">From</legend>

                            <table style="margin-left: 30px">
                                <tr>
                                    <td>
                                        <div>
                                            <asp:Label ID="lblPickUpID" runat="server" Visible="false"></asp:Label>

                                            <b>
                                                <asp:Label ID="Label10" runat="server">Name: </asp:Label></b>
                                            <asp:Label ID="lblFromName" runat="server">3</asp:Label>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div>
                                            <b>
                                                <asp:Label ID="Label12" runat="server">Phone: </asp:Label></b>
                                            <asp:Label ID="lblFromPhone" runat="server">3</asp:Label>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div>
                                            <b>
                                                <asp:Label ID="Label14" runat="server">Address: </asp:Label></b>
                                            <asp:Label ID="lblFromAddress" runat="server">3</asp:Label>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>

                    <td style="width: 25%; border-left: 1px solid black;">
                        <fieldset style="height: auto; border-color: #a8a8a8; margin-left: 6em"
                            class="">
                            <legend id="Legend7" visible="true" style="margin-left: 1em; width: auto; font-size: 14px; font-weight: bold; color: #1f497d;">To</legend>

                            <table style="margin-left: 30px">
                                <tr>
                                    <td>
                                        <div>
                                            <b>
                                                <asp:Label ID="Label5" runat="server">Name: </asp:Label>
                                            </b>
                                            <asp:Label ID="lblToName" runat="server">3</asp:Label>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div>
                                            <b>
                                                <asp:Label ID="Label16" runat="server">Phone: </asp:Label>
                                            </b>
                                            <asp:Label ID="lblToPhone" runat="server">3</asp:Label>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div>
                                            <b>
                                                <asp:Label ID="Label18" runat="server">Address: </asp:Label>
                                            </b>
                                            <asp:Label ID="lblToAddress" runat="server">3</asp:Label>
                                        </div>
                                    </td>
                                </tr>
                            </table>

                        </fieldset>
                    </td>

                    <td style="width: 25%; border-left: 1px solid black;">
                        <fieldset style="height: auto; border-color: #a8a8a8;"
                            class="">
                            <legend id="Legend8" visible="true" style="margin-left: 1em; width: auto; font-size: 14px; font-weight: bold; color: #1f497d;"></legend>

                            <table style="margin-left: 30px">
                                <tr>
                                    <td>
                                        <div>
                                            <b>
                                                <asp:Label ID="Label3" runat="server">Assign Rider </asp:Label>
                                            </b>
                                            <asp:DropDownList ID="dd_riderList" runat="server" Width="180px" Height="30px"
                                                AutoPostBack="true" Style="margin-left: 1em;">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div>
                                            <b>
                                                <asp:Label ID="Label9" runat="server">View Location </asp:Label>
                                            </b>
                                            <asp:Label ID="lblLocationCoordinates" runat="server" Style="margin-left: 1em;"></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div>
                                        </div>
                                    </td>
                                </tr>
                            </table>

                        </fieldset>
                    </td>
                </tr>
            </table>
            <hr style="border: 1px solid black; margin-left: 30px; margin-right: 30px;" />
            <div style="font-size: 12px; margin-left: 3em">
                <asp:Button runat="server" ID="Cancel" Text="Cancel Request" OnClick="Cancel_Click" CssClass="button" OnClientClick="return confirm('Cancelling, are you sure?');" />
                <asp:Button runat="server" ID="ConfirmRequest" Text="Submit Request" CssClass="button" OnClick="ConfirmRequest_Click" OnClientClick="return confirm('Confirm Pickup Request');" />
            </div>
        </fieldset>
    </asp:Panel>
</asp:Content>