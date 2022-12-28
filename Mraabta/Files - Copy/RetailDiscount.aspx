<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="RetailDiscount.aspx.cs" Inherits="MRaabta.Files.RetailDiscount" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <link rel="Stylesheet" href="../assets/bootstrap-4.3.1-dist/css/bootstrap.min.css" />
    <script type="text/javascript" src="../assets/js/jquery-1.11.0.min.js"></script>
    <style type="text/css">
        body 
        {
           font-size:13px;
           font-weight:none;
           line-height:1.20;
        }
        .form-inline
        {
            border-bottom: 1px solid #eee;
            padding: 0;
        }
        .MainTbl th
        {
            background-color: #404040;
            color: white;
        }
        .TdScroll
        {
            overflow-y: scroll;
            height: 500px;
            float: left;
        }
        .TdScrollRight
        {
            overflow-y: scroll;
            height: 500px;
            float: right;
        }
    </style>
    <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
        <tr>
            <td colspan="12" align="center" class="head_column">
                <h3 style="text-align: center">
                    Retail Discount
                </h3>
            </td>
        </tr>
    </table>
    
    <h4 style="color:red" id="StatusBox"></h4>

    <fieldset style="border: solid; border-width: thin; height: auto; border-color: #a8a8a8;"
        class="">

        <legend id="Legend5" visible="true" style="width: auto; font-size: 14px; font-weight: bold;
            color: #1f497d;">Discount</legend>
        <div class="" style="font-size: 12px">
            <div class="ml-3 mr-1">
                <input type="radio" name="dicountTtype" id="NormalDiscountRadio" checked="checked" onclick="toggeDiscountNormal()" />Normal Discount
                <input type="radio" name="dicountTtype" id="OneTimeDiscountRadio" onclick="toggeDiscountOnetime()" />One Time Discount
                <input type="radio" name="dicountTtype" id="DuplicateDiscountRadio" onclick="ToggleDuplicateDiscountRadio()" />Duplicate Discount
            </div>
            <div class="row ml-1 mr-1">
                
                <form role="form" id="form1">
                <div class=" col-xs-10 col-sm-3 col-md-3 col-lg-3 ">
                    <label for="exampleInputPassword1">
                        <b>Zone: </b>
                    </label>
                    <div class=" form-inline">
                        <select id="zonesDropdown" class="form-control" onchange="ZoneChange()" style="font-size: 12px;
                            margin-right: 15px">
                        </select>
                        <b>All:</b>
                        <input type="checkbox" name="zoneAll" id="zoneAll" />
                    </div>
                </div>
                <div class=" col-xs-10 col-sm-3 col-md-3 col-lg-3">
                    <label for="exampleInputFile">
                        <b>Branch:</b></label>
                    <div class=" form-inline">
                        <select id="branchesDropdown" onchange="BranchChange()" class="form-control" style="font-size: 12px;
                            margin-right: 15px">
                        </select>
                        <b>All:</b>
                        <input type="checkbox" name="branchCheckBox" id="branchCheckBoxAll" />
                    </div>
                </div>
                <div class=" col-xs-10 col-sm-3 col-md-3 col-lg-3">
                    <label for="exampleInputEmail1">
                        <b>From Date</b></label><br />
                    <asp:TextBox ID="Fromdate" runat="server" placeholder="From" Width="180px"  AutoPostBack="false"></asp:TextBox>
                        <asp:ImageButton ID="Popup_Button1" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                            Width="23px" Height="23px" />
                        <Ajax1:CalendarExtender ID="CalendarExtender1" TargetControlID="Fromdate" runat="server"
                         Format="yyyy-MM-dd" PopupButtonID="Popup_Button1">
                        </Ajax1:CalendarExtender>
                        
                    <%--<input placeholder="dd/MM/yyyy" class=" form-control" type="date" id="Fromdate" style="font-size: 12px;">--%>
                

                </div>
                <div class="col-xs-10 col-sm-3 col-md-3 col-lg-3">
                    <label for="exampleInputEmail1">
                        <b>To Date</b></label><br />
                    <asp:TextBox ID="Todate" runat="server" placeholder="From" Width="180px"  AutoPostBack="false"></asp:TextBox>
                        <asp:ImageButton ID="Popup_Button2" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                            Width="23px" Height="23px" />
                        <Ajax1:CalendarExtender ID="CalendarExtender2" TargetControlID="Todate" runat="server"
                         Format="yyyy-MM-dd" PopupButtonID="Popup_Button2">
                        </Ajax1:CalendarExtender>
                        
                    <%--<input type="text" class="form-control" id="adverts_startDate" placeholder="Enter Name">--%>
                    <%--<input placeholder="dd/MM/yyyy" class=" form-control" type="date" id="Todate" style="font-size: 12px;">--%>
                    <%--<input type="text" class="form-control" id="adverts_endDate" name="adverts_endDate" style="font-size:12px;" />--%>
                </div>
                <%--                    <br />
                <div class="clearfix"></div>--%>

                <div class="col-xs-10 col-sm-3 col-md-3 col-lg-3">
                    <label>
                        <b>Discount Type:</b></label>
                    <select id="DiscountType" class="form-control" onchange="CheckDiscount()" style="font-size: 12px;">
                        <option selected="selected" value="1">Percentage</option>
                        <option value="2">Amount</option>
                    </select>
                </div>
                <div class=" col-xs-10 col-sm-3 col-md-3 col-lg-3">
                    <label for="exampleInputPassword1">
                        <b>Discount Value:</b></label>
                    <div id="CheckDiscountType">
                        <input type="number" class="form-control" id="discountValuePercentage" name="discountValuePercentage"
                            oninput="return check(event, value)" min="0" max="100" step="0.01" style="font-size: 12px;"   />
                    </div>
                </div>
                <div class=" col-xs-10 col-sm-3 col-md-3 col-lg-3">
                    <label for="exampleInputPassword1">
                        <b>Service type:</b></label>
                    <select id="serviceTypeDropdown" class="form-control" style="font-size: 12px;">
                        <option value="Second Day">Second Day</option>
                        <option value="overnight">overnight</option>
                    </select>
                </div>
                <div class=" col-xs-10 col-sm-3 col-md-3 col-lg-3">
                    <label for="exampleInputPassword1">
                        <b>Short Description:</b></label>
                    <input type="text" class="form-control" id="shortDescription" name="shortDescription"
                        maxlength="50" style="font-size: 12px;"   />
                </div>
                <div class=" col-xs-10 col-sm-3 col-md-3 col-lg-3">
                    <label for="exampleInputPassword1">
                        <b>Long Description:</b></label>
                    <textarea id="LongDescription" rows="1" cols="12" name="LongDescription" class="form-control"
                        maxlength='100' style="font-size: 12px;"></textarea>
                </div>
                <div class=" col-xs-10 col-sm-3 col-md-3 col-lg-3">
                    <label for="exampleInputPassword1">
                        <b>Minimum Shipment Count:</b></label>
                    <input type="text" class="form-control" id="MinShipment" name="MinShipment" onkeypress="return isNumberKey(event)"
                        maxlength="4" style="font-size: 12px;" />
                </div>
                <div class=" col-xs-10 col-sm-3 col-md-3 col-lg-3">
                    <label for="exampleInputPassword1">
                        <b>Maximum Shipment Count:</b></label>
                    <input type="text" class="form-control" id="MaxShipment" name="MaxShipment" onkeypress="return isNumberKey(event)"
                        maxlength="4" style="font-size: 12px;"/>
                </div>
                <div class=" col-xs-10 col-sm-3 col-md-3 col-lg-3">
                    <label for="exampleInputPassword1">
                        <b>Minimum Shipment Weight:</b></label>
                    <input type="number" class="form-control" id="MinShipmentWeight" name="MinShipmentWeight"
                        oninput="return check(event, value)" min="0" max="1000" step="0.1" style="font-size: 12px;" />
                </div>
                <div class=" col-xs-10 col-sm-3 col-md-3 col-lg-3">
                    <label for="exampleInputPassword1">
                        <b>Maximum Shipment Weight:</b></label>
                    <input type="number" class="form-control" id="MaxShipmentWeight" name="MaxShipmentWeight"
                        oninput="return check(event, value)" min="0" max="1000" step="0.1" style="font-size: 12px;" />
                </div>
                <div class=" col-xs-10 col-sm-3 col-md-3 col-lg-3" id="OneTimeDiscountDiv" style="display:none">
                    <label for="exampleInputPassword1">
                        <b>Phone Number:</b></label>
                    <input type="text" class="form-control" id="DiscountId" name="DiscountId" 
                        style="font-size: 12px;" />
                </div>
               <%-- <div class=" col-xs-10 col-sm-3 col-md-3 col-lg-3">
                    <br />
                    <input type="button" value="Generate Special Id (Optional)" onclick="GenerateID()" class="form-control mt-2"
                        style="font-size: 12px; background-color: #f27031; color: white;" />
                </div>--%>
                <div class=" col-xs-10 col-sm-3 col-md-3 col-lg-3" ></div>
                <div class=" col-xs-10 col-sm-3 col-md-3 col-lg-3" id="SecondEmptyDiv"></div>

                <div class=" col-xs-10 col-sm-3 col-md-3 col-lg-3">
                    <label for="exampleInputPassword1">
                        <b>Group Id:</b></label>
                    <input type="text" class="form-control" id="GroupId" name="GroupId" style="font-size: 12px;" />
                </div>
                </form>
                <div class="clearfix">
                </div>
                <br />
                <br />
            </div>
        </div>
        <br />
    </fieldset>
    <h4 style="color:red;" id="ECCountDiv"> </h4>

    <fieldset style="border: solid; border-width: thin; height: auto; border-color: #a8a8a8;"
        class="ml-2 mr-2">
        <legend id="Legend6" visible="true" style="width: auto; margin-bottom: 0px; font-size: 16px;
            font-weight: bold; color: #1f497d;">Express Center</legend>
        <div class="row ml-3 mr-3">
        </div>
        <div id="ExpressBoxDiv"  class=" ml-1 col-md-8" style="font-size: 12px;">
           
            <input type="checkbox" id="AllExpressCenters" onclick="toggle(this)" name="CheckAllECs"
                value="10397" />
            <b>All Express Centers:</b>
            <input type="checkbox" id="AllCompanyMaintained" onclick="toggleCompanyMaintained(this)" name="CheckAllCompanyMaintained"
                value="10397" />
            <b style="background-color:lightcyan;">All Company Maintained:</b>
            <input type="checkbox" id="AllFranchise" onclick="toggleFranchise(this)" name="CheckAllFranchise"
                value="10397" />
            <b style="background-color:aqua;">All Franchise:</b>

             <input type="checkbox" id="AllShopnShop" onclick="toggleShopnShop(this)" name="CheckAllShopnShop"
                value="10397" />
            <b style="background-color:lightskyblue;">All Shop n Shop:</b>

             <input type="checkbox" id="AllFranchiseBranch" onclick="toggleFranchiseBranch(this)" name="CheckAllFranchiseBranch"
                value="10397" />
            <b style="background-color:deepskyblue;">All Franchise Branch:</b>

            <br />
        </div>
        <div class="row ml-1" style="overflow-y: scroll; height: 170px; font-size: 12px;
            width: 99%">
            <br />
            <br />
            <div class="row col-md-12 h-25  ExpressCentersCheckbox" id="ExpressCentersCheckbox">
            </div>
        </div>
    </fieldset>
    <br />
    <div class="row ml-1">
        <div class="col-md-6 row">

        <button id="btnCancelSave" type="button" class="btn" style="background-color: #f27031;
                color: white; height: 2.4em; font-size: 12px;width:20%">
                Reset</button>
            <button id="btnTempSave" type="button" class="btn btn-block " style="background-color: #f27031;
                color: white; height: 2.4em; font-size: 12px;width:20%;margin-left:20px">
                Validate</button>
        
            <button id="btnSubmitSave" type="button" class="btn " style="background-color: #f27031;
                color: white; height: 2.4em; font-size: 12px;width:20%;margin-left:20px" disabled="disabled">
                Save in Database</button>
            <button id="btnActivate" type="button" class="btn btn-block" style="background-color: #f27031;
                color: white; height: 2.4em; font-size: 12px;width:20%;margin-left:20px" disabled="disabled">
                Activate</button>
        </div>
   </div>
    <br />
    <table class="MainTbl table table-striped TdScroll" style="width: 100%; display: none"
        id="DiscountTable">
        <thead>
            <tr style="font-size: 12px;">
                <th>
                    From Date
                </th>
                <th>
                    To Date
                </th>
                <th>
                    Zone
                </th>
                <th>
                    Branch
                </th>
                <th>
                    Express Center
                </th>
                <th>
                    Service Type
                </th>
                <th>
                    Discount Type
                </th>
                <th>
                    Discount Value
                </th>
                <th>
                    Short Description
                </th>
                <th>
                    Long Description
                </th>
                <th>
                    Min. Shipment
                </th>
                <th>
                    Max. Shipment
                </th>
                <th>
                    Min. Shipment Weight
                </th>
                <th>
                    Max. Shipment Weight
                </th>
                <th>
                    Special Discount Id
                </th>
                <th>
                </th>
            </tr>
        </thead>
        <tbody id="DiscountTempSave" style="font-size: 12px;">
        </tbody>
    </table>
    <%--<script type="text/javascript" src="../assets/js/jquery-1.11.0.min.js"></script>--%>
    <script type="text/javascript">

        $(document).ready(function () {
            resetForms();
            GetServiceTypes();

        });

        function resetForms() {
            document.forms['form1'].reset();
        }
        function toggeDiscountNormal() {
            debugger;

            document.getElementById('OneTimeDiscountDiv').style.display = "none";
            document.getElementById('SecondEmptyDiv').style.display = "block";
            $('#NormalDiscountRadio').attr('checked', true);
        }
        function toggeDiscountOnetime() {
            debugger;

            document.getElementById('OneTimeDiscountDiv').style.display = "block";
            document.getElementById('SecondEmptyDiv').style.display = "none";
            $('#OneTimeDiscountRadio').attr('checked', true);
        }

        function ToggleDuplicateDiscountRadio() {
            debugger;
            $("#btnTempSave").prop("disabled", false);
            $("#btnActivate").prop("disabled", true);

            document.getElementById('GroupId').value = '';
            document.getElementById('<%=Fromdate.ClientID %>').disabled = false;
            document.getElementById('<%=Todate.ClientID %>').disabled = false;

            document.getElementById('<%=Popup_Button1.ClientID %>').disabled = false;
            document.getElementById('<%=Popup_Button2.ClientID %>').disabled = false;
        }

        $("#GroupId").blur(function () {
            debugger;
            var groupId = $(this).val();
            if (groupId != "") {
                $.ajax({
                    aysnc: false,
                    type: "GET",
                    url: 'RetailDiscount.aspx/GetDataFromGroupId?groupId=' + groupId,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    //   data: JSON.stringify({ groupId: groupId }),
                    success: (rs) => {
                        debugger;
                        if (rs.d.fromDate != null) {
                            if (rs.d.discountType == 1) {
                                document.getElementById("CheckDiscountType").innerHTML = `<input type="number" class="form-control" id="discountValuePercentage" name="discountValuePercentage" onInput="return check(event, value)" min="0" max="100" step="0.01" style="font-size: 12px; " />`;
                                document.getElementById("DiscountType").value = rs.d.discountType
                                document.getElementById("discountValuePercentage").value = parseFloat(rs.d.discountValue);
                            } else {
                                document.getElementById("CheckDiscountType").innerHTML = `<input type="text" class="form-control" id="discountValueAmount" name="discountValueAmount" onkeypress="return isNumberKeyWithDecimal(event)" maxlength="4" style="font-size:12px;" />`;
                                document.getElementById("DiscountType").value = rs.d.discountType
                                document.getElementById("discountValueAmount").value = parseFloat(rs.d.discountValue);
                            }
                            document.getElementById('serviceTypeDropdown').value = rs.d.serviceType;
                            document.getElementById("shortDescription").value = rs.d.shortDescription;
                            document.getElementById("LongDescription").value = rs.d.longDescription;
                            document.getElementById("MinShipment").value = rs.d.minShipment;
                            document.getElementById("MaxShipment").value = rs.d.maxShipment;
                            document.getElementById("MinShipmentWeight").value = rs.d.minShipmentWeight;
                            document.getElementById("MaxShipmentWeight").value = rs.d.maxShipmentWeight;
                            var kk = rs.d.specialDiscountId;
                            if (kk == null || isNaN(kk)) {
                                document.getElementById("DiscountId").value = "";
                            }
                            else {
                                var specialDiscountId = kk.toString();
                                document.getElementById("DiscountId").value = specialDiscountId;
                            }
                            var is_approved = rs.d.is_Approved;
                            var fromDateTd = rs.d.fromDate.split('/');
                            var newFromDate = fromDateTd[2] + '-' + fromDateTd[0] + '-' + fromDateTd[1];
                            document.getElementById('<%=Fromdate.ClientID %>').disabled = true;
                            document.getElementById('<%=Popup_Button1.ClientID %>').disabled = true;
                            document.getElementById('<%=Fromdate.ClientID %>').value = newFromDate;

                            var toDateTd = rs.d.toDate.split('/');
                            var newToDate = toDateTd[2] + '-' + toDateTd[0] + '-' + toDateTd[1];
                            document.getElementById('<%=Todate.ClientID %>').disabled = true;
                            document.getElementById('<%=Popup_Button2.ClientID %>').disabled = true;

                            document.getElementById('<%=Todate.ClientID %>').value = newToDate;
                            if (is_approved == "True" || is_approved == "False") {
                                $("#btnTempSave").prop("disabled", false);
                                //$("#btnTempSave").prop("disabled", true);
                                $("#btnSubmitSave").prop("disabled", true);
                                $("#btnActivate").prop("disabled", true);
                                if (is_approved == "True") {
                                    document.getElementById('StatusBox').innerText = "Selected discount is approved";
                                    document.getElementById('<%=Fromdate.ClientID %>').disabled = true; 
                                    document.getElementById('<%=Todate.ClientID %>').disabled = true;
                                    document.getElementById('<%=Popup_Button1.ClientID %>').disabled = true;
                                    document.getElementById('<%=Popup_Button2.ClientID %>').disabled = true;
                                    if (rs.d.discountType == 1) {
                                        document.getElementById('discountValuePercentage').disabled = true;

                                    } else {
                                        document.getElementById('discountValueAmount').disabled = true;
                                    }
                                    document.getElementById('shortDescription').disabled = true; 
                                    document.getElementById('LongDescription').disabled = true; 
                                    document.getElementById('MinShipment').disabled = true;
                                    document.getElementById('MaxShipment').disabled = true;
                                    document.getElementById('MinShipmentWeight').disabled = true;
                                    document.getElementById('MaxShipmentWeight').disabled = true;
                                    document.getElementById('DiscountId').disabled = true;
                                    document.getElementById('zonesDropdown').disabled = true;
                                    document.getElementById('branchesDropdown').disabled = true;
                                    document.getElementById('zoneAll').disabled = true;
                                    document.getElementById('branchCheckBoxAll').disabled = true;
                                    document.getElementById('DiscountType').disabled = true;
                                    document.getElementById('serviceTypeDropdown').disabled = true;

                                } else {
                                    document.getElementById('StatusBox').innerText = "Selected discount is cancelled";
                                }
                            } else {
                                $("#btnTempSave").prop("disabled", false);
                                $("#btnActivate").prop("disabled", false);
                                document.getElementById('StatusBox').innerText = "Selected discount pending";
                            }

                                
                            //if (rs.d.Profile_User == "4") {
                            //    $("#btnActivate").prop("disabled", false);
                            //} else {
                            //    $("#btnActivate").prop("disabled", true);

                            //}

                        } else {
                            debugger;
                            document.getElementById('form1').reset();
                            document.getElementById("CheckDiscountType").innerHTML = `<input type="number" class="form-control" id="discountValuePercentage" name="discountValuePercentage" onInput="return check(event, value)" min="0" max="100" step="0.01" style="font-size: 12px; " />`;

                            var day = new Date().getDate();
                            var month = new Date().getMonth() + 1;
                            var year = new Date().getFullYear();
                            var dateFull =year + "-" + month + "-" + day;
                            document.getElementById('<%=Fromdate.ClientID %>').value = dateFull;
                            document.getElementById('<%=Todate.ClientID %>').value = dateFull;

                            document.getElementById('<%=Fromdate.ClientID %>').disabled = false;
                            document.getElementById('<%=Todate.ClientID %>').disabled = false;

                            document.getElementById('<%=Popup_Button1.ClientID %>').disabled = false;
                            document.getElementById('<%=Popup_Button2.ClientID %>').disabled = false;

                            $("#btnTempSave").prop("disabled", false);
                            $("#btnActivate").prop("disabled", true);
                            document.getElementById('StatusBox').innerText = "";



                        }
                    }
                });
                ////////////second method to fetch all zones or 1
                $.ajax({
                    async: false,
                    type: "POST",
                    url: 'RetailDiscount.aspx/GetZoneOneOrAll',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ groupId: groupId }),
                    success: (rs) => {
                        if (rs.d != null) {

                            ///For zone all or one
                            if (rs.d.Zones[0] == "All") {
                                $('#zoneAll').prop('checked', true);
                                getBranches("All");
                                var option = $('<option></option>').attr("value", "All").text("All");
                                $("#zonesDropdown").empty().append(option);
                                $('#branchCheckBoxAll').prop('checked', false);
                                $('#AllExpressCenters').prop('checked', false);
                            } else if (rs.d.Zones[0] == "") {

                            } else {
                                getZones();
                                document.getElementById('zonesDropdown').value = rs.d.Zones[0];
                                document.getElementById("branchCheckBoxAll").checked = false;
                                getBranches(rs.d.Zones[0]);
                            }


                            ///For branch all or one

                            if (rs.d.Branches[0] == "All") {
                                var option = $('<option></option>').attr("value", "All").text("All");
                                $("#branchesDropdown").empty().append(option);
                                $('#branchCheckBoxAll').prop('checked', true);
                                $('#AllExpressCenters').prop('checked', false);
                                var ZoneId = document.getElementById('zonesDropdown').value;
                                GetExpressCenters(ZoneId, rs.d.Branches[0]);
                            } else if (rs.d.Branches[0] == "") {

                            } else {
                                var ZoneId = document.getElementById('zonesDropdown').value;
                                GetExpressCenters(ZoneId, rs.d.Branches[0]);
                                document.getElementById('branchesDropdown').value = rs.d.Branches[0];
                                document.getElementById("AllExpressCenters").checked = false;
                            }

                            for (var i = 0; i < rs.d.ECs.length; i++) {
                                $('#' + rs.d.ECs[i]).prop('checked', true);
                            }

                        }
                        CheckboxesAlreadyChanged();

                    }
                });
            }
            else {

                //if ($('#DuplicateDiscountRadio').is(':checked')) {
                    $("#btnActivate").prop("disabled", true);
                    
                    $("#btnTempSave").prop("disabled", false);
                    document.getElementById('<%=Fromdate.ClientID %>').disabled = false;
                    document.getElementById('<%=Todate.ClientID %>').disabled = false;

                    document.getElementById('<%=Popup_Button1.ClientID %>').disabled = false;
                    document.getElementById('<%=Popup_Button2.ClientID %>').disabled = false;
               // } else {
                   <%-- $("#btnTempSave").prop("disabled", true);
                    document.getElementById('<%=Fromdate.ClientID %>').disabled = true;
                    document.getElementById('<%=Todate.ClientID %>').disabled = true;

                    document.getElementById('<%=Popup_Button1.ClientID %>').disabled = true;
                    document.getElementById('<%=Popup_Button2.ClientID %>').disabled = true;--%>

                //}


            }
        });

        function GenerateID() {
            debugger;
            $.ajax({
                type: "POST",
                url: 'RetailDiscount.aspx/GetSpecialId',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: (rs) => {
                    debugger;
                    var specialDiscountId = rs.d;
                   // specialDiscountId = specialDiscountId.toString().padStart(4, "0");
                    
                    document.getElementById('DiscountId').value = specialDiscountId;
                    
                }
            });
        }

        check = function (e, value) {
            if (!e.target.validity.valid) {
                e.target.value = value.substring(0, value.length - 1);
                return false;
            }
            var idx = value.indexOf('.');
            if (idx >= 0 && value.length - idx > 3) {
                e.target.value = value.substring(0, value.length - 1);
                return false;
            }
            return true;
        }


      


        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }

        function isNumberKeyWithDecimal(evt) {
            var status=false;
            var charCode = (evt.which) ? evt.which : event.keyCode
            
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                status= false;
            if (charCode == 46)
                status = true;
            if (charCode > 47 && charCode < 58)
                status = true;  
            return status;
        }

        function toggle(source) {
            debugger;
           
                checkboxes = document.getElementsByName('ECs');
                for (var i = 0, n = checkboxes.length; i < n; i++) {
                    checkboxes[i].checked = source.checked;
            }
            CheckboxesAlreadyChanged();
        }
        function toggleShopnShop(source) {
            debugger;

            checkboxes = document.getElementsByClassName('ShopnShop');
            for (var i = 0, n = checkboxes.length; i < n; i++) {
                checkboxes[i].checked = source.checked;
            }
            CheckboxesAlreadyChanged();
        }
        function toggleFranchise(source) {

            checkboxes = document.getElementsByClassName('Franchise');
            for (var i = 0, n = checkboxes.length; i < n; i++) {
                checkboxes[i].checked = source.checked;
            }
            CheckboxesAlreadyChanged();
        }
        function toggleFranchiseBranch(source) {
            checkboxes = document.getElementsByClassName('FranchiseBranch');
            for (var i = 0, n = checkboxes.length; i < n; i++) {
                checkboxes[i].checked = source.checked;
            }
            CheckboxesAlreadyChanged();
        }
        function toggleCompanyMaintained(source) {
            debugger;
            checkboxes = document.getElementsByClassName('CompanyMaintained');
            for (var i = 0, n = checkboxes.length; i < n; i++) {
                checkboxes[i].checked = source.checked;
            }
            CheckboxesAlreadyChanged();
        }

        $("#zoneAll").change(function () {
            debugger;
            if (this.checked) {
                getBranches("All");
                var option = $('<option></option>').attr("value", "All").text("All");
                $("#zonesDropdown").empty().append(option);
                $('#branchCheckBoxAll').prop('checked', false);
                $('#AllExpressCenters').prop('checked', false);


            } else {
                getZones();
                $('#branchCheckBoxAll').prop('checked', false);
                $('#AllExpressCenters').prop('checked', false);

            }
        });

        $("#branchCheckBoxAll").change(function () {
            debugger;
            if (this.checked) {
                var option = $('<option></option>').attr("value", "All").text("All");
                $("#branchesDropdown").empty().append(option);
                var ZoneId = document.getElementById('zonesDropdown').value;
                var Id = document.getElementById('branchesDropdown').value;
                GetExpressCenters(ZoneId, Id);
                $('#AllExpressCenters').prop('checked', false);

            } else {
                var Id = document.getElementById('zonesDropdown').value;
                getBranches(Id);
            }
        });
        var AllzoneName = '';
        var AllZoneCode= '';

        var getAllZones = () => {

            
                debugger;
            $.ajax({
                async: false,
                type: "POST",
                url: 'RetailDiscount.aspx/GetZones',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                asyc: false,
                success: (rs) => {
                    AllzoneName = '';
                    AllZoneCode = '';
                    var rr = rs.d;
                    for (let y of rr) {
                        AllzoneName += y.Text + ', ';
                        AllZoneCode += y.Value+',';
                    }
                }
            });
        };

        var AllBranchName = '';
        var AllBranchCode = '';

        var getAllBranches = (zone) => {
            debugger;
            $.ajax({
                async: false,
                type: "POST",
                url: 'RetailDiscount.aspx/GetBranches',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ zone: zone }),                
                success: (rs) => {
                    AllBranchName = '';
                    AllBranchCode = '';
                    var rr = rs.d;
                    for (let y of rr) {
                        AllBranchName += y.Text + ', ';
                        AllBranchCode += y.Value+',';
                    }
                    $('#AllExpressCenters').prop('checked', false);

                    }
             });
        };

        var getZones = () => {
            debugger;
            $.ajax({
                async: false,
                type: "POST",
                url: 'RetailDiscount.aspx/GetZones',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                asyc:true,
                success: (rs) => {
                debugger;
                    var kk = '';
                    var rr = rs.d;
                    for (let y of rr) {
                        kk += `<option value="${y.Value}">${y.Text}</option>`;
                    }
                    $('#zonesDropdown').html(kk);
                    var Id = document.getElementById('zonesDropdown').value;
                    getBranches(Id);
                }
            });
        };

        var getBranches = (zone) => {
            debugger;
            $.ajax({
                async: false,
                type: "POST",
                url: 'RetailDiscount.aspx/GetBranches',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ zone:zone }),
                success: (rs) => {
                    var kk = '';
                    var rr = rs.d;
                    for (let y of rr) {
                        kk += `<option value="${y.Value}">${y.Text}</option>`;
                    }
                    $('#branchesDropdown').html(kk);
                    var branch = document.getElementById('branchesDropdown').value;
                    GetExpressCenters(zone, branch);
                    $('#AllExpressCenters').prop('checked', false);


                }, error: function (jqXHR, textStatus, errorThrown) {
                }
            });
        };

        function CheckboxesChanged() {


            var $checkboxes = $('#ExpressCentersCheckbox input[type="checkbox"]');

            $checkboxes.change(function () {
                var countCheckedCheckboxes = $checkboxes.filter(':checked').length;
                // $('#count-checked-checkboxes').text(countCheckedCheckboxes);

                document.getElementById('ECCountDiv').innerText ="Total express centers selected: "+ countCheckedCheckboxes;
            });
        }
        function CheckboxesAlreadyChanged() {
            var $checkboxes = $('#ExpressCentersCheckbox input[type="checkbox"]');
            var countCheckedCheckboxes = $checkboxes.filter(':checked').length;
            document.getElementById('ECCountDiv').innerText = "Total express centers selected: " + countCheckedCheckboxes;
        }

        var GetExpressCenters = (zone, branch) => {
            GetExpressCentersCompanyMaintained(zone, branch);
            GetExpressCentersFranchise(zone, branch);
            GetExpressCentersShopnShop(zone, branch);
            GetExpressCentersFranchiseBranch(zone, branch);
            CheckboxesChanged();

            //debugger;
            //$.ajax({
            //    async: false,
            //    type: "POST",
            //    url: 'RetailDiscount.aspx/GetExpressCenters',
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    data: JSON.stringify({ zone: zone, branch: branch }),
            //    success: (rs) => {
            //        //$(".ExpressCentersCheckbox").empty();
            //        var kk = '';
            //        var rr = rs.d;
            //        for (let y of rr) {
            //                // $("#ExpressCentersCheckbox").append("<div class='form-inline'> ");
            //            //$("#ExpressCentersCheckbox").append("<label for='chk_" + 55 + "'>" + y.Text + "</label><input id='chk_" + y.Value + "' type='checkbox' value='" + y.Text + "' />");
            //            $("#ExpressCentersCheckbox").append("<div class=' col-md-4'>  <input  id = '" + y.Value + "' type = 'checkbox' value='" + y.Text + "' name='ECs' class='ml-4  ,,," + y.branch + ",,," + y.zone + "' /> " + y.Text + "</div>");

            //                //$("#ExpressCentersCheckbox").append( y.Text +"</div>");
            //        }
                    
            //    }, error: function (jqXHR, textStatus, errorThrown) {
            //    }
            //});
        };
        var GetExpressCentersCompanyMaintained = (zone, branch) => {
            debugger;
            $.ajax({
                async: false,
                type: "POST",
                url: 'RetailDiscount.aspx/GetExpressCentersCompanyMaintained',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ zone: zone, branch: branch }),
                success: (rs) => {
                    $(".ExpressCentersCheckbox").empty();
                    var kk = '';
                    var rr = rs.d;
                    for (let y of rr) {
                        // $("#ExpressCentersCheckbox").append("<div class='form-inline'> ");
                        //$("#ExpressCentersCheckbox").append("<label for='chk_" + 55 + "'>" + y.Text + "</label><input id='chk_" + y.Value + "' type='checkbox' value='" + y.Text + "' />");
                        $("#ExpressCentersCheckbox").append("<div style='background-color: lightcyan;height:72%' class='col-sm-3'> <input id = '" + y.Value + "' type = 'checkbox' value='" + y.Text + "' name='ECs' class='CompanyMaintained ml-4  ,,," + y.branch + ",,," + y.zone + "' /> " + y.Text + "</div>");

                        //$("#ExpressCentersCheckbox").append( y.Text +"</div>");
                    }

                }, error: function (jqXHR, textStatus, errorThrown) {
                }
            });
        }; 

        var GetExpressCentersShopnShop = (zone, branch) => {
            debugger;
            $.ajax({
                async: false,
                type: "POST",
                url: 'RetailDiscount.aspx/GetExpressCentersShopnShop',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ zone: zone, branch: branch }),
                success: (rs) => {
                    var kk = '';
                    var rr = rs.d;
                    for (let y of rr) {
                        // $("#ExpressCentersCheckbox").append("<div class='form-inline'> ");
                        //$("#ExpressCentersCheckbox").append("<label for='chk_" + 55 + "'>" + y.Text + "</label><input id='chk_" + y.Value + "' type='checkbox' value='" + y.Text + "' />");
                        $("#ExpressCentersCheckbox").append(" <div style='background-color: lightskyblue;height:72%' class='col-sm-3'> <input id = '" + y.Value + "' type = 'checkbox' value='" + y.Text + "' name='ECs' class='ShopnShop ml-4  ,,," + y.branch + ",,," + y.zone + "' /> " + y.Text + "</div>");

                        //$("#ExpressCentersCheckbox").append( y.Text +"</div>");
                    }

                }, error: function (jqXHR, textStatus, errorThrown) {
                }
            });
        }; 
        var GetExpressCentersFranchise = (zone, branch) => {
            debugger;
            $.ajax({
                async: false,
                type: "POST",
                url: 'RetailDiscount.aspx/GetExpressCentersFranchise',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ zone: zone, branch: branch }),
                success: (rs) => {
                    var kk = '';
                    var rr = rs.d;
                    for (let y of rr) {
                        // $("#ExpressCentersCheckbox").append("<div class='form-inline'> ");
                        //$("#ExpressCentersCheckbox").append("<label for='chk_" + 55 + "'>" + y.Text + "</label><input id='chk_" + y.Value + "' type='checkbox' value='" + y.Text + "' />");
                        $("#ExpressCentersCheckbox").append("<div style='background-color: aqua;height:72%' class='col-sm-3'>  <input id = '" + y.Value + "' type = 'checkbox' value='" + y.Text + "' name='ECs' class='Franchise ml-4  ,,," + y.branch + ",,," + y.zone + "' /> " + y.Text + "</div>");

                        //$("#ExpressCentersCheckbox").append( y.Text +"</div>");
                    }

                }, error: function (jqXHR, textStatus, errorThrown) {
                }
            });
        };
        var GetExpressCentersFranchiseBranch = (zone, branch) => {
            debugger;
            $.ajax({
                async: false,
                type: "POST",
                url: 'RetailDiscount.aspx/GetExpressCentersFranchiseBranch',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ zone: zone, branch: branch }),
                success: (rs) => {
                    var kk = '';
                    var rr = rs.d;
                    for (let y of rr) {
                        // $("#ExpressCentersCheckbox").append("<div class='form-inline'> ");
                        //$("#ExpressCentersCheckbox").append("<label for='chk_" + 55 + "'>" + y.Text + "</label><input id='chk_" + y.Value + "' type='checkbox' value='" + y.Text + "' />");
                        $("#ExpressCentersCheckbox").append("<div style='background-color: deepskyblue;height:72%' class='col-sm-3'>  <input id = '" + y.Value + "' type = 'checkbox' value='" + y.Text + "' name='ECs' class='FranchiseBranch ml-4  ,,," + y.branch + ",,," + y.zone + "' /> " + y.Text + "</div>");

                        //$("#ExpressCentersCheckbox").append( y.Text +"</div>");
                    }

                }, error: function (jqXHR, textStatus, errorThrown) {
                }
            });
        };

        var GetServiceTypes = () => {
            debugger;
            $.ajax({
                type: "POST",
                url: 'RetailDiscount.aspx/GetServiceTypes',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: (rs) => {
                    var kk = '';
                    var rr = rs.d;
                    for (let y of rr) {
                        kk += `<option value="${y.Value}">${y.Text}</option>`;
                    }
                    $('#serviceTypeDropdown').html(kk);
                }, error: function (jqXHR, textStatus, errorThrown) {
                }
            });
        };

        function CheckDiscount() {
            debugger;
            var Id = document.getElementById('DiscountType').value;
            if (Id == 1) {
                document.getElementById("CheckDiscountType").innerHTML = `<input type="number" class="form-control" id="discountValuePercentage" name="discountValuePercentage" onInput="return check(event, value)" min="0" max="100" step="0.01" style="font-size: 12px; " />`;
                
            } else {
                document.getElementById("CheckDiscountType").innerHTML = ` <input type="text" class="form-control" id="discountValueAmount" name="discountValueAmount" onkeypress="return isNumberKeyWithDecimal(event)" maxlength="4" style="font-size:12px;" />`;

            }

        }

        function ZoneChange() {
            debugger;
            var Id = document.getElementById('zonesDropdown').value;
            document.getElementById("branchCheckBoxAll").checked = false;
            getBranches(Id);
            $('#AllExpressCenters').prop('checked', false);
        }

        function BranchChange() {
            debugger;
            var ZoneId = document.getElementById('zonesDropdown').value;
            var Id = document.getElementById('branchesDropdown').value;
            GetExpressCenters(ZoneId, Id);
            $('#AllExpressCenters').prop('checked', false);

        }

        var save = (data) => {
            debugger;
            $.ajax({
                type: "POST",
                url: 'RetailDiscount.aspx/SaveDiscountEntry',
                contentType: "application/json; charset=utf-8", 
                dataType: "json",
                data: JSON.stringify({ data: data }),
                success: (rs) => {
                    alert(rs.d);
                    document.getElementById('StatusBox').innerText = rs.d;
                    document.getElementById('form1').reset();                 
                    document.getElementById("CheckDiscountType").innerHTML = `<input type="number" class="form-control" id="discountValuePercentage" name="discountValuePercentage" onInput="return check(event, value)" min="0" max="100" step="0.01" style="font-size: 12px; " />`;

                    var day = new Date().getDate();
                    var month = new Date().getMonth() + 1;
                    var year = new Date().getFullYear();
                    var dateFull = year + "-" + month + "-" + day;
                    document.getElementById('<%=Fromdate.ClientID %>').value = dateFull;
                    document.getElementById('<%=Todate.ClientID %>').value = dateFull;

                }
            });
        };

        var EditDatabase = (data) => {
            debugger;
            $.ajax({
                type: "POST",
                url: 'RetailDiscount.aspx/EditDiscountEntry',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ data: data }),
                success: (rs) => {
                    alert(rs.d);
                    document.getElementById('form1').reset();
                    document.getElementById("CheckDiscountType").innerHTML = `<input type="number" class="form-control" id="discountValuePercentage" name="discountValuePercentage" onInput="return check(event, value)" min="0" max="100" step="0.01" style="font-size: 12px; " />`;


                    var day = new Date().getDate();
                    var month = new Date().getMonth() + 1;
                    var year = new Date().getFullYear();
                    var dateFull = year + "-" + month + "-" + day;
                    document.getElementById('<%=Fromdate.ClientID %>').value = dateFull;
                    document.getElementById('<%=Todate.ClientID %>').value = dateFull;

                }
            });
        };

        $(function () {
            debugger;
            var day = new Date().getDate();
            var month = new Date().getMonth()+1;
            var year = new Date().getFullYear();
            var dateFull = year + "-" + month + "-"+day;
            document.getElementById('<%=Fromdate.ClientID %>').value = dateFull;
            document.getElementById('<%=Todate.ClientID %>').value = dateFull;



            getZones();
           // GetServiceTypes();
        });

        $('body').on('click', '#btnActivate', function () {
            debugger;
            var groupId = document.getElementById("GroupId").value;
          
            if (groupId != "") {
                var decision;
                if (window.confirm('Ok to Approve, Cancel to Reject.')) {
                    decision = 1;
                } else {
                    decision = 0;
                    //$("#btnActivate").prop("disabled", true);
                    //$("#btnTempSave").prop("disabled", true);
                }
                $.ajax({
                    async: false,
                    type: "POST",
                    url: 'RetailDiscount.aspx/ActivateDiscount',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ groupId: groupId, decision: decision }),
                    success: (rs) => {
                        alert(rs.d);

                        $("#btnActivate").prop("disabled", true);
                        $("#btnTempSave").prop("disabled", true);
                    }
                });
            }

        });

        $('body').on('click', '#btnCancelSave', function () {

            document.getElementById('form1').reset(); 
            var day = new Date().getDate();
            var month = new Date().getMonth() + 1;
            var year = new Date().getFullYear();
            var dateFull = year + "-" + month + "-" + day;
            document.getElementById('<%=Fromdate.ClientID %>').value = dateFull;
            document.getElementById('<%=Todate.ClientID %>').value = dateFull;

            getZones();

        });

        $('body').on('click', '#btnTempSave', function () {
            debugger;
            $("#btnActivate").prop("disabled", true);

            
            var fromDateFormatted = document.getElementById('<%=Fromdate.ClientID %>').value;
            var fromDate = document.getElementById('<%=Fromdate.ClientID %>').value;
     
            fromDate = fromDate.split('-');
            var newFromDate = fromDate[0] + '-' + fromDate[1] + '-' + fromDate[2];
            var fromDateObject = new Date(fromDate);

            var toDateFormatted = document.getElementById('<%=Todate.ClientID %>').value;
            var toDate = document.getElementById('<%=Todate.ClientID %>').value;
            toDate = toDate.split('-');
            var newToDate = toDate[0] + '-' + toDate[1] + '-' + toDate[2];
            var toDateObject = new Date(toDate);


            var ExpressCenterss = [];
            var ExpressCenterssCodes = [];
            var ExpressCenterCheckbox;
            var branchOfEC = [];
            var zoneOfEC = [];
            var temp;
            $.each($("input[name='ECs']:checked"), function () {
                ExpressCenterss.push($(this).val());
                temp = $(this).attr('class');
                ExpressCenterssCodes.push($(this).attr('id'));
                
                branchOfEC.push(temp.split(',,,')[1])
                zoneOfEC.push(temp.split(',,,')[2]);

            });
            ExpressCenterss = ExpressCenterss.join(", ");
            if (document.getElementById("AllExpressCenters").checked) {
                ExpressCenterCheckbox = "All";
            }

            var zone = document.getElementById("zonesDropdown").value;
            var branch = document.getElementById("branchesDropdown").value;
            var zoneAllCheckbox;
            if (zone == 'All') {
                getAllZones();
                zoneAllCheckbox = "All";
            } else {
                AllzoneName = jQuery("#zonesDropdown :selected").text();
                AllZoneCode = document.getElementById('zonesDropdown').value;
            }
            var branchAllCheckbox;

            if (branch == 'All') {
                getAllBranches(zone);
                branchAllCheckbox = "All";
            } else {
                AllBranchName = jQuery("#branchesDropdown :selected").text();
                AllBranchCode = document.getElementById('branchesDropdown').value;
            }
            var serviceType = document.getElementById("serviceTypeDropdown").value;
            var discountTypeName = jQuery("#DiscountType :selected").text();
            var DiscountTypeId = document.getElementById('DiscountType').value;
            var discountValue;
            if (DiscountTypeId == 1) {
                discountValue = parseFloat(document.getElementById("discountValuePercentage").value);
            } else {
                discountValue = parseFloat(document.getElementById("discountValueAmount").value);
            }
            var shortDescription = document.getElementById("shortDescription").value;
            var longDescription = document.getElementById("LongDescription").value;
            var minShipment = parseFloat(document.getElementById("MinShipment").value);
            var maxShipment = parseFloat(document.getElementById("MaxShipment").value);
            var minShipmentWeight = parseFloat(document.getElementById("MinShipmentWeight").value);
            var maxShipmentWeight = parseFloat(document.getElementById("MaxShipmentWeight").value);
            var discountId = document.getElementById("DiscountId").value;
            var groupId = document.getElementById("GroupId").value;
            var status = true;
            var prevDataValidations = true;
            var DiscountTypeRadio = '';
            if ($('#OneTimeDiscountRadio').is(':checked')) {
                if (discountId == "") {
                    alert("Please provide phone number");
                    return;
                }
                DiscountTypeRadio = 'Onetime';
            } else {
                DiscountTypeRadio = 'Normal';
            }

            

            ////////////////validations start here

            if (ExpressCenterss.length <= 0) {
                checkboxes = document.getElementsByName('ECs');
                for (var i = 0, n = checkboxes.length; i < n; i++) {
                    checkboxes[i].style.outline = "2px solid red";
                }

                alert('Please select minimum 1 express center');
                return;
            } else {
                checkboxes = document.getElementsByName('ECs');
                for (var i = 0, n = checkboxes.length; i < n; i++) {
                    checkboxes[i].style.removeProperty('outline');
                }
            }

            if (fromDate == "") {
                document.getElementById('<%=Fromdate.ClientID %>').style.borderColor = 'red';
                alert('Please provide valid start date');
                return;
            } else {
                document.getElementById('<%=Fromdate.ClientID %>').style.removeProperty('border');
            }
            if (toDate == "") {
                document.getElementById('<%=Todate.ClientID %>').style.borderColor = 'red';
                alert('Please provide valid end date');
                return;
            } else {
                document.getElementById('<%=Todate.ClientID %>').style.removeProperty('border');
            }
            debugger;

            var tempDate = new Date();
            var CurrentDate = new Date(tempDate.getUTCFullYear(), tempDate.getUTCMonth(), tempDate.getUTCDate());
            if (prevDataValidations) {
                if (fromDateObject < CurrentDate) {
                    document.getElementById('<%=Fromdate.ClientID %>').style.borderColor = 'red';
                    document.getElementById('<%=Todate.ClientID %>').style.borderColor = 'red';
                    alert('Starting date cannot be previous days');
                    return;
                } else {
                    document.getElementById('<%=Fromdate.ClientID %>').style.removeProperty('border');
                    document.getElementById('<%=Todate.ClientID %>').style.removeProperty('border');
                }
            }

            debugger;
            if (toDateObject < fromDateObject) {
                document.getElementById('<%=Fromdate.ClientID %>').style.borderColor = 'red';
                document.getElementById('<%=Todate.ClientID %>').style.borderColor = 'red';
                alert('Please provide valid start and end date');
                return;
            } else {

                document.getElementById('<%=Fromdate.ClientID %>').style.removeProperty('border');
                document.getElementById('<%=Todate.ClientID %>').style.removeProperty('border');
            }
            if (DiscountTypeId == 1) {
                if (discountValue < 1 || isNaN(discountValue) || discountValue > 100) {

                    document.getElementById("discountValuePercentage").style.borderColor = 'red';
                    alert('Please provide valid discount percentage');
                    return;
                } else{
                    document.getElementById("discountValuePercentage").style.removeProperty('border');

                }
            } else if (DiscountTypeId==2) {
                if (discountValue <= 0 || isNaN(discountValue) || discountValue > 10000) {

                    document.getElementById("discountValueAmount").style.borderColor = 'red';
                    alert('Please provide valid discount amount');
                    return;
                } else {
                    document.getElementById("discountValueAmount").style.removeProperty('border');
                }
            }
           
            if (document.getElementById('shortDescription').value.length <= 0) {
                document.getElementById("shortDescription").style.borderColor = 'red';
                alert('Please provide short description');
                return;
            } else {
                document.getElementById("shortDescription").style.removeProperty('border');

            }
            if (document.getElementById('LongDescription').value.length <= 0) {
                document.getElementById("LongDescription").style.borderColor = 'red';
                alert('Please provide long description');
                return;
            } else {
                document.getElementById("LongDescription").style.removeProperty('border');

            }
            if (minShipment < 0 || isNaN(minShipment) || minShipment == "") {
                document.getElementById("MinShipment").style.borderColor = 'red';
                alert('Please provide valid minimum shipment count');
                return;
            } else {
                document.getElementById("MinShipment").style.removeProperty('border');

            }
            if (maxShipment <= 0 || isNaN(maxShipment) || maxShipment == "") {
                document.getElementById("MaxShipment").style.borderColor = 'red';
                alert('Please provide valid maximum shipment count');
                return;
            } else {
                document.getElementById("MaxShipment").style.removeProperty('border');

            }
            if (parseInt(minShipment) > parseInt(maxShipment)) {
                document.getElementById("MinShipment").style.borderColor = 'red';
                document.getElementById("MaxShipment").style.borderColor = 'red';
                alert('Please provide valid minimum and maximum shipment count');
                return;
            } else {
                document.getElementById("MinShipment").style.removeProperty('border');
                document.getElementById("MaxShipment").style.removeProperty('border');

            }
             
            if (minShipmentWeight < -1 || isNaN(minShipmentWeight)  || minShipmentWeight > 1000) {
                document.getElementById("MinShipmentWeight").style.borderColor = 'red';
                alert('Please provide valid minimum shipment weight');
                return;
            } else {
                document.getElementById("MinShipmentWeight").style.removeProperty('border');

            }
            if (maxShipmentWeight <= 0 || isNaN(maxShipmentWeight) || maxShipmentWeight > 1000) {
                document.getElementById("MaxShipmentWeight").style.borderColor = 'red';
                alert('Please provide valid maximum shipment weight');
                return;
            } else {
                document.getElementById("MaxShipmentWeight").style.removeProperty('border');

            }
            if (parseFloat(minShipmentWeight) > parseFloat(maxShipmentWeight)) {
                document.getElementById("MinShipmentWeight").style.borderColor = 'red';
                document.getElementById("MaxShipmentWeight").style.borderColor = 'red';
                alert('Please provide valid minimum and maximum shipment weight');
                return;
            } else {
                document.getElementById("MinShipmentWeight").style.removeProperty('border');
                document.getElementById("MaxShipmentWeight").style.removeProperty('border');

            }
           
            ExpressCenterssCodes = ExpressCenterssCodes.toString();
            var data = {
                fromDate: fromDateFormatted,
                toDate: toDateFormatted,
                zone: AllZoneCode,
                Branch: AllBranchCode,
                ExpressCenter: ExpressCenterssCodes,
                serviceType: serviceType,
                discountType: DiscountTypeId,
                discountValue: discountValue,
                shortDescription: null,
                longDescription: null,
                minShipment: minShipment,
                maxShipment: maxShipment,
                minShipmentWeight: minShipmentWeight,
                maxShipmentWeight: maxShipmentWeight,
                specialDiscountId: discountId,
                parentGroupId: null,
                prevDataValidations: prevDataValidations
            };
            var dateInvalid = true;
            var is_DataSame = true;
            var is_SpecialIdUsed = true;

            //if (groupId == "" || isNaN(groupId)) {
            //    if (discountId == "" || discountId == null) {

            //    } else {
            //        $.ajax({
            //            async: false,
            //            type: "POST",
            //            url: 'RetailDiscount.aspx/isSpecialIdUsed',
            //            contentType: "application/json; charset=utf-8",
            //            dataType: "json",
            //            data: JSON.stringify({ specialId: discountId }),
            //            success: (rs) => {
            //                debugger;
            //                if (rs.d == "true") {
            //                } else {
            //                    status = false;
            //                }

            //            }
            //        });
            //    }
            //}
            //else {
            //    //update previous record
            //    prevDataValidations = false;

            //}
            //if (!status) {
            //    document.getElementById("DiscountId").style.borderColor = 'red';
            //    alert('Special discount id exists');
            //    return;
            //}  

            if (groupId == "" || isNaN(groupId)) {
                    $.ajax({
                        async: false,
                        type: "POST",
                        url: 'RetailDiscount.aspx/isDicountEntrySame_AndSpecialDiscountCheck',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify({ data: data }),
                        success: (rs) => {
                            debugger;
                            if (rs.d == "false") {
                                is_DataSame = false;
                            } else if (rs.d =="date") {
                                dateInvalid = false;
                            } else if (rs.d =="SpecialUsed") {
                                is_SpecialIdUsed = "false";
                            }
                            else {
                                is_DataSame = true;
                            }

                        }
                    });
            }
            else {

                is_DataSame = true;
            }
            if (!is_DataSame) {
                alert('Same discount exists in this express center!');
                return;
            }
            if (!dateInvalid) {
                alert('Starting date cannot be previous date!');
                return;
            }
            if (!dateInvalid) {
                alert('Starting date cannot be previous date!');
                return;
            }
            if (!is_SpecialIdUsed) {
                document.getElementById("DiscountId").style.borderColor = 'red';
                alert('Special discount id exists');
                return;
            }

            $("#btnSubmitSave").prop("disabled", false);


            var rows = '';
            rows += `<tr >
                            <td class="DiscountTypeRadio" style="display:none;">${DiscountTypeRadio}</td>
                            <td class="parentGroupId" style="display:none;">${groupId}</td>
                            <td class="fromDateTd">${newFromDate}</td> 
                            <td class="toDateTd">${newToDate}</td> 
                            <td >${AllzoneName}</td> 
                            <td class="allZoneCodeTd" style="display:none;">${AllZoneCode}</td>
                            <td class="allZoneCheckboxTd" style="display:none;">${zoneAllCheckbox}</td>

                            <td >${AllBranchName}</td> 
                            <td class="allBranchCodeTd" style="display:none;">${AllBranchCode}</td>
                            <td class="allbranchCheckboxTd" style="display:none;">${branchAllCheckbox}</td>

                            <td >${ExpressCenterss}</td> 
                            <td class="allExpressCodeTd" style="display:none;">${ExpressCenterssCodes}</td>
                            <td class="allExpressCodeCheckboxTd" style="display:none;">${ExpressCenterCheckbox}</td>
                            <td class="allbranchesExtra" style="display:none;">${branchOfEC}</td>
                            <td class="allZonesExtra" style="display:none;">${zoneOfEC}</td>

                            <td class="serviceTypeTd" >${serviceType}</td> 
                            <td  >${discountTypeName}</td> 
                            <td class="discountTypeIdTd" style="display:none;">${DiscountTypeId}</td>                            
                            <td class="discountValueTd" >${discountValue}</td> 
                            <td class="shortDescriptionTd" >${shortDescription}</td> 
                            <td class="longDescriptionTd" >${longDescription}</td> 
                            <td class="minShipmentCountTd">${minShipment}</td> 
                            <td class="maxShipmentCountTd">${maxShipment}</td> 
                            <td class="minShipmentWeightTd">${minShipmentWeight}</td> 
                            <td class="maxShipmentWeightTd">${maxShipmentWeight}</td> 
                            <td class="specialDiscountId" >${discountId}</td> 
                            <td><input type="button" value="Edit" onclick="EditRecord()"  class="form-control mt-2" style="font-size:12px;background-color: #f27031;color: white;" /></td>
                    </tr>`;
            $('#DiscountTempSave').html(rows);


            $("#btnTempSave").prop("disabled", true);

            //Reset form
            document.getElementById('form1').reset(); 

            var day = new Date().getDate();
            var month = new Date().getMonth() + 1;
            var year = new Date().getFullYear();
            var dateFull = year + "-" + month + "-" + day ;
            document.getElementById('<%=Fromdate.ClientID %>').value = dateFull;
            document.getElementById('<%=Todate.ClientID %>').value = dateFull;

            document.getElementById("DiscountTable").style.display =  'block';
           // document.getElementById("CheckDiscountType").innerHTML = `<input type="number" class="form-control" id="discountValuePercentage" name="discountValuePercentage" onInput="return check(event, value)" min="0" max="100" step="0.01" style="font-size: 12px; " />`;

            getZones();
           // GetServiceTypes();

        });

        function EditRecord() {

            debugger;
            $("#btnSubmitSave").prop("disabled", true);
            var DiscountType = $("#DiscountTempSave").find(".DiscountTypeRadio").html();

            var fromDateTd = $("#DiscountTempSave").find(".fromDateTd").html();
            var toDateTd = $("#DiscountTempSave").find(".toDateTd").html();
            var ZoneCodeTd = $("#DiscountTempSave").find(".allZoneCodeTd").html();
            var ZoneCodeCheckboxTd = $("#DiscountTempSave").find(".allZoneCheckboxTd").html();
            var branchCodeTd = $("#DiscountTempSave").find(".allBranchCodeTd").html();
            var branchCodeCheckboxTd = $("#DiscountTempSave").find(".allbranchCheckboxTd").html();
            var expressCenterCodeTd = $("#DiscountTempSave").find(".allExpressCodeTd").html(); 
            var expressCenterCodeCheckboxTd = $("#DiscountTempSave").find(".allExpressCodeCheckboxTd").html();
            var serviceTypeTd = $("#DiscountTempSave").find(".serviceTypeTd").html();
            var discountValueTd = $("#DiscountTempSave").find(".discountValueTd").html();
            var shortDescriptionTd = $("#DiscountTempSave").find(".shortDescriptionTd").html();
            var longDescriptionTd = $("#DiscountTempSave").find(".longDescriptionTd").html();
            var minShipmentCountTd = $("#DiscountTempSave").find(".minShipmentCountTd").html();
            var maxShipmentCountTd = $("#DiscountTempSave").find(".maxShipmentCountTd").html();
            var minShipmentWeightTd = $("#DiscountTempSave").find(".minShipmentWeightTd").html();
            var maxShipmentWeightTd = $("#DiscountTempSave").find(".maxShipmentWeightTd").html();
            var specialDiscountId = $("#DiscountTempSave").find(".specialDiscountId").html();
            var discounttypeIdTd = $("#DiscountTempSave").find(".discountTypeIdTd").html();
            var parentGroupId = $("#DiscountTempSave").find(".parentGroupId").html();


            if (discounttypeIdTd == 1) {
                document.getElementById("CheckDiscountType").innerHTML = `<input type="number" class="form-control" id="discountValuePercentage" name="discountValuePercentage" onInput="return check(event, value)" min="0" max="100" step="0.01" style="font-size: 12px; " />`;
                document.getElementById("DiscountType").value = discounttypeIdTd
                document.getElementById("discountValuePercentage").value = parseFloat(discountValueTd);
            } else {
                document.getElementById("CheckDiscountType").innerHTML = ` <input type="text" class="form-control" id="discountValueAmount" name="discountValueAmount" onkeypress="return isNumberKeyWithDecimal(event)" maxlength="4" style="font-size:12px;" />`;
                document.getElementById("DiscountType").value = discounttypeIdTd
                document.getElementById("discountValueAmount").value = parseFloat(discountValueTd);
            }
            document.getElementById('serviceTypeDropdown').value = serviceTypeTd;
            document.getElementById("shortDescription").value = shortDescriptionTd;
            document.getElementById("LongDescription").value = longDescriptionTd;
            document.getElementById("MinShipment").value = minShipmentCountTd;
            document.getElementById("MaxShipment").value = maxShipmentCountTd;
            document.getElementById("MinShipmentWeight").value = minShipmentWeightTd;
            document.getElementById("MaxShipmentWeight").value = maxShipmentWeightTd;
            document.getElementById("DiscountId").value = specialDiscountId;
            document.getElementById("GroupId").value = parentGroupId;
            fromDateTd = fromDateTd.split('-');
            var newFromDate = fromDateTd[0] + '-' + fromDateTd[1] + '-' + fromDateTd[2];
            document.getElementById('<%=Fromdate.ClientID %>').value = newFromDate;
            toDateTd = toDateTd.split('-');
            var newToDate = toDateTd[0] + '-' + toDateTd[1] + '-' + toDateTd[2];
            document.getElementById('<%=Todate.ClientID %>').value = newToDate;

            if (ZoneCodeCheckboxTd == "All") {
                $('#zoneAll').prop('checked', true);
                //getBranches("All");
                var option = $('<option></option>').attr("value", "All").text("All");
                $("#zonesDropdown").empty().append(option);
                $('#branchCheckBoxAll').prop('checked', false);
                $('#AllExpressCenters').prop('checked', false);
            } else {
                document.getElementById('zonesDropdown').value = ZoneCodeTd;
                getBranches(ZoneCodeTd);

            }
            if (branchCodeCheckboxTd == "All") {
                $('#branchCheckBoxAll').prop('checked', true);
                var option = $('<option></option>').attr("value", "All").text("All");
                $("#branchesDropdown").empty().append(option);
                var ZoneId = document.getElementById('zonesDropdown').value;
                var Id = document.getElementById('branchesDropdown').value;
                GetExpressCenters(ZoneId, Id);
                $('#AllExpressCenters').prop('checked', false);
            } else {
                document.getElementById('branchesDropdown').value = branchCodeTd;
                var ZoneId = document.getElementById('zonesDropdown').value;
                var Id = document.getElementById('branchesDropdown').value;
                GetExpressCenters(ZoneId, Id);
            }
            if (expressCenterCodeCheckboxTd == "All") {
                $('#AllExpressCenters').prop('checked', true);

                expressCenterCodeTd = expressCenterCodeTd.split(",");
                for (var k = 0; k < expressCenterCodeTd.length; k++) {
                    var kkk = expressCenterCodeTd[k];
                    $('#' + kkk).prop('checked', true);
                }
            }
            else {
                debugger;
                $('#AllExpressCenters').prop('checked', false);
                expressCenterCodeTd = expressCenterCodeTd.split(",");
                for (var k = 0; k < expressCenterCodeTd.length;k++) {
                    var kkk = expressCenterCodeTd[k];
                    $('#'+kkk).prop('checked', true);
                }
            }

            if (DiscountType == "Onetime") {
                toggeDiscountOnetime();
            } else {
                toggeDiscountNormal();
            }

            document.getElementById("DiscountTable").style.display = 'none';
            $("#btnTempSave").prop("disabled", false);

        }

        $('body').on('click', '#btnSubmitSave', function () {
            debugger;

            /////////////////////////////////////
            $("#btnSubmitSave").prop("disabled", true);
            var fromDateTd = $("#DiscountTempSave").find(".fromDateTd").html();
            var toDateTd = $("#DiscountTempSave").find(".toDateTd").html();
            var ZoneCodeTd = $("#DiscountTempSave").find(".allZoneCodeTd").html();
            var ZoneCodeCheckboxTd = $("#DiscountTempSave").find(".allZoneCheckboxTd").html();
            var branchCodeTd = $("#DiscountTempSave").find(".allBranchCodeTd").html();
            var branchCodeCheckboxTd = $("#DiscountTempSave").find(".allbranchCheckboxTd").html();
            var expressCenterCodeTd = $("#DiscountTempSave").find(".allExpressCodeTd").html();
            var allBranchCodeExtraTd = $("#DiscountTempSave").find(".allbranchesExtra").html();
            var allZoneExtraTd = $("#DiscountTempSave").find(".allZonesExtra").html();

            var expressCenterCodeCheckboxTd = $("#DiscountTempSave").find(".allExpressCodeCheckboxTd").html();
            var serviceTypeTd = $("#DiscountTempSave").find(".serviceTypeTd").html();
            var discountValueTd = $("#DiscountTempSave").find(".discountValueTd").html();
            var shortDescriptionTd = $("#DiscountTempSave").find(".shortDescriptionTd").html();
            var longDescriptionTd = $("#DiscountTempSave").find(".longDescriptionTd").html();
            var minShipmentCountTd = $("#DiscountTempSave").find(".minShipmentCountTd").html();
            var maxShipmentCountTd = $("#DiscountTempSave").find(".maxShipmentCountTd").html();
            var minShipmentWeightTd = $("#DiscountTempSave").find(".minShipmentWeightTd").html();
            var maxShipmentWeightTd = $("#DiscountTempSave").find(".maxShipmentWeightTd").html();
            var specialDiscountId = $("#DiscountTempSave").find(".specialDiscountId").html();
            var discounttypeIdTd = $("#DiscountTempSave").find(".discountTypeIdTd").html();
            var parentGroupId = $("#DiscountTempSave").find(".parentGroupId").html();

            document.getElementById("DiscountTable").style.display = 'none';
            var data = {
                fromDate: fromDateTd,
                toDate: toDateTd,
                zone: allZoneExtraTd,
                Branch: allBranchCodeExtraTd,
                ExpressCenter: expressCenterCodeTd,
                serviceType: serviceTypeTd,
                discountType: discounttypeIdTd,
                discountValue: discountValueTd,
                shortDescription: shortDescriptionTd,
                longDescription: longDescriptionTd,
                minShipment: minShipmentCountTd,
                maxShipment: maxShipmentCountTd,
                minShipmentWeight: minShipmentWeightTd,
                maxShipmentWeight: maxShipmentWeightTd,
                specialDiscountId: specialDiscountId,
                parentGroupId: parentGroupId
            };
            if (parentGroupId != "" || isNaN(parentGroupId)) {
                EditDatabase(data);
                document.getElementById('StatusBox').innerText = "Discount is saved";
            }
            else {
                save(data);
                document.getElementById('StatusBox').innerText = "Discount is saved";
            }

            $("#btnTempSave").prop("disabled", false);

        });

    </script>
</asp:Content>
