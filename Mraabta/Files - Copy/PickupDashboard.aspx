<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PickupDashboard.aspx.cs" MasterPageFile="~/BtsMasterPage2.Master" Inherits="MRaabta.Files.PickupDashboard" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<script src="../assets/bootstrap-4.3.1-dist/js/bootstrap.min.js"></script>
    <link href="../assets/bootstrap-4.3.1-dist/css/bootstrap.min.css" rel="stylesheet" />--%>




    <!-- MDBootstrap Simple Charts  -->
    <%--<link href="css/addons-pro/simple-charts.min.css" rel="stylesheet">--%>
    <link href="https://fonts.googleapis.com/css?family=Lato&display=swap" rel="stylesheet">
    <!-- MDBootstrap Simple Charts  -->
    <%--<script type="text/javascript" src="js/addons-pro/simple-charts.min.js"></script>--%>


    <style>
        body {
            font-family: Arial, Helvetica, sans-serif;
        }

        .tab-content > .tab-pane {
            display: block;
            height: 0;
            overflow: hidden;
        }

            .tab-content > .tab-pane.active {
                height: auto;
            }

        .flip-card {
            background-color: transparent;
            width: 30%;
            height: 100px;
            perspective: 1000px;
        }

        .flip-card-inner {
            position: relative;
            width: 100%;
            height: 100%;
            text-align: center;
            transition: transform 0.6s;
            transform-style: preserve-3d;
            box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);
        }

        .flip-card:hover .flip-card-inner {
            transform: rotateY(180deg);
        }

        .flip-card-front, .flip-card-back {
            position: absolute;
            width: 100%;
            height: 100%;
            -webkit-backface-visibility: hidden;
            backface-visibility: hidden;
        }

        .flip-card-front {
            /*background-color: #D66D75;*/
            color: White;
        }

        .flip-card-back {
            background-color: #2980b9;
            color: white;
            transform: rotateY(180deg);
        }

        /*Slider Region*/
        * {
            box-sizing: border-box
        }

        body {
            font-family: Verdana, sans-serif;
            margin: 0
        }
        /* .mySlides {display: none} */
        img {
            vertical-align: middle;
        }

        /* Slideshow container */
        .slideshow-container {
            max-width: 300px;
            height: 100px;
            position: relative;
            margin: auto;
            margin-top: 10px;
        }

        /* Next & previous buttons */
        .prev, .next {
            cursor: pointer;
            position: absolute;
            top: 50%;
            width: auto;
            padding: 16px;
            margin-top: -22px;
            color: white;
            font-weight: bold;
            font-size: 18px;
            transition: 0.6s ease;
            border-radius: 0 3px 3px 0;
            user-select: none;
        }

        /* Position the "next button" to the right */
        .next {
            right: 0;
            border-radius: 3px 0 0 3px;
        }

            /* On hover, add a black background color with a little bit see-through */
            .prev:hover, .next:hover {
                background-color: rgba(0,0,0,0.8);
            }

        /* Caption text */
        .text {
            color: #f2f2f2;
            font-size: 15px;
            padding: 8px 12px;
            position: absolute;
            bottom: 8px;
            width: 100%;
            text-align: center;
        }

        /* Number text (1/3 etc) */
        .numbertext {
            color: #f2f2f2;
            font-size: 12px;
            padding: 8px 12px;
            position: absolute;
            top: 0;
        }

        /* The dots/bullets/indicators */
        .dot {
            cursor: pointer;
            height: 15px;
            width: 15px;
            margin: 0 2px;
            background-color: #bbb;
            border-radius: 50%;
            display: inline-block;
            transition: background-color 0.6s ease;
        }

            .active, .dot:hover {
                background-color: #717171;
            }

        /* Fading animation */
        .fade {
            -webkit-animation-name: fade;
            -webkit-animation-duration: 1.5s;
            animation-name: fade;
            animation-duration: 1.5s;
        }

        @-webkit-keyframes fade {
            from {
                opacity: .4
            }

            to {
                opacity: 1
            }
        }

        @keyframes fade {
            from {
                opacity: .4
            }

            to {
                opacity: 1
            }
        }

        /* On smaller screens, decrease text size */
        @media only screen and (max-width: 300px) {
            .prev, .next, .text {
                font-size: 11px
            }
        }
        /* Image Slidershow */
        * {
            box-sizing: border-box
        }

        body {
            font-family: Verdana, sans-serif;
            margin: 0
        }

        img {
            vertical-align: middle;
        }

        /* Slideshow container */
        .slideshow-container {
            max-width: 1000px;
            position: relative;
            margin: auto;
        }

        /* Next & previous buttons */
        .prev, .next {
            cursor: pointer;
            position: absolute;
            top: 50%;
            width: auto;
            padding: 16px;
            margin-top: -22px;
            color: white;
            font-weight: bold;
            font-size: 18px;
            transition: 0.6s ease;
            border-radius: 0 3px 3px 0;
            user-select: none;
        }

        /* Position the "next button" to the right */
        .next {
            right: 0;
            border-radius: 3px 0 0 3px;
        }

            /* On hover, add a black background color with a little bit see-through */
            .prev:hover, .next:hover {
                background-color: rgba(0,0,0,0.8);
            }

        /* Caption text */
        .text {
            color: #f2f2f2;
            font-size: 15px;
            padding: 8px 12px;
            position: absolute;
            bottom: 8px;
            width: 100%;
            text-align: center;
        }

        /* Number text (1/3 etc) */
        .numbertext {
            color: #f2f2f2;
            font-size: 12px;
            padding: 8px 12px;
            position: absolute;
            top: 0;
        }

        /* The dots/bullets/indicators */
        .dot {
            cursor: pointer;
            height: 15px;
            width: 15px;
            margin: 0 2px;
            background-color: #bbb;
            border-radius: 50%;
            display: inline-block;
            transition: background-color 0.6s ease;
        }

            .active, .dot:hover {
                background-color: #717171;
            }

        /* Fading animation */
        .fade {
            -webkit-animation-name: fade;
            -webkit-animation-duration: 1.5s;
            animation-name: fade;
            animation-duration: 1.5s;
        }

        @-webkit-keyframes fade {
            from {
                opacity: .4
            }

            to {
                opacity: 1
            }
        }

        @keyframes fade {
            from {
                opacity: .4
            }

            to {
                opacity: 1
            }
        }

        /* On smaller screens, decrease text size */
        @media only screen and (max-width: 300px) {
            .prev, .next, .text {
                font-size: 11px
            }
        }
    </style>
    <script src="https://canvasjs.com/assets/script/jquery-1.11.1.min.js"></script>
    <script src="https://canvasjs.com/assets/script/jquery.canvasjs.min.js"></script>
    <%-- <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.9.3/Chart.bundle.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.9.3/Chart.bundle.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.9.3/Chart.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.9.3/Chart.min.js"></script>--%>

    <script src="https://canvasjs.com/assets/script/canvasjs.min.js"></script>



    <div style="display: flex; flex-wrap: wrap;">
        <%-- Flip Cards --%>
        <div class="col-md-6 ">
            <div class="card " style="width: 100%;">
                <div class="card mt-1" style="margin-left: 3px;">
                    <div class="card-header" style="font-size: 20px; font-weight: bolder; font-family: 'Cambria Math'; background-color: #424242;">
                        <span class="ml-1" style="color: white;">User Statistics</span>
                    </div>
                    <div class="m-1 ">
                        <div style="margin-left: 1px; display: flex; flex-wrap: wrap;">
                            <div class="col-md-3 " style="width: 20%; margin-left: 25px;">
                                <span style="font-size: 1vw; font-weight: bolder; font-family: Lato;">Total Users</span><br />
                                <div class="row">
                                    <img src="../images/yellow.jpg" style="height: auto; width: 40%; margin-top: 4px; object-fit: cover; margin-left: 10px;" /><span id="RegisteredUsers" style="font-size: 2vw; text-align: center; font-family: Lato;"></span>
                                </div>
                            </div>
                            <div class="col-md-3" style="width: 20%; margin-left: 25px;">
                                <span style="font-size: 1vw; font-weight: bolder; font-family: Lato;">Active Users</span><br />
                                <div class="row">
                                    <img src="../images/green02.png" style="height: auto; width: 40%; margin-top: 4px; object-fit: cover; margin-left: 10px;" /><span id="ActiveUsers" style="font-size: 2vw; text-align: center; font-family: Lato;"></span>
                                </div>
                            </div>
                            <div class="col-md-3" style="width: 20%; margin-left: 25px;">
                                <span style="font-size: 1vw; font-weight: bolder; font-family: Lato;">In-Active Users</span><br />
                                <div class="row">
                                    <img src="../images/red.png" style="height: auto; width: 40%; margin-top: 4px; object-fit: cover; margin-left: 10px;" /><span id="InActiveUsers" style="font-size: 2vw; text-align: center; font-family: Lato;"></span>
                                </div>
                            </div>


                        </div>
                    </div>

                </div>
            </div>
            <div class="card mt-2" style="width: 100%;">
                <div class="card mt-2" style="margin-left: 3px;">
                    <div class="card-header" style="font-size: 20px; font-weight: bolder; font-family: 'Cambria Math'; background-color: #424242;">
                        <span class="ml-1" style="color: white;">Today's Pickup Statistics</span>
                    </div>
                    <div class="m-1 ">
                        <div style="margin-left: 1px; display: flex; flex-wrap: wrap;">
                            <div class="col-md-2 " style="width: 20%; margin-left: 10px;">
                                <span style="font-size: 1vw; font-weight: bolder; font-family: Lato;">Requests</span><br />
                                <div class="row">
                                    <img src="../images/reservation.png" style="height: auto; width: 35%; margin-top: 4px; object-fit: cover; margin-left: 10px;" /><span id="TodayRequest" style="font-size: 2vw; text-align: center; font-family: Lato;"></span>
                                </div>
                            </div>
                            <div class="col-md-2" style="width: 20%; margin-left: 10px;">
                                <span style="font-size: 1vw; font-weight: bolder; font-family: Lato;">Pending</span><br />
                                <div class="row">
                                    <img src="../images/data-pending.png" style="height: auto; width: 35%; margin-top: 4px; object-fit: cover; margin-left: 10px;" /><span id="TodayPending" style="font-size: 2vw; text-align: center; font-family: Lato;"></span>
                                </div>
                            </div>
                            <div class="col-md-2" style="width: 20%; margin-left: 10px;">
                                <span style="font-size: 1vw; font-weight: bolder; font-family: Lato;">Process</span><br />
                                <div class="row">
                                    <img src="../images/scooter.png" style="height: auto; width: 35%; margin-top: 4px; object-fit: cover; margin-left: 10px;" /><span id="TodayProcess" style="font-size: 2vw; text-align: center; font-family: Lato;"></span>
                                </div>
                            </div>
                            <div class="col-md-2" style="width: 20%; margin-left: 10px;">
                                <span style="font-size: 1vw; font-weight: bolder; font-family: Lato;">Cancel</span><br />
                                <div class="row">
                                    <img src="../images/close.png" style="height: auto; width: 30%; margin-top: 4px; object-fit: scale-down; margin-left: 10px;" /><span id="TodayCancel" style="font-size: 2vw; text-align: center; font-family: Lato;"></span>
                                </div>
                            </div>
                            <div style="width: 20%; margin-left: 10px;">
                                <span style="font-size: 1vw; font-weight: bolder; font-family: Lato;">Booked</span><br />
                                <div class="row">
                                    <img src="../images/check-file.png" style="height: auto; width: 28%; margin-top: 4px; object-fit: cover; margin-left: 10px;" /><span id="TodayPerformed" style="font-size: 2vw; text-align: center; font-family: Lato;"></span>
                                </div>
                            </div>

                        </div>
                    </div>

                </div>
            </div>
            <div class="card" style="margin-left: 0.3em; margin-top: 0.9em; width: 100%;">
                <div class="card-header" style="width: 100%; font-size: 20px; font-weight: bolder; font-family: 'Cambria Math'; background-color: #424242;">
                    <span class="ml-2" style="color: white;">Monthly Stats</span>
                </div>
                <div class="card-body">
                    <div id="chartContainer1" style="position: relative; height: 180px; width: 100%;"></div>
                </div>


                <%--<div class="card ml-1" style=" margin-left:0.5em; margin-top:0.9em; width:102%;">
            <div class="card-header" style="width:100%;font-size:20px; font-weight:bolder; font-family:'Cambria Math';background-color:#424242;">
            <span class="ml-2" style="color:white;">Daily Stats</span> 
            </div>
                <div class="card-body">
                <div id="chartContainer" style="position: relative; height:120px; width: 100%;"></div>
                </div>--%>
            </div>
        </div>
        <div class="col-md-6">
            <div class="card">
                <div class="card-header mt-1" style="font-size: 20px; font-weight: bolder; font-family: 'Cambria Math'; background-color: #424242;">
                    <span style="color: white;">Application Usage</span>
                </div>
                <div style="display: flex; flex-wrap: wrap;">
                    <div class="col-md-8">
                        <div id="chartContainer" style="height: 200px; width: 100%;"></div>
                    </div>
                    <div class="col-md-4 mt-5">
                        <div style="font-size: 13px; font-weight: bolder; font-family: 'Cambria Math';">
                            <span>Total Requests:
                                <label id="TotalRequest"></label>
                            </span></br>
                           <span>Booked:
                               <label id="TotalBooked"></label>
                               %</span></br>
                           <span>Cancelled:
                               <label id="TotalCancel"></label>
                               %</span></br>
                           <span>Pending:
                               <label id="TotalPending"></label>
                               %</span></br>
                        </div>
                    </div>

                    <div class="card" style="width: 98%;">
                    </div>
                </div>
            </div>
            <div style="display: flex; flex-wrap: wrap;">
                <div style="width: 100%; height: 70%;" class="mt-2">
                    <div class="card" id="drwconsignments" style="width: 100%;">
                        <div class="card-header" style="width: 100%; font-size: 20px; font-weight: bolder; font-family: 'Cambria Math'; background-color: #424242;">
                            <div style="display: flex; flex-wrap: wrap;">
                                <div class="col-md-9">
                                    <span class="mr-1" style="color: white;">Latest Pickup Requests</span>
                                </div>
                                <%--  <div class="col-md-3">
                                             <button  class="btn btn-success" style="font-size:12px;" onclick="myFunction();">View More</button>    
                                      </div>--%>
                            </div>

                        </div>
                        <table id="tbl_status" style="width: 100%" class="mt-2 mb-2">
                            <tr>
                                <th class="ml-2 mt-3" style="font-size: 12px; margin: 0px; text-align: center;">Status</th>
                                <th class="ml-2" style="font-size: 12px; margin: 0px; text-align: center;">
                                    <input type="radio" style="-webkit-appearance: none; -moz-appearance: none; display: inline-block; width: 16px; height: 16px; padding: 2px; background-clip: content-box; border: 2px solid black; background-color: #ffff00; border-radius: 100%;" checked></input></th>
                                <th class="ml-2" style="font-size: 12px; margin: 0px; text-align: center;">
                                    <input type="radio" style="-webkit-appearance: none; -moz-appearance: none; display: inline-block; width: 16px; height: 16px; padding: 2px; background-clip: content-box; border: 2px solid black; background-color: #43a047; border-radius: 100%;" checked></input></th>
                                <th class="ml-2" style="font-size: 12px; margin: 0px; text-align: center;">
                                    <input type="radio" style="-webkit-appearance: none; -moz-appearance: none; display: inline-block; width: 16px; height: 16px; padding: 2px; background-clip: content-box; border: 2px solid black; background-color: #ff8000; border-radius: 100%;" checked></input></th>
                                <th class="ml-2" style="font-size: 12px; margin: 0px; text-align: center;">
                                    <input type="radio" style="-webkit-appearance: none; -moz-appearance: none; display: inline-block; width: 16px; height: 16px; padding: 2px; background-clip: content-box; border: 2px solid black; background-color: #ff0000; border-radius: 100%;" checked></input></th>

                            </tr>
                            <tr>
                                <td></td>
                                <td class="ml-2" style="font-size: 12px; margin: 0px; text-align: center;">InProcess</td>
                                <td class="ml-2" style="font-size: 12px; margin: 0px; text-align: center;">Booked</td>
                                <td class="ml-2" style="font-size: 12px; margin: 0px; text-align: center;">Pending</td>
                                <td class="ml-2" style="font-size: 12px; margin: 0px; text-align: center;">Cancelled</td>

                            </tr>
                        </table>
                        <table id="table1" style="width: 100%" class="table-bordered">
                            <thead class="thead-dark" style="background-color: #eeeeee">

                                <tr>
                                    <th class="ml-2" style="font-size: 14px; margin: 0px; text-align: center;"></th>
                                    <th class="ml-2" style="font-size: 14px; margin: 0px; text-align: center;">Ticket Number</th>
                                    <th class="ml-2" style="font-size: 14px; margin: 0px; text-align: center;">Consignor</th>
                                    <th class="ml-2" style="font-size: 14px; margin: 0px; text-align: center;">Amount</th>
                                    <th class="ml-2" style="font-size: 14px; margin: 0px; text-align: center;">Pickup Scheduled</th>


                                </tr>
                            </thead>
                            <tbody id="test">
                            </tbody>

                        </table>

                    </div>

                </div>
            </div>

        </div>

        <%-- <div class="flip-card mt-2" style="margin-left:10px;">
                  <div class="flip-card-inner">
                    <div class="flip-card-front" style=" background: linear-gradient(to bottom, #B24592 0%, #F15F79 100%)">
                    <p style="margin-top:1em; text-align:center; font-size:2vw;font-family:'Trebuchet MS', 'Lucida Sans Unicode', 'Lucida Grande', 'Lucida Sans', Arial, sans-serif">Total Pickup Request</p>
                    </div>
                    <div class="flip-card-back">
                       <p style="margin-top:0.5em; text-align:center; font-size:3vw; font-family:'Trebuchet MS', 'Lucida Sans Unicode', 'Lucida Grande', 'Lucida Sans', Arial, sans-serif"" id="TotalRequest"></p>
                    </div>
                  </div>
           </div>--%>
        <div class="col-md-6 ">




            <%-- Slider --%>
        </div>


        <div class="col-md-6">
        </div>
    </div>

    <script type="text/javascript">
        $(document).ready(function () {
            setInterval(function () {
                getRealData()
            }, 1000);//request every x seconds


            //Latest Consignments table scripts
            function getRealData() {
                $.ajax({
                    type: "POST",
                    url: "../PickupReport/GetTodayConsignments",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (rs) {

                        $("#test").remove();

                        $(function () {

                            var tbody = $("<tbody id='test'/>"), tr;
                            //tr.empty();
                            var i = 0;
                            var status = '';
                            $.each(rs, function (_, obj) {
                                //status = rs[i].status;
                                //alert("AAya",status);
                                //i++;
                                //var i = 0;
                                //for (var obj in rs) {
                                //    alert(rs[i].status)
                                //    i++

                                //var status = rs[0].status;
                                //alert(status);

                                tr = $("<tr id='latest_arr' style='font-size:14px; text-align:center; margin:0px; padding:0px; font-family:Lato;'/>");
                                var text = '';
                                for (var i = 0; i <= 4; i++) {
                                    if (i == 0) {
                                        if (obj["Status"] == "Performed") {
                                            text = '<input type="radio" style="  -webkit-appearance: none; -moz-appearance: none; appearance: none;* create custom radiobutton appearance */display: inline-block;width: 16px;height: 16px;padding: 2px;/* background-color only for content */background-clip: content-box;border: 2px solid black;background-color: #43a047; border-radius:100%;" checked></input>';
                                        }
                                        else if (obj["Status"] == "Cancelled") {
                                            text = '<input type="radio" style="-webkit-appearance: none; -moz-appearance: none; appearance: none;* create custom radiobutton appearance */display: inline-block;width: 16px;height: 16px;padding: 2px;/* background-color only for content */background-clip: content-box;border: 2px solid black;background-color: #ef5350; border-radius:100%;" checked></input>';
                                        }
                                        else if (obj["Status"] == "InProcess") {
                                            text = '<input type="radio" style="-webkit-appearance: none; -moz-appearance: none; appearance: none;* create custom radiobutton appearance */display: inline-block;width: 16px;height: 16px;padding: 2px;/* background-color only for content */background-clip: content-box;border: 2px solid black;background-color: #ffff00; border-radius:100%;" checked></input>';
                                        }
                                        else if (obj["Status"] == "Pending") {
                                            text = '<input type="radio" style="-webkit-appearance: none; -moz-appearance: none; appearance: none;* create custom radiobutton appearance */display: inline-block;width: 16px;height: 16px;padding: 2px;/* background-color only for content */background-clip: content-box;border: 2px solid black;background-color: #ff8000; border-radius:100%;" checked></input>';
                                        }
                                    }
                                    else if (i == 1) {
                                        text = obj["ticketNumber"];//'<button id="btnticket" value=' + obj["ticketNumber"] + ' type="button" class="btn btn-link"  >' + obj["ticketNumber"] + '</button>';
                                    }
                                    else if (i == 2) {
                                        text = obj["consigner"];
                                    }
                                    else if (i == 3) {
                                        text = obj["chargedAmount"];
                                    }
                                    else if (i == 4) {
                                        text = obj["scheduledtime"];
                                    }

                                    tr.append("<td class='rotate' style='padding:3px;'>" + text + "</td>")
                                }
                                //$.each(obj, function (_, text) {
                                //    tr.append("<td class='rotate'>" + text + "</td>")
                                //});
                                tr.appendTo(tbody);


                            });
                            tbody.appendTo("#table1");

                            //For Animations 

                        })

                        // $('drwconsignments').html("<table><thead><th>Consignment No</th><th>Ridercode</th><th>Date</th></thead><tbody><td>" + consignmentNumbers + "</td>" + riderCode + "<td>" + Date + "</td></tbody></table>")
                    }
                });
            }

            window.onload = function () {

                $.ajax({
                    type: 'post',
                    url: '../PickupReport/GetPoints',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    cache: false,
                    //data: rs,
                    success: function (rs) {
                        if (rs[0].length == 0) {

                        }
                        else {
                            //debugger;
                            var RegisteredUsers = rs[0].RegisteredUsers;
                            var ActiveUsers = rs[0].ActiveUsers;
                            var InactiveUsers = RegisteredUsers - ActiveUsers;
                            //var TotalPending = rs[0].TotalPending;
                            var TotalRequest = rs[0].TotalRequest;
                            var TodayRequest = rs[0].TodayRequest;
                            var TodayPending = rs[0].TodayPending;
                            var TodayProcess = rs[0].TodayProcess;
                            var TodayCancel = rs[0].TodayCancel;
                            // var totalRunsheets = rs[0].TotalRunsheet;
                            var TodayPerformed = rs[0].TodayPerformed;
                            //TodayPending = TodayRequest - TodayProcess - TodayCancel - TodayPerformed;
                            var MonthlyRequest = rs[0].Request;
                            var MonthlyPending = rs[0].Pending;
                            var MonthlyProcess = rs[0].Process;
                            var MonthlyCancel = rs[0].Cancel;
                            var MonthlyPerformed = rs[0].Booked;
                            MonthlyPending = MonthlyRequest - MonthlyProcess - MonthlyCancel - MonthlyPerformed;
                            var TotalBookings = rs[0].TotalBookings;
                            var TotalCancelled = rs[0].TotalCancelled;
                            var TotalPending = TotalRequest - TotalBookings - TotalCancelled;
                            var Bookingpercent = (TotalBookings / TotalRequest) * 100;
                            Bookingpercent = Bookingpercent.toFixed(2);
                            var Pendingpercent = (TotalPending / TotalRequest) * 100;
                            Pendingpercent = Pendingpercent.toFixed(2);
                            var Cancelpercent = (TotalCancelled / TotalRequest) * 100;
                            Cancelpercent = Cancelpercent.toFixed(2);
                            $('#RegisteredUsers').html(RegisteredUsers);
                            $('#ActiveUsers').html(ActiveUsers);
                            $('#InActiveUsers').html(InactiveUsers);
                            $('#TotalRequest').html(TotalRequest);
                            $('#TotalBooked').html(Bookingpercent);
                            $('#TotalCancel').html(Cancelpercent);
                            $('#TotalPending').html(Pendingpercent);
                            $('#TodayPending').html(TodayPending);
                            $('#TodayProcess').html(TodayProcess);
                            $('#TodayCancel').html(TodayCancel);
                            //$('#totalrunsheets').html(totalRunsheets);
                            $('#TodayRequest').html(TodayRequest);
                            $('#TodayPerformed').html(TodayPerformed);



                            //var options = {
                            //    //title: {
                            //    //    text: "Daily Stats"
                            //    //    // more attributes 
                            //    //},
                            //    theme: "light1",
                            //    animationEnabled: true,
                            //    axisY: [{
                            //      stepsize : 1
                            //        }],

                            //    data: [{
                            //        type: "bar",
                            //        height: "70",

                            //        exportEnabled: true,
                            //        innerRadius: "30%",
                            //        legendText: "{label}",
                            //        dataPoints: [
                            //            { label: "Request", y: TodayRequest, color: "#85C1E9" },
                            //            { label: "Process", y: TodayProcess, color: "#ffff00" },
                            //            { label: "Pending", y: TodayPending, color: "#ff8000" },
                            //            { label: "Booked", y: TodayProcess, color: "#43a047" },
                            //            { label: "Cancel", y: TodayCancel, color: "#ff0000" }
                            //        ]
                            //    }]

                            //};
                            //$("#chartContainer").CanvasJSChart(options);

                            var options = {
                                //title: {
                                //    text: "Daily Stats"
                                //    // more attributes 
                                //},
                                theme: "light1",
                                animationEnabled: true,
                                axisY: [{
                                    stepsize: 1
                                }],

                                data: [{
                                    type: "bar",
                                    height: "70",

                                    exportEnabled: true,
                                    innerRadius: "30%",
                                    legendText: "{label} - {y} ",
                                    dataPoints: [
                                        { label: "Request", y: MonthlyRequest, color: "#009688" },
                                        { label: "Process", y: MonthlyProcess, color: "#ffff00" },
                                        { label: "Pending", y: MonthlyPending, color: "#ff8000" },
                                        { label: "Booked", y: MonthlyPerformed, color: "#43a047" },
                                        { label: "Cancel", y: MonthlyCancel, color: "#b71c1c" }
                                    ]
                                }]

                            };
                            $("#chartContainer1").CanvasJSChart(options);
                            var options = {

                                theme: "light1",
                                animationEnabled: true,

                                data: [{
                                    type: "doughnut",
                                    startAngle: 60,
                                    height: "70",

                                    exportEnabled: true,
                                    innerRadius: "60%",
                                    legendText: "{label}",
                                    indexLabelFontSize: 12,
                                    indexLabel: "{label} - {y}",
                                    toolTipContent: "<b>{label}:</b> {y}",
                                    dataPoints: [
                                        { y: TotalBookings, label: "Booked", color: "#43a047" },
                                        { y: TotalCancelled, label: "Cancelled", color: "#b71c1c" },
                                        { y: TotalPending, label: "Pending", color: "#ff8000" },
                                    ]
                                }]

                            };
                            $("#chartContainer").CanvasJSChart(options);
                        }
                    }

                });





                //$.ajax({
                //    type: "POST",
                //    url: '../ConsigneeDetail/GetMonthlyStats',
                //    contentType: "application/json; charset=utf-8",
                //    dataType: "json",
                //    cache: false,
                //    success: function (rs) {

                //        if (rs.dataPoints.length > 0) {
                //            var dataPoints = [];
                //            var y = 0;
                //            var z = 0;
                //            var charts = [];
                //            for (x in rs.dataPoints) {
                //                dataPoints.push({ y: rs.dataPoints[x].Y, label: rs.dataPoints[x].X });

                //                y += rs.dataPoints[x].Y;
                //                z += rs.dataPoints[x].X;
                //            }

                //            var options = {
                //                //title: {
                //                //    text: "Monthly Stats"
                //                //    // more attributes 
                //                //},
                //                axisY: {
                //                    titleFontWeight: "bold",
                //                    title: "Delivered",
                //                    titleFontSize: 12,
                //                    includeZero: false,
                //                    titleFontColor: "black",
                //                    labelFontColor: "black",
                //                    autoWidth: true,
                //                    crosshair: {
                //                        enabled: true,
                //                        snapToDataPoint: true
                //                    }
                //                },
                //                axisX: {
                //                    interval: 5,
                //                    visibility: false,
                //                    type: 'date',
                //                    labelAutoFit: true,
                //                    labelWrap: true,
                //                    labelMaxWidth: 100,
                //                    labelAngle: 0,
                //                    labelFontSize: 12,
                //                    labelFontColor: "black",
                //                    title: "Date",
                //                    titleFontSize: 12,
                //                    titleFontColor: "black",
                //                    crosshair: {
                //                        enabled: true,
                //                        snapToDataPoint: true
                //                    }
                //                },
                //                checkVisibility: true,
                //                theme: "light1",
                //                animationEnabled: true,
                //                data: [
                //                    {
                //                        indexLabelFontWeight: "bold",
                //                        type: "line",
                //                        //  exportEnabled: true,
                //                        //  exportEnabled: true,
                //                        indexLabelFontSize: 10,
                //                        labelFontSize: 10,
                //                        indexLabel: "{y}",
                //                        legendText: "{X}",
                //                        toolTipContent: "{X}<b>{y}",
                //                        dataPoints: dataPoints

                //                    }]
                //            };


                //            $("#chartContainer1").CanvasJSChart(options);

                //            //var chart = new CanvasJS.Chart("chartContainer1", {
                //            //    animationEnabled: true,
                //            //    theme: "light2",
                //            //    exportEnabled: true,
                //            //    title: {
                //            //        text: "Monthly Stats",
                //            //        fontWeight: "bold",
                //            //         more attributes 
                //            //    },
                //            //    axisY: {
                //            //        titleFontWeight: "bold",
                //            //        title: "Delivered",
                //            //        titleFontSize: 12,
                //            //        includeZero: false,
                //            //        titleFontColor: "black",
                //            //        labelFontColor: "black",
                //            //        crosshair: {
                //            //            enabled: true,
                //            //            snapToDataPoint: true
                //            //        }
                //            //    },
                //            //    axisX: {
                //            //        type: 'date',
                //            //        labelAutoFit: true,
                //            //        labelWrap: true,
                //            //        labelMaxWidth: 100,
                //            //        labelAngle: 0,
                //            //        labelFontSize: 12,
                //            //        labelFontColor: "black",
                //            //        title: "Date",
                //            //        titleFontSize: 12,
                //            //        titleFontColor: "black",
                //            //        crosshair: {
                //            //            enabled: true,
                //            //            snapToDataPoint: true
                //            //        }
                //            //    },
                //            //    data: [
                //            //        {
                //            //            indexLabelFontWeight: "bold",
                //            //            type: "line",
                //            //            exportEnabled: true,
                //            //            indexLabelFontSize: 10,
                //            //            labelFontSize: 10,
                //            //            indexLabel: "{y}",
                //            //            legendText: "{z}",
                //            //            toolTipContent: "{X}<b>{y}",
                //            //            dataPoints: dataPoints

                //            //        }
                //            //    ]


                //            //});
                //            //var slideIndex = 1;
                //            //showSlides(slideIndex);
                //            //function showSlides(n) {
                //            //    var i;
                //            //    var slides = document.getElementsByClassName("mySlides");
                //            //    var dots = document.getElementsByClassName("dot");
                //            //    if (n > slides.length) { slideIndex = 1 }
                //            //    if (n < 1) { slideIndex = slides.length }
                //            //    for (i = 0; i < slides.length; i++) {
                //            //        slides[i].style.display = "none";
                //            //    }
                //            //    for (i = 0; i < dots.length; i++) {
                //            //        dots[i].className = dots[i].className.replace(" active", "");
                //            //    }
                //            //    slides[slideIndex - 1].style.display = "block";
                //            //    dots[slideIndex - 1].className += " active";
                //            //}
                //            //chart.render();
                //            //charts.push(chart);

                //        }
                //        else {
                //            $("#chartContainer").empty();
                //        }
                //    }
                //});

            }

        });

         //$("#btnticket").click(function () {
         //    debugger;

         //    //$("#ModalPopUpLocation").modal('show');

         //});

    </script>
</asp:Content>
