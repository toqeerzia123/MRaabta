<%@ Page Title="BTSDashoard" Language="C#" AutoEventWireup="true" CodeBehind="BTSDashoard.aspx.cs" Inherits="MRaabta.Files.BTSDashoard" MasterPageFile="~/BtsMasterPage2.master" %>

<%--  Layout = "~/Views/Shared/_Layout.cshtml";--%>


<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

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
            width: 47%;
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
    <%-- <script src="https://canvasjs.com/assets/script/jquery-1.11.1.min.js"></script>--%>
    <script type="text/javascript" src="https://canvasjs.com/assets/script/jquery.canvasjs.min.js"></script>
    <script type="text/javascript">

        var getRealData = () => {
            $.ajax({
                type: "POST",
                url: "../ConsigneeDetail/GetTodayConsignments",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (rs) {
                    //By Fahad Ali Start
                    var html = '';
                    for (let x of rs) {
                        html += `<tr>
                            <td class="text-center">${[1, 3].includes(x.StatusId) ? `<span style="height: 25px;width: 25px;background-color: green;border-radius: 50%;display: inline-block;"></span>` : `<span style="height: 25px;width: 25px;background-color: red;border-radius: 50%;display: inline-block;"></span>`}</td >
                            <td class="text-center">${x.ConsignmentNumber}</td>
                            <td class="text-center">${x.userName}</td>
                            <td class="text-center">${x.Time}</td>
                            </tr>`;
                    }
                    $("#test").html(html);
                    //By Fahad Ali End
                }
            });
        }

        function myFunction() {
            debugger;
            window.location = '<%= ResolveUrl("~/AllCNDebriefing") %>';
        }

        $(document).ready(function () {

            const as = document.querySelectorAll('#Menuid > li > a');

            for (let x of as) {
                x.onclick = () => false;
            }


            setInterval(function () {
                getRealData();
            }, 10000);//request every x seconds



            getRealData();

            window.onload = function () {
                $.ajax({
                    type: 'post',
                    url: '../ConsigneeDetail/GetPoints',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    cache: false,
                    //data: rs,
                    success: function (rs) {
                        if (rs[0].length == 0) {

                        }
                        else {
                            //debugger;
                            var ActiveRiders = rs[0].ActiveRiders;
                            var TotalRiders = rs[0].RidersCount;
                            var OfflineRiders = rs[0].OfflineRiders;
                            var RouteCount = rs[0].Touchpoints;
                            var CurrentUser = rs[0].CurrentUsers;
                            var TCNDownloaded = rs[0].TCNDownloaded
                            var delivered = rs[0].delivered;
                            var deliveredRts = rs[0].deliveredRts;
                            var undelivered = rs[0].undelivered;
                            var Performed = delivered + undelivered + deliveredRts;
                            var UnPerformed = TCNDownloaded - Performed;
                            var totalRunsheets = rs[0].TotalRunsheet;
                            var totaldwnRunsheets = rs[0].DownloadedRunsheet;
                            $('#totalriders').html(TotalRiders);
                            $('#activeriders').html(ActiveRiders);
                            $('#offlineriders').html(OfflineRiders);
                            $('#routecount').html(RouteCount);
                            $('#currentusers').html(CurrentUser);
                            $('#dwnConsignments').html(TCNDownloaded);
                            $('#dwnRunsheets').html(totaldwnRunsheets);
                            //$('#totalrunsheets').html(totalRunsheets);
                            $('#pending').html(UnPerformed);
                            $('#performed').html(Performed);

                            var options = {
                                //title: {
                                //    text: "Daily Stats"
                                //    // more attributes 
                                //},
                                theme: "light1",
                                animationEnabled: true,
                                data: [{
                                    type: "bar",
                                    height: "60",

                                    exportEnabled: true,
                                    innerRadius: "30%",
                                    legendText: "{label}",
                                    dataPoints: [
                                        { label: "Delivered", y: rs[0].delivered, color: "#388e3c" },
                                        { label: "DeliveredRts", y: rs[0].deliveredRts, color: "#ff5e00" },
                                        { label: "UnDelivered", y: rs[0].undelivered, color: "#b71c1c" },
                                        { label: "Unperformed", y: UnPerformed, color: "#ff7043" }
                                    ]
                                }]
                            };
                            $("#chartContainer").CanvasJSChart(options);
                        }
                    }

                });


                $.ajax({
                    type: "POST",
                    url: '../ConsigneeDetail/GetMonthlyStats',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    cache: false,
                    success: function (rs) {

                        if (rs.dataPoints.length > 0) {
                            var dataPoints = [];
                            var y = 0;
                            var z = 0;
                            var charts = [];
                            for (x in rs.dataPoints) {
                                dataPoints.push({ y: rs.dataPoints[x].Y, label: rs.dataPoints[x].X });
                                y += rs.dataPoints[x].Y;
                                z += rs.dataPoints[x].X;
                            }

                            var options = {
                                //title: {
                                //    text: "Monthly Stats"
                                //    // more attributes 
                                //},
                                axisY: {
                                    titleFontWeight: "bold",
                                    title: "Delivered",
                                    titleFontSize: 12,
                                    includeZero: false,
                                    titleFontColor: "black",
                                    labelFontColor: "black",
                                    autoWidth: true,
                                    crosshair: {
                                        enabled: true,
                                        snapToDataPoint: true
                                    }
                                },
                                axisX: {
                                    interval: 5,
                                    visibility: false,
                                    type: 'date',
                                    labelAutoFit: true,
                                    labelWrap: true,
                                    labelMaxWidth: 100,
                                    labelAngle: 0,
                                    labelFontSize: 12,
                                    labelFontColor: "black",
                                    title: "Date",
                                    titleFontSize: 12,
                                    titleFontColor: "black",
                                    crosshair: {
                                        enabled: true,
                                        snapToDataPoint: true
                                    }
                                },
                                checkVisibility: true,
                                theme: "light1",
                                animationEnabled: true,
                                data: [
                                    {
                                        indexLabelFontWeight: "bold",
                                        type: "line",
                                        //  exportEnabled: true,
                                        //  exportEnabled: true,
                                        indexLabelFontSize: 10,
                                        labelFontSize: 10,
                                        indexLabel: "{y}",
                                        legendText: "{X}",
                                        toolTipContent: "{X}<b>{y}",
                                        dataPoints: dataPoints

                                    }]
                            };


                            $("#chartContainer1").CanvasJSChart(options);

                            //var chart = new CanvasJS.Chart("chartContainer1", {
                            //    animationEnabled: true,
                            //    theme: "light2",
                            //    exportEnabled: true,
                            //    title: {
                            //        text: "Monthly Stats",
                            //        fontWeight: "bold",
                            //         more attributes 
                            //    },
                            //    axisY: {
                            //        titleFontWeight: "bold",
                            //        title: "Delivered",
                            //        titleFontSize: 12,
                            //        includeZero: false,
                            //        titleFontColor: "black",
                            //        labelFontColor: "black",
                            //        crosshair: {
                            //            enabled: true,
                            //            snapToDataPoint: true
                            //        }
                            //    },
                            //    axisX: {
                            //        type: 'date',
                            //        labelAutoFit: true,
                            //        labelWrap: true,
                            //        labelMaxWidth: 100,
                            //        labelAngle: 0,
                            //        labelFontSize: 12,
                            //        labelFontColor: "black",
                            //        title: "Date",
                            //        titleFontSize: 12,
                            //        titleFontColor: "black",
                            //        crosshair: {
                            //            enabled: true,
                            //            snapToDataPoint: true
                            //        }
                            //    },
                            //    data: [
                            //        {
                            //            indexLabelFontWeight: "bold",
                            //            type: "line",
                            //            exportEnabled: true,
                            //            indexLabelFontSize: 10,
                            //            labelFontSize: 10,
                            //            indexLabel: "{y}",
                            //            legendText: "{z}",
                            //            toolTipContent: "{X}<b>{y}",
                            //            dataPoints: dataPoints

                            //        }
                            //    ]


                            //});
                            //var slideIndex = 1;
                            //showSlides(slideIndex);
                            //function showSlides(n) {
                            //    var i;
                            //    var slides = document.getElementsByClassName("mySlides");
                            //    var dots = document.getElementsByClassName("dot");
                            //    if (n > slides.length) { slideIndex = 1 }
                            //    if (n < 1) { slideIndex = slides.length }
                            //    for (i = 0; i < slides.length; i++) {
                            //        slides[i].style.display = "none";
                            //    }
                            //    for (i = 0; i < dots.length; i++) {
                            //        dots[i].className = dots[i].className.replace(" active", ");
                            //    }
                            //    slides[slideIndex - 1].style.display = "block";
                            //    dots[slideIndex - 1].className += " active";
                            //}
                            //chart.render();
                            //charts.push(chart);

                        }
                        else {
                            $("#chartContainer").empty();
                        }
                    }
                });

            }
        });


    </script>
    <div class="row">
        <%-- Flip Cards --%>
        <div class="col-md-6">
            <div class="row">
                <div class="flip-card mt-2" style="margin-left: 20px;">
                    <div class="flip-card-inner">
                        <div class="flip-card-front" style="background: linear-gradient(to bottom, #43cea2 0%, #185a9d 100%)">
                            <h2 style="margin-top: 1em; text-align: center; font-size: 2vw; font-family: 'Trebuchet MS', 'Lucida Sans Unicode', 'Lucida Grande', 'Lucida Sans', Arial, sans-serif"><%--<img src="../images/delivery-boy.png" style="width: 2em; height: 2em;"/>--%>Total Riders</h2>
                        </div>
                        <div class="flip-card-back">
                            <h2 style="margin-top: 0.5em; text-align: center; font-size: 3vw; font-family: 'Trebuchet MS', 'Lucida Sans Unicode', 'Lucida Grande', 'Lucida Sans', Arial, sans-serif" id="totalriders"></h2>
                        </div>
                    </div>
                </div>
                <div class="flip-card mt-2" style="margin-left: 10px;">
                    <div class="flip-card-inner">
                        <div class="flip-card-front" style="background: linear-gradient(to bottom, #FFB75E 0%, #ED8F03 100%)">
                            <p style="margin-top: 1em; text-align: center; font-size: 2vw; font-family: 'Trebuchet MS', 'Lucida Sans Unicode', 'Lucida Grande', 'Lucida Sans', Arial, sans-serif"><%--<img src="../images/motorbike.png" style="width: 2em; height: 2em;"/>--%>Active Riders</p>
                        </div>
                        <div class="flip-card-back">
                            <p style="margin-top: 0.5em; text-align: center; font-size: 3vw; font-family: 'Trebuchet MS', 'Lucida Sans Unicode', 'Lucida Grande', 'Lucida Sans', Arial, sans-serif" id="activeriders">50</p>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="flip-card mt-3" style="margin-left: 20px;">
                    <div class="flip-card-inner">
                        <div class="flip-card-front" style="background: linear-gradient(to bottom, #ff9966 0%, #ff5e62 100%)">
                            <p style="margin-top: 1em; text-align: center; font-size: 2vw; font-family: 'Trebuchet MS', 'Lucida Sans Unicode', 'Lucida Grande', 'Lucida Sans', Arial, sans-serif"><%--<img src="../images/scooter.png" style="width: 2em; height: 2em;"/>--%>Offline Riders</p>
                        </div>
                        <div class="flip-card-back">
                            <p style="margin-top: 0.5em; text-align: center; font-size: 3vw; font-family: 'Trebuchet MS', 'Lucida Sans Unicode', 'Lucida Grande', 'Lucida Sans', Arial, sans-serif" id="offlineriders"></p>
                        </div>
                    </div>
                </div>
                <div class="flip-card mt-3" style="margin-left: 10px;">
                    <div class="flip-card-inner">
                        <div class="flip-card-front" style="background: linear-gradient(to bottom, #B24592 0%, #F15F79 100%)">
                            <p style="margin-top: 1em; text-align: center; font-size: 2vw; font-family: 'Trebuchet MS', 'Lucida Sans Unicode', 'Lucida Grande', 'Lucida Sans', Arial, sans-serif"><%--<img src="../images/pin.png" style="width: 2em; height: 2em;"/>--%>Total Touchpoints</p>
                        </div>
                        <div class="flip-card-back">
                            <p style="margin-top: 0.5em; text-align: center; font-size: 3vw; font-family: 'Trebuchet MS', 'Lucida Sans Unicode', 'Lucida Grande', 'Lucida Sans', Arial, sans-serif" id="routecount"></p>
                        </div>
                    </div>
                </div>

            </div>
            <div class="row">
                <div class="card mt-2" style="margin-left: 20px;">
                    <div class="card-header" style="font-size: 20px; font-weight: bolder; font-family: 'Cambria Math'; background-color: #424242;">
                        <span class="ml-2" style="color: white;">Today's Report</span>
                    </div>
                    <div class="m-1 ml-1">
                        <div class="row" style="margin-left: 1px;">
                            <div class="col-md-3">
                                <span style="font-size: 1vw; font-weight: bolder; font-family: Lato;">Downloaded Runsheets</span><br />
                                <div class="row">
                                    <img src="../images/RunsheetRep.png" style="height: auto; width: 25%; margin-top: 4px;" /><span id="dwnRunsheets" style="font-size: 2vw; text-align: center; font-family: Lato;"></span>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <span style="font-size: 1vw; font-weight: bolder; font-family: Lato;">Downloaded Consignments </span>
                                <br />
                                <div class="row">
                                    <img src="../images/Box.png" style="height: auto; width: 25%; margin-top: 4px;" /><span id="dwnConsignments" style="font-size: 2vw; text-align: center; font-family: Lato;"></span>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <span style="font-size: 1vw; font-weight: bolder; font-family: Lato;">Performed Consignments</span><br />
                                <div class="row">
                                    <img src="../images/list.png" style="height: auto; width: 25%; margin-top: 4px;" /><span id="performed" style="font-size: 2vw; text-align: center; font-family: Lato;"></span>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <span style="font-size: 1vw; font-weight: bolder; font-family: Lato;">Pending Consignments</span><br />
                                <div class="row">
                                    <img src="../images/pending.png" style="height: auto; width: 25%; margin-top: 4px;" /><span id="pending" style="font-size: 2vw; text-align: center; font-family: Lato;"></span>
                                </div>
                            </div>


                        </div>
                    </div>

                </div>
            </div>


            <%-- Slider --%>

            <div class="card ml-1" style="margin-left: 0.5em; margin-top: 0.9em; width: 102%;">
                <div class="card-header" style="width: 100%; font-size: 20px; font-weight: bolder; font-family: 'Cambria Math'; background-color: #424242;">
                    <span class="ml-2" style="color: white;">Daily Stats</span>
                </div>
                <div class="card-body">
                    <div id="chartContainer" style="position: relative; height: 120px; width: 100%;"></div>
                </div>
            </div>


        </div>

        <div class="col-md-6">
            <div style="width: 95%; height: 16em;" class="mt-2">
                <div class="card" id="drwconsignments" style="width: 100%;">
                    <div class="card-header" style="width: 100%; font-size: 20px; font-weight: bolder; font-family: 'Cambria Math'; background-color: #424242;">
                        <div class="row">
                            <div class="col-md-9">
                                <span class="ml-1" style="color: white;">Latest Consignments Performed</span>
                            </div>
                            <div class="col-md-3">
                                <button class="btn btn-success" style="font-size: 12px;" onclick="myFunction();">View More</button>
                            </div>
                        </div>

                    </div>
                    <table id="table1" style="width: 100%" class="table-bordered">
                        <thead class="thead-dark" style="background-color: #eeeeee">
                            <tr>
                                <th class="ml-2" style="font-size: 14px; margin: 0px; text-align: center;">Status</th>
                                <th class="ml-2" style="font-size: 14px; margin: 0px; text-align: center;">Consignment No.</th>
                                <th class="ml-2" style="font-size: 14px; margin: 0px; text-align: center;">Rider Name</th>
                                <th class="ml-2" style="font-size: 14px; margin: 0px; text-align: center;">Time</th>
                            </tr>
                        </thead>
                        <tbody id="test">
                        </tbody>
                    </table>
                </div>
            </div>
            <div class="card" style="width: 95%; margin-top: 7.9em;">
                <div class="card-header" style="width: 100%; font-size: 20px; font-weight: bolder; font-family: 'Cambria Math'; background-color: #424242;">
                    <span class="ml-1" style="color: white;">Monthly Stats</span>
                </div>
                <div class="card-body">
                    <div id="chartContainer1" style="position: relative; height: 120px; width: 100%;"></div>
                </div>
            </div>
        </div>
    </div>


</asp:Content>